using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class SuccubusSkinMagic : EnchantMagic
    {
        private static SpellInfo m_Info = new SpellInfo("", "", 0);
        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(3); }
        }
        public override double RequiredSkill
        {
            get { return 0.0; }
        }
        public override int RequiredMana
        {
            get { return 0; }
        }

        private static Hashtable m_Table = new Hashtable();

        public SuccubusSkinMagic(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public static bool HasEffect(Mobile m)
        {
            return (m_Table[m] != null);
        }

        public static void RemoveEffect(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            if (m_Table.Contains(m))
            {
                Caster.LocalOverheadMessage(
                    MessageType.Regular,
                    0x481,
                    false,
                    "That target already has this affect."
                );
            }
            else if (CheckBSequence(m, false) && CheckFizzle())
            {
                SpellHelper.Turn(Caster, m);

                Timer t = new InternalTimer(m, Caster);
                t.Start();
                m_Table[m] = t;
                m.PlaySound(0x202);
                m.FixedParticles(0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist);
                m.SendMessage("Your skin changes, causing your wounds to heal faster.");

                double timer = 100.0;

                BuffInfo.RemoveBuff(m, BuffIcon.SuccubusSkin);
                BuffInfo.AddBuff(
                    m,
                    new BuffInfo(BuffIcon.SuccubusSkin, 1063559, TimeSpan.FromSeconds(timer), m)
                );
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SuccubusSkinMagic m_Owner;

            public InternalTarget(SuccubusSkinMagic owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile dest,
                source;
            private DateTime NextTick;
            private DateTime Expire;

            public InternalTimer(Mobile m, Mobile from)
                : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {
                dest = m;
                source = from;
                Priority = TimerPriority.FiftyMS;
                double timer = 100.0;
                Expire = DateTime.Now + TimeSpan.FromSeconds(timer);
            }

            protected override void OnTick()
            {
                if (!dest.CheckAlive())
                {
                    Stop();
                    BuffInfo.RemoveBuff(dest, BuffIcon.SuccubusSkin);
                    m_Table.Remove(dest);
                }

                if (DateTime.Now < NextTick)
                    return;

                if (DateTime.Now >= NextTick)
                {
                    double heal = Server.Misc.MyServerSettings.PlayerLevelMod(
                        Utility.RandomMinMax(5, 10),
                        dest
                    );
                    dest.Heal((int)heal);
                    dest.FixedParticles(0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist);
                    NextTick = DateTime.Now + TimeSpan.FromSeconds(4);
                }

                if (DateTime.Now >= Expire)
                {
                    Stop();
                    BuffInfo.RemoveBuff(dest, BuffIcon.SuccubusSkin);
                    if (m_Table.Contains(dest))
                        m_Table.Remove(dest);
                }
            }
        }
    }
}
