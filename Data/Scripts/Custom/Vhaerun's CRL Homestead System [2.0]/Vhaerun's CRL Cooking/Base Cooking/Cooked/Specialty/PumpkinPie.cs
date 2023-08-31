using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class PumpkinPie : Food
    {
        // public override int LabelNumber{ get{ return 1041343; } } // baked apple pie

        [Constructable]
        public PumpkinPie()
            : base(0x1041)
        {
            Name = "baked pumpkin pie";
            Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 5;
        }

        public PumpkinPie(Serial serial)
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
