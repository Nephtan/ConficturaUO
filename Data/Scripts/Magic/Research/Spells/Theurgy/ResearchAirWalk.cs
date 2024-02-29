using System;
using System.Collections;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Spells.Research
{
    public class ResearchAirWalk : ResearchSpell
    {
        public override int spellIndex
        {
            get { return 55; }
        }
        public int CirclePower = 7;
        public static int spellID = 55;
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.0); }
        }
        public override double RequiredSkill
        {
            get
            {
                return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8)));
            }
        }
        public override int RequiredMana
        {
            get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); }
        }

        private static SpellInfo m_Info = new SpellInfo(
            Server.Misc.Research.SpellInformation(spellID, 2),
            Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
            203,
            9031,
            Reagent.SpidersSilk,
            Reagent.PixieSkull,
            Reagent.ButterflyWings
        );

        public ResearchAirWalk(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public static void RemoveEffect(Mobile m)
        {
            m.EndAction(typeof(ResearchAirWalk));
        }

        public static bool UnderEffect(Mobile m)
        {
            if (!m.CanBeginAction(typeof(ResearchAirWalk)))
                return true;

            return false;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!Caster.CanBeginAction(typeof(ResearchAirWalk)))
                {
                    ResearchAirWalk.RemoveEffect(Caster);
                }

                int TotalTime = (int)((DamagingSkill(Caster) * 20) / 60);
                new InternalTimer(Caster, TimeSpan.FromSeconds(TotalTime)).Start();
                Caster.BeginAction(typeof(ResearchAirWalk));
                Point3D air = new Point3D((Caster.X + 1), (Caster.Y + 1), (Caster.Z + 5));
                Effects.SendLocationParticles(
                    EffectItem.Create(air, Caster.Map, EffectItem.DefaultDuration),
                    0x5590,
                    9,
                    32,
                    Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0),
                    0,
                    5022,
                    0
                );
                Caster.PlaySound(0x014);
                Server.Misc.Research.ConsumeScroll(Caster, true, spellID, alwaysConsume, Scroll);

                BuffInfo.RemoveBuff(Caster, BuffIcon.AirWalk);
                BuffInfo.AddBuff(
                    Caster,
                    new BuffInfo(
                        BuffIcon.AirWalk,
                        1063632,
                        1063633,
                        TimeSpan.FromSeconds(TotalTime),
                        Caster
                    )
                );
            }
            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_m;
            private DateTime m_Expire;

            public InternalTimer(Mobile Caster, TimeSpan duration)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
            {
                m_m = Caster;
                m_Expire = DateTime.Now + duration;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_Expire)
                {
                    ResearchAirWalk.RemoveEffect(m_m);
                    BuffInfo.RemoveBuff(m_m, BuffIcon.AirWalk);
                    Stop();
                }
            }
        }
    }
}
