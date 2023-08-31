// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class GarlicPlant : BaseCrop
    {
        [Constructable]
        public GarlicPlant()
            : this(0x0C68, 1, 5) { }

        [Constructable]
        public GarlicPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0C68, 0x0C69, 0x18E1 };

            AllId = Id;

            NumAges = 3;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 1;
            MaxHarvest = 3;
        }

        public override Item FinalItem(int Count)
        {
            return new Garlic(Count);
        }

        public GarlicPlant(Serial serial)
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
