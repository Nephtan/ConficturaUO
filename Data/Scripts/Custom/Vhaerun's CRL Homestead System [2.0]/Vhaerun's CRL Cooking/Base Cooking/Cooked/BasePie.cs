using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class BasePie : Food
    {
        //public override int LabelNumber{ get{ return 1041343; } } // baked apple pie

        [Constructable]
        public BasePie()
            : this(null, 0) { }

        [Constructable]
        public BasePie(string Desc)
            : this(Desc, 0) { }

        [Constructable]
        public BasePie(int Color)
            : this(null, Color) { }

        [Constructable]
        public BasePie(string Desc, int Color)
            : base(0x1041)
        {
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 5;
            if (Desc != "" && Desc != null)
                this.Name = "a " + Desc + " pie";
            else
                this.Name = "a pie";

            this.Hue = Color;
        }

        public BasePie(Serial serial)
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
