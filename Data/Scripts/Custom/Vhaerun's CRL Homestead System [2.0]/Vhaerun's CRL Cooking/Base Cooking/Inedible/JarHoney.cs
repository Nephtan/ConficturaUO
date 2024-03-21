using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class JarHoney : Item
    {
        [Constructable]
        public JarHoney()
            : this(1) { }

        [Constructable]
        public JarHoney(int amount)
            : base(0x9ec)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public JarHoney(Serial serial)
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
    }
}
