// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class HayPlant : BaseCrop
    {
        [Constructable]
        public HayPlant()
            : this(0x0D33, 1, 5) { }

        [Constructable]
        public HayPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0D33, 0x1A96, 0x1A95, 0x1A94, 0x1A92 };

            AllId = Id;

            NumAges = 5;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 1;
            MaxHarvest = 1;
        }

        public override Item FinalItem(int Count)
        {
            return new Hay(Count);
        }

        public HayPlant(Serial serial)
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
