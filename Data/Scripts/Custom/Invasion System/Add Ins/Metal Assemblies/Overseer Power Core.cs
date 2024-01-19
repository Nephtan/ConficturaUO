using System;
using Server;

namespace Server.Items
{
    public class OverseerPowerCore : Item
    {
        [Constructable]
        public OverseerPowerCore()
            : base(0x1f1c)
        {
            Name = "overseer power core";
            Weight = 5.0;
            Hue = 0x8d;
        }

        public OverseerPowerCore(Serial serial)
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
