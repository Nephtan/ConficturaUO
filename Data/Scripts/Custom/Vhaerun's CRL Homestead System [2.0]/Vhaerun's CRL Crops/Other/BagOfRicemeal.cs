using System;
using Server.Network;

namespace Server.Items
{
    public class BagOfRicemeal : Item
    {
        [Constructable]
        public BagOfRicemeal()
            : base(0x1039)
        {
            Weight = 5.0;
            Stackable = true;
            Hue = 0x303;
            Name = "Bag of Ricemeal";
        }

        public BagOfRicemeal(Serial serial)
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
