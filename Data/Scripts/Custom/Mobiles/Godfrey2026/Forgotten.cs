using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Forgotten : Mummy
    {
        [Constructable]
        public Forgotten()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Forgotten", null, 500, 80, 100);
            Godfrey2026Support.AddMortalStrike(this);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public Forgotten(Serial serial)
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
