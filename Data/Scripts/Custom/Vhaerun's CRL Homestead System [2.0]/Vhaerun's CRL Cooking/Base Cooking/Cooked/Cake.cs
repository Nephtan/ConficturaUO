using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Cake : Food
    {
        public string Desc;

        [Constructable]
        public Cake()
            : this(null, 0) { }

        [Constructable]
        public Cake(string desc)
            : this(desc, 0) { }

        [Constructable]
        public Cake(int Color)
            : this(null, Color) { }

        [Constructable]
        public Cake(string desc, int Color)
            : base(0x9E9)
        {
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 10;

            if (desc != "" && desc != null)
            {
                this.Desc = desc;
                this.Name = "a " + this.Desc + " cake";
            }

            if (Color != 0)
                this.Hue = Color;
        }

        public Cake(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((string)Desc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                {
                    Desc = reader.ReadString();
                    break;
                }
            }
        }
    }
}
