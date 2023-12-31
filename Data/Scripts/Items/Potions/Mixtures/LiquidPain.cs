using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public class LiquidPain : BaseLiquid
    {
        [Constructable]
        public LiquidPain()
            : base(PotionEffect.LiquidPain)
        {
            LiquidGlow = 0;
            Name = "liquid pain";
        }

        public LiquidPain(Serial serial)
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
