using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class ManOWar : Jellyfish
    {
        [Constructable]
        public ManOWar()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Man o' War", null, 50, 180, 200);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
            Godfrey2026Support.SetResistance(this, ResistanceType.Energy);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public ManOWar(Serial serial)
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
