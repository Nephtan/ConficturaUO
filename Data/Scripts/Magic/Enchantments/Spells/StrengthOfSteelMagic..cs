using System;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class StrengthOfSteelMagic : EnchantMagic
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

        public StrengthOfSteelMagic(Mobile caster, Item scroll)
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
            else if (CheckBSequence(m) && CheckFizzle())
            {
                SpellHelper.Turn(Caster, m);

                int bonus = Server.Misc.MyServerSettings.PlayerLevelMod((int)(100 / 3), Caster);
                double timer = 10.0;
                SpellHelper.AddStatBonus(
                    Caster,
                    m,
                    StatType.Str,
                    bonus,
                    TimeSpan.FromMinutes(timer)
                );

                m.PlaySound(0x1EB);
                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);

                string args = String.Format("{0}", bonus);

                BuffInfo.RemoveBuff(Caster, BuffIcon.StrengthOfSteel);
                BuffInfo.AddBuff(
                    Caster,
                    new BuffInfo(
                        BuffIcon.StrengthOfSteel,
                        1063557,
                        1063558,
                        TimeSpan.FromMinutes(timer),
                        Caster,
                        args.ToString(),
                        true
                    )
                );
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private StrengthOfSteelMagic m_Owner;

            public InternalTarget(StrengthOfSteelMagic owner)
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
