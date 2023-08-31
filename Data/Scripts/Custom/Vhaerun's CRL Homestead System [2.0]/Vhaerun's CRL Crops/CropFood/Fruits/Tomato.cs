using System;
using Server.Network;

namespace Server.Items
{
    public class Tomato : Food
    {
        [Constructable]
        public Tomato()
            : this(1) { }

        [Constructable]
        public Tomato(int amount)
            : base(amount, 0x9D0)
        {
            Name = "a tomato";
            Hue = 0x8F;
            this.Weight = 1.0;
            this.FillFactor = 1;
        }

        public Tomato(Serial serial)
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
