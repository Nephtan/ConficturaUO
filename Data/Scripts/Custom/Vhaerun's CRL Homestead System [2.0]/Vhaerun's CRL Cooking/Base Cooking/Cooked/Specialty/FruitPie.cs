using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class FruitPie : Food
    {
        [Constructable]
        public FruitPie()
            : this("", 0) { }

        [Constructable]
        public FruitPie(string Desc)
            : this(Desc, 0) { }

        [Constructable]
        public FruitPie(int Color)
            : this("", Color) { }

        [Constructable]
        public FruitPie(string Desc, int Color)
            : base(0x1041)
        {
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 5;
            if (Desc != "")
                Name = Desc + " pie";
            else
                Name = "fruit pie";

            this.Hue = Color;
        }

        public FruitPie(Serial serial)
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
