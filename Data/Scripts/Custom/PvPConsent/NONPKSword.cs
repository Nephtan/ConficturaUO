using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0xF61, 0xF60)]
    public class NONPKSword : BaseSword
    {
        private int BoundToSoul = 0; // Start binding value as zero.

        public override WeaponAbility PrimaryAbility
        {
            get { return WeaponAbility.ArmorIgnore; }
        }
        public override WeaponAbility SecondaryAbility
        {
            get { return WeaponAbility.ConcussionBlow; }
        }

        public override int AosStrengthReq
        {
            get { return 35; }
        }
        public override int AosMinDamage
        {
            get { return 15; }
        }
        public override int AosMaxDamage
        {
            get { return 16; }
        }
        public override int AosSpeed
        {
            get { return 30; }
        }
        public override float MlSpeed
        {
            get { return 3.50f; }
        }

        public override int OldStrengthReq
        {
            get { return 25; }
        }
        public override int OldMinDamage
        {
            get { return 5; }
        }
        public override int OldMaxDamage
        {
            get { return 33; }
        }
        public override int OldSpeed
        {
            get { return 35; }
        }

        public override int DefHitSound
        {
            get { return 0x237; }
        }
        public override int DefMissSound
        {
            get { return 0x23A; }
        }

        public override int InitMinHits
        {
            get { return 31; }
        }
        public override int InitMaxHits
        {
            get { return 110; }
        }

        [Constructable]
        public NONPKSword()
            : base(0xF61)
        {
            Weight = 7.0;

            Name = "NONPK sword";
            Slayer = SlayerName.Silver;
            DamageLevel = WeaponDamageLevel.Force;
            AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
            DurabilityLevel = WeaponDurabilityLevel.Massive;
        }

        public override bool OnEquip(Mobile from)
        {
            PlayerMobile nonpk = from as PlayerMobile;

            if (nonpk.NONPK == NONPK.NONPK)
            {
                nonpk.Emote("* " + from.Name + " weilds the sword of power *");

                base.OnEquip(nonpk);

                return true;
            }
            else
            {
                nonpk.SendMessage(33, from.Name + " is unable to weild this weapon");

                return false; //Disallow any non NONPK charactor from equiping the sword.
            }
        }

        public NONPKSword(Serial serial)
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
