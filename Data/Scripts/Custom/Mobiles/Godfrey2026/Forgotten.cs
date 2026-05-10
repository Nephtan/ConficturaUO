using Server;
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
        }

        public Forgotten(Serial serial)
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
