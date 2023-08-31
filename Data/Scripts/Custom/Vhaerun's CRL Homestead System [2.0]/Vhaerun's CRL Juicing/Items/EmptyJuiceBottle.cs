using System;

namespace Server.Items
{
    public class EmptyJuiceBottle : Item
    {
        [Constructable]
        public EmptyJuiceBottle()
            : this(1) { }

        [Constructable]
        public EmptyJuiceBottle(int amount)
            : base(0x99B)
        {
            Stackable = true;
            Weight = 1.0;
            Name = "Empty Juice Bottle";
            Amount = amount;
        }

        public EmptyJuiceBottle(Serial serial)
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
