using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class Dough : CookableFood
    {
        [Constructable]
        public Dough()
            : this(1) { }

        [Constructable]
        public Dough(int amount)
            : base(0x103d, 0)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public Dough(Serial serial)
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
            return new BreadLoaf();
        }
    }
}
