using System;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0xc7b, 0xc7c)]
    public class Cabbage : Food
    {
        [Constructable]
        public Cabbage()
            : this(1) { }

        [Constructable]
        public Cabbage(int amount)
            : base(amount, 0xc7b)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Name = "cabbage";
        }

        public Cabbage(Serial serial)
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
