using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class UnbakedChickenPotPie : CookableFood
    {
        [Constructable]
        public UnbakedChickenPotPie()
            : base(0x1042, 25)
        {
            Name = "unbaked chicken pot pie";
            Weight = 1.0;
        }

        public UnbakedChickenPotPie(Serial serial)
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
            return new ChickenPotPie();
        }
    }
}
