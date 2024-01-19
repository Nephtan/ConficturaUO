using System;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a pirate corpse")]
    public class PirateLeader : BaseSubChampion
    {
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Pain; }
        }

        [Constructable]
        public PirateLeader()
            : base(
                AIType.AI_Melee /*, FightMode.Closest, 10, 1, 0.15, 0.4*/
            )
        {
            Name = "Pirate Lord";

            if (Female = Utility.RandomBool())
                Body = 186;
            else
                Body = 185;

            SetStr(578, 620);
            SetDex(450, 500);
            SetInt(200, 300);

            SetHits(4000, 6000);
            SetDamage(40, 55);

            SetDamageType(ResistanceType.Physical, 100);

            SetSkill(SkillName.Fencing, 100.5, 120.0);
            SetSkill(SkillName.Healing, 60.3, 100.0);
            SetSkill(SkillName.Bludgeoning, 100.5, 140.0);
            SetSkill(SkillName.Poisoning, 60.0, 100.5);
            SetSkill(SkillName.MagicResist, 72.5, 100.0);
            SetSkill(SkillName.Swords, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 72.5, 170.0);

            Fame = 50000;
            Karma = -50000;

            CanSwim = true;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 99.1;
            MF_Displacer = true;
            MF_Bomber = true;
            MF_HumanRevealer = true;
            PackItem(new Gold(6500, 15000));
            //PackItem( new Musket2() );

            //new ThoroughbredHorse().Rider = this;

            switch (Utility.Random(5))
            {
                case 0:
                    PackItem(new MessageInABottle(this.Map, 0, this.Location, this.X, this.Y));
                    break;
                case 1:
                    PackItem(new SpecialFishingNet());
                    break;
            }

            Item LongPants = new LongPants();
            LongPants.Movable = false;
            AddItem(LongPants);
            LongPants.Hue = 1;

            Item LeatherGloves = new LeatherGloves();
            LeatherGloves.Hue = 1;
            LeatherGloves.Movable = false;
            AddItem(LeatherGloves);

            Item FancyShirt = new FancyShirt();
            FancyShirt.Hue = 143;
            FancyShirt.Movable = false;
            AddItem(FancyShirt);

            Item BodySash = new BodySash();
            BodySash.Hue = 1;
            BodySash.Movable = false;
            AddItem(BodySash);

            Item TricorneHat = new TricorneHat();
            TricorneHat.Hue = 1;
            TricorneHat.Movable = false;
            AddItem(TricorneHat);

            Item ThighBoots = new ThighBoots();
            ThighBoots.Hue = 143;
            ThighBoots.Movable = false;
            AddItem(ThighBoots);

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
            /*Item Musket2 = new Musket2();
            Musket2.LootType = LootType.Blessed;
            Musket2.Movable = false;
            AddItem( Musket2 );
            
            switch ( Utility.Random( 1 ))
            {
                case 0: PackItem( new ClothingBlessDeed() );
                break;
                }*/
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Regular; }
        }
        public override int Meat
        {
            get { return 10; }
        }
        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool BardImmune
        {
            get { return true; }
        }
        public override bool ShowFameTitle
        {
            get { return false; }
        }
        public override bool Unprovokable
        {
            get { return true; }
        }

        /*public override bool OnBeforeDeath()
        {
            IMount mount = this.Mount;

            if ( mount != null )
                mount.Rider = null;

            if ( mount is Mobile )
                ((Mobile)mount).Delete();

            return base.OnBeforeDeath();
        }*/

        public override bool IsEnemy(Mobile m)
        {
            if (m.BodyMod == 183 || m.BodyMod == 184)
                return false;

            return base.IsEnemy(m);
        }

        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (
                to is Dragon
                || to is WhiteWyrm
                || to is SwampDragon
                || to is Drake
                || to is Nightmare
                || to is Daemon
            )
                damage *= 5;
        }

        public PirateLeader(Serial serial)
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
