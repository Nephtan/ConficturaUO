using System;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class BanishMagic : EnchantMagic
    {
        private static SpellInfo m_Info = new SpellInfo("", "", 0);
        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
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

        public BanishMagic(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckFizzle())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public class InternalTarget : Target
        {
            private BanishMagic m_Owner;

            public InternalTarget(BanishMagic owner)
                : base(Core.ML ? 10 : 12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile && o is BaseCreature)
                {
                    Mobile m = (Mobile)o;
                    BaseCreature bc = m as BaseCreature;

                    if (!from.CanSee(m))
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (bc.ControlSlots == 666)
                    {
                        SpellHelper.Turn(from, m);

                        if (100 > Utility.RandomMinMax(1, 100))
                        {
                            Effects.SendLocationParticles(
                                EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                                0x3728,
                                8,
                                20,
                                5042
                            );
                            Effects.PlaySound(m, m.Map, 0x201);

                            m.Delete();
                        }
                        else
                        {
                            m.FixedEffect(0x3779, 10, 20);
                            from.SendLocalizedMessage(1010084); // The creature resisted the attempt to dispel it!
                        }
                    }
                    else if (bc == null || !bc.IsDispellable)
                    {
                        from.SendLocalizedMessage(1005049); // That cannot be dispelled.
                    }
                    else if (m_Owner.CheckHSequence(m))
                    {
                        SpellHelper.Turn(from, m);

                        double dispelChance =
                            (
                                50.0
                                + (
                                    (100 * (CastingSkill() - bc.DispelDifficulty))
                                    / (bc.DispelFocus * 2)
                                )
                            ) / 100;

                        if (dispelChance > Utility.RandomDouble())
                        {
                            Effects.SendLocationParticles(
                                EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                                0x3728,
                                8,
                                20,
                                5042
                            );
                            Effects.PlaySound(m, m.Map, 0x201);

                            m.Delete();
                        }
                        else
                        {
                            m.FixedEffect(0x3779, 10, 20);
                            m.PlaySound(0x3EA);
                            from.SendLocalizedMessage(1010084); // The creature resisted the attempt to dispel it!
                        }
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
