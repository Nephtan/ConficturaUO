using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    // ********** EasterEggs **********
    public class EasterEggs : Eggs
    {
        public override int LabelNumber
        {
            get { return 1016105; }
        } // Easter Eggs

        [Constructable]
        public EasterEggs()
            : base()
        {
            Hue = 3 + (Utility.Random(20) * 5);
        }

        public EasterEggs(Serial serial)
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
