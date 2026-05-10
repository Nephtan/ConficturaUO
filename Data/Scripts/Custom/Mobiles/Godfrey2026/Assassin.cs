using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Assassin : Rogue
    {
        [Constructable]
        public Assassin()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Assassin", null, 300, 180, 200);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public Assassin(Serial serial)
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
