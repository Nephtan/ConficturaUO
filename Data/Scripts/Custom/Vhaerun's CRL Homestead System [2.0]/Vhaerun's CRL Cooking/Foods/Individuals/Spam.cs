using System;
using Server.Network;

namespace Server.Items
{
    public class Spam : Food // debatable.. ;)  definitely not meat tho. =D
    {
        [Constructable]
        public Spam()
            : this(1) { }

        [Constructable]
        public Spam(int amount)
            : base(amount, 0x1044)
        {
            Weight = 1.0;
            FillFactor = 7;
            //	Stackable = false;  // ?
        }

        public Spam(Serial serial)
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
