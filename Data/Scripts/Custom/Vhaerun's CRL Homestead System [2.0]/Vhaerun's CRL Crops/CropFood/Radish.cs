using System;
using Server.Network;

namespace Server.Items
{
    public class Radish : Food
    {
        [Constructable]
        public Radish()
            : this(1) { }

        [Constructable]
        public Radish(int amount)
            : base(amount, 0xD39)
        {
            this.Weight = 0.5;
            this.FillFactor = 1;
            this.Name = "Radish";
            this.Hue = 0x1B9;
        }

        public Radish(Serial serial)
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
