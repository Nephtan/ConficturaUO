using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class DeepOne : Dagon
    {
        [Constructable]
        public DeepOne()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Deep One", null, 900, 100, 200);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.AddMortalStrike(this);
        }

        public DeepOne(Serial serial)
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
