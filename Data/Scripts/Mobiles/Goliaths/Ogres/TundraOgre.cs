using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an ogre corpse")]
    public class TundraOgre : BaseCreature
    {
        public override int BreathPhysicalDamage
        {
            get { return 100; }
        }
        public override int BreathFireDamage
        {
            get { return 0; }
        }
        public override int BreathColdDamage
        {
            get { return 0; }
        }
        public override int BreathPoisonDamage
        {
            get { return 0; }
        }
        public override int BreathEnergyDamage
        {
            get { return 0; }
        }
        public override int BreathEffectHue
        {
            get { return 0; }
        }
        public override int BreathEffectSound
        {
            get { return 0x65A; }
        }
        public override int BreathEffectItemID
        {
            get { return 0x1365; }
        } // SMALL BOULDER
        public override bool HasBreath
        {
            get { return true; }
        }
        public override double BreathEffectDelay
        {
            get { return 0.1; }
        }

        public override void BreathDealDamage(Mobile target, int form)
        {
            base.BreathDealDamage(target, 7);
        }

        public override double BreathDamageScalar
        {
            get { return 0.35; }
        }

        [Constructable]
        public TundraOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tundra ogre";
            Body = 1;
            BaseSoundID = 427;
            Hue = Utility.RandomList(0xB78, 0xB33, 0xB34, 0xB35, 0xB36, 0xB37);

            SetStr(166, 195);
            SetDex(46, 65);
            SetInt(46, 70);

            SetHits(100, 117);
            SetMana(0);

            SetDamage(9, 11);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 25);

            SetSkill(SkillName.MagicResist, 55.1, 70.0);
            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.FistFighting, 70.1, 80.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 32;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Mobile killer = this.LastKiller;
            if (killer != null)
            {
                if (killer is BaseCreature)
                    killer = ((BaseCreature)killer).GetMaster();

                if (killer is PlayerMobile)
                {
                    if (GetPlayerInfo.LuckyKiller(killer.Luck) && Utility.RandomMinMax(1, 4) == 1)
                    {
                        BaseWeapon weapon = new Club();
                        weapon.MinDamage = weapon.MinDamage + 4;
                        weapon.MaxDamage = weapon.MaxDamage + 8;
                        weapon.DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                        weapon.AosElementDamages.Cold = 50;
                        weapon.Name = "ogre club";
                        weapon.Hue = 0x47E;
                        c.DropItem(weapon);
                    }
                }
            }
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }
        public override int TreasureMapLevel
        {
            get { return 1; }
        }
        public override int Meat
        {
            get { return 2; }
        }

        public TundraOgre(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
