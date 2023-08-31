using System;
using Server.Network;

namespace Server.Items
{
    public class EdibleSun : Food
    {
        [Constructable]
        public EdibleSun()
            : this(1) { }

        [Constructable]
        public EdibleSun(int amount)
            : base(amount, 0xF27)
        {
            this.Weight = 0.1;
            this.Stackable = true;
            this.FillFactor = 1;
            this.Hue = 0xF7E;
            this.Name = "Sunflower Seeds";
        }

        public EdibleSun(Serial serial)
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
