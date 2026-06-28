using System;
using System.Collections;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a lizard queen corpse")]
    public class Slither : BaseSubChampion
    {
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Greed; }
        }

        [Constructable]
        public Slither()
            : base(AIType.AI_Mage)
        {
            Name = "Queen Ssslither";
            Body = 35;
            Hue = 236;
            BaseSoundID = 417;

            SetStr(767, 1050);
            SetDex(116, 225);
            SetInt(455, 600);

            SetHits(3500, 4500);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Psychology, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 100.0);
            SetSkill(SkillName.Meditation, 35.4, 55.0);
            SetSkill(SkillName.MagicResist, 95.1, 115.0);
            SetSkill(SkillName.Tactics, 75.1, 95.0);
            SetSkill(SkillName.FistFighting, 75.1, 95.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 45;

            /*if ( Utility.RandomDouble() <= 0.4 )
            {
                PackItem( new Token ( 6 ) );
            }*/

            PackGem();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; }
        }
        public override int TreasureMapLevel
        {
            get { return 4; }
        }
        public override int Meat
        {
            get { return 4; }
        }

        public void SpawnLizardmanMage(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int mage = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is LizardmanMage)
                    ++mage;
            }

            if (mage < 3)
            {
                BaseCreature mage1 = new LizardmanMage();

                mage1.Team = this.Team;

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                mage1.MoveToWorld(loc, map);

                mage1.Combatant = target;
            }
        }

        /*public override void OnDamage( int amount, Mobile attacker, bool willKill )
                {
                    if( attacker is dovPlayerMobile )
                    {
                        dovPlayerMobile mobile = attacker as dovPlayerMobile;
        
                        if( mobile.LizRatFriendship == LizRat.Lizardman )
                        {
                            mobile.LizRatFriendship = LizRat.None;
                            mobile.SendLocalizedMessage( 1054103 );
                        }
                    }
           if ( 0.1 >= Utility.RandomDouble() )
                                 SpawnLizardmanMage( attacker );
                    base.OnDamage( amount, attacker, willKill );
                }*/
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (0.1 >= Utility.RandomDouble())
                SpawnLizardmanMage(attacker);
        }

        public Slither(Serial serial)
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
