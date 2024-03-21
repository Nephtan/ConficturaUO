using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class BrightEggs : Eggs
    {
        [Constructable]
        public BrightEggs()
            : base()
        {
            Name = "brightly colored eggs";
            Hue = Utility.RandomList(
                0x135,
                0xcd,
                0x38,
                0x3b,
                0x42,
                0x4f,
                0x11e,
                0x60,
                0x317,
                0x10,
                0x136,
                0x1f9,
                0x1a,
                0xeb,
                0x86,
                0x2e,
                0x0497,
                0x0481
            );
        }

        public BrightEggs(Serial serial)
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
    }
}
