using System;

namespace Server.Items
{
    public class EmptyJug : Item
    {
        [Constructable]
        public EmptyJug()
            : this(1) { }

        [Constructable]
        public EmptyJug(int amount)
            : base(0x9C8)
        {
            Stackable = true;
            Weight = 1.0;
            Name = "Empty Jug";
            Amount = amount;
        }

        public EmptyJug(Serial serial)
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
