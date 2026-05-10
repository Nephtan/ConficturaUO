using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class GoblinRanger : GoblinArcher
    {
        [Constructable]
        public GoblinRanger()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Goblin Ranger", null, 20, 50, 100);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.SetResistance(this, ResistanceType.Cold);
        }

        public GoblinRanger(Serial serial)
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
