using System;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0xc79, 0xc7a)]
    public class Cantaloupe : Food
    {
        [Constructable]
        public Cantaloupe()
            : this(1) { }

        [Constructable]
        public Cantaloupe(int amount)
            : base(amount, 0xc79)
        {
            this.Name = "cantaloupe"; // to correct UO data files
            this.Weight = 1.0;
            this.FillFactor = 2;
        }

        public Cantaloupe(Serial serial)
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
