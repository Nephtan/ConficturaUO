using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Spells.Enchantments
{
    public class OrbOfOrcusMagic : EnchantMagic
    {
        private static SpellInfo m_Info = new SpellInfo("", "", 0);
        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
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

        public OrbOfOrcusMagic(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        public override bool CheckCast()
        {
            DefensiveSpell.EndDefense(Caster);

            if (!base.CheckCast())
                return false;

            if (Caster.MagicDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }

            return true;
        }

        private static Hashtable m_Table = new Hashtable();

        public override void OnCast()
        {
            DefensiveSpell.EndDefense(Caster);

            if (Caster.MagicDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (CheckSequence())
            {
                if (CheckFizzle())
                {
                    int value = (int)(100 / 4);

                    Caster.MagicDamageAbsorb = value;

                    Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                    Caster.PlaySound(0x1E9);

                    BuffInfo.RemoveBuff(Caster, BuffIcon.OrbOfOrcus);
                    BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.OrbOfOrcus, 1063551));
                }
            }

            FinishSequence();
        }
    }
}
