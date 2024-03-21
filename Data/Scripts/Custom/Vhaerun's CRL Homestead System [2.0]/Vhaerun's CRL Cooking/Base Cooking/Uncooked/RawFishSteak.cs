using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RawFishSteak : CookableFood
    {
        [Constructable]
        public RawFishSteak()
            : this(1) { }

        [Constructable]
        public RawFishSteak(int amount)
            : base(0x097A, 0)
        {
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }

        public RawFishSteak(Serial serial)
            : base(serial) { }

        public override Food Cook()
        {
            return new FishSteak();
        }

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
