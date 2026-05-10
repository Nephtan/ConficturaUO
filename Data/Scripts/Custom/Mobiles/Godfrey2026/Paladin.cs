using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    public class Paladin : KeeperOfChivalry
    {
        [Constructable]
        public Paladin()
            : base()
        {
            Godfrey2026Support.ApplyProfile(this, "Paladin", null, 700, 100, 150);
            Godfrey2026Support.SetResistance(this, ResistanceType.Fire);
            Godfrey2026Support.SetResistance(this, ResistanceType.Poison);
        }

        public Paladin(Serial serial)
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
