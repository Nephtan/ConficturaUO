using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RawChickenLeg : CookableFood
    {
        [Constructable]
        public RawChickenLeg()
            : this(1) { }

        [Constructable]
        public RawChickenLeg(int amount)
            : base(0x1607, 0)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public RawChickenLeg(Serial serial)
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
            return new ChickenLeg();
        }
    }
}
