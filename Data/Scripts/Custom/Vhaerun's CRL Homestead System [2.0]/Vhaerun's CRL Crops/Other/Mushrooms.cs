// by Alari
using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Mushrooms : Food
    {
        [Constructable]
        public Mushrooms()
            : this(Utility.RandomMinMax(3340, 3348)) { }

        [Constructable]
        public Mushrooms(int id)
            : base(1, id)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Stackable = false;
        }

        public Mushrooms(Serial serial)
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
