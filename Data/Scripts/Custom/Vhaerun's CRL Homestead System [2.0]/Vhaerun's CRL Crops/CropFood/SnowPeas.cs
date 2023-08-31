using System;
using Server.Network;

namespace Server.Items
{
    public class SnowPeas : Food
    {
        [Constructable]
        public SnowPeas()
            : this(1) { }

        [Constructable]
        public SnowPeas(int amount)
            : base(amount, 0xF2F)
        {
            this.Weight = 0.1;
            this.FillFactor = 1;
            this.Hue = 0x29A;
            this.Name = "Snow Peas";
        }

        public SnowPeas(Serial serial)
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
