using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class UncookedFrenchBread : CookableFood
    {
        [Constructable]
        public UncookedFrenchBread()
            : base(0x98C, 0)
        {
            Weight = 1.0;
            Hue = 51;
            Name = "uncooked french bread";
        }

        public UncookedFrenchBread(Serial serial)
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
            return new FrenchBread();
        }
    }
}
