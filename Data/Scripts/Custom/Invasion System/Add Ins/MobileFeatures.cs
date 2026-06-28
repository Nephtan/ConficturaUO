using System;
using System.Collections;
using Server;
using Server.ContextMenus;
using Server.Engines.Quests;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

//using Server.Engines.Level;
//using Server.Engines.Jail;
//using Server.Engines.Staff;

namespace Server.Mobiles
{
    public class MobileFeatures
    {
        private static string[] m_MobileFeatures = new string[]
        {
            "AntiMarksmanship", ////Teleports to archers outside of 5 tile radius (chance 50%)//
            "AntiEscape", ////Teleports to players that run (chance 50%)//
            "FireAreaAttack", ////FlameStrikes players within 15 tiles (chance 50%)
            "WaterAreaAttack", ////WaterStrikes players within 15 tiles (chance 50%)
            "AirAreaAttack", ////AirStrikes players within 15 tiles (chance 50%)
            "RobotRevealer", ////Reveals players within 10 tiles, talking like a robot//
            "HumanRevealer", ////Reveals players within 10 tiles, talking like a human//
            "Bomber", ////Bombs players//
            "MassPeace", ////Makes all mobiles within 25 tiles Peace (nullifies combatant) (chance 50%)//
            "MassProvoke", ////Makes all mobiles within 25 tiles Attack the melee damage of the mobile (chance 50%)//
            "DrainStam", ////Drains some stamina of the combatant (chance 50%)
            "DrainMana", ////Drains some mana of the combatant (chance 50%)
            "DrainHits", ////Drains some hit points of the combatant (chance 50%)
            "TakesDrugs", //Makes the mobile talk as if it were stoned
            "Displacer" ////Absorbs one hit (like displacement cloak) (chance 20%)
        };

        //BEGIN ANTI ARCHERY

        public static void DoAntiMarksmanship(BaseCreature mobile, Mobile player)
        {
            if (mobile.Combatant != null)
                if (!player.InRange(mobile, 5))
                {
                    Point3D from = mobile.Location;
                    Point3D to = player.Location;

                    if (mobile.Mana >= 10)
                    {
                        mobile.Location = to;

                        mobile.Mana -= 10;

                        mobile.Say("Grrrr");

                        Effects.SendLocationParticles(
                            EffectItem.Create(from, mobile.Map, EffectItem.DefaultDuration),
                            0x3728,
                            10,
                            10,
                            2023
                        );
                        Effects.SendLocationParticles(
                            EffectItem.Create(to, mobile.Map, EffectItem.DefaultDuration),
                            0x3728,
                            10,
                            10,
                            5023
                        );

                        mobile.PlaySound(0x1FE);
                    }
                }
        }

        //END ANTI ARCHERY

        //BEGIN ANTI ESCAPE

        public static void DoAntiEscape(BaseCreature mobile, Mobile player)
        {
            if (mobile.Combatant != null)
                if (!player.InRange(mobile, 5))
                {
                    Point3D from = mobile.Location;
                    Point3D to = player.Location;

                    if (mobile.Mana >= 10)
                    {
                        mobile.Location = to;

                        mobile.Mana -= 10;

                        mobile.Say("Grrrr");

                        Effects.SendLocationParticles(
                            EffectItem.Create(from, mobile.Map, EffectItem.DefaultDuration),
                            0x3728,
                            10,
                            10,
                            2023
                        );
                        Effects.SendLocationParticles(
                            EffectItem.Create(to, mobile.Map, EffectItem.DefaultDuration),
                            0x3728,
                            10,
                            10,
                            5023
                        );

                        mobile.PlaySound(0x1FE);
                    }
                    else if (!player.InRange(mobile, 20))
                        return;
                }
        }

        //END ANTI ESCAPE

        //BEGIN BOMBER

        private static DateTime m_NextBomb;
        private static int m_Thrown;

        public static void DoBomber(BaseCreature mobile, Mobile player)
        {
            Mobile combatant = player;

            if (
                combatant == null
                || combatant.Deleted
                || combatant.Map != mobile.Map
                || !mobile.InRange(combatant, 12)
                || !mobile.CanBeHarmful(combatant)
                || !mobile.InLOS(combatant)
            )
                return;

            if (DateTime.Now >= m_NextBomb)
            {
                ThrowBomb(combatant, player);

                m_Thrown++;

                if (0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1)
                    m_NextBomb = DateTime.Now + TimeSpan.FromSeconds(3.0);
                else
                    m_NextBomb =
                        DateTime.Now + TimeSpan.FromSeconds(5.0 + (10.0 * Utility.RandomDouble()));
            }
        }

        public static void ThrowBomb(Mobile m, Mobile bomber)
        {
            m.DoHarmful(m);

            bomber.MovingParticles(
                m,
                0x1AD7,
                1,
                0,
                false,
                true,
                0,
                0,
                9502,
                6014,
                0x11D,
                EffectLayer.Waist,
                0
            );

            new BomberTimer(m, bomber).Start();
        }

        private class BomberTimer : Timer
        {
            private Mobile m_Mobile,
                m_From;

            public BomberTimer(Mobile m, Mobile from)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = m;
                m_From = from;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                m_Mobile.PlaySound(0x11D);

                //int toStrike = Utility.RandomMinMax( 25, 35 );
                //m.Damage( toStrike, mobile );
                m_Mobile.Damage(((Utility.Random(20, 60)) - (m_Mobile.FireResistance / 2)));
                //AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 20, 60 ), 0, 100, 0, 0, 0 );
            }
        }

        //END BOMBER

        //BEGIN MASS PEACE


        public static void DoMassPeace(BaseCreature mobile, Mobile player)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in mobile.GetMobilesInRange(15))
            {
                if (m == mobile || !m.CanBeHarmful(m))
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != mobile.Team
                    )
                )
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                m.DoHarmful(m);
                m.Combatant = null;
                m.PlaySound(0x418);
                m.Emote("*you see {0} looks peacful*", m.Name);
            }
        }

        //END MASS PEACE

        //BEGIN MASS PROVOKE

        public static void DoMassProvoke(BaseCreature mobile, Mobile player)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in mobile.GetMobilesInRange(15))
            {
                if (m == mobile || !m.CanBeHarmful(m))
                    continue;

                if (m is BaseCreature)
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                m.DoHarmful(m);
                player.Combatant = null;
                m.Combatant = player;
                m.PlaySound(0x403);
                m.Emote("*you see {0} looks furious*", m.Name);
            }
        }

        //END MASS PROVOKE

        //BEGIN ROBOT SPEAKING REVEALER

        private static bool s_RoboTalked;

        static string[] robotsay = new string[]
        {
            "Infra-Red Camera Activated!",
            "Search & Destroy!",
            "Heat-Sensor: Detecting Life-form!"
        };

        public static void DoRobotReveal(BaseCreature mobile)
        {
            foreach (Mobile m in mobile.GetMobilesInRange(10))
                if (m != null && m.Hidden && m.AccessLevel == AccessLevel.Player)
                    m.Hidden = false;

            if (s_RoboTalked == false)
            {
                s_RoboTalked = true;
                SayRobotRandom(robotsay, mobile);
                RobotSpamTimer t = new RobotSpamTimer();
                t.Start();
            }
        }

        private class RobotSpamTimer : Timer
        {
            public RobotSpamTimer()
                : base(TimeSpan.FromSeconds(1.5))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                s_RoboTalked = false;
            }
        }

        private static void SayRobotRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        //END ROBOT SPEAKING REVEALER

        //BEGIN HUMAN SPEAKING REVEALER

        private static bool s_HumanTalked;

        static string[] humansay = new string[]
        {
            "Reveal!",
            "I Sense Someone",
            "You Can Run, But You Can't Hide!"
        };

        public static void DoHumanReveal(BaseCreature mobile)
        {
            foreach (Mobile m in mobile.GetMobilesInRange(10))
                if (m != null && m.Hidden && m.AccessLevel == AccessLevel.Player)
                    m.Hidden = false;

            if (s_HumanTalked == false)
            {
                s_HumanTalked = true;
                SayHumanRandom(humansay, mobile);
                HumanSpamTimer t = new HumanSpamTimer();
                t.Start();
            }
        }

        private class HumanSpamTimer : Timer
        {
            public HumanSpamTimer()
                : base(TimeSpan.FromSeconds(1.5))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                s_HumanTalked = false;
            }
        }

        private static void SayHumanRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        //END HUMAN REVEALER

        //BEGIN AREA FIRE ATTACK

        public static void DoFireAreaAttack(BaseCreature mobile, Mobile player)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in mobile.GetMobilesInRange(10))
            {
                if (m == mobile || !m.CanBeHarmful(m))
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != mobile.Team
                    )
                )
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                /*if ( CheckResisted( m ) )
{
                 m.DoHarmful( m );
                int Strike = Utility.RandomMinMax( 5, 15 );
    m.Damage( Strike, mobile );
m.SendMessage( "Your feel the heat of fire!" );
                return;
}*/
                m.DoHarmful(m);

                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                m.PlaySound(0x208);

                m.SendMessage("Your skin blisters as the fire burns you!");

                //int toStrike = Utility.RandomMinMax( 25, 35 );

                m.Damage(((Utility.Random(25, 35)) - (m.FireResistance / 2)));
                //m.Damage( toStrike, mobile );
            }
        }

        //END FIRE AREA ATTACK

        //BEGIN WATER  ATTACK

        public static void DoWaterAreaAttack(BaseCreature mobile, Mobile player)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in mobile.GetMobilesInRange(10))
            {
                if (m == mobile || !m.CanBeHarmful(m))
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != mobile.Team
                    )
                )
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                m.DoHarmful(m);

                m.FixedParticles(0x1fb7, 50, 50, 5052, EffectLayer.Waist);
                m.PlaySound(279);
                m.PlaySound(280);

                m.SendMessage("Your skin numbs as the cold freezes you!");

                //int toStrike = Utility.RandomMinMax( 25, 35 );

                m.Damage(((Utility.Random(25, 35)) - (m.ColdResistance / 2)));
                //m.Damage( toStrike, mobile );
            }
        }

        //END WATER AREA ATTACK

        //BEGIN AIR AREA ATTACK

        public static void DoAirAreaAttack(BaseCreature mobile, Mobile player)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in mobile.GetMobilesInRange(10))
            {
                if (m == mobile || !m.CanBeHarmful(m))
                    continue;

                if (
                    m is BaseCreature
                    && (
                        ((BaseCreature)m).Controlled
                        || ((BaseCreature)m).Summoned
                        || ((BaseCreature)m).Team != mobile.Team
                    )
                )
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                m.DoHarmful(m);

                m.FixedParticles(0x3728, 50, 50, 5052, EffectLayer.Waist);
                m.PlaySound(655);

                m.SendMessage("Your lose your breath as the air hits you!");

                int toStrike = Utility.RandomMinMax(25, 35);

                m.Damage(toStrike, mobile);
            }
        }

        //END AIR AREA ATTACK

        //BEGIN HITS DRAIN ATTACK

        public static void DoHitsDrainAttack(BaseCreature mobile)
        {
            foreach (Mobile m in mobile.GetMobilesInRange(3))
                if (m != null && m.Hits >= 50 && m.AccessLevel == AccessLevel.Player)
                    mobile.Hits += 2;
        }

        //END HITS DRAIN ATTACK

        //BEGIN STAM DRAIN ATTACK

        public static void DoStamDrainAttack(BaseCreature mobile)
        {
            foreach (Mobile m in mobile.GetMobilesInRange(3))
                if (m != null && m.Stam >= 50 && m.AccessLevel == AccessLevel.Player)
                    mobile.Stam += 5;
        }

        //END STAM DRAIN ATTACK

        //BEGIN MANA DRAIN ATTACK

        public static void DoManaDrainAttack(BaseCreature mobile)
        {
            foreach (Mobile m in mobile.GetMobilesInRange(3))
                if (m != null && m.Mana >= 50 && m.AccessLevel == AccessLevel.Player)
                    mobile.Mana += 5;
        }

        //END MANA DRAIN ATTACK

        //BEGIN DISPLACER

        private static Timer m_Timer;
        private static BaseCreature mob;

        public static void DoDisplace(BaseCreature mobile, Mobile player)
        {
            //player.Emote("*Your attack was displaced*");
            //mobile.Hidden = true;
            mobile.Frozen = true;
            mobile.Frozen = false;
            player.Combatant = null;
            Effects.SendLocationParticles(
                EffectItem.Create(mobile.Location, mobile.Map, EffectItem.DefaultDuration),
                0x3779,
                50,
                50,
                5052
            );
            player.PlaySound(964);
        }

        //END DISPLACER

        //BEGIN DRUG TAKER

        private static bool s_StonedTalked;

        static string[] stonedsay = new string[]
        {
            "*That's gooooood shiiiiiit!*",
            "*Paff*",
            "A friend with weed is a friend indeed!",
            "This shit's da bomb!",
            "Dude... Sweet... Dude... Sweet...",
            "I'm all high"
        };

        public static void ActStoned(BaseCreature mobile)
        {
            mobile.Direction = (Direction)Utility.Random(8);

            if (s_HumanTalked == false)
            {
                s_HumanTalked = true;
                SayStonedRandom(stonedsay, mobile);
                mobile.PlaySound(0x420); // coughing
                StonedSpamTimer t = new StonedSpamTimer();
                t.Start();

                if (1 == Utility.Random(20))
                    mobile.PlaySound(mobile.Female ? 794 : 1066);
            }
        }

        private class StonedSpamTimer : Timer
        {
            public StonedSpamTimer()
                : base(TimeSpan.FromSeconds(1.5))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                s_StonedTalked = false;
            }
        }

        private static void SayStonedRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        //END TAKES DRUGS
    }
}
