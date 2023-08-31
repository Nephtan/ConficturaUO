using System;
using Server.Network;

namespace Server.Items
{
    public class Beet : Food
    {
        [Constructable]
        public Beet()
            : this(1) { }

        [Constructable]
        public Beet(int amount)
            : base(amount, 0xD39)
        {
            this.Weight = 0.5;
            this.FillFactor = 1;
            this.Hue = 0x1B;
            this.Name = "Beet";
        }

        public Beet(Serial serial)
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
