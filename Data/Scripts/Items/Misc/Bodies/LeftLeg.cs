using System;
using Server;

namespace Server.Items
{
    public class LeftLeg : Item
    {
        [Constructable]
        public LeftLeg()
            : base(0x1DA3)
        {
            Weight = 1.0;
        }

        public LeftLeg(Serial serial)
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
