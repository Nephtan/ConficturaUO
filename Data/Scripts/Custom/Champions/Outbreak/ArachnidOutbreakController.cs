using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController : Item
    {
        // Open only after reward, site, combat and native-client acceptance gates.
        public static bool DeliveryReady { get { return false; } }
        private static readonly List<ArachnidOutbreakController> m_Controllers = new List<ArachnidOutbreakController>();
        private ArachnidOutbreakState m_State = new ArachnidOutbreakState();
        private ArachnidOutbreakLedger m_Ledger;
        private ArachnidOutbreakWounds m_Wounds = new ArachnidOutbreakWounds();
        private ArachnidOutbreakPoisons m_Poisons = new ArachnidOutbreakPoisons();
        private ArachnidOutbreakBards m_Bards = new ArachnidOutbreakBards();
        private List<Mobile> m_Creatures = new List<Mobile>();
        private List<Item> m_Signs = new List<Item>();
        private ArachnidOutbreakPlacement m_Placement;
        private bool m_BossCreated;
        private Timer m_Timer;
        private DateTime m_NextSearch;
        private DateTime m_NextSafetyCheck;
        private string m_LastSite = String.Empty;
        private string m_Status = "Disabled.";

        public string AttemptId { get { return m_State.AttemptId; } }
        public ArachnidOutbreakPhase Phase { get { return m_State.Phase; } }
        public string SiteId { get { return m_State.SiteId; } }
        public string Status { get { return m_Status; } }
        public int ClearedBroods { get { return m_State.ClearedBroods; } }

        public static void Initialize()
        {
            CommandSystem.Register("ArachnidOutbreak", AccessLevel.Administrator, OnCommand);
            EventSink.HealthChanged -= OnHealthChanged;
            EventSink.HealthChanged += OnHealthChanged;
            EventSink.MobileDeath -= OnMobileDeath;
            EventSink.MobileDeath += OnMobileDeath;
            EventSink.PoisonChanged -= OnPoisonChanged;
            EventSink.PoisonChanged += OnPoisonChanged;
            // Initialize runs after World.Load. Never create or enable a controller here.
            foreach (ArachnidOutbreakController controller in m_Controllers.ToArray())
                controller.Restore();
        }

        [Usage("ArachnidOutbreak <status|settings|create|start|stop> [serial]")]
        [Description("Inspect or control the separate arachnid outbreak director.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted || from.AccessLevel < AccessLevel.Administrator)
                return;
            string action = e.Length >= 1 ? e.GetString(0).ToLowerInvariant() : "status";
            int count;
            ArachnidOutbreakController controller = FindUnique(out count);
            if (e.Length >= 2) controller = World.FindItem(e.GetInt32(1)) as ArachnidOutbreakController;
            if (action == "create")
            {
                controller = new ArachnidOutbreakController();
                from.SendMessage("Created a disabled outbreak controller ({0}).", controller.Serial);
                return;
            }
            if (controller == null)
            {
                from.SendMessage("Found {0} controllers; specify the desired controller serial. No outbreak action taken.", count);
                return;
            }
            if (action == "start")
            {
                string reason;
                controller.TryEnable(out reason);
                from.SendMessage(reason);
            }
            else if (action == "stop")
            {
                controller.Stop();
                from.SendMessage("Outbreak stopped; only this controller's owned entities were removed.");
            }
            else if (action == "settings")
                controller.SendTuning(from);
            else if (action != "status")
                from.SendMessage("Usage: ArachnidOutbreak <status|settings|create|start|stop> [serial]");
            else
                from.SendMessage("{0}: {1}; site={2}; cleared={3}; {4}", controller.Serial,
                    controller.Phase, controller.SiteId, controller.ClearedBroods, controller.Status);
        }

        private static ArachnidOutbreakController FindUnique(out int count)
        {
            count = 0;
            ArachnidOutbreakController result = null;
            foreach (ArachnidOutbreakController controller in m_Controllers)
                if (!controller.Deleted)
                {
                    ++count;
                    result = controller;
                }
            return count == 1 ? result : null;
        }

        [Constructable]
        public ArachnidOutbreakController() : base(0x1F14)
        {
            Name = "arachnid outbreak controller";
            Movable = false;
            Visible = false;
            m_Controllers.Add(this);
        }

        public ArachnidOutbreakController(Serial serial) : base(serial)
        {
            m_Controllers.Add(this);
        }

        public bool TryEnable(out string reason)
        {
            if (Deleted)
            {
                reason = "Controller missing or ambiguous.";
                return false;
            }
            if (!DeliveryReady)
            {
                reason = "Delivery acceptance remains incomplete; activation is disabled.";
                return false;
            }
            m_State.SetEnabled(true);
            StartTimer();
            reason = "Director enabled; it will search for a suitable random infestation origin.";
            return true;
        }

        public void Stop()
        {
            string attempt = AttemptId;
            m_State.SetEnabled(false);
            SpreadingEncounterScheduler.Unregister(this);
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
            Cleanup(attempt);
            m_Placement = null;
            m_BossCreated = false;
            m_Status = "Disabled.";
        }

        private void Restore()
        {
            if (Deleted)
                return;
            if (!DeliveryReady)
            {
                Stop();
                return;
            }
            if (m_State.Active && (m_Ledger == null || m_Ledger.AttemptId != AttemptId))
            {
                Stop();
                m_Status = "Attempt disarmed: contribution state is missing or belongs to another attempt.";
                return;
            }
            if (m_State.Active && m_Infestation == null)
            {
                Abort(DateTime.UtcNow, "Legacy staged outbreak retired during infestation migration; earned claims retained.");
            }
            if (m_Infestation != null)
            {
                m_Infestation.Resume(DateTime.UtcNow);
                RecoverInfestation();
            }
            if (m_State.Phase != ArachnidOutbreakPhase.Disabled)
                StartTimer();
        }

        private void StartTimer()
        {
            SpreadingEncounterScheduler.Register(this);
        }

        private void OnTick()
        {
            Tick(DateTime.UtcNow);
        }

        internal void Tick(DateTime utcNow)
        {
            if (Deleted || m_State.Phase == ArachnidOutbreakPhase.Disabled)
                return;
            string attempt = AttemptId;
            if (m_Infestation != null && m_State.Active)
            {
                TickInfestation(utcNow);
                ObserveBards(utcNow, true);
                return;
            }
            if (m_State.Advance(utcNow, Utility.RandomMinMax(30, 60)))
            {
                Cleanup(attempt);
                m_Placement = null;
            }
            if (m_State.Phase == ArachnidOutbreakPhase.Searching && utcNow >= m_NextSearch)
            {
                m_NextSearch = utcNow.AddMinutes(5);
                if (TryRandomInfestation(utcNow)) return;
            }
            if (m_State.Active)
            {
                Reconcile(utcNow);
                ObserveBards(utcNow, true);
            }
        }

        private bool TryStart(ArachnidOutbreakSite site, DateTime utcNow, out string reason)
        {
            if (!site.Reviewed || m_State.Phase != ArachnidOutbreakPhase.Searching)
            {
                reason = "Site unreviewed or an attempt is already in progress.";
                return false;
            }
            ArachnidOutbreakPlacement placement;
            if (!site.Validate(new SosariaOutbreakTerrain(), out placement, out reason))
                return false;
            for (int stage = 0; stage < 3; ++stage)
                if (!Map.Sosaria.CanSpawnMobile(placement.Brood(stage)))
                {
                    reason = "A brood anchor is currently occupied.";
                    return false;
                }
            BeginAt(site, placement);
            reason = m_Status;
            return true;
        }

        private void BeginAt(ArachnidOutbreakSite site, ArachnidOutbreakPlacement placement)
        {
            if (!m_State.TryStart(site.Id))
                throw new InvalidOperationException("Outbreak lifecycle refused start.");
            m_Placement = placement;
            // Defined maximum stage budgets include every finite reinforcement,
            // even when a fast clear prevents some from appearing.
            m_Ledger = new ArachnidOutbreakLedger(AttemptId,
                ArachnidOutbreakSpider.GuardianHealth + 10 * ArachnidOutbreakSpider.DefenderMaximumHealth,
                ArachnidOutbreakChampion.ChampionHealth + 6 * ArachnidOutbreakSpider.DefenderMaximumHealth);
            m_BossCreated = false;
            m_NextSafetyCheck = DateTime.MinValue;
            m_LastSite = site.Id;
            try
            {
                for (int stage = 0; stage < 3; ++stage)
                {
                    ArachnidOutbreakSpider guardian = new ArachnidOutbreakSpider(this, stage, true);
                    m_Creatures.Add(guardian);
                    guardian.Home = placement.Brood(stage);
                    guardian.RangeHome = 6;
                    guardian.MoveToWorld(guardian.Home, Map.Sosaria);
                    ArachnidBroodSign sign = new ArachnidBroodSign(this);
                    m_Signs.Add(sign);
                    sign.MoveToWorld(guardian.Home, Map.Sosaria);
                }
                m_Status = "Three broods active.";
            }
            catch
            {
                Stop();
                throw;
            }
        }

        private bool Owns(object entity, string attempt)
        {
            IArachnidOutbreakOwned owned = entity as IArachnidOutbreakOwned;
            return owned != null && owned.Controller == this && owned.AttemptId == attempt;
        }

        private void Cleanup(string attempt)
        {
            SpreadingEncounterScheduler.ReleaseTerritory(this);
            m_Bards.Clear();
            m_Wounds.Clear();
            m_Poisons.Clear();
            m_NestResistance.Clear(); m_BroodAttackers.Clear();
            ResetQueenTactics();
            foreach (Mobile mobile in m_Creatures.ToArray())
                if (mobile != null && !mobile.Deleted && Owns(mobile, attempt))
                    mobile.Delete();
            foreach (Item item in m_Signs.ToArray())
                if (item != null && !item.Deleted && Owns(item, attempt))
                    item.Delete();
            m_Creatures.Clear();
            m_Signs.Clear();
            if (m_Infestation != null)
                foreach (ArachnidInfestationPatch patch in m_Infestation.Patches)
                    foreach (ArachnidInfestationResident resident in patch.Residents)
                        if (resident.Actor == null && resident.Inventory != null && !resident.Inventory.Deleted && resident.Inventory.Parent == null)
                            resident.Inventory.Delete();
            m_Infestation = null;
            m_FounderPath = null;
            m_PendingPatch = null; m_LivePatches.Clear(); m_LiveSlots.Clear();
        }

        private void Abort(DateTime utcNow, string reason)
        {
            string attempt = AttemptId;
            if (m_State.AbortAttempt(utcNow, Utility.RandomMinMax(30, 60)))
            {
                Cleanup(attempt);
                m_Placement = null;
                m_Status = reason;
            }
        }

        private void Reconcile(DateTime utcNow)
        {
            ArachnidOutbreakSite site = ArachnidOutbreakSites.Find(SiteId);
            if (site == null || !site.Reviewed)
            {
                Abort(utcNow, "Site no longer approved; attempt cancelled without victory.");
                return;
            }
            if (m_Placement == null)
            {
                string reason;
                if (!site.Validate(new SosariaOutbreakTerrain(), out m_Placement, out reason))
                {
                    Abort(utcNow, reason);
                    return;
                }
            }
            if (utcNow >= m_NextSafetyCheck)
            {
                m_NextSafetyCheck = utcNow.AddSeconds(30);
                SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
                if (!terrain.HasPermanentHousingProhibition(site.ClearanceBounds)
                    || terrain.HasStructuralConflict(site.ClearanceBounds))
                {
                    Abort(utcNow, "Site protection or clearance changed; attempt cancelled without victory.");
                    return;
                }
            }
            for (int stage = 0; stage < 3; ++stage)
                if ((m_State.ClearedBroods & (1 << stage)) == 0 && FindGuardian(stage) == null)
                {
                    Abort(utcNow, "A guardian was removed without a death outcome.");
                    return;
                }
            if (m_State.Phase == ArachnidOutbreakPhase.Boss)
            {
                foreach (Mobile mobile in m_Creatures)
                    if (mobile is ArachnidOutbreakChampion && !mobile.Deleted)
                        return;
                if (m_BossCreated)
                {
                    Abort(utcNow, "Mephitis was removed without a death outcome.");
                    return;
                }
                Point3D point;
                if (m_Placement.TryFindSpawn(m_Placement.Boss, 3, out point))
                {
                    ArachnidOutbreakChampion boss = new ArachnidOutbreakChampion(this);
                    m_Creatures.Add(boss);
                    boss.Home = m_Placement.Boss;
                    boss.RangeHome = 12;
                    boss.MoveToWorld(point, Map.Sosaria);
                    m_BossCreated = true;
                    m_Status = "Mephitis has emerged.";
                }
            }
        }

        private ArachnidOutbreakSpider FindGuardian(int stage)
        {
            foreach (Mobile mobile in m_Creatures)
            {
                ArachnidOutbreakSpider spider = mobile as ArachnidOutbreakSpider;
                if (spider != null && !spider.Deleted && spider.Guardian && spider.Stage == stage && Owns(spider, AttemptId))
                    return spider;
            }
            return null;
        }

        private bool OwnedStage(Mobile mobile, out int stage)
        {
            stage = -1;
            if (m_Ledger == null || mobile == null || mobile.Deleted || !m_Creatures.Contains(mobile) || !Owns(mobile, AttemptId))
                return false;
            ArachnidOutbreakSpider spider = mobile as ArachnidOutbreakSpider;
            stage = m_Infestation != null && mobile is ArachnidInfestationCreature ? InfestationMilestone() : spider == null ? 3 : spider.Stage;
            return stage >= 0 && stage < 4 && !m_Ledger.IsSealed(stage);
        }

        private static bool IsHealingPatient(Mobile mobile)
        {
            if (mobile.Player)
                return true;
            BaseCreature creature = mobile as BaseCreature;
            return creature != null && ((creature.Controlled && creature.ControlMaster != null && creature.ControlMaster.Player)
                || (creature.Summoned && creature.SummonMaster != null && creature.SummonMaster.Player));
        }

        private int BardStage(Mobile enemy)
        {
            int stage;
            return OwnedStage(enemy, out stage) ? stage : -1;
        }

        internal bool IsWebContext(ArachnidOutbreakChampion champion, Mobile victim)
        {
            int stage;
            if (Deleted || m_State.Phase != ArachnidOutbreakPhase.Boss || m_Placement == null
                || m_Ledger == null || m_Ledger.AttemptId != AttemptId || !OwnedStage(champion, out stage) || stage != 3
                || !champion.Alive || champion.Controlled || champion.Summoned || champion.BardProvoked
                || victim == null || victim.Deleted || !victim.Alive || victim.IsDeadBondedPet || !IsHealingPatient(victim)
                || victim.AccessLevel != AccessLevel.Player || champion.Map != Map.Sosaria || victim.Map != champion.Map
                || !champion.InRange(victim.Location, 12) || !champion.CanSee(victim) || !champion.InLOS(victim) || !champion.CanBeHarmful(victim, false))
                return false;
            if (m_Infestation != null) return InfestationContains(victim.Location);
            int radius = ArachnidOutbreakSite.Radius;
            return new Rectangle2D(m_Placement.Boss.X - radius, m_Placement.Boss.Y - radius, radius * 2 + 1, radius * 2 + 1).Contains(victim.Location);
        }

        internal void RegisterWeb(ArachnidOutbreakWebItem web)
        {
            if (!Owns(web, AttemptId)) throw new InvalidOperationException("Foreign outbreak web.");
            m_Signs.Add(web);
        }

        internal void RecordWebRescue(ArachnidOutbreakChampion champion, Mobile helper, Mobile patient)
        {
            if (helper != null && helper.Player && !helper.Deleted && helper != patient && IsWebContext(champion, patient))
            {
                m_Ledger.CreditValidatedSupport(3, helper, 2.0);
                m_State.RecordActivity(DateTime.UtcNow);
            }
        }

        private void ObserveBards(DateTime utcNow, bool credit)
        {
            if (!Deleted && m_State.Active && m_Ledger != null && m_Ledger.AttemptId == AttemptId
                && m_Bards.Observe(m_Creatures, BardStage, m_Ledger, utcNow.ToLocalTime(), credit))
                m_State.RecordActivity(utcNow);
        }

        // Skill effects flush their useful interval before changing, then reset
        // observation afterwards. Refreshing/replacing an effect cannot add time.
        internal static void BardEffectsChanging(Mobile enemy)
        {
            BardEffectBoundary(enemy, true);
        }

        internal static void BardEffectsChanged(Mobile enemy)
        {
            BardEffectBoundary(enemy, false);
        }

        private static void BardEffectBoundary(Mobile enemy, bool credit)
        {
            if (World.Loading)
                return;
            int stage;
            IArachnidOutbreakOwned owned = enemy as IArachnidOutbreakOwned;
            ArachnidOutbreakController controller = owned == null ? null : owned.Controller;
            if (controller != null && controller.m_State.Active && controller.OwnedStage(enemy, out stage))
                controller.ObserveBards(DateTime.UtcNow, credit);
        }

        private void RecordBardCombat(Mobile enemy, Mobile participant)
        {
            DateTime now = DateTime.UtcNow;
            ObserveBards(now, true);
            m_Bards.RecordCombat(enemy, participant);
            ObserveBards(now, false);
        }

        private static void OnMobileDeath(Mobile mobile)
        {
            if (mobile == null)
                return;
            foreach (ArachnidOutbreakController controller in m_Controllers)
            {
                controller.m_Wounds.Forget(mobile);
                controller.m_Poisons.Forget(mobile);
            }
        }

        private static void OnPoisonChanged(PoisonChangedEventArgs e)
        {
            foreach (ArachnidOutbreakController controller in m_Controllers.ToArray())
                if (!controller.Deleted) controller.OnPoisonChangedForAttempt(e);
        }

        private void OnPoisonChangedForAttempt(PoisonChangedEventArgs e)
        {
            ArachnidOutbreakController controller = this;
            if (controller == null || !controller.m_State.Active || controller.m_Ledger == null
                || controller.m_Ledger.AttemptId != controller.AttemptId || e.Mobile == null || e.Mobile.Deleted)
                return;
            if (!IsHealingPatient(e.Mobile))
            {
                controller.m_Poisons.Forget(e.Mobile);
                return;
            }
            int stage, incomingStage = -1;
            BaseCreature source = e.Source as BaseCreature;
            if (e.Kind == PoisonChangeKind.Application && source != null && !source.Player && !source.Controlled && !source.Summoned
                && !source.BardProvoked && controller.OwnedStage(source, out stage))
            {
                incomingStage = stage;
                controller.RecordBardCombat(source, e.Mobile.Player ? e.Mobile : e.Mobile.GetDamageMaster(source));
            }
            Mobile healer = e.Source == null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile);
            if (controller.m_Poisons.Observe(e, incomingStage, healer, controller.m_Ledger))
                controller.m_State.RecordActivity(DateTime.UtcNow);
        }

        internal static void RecordPoisonPrevention(Mobile patient, Mobile source, Mobile helper)
        {
            int stage;
            IArachnidOutbreakOwned owned = source as IArachnidOutbreakOwned;
            ArachnidOutbreakController controller = owned == null ? null : owned.Controller;
            if (controller == null || !controller.m_State.Active || controller.m_Ledger == null || controller.m_Ledger.AttemptId != controller.AttemptId
                || patient == null || patient.Deleted || !IsHealingPatient(patient) || helper == null || helper.Deleted)
                return;
            Mobile principal = helper.Player ? helper : helper.GetDamageMaster(patient);
            if (!controller.OwnedStage(source, out stage))
                return;
            controller.RecordBardCombat(source, patient.Player ? patient : patient.GetDamageMaster(source));
            if (principal != null && principal.Player && !principal.Deleted && principal != patient)
            {
                controller.m_Ledger.CreditValidatedSupport(stage, principal, 2.0);
                controller.m_State.RecordActivity(DateTime.UtcNow);
            }
        }

        private static void OnHealthChanged(HealthChangedEventArgs e)
        {
            foreach (ArachnidOutbreakController controller in m_Controllers.ToArray())
                if (!controller.Deleted) controller.OnHealthChangedForAttempt(e);
        }

        private void OnHealthChangedForAttempt(HealthChangedEventArgs e)
        {
            ArachnidOutbreakController controller = this;
            if (controller == null || !controller.m_State.Active || controller.m_Ledger == null
                || controller.m_Ledger.AttemptId != controller.AttemptId || e.Mobile == null || e.Mobile.Deleted)
                return;
            int stage;
            if (e.Mobile is ArachnidOutbreakChampion && controller.m_Creatures.Contains(e.Mobile) && Owns(e.Mobile,controller.AttemptId)) controller.RecordQueenPressure(e);
            if (e.Kind == HealthChangeKind.Damage && e.NewHits < e.OldHits
                && controller.m_Creatures.Contains(e.Mobile) && Owns(e.Mobile,controller.AttemptId))
                controller.RecordInfestationAttacker(e.Mobile,e.Source == null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile));
            if (e.Kind == HealthChangeKind.Damage && e.NewHits < e.OldHits && controller.OwnedStage(e.Mobile, out stage))
            {
                Mobile principal = e.Source == null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile);
                if (principal != null && principal.Player && !principal.Deleted)
                {
                    controller.RecordBardCombat(e.Mobile, principal);
                    controller.m_Ledger.CreditDamage(stage, principal, e.OldHits - e.NewHits);
                    controller.m_State.RecordActivity(DateTime.UtcNow);
                }
                else
                {
                    BaseCreature provoked = e.Mobile as BaseCreature;
                    BaseCreature opponent = e.Source as BaseCreature;
                    if (provoked != null && provoked.BardProvoked && provoked.BardTarget == opponent && provoked.BardMaster != null
                        && provoked.BardMaster.Player && opponent != null && !opponent.Player && !opponent.Controlled && !opponent.Summoned)
                        controller.RecordBardCombat(e.Mobile, provoked.BardMaster);
                }
            }
            if (!IsHealingPatient(e.Mobile))
            {
                controller.m_Wounds.Forget(e.Mobile);
                return;
            }
            int incomingStage = -1;
            BaseCreature source = e.Source as BaseCreature;
            if (e.Kind == HealthChangeKind.Damage && e.NewHits < e.OldHits && source != null && !source.Controlled && !source.Summoned
                && !source.BardProvoked && controller.OwnedStage(source, out stage))
            {
                incomingStage = stage;
                controller.RecordBardCombat(source, e.Mobile.Player ? e.Mobile : e.Mobile.GetDamageMaster(source));
            }
            Mobile healer = e.Source == null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile);
            if (controller.m_Wounds.Observe(e, incomingStage, healer, controller.m_Ledger))
                controller.m_State.RecordActivity(DateTime.UtcNow);
        }

        private void QueueStage(int stage)
        {
            if (m_Ledger == null || m_Ledger.AttemptId != AttemptId || m_Ledger.IsSealed(stage))
                return;
            foreach (Mobile participant in m_Ledger.Qualified(stage))
                ArachnidOutbreakClaim.QueueGlands(participant, AttemptId, stage);
            m_Ledger.Seal(stage);
        }

        internal void CreatureDied(Mobile mobile, Container corpse)
        {
            if (Deleted || !m_State.Active || !m_Creatures.Contains(mobile) || !Owns(mobile, AttemptId))
                return;
            if (m_Infestation != null) { InfestationDeath(mobile); return; }
            ArachnidOutbreakSpider spider = mobile as ArachnidOutbreakSpider;
            if (spider != null && spider.Guardian)
            {
                QueueStage(spider.Stage);
                m_State.CompleteBrood(spider.Stage, DateTime.UtcNow);
            }
            ArachnidOutbreakChampion champion = mobile as ArachnidOutbreakChampion;
            if (champion != null && m_State.Phase == ArachnidOutbreakPhase.Boss)
            {
                if (!TryBossRewards(champion))
                {
                    Abort(DateTime.UtcNow, "Boss outcome cancelled: no safe shared loot ground.");
                    return;
                }
                QueueStage(3);
            }
            if (mobile is ArachnidOutbreakChampion && m_State.CompleteBoss(DateTime.UtcNow, Utility.RandomMinMax(30, 60)))
            {
                Cleanup(AttemptId);
                m_Placement = null;
                m_Status = "Completed; cooldown active.";
            }
        }

        private bool TryBossRewards(ArachnidOutbreakChampion champion)
        {
            if (m_Placement == null || m_Ledger == null || m_Ledger.AttemptId != AttemptId || m_Ledger.IsSealed(3))
                return false;
            Point3D[] points;
            if (!m_Placement.TryFindLootPoints(champion.Location, 12, 25, out points)
                && !m_Placement.TryFindLootPoints(champion.Location, 56, 25, out points))
                return false;
            List<Mobile> qualified = m_Ledger.Qualified(3);
            if (qualified.Count > 0)
            {
                ArachnidOutbreakClaim.QueueReward(qualified[Utility.Random(qualified.Count)], AttemptId, 3,
                    ArachnidOutbreakClaimKind.Skull, delegate { return new ChampionSkull(champion.SkullType); });
                ArachnidOutbreakClaim.QueueReward(qualified[Utility.Random(qualified.Count)], AttemptId, 3,
                    ArachnidOutbreakClaimKind.Artifact, delegate { return champion.RollVictoryArtifact(); });
            }
            for (int i = 0; i < points.Length; ++i)
            {
                Gold gold = new Gold(600);
                gold.MoveToWorld(points[i], Map.Sosaria);
                // Award immediately; delayed callbacks only decorate existing gold.
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 160), new TimerStateCallback(PlayGoldShower), gold);
            }
            return true;
        }

        private static void PlayGoldShower(object state)
        {
            Gold gold = state as Gold;
            if (gold == null || gold.Deleted || gold.Parent != null || gold.Map != Map.Sosaria)
                return;
            Point3D above = new Point3D(gold.X, gold.Y, gold.Z + 40);
            Effects.SendMovingEffect(new Entity(Serial.Zero, above, gold.Map), gold, 0xEED, 5, 0, true, false);
            Effects.SendLocationEffect(gold.Location, gold.Map, 0x376A, 20, 10);
            Effects.PlaySound(gold.Location, gold.Map, gold.GetDropSound());
        }

        internal bool KeepCreatureInBounds(BaseCreature creature)
        {
            if (Deleted || !m_State.Active || !m_Creatures.Contains(creature) || !Owns(creature, AttemptId))
            {
                creature.Delete();
                return false;
            }
            if (m_Infestation != null)
            {
                // Pursuit and combat use ordinary AI, including outside established territory.
                if (creature.Map == Map.Sosaria && creature.Combatant == null && !InfestationContains(creature.Location))
                {
                    creature.RangeHome = 0;
                    if (creature.AIObject != null) creature.AIObject.Action = ActionType.Wander;
                }
                return true;
            }
            ArachnidOutbreakSite site = ArachnidOutbreakSites.Find(SiteId);
            if (site != null && creature.Map == Map.Sosaria && site.Bounds.Contains(creature.Location))
                return true;
            creature.Combatant = null;
            creature.FocusMob = null;
            Point3D point;
            if (m_Placement != null && m_Placement.TryFindSpawn(creature.Home, 5, out point))
                creature.MoveToWorld(point, Map.Sosaria);
            else
                Abort(DateTime.UtcNow, "No safe return point; attempt cancelled without victory.");
            return false;
        }

        internal void SpawnDefenders(Mobile source, int stage, int count)
        {
            if (m_Infestation != null) return;
            if (Deleted || !m_State.Active || m_Placement == null || !m_Creatures.Contains(source)
                || !Owns(source, AttemptId) || source.Deleted || !source.Alive || count < 1 || count > 4)
                return;
            if (stage < 0 || stage > 3 || (stage < 3 && (m_State.ClearedBroods & (1 << stage)) != 0))
                return;
            ArachnidOutbreakSite site = ArachnidOutbreakSites.Find(SiteId);
            if (site == null || new SosariaOutbreakTerrain().HasStructuralConflict(site.ClearanceBounds))
                return;
            Point3D anchor = stage == 3 ? m_Placement.Boss : m_Placement.Brood(stage);
            for (int i = 0; i < count; ++i)
            {
                Point3D point;
                if (!m_Placement.TryFindSpawn(anchor, 6, out point))
                    break;
                ArachnidOutbreakSpider defender = new ArachnidOutbreakSpider(this, stage, false);
                m_Creatures.Add(defender);
                defender.Home = anchor;
                defender.RangeHome = 6;
                defender.MoveToWorld(point, Map.Sosaria);
            }
        }

        public override void OnAfterDelete()
        {
            Stop();
            m_Controllers.Remove(this);
            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            CaptureInfestation();
            base.Serialize(writer);
            writer.Write((int)4);
            m_State.Serialize(writer);
            writer.Write(m_Creatures, true);
            writer.Write(m_Signs, true);
            writer.Write(m_NextSearch);
            writer.Write(m_LastSite);
            writer.Write(m_BossCreated);
            writer.Write(m_Ledger != null);
            if (m_Ledger != null)
                m_Ledger.Serialize(writer);
            m_Wounds.Serialize(writer);
            writer.Write(m_Infestation != null);
            if (m_Infestation != null) m_Infestation.Serialize(writer);
            m_Tuning.Serialize(writer);
            writer.Write(m_QueenBackupSpent);
            writer.Write(m_FrontierRewarded,true);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_State.Deserialize(reader);
            m_Creatures = reader.ReadStrongMobileList();
            m_Signs = reader.ReadStrongItemList();
            m_NextSearch = reader.ReadDateTime();
            m_LastSite = reader.ReadString();
            m_BossCreated = reader.ReadBool();
            if (version >= 1 && reader.ReadBool())
            {
                m_Ledger = new ArachnidOutbreakLedger();
                m_Ledger.Deserialize(reader);
            }
            if (version >= 2)
                m_Wounds.Deserialize(reader);
            if (version >= 3 && reader.ReadBool())
            {
                m_Infestation = new ArachnidInfestationState();
                m_Infestation.Deserialize(reader);
            }
            if (version >= 4)
            {
                m_Tuning.Deserialize(reader);
                m_QueenBackupSpent=reader.ReadInt();
                if (m_QueenBackupSpent<0 || m_QueenBackupSpent>12) throw new InvalidOperationException("Invalid queen backup expenditure.");
                m_FrontierRewarded=reader.ReadStrongMobileList();
            }
        }
    }
}
