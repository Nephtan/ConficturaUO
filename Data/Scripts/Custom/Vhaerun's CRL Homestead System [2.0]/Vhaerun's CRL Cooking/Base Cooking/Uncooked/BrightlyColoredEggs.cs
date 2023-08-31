using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    // ********** BrightlyColoredEggs **********
    public class BrightlyColoredEggs : Eggs
    {
        [Constructable]
        public BrightlyColoredEggs()
            : base()
        {
            Hue = 3 + (Utility.Random(20) * 5);
        }

        public BrightlyColoredEggs(Serial serial)
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
