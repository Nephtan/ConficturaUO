using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RawBird : CookableFood, ICarvable
    {
        [Constructable]
        public RawBird()
            : this(1) { }

        [Constructable]
        public RawBird(int amount)
            : base(0x9B9, 0)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public void Carve(Mobile from, Item item)
        {
            if (!Movable)
                return;

            base.ScissorHelper(from, new RawChickenLeg(), 4);
            from.SendMessage("You cut the bird up.");
        }

        public RawBird(Serial serial)
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

        public override Food Cook()
        {
            return new CookedBird();
        }
    }
}
