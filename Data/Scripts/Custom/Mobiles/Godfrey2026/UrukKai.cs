using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class UrukKai : OrxWarrior
    {
        [Constructable]
        public UrukKai()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Uruk-kai", null, 500, 80, 100);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public UrukKai(Serial serial)
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
