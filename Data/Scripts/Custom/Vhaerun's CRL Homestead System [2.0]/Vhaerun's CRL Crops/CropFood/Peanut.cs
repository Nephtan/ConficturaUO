using System;
using Server.Network;

namespace Server.Items
{
    public class Peanut : Food
    {
        [Constructable]
        public Peanut()
            : this(1) { }

        [Constructable]
        public Peanut(int amount)
            : base(amount, 0x14FD)
        {
            this.Weight = 0.1;
            this.FillFactor = 1;
            this.Hue = 0x224;
            this.Name = "Peanut";
        }

        public Peanut(Serial serial)
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
