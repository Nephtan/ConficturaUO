using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Unspeakable : SeaGhost
    {
        [Constructable]
        public Unspeakable()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Unspeakable", null, 500, 150, 200);
            Godfrey2026Support.SetResistance(this, ResistanceType.Physical);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
        }

        public Unspeakable(Serial serial)
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
