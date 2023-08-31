// created on 12/16/2003 at 10:33 PM

// extra crops for Korbas' crop system. Much easier to use and add on to.

using System;
using Server.Items;

namespace Server.Items
{
    public class TurnipPlant : BaseCrop
    {
        [Constructable]
        public TurnipPlant()
            : this(0xC68, 1, 5) { }

        [Constructable]
        public TurnipPlant(int ItemID, int MinAgeDelay, int MaxAgeDelay)
            : base(ItemID, MinAgeDelay, MaxAgeDelay)
        {
            int[] Id = { 0x0C68, 0x0C69, 0xC61, 0xC62 };

            AllId = Id;

            NumAges = 4;
            Harvestable = true;
            DeleteWhenDone = true;
            MinHarvest = 2;
            MaxHarvest = 4;
        }

        public override Item FinalItem(int Count)
        {
            return new Turnip(Count);
        }

        public TurnipPlant(Serial serial)
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
