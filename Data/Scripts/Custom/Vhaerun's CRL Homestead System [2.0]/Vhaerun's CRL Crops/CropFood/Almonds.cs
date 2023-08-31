using System;
using Server.Network;

namespace Server.Items
{
    public class Almonds : Food
    {
        [Constructable]
        public Almonds()
            : this(1) { }

        [Constructable]
        public Almonds(int amount)
            : base(amount, 0x1AA2)
        {
            this.Weight = 0.1;
            this.FillFactor = 1;
            this.Hue = 0x482;
            this.Name = "Almonds";
        }

        public Almonds(Serial serial)
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
