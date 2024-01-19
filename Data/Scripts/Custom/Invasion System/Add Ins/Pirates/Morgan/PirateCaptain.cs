//Pirate Captain Invader....Made by Grae using RunUO RCO 1.0;
using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a pirate captain invader corpse")]
    public class piratecaptain2 : BaseCreature
    {
        [Constructable]
        public piratecaptain2()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4) // AI_ melee or mage.
        {
            SpeechHue = Utility.RandomDyedHue();
            Title = "the Pirate Captain Invader";
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNeutralHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (Utility.RandomBool() && !this.Female)
            {
                Item beard = new Item(
                    Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D)
                );

                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;

                AddItem(beard);
            }

            SetStr(100, 200);
            SetDex(81, 95);
            SetInt(61, 75);

            SetHits(88, 108);

            SetDamage(10, 23);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 20);
            SetResistance(ResistanceType.Cold, 20);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 20);

            SetSkill(SkillName.Fencing, 65.0, 97.5);
            SetSkill(SkillName.Bludgeoning, 65.0, 87.5);
            SetSkill(SkillName.MagicResist, 25.0, 55.5);
            SetSkill(SkillName.Swords, 65.0, 97.5);
            SetSkill(SkillName.Tactics, 65.0, 97.5);
            SetSkill(SkillName.FistFighting, 15.0, 37.5);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 50;

            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt());
            AddItem(new TricorneHat(Utility.RandomNeutralHue()));
            AddItem(new BodySash(Utility.RandomNeutralHue()));
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            switch (Utility.Random(6))
            {
                case 0:
                    AddItem(new Longsword());
                    break;
                case 1:
                    AddItem(new Cutlass());
                    break;
                case 2:
                    AddItem(new Broadsword());
                    break;
                case 3:
                    AddItem(new Axe());
                    break;
                case 4:
                    AddItem(new Club());
                    break;
                case 5:
                    AddItem(new Dagger());
                    break;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override bool BardImmune
        {
            get { return true; }
        }

        public override bool AlwaysMurderer
        {
            get { return true; }
        }

        public piratecaptain2(Serial serial)
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
