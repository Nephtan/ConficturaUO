using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RawBacon : CookableFood
    {
        [Constructable]
        public RawBacon()
            : this(1) { }

        [Constructable]
        public RawBacon(int amount)
            : base(0x979, 0)
        {
            Name = "raw slice of bacon";
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
            Hue = 336;
        }

        public RawBacon(Serial serial)
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

        public override Food Cook()
        {
            return new Bacon();
        }
    }
}
