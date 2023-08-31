using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class UnbakedKeyLimePie : CookableFood
    {
        // public override int LabelNumber{ get{ return 1041336; } } // unbaked apple pie

        [Constructable]
        public UnbakedKeyLimePie()
            : base(0x1042, 25)
        {
            Name = "unbaked key lime pie";
            Weight = 1.0;
        }

        public UnbakedKeyLimePie(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override Food Cook()
        {
            return new KeyLimePie();
        }
    }
}
