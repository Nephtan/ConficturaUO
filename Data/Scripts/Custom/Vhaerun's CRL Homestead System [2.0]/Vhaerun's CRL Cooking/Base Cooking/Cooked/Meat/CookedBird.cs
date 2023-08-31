using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class CookedBird : Food
    {
        [Constructable]
        public CookedBird()
            : this(1) { }

        [Constructable]
        public CookedBird(int amount)
            : base(amount, 0x9B7)
        {
            this.Weight = 1.0;
            this.FillFactor = 10;
        }

        public CookedBird(Serial serial)
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
