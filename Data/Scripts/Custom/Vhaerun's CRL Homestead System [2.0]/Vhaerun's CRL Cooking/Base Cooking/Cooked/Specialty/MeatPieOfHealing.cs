using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class MeatPieOfHealing : Food
    {
        [Constructable]
        public MeatPieOfHealing()
            : base(0x1041)
        {
            Name = "a meat pie of healing";
            /*CookedMessage = "You baked a meat pie of healing";
            Difficulty = 60;*/
            HitsBonus = 10;
            ManaBonus = 0;

            FillFactor = 5;
            Weight = 1.0;
            Stackable = false;
        }

        public MeatPieOfHealing(Serial serial)
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
