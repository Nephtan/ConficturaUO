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
    public class PoisonPotion : BasePoisonPotion
    {
        public override Poison Poison
        {
            get { return Poison.Regular; }
        }

        public override double MinPoisoningSkill
        {
            get { return 20.0; }
        }
        public override double MaxPoisoningSkill
        {
            get { return 60.0; }
        }

        [Constructable]
        public PoisonPotion()
            : base(PotionEffect.Poison)
        {
            Name = "poison potion";
        }

        public PoisonPotion(Serial serial)
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
