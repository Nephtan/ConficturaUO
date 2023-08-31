// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class WheatPlant : BaseCrop
    {
        [Constructable]
        public WheatPlant()
            : this(0x1EBF, 1, 5) { }

        [Constructable]
        public WheatPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x1EBF, 0x1EBE, 0x0DAF, 0x0DAE, 0x0C55, 0x0C56, 0x0C57, 0x0C58 };

            AllId = Id;

            NumAges = 8;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 1;
            MaxHarvest = 1;
        }

        public override Item FinalItem(int Count)
        {
            return new Wheat(Count);
        }

        public WheatPlant(Serial serial)
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
