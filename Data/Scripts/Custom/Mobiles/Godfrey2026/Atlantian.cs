using Server;
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
        }

        public Atlantian(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
