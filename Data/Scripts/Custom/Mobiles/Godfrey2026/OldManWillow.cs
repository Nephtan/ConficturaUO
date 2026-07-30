using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class OldManWillow : EvilEnt
    {
        [Constructable]
        public OldManWillow()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Old Man Willow", null, 700, 40, 250);
            Godfrey2026Support.SetResistance(this, ResistanceType.Physical);
            Godfrey2026Support.AddMortalStrike(this);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            Godfrey2026Support.AddGodfreyLoot(this, 1);
        }

        public OldManWillow(Serial serial)
            : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            MobileBalanceCatalog.DropLoot(this, c);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 1)
            {
                MobileBalanceCatalog.ApplyProfile(this);
            }
        }
    }
}
