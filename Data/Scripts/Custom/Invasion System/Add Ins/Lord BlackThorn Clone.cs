using System;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public class BlackthornClone : BaseSpecialCreature
    {
        [Constructable]
        public BlackthornClone()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "BlackThorn";
            Body = 400;
            //Hue = 1404;
            //BaseSoundID = 0x45A;

            SetStr(959, 1182);
            SetDex(95, 99);
            SetInt(60, 90);

            SetHits(1500, 2000);

            SetDamage(50, 65);

            MF_Displacer = true;
            MF_MassProvoke = true;
            MF_AntiEscape = true;

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
            LordBlackthorneSuit suit = new LordBlackthorneSuit();
            suit.Movable = false;
            AddItem(suit);
            VirtualArmor = 83;
        }

        public override bool DoesTripleBolting
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
            get { return 5; }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            SpawnMech(caster);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SpawnMech(from);
            if (from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random(100))
            {
                string[] toSay = new string[]
                {
                    "{0}!!  I'm just here to stall you!",
                    "{0}!!  The real Blackthorn is already gone!",
                    "{0}!!  You will never find the secret door!",
                    "{0}!!  The true Blackthorn will crush you!"
                };

                this.Say(true, String.Format(toSay[Utility.Random(toSay.Length)], from.Name));
            }
            base.OnDamage(amount, from, willKill);
        }

        public void SpawnMech(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int mechs = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is RunicGolemInvader)
                    ++mechs;
            }

            if (mechs < 10)
            {
                BaseCreature mech = new RunicGolemInvader();

                mech.Team = this.Team;

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

                mech.MoveToWorld(loc, map);

                mech.Combatant = target;
            }
        }

        public BlackthornClone(Serial serial)
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
