using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class UnbakedFruitPie : CookableFood
    {
        public override int LabelNumber
        {
            get { return 1041334; }
        } // unbaked fruit pie

        [Constructable]
        public UnbakedFruitPie()
            : this("", 0) { }

        [Constructable]
        public UnbakedFruitPie(string Desc)
            : this(Desc, 0) { }

        [Constructable]
        public UnbakedFruitPie(int Color)
            : this("", Color) { }

        [Constructable]
        public UnbakedFruitPie(string Desc, int Color)
            : base(0x1042, 25)
        {
            Weight = 1.0;

            if (Desc != "" && Desc != null)
                if (Desc == "peach")
                    Name = "unbaked peach cobbler";
                else
                    Name = "unbaked " + Desc + " fruit pie";
            else
                Name = "unbaked fruit pie";

            this.Hue = Color;
        }

        public UnbakedFruitPie(Serial serial)
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

        public override Food Cook()
        {
            return new FruitPie();
        }
    }
}
