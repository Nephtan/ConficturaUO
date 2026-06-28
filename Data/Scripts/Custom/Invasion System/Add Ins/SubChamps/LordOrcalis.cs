using System;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    public class LordOrcalis : BaseSubChampion
    {
        public override InhumanSpeech SpeechType
        {
            get { return InhumanSpeech.Orc; }
        }
        public override ChampionSkullType SkullType
        {
            get { return ChampionSkullType.Pain; }
        }

        [Constructable]
        public LordOrcalis()
            : base(AIType.AI_Melee)
        {
            Title = "the Orc Lord";
            Name = "Orcalis";
            Body = 182;
            //Hue = 1404;
            //BaseSoundID = 0x45A;

            SetStr(959, 1182);
            SetDex(95, 99);
            SetInt(60, 90);

            SetHits(3000, 4000);

            SetDamage(50, 65);

            MF_Displacer = true;
            MF_Bomber = true;
            MF_HumanRevealer = true;
            SetDamageType(ResistanceType.Physical, 94);
            SetDamageType(ResistanceType.Cold, 32);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Bludgeoning, 112.1, 125.0);
            SetSkill(SkillName.MagicResist, 125.5, 188.0);
            SetSkill(SkillName.Tactics, 122.6, 125.0);
            SetSkill(SkillName.FistFighting, 122.6, 125.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 63;
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is Artifact_OrcishVisage)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            Item item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is Artifact_OrcishVisage)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override bool AlwaysMurderer
        {
            get { return true; }
        }
        public override bool BardImmune
        {
            get { return true; }
        }
        public override bool DoesLifeDraining
        {
            get { return true; }
        }
        public override bool DoesNoxStriking
        {
            get { return true; }
        }

        public override bool Unprovokable
        {
            get { return true; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Regular; }
        }
        public override int TreasureMapLevel
        {
            get { return 5; }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            SpawnOrcMage(caster);
        }

        public override void OnDamage(int amount, Mobile attacker, bool willKill)
        {
            SpawnOrcLord(attacker);
            base.OnDamage(amount, attacker, willKill);
        }

        public void SpawnOrcLord(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int orcs = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is OrcishLord)
                    ++orcs;
            }

            if (orcs < 10)
            {
                BaseCreature orc = new OrcishLord();

                orc.Team = this.Team;

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

                orc.MoveToWorld(loc, map);

                orc.Combatant = target;
            }
        }

        public void SpawnOrcMage(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int morcs = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is OrcishMage)
                    ++morcs;
            }

            if (morcs < 10)
            {
                BaseCreature morc = new OrcishMage();

                morc.Team = this.Team;

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

                morc.MoveToWorld(loc, map);

                morc.Combatant = target;
            }
        }

        public LordOrcalis(Serial serial)
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
