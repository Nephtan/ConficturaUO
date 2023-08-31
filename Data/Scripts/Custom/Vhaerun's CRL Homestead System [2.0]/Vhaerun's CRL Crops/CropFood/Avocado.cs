using System;
using Server.Network;

namespace Server.Items
{
    public class Avocado : Food
    {
        [Constructable]
        public Avocado()
            : this(1) { }

        [Constructable]
        public Avocado(int amount)
            : base(amount, 0xC80)
        {
            this.Weight = 0.5;
            this.FillFactor = 1;
            this.Stackable = true;
            this.Hue = 0x483;
            this.Name = "Avocado";
        }

        public Avocado(Serial serial)
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
