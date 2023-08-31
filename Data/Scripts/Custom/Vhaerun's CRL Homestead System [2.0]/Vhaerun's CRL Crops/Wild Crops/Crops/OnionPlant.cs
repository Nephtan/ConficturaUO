// created on 12/16/2003 at 10:33 PM

// extra crops for Korbas' crop system. Much easier to use and add on to.

using System;
using Server.Items;

namespace Server.Items
{
    public class OnionPlant : BaseCrop
    {
        [Constructable]
        public OnionPlant()
            : this(0xC68, 1, 5) { }

        [Constructable]
        public OnionPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0xC68, 0xC69, 0xC6F };

            AllId = Id;

            NumAges = 3;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 2;
            MaxHarvest = 4;
        }

        public override Item FinalItem(int Count)
        {
            return new Onion(Count);
        }

        public OnionPlant(Serial serial)
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
