//================================================//
// Created by dracana				  //
// Desc: Empty bottle for crafting wine.          //
//================================================//
using System;

namespace Server.Items
{
    public class EmptyWineBottle : Item
    {
        [Constructable]
        public EmptyWineBottle()
            : this(1) { }

        [Constructable]
        public EmptyWineBottle(int amount)
            : base(0x99B)
        {
            Stackable = true;
            Weight = 1.0;
            Name = "Empty Wine Bottle";
            Amount = amount;
        }

        public EmptyWineBottle(Serial serial)
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
