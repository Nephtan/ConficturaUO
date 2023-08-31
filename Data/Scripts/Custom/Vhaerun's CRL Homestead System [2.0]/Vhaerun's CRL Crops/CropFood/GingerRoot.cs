using System;
using Server.Network;

namespace Server.Items
{
    public class GingerRoot : Food
    {
        [Constructable]
        public GingerRoot()
            : this(1) { }

        [Constructable]
        public GingerRoot(int amount)
            : base(amount, 0xF85)
        {
            this.Weight = 1.0;
            this.FillFactor = 1;
            this.Hue = 0x413;
            this.Name = "Ginger Root";
        }

        public GingerRoot(Serial serial)
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
