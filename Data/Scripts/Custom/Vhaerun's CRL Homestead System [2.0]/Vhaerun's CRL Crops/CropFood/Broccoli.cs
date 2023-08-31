using System;
using Server.Network;

namespace Server.Items
{
    public class Broccoli : Food
    {
        [Constructable]
        public Broccoli()
            : this(1) { }

        [Constructable]
        public Broccoli(int amount)
            : base(amount, 0xC78)
        {
            this.Weight = 0.3;
            this.FillFactor = 1;
            this.Hue = 0x48F;
            this.Name = "Broccoli";
        }

        public Broccoli(Serial serial)
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
