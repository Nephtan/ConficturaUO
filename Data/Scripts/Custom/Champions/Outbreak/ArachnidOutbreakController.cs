using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public class ArachnidOutbreakController : Item
    {
        // Open only after reward, site, combat and native-client acceptance gates.
        public static bool DeliveryReady { get { return false; } }
        private static readonly List<ArachnidOutbreakController> m_Controllers = new List<ArachnidOutbreakController>();
        private ArachnidOutbreakState m_State = new ArachnidOutbreakState();
        private ArachnidOutbreakLedger m_Ledger;
        private ArachnidOutbreakWounds m_Wounds = new ArachnidOutbreakWounds();
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
            // Initialize runs after World.Load. Never create or enable a controller here.
            foreach (ArachnidOutbreakController controller in m_Controllers.ToArray())
                controller.Restore();
        }

        [Usage("ArachnidOutbreak <status|create|start|stop>")]
        [Description("Inspect or control the separate arachnid outbreak director.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || from.Deleted || from.AccessLevel < AccessLevel.Administrator)
                return;
            string action = e.Length == 1 ? e.GetString(0).ToLowerInvariant() : "status";
            int count;
            ArachnidOutbreakController controller = FindUnique(out count);
            if (action == "create" && count == 0)
            {
                controller = new ArachnidOutbreakController();
                from.SendMessage("Created a disabled outbreak controller ({0}).", controller.Serial);
                return;
            }
            if (controller == null)
            {
                from.SendMessage("Expected one controller; found {0}. No outbreak action taken.", count);
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
            else if (action != "status")
                from.SendMessage("Usage: ArachnidOutbreak <status|create|start|stop>");
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
            int count;
            if (Deleted || FindUnique(out count) != this)
            {
                reason = "Controller missing or ambiguous.";
                return false;
            }
            if (!DeliveryReady)
            {
                reason = "Delivery acceptance remains incomplete; activation is disabled.";
                return false;
            }
            bool reviewed = false;
            foreach (ArachnidOutbreakSite site in ArachnidOutbreakSites.Candidates)
                reviewed |= site.Reviewed;
            if (!reviewed)
            {
                reason = "No reviewed permanently non-housing site is available.";
                return false;
            }
            m_State.SetEnabled(true);
            StartTimer();
            reason = "Director enabled; it will select a currently safe reviewed site.";
            return true;
        }

        public void Stop()
        {
            string attempt = AttemptId;
            m_State.SetEnabled(false);
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
            int count;
            if (!DeliveryReady || FindUnique(out count) != this)
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
            if (m_State.Phase != ArachnidOutbreakPhase.Disabled)
                StartTimer();
        }

        private void StartTimer()
        {
            if (m_Timer == null)
                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), OnTick);
        }

        private void OnTick()
        {
            Tick(DateTime.UtcNow);
        }

        internal void Tick(DateTime utcNow)
        {
            if (Deleted || m_State.Phase == ArachnidOutbreakPhase.Disabled)
                return;
            int count;
            if (FindUnique(out count) != this)
            {
                Stop();
                return;
            }
            string attempt = AttemptId;
            if (m_State.Advance(utcNow, Utility.RandomMinMax(30, 60)))
            {
                Cleanup(attempt);
                m_Placement = null;
            }
            if (m_State.Phase == ArachnidOutbreakPhase.Searching && utcNow >= m_NextSearch)
            {
                m_NextSearch = utcNow.AddMinutes(5);
                List<ArachnidOutbreakSite> sites = new List<ArachnidOutbreakSite>();
                foreach (ArachnidOutbreakSite site in ArachnidOutbreakSites.Candidates)
                    if (site.Reviewed)
                        sites.Add(site);
                int offset = Utility.Random(sites.Count);
                bool started = false;
                for (int pass = 0; pass < 2 && !started; ++pass)
                {
                    for (int i = 0; i < sites.Count; ++i)
                    {
                        ArachnidOutbreakSite site = sites[(offset + i) % sites.Count];
                        if ((pass == 0 && site.Id == m_LastSite) || (pass == 1 && site.Id != m_LastSite))
                            continue;
                        string reason;
                        if (TryStart(site, utcNow, out reason))
                        {
                            started = true;
                            break;
                        }
                        m_Status = reason;
                    }
                }
                if (sites.Count == 0)
                    m_Status = "Waiting for a reviewed site; no placement fallback.";
            }
            if (m_State.Active)
                Reconcile(utcNow);
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
            m_Wounds.Clear();
            foreach (Mobile mobile in m_Creatures.ToArray())
                if (mobile != null && !mobile.Deleted && Owns(mobile, attempt))
                    mobile.Delete();
            foreach (Item item in m_Signs.ToArray())
                if (item != null && !item.Deleted && Owns(item, attempt))
                    item.Delete();
            m_Creatures.Clear();
            m_Signs.Clear();
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
            if (mobile == null || mobile.Deleted || !m_Creatures.Contains(mobile) || !Owns(mobile, AttemptId))
                return false;
            ArachnidOutbreakSpider spider = mobile as ArachnidOutbreakSpider;
            stage = spider == null ? 3 : spider.Stage;
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

        private static void OnMobileDeath(Mobile mobile)
        {
            if (mobile == null)
                return;
            foreach (ArachnidOutbreakController controller in m_Controllers)
                controller.m_Wounds.Forget(mobile);
        }

        private static void OnHealthChanged(HealthChangedEventArgs e)
        {
            int count;
            ArachnidOutbreakController controller = FindUnique(out count);
            if (controller == null || !controller.m_State.Active || controller.m_Ledger == null
                || controller.m_Ledger.AttemptId != controller.AttemptId || e.Mobile == null || e.Mobile.Deleted)
                return;
            int stage;
            if (e.Kind == HealthChangeKind.Damage && e.NewHits < e.OldHits && controller.OwnedStage(e.Mobile, out stage))
            {
                Mobile principal = e.Source == null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile);
                if (principal != null && principal.Player && !principal.Deleted)
                {
                    controller.m_Ledger.CreditDamage(stage, principal, e.OldHits - e.NewHits);
                    controller.m_State.RecordActivity(DateTime.UtcNow);
                }
            }
            if (!IsHealingPatient(e.Mobile))
            {
                controller.m_Wounds.Forget(e.Mobile);
                return;
            }
            int incomingStage = -1;
            BaseCreature source = e.Source as BaseCreature;
            if (e.Kind == HealthChangeKind.Damage && source != null && !source.Controlled && !source.Summoned
                && !source.BardProvoked && controller.OwnedStage(source, out stage))
                incomingStage = stage;
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
            Point3D[] points = new Point3D[25];
            for (int i = 0; i < points.Length; ++i)
                if (!m_Placement.TryFindLoot(m_Placement.Boss, 12, out points[i])
                    && !m_Placement.TryFindLoot(m_Placement.Boss, 56, out points[i]))
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
                new Gold(600).MoveToWorld(points[i], Map.Sosaria);
            return true;
        }

        internal bool KeepCreatureInBounds(BaseCreature creature)
        {
            if (Deleted || !m_State.Active || !m_Creatures.Contains(creature) || !Owns(creature, AttemptId))
            {
                creature.Delete();
                return false;
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
            base.Serialize(writer);
            writer.Write((int)2);
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
        }
    }
}
