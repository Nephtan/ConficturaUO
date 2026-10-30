using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public class ArachnidInfestationCreature : BaseCreature, IArachnidOutbreakOwned
    {
        private ArachnidOutbreakController m_Controller;
        private string m_AttemptId;
        private int m_Patch, m_Tier, m_Kind;
        private bool m_Guardian, m_Founder;
        private DateTime m_NextTownScan;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public string AttemptId { get { return m_AttemptId; } }
        public int PatchId { get { return m_Patch; } }
        public int Tier { get { return m_Tier; } }
        public bool Guardian { get { return m_Guardian; } }
        public bool Founder { get { return m_Founder; } }
        public override bool HasBreath { get { return false; } }
        public override bool PlayerRangeSensitive { get { return false; } }
        public override Poison HitPoison { get { return m_Tier == 2 ? Poison.Greater : m_Kind == 1 && m_Tier == 1 ? Poison.Regular : null; } }

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
            tier = Math.Max(0,Math.Min(2,tier));
            double remaining = m_Tier < 0 ? 1.0 : HitsMax > 0 ? (double)Hits / HitsMax : 1.0;
            m_Tier = tier;
            int[] health = new int[] { 30,150,350 }, damage = new int[] { 2,10,22 };
            Body = m_Guardian ? 319 : tier == 0 ? (m_Kind == 0 ? 28 : 48) : tier == 1 ? (m_Kind == 0 ? 68 : 315) : (m_Kind == 0 ? 460 : 873);
            Hue = 0;
            Name = m_Founder ? "an arachnid brood founder" : m_Guardian ? "an arachnid nest guardian"
                : tier == 0 ? "a frontier arachnid" : tier == 1 ? "a brood defender" : "an inner brood sentinel";
            SetStr(40+tier*80); SetDex(50+tier*15); SetInt(20+tier*20);
            SetHits(health[tier]*(m_Guardian ? 4 : 1)); SetDamage(damage[tier],damage[tier]+(tier == 0 ? 6 : 8));
            SetDamageType(ResistanceType.Physical,100);
            SetResistance(ResistanceType.Physical,10+tier*20);
            SetSkill(SkillName.FistFighting,30+tier*30); SetSkill(SkillName.Tactics,30+tier*30);
            SetSkill(SkillName.MagicResist,20+tier*30);
            Hits = Math.Max(1,(int)Math.Ceiling(HitsMax*remaining));
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
            return TownEnemy(this,target) || base.IsEnemy(target);
        }

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
            if (m_Controller == null || m_Controller.Deleted || !m_Controller.KeepCreatureInBounds(this)) { if (!Deleted) Delete(); return; }
            if (DateTime.UtcNow >= m_NextTownScan) { m_NextTownScan = DateTime.UtcNow.AddSeconds(2); AcquireTownTarget(this); }
            base.OnThink();
        }

        public override void GenerateLoot() { AddLoot(m_Tier == 0 ? LootPack.Meager : m_Tier == 1 ? LootPack.Average : LootPack.Rich); }
        public override void OnDeath(Container corpse) { base.OnDeath(corpse); if (m_Controller != null) m_Controller.CreatureDied(this,corpse); }
        public ArachnidInfestationCreature(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); writer.Write(0); writer.Write(m_Controller); writer.Write(m_AttemptId);
            writer.Write(m_Patch); writer.Write(m_Tier); writer.Write(m_Kind); writer.Write(m_Guardian); writer.Write(m_Founder);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); int version = reader.ReadInt(); m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_AttemptId = reader.ReadString(); m_Patch = reader.ReadInt(); m_Tier = reader.ReadInt(); m_Kind = reader.ReadInt();
            m_Guardian = reader.ReadBool(); m_Founder = reader.ReadBool();
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
