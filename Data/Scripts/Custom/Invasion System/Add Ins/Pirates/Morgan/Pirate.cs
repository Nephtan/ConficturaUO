//Pirate....Made by Grae using RunUO RCO 1.0;
using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a corpse of a Pirate")]
    public class piratea : BaseCreature
    {
        [Constructable]
        public piratea()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4) // AI_ melee or mage.
        {
            SpeechHue = Utility.RandomDyedHue();
            Title = "the Pirate";
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

            SetStr(86, 100);
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

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 25;

            AddItem(new Shoes(Utility.RandomNeutralHue()));
            AddItem(new Shirt());
            AddItem(new SkullCap(Utility.RandomNeutralHue()));
            AddItem(new BodySash(Utility.RandomNeutralHue()));
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            switch (Utility.Random(5))
            {
                case 0:
                    AddItem(new Longsword());
                    break;
                case 1:
                    AddItem(new Cutlass());
                    break;
                case 2:
                    AddItem(new Axe());
                    break;
                case 3:
                    AddItem(new Club());
                    break;
                case 4:
                    AddItem(new Dagger());
                    break;
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNondyedHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
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

        public piratea(Serial serial)
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
