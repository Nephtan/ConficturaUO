// created on 12/16/2003 at 10:33 PM

// extra crops for Korbas' crop system. Much easier to use and add on to.

using System;
using Server.Items;

namespace Server.Items
{
    public class CarrotPlant : BaseCrop
    {
        [Constructable]
        public CarrotPlant()
            : this(0x0C68, 1, 5) { }

        [Constructable]
        public CarrotPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0C68, 0xC69, 0xC76 };

            AllId = Id;

            NumAges = 3;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 2;
            MaxHarvest = 4;
        }

        public override Item FinalItem(int Count)
        {
            return new Carrot(Count);
        }

        public CarrotPlant(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)version);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
