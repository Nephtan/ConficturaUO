using System;
using Server.Network;

namespace Server.Items
{
    public class Asparagus : Food
    {
        [Constructable]
        public Asparagus()
            : this(1) { }

        [Constructable]
        public Asparagus(int amount)
            : base(amount, 0xC77)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Name = "Asparagus";
            this.Hue = 0x1D3;
        }

        public Asparagus(Serial serial)
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
