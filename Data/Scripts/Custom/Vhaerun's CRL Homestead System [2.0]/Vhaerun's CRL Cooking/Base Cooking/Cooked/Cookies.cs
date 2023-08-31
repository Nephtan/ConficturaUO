using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Cookies : Food
    {
        [Constructable]
        public Cookies()
            : this(0) { }

        [Constructable]
        public Cookies(int Color)
            : base(0x160b)
        {
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 4;
            this.Hue = Color;
        }

        public Cookies(Serial serial)
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
