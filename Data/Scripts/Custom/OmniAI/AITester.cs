// Created by Peoharen
using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a corpse")]
    public class AITester : BaseCreature
    {
        [Constructable]
        public AITester()
            : this(12) { }

        [Constructable]
        public AITester(int i)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dark Knight";
            Body = 400;
            Hue = 1175;

            SetStr(1000);
            SetDex(150);
            SetInt(1500);

            SetDamage(25, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 50);

            if (i == 0)
            {
                SetSkill(SkillName.Psychology, 120.0);
                SetSkill(SkillName.Magery, 120.0);
            }
            else if (i == 1)
            {
                SetSkill(SkillName.Necromancy, 120.0);
                SetSkill(SkillName.Spiritualism, 120.0);
            }
            else if (i == 2)
            {
                SetSkill(SkillName.Bushido, 120.0);
                SetSkill(SkillName.Parry, 120.0);
            }
            else if (i == 3)
            {
                SetSkill(SkillName.Ninjitsu, 120.0);
                SetSkill(SkillName.Hiding, 120.0);
                SetSkill(SkillName.Stealth, 120.0);
            }
            //else if ( i == 4 )
            //{
            //	SetSkill( SkillName.Spellweaving, 120.0 );
            //}

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Swords, 100.0);
            SetSkill(SkillName.Tactics, 100.0);

            Fame = 32000;
            Karma = -32000;

            VirtualArmor = 80;

            int hue = 1175;
            GiveItem(this, hue, new DragonHelm());
            GiveItem(this, hue, new PlateGorget());
            GiveItem(this, hue, new DragonChest());
            GiveItem(this, hue, new DragonLegs());
            GiveItem(this, hue, new DragonArms());
            GiveItem(this, hue, new DragonGloves());

            Longsword sword = new Longsword();
            sword.ItemID = 9934;
            GiveItem(this, hue, sword);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 35);
            AddLoot(LootPack.Gems, 15);
        }

        public static void GiveItem(Mobile to, int hue, Item item)
        {
            if (to == null && item == null)
                return;

            if (hue != 0)
                item.Hue = hue;

            item.Movable = false;
            to.EquipItem(item);
            return;
        }

        protected override BaseAI ForcedAI
        {
            get { return new OmniAI(this); }
        }
        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool ShowFameTitle
        {
            get { return false; }
        }

        public AITester(Serial serial)
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
