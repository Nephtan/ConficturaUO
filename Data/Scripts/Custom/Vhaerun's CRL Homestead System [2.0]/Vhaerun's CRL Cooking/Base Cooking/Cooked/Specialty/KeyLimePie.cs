using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class KeyLimePie : Food
    {
        // public override int LabelNumber{ get{ return 1041343; } } // baked apple pie

        [Constructable]
        public KeyLimePie()
            : base(0x1041)
        {
            Name = "baked key lime pie";
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 5;
        }

        public KeyLimePie(Serial serial)
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
