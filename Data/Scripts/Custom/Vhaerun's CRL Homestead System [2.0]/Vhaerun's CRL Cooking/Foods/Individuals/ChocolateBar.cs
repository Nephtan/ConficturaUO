using System;
using Server.Network;

namespace Server.Items
{
    public class ChocolateBar : Food
    {
        [Constructable]
        public ChocolateBar()
            : this(1) { }

        [Constructable]
        public ChocolateBar(int amount)
            : base(amount, 0xF8A)
        {
            this.Name = "Chocolate Bar";
            this.Stackable = true;
            this.Weight = 1.0;
            this.FillFactor = 1;
            this.Hue = 0x41B;
        }

        public ChocolateBar(Serial serial)
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
