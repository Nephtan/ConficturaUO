using System;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0xc6a, 0xc6b)]
    public class Pumpkin : Food
    {
        [Constructable]
        public Pumpkin()
            : this(1) { }

        [Constructable]
        public Pumpkin(int amount)
            : base(amount, 0xc6a)
        {
            this.Weight = 5.0;
            this.FillFactor = 4;
        }

        public Pumpkin(Serial serial)
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
