using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.Invasions
{
    public static partial class InvasionService
    {
        private static DateTime m_LastMaintenanceUtc;

        private static void Tick()
        {
            InvasionWorldState worldState = InvasionWorldState.Instance;

            if (worldState == null || m_DuplicateState)
                return;

            worldState.LastEngineTickUtc = DateTime.UtcNow;

            List<NetState> states = new List<NetState>(NetState.Instances);

            for (int i = 0; i < states.Count; ++i)
            {
                PlayerMobile player = states[i].Mobile as PlayerMobile;

                if (player != null && player.NetState != null)
                    UpdatePlayerConflictState(player);
            }

            ProcessChannels();

            if (DateTime.UtcNow - m_LastMaintenanceUtc >= TimeSpan.FromMinutes(1.0))
            {
                PruneLedgers();
                m_LastMaintenanceUtc = DateTime.UtcNow;
            }

            InvasionCityState[] cities = worldState.GetCities();

            for (int i = 0; i < cities.Length; ++i)
            {
                InvasionCityState city = cities[i];
                ApplyCorruptionDecay(city);

                if (city.StaffPaused)
                {
                    ExtendPhaseDeadline(city);
                    continue;
                }

                if (!worldState.EngineEnabled || m_DuplicateState || !city.Enabled)
                    continue;

                bool playersPresent;
                int activePlayers = CountActivePlayers(city.City, out playersPresent);

                if (city.IsConflictActive && city.State != InvasionState.Occupied)
                {
                    if (playersPresent)
                    {
                        city.LastPlayerSeenUtc = DateTime.UtcNow;

                        if (city.IdlePaused)
                        {
                            city.IdlePaused = false;
                            Announce(InvasionCities.Get(city.City).Name + "'s invasion has resumed.");
                        }
                    }
                    else if (DateTime.UtcNow - city.LastPlayerSeenUtc >= TimeSpan.FromMinutes(15.0))
                    {
                        if (!city.IdlePaused)
                        {
                            city.IdlePaused = true;
                            DeleteTransientForces(city);
                            Announce(InvasionCities.Get(city.City).Name + "'s invasion has gone quiet and will resume when challenged.");
                        }
                    }
                }

                if (city.IdlePaused)
                {
                    ExtendPhaseDeadline(city);
                    continue;
                }

                TickCity(city, activePlayers);
            }
        }

        private static void TickCity(InvasionCityState city, int activePlayers)
        {
            if (activePlayers > city.HighestParticipants)
                city.HighestParticipants = activePlayers;

            switch (city.State)
            {
                case InvasionState.Ritual:
                    TickRitual(city);
                    break;
                case InvasionState.Encirclement:
                    TickEncirclement(city);
                    MaintainSiegeForces(city, activePlayers);
                    break;
                case InvasionState.Breach:
                    TickBreach(city);
                    MaintainSiegeForces(city, activePlayers);
                    break;
                case InvasionState.FinalBattle:
                    MaintainSiegeForces(city, activePlayers);
                    EnsureFinalBattle(city);
                    break;
                case InvasionState.Occupied:
                    ReconcileOccupiedSpawners(city);
                    EnsureOccupation(city);
                    break;
            }
        }

        private static void TickRitual(InvasionCityState city)
        {
            int invaders;
            int defenders;
            CountPresence(city.City, InvasionCities.Get(city.City).Ritual, 24, out invaders, out defenders);

            if (invaders > 0)
                city.CampHoldSeconds++;

            if (city.CampHoldSeconds >= 600)
                EnterEncirclement(city);
        }

        private static void TickEncirclement(InvasionCityState city)
        {
            InvasionCityDefinition definition = InvasionCities.Get(city.City);
            int controlled = 0;

            for (int i = 0; i < definition.Camps.Length; ++i)
            {
                if (city.CampsCleansed[i])
                    continue;

                int invaders;
                int defenders;
                CountPresence(city.City, definition.Camps[i], 16, out invaders, out defenders);

                if (invaders > 0 && defenders == 0)
                    controlled++;
            }

            if (controlled >= 2)
                city.CampHoldSeconds++;

            if (city.CampHoldSeconds >= 300)
                EnterBreach(city);
        }

        private static void TickBreach(InvasionCityState city)
        {
            InvasionCityDefinition definition = InvasionCities.Get(city.City);

            for (int i = 0; i < definition.Wards.Length; ++i)
            {
                int invaders;
                int defenders;
                CountPresence(city.City, definition.Wards[i], 12, out invaders, out defenders);

                if (invaders > 0 && defenders == 0)
                    city.WardProgressSeconds[i] = Math.Min(300, city.WardProgressSeconds[i] + 1);
                else if (defenders > 0 && invaders == 0)
                    city.WardProgressSeconds[i] = Math.Max(0, city.WardProgressSeconds[i] - 1);
            }

            if (DateTime.UtcNow >= city.PhaseEndsUtc)
            {
                int corrupted = 0;

                for (int i = 0; i < city.WardProgressSeconds.Length; ++i)
                {
                    if (city.WardProgressSeconds[i] >= 300)
                        corrupted++;
                }

                if (corrupted >= 2)
                    EnterFinalBattle(city);
                else
                    DefenderVictory(city, "The civic wards hold and the invasion is repelled.");
            }
        }

        private static void ExtendPhaseDeadline(InvasionCityState city)
        {
            if (city.PhaseEndsUtc > DateTime.UtcNow && city.PhaseEndsUtc < DateTime.MaxValue)
                city.PhaseEndsUtc = city.PhaseEndsUtc + TimeSpan.FromSeconds(1.0);
        }

        private static void ApplyCorruptionDecay(InvasionCityState city)
        {
            if (city.State != InvasionState.Dormant)
                return;

            if (city.Corruption <= 0)
            {
                city.LastDecayUtc = DateTime.UtcNow;
                return;
            }

            int days = (int)(DateTime.UtcNow - city.LastDecayUtc).TotalDays;

            if (days <= 0)
                return;

            city.Corruption = Math.Max(0, city.Corruption - (days * 10));
            city.LastDecayUtc = city.LastDecayUtc.AddDays(days);
        }

        private static void PruneLedgers()
        {
            InvasionWorldState state = InvasionWorldState.Instance;
            DateTime contributionCutoff = DateTime.UtcNow - ContributionWindow;

            for (int i = state.Contributions.Count - 1; i >= 0; --i)
            {
                if (state.Contributions[i].CreatedUtc < contributionCutoff)
                    state.Contributions.RemoveAt(i);
            }

            for (int i = state.Cooldowns.Count - 1; i >= 0; --i)
            {
                if (state.Cooldowns[i].ExpiresUtc <= DateTime.UtcNow)
                    state.Cooldowns.RemoveAt(i);
            }

            foreach (KeyValuePair<InvasionCityId, Dictionary<string, DateTime>> pair in m_RecentParticipants)
            {
                List<string> expired = new List<string>();

                foreach (KeyValuePair<string, DateTime> participant in pair.Value)
                {
                    if (participant.Value < DateTime.UtcNow - TimeSpan.FromMinutes(5.0))
                        expired.Add(participant.Key);
                }

                for (int i = 0; i < expired.Count; ++i)
                    pair.Value.Remove(expired[i]);
            }
        }

        private static int CountActivePlayers(InvasionCityId city, out bool playersPresent)
        {
            int count = 0;

            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                PlayerMobile player = states[i].Mobile as PlayerMobile;

                if (player == null || player.AccessLevel > AccessLevel.Player || player.NetState == null || player.Map != Map.Sosaria)
                    continue;

                if (InvasionCities.Get(city).ContainsConflict(player.Location))
                {
                    NoteParticipant(player, city);
                    count++;
                }
            }

            Dictionary<string, DateTime> participants = m_RecentParticipants[city];
            int established = 0;

            foreach (DateTime seen in participants.Values)
            {
                if (seen >= DateTime.UtcNow - TimeSpan.FromMinutes(5.0))
                    established++;
            }

            playersPresent = count > 0;
            return Math.Max(playersPresent ? 1 : 0, Math.Min(8, established));
        }

        private static void CountPresence(
            InvasionCityId city,
            Point3D center,
            int range,
            out int invaders,
            out int defenders
        )
        {
            invaders = 0;
            defenders = 0;
            int rangeSquared = range * range;

            List<NetState> states = NetState.Instances;

            for (int i = 0; i < states.Count; ++i)
            {
                PlayerMobile player = states[i].Mobile as PlayerMobile;

                if (player == null || player.AccessLevel > AccessLevel.Player || player.NetState == null || !player.Alive || player.Map != Map.Sosaria)
                    continue;

                if (DistanceSquared(player.Location, center) > rangeSquared || HasGrace(player))
                    continue;

                if (GetSide(player, city) == InvasionSide.Invader)
                    invaders++;
                else
                    defenders++;
            }
        }

        private static void EnterRitualReady(InvasionCityState city)
        {
            if (city.State == InvasionState.RitualReady)
                return;

            city.State = InvasionState.RitualReady;
            city.StateStartedUtc = DateTime.UtcNow;
            city.Faction = InvasionFaction.None;
            city.Session = InvasionWorldState.Instance.AcquireSession();
            city.CampHoldSeconds = 0;
            DeleteOwnedEntities(city);
            SpawnObjective(city, InvasionRole.RitualFocus, -1, InvasionSide.Invader, InvasionCities.Get(city.City).Ritual);
            Announce(InvasionCities.Get(city.City).Name + " is vulnerable. An eligible villain may now choose an invading faction at the ritual focus.");
        }

        public static bool TryStartRitual(
            Mobile leaderMobile,
            InvasionCityId cityId,
            InvasionFaction faction,
            int expectedSession,
            out string reason
        )
        {
            reason = null;
            PlayerMobile leader = ResolvePlayer(leaderMobile);
            InvasionCityState city = GetCityState(cityId);

            if (leader == null
                || leader.Deleted
                || !leader.Alive
                || leader.AccessLevel > AccessLevel.Player
                || city == null
                || city.State != InvasionState.RitualReady
                || city.Session != expectedSession)
            {
                reason = "That ritual opportunity is no longer current.";
                return false;
            }

            if (InvasionWorldState.Instance == null
                || !InvasionWorldState.Instance.EngineEnabled
                || m_DuplicateState
                || !city.Enabled
                || faction < InvasionFaction.ClockworkDominion
                || faction > InvasionFaction.AbyssalLegion)
            {
                reason = "The invasion engine cannot accept that ritual choice.";
                return false;
            }

            InvasionObjectiveItem focus = FindOwnedObjective(city, InvasionRole.RitualFocus, -1);

            if (focus == null || leader.Map != focus.Map || !leader.InRange(focus.GetWorldLocation(), 3))
            {
                reason = "You must remain beside the current ritual focus.";
                return false;
            }

            if (!IsEligibleEvil(leader) || !IsEstablished(leader) || GetSide(leader, cityId) != InvasionSide.Invader)
            {
                reason = "You are not eligible to lead this corruption ritual.";
                return false;
            }

            InvasionCycleCredit credit = FindCycleCredit(GetAccountName(leader), cityId);

            if (credit == null || credit.Points < 10)
            {
                reason = "You must personally contribute at least ten corruption during this cycle.";
                return false;
            }

            if (city.RitualLockUntilUtc > DateTime.UtcNow)
            {
                reason = "The ritual focus is still locked by its last disruption.";
                return false;
            }

            StartRitualInternal(city, faction, leader.Serial.Value);
            return true;
        }

        private static void StartRitualInternal(InvasionCityState city, InvasionFaction faction, int leaderSerial)
        {
            city.State = InvasionState.Ritual;
            city.StateStartedUtc = DateTime.UtcNow;
            city.Faction = faction;
            city.RitualLeaderSerial = leaderSerial;
            city.CampHoldSeconds = 0;
            city.LastPlayerSeenUtc = DateTime.UtcNow;
            city.HighestParticipants = 1;
            DeleteOwnedEntities(city);
            SpawnObjective(city, InvasionRole.RitualFocus, -1, InvasionSide.Invader, InvasionCities.Get(city.City).Ritual);
            Announce(GetFactionName(faction) + " has begun a corruption ritual against " + InvasionCities.Get(city.City).Name + ".");
        }

        private static void EnterEncirclement(InvasionCityState city)
        {
            city.State = InvasionState.Encirclement;
            city.StateStartedUtc = DateTime.UtcNow;
            city.CampHoldSeconds = 0;
            city.CampsCleansed = new bool[3];
            city.HighestParticipants = Math.Max(1, city.HighestParticipants);
            DeleteOwnedEntities(city);

            InvasionCityDefinition definition = InvasionCities.Get(city.City);

            for (int i = 0; i < definition.Camps.Length; ++i)
            {
                SpawnObjective(city, InvasionRole.CampStandard, i, InvasionSide.Invader, definition.Camps[i]);
                SpawnCreature(city, InvasionRole.Lieutenant, i, InvasionSide.Invader, definition.Camps[i], city.HighestParticipants);
            }

            Announce(GetFactionName(city.Faction) + " has encircled " + definition.Name + ". Defenders must break at least two camps.");
        }

        private static void EnterBreach(InvasionCityState city)
        {
            city.State = InvasionState.Breach;
            city.StateStartedUtc = DateTime.UtcNow;
            city.PhaseEndsUtc = DateTime.UtcNow + TimeSpan.FromMinutes(15.0);
            city.WardProgressSeconds = new int[3];
            city.HighestParticipants = Math.Max(1, city.HighestParticipants);
            DeleteOwnedEntities(city);

            InvasionCityDefinition definition = InvasionCities.Get(city.City);

            for (int i = 0; i < definition.Wards.Length; ++i)
                SpawnObjective(city, InvasionRole.CivicWard, i, InvasionSide.Defender, definition.Wards[i]);

            Announce(GetFactionName(city.Faction) + " has reached the civic wards of " + definition.Name + ".");
        }

        private static void EnterFinalBattle(InvasionCityState city)
        {
            city.State = InvasionState.FinalBattle;
            city.StateStartedUtc = DateTime.UtcNow;
            city.PhaseEndsUtc = DateTime.MinValue;
            city.CommanderDead = false;
            city.WardenDead = false;
            city.HighestParticipants = Math.Max(1, city.HighestParticipants);
            DeleteOwnedEntities(city);
            EnsureFinalBattle(city);
            Announce("The fate of " + InvasionCities.Get(city.City).Name + " now rests on its civic warden and the invading commander.");
        }

        private static void EnsureFinalBattle(InvasionCityState city)
        {
            Point3D location = InvasionCities.Get(city.City).FinalBattle;
            InvasionCreature commander = FindOwnedCreature(city, InvasionRole.Commander, -1);
            InvasionCreature warden = FindOwnedCreature(city, InvasionRole.CivicWarden, -1);

            if (!city.CommanderDead && commander == null)
                commander = SpawnCreature(city, InvasionRole.Commander, -1, InvasionSide.Invader, FindSpawnPoint(location, 6), city.HighestParticipants);

            if (!city.WardenDead && warden == null)
                warden = SpawnCreature(city, InvasionRole.CivicWarden, -1, InvasionSide.Defender, FindSpawnPoint(new Point3D(location.X + 4, location.Y, location.Z), 6), city.HighestParticipants);

            if (commander != null)
                commander.RaiseBossScale(city.HighestParticipants);

            if (warden != null)
                warden.RaiseBossScale(city.HighestParticipants);

            if (commander != null && warden != null)
            {
                commander.Combatant = warden;
                warden.Combatant = commander;
            }
        }

        private static void EnterOccupation(InvasionCityState city)
        {
            city.State = InvasionState.Occupied;
            city.StateStartedUtc = DateTime.UtcNow;
            city.PhaseEndsUtc = DateTime.MinValue;
            city.CampsCleansed = new bool[3];
            city.AnchorIntegrity = new int[] { 100, 100, 100 };
            city.AnchorDailyRepair = new int[3];
            city.AnchorRepairDayUtc = new DateTime[] { DateTime.UtcNow.Date, DateTime.UtcNow.Date, DateTime.UtcNow.Date };
            city.CommanderDead = false;
            city.WardenDead = true;
            DeleteOwnedEntities(city);
            SuppressCivilizedSpawners(city);
            EnsureOccupation(city);
            Announce(GetFactionName(city.Faction) + " has occupied " + InvasionCities.Get(city.City).Name + ". Its supply anchors must be destroyed before liberation is possible.");
        }

        private static void EnsureOccupation(InvasionCityState city)
        {
            InvasionCityDefinition definition = InvasionCities.Get(city.City);

            for (int i = 0; i < definition.Camps.Length; ++i)
            {
                if (city.AnchorIntegrity[i] > 0 && FindOwnedObjective(city, InvasionRole.SupplyAnchor, i) == null)
                    SpawnObjective(city, InvasionRole.SupplyAnchor, i, InvasionSide.Invader, definition.Camps[i]);

                if (city.AnchorIntegrity[i] > 0 && !city.CampsCleansed[i] && FindOwnedCreature(city, InvasionRole.AnchorCaptain, i) == null)
                    SpawnCreature(city, InvasionRole.AnchorCaptain, i, InvasionSide.Invader, FindSpawnPoint(definition.Camps[i], 5), Math.Max(1, city.HighestParticipants));
            }

            EnsureOccupationVendors(city);

            InvasionCreature commander = FindOwnedCreature(city, InvasionRole.Commander, -1);

            if (!city.CommanderDead && commander == null)
                commander = SpawnCreature(city, InvasionRole.Commander, -1, InvasionSide.Invader, definition.FinalBattle, Math.Max(1, city.HighestParticipants));

            if (commander != null)
            {
                commander.RaiseBossScale(city.HighestParticipants);
                commander.Blessed = !AllAnchorsDestroyed(city);
            }

            int destroyed = CountDestroyedAnchors(city);
            int patrolTarget = (int)Math.Ceiling(6.0 * (1.0 - (destroyed * 0.25)));
            patrolTarget = Math.Max(0, patrolTarget);
            int patrolCount = CountOwnedCreatures(city, InvasionRole.OccupationPatrol);

            while (patrolCount < patrolTarget)
            {
                Point3D anchor = definition.Camps[Utility.Random(definition.Camps.Length)];
                SpawnCreature(city, InvasionRole.OccupationPatrol, -1, InvasionSide.Invader, FindSpawnPoint(anchor, 10), Math.Max(1, city.HighestParticipants));
                patrolCount++;
            }

            TrimOwnedCreatures(city, InvasionRole.OccupationPatrol, patrolTarget);
        }

        private static void EnsureOccupationVendors(InvasionCityState city)
        {
            Point3D center = InvasionCities.Get(city.City).FinalBattle;

            if (FindOwnedMobile(city, InvasionRole.Banker) == null)
                RegisterAndMove(city, new InvasionBanker(city.Session, city.City, city.Faction), FindSpawnPoint(new Point3D(center.X - 4, center.Y - 2, center.Z), 5));

            if (FindOwnedMobile(city, InvasionRole.Healer) == null)
                RegisterAndMove(city, new InvasionHealer(city.Session, city.City, city.Faction), FindSpawnPoint(new Point3D(center.X - 2, center.Y + 2, center.Z), 5));

            if (FindOwnedMobile(city, InvasionRole.Provisioner) == null)
                RegisterAndMove(city, new InvasionOccupationVendor(city.Session, city.City, city.Faction, InvasionRole.Provisioner), FindSpawnPoint(new Point3D(center.X + 2, center.Y - 2, center.Z), 5));

            if (FindOwnedMobile(city, InvasionRole.Fence) == null)
                RegisterAndMove(city, new InvasionOccupationVendor(city.Session, city.City, city.Faction, InvasionRole.Fence), FindSpawnPoint(new Point3D(center.X + 4, center.Y + 2, center.Z), 5));

            if (FindOwnedMobile(city, InvasionRole.Quartermaster) == null)
                RegisterAndMove(city, new InvasionOccupationVendor(city.Session, city.City, city.Faction, InvasionRole.Quartermaster), FindSpawnPoint(new Point3D(center.X, center.Y + 4, center.Z), 5));
        }

        private static void MaintainSiegeForces(InvasionCityState city, int activePlayers)
        {
            int participantScale = Math.Max(1, Math.Min(8, activePlayers));
            int target = Math.Min(24, 8 + (2 * participantScale));
            int current = CountTransientForces(city);

            if (current > target)
            {
                TrimTransientForces(city, target);
                current = target;
            }

            if (current >= target)
                return;

            int roll = Utility.Random(4);
            InvasionRole role = roll < 2 ? InvasionRole.RankMelee : (roll == 2 ? InvasionRole.RankRanged : InvasionRole.RankCaster);
            Point3D anchor;

            if (city.State == InvasionState.Breach)
                anchor = InvasionCities.Get(city.City).Wards[Utility.Random(3)];
            else if (city.State == InvasionState.FinalBattle)
                anchor = InvasionCities.Get(city.City).FinalBattle;
            else
                anchor = InvasionCities.Get(city.City).Camps[Utility.Random(3)];

            SpawnCreature(city, role, -1, InvasionSide.Invader, FindSpawnPoint(anchor, 12), participantScale);
        }

        private static void TrimTransientForces(InvasionCityState city, int target)
        {
            int count = CountTransientForces(city);

            for (int i = city.OwnedSerials.Count - 1; i >= 0 && count > target; --i)
            {
                InvasionCreature creature = World.FindMobile((Serial)city.OwnedSerials[i]) as InvasionCreature;

                if (creature == null || creature.InvasionSession != city.Session)
                    continue;

                if (creature.InvasionRole != InvasionRole.RankMelee
                    && creature.InvasionRole != InvasionRole.RankRanged
                    && creature.InvasionRole != InvasionRole.RankCaster)
                {
                    continue;
                }

                creature.Delete();
                city.OwnedSerials.RemoveAt(i);
                count--;
            }
        }

        private static int CountTransientForces(InvasionCityState city)
        {
            return CountOwnedCreatures(city, InvasionRole.RankMelee)
                + CountOwnedCreatures(city, InvasionRole.RankRanged)
                + CountOwnedCreatures(city, InvasionRole.RankCaster);
        }

        private static void DeleteTransientForces(InvasionCityState city)
        {
            DeleteOwnedRole(city, InvasionRole.RankMelee);
            DeleteOwnedRole(city, InvasionRole.RankRanged);
            DeleteOwnedRole(city, InvasionRole.RankCaster);
        }

        public static void NotifyCreatureDeath(InvasionCreature creature)
        {
            if (creature == null)
                return;

            InvasionCityState city = GetCityState(creature.InvasionCity);

            if (city == null || city.Session != creature.InvasionSession)
                return;

            if (creature.InvasionRole == InvasionRole.AnchorCaptain && city.State == InvasionState.Occupied)
            {
                int index = creature.InvasionObjectiveIndex;

                if (index >= 0 && index < city.CampsCleansed.Length)
                    city.CampsCleansed[index] = true;

                return;
            }

            if (creature.InvasionRole == InvasionRole.Commander)
                city.CommanderDead = true;
            else if (creature.InvasionRole == InvasionRole.CivicWarden)
                city.WardenDead = true;
            else
                return;

            Timer.DelayCall(TimeSpan.FromMilliseconds(250.0), new TimerStateCallback(ResolveBossDeaths), city.City);
        }

        private static void ResolveBossDeaths(object state)
        {
            InvasionCityId cityId = (InvasionCityId)state;
            InvasionCityState city = GetCityState(cityId);

            if (city == null)
                return;

            if (city.State == InvasionState.FinalBattle)
            {
                if (city.CommanderDead)
                    DefenderVictory(city, "The invading commander has fallen. " + InvasionCities.Get(city.City).Name + " is saved.");
                else if (city.WardenDead)
                    EnterOccupation(city);
            }
            else if (city.State == InvasionState.Occupied && city.CommanderDead && AllAnchorsDestroyed(city))
            {
                Liberate(city, "The occupation commander has fallen and " + InvasionCities.Get(city.City).Name + " is liberated.");
            }
        }

        public static void InteractWithObjective(Mobile from, InvasionObjectiveItem objective)
        {
            PlayerMobile player = ResolvePlayer(from);

            if (player == null || player.AccessLevel > AccessLevel.Player || objective == null || objective.Deleted)
                return;

            InvasionCityState city = GetCityState(objective.InvasionCity);

            if (city == null || city.Session != objective.InvasionSession || city.StaffPaused || city.IdlePaused)
            {
                player.SendMessage("That invasion objective is not currently active.");
                return;
            }

            EndGrace(player);
            InvasionSide side = GetSide(player, city.City);

            switch (objective.InvasionRole)
            {
                case InvasionRole.RitualFocus:
                    if (city.State == InvasionState.RitualReady)
                    {
                        if (side == InvasionSide.Invader)
                            player.SendGump(new Server.Custom.Confictura.Gumps.Invasions.InvasionFactionGump(player, city.City, city.Session));
                        else
                            BeginChannel(player, objective, "cleanse-ready", TimeSpan.FromSeconds(60.0), "You begin cleansing corruption from the ritual focus.");
                    }
                    else if (city.State == InvasionState.Ritual && side == InvasionSide.Defender)
                    {
                        BeginChannel(player, objective, "destroy-ritual", TimeSpan.FromSeconds(60.0), "You begin disrupting the active ritual.");
                    }
                    break;
                case InvasionRole.CampStandard:
                    if (city.State != InvasionState.Encirclement || side != InvasionSide.Defender)
                        break;

                    if (FindOwnedCreature(city, InvasionRole.Lieutenant, objective.InvasionObjectiveIndex) != null)
                    {
                        player.SendMessage("The camp lieutenant must be defeated before this standard can be cleansed.");
                        break;
                    }

                    BeginChannel(player, objective, "cleanse-camp", TimeSpan.FromSeconds(30.0), "You begin tearing down the invasion standard.");
                    break;
                case InvasionRole.SupplyAnchor:
                    if (city.State != InvasionState.Occupied)
                        break;

                    int index = objective.InvasionObjectiveIndex;

                    if (index < 0 || index >= city.AnchorIntegrity.Length || city.AnchorIntegrity[index] <= 0)
                        break;

                    if (side == InvasionSide.Defender)
                    {
                        if (!city.CampsCleansed[index])
                        {
                            player.SendMessage("The supply captain must be defeated first.");
                            break;
                        }

                        BeginChannel(player, objective, "damage-anchor", TimeSpan.FromSeconds(30.0), "You begin dismantling the supply anchor.");
                    }
                    else
                    {
                        BeginChannel(player, objective, "repair-anchor", TimeSpan.FromSeconds(30.0), "You begin reinforcing the supply anchor.");
                    }
                    break;
                case InvasionRole.CivicWard:
                    player.SendMessage("This ward changes through uncontested control of the surrounding ground.");
                    break;
            }
        }

        private static void BeginChannel(
            PlayerMobile player,
            InvasionObjectiveItem objective,
            string action,
            TimeSpan duration,
            string message
        )
        {
            CancelChannel(player, null);
            InvasionChannel channel = new InvasionChannel();
            channel.PlayerSerial = player.Serial.Value;
            channel.ObjectiveSerial = objective.Serial.Value;
            channel.StartLocation = player.Location;
            channel.CompletesUtc = DateTime.UtcNow + duration;
            channel.Action = action;
            m_Channels[player.Serial.Value] = channel;
            player.SendMessage(0x59, message + " Remain still for {0} seconds.", (int)duration.TotalSeconds);
        }

        private static void ProcessChannels()
        {
            List<InvasionChannel> channels = new List<InvasionChannel>(m_Channels.Values);

            for (int i = 0; i < channels.Count; ++i)
            {
                InvasionChannel channel = channels[i];
                PlayerMobile player = World.FindMobile((Serial)channel.PlayerSerial) as PlayerMobile;
                InvasionObjectiveItem objective = World.FindItem((Serial)channel.ObjectiveSerial) as InvasionObjectiveItem;

                if (player == null || player.Deleted || !player.Alive || player.NetState == null || objective == null || objective.Deleted)
                {
                    m_Channels.Remove(channel.PlayerSerial);
                    continue;
                }

                if (player.Location != channel.StartLocation || player.Map != objective.Map || !player.InRange(objective.GetWorldLocation(), 3))
                {
                    CancelChannel(player, "Your invasion action was interrupted.");
                    continue;
                }

                if (DateTime.UtcNow >= channel.CompletesUtc)
                {
                    m_Channels.Remove(channel.PlayerSerial);
                    CompleteChannel(player, objective, channel.Action);
                }
            }
        }

        private static void CompleteChannel(PlayerMobile player, InvasionObjectiveItem objective, string action)
        {
            InvasionCityState city = GetCityState(objective.InvasionCity);

            if (city == null
                || city.Session != objective.InvasionSession
                || city.StaffPaused
                || city.IdlePaused
                || player.AccessLevel > AccessLevel.Player)
                return;

            InvasionSide side = GetSide(player, city.City);

            if (action == "cleanse-ready")
            {
                if (city.State != InvasionState.RitualReady
                    || objective.InvasionRole != InvasionRole.RitualFocus
                    || side != InvasionSide.Defender)
                {
                    return;
                }

                string key = "cleanse:" + city.City + ":" + GetAccountName(player);

                if (HasCooldown(key))
                {
                    player.SendMessage("You cannot cleanse this city's focus again yet.");
                    return;
                }

                SetCooldown(key, DateTime.UtcNow + TimeSpan.FromHours(1.0));
                city.Corruption = Math.Max(0, city.Corruption - 10);
                player.SendMessage(0x59, "You remove a measure of corruption from the city.");

                if (city.Corruption < CorruptionThreshold)
                {
                    DeleteOwnedEntities(city);
                    city.State = InvasionState.Dormant;
                    city.Faction = InvasionFaction.None;
                    city.Session = 0;
                    city.RitualLeaderSerial = 0;
                    city.StateStartedUtc = DateTime.UtcNow;
                    city.PhaseEndsUtc = DateTime.MinValue;
                    city.LastDecayUtc = DateTime.UtcNow;
                    city.CampHoldSeconds = 0;
                }
            }
            else if (action == "destroy-ritual")
            {
                if (city.State != InvasionState.Ritual
                    || objective.InvasionRole != InvasionRole.RitualFocus
                    || side != InvasionSide.Defender)
                {
                    return;
                }

                ResetCity(city, 50, TimeSpan.Zero, TimeSpan.FromHours(6.0), true);
                Announce("Defenders have shattered the ritual against " + InvasionCities.Get(city.City).Name + ".");
            }
            else if (action == "cleanse-camp")
            {
                int index = objective.InvasionObjectiveIndex;

                if (city.State == InvasionState.Encirclement
                    && objective.InvasionRole == InvasionRole.CampStandard
                    && side == InvasionSide.Defender
                    && index >= 0
                    && index < city.CampsCleansed.Length
                    && FindOwnedCreature(city, InvasionRole.Lieutenant, index) == null)
                {
                    city.CampsCleansed[index] = true;
                    objective.Delete();
                    player.SendMessage(0x59, "The camp standard collapses.");

                    int cleansed = 0;

                    for (int i = 0; i < city.CampsCleansed.Length; ++i)
                    {
                        if (city.CampsCleansed[i])
                            cleansed++;
                    }

                    if (cleansed >= 2)
                        DefenderVictory(city, "Defenders have broken the encirclement of " + InvasionCities.Get(city.City).Name + ".");
                }
            }
            else if (action == "damage-anchor")
            {
                int index = objective.InvasionObjectiveIndex;

                if (city.State == InvasionState.Occupied
                    && objective.InvasionRole == InvasionRole.SupplyAnchor
                    && side == InvasionSide.Defender
                    && index >= 0
                    && index < city.AnchorIntegrity.Length
                    && city.CampsCleansed[index]
                    && city.AnchorIntegrity[index] > 0)
                {
                    city.AnchorIntegrity[index] = Math.Max(0, city.AnchorIntegrity[index] - 10);
                    player.SendMessage(0x59, "The supply anchor falls to {0} integrity.", city.AnchorIntegrity[index]);

                    if (city.AnchorIntegrity[index] == 0)
                    {
                        objective.Delete();
                        Announce("An occupation supply anchor in " + InvasionCities.Get(city.City).Name + " has been destroyed.");
                    }
                }
            }
            else if (action == "repair-anchor")
            {
                int index = objective.InvasionObjectiveIndex;

                if (city.State != InvasionState.Occupied
                    || objective.InvasionRole != InvasionRole.SupplyAnchor
                    || side != InvasionSide.Invader
                    || index < 0
                    || index >= city.AnchorIntegrity.Length
                    || city.AnchorIntegrity[index] <= 0)
                {
                    return;
                }

                if (city.AnchorRepairDayUtc[index].Date != DateTime.UtcNow.Date)
                {
                    city.AnchorRepairDayUtc[index] = DateTime.UtcNow.Date;
                    city.AnchorDailyRepair[index] = 0;
                }

                if (city.AnchorDailyRepair[index] >= 25 || city.AnchorIntegrity[index] >= 100)
                {
                    player.SendMessage("That anchor cannot receive more reinforcement now.");
                    return;
                }

                int repair = Math.Min(5, Math.Min(25 - city.AnchorDailyRepair[index], 100 - city.AnchorIntegrity[index]));
                city.AnchorDailyRepair[index] += repair;
                city.AnchorIntegrity[index] += repair;
                player.SendMessage(0x59, "You reinforce the supply anchor to {0} integrity.", city.AnchorIntegrity[index]);
            }
        }

        private static void CancelChannel(PlayerMobile player, string message)
        {
            if (player == null)
                return;

            if (m_Channels.Remove(player.Serial.Value) && !String.IsNullOrEmpty(message))
                player.SendMessage(message);
        }

        public static string GetObjectiveStatus(InvasionObjectiveItem objective)
        {
            if (objective == null)
                return null;

            InvasionCityState city = GetCityState(objective.InvasionCity);

            if (city == null || city.Session != objective.InvasionSession)
                return "[inactive]";

            int index = objective.InvasionObjectiveIndex;

            if (objective.InvasionRole == InvasionRole.CivicWard && index >= 0 && index < city.WardProgressSeconds.Length)
                return String.Format("[corruption {0}%]", (city.WardProgressSeconds[index] * 100) / 300);

            if (objective.InvasionRole == InvasionRole.SupplyAnchor && index >= 0 && index < city.AnchorIntegrity.Length)
                return String.Format("[integrity {0}]", city.AnchorIntegrity[index]);

            return null;
        }

        private static void DefenderVictory(InvasionCityState city, string message)
        {
            ResetCity(city, 0, TimeSpan.FromHours(72.0), TimeSpan.Zero, false);
            Announce(message);
        }

        private static void Liberate(InvasionCityState city, string message)
        {
            city.State = InvasionState.Liberation;
            ResetCity(city, 0, TimeSpan.FromHours(72.0), TimeSpan.Zero, false);
            Announce(message);
        }

        private static void ResetCity(
            InvasionCityState city,
            int corruption,
            TimeSpan protection,
            TimeSpan ritualLock,
            bool preserveCycleCredits
        )
        {
            DeleteOwnedEntities(city);
            RestoreCivilizedSpawners(city);
            RestoreCityConsent(city.City);
            CancelCityChannels(city.City);
            if (!preserveCycleCredits)
                RemoveCycleCredits(city.City);
            city.State = InvasionState.Dormant;
            city.Faction = InvasionFaction.None;
            city.Session = 0;
            city.Corruption = Math.Max(0, Math.Min(100, corruption));
            city.RitualLeaderSerial = 0;
            if (!preserveCycleCredits)
                city.CycleStartedUtc = DateTime.UtcNow;
            city.StateStartedUtc = DateTime.UtcNow;
            city.PhaseEndsUtc = DateTime.MinValue;
            city.LastPlayerSeenUtc = DateTime.UtcNow;
            city.LastDecayUtc = DateTime.UtcNow;
            city.ProtectionUntilUtc = protection > TimeSpan.Zero ? DateTime.UtcNow + protection : DateTime.MinValue;
            city.RitualLockUntilUtc = ritualLock > TimeSpan.Zero ? DateTime.UtcNow + ritualLock : DateTime.MinValue;
            city.CampHoldSeconds = 0;
            city.CampsCleansed = new bool[3];
            city.WardProgressSeconds = new int[3];
            city.AnchorIntegrity = new int[] { 100, 100, 100 };
            city.AnchorDailyRepair = new int[3];
            city.AnchorRepairDayUtc = new DateTime[] { DateTime.UtcNow.Date, DateTime.UtcNow.Date, DateTime.UtcNow.Date };
            city.HighestParticipants = 0;
            city.CommanderDead = false;
            city.WardenDead = false;
            city.IdlePaused = false;
            city.StaffPaused = false;
        }

        private static void RemoveCycleCredits(InvasionCityId city)
        {
            List<InvasionCycleCredit> credits = InvasionWorldState.Instance.CycleCredits;

            for (int i = credits.Count - 1; i >= 0; --i)
            {
                if (credits[i].City == city)
                    credits.RemoveAt(i);
            }
        }

        private static void CancelCityChannels(InvasionCityId city)
        {
            List<int> remove = new List<int>();

            foreach (KeyValuePair<int, InvasionChannel> pair in m_Channels)
            {
                InvasionObjectiveItem objective = World.FindItem((Serial)pair.Value.ObjectiveSerial) as InvasionObjectiveItem;

                if (objective == null || objective.InvasionCity == city)
                    remove.Add(pair.Key);
            }

            for (int i = 0; i < remove.Count; ++i)
                m_Channels.Remove(remove[i]);
        }

        private static bool AllAnchorsDestroyed(InvasionCityState city)
        {
            return CountDestroyedAnchors(city) == city.AnchorIntegrity.Length;
        }

        private static int CountDestroyedAnchors(InvasionCityState city)
        {
            int count = 0;

            for (int i = 0; i < city.AnchorIntegrity.Length; ++i)
            {
                if (city.AnchorIntegrity[i] <= 0)
                    count++;
            }

            return count;
        }

        private static InvasionObjectiveItem SpawnObjective(
            InvasionCityState city,
            InvasionRole role,
            int index,
            InvasionSide side,
            Point3D location
        )
        {
            InvasionObjectiveItem objective = new InvasionObjectiveItem(city.Session, city.City, city.Faction, side, role, index);
            RegisterAndMove(city, objective, FindSpawnPoint(location, 4));
            return objective;
        }

        private static InvasionCreature SpawnCreature(
            InvasionCityState city,
            InvasionRole role,
            int index,
            InvasionSide side,
            Point3D location,
            int participantScale
        )
        {
            InvasionCreature creature = new InvasionCreature(city.Session, city.City, city.Faction, side, role, index, participantScale);
            RegisterAndMove(city, creature, FindSpawnPoint(location, 6));
            return creature;
        }

        public static void TrySummonAbyssalSupport(InvasionCreature source)
        {
            if (source == null
                || source.Deleted
                || source.InvasionFaction != InvasionFaction.AbyssalLegion
                || source.InvasionRole != InvasionRole.RankCaster)
            {
                return;
            }

            InvasionCityState city = GetCityState(source.InvasionCity);

            if (city == null
                || city.Session != source.InvasionSession
                || !city.IsConflictActive
                || city.StaffPaused
                || city.IdlePaused
                || CountTransientForces(city) >= 24)
            {
                return;
            }

            SpawnCreature(
                city,
                InvasionRole.RankMelee,
                -1,
                InvasionSide.Invader,
                FindSpawnPoint(source.Location, 4),
                Math.Max(1, city.HighestParticipants)
            );
        }

        private static void RegisterAndMove(InvasionCityState city, IEntity entity, Point3D location)
        {
            if (entity == null)
                return;

            Item item = entity as Item;
            Mobile mobile = entity as Mobile;

            if (item != null)
                item.MoveToWorld(location, Map.Sosaria);
            else if (mobile != null)
            {
                mobile.MoveToWorld(location, Map.Sosaria);

                BaseCreature creature = mobile as BaseCreature;
                IInvasionOwned owned = mobile as IInvasionOwned;

                if (creature != null && owned != null)
                {
                    creature.Home = location;
                    creature.RangeHome = owned.InvasionRole == InvasionRole.Commander
                            || owned.InvasionRole == InvasionRole.CivicWarden
                        ? 8
                        : 12;
                }
            }
            else
                return;

            if (!city.OwnedSerials.Contains(entity.Serial.Value))
                city.OwnedSerials.Add(entity.Serial.Value);
        }

        private static Point3D FindSpawnPoint(Point3D preferred, int radius)
        {
            Map map = Map.Sosaria;

            if (map.CanSpawnMobile(preferred))
                return preferred;

            for (int distance = 1; distance <= radius; ++distance)
            {
                for (int x = -distance; x <= distance; ++x)
                {
                    for (int y = -distance; y <= distance; ++y)
                    {
                        if (Math.Abs(x) != distance && Math.Abs(y) != distance)
                            continue;

                        int worldX = preferred.X + x;
                        int worldY = preferred.Y + y;
                        int z = map.GetAverageZ(worldX, worldY);
                        Point3D candidate = new Point3D(worldX, worldY, z);

                        if (map.CanSpawnMobile(candidate))
                            return candidate;
                    }
                }
            }

            return preferred;
        }

        private static InvasionObjectiveItem FindOwnedObjective(InvasionCityState city, InvasionRole role, int index)
        {
            for (int i = 0; i < city.OwnedSerials.Count; ++i)
            {
                InvasionObjectiveItem objective = World.FindItem((Serial)city.OwnedSerials[i]) as InvasionObjectiveItem;

                if (objective != null && !objective.Deleted && objective.InvasionSession == city.Session && objective.InvasionRole == role && objective.InvasionObjectiveIndex == index)
                    return objective;
            }

            return null;
        }

        private static InvasionCreature FindOwnedCreature(InvasionCityState city, InvasionRole role, int index)
        {
            for (int i = 0; i < city.OwnedSerials.Count; ++i)
            {
                InvasionCreature creature = World.FindMobile((Serial)city.OwnedSerials[i]) as InvasionCreature;

                if (creature != null && !creature.Deleted && creature.InvasionSession == city.Session && creature.InvasionRole == role && creature.InvasionObjectiveIndex == index)
                    return creature;
            }

            return null;
        }

        private static Mobile FindOwnedMobile(InvasionCityState city, InvasionRole role)
        {
            for (int i = 0; i < city.OwnedSerials.Count; ++i)
            {
                Mobile mobile = World.FindMobile((Serial)city.OwnedSerials[i]);
                IInvasionOwned owned = mobile as IInvasionOwned;

                if (mobile != null && !mobile.Deleted && owned != null && owned.InvasionSession == city.Session && owned.InvasionRole == role)
                    return mobile;
            }

            return null;
        }

        private static int CountOwnedCreatures(InvasionCityState city, InvasionRole role)
        {
            int count = 0;

            for (int i = 0; i < city.OwnedSerials.Count; ++i)
            {
                InvasionCreature creature = World.FindMobile((Serial)city.OwnedSerials[i]) as InvasionCreature;

                if (creature != null && !creature.Deleted && creature.InvasionSession == city.Session && creature.InvasionRole == role)
                    count++;
            }

            return count;
        }

        private static void TrimOwnedCreatures(InvasionCityState city, InvasionRole role, int target)
        {
            int count = CountOwnedCreatures(city, role);

            for (int i = city.OwnedSerials.Count - 1; i >= 0 && count > target; --i)
            {
                InvasionCreature creature = World.FindMobile((Serial)city.OwnedSerials[i]) as InvasionCreature;

                if (creature != null && creature.InvasionSession == city.Session && creature.InvasionRole == role)
                {
                    creature.Delete();
                    city.OwnedSerials.RemoveAt(i);
                    count--;
                }
            }
        }

        private static void DeleteOwnedRole(InvasionCityState city, InvasionRole role)
        {
            for (int i = city.OwnedSerials.Count - 1; i >= 0; --i)
            {
                int serial = city.OwnedSerials[i];
                IEntity entity = World.FindItem((Serial)serial);

                if (entity == null)
                    entity = World.FindMobile((Serial)serial);

                IInvasionOwned owned = entity as IInvasionOwned;

                if (owned != null && owned.InvasionSession == city.Session && owned.InvasionRole == role)
                {
                    entity.Delete();
                    city.OwnedSerials.RemoveAt(i);
                }
            }
        }

        private static void DeleteOwnedEntities(InvasionCityState city)
        {
            for (int i = city.OwnedSerials.Count - 1; i >= 0; --i)
            {
                int serial = city.OwnedSerials[i];
                IEntity entity = World.FindItem((Serial)serial);

                if (entity == null)
                    entity = World.FindMobile((Serial)serial);

                IInvasionOwned owned = entity as IInvasionOwned;

                if (entity != null && owned != null && owned.InvasionCity == city.City && owned.InvasionSession == city.Session)
                    entity.Delete();
            }

            city.OwnedSerials.Clear();
        }

        private static void SuppressCivilizedSpawners(InvasionCityState city)
        {
            foreach (Item item in World.Items.Values)
            {
                Spawner spawner = item as Spawner;

                if (spawner == null || spawner.Deleted || spawner.Map != Map.Sosaria || !InvasionCities.Get(city.City).ContainsTown(spawner.Location))
                    continue;

                if (HasSpawnerRecord(city, spawner.Serial.Value) || !IsExclusivelyCivilized(spawner))
                    continue;

                InvasionSpawnerRecord record = new InvasionSpawnerRecord();
                record.Serial = spawner.Serial.Value;
                record.WasRunning = spawner.Running;
                city.SuppressedSpawners.Add(record);
                spawner.Running = false;
                spawner.RemoveSpawned();
            }
        }

        private static void ReconcileOccupiedSpawners(InvasionCityState city)
        {
            for (int i = 0; i < city.SuppressedSpawners.Count; ++i)
            {
                InvasionSpawnerRecord record = city.SuppressedSpawners[i];
                Spawner spawner = World.FindItem((Serial)record.Serial) as Spawner;

                if (spawner != null && !spawner.Deleted && spawner.Running)
                {
                    spawner.Running = false;
                    spawner.RemoveSpawned();
                    Log("Reconciliation re-suppressed civilized spawner 0x" + spawner.Serial.Value.ToString("X8") + " in " + InvasionCities.Get(city.City).Name + ".");
                }
            }

            SuppressCivilizedSpawners(city);
        }

        private static bool HasSpawnerRecord(InvasionCityState city, int serial)
        {
            for (int i = 0; i < city.SuppressedSpawners.Count; ++i)
            {
                if (city.SuppressedSpawners[i].Serial == serial)
                    return true;
            }

            return false;
        }

        private static bool IsExclusivelyCivilized(Spawner spawner)
        {
            if (spawner.SpawnNames == null || spawner.SpawnNames.Count == 0)
                return false;

            for (int i = 0; i < spawner.SpawnNames.Count; ++i)
            {
                Type type = ScriptCompiler.FindTypeByName(Spawner.ParseType(spawner.SpawnNames[i]));

                if (type == null || !IsCivilizedType(type))
                    return false;
            }

            return true;
        }

        private static bool IsCivilizedType(Type type)
        {
            if (typeof(BaseVendor).IsAssignableFrom(type)
                || typeof(BaseHealer).IsAssignableFrom(type)
                || typeof(TownGuards).IsAssignableFrom(type)
                || typeof(Citizens).IsAssignableFrom(type))
            {
                return true;
            }

            string name = type.Name;
            return name == "TownCrier" || name == "Peasant" || name == "Noble" || name == "Citizen" || name == "Beggar";
        }

        public static List<string> GetSpawnerValidationWarnings(InvasionCityId cityId)
        {
            List<string> warnings = new List<string>();
            InvasionCityDefinition definition = InvasionCities.Get(cityId);

            foreach (Item item in World.Items.Values)
            {
                Spawner spawner = item as Spawner;

                if (spawner == null
                    || spawner.Deleted
                    || spawner.Map != Map.Sosaria
                    || !definition.ContainsTown(spawner.Location)
                    || spawner.SpawnNames == null
                    || spawner.SpawnNames.Count == 0)
                {
                    continue;
                }

                bool civilized = false;
                bool other = false;
                bool unresolved = false;

                for (int i = 0; i < spawner.SpawnNames.Count; ++i)
                {
                    Type type = ScriptCompiler.FindTypeByName(Spawner.ParseType(spawner.SpawnNames[i]));

                    if (type == null)
                        unresolved = true;
                    else if (IsCivilizedType(type))
                        civilized = true;
                    else
                        other = true;
                }

                if (unresolved || (civilized && other))
                {
                    warnings.Add(
                        String.Format(
                            "{0} spawner 0x{1:X8} at {2} is {3} and will be left untouched.",
                            definition.Name,
                            spawner.Serial.Value,
                            spawner.Location,
                            unresolved ? "unresolved" : "mixed"
                        )
                    );
                }
            }

            return warnings;
        }

        private static void RestoreCivilizedSpawners(InvasionCityState city)
        {
            for (int i = 0; i < city.SuppressedSpawners.Count; ++i)
            {
                InvasionSpawnerRecord record = city.SuppressedSpawners[i];
                Spawner spawner = World.FindItem((Serial)record.Serial) as Spawner;

                if (spawner == null || spawner.Deleted)
                    continue;

                spawner.Running = record.WasRunning;

                if (record.WasRunning)
                    spawner.Respawn();
            }

            city.SuppressedSpawners.Clear();
        }

        public static void ReconcileWorld()
        {
            InvasionWorldState state = InvasionWorldState.Instance;

            if (state == null)
                return;

            InvasionCityState[] cities = state.GetCities();

            if (state.EngineEnabled && !m_DuplicateState)
            {
                for (int i = 0; i < cities.Length; ++i)
                    ReconcileCity(cities[i]);
            }

            for (int i = state.ConsentRecords.Count - 1; i >= 0; --i)
            {
                InvasionConsentRecord record = state.ConsentRecords[i];
                InvasionCityState city = GetCityState(record.City);
                PlayerMobile player = World.FindMobile((Serial)record.PlayerSerial) as PlayerMobile;

                if (!state.EngineEnabled || m_DuplicateState || city == null || !city.IsConflictActive)
                {
                    if (player != null && record.ConsentChanged && player.NONPK == NONPK.NONPKinEvent)
                        player.NONPK = record.PreviousConsent;

                    state.ConsentRecords.RemoveAt(i);
                    continue;
                }

                if (player == null || player.Deleted)
                {
                    state.ConsentRecords.RemoveAt(i);
                    continue;
                }

                bool inside = player.Map == Map.Sosaria && InvasionCities.Get(record.City).ContainsConflict(player.Location);
                bool combatLocked = record.LastCombatUtc > DateTime.MinValue
                    && record.LastCombatUtc + ConflictExitLock > DateTime.UtcNow;

                if (!inside && !combatLocked)
                {
                    if (record.ConsentChanged && player.NONPK == NONPK.NONPKinEvent)
                        player.NONPK = record.PreviousConsent;

                    state.ConsentRecords.RemoveAt(i);
                }
                else if (record.ConsentChanged && player.NONPK == record.PreviousConsent)
                {
                    player.NONPK = NONPK.NONPKinEvent;
                    Log("Reconciliation restored invasion consent for " + player.Name + ".");
                }
            }
        }

        private static void ReconcileCity(InvasionCityState city)
        {
            for (int i = city.OwnedSerials.Count - 1; i >= 0; --i)
            {
                int serial = city.OwnedSerials[i];
                IEntity entity = World.FindItem((Serial)serial);

                if (entity == null)
                    entity = World.FindMobile((Serial)serial);

                IInvasionOwned owned = entity as IInvasionOwned;

                if (entity == null || owned == null || owned.InvasionCity != city.City || owned.InvasionSession != city.Session)
                    city.OwnedSerials.RemoveAt(i);
            }

            foreach (Item item in World.Items.Values)
            {
                IInvasionOwned owned = item as IInvasionOwned;

                if (owned != null && owned.InvasionCity == city.City && owned.InvasionSession == city.Session && !city.OwnedSerials.Contains(item.Serial.Value))
                    city.OwnedSerials.Add(item.Serial.Value);
            }

            foreach (Mobile mobile in World.Mobiles.Values)
            {
                IInvasionOwned owned = mobile as IInvasionOwned;

                if (owned != null && owned.InvasionCity == city.City && owned.InvasionSession == city.Session && !city.OwnedSerials.Contains(mobile.Serial.Value))
                    city.OwnedSerials.Add(mobile.Serial.Value);
            }

            if (!city.Enabled)
                return;

            if ((city.State == InvasionState.RitualReady || city.State == InvasionState.Ritual) &&
                FindOwnedObjective(city, InvasionRole.RitualFocus, -1) == null)
                SpawnObjective(city, InvasionRole.RitualFocus, -1, InvasionSide.Invader, InvasionCities.Get(city.City).Ritual);
            else if (!city.IsConflictActive)
                return;
            else if (city.State == InvasionState.Encirclement)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (!city.CampsCleansed[i] && FindOwnedObjective(city, InvasionRole.CampStandard, i) == null)
                        SpawnObjective(city, InvasionRole.CampStandard, i, InvasionSide.Invader, InvasionCities.Get(city.City).Camps[i]);
                }
            }
            else if (city.State == InvasionState.Breach)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (FindOwnedObjective(city, InvasionRole.CivicWard, i) == null)
                        SpawnObjective(city, InvasionRole.CivicWard, i, InvasionSide.Defender, InvasionCities.Get(city.City).Wards[i]);
                }
            }
            else if (city.State == InvasionState.FinalBattle)
            {
                if (city.CommanderDead)
                    DefenderVictory(city, "The invading commander has fallen. " + InvasionCities.Get(city.City).Name + " is saved.");
                else if (city.WardenDead)
                    EnterOccupation(city);
                else
                    EnsureFinalBattle(city);
            }
            else if (city.State == InvasionState.Occupied)
            {
                if (city.CommanderDead && AllAnchorsDestroyed(city))
                    Liberate(city, "The occupation commander has fallen and " + InvasionCities.Get(city.City).Name + " is liberated.");
                else
                {
                    ReconcileOccupiedSpawners(city);
                    EnsureOccupation(city);
                }
            }
        }

        public static bool ValidateCity(InvasionCityId cityId, out string reason)
        {
            reason = null;
            InvasionCityDefinition definition = InvasionCities.Get(cityId);
            InvasionCityState city = GetCityState(cityId);

            if (city != null)
                city.ValidationPassed = false;

            if (Map.Sosaria == null)
            {
                reason = "Map.Sosaria is unavailable.";
                return false;
            }

            if (!ValidateFactionConstruction(cityId, out reason))
                return false;

            for (int i = 0; i < definition.RegionBounds.Length; ++i)
            {
                Rectangle2D bounds = definition.RegionBounds[i];
                Point3D center = new Point3D(
                    bounds.X + (bounds.Width / 2),
                    bounds.Y + (bounds.Height / 2),
                    Map.Sosaria.GetAverageZ(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2))
                );
                Region region = Region.Find(center, Map.Sosaria);

                if (!IsRegionNamed(region, definition.RegionName))
                {
                    reason = "Configured city rectangle " + (i + 1) + " does not resolve through region '" + definition.RegionName + "'.";
                    return false;
                }
            }

            Point3D resolved;

            if (!TryResolveAnchor(definition.Ritual, out resolved))
            {
                reason = "The ritual anchor has no spawnable tile within twelve tiles.";
                return false;
            }

            definition.Ritual = resolved;

            for (int i = 0; i < definition.Camps.Length; ++i)
            {
                if (!TryResolveAnchor(definition.Camps[i], out resolved))
                {
                    reason = "Camp anchor " + (i + 1) + " has no spawnable tile within twelve tiles.";
                    return false;
                }

                definition.Camps[i] = resolved;
            }

            for (int i = 0; i < definition.Wards.Length; ++i)
            {
                if (!TryResolveAnchor(definition.Wards[i], out resolved))
                {
                    reason = "Civic ward " + (i + 1) + " has no spawnable tile within twelve tiles.";
                    return false;
                }

                definition.Wards[i] = resolved;
            }

            if (!TryResolveAnchor(definition.FinalBattle, out resolved))
            {
                reason = "The final-battle anchor has no spawnable tile within twelve tiles.";
                return false;
            }

            definition.FinalBattle = resolved;

            if (HasLegacyOverlap(cityId))
            {
                reason = "Legacy invasion objects overlap this city's conflict area. Review the legacy scan first.";
                return false;
            }

            if (city != null)
                city.ValidationPassed = true;

            return true;
        }

        private static bool ValidateFactionConstruction(InvasionCityId cityId, out string reason)
        {
            reason = null;
            InvasionRole[] roles = new InvasionRole[]
            {
                InvasionRole.RankMelee,
                InvasionRole.RankRanged,
                InvasionRole.RankCaster
            };

            try
            {
                for (InvasionFaction faction = InvasionFaction.ClockworkDominion; faction <= InvasionFaction.AbyssalLegion; faction++)
                {
                    for (int i = 0; i < roles.Length; ++i)
                    {
                        InvasionCreature creature = new InvasionCreature(0, cityId, faction, InvasionSide.Invader, roles[i], -1, 1);
                        creature.Delete();
                    }
                }
            }
            catch (Exception exception)
            {
                reason = "Typed faction construction failed: " + exception.Message;
                return false;
            }

            return true;
        }

        private static bool IsRegionNamed(Region region, string expectedName)
        {
            while (region != null)
            {
                if (String.Equals(region.Name, expectedName, StringComparison.OrdinalIgnoreCase))
                    return true;

                region = region.Parent;
            }

            return false;
        }

        private static bool TryResolveAnchor(Point3D preferred, out Point3D resolved)
        {
            resolved = FindSpawnPoint(preferred, 12);
            return Map.Sosaria.CanSpawnMobile(resolved);
        }

        public static bool TrySetCityEnabled(InvasionCityId cityId, bool enabled, out string reason)
        {
            reason = null;
            InvasionCityState city = GetCityState(cityId);

            if (city == null)
            {
                reason = "The invasion state is unavailable.";
                return false;
            }

            if (!enabled)
            {
                if (city.State != InvasionState.Dormant)
                {
                    reason = "Abort or resolve the active invasion before disabling this city.";
                    return false;
                }

                city.Enabled = false;
                city.ValidationPassed = false;
                return true;
            }

            if (InvasionWorldState.Instance == null || !InvasionWorldState.Instance.LegacyScanReviewed)
            {
                reason = "Run and review the legacy invasion scan before enabling any city.";
                return false;
            }

            if (!ValidateCity(cityId, out reason))
                return false;

            city.Enabled = true;
            return true;
        }

        public static bool TrySetEngineEnabled(bool enabled, out string reason)
        {
            reason = null;

            if (InvasionWorldState.Instance == null)
            {
                reason = "The invasion state is unavailable.";
                return false;
            }

            if (enabled && m_DuplicateState)
            {
                reason = "Multiple world-state items exist.";
                return false;
            }

            if (enabled)
            {
                InvasionCityState[] cities = InvasionWorldState.Instance.GetCities();

                for (int i = 0; i < cities.Length; ++i)
                {
                    if (cities[i].State != InvasionState.Dormant)
                    {
                        reason = "Abort the disabled recovery state for " + InvasionCities.Get(cities[i].City).Name + " before enabling the engine.";
                        return false;
                    }
                }
            }

            if (!enabled)
            {
                InvasionCityState[] cities = InvasionWorldState.Instance.GetCities();

                for (int i = 0; i < cities.Length; ++i)
                {
                    if (cities[i].State != InvasionState.Dormant)
                        ResetCity(cities[i], 0, TimeSpan.FromHours(72.0), TimeSpan.Zero, false);
                }
            }

            InvasionWorldState.Instance.EngineEnabled = enabled;
            return true;
        }

        public static bool AddTestCorruption(InvasionCityId cityId, int amount)
        {
            InvasionCityState city = GetCityState(cityId);

            if (InvasionWorldState.Instance == null
                || !InvasionWorldState.Instance.EngineEnabled
                || m_DuplicateState
                || city == null
                || !city.Enabled
                || city.State != InvasionState.Dormant)
            {
                return false;
            }

            int previous = city.Corruption;
            city.Corruption = Math.Max(0, Math.Min(100, city.Corruption + amount));
            AnnounceCorruptionThresholds(city, previous);

            if (city.Corruption >= 100)
                EnterRitualReady(city);

            return true;
        }

        public static bool ForceAdvance(InvasionCityId cityId, int expectedSession, out string reason)
        {
            reason = null;
            InvasionCityState city = GetCityState(cityId);

            if (InvasionWorldState.Instance == null
                || !InvasionWorldState.Instance.EngineEnabled
                || m_DuplicateState
                || city == null
                || !city.Enabled
                || (city.Session != expectedSession && city.State != InvasionState.Dormant))
            {
                reason = "The city state changed; reopen the invasion console.";
                return false;
            }

            switch (city.State)
            {
                case InvasionState.Dormant:
                    city.Corruption = 100;
                    EnterRitualReady(city);
                    break;
                case InvasionState.RitualReady:
                    StartRitualInternal(city, InvasionFaction.ClockworkDominion, 0);
                    break;
                case InvasionState.Ritual:
                    EnterEncirclement(city);
                    break;
                case InvasionState.Encirclement:
                    EnterBreach(city);
                    break;
                case InvasionState.Breach:
                    EnterFinalBattle(city);
                    break;
                case InvasionState.FinalBattle:
                    EnterOccupation(city);
                    break;
                default:
                    reason = "That phase cannot be advanced.";
                    return false;
            }

            return true;
        }

        public static bool SetPaused(InvasionCityId cityId, int expectedSession, bool paused)
        {
            InvasionCityState city = GetCityState(cityId);

            if (city == null || city.Session != expectedSession || city.State == InvasionState.Dormant)
                return false;

            city.StaffPaused = paused;

            if (paused)
                CancelCityChannels(cityId);

            return true;
        }

        public static bool Abort(InvasionCityId cityId, int expectedSession)
        {
            InvasionCityState city = GetCityState(cityId);

            if (city == null || city.Session != expectedSession || city.State == InvasionState.Dormant)
                return false;

            ResetCity(city, 0, TimeSpan.FromHours(72.0), TimeSpan.Zero, false);
            Announce("Staff have safely aborted the invasion of " + InvasionCities.Get(cityId).Name + ".");
            return true;
        }

        public static bool ForceLiberate(InvasionCityId cityId, int expectedSession)
        {
            InvasionCityState city = GetCityState(cityId);

            if (city == null || city.Session != expectedSession || city.State != InvasionState.Occupied)
                return false;

            Liberate(city, InvasionCities.Get(cityId).Name + " has been liberated by staff intervention.");
            return true;
        }

        public static string GetFactionName(InvasionFaction faction)
        {
            switch (faction)
            {
                case InvasionFaction.ClockworkDominion:
                    return "The Clockwork Dominion";
                case InvasionFaction.BloodCourt:
                    return "The Blood Court";
                case InvasionFaction.AbyssalLegion:
                    return "The Abyssal Legion";
                default:
                    return "No faction";
            }
        }

        public static string GetQualitativeCorruption(int corruption)
        {
            if (corruption >= 100)
                return "ritual-ready";
            if (corruption >= 75)
                return "the wards are faltering";
            if (corruption >= 50)
                return "dark influence is spreading";
            if (corruption >= 25)
                return "uneasy";

            return "calm";
        }

        public static string GetObjectiveSummary(InvasionCityState city)
        {
            if (city == null)
                return "No active objectives.";

            switch (city.State)
            {
                case InvasionState.RitualReady:
                    return "Ritual focus ready; defenders may cleanse corruption.";
                case InvasionState.Ritual:
                    return String.Format("Ritual presence: {0}/600 active seconds.", Math.Min(600, city.CampHoldSeconds));
                case InvasionState.Encirclement:
                {
                    int cleansed = 0;

                    for (int i = 0; i < city.CampsCleansed.Length; ++i)
                    {
                        if (city.CampsCleansed[i])
                            cleansed++;
                    }

                    return String.Format("Camps cleansed: {0}/2; invader hold: {1}/300 seconds.", cleansed, Math.Min(300, city.CampHoldSeconds));
                }
                case InvasionState.Breach:
                    return String.Format(
                        "Ward corruption: {0}% / {1}% / {2}%.",
                        (city.WardProgressSeconds[0] * 100) / 300,
                        (city.WardProgressSeconds[1] * 100) / 300,
                        (city.WardProgressSeconds[2] * 100) / 300
                    );
                case InvasionState.FinalBattle:
                    return String.Format("Commander {0}; civic warden {1}.", city.CommanderDead ? "dead" : "alive", city.WardenDead ? "dead" : "alive");
                case InvasionState.Occupied:
                    return String.Format(
                        "Supply integrity: {0} / {1} / {2}; commander {3}.",
                        city.AnchorIntegrity[0],
                        city.AnchorIntegrity[1],
                        city.AnchorIntegrity[2],
                        AllAnchorsDestroyed(city) ? "vulnerable" : "protected"
                    );
                default:
                    return "No active objectives.";
            }
        }

        public static void RecordStaffAction(Mobile staff, string action)
        {
            string name = staff == null ? "unknown staff" : staff.Name + " (0x" + staff.Serial.Value.ToString("X8") + ")";
            Log(name + " performed invasion staff action: " + action + ".");
        }

        public static List<Item> GetLegacyInvasionObjects()
        {
            List<Item> objects = new List<Item>();

            foreach (Item item in World.Items.Values)
            {
                if (IsLegacyInvasionObject(item))
                    objects.Add(item);
            }

            return objects;
        }

        public static void MarkLegacyScanReviewed()
        {
            if (InvasionWorldState.Instance != null)
                InvasionWorldState.Instance.LegacyScanReviewed = true;
        }

        private static bool IsLegacyInvasionObject(Item item)
        {
            if (item == null || item.Deleted || item is InvasionWorldState || item is InvasionObjectiveItem)
                return false;

            if (item.GetType().Name.EndsWith("InvasionStone", StringComparison.OrdinalIgnoreCase))
                return true;

            if (!(item is Spawner) && !(item is WayPoint))
                return false;

            return !String.IsNullOrEmpty(item.Name) && item.Name.IndexOf("invasion", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool HasLegacyOverlap(InvasionCityId city)
        {
            List<Item> objects = GetLegacyInvasionObjects();
            InvasionCityDefinition definition = InvasionCities.Get(city);

            for (int i = 0; i < objects.Count; ++i)
            {
                Item item = objects[i];

                if (item.Map == Map.Sosaria && definition.ContainsConflict(item.Location))
                    return true;
            }

            return false;
        }

        public static int CleanupLegacyInvasionObjects(IList<int> displayedSerials)
        {
            if (displayedSerials == null)
                return 0;

            int removed = 0;

            for (int i = 0; i < displayedSerials.Count; ++i)
            {
                Item item = World.FindItem((Serial)displayedSerials[i]);

                if (!IsLegacyInvasionObject(item))
                    continue;

                item.Delete();
                removed++;
            }

            MarkLegacyScanReviewed();
            Log("Staff removed " + removed + " explicitly confirmed legacy invasion objects.");
            return removed;
        }
    }
}
