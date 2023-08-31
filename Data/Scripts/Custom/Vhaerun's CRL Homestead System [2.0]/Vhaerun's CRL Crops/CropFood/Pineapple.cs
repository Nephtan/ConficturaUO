using System;
using Server.Network;

namespace Server.Items
{
    public class Pineapple : Food
    {
        [Constructable]
        public Pineapple()
            : this(1) { }

        [Constructable]
        public Pineapple(int amount)
            : base(0x1726)
        {
            this.Name = "Pineapple";
            this.Hue = 0x7EB;
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Stackable = true;
            this.Amount = amount;
        }

        public Pineapple(Serial serial)
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
