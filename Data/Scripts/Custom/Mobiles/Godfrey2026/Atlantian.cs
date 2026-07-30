using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Atlantian : Serpyn
    {
        [Constructable]
        public Atlantian()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Atlantian", null, 700, 90, 160);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public Atlantian(Serial serial)
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
