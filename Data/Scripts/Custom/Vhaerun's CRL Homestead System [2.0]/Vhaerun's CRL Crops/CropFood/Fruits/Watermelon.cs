using System;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0xc5c, 0xc5d)]
    public class Watermelon : Food
    {
        [Constructable]
        public Watermelon()
            : this(1) { }

        [Constructable]
        public Watermelon(int amount)
            : base(amount, 0xc5c)
        {
            this.Weight = 2.0;
            this.FillFactor = 2;
        }

        public Watermelon(Serial serial)
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
