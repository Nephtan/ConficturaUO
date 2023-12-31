using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class Artifact_DeathsMask : GiftBoneHelm, IIslesDreadDyable
    {
        public override int InitMinHits
        {
            get { return 80; }
        }
        public override int InitMaxHits
        {
            get { return 160; }
        }

        public override int BaseColdResistance
        {
            get { return 11; }
        }
        public override int BaseEnergyResistance
        {
            get { return 3; }
        }
        public override int BasePhysicalResistance
        {
            get { return 15; }
        }
        public override int BasePoisonResistance
        {
            get { return 12; }
        }
        public override int BaseFireResistance
        {
            get { return 12; }
        }

        [Constructable]
        public Artifact_DeathsMask()
        {
            Name = "Mask of Death";
            Hue = 2518;
            ArmorAttributes.MageArmor = 1;
            Attributes.BonusInt = 5;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.NightSight = 1;
            Attributes.SpellDamage = 15;
            Server.Misc.Arty.ArtySetup(this, 11, "");
        }

        public Artifact_DeathsMask(Serial serial)
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
