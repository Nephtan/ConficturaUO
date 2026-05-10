using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Crusader : Adventurers
    {
        [Constructable]
        public Crusader()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Crusader", null, 900, 120, 190);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public Crusader(Serial serial)
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
