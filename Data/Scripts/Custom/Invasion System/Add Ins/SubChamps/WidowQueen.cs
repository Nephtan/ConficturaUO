using System;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public class BlackWidowQueen : BaseSubChampion
    {
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Venom; }
        }

        [Constructable]
        public BlackWidowQueen()
            : base(AIType.AI_Melee)
        {
            Body = 173;
            Name = "Queen of the Widows";
            Hue = 1336;

            BaseSoundID = 0x183;

            SetStr(505, 1000);
            SetDex(102, 300);
            SetInt(402, 600);

            SetHits(6000);
            SetStam(105, 600);

            SetDamage(21, 33);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.MagicResist, 70.7, 140.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.FistFighting, 97.6, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override bool BardImmune
        {
            get { return true; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; }
        }
        public override Poison HitPoison
        {
            get { return Poison.Lethal; }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // TODO: Web ability
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;

            SpawnDreadSpider(caster);
        }

        public void SpawnDreadSpider(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int dreadspiders = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is DreadSpider)
                    ++dreadspiders;
            }

            if (dreadspiders < 10)
            {
                BaseCreature dreadspider = new DreadSpider();

                dreadspider.Team = this.Team;

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

                dreadspider.MoveToWorld(loc, map);

                dreadspider.Combatant = target;
            }
        }

        public BlackWidowQueen(Serial serial)
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
