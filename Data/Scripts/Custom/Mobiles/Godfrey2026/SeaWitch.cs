using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class SeaWitch : WaterNaga
    {
        [Constructable]
        public SeaWitch()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Sea Witch", null, 500, 100, 190);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public SeaWitch(Serial serial)
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
