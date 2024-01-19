using System;
using Server;

namespace Server.Items
{
    public class DaemonPowerCore : Item
    {
        [Constructable]
        public DaemonPowerCore()
            : base(0x1f19)
        {
            Name = "daemon power core";
            Weight = 5.0;
            Hue = 0x5d;
        }

        public DaemonPowerCore(Serial serial)
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
