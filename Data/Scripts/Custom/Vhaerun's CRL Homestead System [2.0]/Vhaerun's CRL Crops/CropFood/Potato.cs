using System;
using Server.Network;

namespace Server.Items
{
    public class Potato : Food
    {
        [Constructable]
        public Potato()
            : this(1) { }

        [Constructable]
        public Potato(int amount)
            : base(amount, 0xC5D)
        {
            this.Weight = 1.0;
            this.FillFactor = 3;
            this.Hue = 0x6C0;
            this.Name = "Potato";
        }

        public Potato(Serial serial)
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
