using System;
using Server.Network;

namespace Server.Items
{
    public class GreenSquash : Food
    {
        [Constructable]
        public GreenSquash()
            : this(1) { }

        [Constructable]
        public GreenSquash(int amount)
            : base(amount, 0xC66)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Name = "Green Squash";
            this.Hue = 0x1D8;
        }

        public GreenSquash(Serial serial)
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
