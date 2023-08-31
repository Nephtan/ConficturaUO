using System;
using Server.Network;

namespace Server.Items
{
    public class TeaLeaves : Item
    {
        [Constructable]
        public TeaLeaves()
            : base(0x1AA2)
        {
            Name = "Tea Leaves";
            Weight = 0.1;
            Hue = 0x44;
            Stackable = true;
        }

        public TeaLeaves(Serial serial)
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
