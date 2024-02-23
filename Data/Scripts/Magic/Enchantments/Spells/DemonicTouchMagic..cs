using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class DemonicTouchMagic : EnchantMagic
    {
        private static SpellInfo m_Info = new SpellInfo("", "", 0);
        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1); }
        }
        public override double RequiredSkill
        {
            get { return 0.0; }
        }
        public override int RequiredMana
        {
            get { return 0; }
        }

        public DemonicTouchMagic(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

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
            else if (CheckBSequence(m, false) && CheckFizzle())
            {
                SpellHelper.Turn(Caster, m);

                m.PlaySound(0x202);
                m.FixedParticles(0x376A, 1, 62, 0x480, 3, 3, EffectLayer.Waist);
                m.FixedParticles(0x37C4, 1, 46, 0x481, 5, 3, EffectLayer.Waist);

                double toHeal = 50;
                int heal = Server.Misc.MyServerSettings.PlayerLevelMod((int)toHeal, Caster);
                m.Heal(heal);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private DemonicTouchMagic m_Owner;

            public InternalTarget(DemonicTouchMagic owner)
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
    }
}
