// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class CottonPlantWest : BaseCrop
    {
        [Constructable]
        public CottonPlantWest()
            : this(0x0C68, 1, 5) { }

        [Constructable]
        public CottonPlantWest(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0C68, 0x0C69, 0x0C52, 0x0C54, 0x0C50 };

            AllId = Id;

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

        public CottonPlantWest(Serial serial)
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
