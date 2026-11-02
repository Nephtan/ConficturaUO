using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public class ArachnidInfestationCreature : BaseCreature, IArachnidOutbreakOwned, ISpreadingEncounterActor
    {
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Patch, m_Tier, m_Kind;
        private bool m_Guardian, m_Founder;
        private DateTime m_NextTownScan;
        private Guid m_Identity = Guid.NewGuid();
        private byte m_Slot;
        public ISpreadingEncounter Encounter { get { return m_Controller; } }
        public Guid Identity { get { return m_Identity; } }
        public byte Slot { get { return m_Slot; } }
        public int Kind { get { return m_Kind; } }
        public DateTime LastRelevant, NextSpecial;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int PatchId { get { return m_Patch; } }
        public int Tier { get { return m_Tier; } }
        public bool Guardian { get { return m_Guardian; } }
        public bool EggSac { get { return m_Guardian && m_Tier == 0 && CantWalk; } }
        public bool Founder { get { return m_Founder; } }
        public override bool HasBreath { get { return false; } }
        public override bool PlayerRangeSensitive { get { return false; } }
        public override Poison HitPoison { get { return m_Tier >= 3 ? Poison.Greater : m_Kind == 1 && m_Tier >= 1 ? Poison.Regular : null; } }

        public ArachnidInfestationCreature(ArachnidOutbreakController controller, int patch, int tier, bool guardian, bool founder)
            : base(AIType.AI_Melee,FightMode.Closest,10,1,0.2,0.4)
        {
            m_Controller = controller; m_AttemptId = controller.AttemptId; m_Patch = patch;
            m_Guardian = guardian; m_Founder = founder; m_Kind = Utility.Random(2);
            Tamable = false; Fame = 300; Karma = -5000; BaseSoundID = 0x388;
            m_Tier = -1; ApplyTier(tier);
        }

        public void ApplyTier(int tier)
        {
            tier = Math.Max(0,Math.Min(4,tier));
            bool initial = m_Tier < 0;
            int previousHits = Hits, previousStam = Stam, previousMana = Mana;
            m_Tier = tier;
            bool egg = m_Guardian && tier == 0 && m_Controller != null && m_Controller.IsOutskirtsEgg(this)
                && (initial || !SpreadingEncounterScheduler.Busy(this));
            int[] health = new int[] { 30,80,150,250,350 }, damage = new int[] { 2,6,10,16,22 };
            Body = egg ? 51 : m_Guardian && tier > 0 ? (tier == 4 ? 460 : 319) : tier == 0 ? 28 : tier == 1 ? (m_Kind == 0 ? 140 : 48)
                : tier == 2 ? (m_Kind == 0 ? 28 : 315) : tier == 3 ? (m_Kind == 0 ? 140 : 315) : (m_Kind == 0 ? 140 : 873);
            Hue = egg ? 0x481 : 0;
            Name = egg ? "an arachnid egg sac" : m_Founder ? "an arachnid brood founder" : m_Guardian && tier > 0 ? "an arachnid nest guardian"
                : tier == 0 ? "a frontier spider" : tier == 1 ? "a venom brood defender" : m_Kind == 0 ? "an arachnid brood sorcerer"
                : tier == 3 ? "an arachnid brood hunter" : "an inner brood sentinel";
            SetStr(40+tier*40); SetDex(50+tier*8); SetInt(20+tier*40);
            SetHits(egg ? 60 : health[tier]*(m_Guardian && tier > 0 ? 4 : 1));
            SetDamage(egg ? 0 : damage[tier],egg ? 0 : damage[tier]+(tier == 0 ? 6 : 8));
            CantWalk = egg;
            Fame = egg ? 0 : 300; Karma = egg ? 0 : -5000;
            SetDamageType(ResistanceType.Physical,100);
            SetResistance(ResistanceType.Physical,10+tier*10);
            SetSkill(SkillName.FistFighting,30+tier*15); SetSkill(SkillName.Tactics,30+tier*15);
            SetSkill(SkillName.MagicResist,20+tier*15);
            if (egg)
            {
                SetStr(1); SetDex(1); SetInt(1);
                SetSkill(SkillName.FistFighting,0); SetSkill(SkillName.Tactics,0); SetSkill(SkillName.MagicResist,0);
                SetResistance(ResistanceType.Physical,0);
            }
            bool caster = !m_Guardian && !m_Founder && m_Kind == 0 && tier >= 2;
            AIType desired = caster ? AIType.AI_Mage : AIType.AI_Melee;
            if (AI != desired) ChangeAIType(desired);
            if (caster) { SetSkill(SkillName.Magery,50+tier*10); SetSkill(SkillName.Psychology,40+tier*10); }
            FightMode = egg ? FightMode.None : !m_Guardian && !caster && tier >= 3 ? FightMode.Weakest : FightMode.Closest;
            ActiveSpeed = tier == 0 ? 0.3 : !m_Guardian && !caster && tier >= 3 ? 0.15 : 0.2;
            RangePerception = tier == 0 ? m_Controller == null ? 8 : m_Controller.FrontierPerception : 10;
            Hits = initial ? HitsMax : Math.Max(1,Math.Min(HitsMax,previousHits));
            Stam = initial ? StamMax : Math.Min(StamMax,previousStam);
            Mana = initial ? ManaMax : Math.Min(ManaMax,previousMana);
        }

        internal void Identify(Guid identity, byte slot, int kind)
        {
            m_Identity = identity; m_Slot = slot; m_Kind = kind; ApplyTier(m_Tier);
        }

        internal static bool TownEnemy(BaseCreature creature, Mobile target)
        {
            return target != null && target != creature && !target.Deleted && target.Alive && !target.Blessed
                && target.AccessLevel == AccessLevel.Player && (target is TownGuards || target is Citizens || target is BaseVendor)
                && creature.CanSee(target) && creature.InLOS(target) && creature.CanBeHarmful(target,false);
        }

        public override bool IsEnemy(Mobile target)
        {
            if (target is IArachnidOutbreakOwned) return false;
            return FrontierReach(target) && (TownEnemy(this,target) || base.IsEnemy(target));
        }

        private bool FrontierReach(Mobile target)
        {
            return m_Tier != 0 || Home == Point3D.Zero
                || target != null && target.Map == Map && target.InRange(Home,m_Controller == null ? 24 : m_Controller.FrontierPursuit);
        }

        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        { return !EggSac && FrontierReach(target) && base.CanBeHarmful(target,message,ignoreOurBlessedness); }

        internal static void AcquireTownTarget(BaseCreature creature)
        {
            if (creature.Combatant != null && creature.Combatant.Alive && !creature.Combatant.Deleted) return;
            IPooledEnumerable mobiles = creature.GetMobilesInRange(8);
            try
            {
                int examined = 0; Mobile nearest = null; double distance = Double.MaxValue;
                foreach (Mobile mobile in mobiles)
                {
                    if (++examined > 32) break;
                    double next = creature.GetDistanceToSqrt(mobile);
                    if (next < distance && TownEnemy(creature,mobile)) { nearest = mobile; distance = next; }
                }
                if (nearest != null) { creature.Combatant = nearest; creature.FocusMob = nearest; }
            }
            finally { mobiles.Free(); }
        }

        public override void OnThink()
        {
            if (m_Controller == null || m_Controller.Deleted) { Delete(); return; }
            if (!m_Controller.KeepCreatureInBounds(this)) return;
            // Weak frontier patrols defend their local patch; travel must not drag them across the world.
            if (Combatant != null && !FrontierReach(Combatant)) { Combatant = null; FocusMob = null; }
            if (FocusMob != null && !FrontierReach(FocusMob)) FocusMob = null;
            SpreadingEncounterScheduler.Track(this);
            if (DateTime.UtcNow >= m_NextTownScan)
            {
                m_NextTownScan = DateTime.UtcNow.AddSeconds(2);
                if (SpreadingEncounterScheduler.Interested(this) || SpreadingEncounterScheduler.Busy(this)) LastRelevant = DateTime.UtcNow;
                if (!EggSac && !(this is ArachnidBroodReinforcement)) AcquireTownTarget(this);
            }
            base.OnThink();
        }

        public override void GenerateLoot() { if (!EggSac) AddLoot(m_Tier == 0 ? LootPack.Meager : m_Tier <= 2 ? LootPack.Average : LootPack.Rich); }
        public override void OnDeath(Container corpse) { base.OnDeath(corpse); if (m_Controller != null) m_Controller.CreatureDied(this,corpse); }
        public override void OnAfterDelete() { SpreadingEncounterScheduler.Untrack(this); base.OnAfterDelete(); }
        public ArachnidInfestationCreature(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); writer.Write(1); writer.Write(m_Controller); writer.Write(m_AttemptId);
            writer.Write(m_Patch); writer.Write(m_Tier); writer.Write(m_Kind); writer.Write(m_Guardian); writer.Write(m_Founder);
            writer.Write(m_Identity.ToString("N")); writer.Write(m_Slot); writer.Write(LastRelevant); writer.Write(NextSpecial);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); int version = reader.ReadInt(); m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString(); m_Patch = reader.ReadInt(); m_Tier = reader.ReadInt(); m_Kind = reader.ReadInt();
            m_Guardian = reader.ReadBool(); m_Founder = reader.ReadBool();
            if (version >= 1) { m_Identity = new Guid(reader.ReadString()); m_Slot = reader.ReadByte(); LastRelevant = reader.ReadDateTime(); NextSpecial = reader.ReadDateTime(); }
            else { m_Identity = Guid.NewGuid(); LastRelevant = DateTime.UtcNow; }
        }
    }

    public class ArachnidInfestationWeb : Item, IArachnidOutbreakOwned
    {
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Patch;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int PatchId { get { return m_Patch; } }
        public ArachnidInfestationWeb(ArachnidOutbreakController controller, int patch, int art) : base(art)
        {
            m_Controller = controller; m_AttemptId = controller.AttemptId; m_Patch = patch;
            Movable = false; Name = "infestation webbing";
        }
        public ArachnidInfestationWeb(Serial serial) : base(serial) { }
        public override void OnAfterDelete() { SpreadingEncounterScheduler.Untrack(this); base.OnAfterDelete(); }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); writer.Write(0); writer.Write(m_Controller); writer.Write(m_AttemptId); writer.Write(m_Patch);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); int version = reader.ReadInt(); m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString(); m_Patch = reader.ReadInt();
        }
    }
}
