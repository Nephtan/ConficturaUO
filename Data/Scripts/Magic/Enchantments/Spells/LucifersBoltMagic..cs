using System;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class LucifersBoltMagic : EnchantMagic
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

        public LucifersBoltMagic(Mobile caster, Item scroll)
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
            else if (
                Core.AOS && (m.Frozen || m.Paralyzed || (m.Spell != null && m.Spell.IsCasting))
            )
            {
                Caster.SendLocalizedMessage(1061923); // The target is already frozen.
            }
            else if (CheckHSequence(m) && CheckFizzle())
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect(4, Caster, ref m);

                double duration = 7.0 + (100 * 0.2);

                m.Paralyze(TimeSpan.FromSeconds(duration));
                m.FixedEffect(0x376A, 6, 1);
                m.BoltEffect(0);
            }

            FinishSequence();
        }

        public class InternalTarget : Target
        {
            private LucifersBoltMagic m_Owner;

            public InternalTarget(LucifersBoltMagic owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
