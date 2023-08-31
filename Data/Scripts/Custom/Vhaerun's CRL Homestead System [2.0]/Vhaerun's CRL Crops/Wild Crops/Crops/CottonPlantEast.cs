// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class CottonPlantEast : BaseCrop
    {
        // This item ID is the item that will appear first for AgeDelay minutes.
        [Constructable]
        public CottonPlantEast()
            : this(0x0C68, 1, 5) { }

        [Constructable]
        public CottonPlantEast(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            // The next item ID's are the first 3 here.
            int[] Id = { 0x0C68, 0x0C69, 0x0C51, 0x0C53, 0x0C4F };

            AllId = Id;

            // Number of animations listed above
            NumAges = 5;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 1;
            MaxHarvest = 1;
        }

        public override Item FinalItem(int Count)
        {
            return new Cotton(Count);
        }

        public CottonPlantEast(Serial serial)
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
} // EOF
