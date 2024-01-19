using System;
using Server;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a Pirate corpse")]
    public class PirateFirstMate : BaseCreature
    {
        [Constructable]
        public PirateFirstMate()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4)
        {
            Name = "Pirate's First Mate";

            if (Female = Utility.RandomBool())
                Body = 186;
            else
                Body = 185;

            SetStr(578, 620);
            SetDex(450, 500);
            SetInt(200, 300);

            SetHits(450, 550);
            SetDamage(30, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetSkill(SkillName.Fencing, 72.5, 100.0);
            SetSkill(SkillName.Healing, 60.3, 100.0);
            SetSkill(SkillName.Bludgeoning, 72.5, 100.0);
            SetSkill(SkillName.Poisoning, 60.0, 100.5);
            SetSkill(SkillName.MagicResist, 72.5, 100.0);
            SetSkill(SkillName.Swords, 72.5, 100.0);
            SetSkill(SkillName.Tactics, 72.5, 100.0);

            Fame = 10000;
            Karma = -10000;

            CanSwim = false;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 99.1;

            PackItem(new Gold(650, 1500));
            //PackItem( new Musket() );
            //PackItem( new TreasureChestLevel4() );

            //new ArabianHorse().Rider = this;

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
            FancyShirt.Hue = 0;
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
            ThighBoots.Hue = 1;
            ThighBoots.Movable = false;
            AddItem(ThighBoots);

            /*Item Musket = new Musket();
            Musket.LootType = LootType.Blessed;
            Musket.Movable = false;
            AddItem( Musket );
            
            switch ( Utility.Random( 1 ))
            {
                case 0: PackItem( new ClothingBlessDeed() );
                break;
                }*/
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override int Meat
        {
            get { return 1; }
        }
        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool ShowFameTitle
        {
            get { return true; }
        }

        public override bool OnBeforeDeath()
        {
            IMount mount = this.Mount;

            if (mount != null)
                mount.Rider = null;

            if (mount is Mobile)
                ((Mobile)mount).Delete();

            return base.OnBeforeDeath();
        }

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

        public PirateFirstMate(Serial serial)
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
