using System;
using System.Collections;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("A Pirate's Corpse")]
    public class PirateCrewInvader : BaseCreature
    {
        [Constructable]
        public PirateCrewInvader()
            : base(AIType.AI_Archer, FightMode.Closest, 15, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            Title = "[Crew]";

            AddItem(new ThighBoots());

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNondyedHue();
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

            SetStr(195, 200);
            SetDex(181, 195);
            SetInt(61, 75);
            SetHits(288, 308);

            SetDamage(20, 40);

            SetSkill(SkillName.Fencing, 86.0, 97.5);
            SetSkill(SkillName.Bludgeoning, 85.0, 87.5);
            SetSkill(SkillName.MagicResist, 55.0, 67.5);
            SetSkill(SkillName.Swords, 85.0, 87.5);
            SetSkill(SkillName.Tactics, 85.0, 87.5);
            SetSkill(SkillName.FistFighting, 35.0, 37.5);
            SetSkill(SkillName.Marksmanship, 85.0, 87.5);

            CantWalk = false;

            Fame = 2000;
            Karma = -2000;
            VirtualArmor = 66;

            switch (Utility.Random(1))
            {
                case 0:
                    AddItem(new LongPants(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new ShortPants(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new FancyShirt(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new Shirt(Utility.RandomRedHue()));
                    break;
                case 2:
                    AddItem(new Doublet(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new Bandana(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new SkullCap(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(5))
            {
                case 0:
                    AddItem(new Bow());
                    break;
                case 1:
                    AddItem(new CompositeBow());
                    break;
                case 2:
                    AddItem(new Crossbow());
                    break;
                case 3:
                    AddItem(new RepeatingCrossbow());
                    break;
                case 4:
                    AddItem(new HeavyCrossbow());
                    break;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override bool IsScaredOfScaryThings
        {
            get { return false; }
        }
        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool CanRummageCorpses
        {
            get { return true; }
        }
        public override bool PlayerRangeSensitive
        {
            get { return false; }
        }

        public PirateCrewInvader(Serial serial)
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
