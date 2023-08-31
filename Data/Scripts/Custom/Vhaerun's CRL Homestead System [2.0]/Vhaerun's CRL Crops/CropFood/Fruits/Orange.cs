using System;
using Server.Network;

namespace Server.Items
{
    public class Orange : Food
    {
        [Constructable]
        public Orange()
            : this(1) { }

        [Constructable]
        public Orange(int amount)
            : base(amount, 0x9D0)
        {
            Name = "orange";
            Hue = 48;
            this.Weight = 1.0;
            this.FillFactor = 1;
        }

        public Orange(Serial serial)
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
