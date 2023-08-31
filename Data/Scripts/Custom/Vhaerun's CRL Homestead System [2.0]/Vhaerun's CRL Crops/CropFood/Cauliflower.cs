using System;
using Server.Network;

namespace Server.Items
{
    public class Cauliflower : Food
    {
        [Constructable]
        public Cauliflower()
            : this(1) { }

        [Constructable]
        public Cauliflower(int amount)
            : base(amount, 0xC7B)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Hue = 0x47E;
            this.Name = "Cauliflower";
        }

        public Cauliflower(Serial serial)
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
