using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RawLambLeg : CookableFood
    {
        [Constructable]
        public RawLambLeg()
            : this(1) { }

        [Constructable]
        public RawLambLeg(int amount)
            : base(0x1609, 10)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public RawLambLeg(Serial serial)
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
            return new LambLeg();
        }
    }
}
