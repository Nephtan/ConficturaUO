using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Muffins : Food
    {
        [Constructable]
        public Muffins()
            : this(1) { }

        [Constructable]
        public Muffins(int amount)
            : base(0x9ea)
        {
            Amount = amount;
            Stackable = true;
            this.Weight = 1.0;
            this.FillFactor = 2; // but you get 3 of them now
        }

        public Muffins(Serial serial)
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
