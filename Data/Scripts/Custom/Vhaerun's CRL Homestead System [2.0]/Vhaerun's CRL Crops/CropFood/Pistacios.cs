using System;
using Server.Network;

namespace Server.Items
{
    public class Pistacios : Food
    {
        [Constructable]
        public Pistacios()
            : this(1) { }

        [Constructable]
        public Pistacios(int amount)
            : base(amount, 0x1AA2)
        {
            this.Weight = 0.1;
            this.FillFactor = 1;
            this.Hue = 0x47E;
            this.Name = "Pistacios";
        }

        public Pistacios(Serial serial)
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
