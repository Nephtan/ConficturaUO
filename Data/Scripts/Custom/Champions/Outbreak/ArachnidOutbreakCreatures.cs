using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.Confictura.Champions
{
    public interface IArachnidOutbreakOwned
    {
        ArachnidOutbreakController Controller { get; }
        string AttemptId { get; }
    }

    public class ArachnidOutbreakSpider : DreadSpider, IArachnidOutbreakOwned
    {
        public const int GuardianHealth = 3000;
        public const int DefenderMaximumHealth = 132;
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Stage;
        private bool m_Guardian;
        private int m_Reinforcements;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int Stage { get { return m_Stage; } }
        public bool Guardian { get { return m_Guardian; } }
        // Outbreak webs use the explicit warning/restraint system. Keep ordinary
        // mage AI and poison, but do not inherit HP-scaled breath/paralysis.
        public override bool HasBreath { get { return false; } }

        public ArachnidOutbreakSpider(ArachnidOutbreakController controller, int stage, bool guardian)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            m_Stage = stage;
            m_Guardian = guardian;
            Tamable = false;
            if (guardian)
            {
                Name = "an arachnid brood guardian";
                SetHits(GuardianHealth);
            }
        }


        public override void OnThink()
        {
            if (m_Controller == null || m_Controller.Deleted)
            {
                Delete();
                return;
            }
            if (!m_Controller.KeepCreatureInBounds(this))
                return;
            base.OnThink();
            if (m_Guardian)
            {
                if ((m_Reinforcements & 8) == 0)
                {
                    m_Reinforcements |= 8;
                    m_Controller.SpawnDefenders(this, m_Stage, 4);
                }
                for (int i = 0; i < 3; ++i)
                {
                    int bit = 1 << i;
                    if ((m_Reinforcements & bit) == 0 && (long)Hits * 100 <= (long)HitsMax * (75 - i * 25))
                    {
                        m_Reinforcements |= bit;
                        m_Controller.SpawnDefenders(this, m_Stage, 2);
                    }
                }
            }
        }

        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);
            if (m_Controller != null)
                m_Controller.CreatureDied(this, corpse);
        }

        public ArachnidOutbreakSpider(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
            writer.Write(m_Stage);
            writer.Write(m_Guardian);
            writer.Write(m_Reinforcements);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
            m_Stage = reader.ReadInt();
            m_Guardian = reader.ReadBool();
            m_Reinforcements = reader.ReadInt();
        }
    }

    public class ArachnidOutbreakChampion : Mephitis, IArachnidOutbreakOwned, ISpreadingEncounterActor
    {
        public const int ChampionHealth = 9000;
        public const int WebIntervalSeconds = 15;
        private DateTime m_NextWeb;
        private ArachnidWebWarning m_Web;
        private bool m_WebClockSet;
        private TurnParticipant m_WebActor;
        private TurnCombatGroup m_WebGroup;
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Reinforcements;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public override bool AwardLegacyChampionRewards { get { return false; } }
        public override bool HasBreath { get { return false; } }
        public ISpreadingEncounter Encounter { get { return m_Controller; } }
        public override bool IsEnemy(Mobile mobile)
        {
            if (m_Controller != null && m_Controller.Infestation)
            {
                if (mobile is IArachnidOutbreakOwned) return false;
                if (ArachnidInfestationCreature.TownEnemy(this,mobile)) return true;
            }
            return base.IsEnemy(mobile);
        }
        public virtual Item RollVictoryArtifact() { return GetArtifact(); }
        public override void OnAfterDelete() { SpreadingEncounterScheduler.Untrack(this); base.OnAfterDelete(); }

        private void ThinkWeb()
        {
            if (m_Web != null && !m_Web.Deleted)
            {
                m_Web.TryResolve(true);
                if (!m_Web.Deleted) return;
            }
            TryStartWeb(Combatant);
        }

        internal bool TryStartWeb(Mobile victim)
        {
            DateTime now = TurnBasedCombatBridge.GetTime(this);
            TurnParticipant actor = ArachnidOutbreakWebItem.Actor(this);
            TurnCombatGroup group = actor == null ? null : actor.Group;
            if (m_WebClockSet && (actor != m_WebActor || group != m_WebGroup))
            {
                if (m_Web != null) m_Web.Delete();
                m_NextWeb = now.AddSeconds(WebIntervalSeconds);
            }
            m_WebClockSet = true; m_WebActor = actor; m_WebGroup = group;
            if ((m_Web != null && !m_Web.Deleted) || now < m_NextWeb || ArachnidWebRestraint.Find(victim) != null
                || m_Controller == null || !m_Controller.IsWebContext(this, victim))
                return false;
            bool started = PerformWebAction(victim, delegate
            {
                m_Web = new ArachnidWebWarning(this, victim);
                return true;
            });
            if (started) m_NextWeb = TurnBasedCombatBridge.GetTime(this).AddSeconds(WebIntervalSeconds);
            return started;
        }

        internal bool PerformWebAction(Mobile victim, Func<bool> apply)
        {
            if (Deleted || !Alive || Paralyzed || Frozen || BardPacified || m_Controller == null || !m_Controller.IsWebContext(this, victim))
                return false;
            TurnParticipant participant = ArachnidOutbreakWebItem.Actor(this);
            if (participant != null && (participant.Group.Current != participant || participant.Pending != null))
                return false;
            TurnActionDecision decision = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(this, victim, this, TurnActionKind.ItemUse, 4, true, false));
            if (!decision.Allowed) return false;
            bool succeeded = false;
            try
            {
                using (IDisposable action = TurnBasedCombatBridge.BeginActionScope(decision.Lease))
                using (IDisposable effect = TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(this, victim, TurnMutationKind.Effect, 0, 0)))
                {
                    TurnParticipant targetActor;
                    if (effect == null || !ArachnidOutbreakWebItem.SameMode(this, victim, out targetActor)) return false;
                    RevealingAction();
                    DoHarmful(victim);
                    if (!m_Controller.IsWebContext(this, victim)) return false;
                    succeeded = apply();
                    return succeeded;
                }
            }
            finally
            {
                TurnBasedCombatBridge.CompleteAction(decision.Lease,
                    new TurnActionResult(this, victim, succeeded ? TurnActionPhase.Commit : TurnActionPhase.Refund, succeeded));
            }
        }

        public ArachnidOutbreakChampion(ArachnidOutbreakController controller)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            SetHits(ChampionHealth);
            Tamable = false;
        }
        public override void OnThink()
        {
            if (m_Controller == null || m_Controller.Deleted)
            {
                Delete();
                return;
            }
            if (m_Controller.Infestation) m_Controller.TrackInfestationQueen(this);
            if (!m_Controller.KeepCreatureInBounds(this))
                return;
            base.OnThink();
            if (m_Controller.Infestation) ArachnidInfestationCreature.AcquireTownTarget(this);
            ThinkWeb();
            for (int i = 0; i < 3; ++i)
            {
                int bit = 1 << i;
                if ((m_Reinforcements & bit) == 0 && (long)Hits * 100 <= (long)HitsMax * (75 - i * 25))
                {
                    m_Reinforcements |= bit;
                    m_Controller.SpawnDefenders(this, 3, 2);
                }
            }
        }
        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);
            if (m_Controller != null)
                m_Controller.CreatureDied(this, corpse);
        }
        public ArachnidOutbreakChampion(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
            writer.Write(m_Reinforcements);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
            m_Reinforcements = reader.ReadInt();
        }
    }

    public class ArachnidBroodSign : Item, IArachnidOutbreakOwned
    {
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public ArachnidBroodSign(ArachnidOutbreakController controller) : base(0x10D5)
        {
            m_Controller = controller;
            m_AttemptId = controller.AttemptId;
            Movable = false;
            Name = "a nest of thick arachnid webs";
        }
        public ArachnidBroodSign(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_AttemptId);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString();
        }
    }
}
