using System;

namespace Server.Items
{
    public class EmptyAleBottle : Item
    {
        [Constructable]
        public EmptyAleBottle()
            : this(1) { }

        [Constructable]
        public EmptyAleBottle(int amount)
            : base(0x99B)
        {
            Stackable = true;
            Weight = 1.0;
            Name = "Empty Ale Bottle";
            Amount = amount;
        }

        public EmptyAleBottle(Serial serial)
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
