using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.Invasions
{
    public static partial class InvasionService
    {
        private const int CorruptionThreshold = 100;
        private static readonly TimeSpan EstablishedAccountAge = TimeSpan.FromDays(7.0);
        private static readonly TimeSpan EstablishedGameTime = TimeSpan.FromHours(24.0);
        private static readonly TimeSpan ContributionWindow = TimeSpan.FromHours(24.0);
        private static readonly TimeSpan ConflictExitLock = TimeSpan.FromMinutes(2.0);
        private static readonly TimeSpan TraitorDuration = TimeSpan.FromMinutes(30.0);

        private static readonly Dictionary<InvasionCityId, Dictionary<string, DateTime>> m_RecentParticipants =
            new Dictionary<InvasionCityId, Dictionary<string, DateTime>>();
        private static readonly Dictionary<int, InvasionChannel> m_Channels = new Dictionary<int, InvasionChannel>();

        private static InvasionTimer m_Timer;
        private static bool m_DuplicateState;
        public static bool DuplicateStateDetected { get { return m_DuplicateState; } }
        public static InvasionWorldState WorldState { get { return InvasionWorldState.Instance; } }

        public static void Initialize()
        {
            EventSink.WorldLoad += new WorldLoadEventHandler(EventSink_WorldLoad);
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            EventSink.Logout += new LogoutEventHandler(EventSink_Logout);
            EventSink.Movement += new MovementEventHandler(EventSink_Movement);
            EventSink.AggressiveAction += new AggressiveActionEventHandler(EventSink_AggressiveAction);
            EventSink.CastSpellRequest += new CastSpellRequestEventHandler(EventSink_CastSpellRequest);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_PlayerDeath);

            for (int i = 0; i < InvasionCities.Definitions.Length; ++i)
                m_RecentParticipants[(InvasionCityId)i] = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        }

        private static void EventSink_WorldLoad()
        {
            List<InvasionWorldState> states = new List<InvasionWorldState>();

            foreach (Item item in World.Items.Values)
            {
                InvasionWorldState state = item as InvasionWorldState;

                if (state != null && !state.Deleted)
                    states.Add(state);
            }

            if (states.Count == 0)
            {
                InvasionWorldState.SetInstance(new InvasionWorldState());
                Log("Created the organic invasion world-state singleton.");
            }
            else
            {
                InvasionWorldState.SetInstance(states[0]);
            }

            m_DuplicateState = states.Count > 1;

            if (m_DuplicateState)
            {
                for (int i = 0; i < states.Count; ++i)
                    states[i].EngineEnabled = false;

                RestoreDuplicateConsentStates(states);
                Log("Multiple organic invasion world-state items were found. The engine is disabled until staff resolve them.");
            }
            else
            {
                AdjustForOfflineTime(InvasionWorldState.Instance);
            }

            InvasionCityState[] cities = InvasionWorldState.Instance.GetCities();

            for (int i = 0; i < cities.Length; ++i)
                cities[i].ValidationPassed = false;

            if (!m_DuplicateState)
                ReconcileWorld();

            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = new InvasionTimer();
            m_Timer.Start();
        }

        private static void AdjustForOfflineTime(InvasionWorldState state)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan offline = state.LastEngineTickUtc == DateTime.MinValue
                ? TimeSpan.Zero
                : now - state.LastEngineTickUtc;
            state.LastEngineTickUtc = now;

            if (!state.EngineEnabled || offline <= TimeSpan.Zero)
                return;

            InvasionCityState[] cities = state.GetCities();

            for (int i = 0; i < cities.Length; ++i)
            {
                InvasionCityState city = cities[i];

                if (!city.IsConflictActive || city.State == InvasionState.Occupied)
                    continue;

                if (city.PhaseEndsUtc > DateTime.MinValue && city.PhaseEndsUtc < DateTime.MaxValue)
                    city.PhaseEndsUtc = city.PhaseEndsUtc + offline;

                city.LastPlayerSeenUtc = now;
            }

            Log("Reconciliation froze " + Math.Max(0, (int)offline.TotalSeconds) + " seconds of offline invasion objective time.");
        }

        private static void RestoreDuplicateConsentStates(List<InvasionWorldState> states)
        {
            for (int stateIndex = 0; stateIndex < states.Count; ++stateIndex)
            {
                List<InvasionConsentRecord> records = states[stateIndex].ConsentRecords;

                for (int i = 0; i < records.Count; ++i)
                {
                    InvasionConsentRecord record = records[i];
                    PlayerMobile player = World.FindMobile((Serial)record.PlayerSerial) as PlayerMobile;

                    if (player != null && record.ConsentChanged && player.NONPK == NONPK.NONPKinEvent)
                        player.NONPK = record.PreviousConsent;
                }
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;

            if (player != null)
                UpdatePlayerConflictState(player);
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;

            if (player != null)
                CancelChannel(player, "Your invasion action was interrupted.");
        }

        private static void EventSink_Movement(MovementEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;

            if (player == null)
                return;

            UpdatePlayerConflictState(player);

            InvasionConsentRecord record = FindConsent(player);

            if (record != null && IsGraceActive(record))
            {
                if (DistanceSquared(player.Location, record.EntryLocation) > 9)
                    EndGrace(player);
            }

            InvasionChannel channel;

            if (m_Channels.TryGetValue(player.Serial.Value, out channel) && player.Location != channel.StartLocation)
                CancelChannel(player, "Movement interrupted your invasion action.");
        }

        private static void EventSink_AggressiveAction(AggressiveActionEventArgs e)
        {
            PlayerMobile aggressor = ResolvePlayer(e.Aggressor);
            PlayerMobile victim = ResolvePlayer(e.Aggressed);

            if (e.Aggressor != null)
                EndGrace(aggressor);

            CancelChannel(aggressor, "Combat interrupted your invasion action.");
            CancelChannel(victim, "Combat interrupted your invasion action.");

            if (e.Criminal)
                RecordCriminalAggression(e.Aggressor, e.Aggressed);
        }

        private static void EventSink_CastSpellRequest(CastSpellRequestEventArgs e)
        {
            EndGrace(e.Mobile);
            CancelChannel(ResolvePlayer(e.Mobile), "Spellcasting interrupted your invasion action.");
        }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;

            if (player != null)
            {
                CancelChannel(player, null);
                RestoreConsent(player, true);
            }
        }

        public static InvasionCityState GetCityState(InvasionCityId city)
        {
            if (InvasionWorldState.Instance == null)
                return null;

            return InvasionWorldState.Instance.GetCity(city);
        }

        public static bool TryGetCity(Map map, Point3D location, out InvasionCityId city)
        {
            city = InvasionCityId.Britain;

            if (map != Map.Sosaria)
                return false;

            InvasionCityDefinition[] definitions = InvasionCities.Definitions;

            for (int i = 0; i < definitions.Length; ++i)
            {
                if (definitions[i].ContainsTown(location))
                {
                    city = definitions[i].City;
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetConflictCity(Map map, Point3D location, out InvasionCityId city)
        {
            city = InvasionCityId.Britain;

            if (map != Map.Sosaria
                || InvasionWorldState.Instance == null
                || !InvasionWorldState.Instance.EngineEnabled
                || m_DuplicateState)
                return false;

            InvasionCityDefinition[] definitions = InvasionCities.Definitions;

            for (int i = 0; i < definitions.Length; ++i)
            {
                InvasionCityState state = GetCityState(definitions[i].City);

                if (state != null && state.Enabled && state.IsConflictActive && definitions[i].ContainsConflict(location))
                {
                    city = definitions[i].City;
                    return true;
                }
            }

            return false;
        }

        public static bool IsEligibleEvil(Mobile mobile)
        {
            PlayerMobile player = ResolvePlayer(mobile);

            if (player == null || player.AccessLevel > AccessLevel.Player)
                return false;

            if (player.Kills >= 5)
                return true;

            return player.Karma <= -5000
                && (player.Skills[SkillName.Necromancy].Base >= 50.0 || player.Skills[SkillName.Knightship].Base >= 50.0);
        }

        public static InvasionSide GetSide(Mobile mobile, InvasionCityId city)
        {
            IInvasionOwned owned = mobile as IInvasionOwned;

            if (owned != null && owned.InvasionCity == city)
                return owned.InvasionSide;

            PlayerMobile player = ResolvePlayer(mobile);

            if (player == null || player.AccessLevel > AccessLevel.Player)
                return InvasionSide.None;

            InvasionConsentRecord record = FindConsent(player);

            if (record != null && record.City == city)
            {
                if (record.TraitorUntilUtc > DateTime.UtcNow || IsTraitor(player, city))
                    return InvasionSide.Defender;

                return record.AssignedSide;
            }

            if (IsTraitor(player, city))
                return InvasionSide.Defender;

            return IsEligibleEvil(player) ? InvasionSide.Invader : InvasionSide.Defender;
        }

        public static bool CanHarm(Mobile from, Mobile target)
        {
            bool allowed;

            if (TryAllowHarmful(from, target, out allowed))
                return allowed;

            return true;
        }

        public static bool CanBenefit(Mobile from, Mobile target)
        {
            bool allowed;

            if (TryAllowBeneficial(from, target, out allowed))
                return allowed;

            return true;
        }

        public static bool IsValidInvasionCombat(Mobile first, Mobile second)
        {
            InvasionCityId city;

            return TryResolveConflictPair(first, second, out city)
                && GetSide(first, city) != InvasionSide.None
                && GetSide(second, city) != InvasionSide.None
                && GetSide(first, city) != GetSide(second, city);
        }

        public static bool TryAllowHarmful(Mobile from, Mobile target, out bool allowed)
        {
            allowed = false;

            InvasionCityId city;

            if (!TryResolveConflictPair(from, target, out city))
                return false;

            PlayerMobile attackingPlayer = ResolvePlayer(from);
            PlayerMobile targetPlayer = ResolvePlayer(target);

            EndGrace(attackingPlayer);
            CancelChannel(attackingPlayer, "Combat interrupted your invasion action.");

            if (targetPlayer != null && HasGrace(targetPlayer))
            {
                if (attackingPlayer != null)
                    attackingPlayer.SendMessage(0x22, "That player is still under invasion-entry grace.");

                return true;
            }

            InvasionSide fromSide = GetSide(from, city);
            InvasionSide targetSide = GetSide(target, city);

            if (fromSide == InvasionSide.Invader && targetSide == InvasionSide.Invader && target is IInvasionOwned && attackingPlayer != null)
            {
                MarkTraitor(attackingPlayer, city);
                fromSide = InvasionSide.Defender;
            }

            if (fromSide == InvasionSide.None || targetSide == InvasionSide.None)
                return false;

            if (fromSide == targetSide)
            {
                if (attackingPlayer != null)
                    attackingPlayer.SendMessage(0x22, "You cannot harm allies in this invasion.");

                return true;
            }

            MarkCombat(attackingPlayer);
            MarkCombat(targetPlayer);
            allowed = true;
            return true;
        }

        public static bool TryAllowBeneficial(Mobile from, Mobile target, out bool allowed)
        {
            allowed = false;
            InvasionCityId city;

            if (!TryResolveConflictPair(from, target, out city))
                return false;

            EndGrace(from);
            CancelChannel(ResolvePlayer(from), "Using an ability interrupted your invasion action.");
            InvasionSide fromSide = GetSide(from, city);
            InvasionSide targetSide = GetSide(target, city);

            if (fromSide == InvasionSide.None || targetSide == InvasionSide.None)
                return false;

            if (fromSide != targetSide)
            {
                from.SendMessage(0x22, "You cannot aid the opposing invasion force.");
                return true;
            }

            return false;
        }

        public static bool TryResolveEnemy(IInvasionOwned source, Mobile target, out bool enemy)
        {
            enemy = false;

            if (source == null || target == null)
                return false;

            InvasionCityState state = GetCityState(source.InvasionCity);

            if (state == null || !state.IsConflictActive || state.Session != source.InvasionSession)
                return false;

            InvasionSide targetSide = GetSide(target, source.InvasionCity);

            if (targetSide == InvasionSide.None)
                return false;

            enemy = source.InvasionSide != targetSide;
            return true;
        }

        private static bool TryResolveConflictPair(Mobile first, Mobile second, out InvasionCityId city)
        {
            city = InvasionCityId.Britain;

            if (first == null || second == null || first.Map == null || first.Map != second.Map)
                return false;

            IInvasionOwned firstOwned = first as IInvasionOwned;
            IInvasionOwned secondOwned = second as IInvasionOwned;

            if (firstOwned != null)
            {
                city = firstOwned.InvasionCity;
                return IsCurrentOwned(firstOwned) && IsMobileInConflict(second, city);
            }

            if (secondOwned != null)
            {
                city = secondOwned.InvasionCity;
                return IsCurrentOwned(secondOwned) && IsMobileInConflict(first, city);
            }

            InvasionCityId firstCity;
            InvasionCityId secondCity;

            if (!TryGetConflictCity(first.Map, first.Location, out firstCity) || !TryGetConflictCity(second.Map, second.Location, out secondCity))
                return false;

            city = firstCity;
            return firstCity == secondCity;
        }

        private static bool IsCurrentOwned(IInvasionOwned owned)
        {
            InvasionCityState state = GetCityState(owned.InvasionCity);
            return state != null && state.Enabled && state.IsConflictActive && state.Session == owned.InvasionSession;
        }

        public static bool IsActiveOwned(IInvasionOwned owned)
        {
            return owned != null && IsCurrentOwned(owned);
        }

        private static bool IsMobileInConflict(Mobile mobile, InvasionCityId city)
        {
            if (mobile == null || mobile.Map != Map.Sosaria)
                return false;

            IInvasionOwned owned = mobile as IInvasionOwned;

            if (owned != null)
                return owned.InvasionCity == city && IsCurrentOwned(owned);

            return InvasionCities.Get(city).ContainsConflict(mobile.Location);
        }

        public static void RecordReportedMurder(Mobile killer, Mobile victim)
        {
            RecordPlayerCrime(killer, victim, InvasionContributionType.Murder, 25, TimeSpan.FromHours(24.0), 50);
        }

        public static void RecordTheft(Mobile thief, Mobile victim)
        {
            RecordPlayerCrime(thief, victim, InvasionContributionType.Theft, 5, TimeSpan.FromHours(1.0), 20);
        }

        public static void RecordCriminalAggression(Mobile aggressor, Mobile victim)
        {
            RecordPlayerCrime(aggressor, victim, InvasionContributionType.CriminalAggression, 2, TimeSpan.FromMinutes(30.0), 10);
        }

        private static void RecordPlayerCrime(
            Mobile offenderMobile,
            Mobile victimMobile,
            InvasionContributionType type,
            int points,
            TimeSpan pairCooldown,
            int categoryCap
        )
        {
            PlayerMobile offender = ResolvePlayer(offenderMobile);
            PlayerMobile victim = ResolvePlayer(victimMobile);

            if (!CanRecordCrime(offender, victim))
                return;

            InvasionCityId city;

            if (!TryGetCity(offender.Map, offender.Location, out city) || !InvasionCities.Get(city).ContainsTown(victim.Location))
                return;

            InvasionCityState cityState = GetCityState(city);

            if (!CanGainCorruption(cityState) || IsValidOpposingConflict(offender, victim, city))
                return;

            string offenderAccount = GetAccountName(offender);
            string victimAccount = GetAccountName(victim);
            string key = String.Format("crime:{0}:{1}:{2}:{3}", city, type, offenderAccount, victimAccount);

            if (HasCooldown(key))
            {
                Log(String.Format("Rejected {0} corruption credit for {1} in {2}: pair cooldown.", type, offenderAccount, InvasionCities.Get(city).Name));
                return;
            }

            int categoryTotal = SumContributions(offenderAccount, city, type);
            int total = SumContributions(offenderAccount, city, null);
            int awarded = Math.Min(points, Math.Min(categoryCap - categoryTotal, 80 - total));

            if (awarded <= 0)
            {
                Log(String.Format("Rejected {0} corruption credit for {1} in {2}: rolling cap reached.", type, offenderAccount, InvasionCities.Get(city).Name));
                return;
            }

            if (awarded < points)
                Log(String.Format("Capped {0} corruption credit for {1} in {2} from {3} to {4}.", type, offenderAccount, InvasionCities.Get(city).Name, points, awarded));

            SetCooldown(key, DateTime.UtcNow + pairCooldown);
            ApplyContribution(offenderAccount, cityState, type, awarded);
        }

        public static void RecordNpcCrime(Mobile offenderMobile, Mobile victim)
        {
            PlayerMobile offender = ResolvePlayer(offenderMobile);

            if (offender == null || victim == null || victim.Deleted || victim is IInvasionOwned || !IsEstablished(offender) || !IsEligibleEvil(offender))
                return;

            int points;

            if (victim is TownGuards)
                points = 8;
            else if (victim is BaseVendor || victim is Citizens)
                points = 4;
            else
                return;

            InvasionCityId city;

            if (!TryGetCity(victim.Map, victim.Location, out city))
                return;

            InvasionCityState cityState = GetCityState(city);

            if (!CanGainCorruption(cityState))
                return;

            string account = GetAccountName(offender);
            BaseCreature creature = victim as BaseCreature;
            int spawnerId = creature == null ? 0 : creature.SpawnerID;
            string key = String.Format("npc:{0}:{1}:{2}:{3}", city, account, spawnerId, victim.GetType().Name);

            if (HasCooldown(key))
            {
                Log(String.Format("Rejected NPC corruption credit for {0} in {1}: source/type cooldown.", account, InvasionCities.Get(city).Name));
                return;
            }

            int categoryTotal = SumContributions(account, city, InvasionContributionType.NpcCrime);
            int total = SumContributions(account, city, null);
            int awarded = Math.Min(points, Math.Min(10 - categoryTotal, 80 - total));

            if (awarded <= 0)
            {
                Log(String.Format("Rejected NPC corruption credit for {0} in {1}: rolling cap reached.", account, InvasionCities.Get(city).Name));
                return;
            }

            if (awarded < points)
                Log(String.Format("Capped NPC corruption credit for {0} in {1} from {2} to {3}.", account, InvasionCities.Get(city).Name, points, awarded));

            SetCooldown(key, DateTime.UtcNow + TimeSpan.FromHours(1.0));
            ApplyContribution(account, cityState, InvasionContributionType.NpcCrime, awarded);
        }

        private static bool CanRecordCrime(PlayerMobile offender, PlayerMobile victim)
        {
            if (offender == null || victim == null || offender == victim)
                return false;

            if (offender.AccessLevel > AccessLevel.Player || victim.AccessLevel > AccessLevel.Player)
                return false;

            if (!IsEligibleEvil(offender) || !IsEstablished(offender) || !IsEstablished(victim))
                return false;

            return !String.Equals(GetAccountName(offender), GetAccountName(victim), StringComparison.OrdinalIgnoreCase);
        }

        private static bool CanGainCorruption(InvasionCityState state)
        {
            return InvasionWorldState.Instance != null
                && InvasionWorldState.Instance.EngineEnabled
                && !m_DuplicateState
                && state != null
                && state.Enabled
                && state.State == InvasionState.Dormant
                && state.ProtectionUntilUtc <= DateTime.UtcNow;
        }

        private static bool IsValidOpposingConflict(PlayerMobile offender, PlayerMobile victim, InvasionCityId city)
        {
            InvasionCityState state = GetCityState(city);

            if (state == null || !state.IsConflictActive)
                return false;

            return GetSide(offender, city) != GetSide(victim, city);
        }

        private static void ApplyContribution(
            string accountName,
            InvasionCityState city,
            InvasionContributionType type,
            int points
        )
        {
            int oldCorruption = city.Corruption;
            InvasionContributionEvent entry = new InvasionContributionEvent();
            entry.AccountName = accountName;
            entry.City = city.City;
            entry.Type = type;
            entry.Points = points;
            entry.CreatedUtc = DateTime.UtcNow;
            InvasionWorldState.Instance.Contributions.Add(entry);

            InvasionCycleCredit credit = FindCycleCredit(accountName, city.City);

            if (credit == null)
            {
                credit = new InvasionCycleCredit();
                credit.AccountName = accountName;
                credit.City = city.City;
                InvasionWorldState.Instance.CycleCredits.Add(credit);
            }

            credit.Points += points;

            if (oldCorruption <= 0)
                city.LastDecayUtc = DateTime.UtcNow;

            city.Corruption = Math.Min(CorruptionThreshold, city.Corruption + points);
            AnnounceCorruptionThresholds(city, oldCorruption);
            Log(String.Format("{0} gained {1} corruption in {2} from {3}; total {4}.", accountName, points, InvasionCities.Get(city.City).Name, type, city.Corruption));

            if (city.Corruption >= CorruptionThreshold)
                EnterRitualReady(city);
        }

        private static int SumContributions(
            string accountName,
            InvasionCityId city,
            InvasionContributionType? type
        )
        {
            int total = 0;
            DateTime cutoff = DateTime.UtcNow - ContributionWindow;
            List<InvasionContributionEvent> entries = InvasionWorldState.Instance.Contributions;

            for (int i = 0; i < entries.Count; ++i)
            {
                InvasionContributionEvent entry = entries[i];

                if (entry.CreatedUtc >= cutoff
                    && entry.City == city
                    && String.Equals(entry.AccountName, accountName, StringComparison.OrdinalIgnoreCase)
                    && (!type.HasValue || entry.Type == type.Value))
                {
                    total += entry.Points;
                }
            }

            return total;
        }

        private static InvasionCycleCredit FindCycleCredit(string accountName, InvasionCityId city)
        {
            List<InvasionCycleCredit> credits = InvasionWorldState.Instance.CycleCredits;

            for (int i = 0; i < credits.Count; ++i)
            {
                if (credits[i].City == city && String.Equals(credits[i].AccountName, accountName, StringComparison.OrdinalIgnoreCase))
                    return credits[i];
            }

            return null;
        }

        private static bool HasCooldown(string key)
        {
            List<InvasionCooldownRecord> cooldowns = InvasionWorldState.Instance.Cooldowns;

            for (int i = 0; i < cooldowns.Count; ++i)
            {
                if (String.Equals(cooldowns[i].Key, key, StringComparison.OrdinalIgnoreCase))
                    return cooldowns[i].ExpiresUtc > DateTime.UtcNow;
            }

            return false;
        }

        private static void SetCooldown(string key, DateTime expiresUtc)
        {
            List<InvasionCooldownRecord> cooldowns = InvasionWorldState.Instance.Cooldowns;

            for (int i = 0; i < cooldowns.Count; ++i)
            {
                if (String.Equals(cooldowns[i].Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    cooldowns[i].ExpiresUtc = expiresUtc;
                    return;
                }
            }

            InvasionCooldownRecord record = new InvasionCooldownRecord();
            record.Key = key;
            record.ExpiresUtc = expiresUtc;
            cooldowns.Add(record);
        }

        public static bool IsEstablished(PlayerMobile player)
        {
            Account account = player == null ? null : player.Account as Account;

            return account != null
                && DateTime.UtcNow - account.Created.ToUniversalTime() >= EstablishedAccountAge
                && account.TotalGameTime >= EstablishedGameTime;
        }

        private static string GetAccountName(PlayerMobile player)
        {
            Account account = player == null ? null : player.Account as Account;
            return account == null ? String.Empty : account.Username.ToLowerInvariant();
        }

        private static PlayerMobile ResolvePlayer(Mobile mobile)
        {
            PlayerMobile player = mobile as PlayerMobile;

            if (player != null)
                return player;

            BaseCreature creature = mobile as BaseCreature;

            if (creature == null)
                return null;

            Mobile master = creature.ControlMaster ?? creature.SummonMaster ?? creature.BardMaster;
            return master as PlayerMobile;
        }

        private static void AnnounceCorruptionThresholds(InvasionCityState city, int previous)
        {
            int[] thresholds = new int[] { 25, 50, 75, 100 };

            for (int i = 0; i < thresholds.Length; ++i)
            {
                if (previous < thresholds[i] && city.Corruption >= thresholds[i])
                {
                    string message;

                    switch (thresholds[i])
                    {
                        case 25:
                            message = "Unease gathers in " + InvasionCities.Get(city.City).Name + ".";
                            break;
                        case 50:
                            message = "Crime is feeding a dark influence in " + InvasionCities.Get(city.City).Name + ".";
                            break;
                        case 75:
                            message = "The wards of " + InvasionCities.Get(city.City).Name + " are faltering.";
                            break;
                        default:
                            message = "A corruption ritual can now be raised against " + InvasionCities.Get(city.City).Name + ".";
                            break;
                    }

                    Announce(message);
                }
            }
        }

        public static bool HasGrace(PlayerMobile player)
        {
            InvasionConsentRecord record = FindConsent(player);
            return record != null && IsGraceActive(record);
        }

        public static string GetPlayerConflictStatus(PlayerMobile player)
        {
            InvasionConsentRecord record = FindConsent(player);

            if (record == null)
                return "No invasion consent override is active.";

            if (IsGraceActive(record))
                return "Entry grace: " + Math.Max(0, (int)Math.Ceiling((record.GraceUntilUtc - DateTime.UtcNow).TotalSeconds)) + " seconds.";

            if (record.LastCombatUtc > DateTime.MinValue && record.LastCombatUtc + ConflictExitLock > DateTime.UtcNow)
                return "Conflict exit lock: " + Math.Max(0, (int)Math.Ceiling((record.LastCombatUtc + ConflictExitLock - DateTime.UtcNow).TotalSeconds)) + " seconds.";

            if (record.TraitorUntilUtc > DateTime.UtcNow)
                return "Traitor status: " + Math.Max(0, (int)Math.Ceiling((record.TraitorUntilUtc - DateTime.UtcNow).TotalMinutes)) + " minutes.";

            return "Conflict rules are active.";
        }

        private static bool IsGraceActive(InvasionConsentRecord record)
        {
            return record.GraceUntilUtc > DateTime.UtcNow;
        }

        public static void EndGrace(Mobile mobile)
        {
            PlayerMobile player = ResolvePlayer(mobile);
            InvasionConsentRecord record = FindConsent(player);

            if (record != null && record.GraceUntilUtc > DateTime.MinValue)
            {
                record.GraceUntilUtc = DateTime.MinValue;
                player.SendMessage(0x22, "Your invasion-entry grace has ended.");
            }
        }

        private static void MarkCombat(PlayerMobile player)
        {
            InvasionConsentRecord record = FindConsent(player);

            if (record != null)
                record.LastCombatUtc = DateTime.UtcNow;
        }

        private static void MarkTraitor(PlayerMobile player, InvasionCityId city)
        {
            if (player == null)
                return;

            DateTime traitorUntilUtc = DateTime.UtcNow + TraitorDuration;
            SetCooldown(GetTraitorKey(player, city), traitorUntilUtc);
            InvasionConsentRecord record = FindConsent(player);

            if (record != null && record.City == city)
            {
                record.AssignedSide = InvasionSide.Defender;
                record.TraitorUntilUtc = traitorUntilUtc;
            }

            player.SendMessage(0x22, "The occupation brands you a traitor. You now fight as a defender.");
            Log(player.Name + " became an invasion traitor in " + InvasionCities.Get(city).Name + ".");
        }

        private static string GetTraitorKey(PlayerMobile player, InvasionCityId city)
        {
            return String.Format("traitor:{0}:{1}", city, GetAccountName(player));
        }

        private static bool IsTraitor(PlayerMobile player, InvasionCityId city)
        {
            return player != null && HasCooldown(GetTraitorKey(player, city));
        }

        private static DateTime GetCooldownExpiry(string key)
        {
            if (InvasionWorldState.Instance == null)
                return DateTime.MinValue;

            List<InvasionCooldownRecord> cooldowns = InvasionWorldState.Instance.Cooldowns;

            for (int i = 0; i < cooldowns.Count; ++i)
            {
                if (String.Equals(cooldowns[i].Key, key, StringComparison.OrdinalIgnoreCase))
                    return cooldowns[i].ExpiresUtc;
            }

            return DateTime.MinValue;
        }

        private static InvasionConsentRecord FindConsent(PlayerMobile player)
        {
            if (player == null || InvasionWorldState.Instance == null)
                return null;

            List<InvasionConsentRecord> records = InvasionWorldState.Instance.ConsentRecords;

            for (int i = 0; i < records.Count; ++i)
            {
                if (records[i].PlayerSerial == player.Serial.Value)
                    return records[i];
            }

            return null;
        }

        private static void UpdatePlayerConflictState(PlayerMobile player)
        {
            if (player == null || player.Deleted || InvasionWorldState.Instance == null)
                return;

            if (player.AccessLevel > AccessLevel.Player)
            {
                RestoreConsent(player, true);
                return;
            }

            InvasionConsentRecord record = FindConsent(player);
            InvasionCityId city;
            bool inside = TryGetConflictCity(player.Map, player.Location, out city);

            if (inside)
            {
                if (record != null && record.City != city)
                {
                    RestoreConsent(player, true);
                    record = null;
                }

                if (record == null)
                {
                    record = new InvasionConsentRecord();
                    record.PlayerSerial = player.Serial.Value;
                    record.City = city;
                    record.AssignedSide = IsEligibleEvil(player) && !IsTraitor(player, city)
                        ? InvasionSide.Invader
                        : InvasionSide.Defender;
                    record.PreviousConsent = player.NONPK;
                    record.ConsentChanged = player.NONPK == NONPK.NONPK;
                    record.EntryLocation = player.Location;
                    record.GraceUntilUtc = DateTime.UtcNow + TimeSpan.FromSeconds(10.0);
                    record.TraitorUntilUtc = GetCooldownExpiry(GetTraitorKey(player, city));

                    if (record.ConsentChanged)
                        player.NONPK = NONPK.NONPKinEvent;

                    InvasionWorldState.Instance.ConsentRecords.Add(record);
                    player.SendMessage(0x59, "You have entered an invasion conflict zone. Entry grace lasts ten seconds unless you move or act.");
                }

                NoteParticipant(player, city);
            }
            else if (record != null)
            {
                bool locked = record.LastCombatUtc > DateTime.MinValue && record.LastCombatUtc + ConflictExitLock > DateTime.UtcNow;

                if (!locked)
                    RestoreConsent(player, true);
            }
        }

        private static void RestoreConsent(PlayerMobile player, bool removeRecord)
        {
            InvasionConsentRecord record = FindConsent(player);

            if (record == null)
                return;

            if (player != null && !player.Deleted && record.ConsentChanged && player.NONPK == NONPK.NONPKinEvent)
                player.NONPK = record.PreviousConsent;

            if (removeRecord)
                InvasionWorldState.Instance.ConsentRecords.Remove(record);
        }

        private static void RestoreCityConsent(InvasionCityId city)
        {
            List<InvasionConsentRecord> records = InvasionWorldState.Instance.ConsentRecords;

            for (int i = records.Count - 1; i >= 0; --i)
            {
                InvasionConsentRecord record = records[i];

                if (record.City != city)
                    continue;

                PlayerMobile player = World.FindMobile((Serial)record.PlayerSerial) as PlayerMobile;

                if (player != null && record.ConsentChanged && player.NONPK == NONPK.NONPKinEvent)
                    player.NONPK = record.PreviousConsent;

                records.RemoveAt(i);
            }
        }

        public static bool CanUseOccupationService(Mobile mobile, InvasionCityId city, int session)
        {
            PlayerMobile player = ResolvePlayer(mobile);
            InvasionCityState state = GetCityState(city);

            if (player == null || state == null || state.State != InvasionState.Occupied || state.Session != session)
                return false;

            EndGrace(player);
            return IsEligibleEvil(player)
                && !IsTraitor(player, city)
                && GetSide(player, city) == InvasionSide.Invader;
        }

        private static void NoteParticipant(PlayerMobile player, InvasionCityId city)
        {
            if (!IsEstablished(player))
                return;

            string account = GetAccountName(player);

            if (!String.IsNullOrEmpty(account))
                m_RecentParticipants[city][account] = DateTime.UtcNow;
        }

        private static int DistanceSquared(Point3D first, Point3D second)
        {
            int x = first.X - second.X;
            int y = first.Y - second.Y;
            return (x * x) + (y * y);
        }

        private static void Announce(string message)
        {
            World.Broadcast(0x35, true, message);
            Log(message);

            try
            {
                LoggingFunctions.LogEvent(message, "Logging Battles");
            }
            catch
            {
                // External event delivery is informational and must never stop invasion gameplay.
            }
        }

        private static void Log(string message)
        {
            Console.WriteLine("[Organic Invasions] {0}", message);
        }

        private sealed class InvasionTimer : Timer
        {
            public InvasionTimer()
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                InvasionService.Tick();
            }
        }

        private sealed class InvasionChannel
        {
            public int PlayerSerial;
            public int ObjectiveSerial;
            public Point3D StartLocation;
            public DateTime CompletesUtc;
            public string Action;
        }
    }
}
