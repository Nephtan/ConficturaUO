using System;
using System.Collections;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an ogre cheiftan corpse")]
    public class OgreCheif : BaseSubChampion
    {
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Pain; }
        }

        [Constructable]
        public OgreCheif()
            : base(AIType.AI_Melee)
        {
            Name = "an ogre cheiftan";
            Body = 83;
            Hue = 1654;
            BaseSoundID = 427;

            SetStr(767, 1050);
            SetDex(70, 105);
            SetInt(55, 75);

            SetHits(4500, 5500);

            SetDamage(24, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 125.1, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.FistFighting, 100.0, 120.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 60;

            /*if ( Utility.RandomDouble() <= 0.4 )
            {
                PackItem( new Token ( 4 ) );
            }*/

            PackGem();

            PackItem(new Club());
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
            get { return Poison.Regular; }
        }
        public override int TreasureMapLevel
        {
            get { return 3; }
        }
        public override int Meat
        {
            get { return 2; }
        }

        public void SpawnOgreLord(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int ogrelords = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is OgreLord)
                    ++ogrelords;
            }

            if (ogrelords < 3)
            {
                BaseCreature ogrelord = new OgreLord();

                ogrelord.Team = this.Team;

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

                ogrelord.MoveToWorld(loc, map);

                ogrelord.Combatant = target;
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (0.1 >= Utility.RandomDouble())
                SpawnOgreLord(attacker);
        }

        public OgreCheif(Serial serial)
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
