// by Korbas
using System;
using Server.Items;

namespace Server.Items
{
    public class FlaxPlant : BaseCrop
    {
        [Constructable]
        public FlaxPlant()
            : this(0x0C68, 1, 5) { }

        [Constructable]
        public FlaxPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0C68, 0x1A99, 0x1A9A, 0x1A9B };

            AllId = Id;

            NumAges = 4;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 1;
            MaxHarvest = 3;
        }

        public override Item FinalItem(int Count)
        {
            return new Flax(Count);
        }

        public FlaxPlant(Serial serial)
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
