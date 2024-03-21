using System;
using System.Collections;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("Captin Morgain's corpse")] // TODO: Corpse name?
    public class Morgan : BaseCreature
    {
        private static bool m_Talked; // flag to prevent spam

        string[] kfcsay = new string[] // things to say while greating
        {
            "Arrgg",
            "I'll see ya to Davy Jones",
            "Load the Cannons Boy's!",
            "What's ya be Look'en at, move ye swab's",
            "Keg of rum for their Heads"
        };

        [Constructable]
        public Morgan()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Name = "Captain Morgan";
            Body = 0x190;

            SetStr(999, 1500);
            SetDex(700, 999);
            SetInt(966, 1045);

            SetHits(5000, 10000);

            SetDamage(15, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Energy, 170);

            SetResistance(ResistanceType.Physical, 65, 70);
            SetResistance(ResistanceType.Fire, 65, 70);
            SetResistance(ResistanceType.Cold, 68, 70);
            SetResistance(ResistanceType.Poison, 68, 70);
            SetResistance(ResistanceType.Energy, 68, 70);

            SetSkill(SkillName.Psychology, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.Meditation, 100.1, 101.0);
            SetSkill(SkillName.Poisoning, 100.1, 101.0);
            SetSkill(SkillName.MagicResist, 175.2, 200.0);
            SetSkill(SkillName.Tactics, 120.1, 140.0);
            SetSkill(SkillName.FistFighting, 99.1, 120.0);
            SetSkill(SkillName.Swords, 120.0, 165.0);
            SetSkill(SkillName.Parry, 120.0, 155.5);

            Fame = 88000;
            Karma = -88000;
            AddItem(new FancyShirt());
            AddItem(new Server.Items.Doublet(137));
            AddItem(new Server.Items.TricorneHat(137));
            AddItem(new Server.Items.ShortPants(889));
            AddItem(new Server.Items.ThighBoots(2691));
            LeatherGloves gloves = new LeatherGloves();
            gloves.Hue = 2691;
            AddItem(gloves);
            AddItem(new Buckler());

            Item hair = new Item(0x203C);
            hair.Hue = 0x2691;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);
            Item beard = new Item(0x2691);
            beard.Hue = hair.Hue;
            beard.Layer = Layer.FacialHair;
            beard.Movable = false;
            AddItem(beard);
            VirtualArmor = 160;

            Scimitar weapon = new Scimitar();
            weapon.Hue = 0x966;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Exceptional;
            weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(8, 9);
            weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(8, 9);
            weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(8, 9);

            AddItem(weapon);

            //PackItem( new Token( 300, 500 ) );
            PackGold(1400, 1700);
        }

        public override bool AutoDispel
        {
            get { return true; }
        }
        public override bool Unprovokable
        {
            get { return true; }
        }
        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; }
        }
        public override int TreasureMapLevel
        {
            get { return 5; }
        }
        public override bool BardImmune
        {
            get { return true; }
        }

        public Morgan(Serial serial)
            : base(serial) { }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m_Talked == false)
            {
                if (m.InRange(this, 4))
                {
                    m_Talked = true;
                    SayRandom(kfcsay, this);
                    this.Move(GetDirectionTo(m.Location));
                    // Start timer to prevent spam
                    SpamTimer t = new SpamTimer();
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            public SpamTimer()
                : base(TimeSpan.FromSeconds(3))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Talked = false;
            }
        }

        private static void SayRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

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

        public void SpawnEvilPirate(Mobile m)
        {
            Map map = this.Map;

            if (map == null)
                return;

            EvilPirate spawned = new EvilPirate();

            spawned.Team = this.Team;
            spawned.Map = map;

            bool validLocation = false;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                    spawned.Location = new Point3D(x, y, Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    spawned.Location = new Point3D(x, y, z);
            }

            if (!validLocation)
                spawned.Location = this.Location;

            spawned.Combatant = m;
        }

        public void EatEvilPirate()
        {
            Map map = this.Map;

            if (map == null)
                return;

            ArrayList toEat = new ArrayList();

            IPooledEnumerable eable = map.GetMobilesInRange(this.Location, 2);

            foreach (Mobile m in eable)
            {
                if (m is HordeMinion)
                    toEat.Add(m);
            }

            eable.Free();

            if (toEat.Count > 0)
            {
                PlaySound(Utility.Random(0x3B, 2)); // Eat sound

                foreach (Mobile m in toEat)
                {
                    Hits += (m.Hits / 2);
                    m.Delete();
                }
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (this.Hits > (this.HitsMax / 4))
            {
                if (0.25 >= Utility.RandomDouble())
                    SpawnEvilPirate(attacker);
            }
            else if (0.25 >= Utility.RandomDouble())
            {
                EatEvilPirate();
            }
        }
    }
}
