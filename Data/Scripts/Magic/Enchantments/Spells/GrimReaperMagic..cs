using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Chivalry;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class GrimReaperMagic : EnchantMagic
    {
        private static SpellInfo m_Info = new SpellInfo("", "", 0);
        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(0.5); }
        }
        public override double RequiredSkill
        {
            get { return 0.0; }
        }
        public override int RequiredMana
        {
            get { return 0; }
        }

        public GrimReaperMagic(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence() && CheckFizzle())
            {
                Caster.PlaySound(0x0F5);
                Caster.PlaySound(0x1ED);
                Caster.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
                Caster.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);

                Timer t = (Timer)m_Table[Caster];

                if (t != null)
                    t.Stop();

                double delay = (double)ComputePowerValue(1) / 60;

                // TODO: Should caps be applied?
                if (delay < 1.5)
                    delay = 1.5;
                else if (delay > 3.5)
                    delay = 3.5;

                m_Table[Caster] = Timer.DelayCall(
                    TimeSpan.FromMinutes(delay),
                    new TimerStateCallback(Expire_Callback),
                    Caster
                );

                if (Caster is PlayerMobile)
                {
                    ((PlayerMobile)Caster).EnemyOfOneType = null;
                    ((PlayerMobile)Caster).WaitingForEnemy = true;

                    BuffInfo.AddBuff(
                        Caster,
                        new BuffInfo(
                            BuffIcon.GrimReaper,
                            1063546,
                            1063547,
                            TimeSpan.FromMinutes(delay),
                            Caster
                        )
                    );
                }
            }

            FinishSequence();
        }

        private static Hashtable m_Table = new Hashtable();

        private static void Expire_Callback(object state)
        {
            Mobile m = (Mobile)state;

            m_Table.Remove(m);

            m.PlaySound(0x1F8);

            if (m is PlayerMobile)
            {
                ((PlayerMobile)m).EnemyOfOneType = null;
                ((PlayerMobile)m).WaitingForEnemy = false;
            }
        }
    }
}
