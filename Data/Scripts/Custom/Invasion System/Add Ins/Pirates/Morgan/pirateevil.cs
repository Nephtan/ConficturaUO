using System;
using System.Collections;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class EvilPirate : BaseCreature
    {
        [Constructable]
        public EvilPirate()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.17, 0.2)
        {
            SpeechHue = Utility.RandomDyedHue();
            Title = "the pirate";
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                AddItem(new Bandana(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                AddItem(new SkullCap(Utility.RandomNeutralHue()));
            }

            SetStr(56, 80);
            SetDex(71, 90);
            SetInt(21, 35);

            SetDamage(10, 20);

            SetSkill(SkillName.Fencing, 66.0, 90.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Swords, 65.0, 87.5);
            SetSkill(SkillName.Tactics, 55.0, 77.5);
            SetSkill(SkillName.FistFighting, 55.0, 90.5);
            SetSkill(SkillName.Parry, 55.0, 80.5);

            Fame = 1000;
            Karma = -1000;

            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new Boots(Utility.RandomNeutralHue()));
                    break;
                case 1:
                    AddItem(new Shoes(Utility.RandomNeutralHue()));
                    break;
                case 2:
                    break;
            }

            switch (Utility.Random(2))
            {
                case 0:
                    AddItem(new FancyShirt(Utility.RandomNeutralHue()));
                    break;
                case 1:
                    AddItem(new Shirt(Utility.RandomNeutralHue()));
                    break;
            }
            switch (Utility.Random(5))
            {
                case 0:
                    AddItem(new Longsword());
                    break;
                case 1:
                    AddItem(new Cutlass());
                    break;
                case 2:
                    AddItem(new Kryss());
                    break;
                case 3:
                    AddItem(new Scimitar());
                    break;
                case 4:
                    AddItem(new Dagger());
                    break;
            }
            switch (Utility.Random(8))
            {
                case 0:
                    AddItem(new BeverageBottle(BeverageType.Ale));
                    break;
                case 1:
                    AddItem(new Lime());
                    break;
                case 2:
                    AddItem(new GoldRing());
                    break;
                case 3:
                    AddItem(new SeaChart());
                    break;
                case 4:
                    AddItem(new BeverageBottle(BeverageType.Wine));
                    break;
                case 5:
                    AddItem(new BeverageBottle(BeverageType.Liquor));
                    break;
                case 6:
                    AddItem(new Sextant());
                    break;
                case 7:
                    AddItem(new Lemon());
                    break;
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);
            //PackItem( new Token( 50, 125 ) );
            PackGold(100, 250);
        }

        public override bool AlwaysMurderer
        {
            get { return true; }
        }

        public EvilPirate(Serial serial)
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
