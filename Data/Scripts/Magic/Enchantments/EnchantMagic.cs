using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Spells.Enchantments
{
    public abstract class EnchantMagic : Spell
    {
        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public override bool ClearHandsOnCast
        {
            get { return false; }
        }
        public override int CastRecoveryBase
        {
            get { return 7; }
        }
        public abstract SpellCircle Circle { get; }

        public EnchantMagic(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info) { }

        public static double CastingSkill()
        {
            return 100.0;
        }

        public override bool CheckCast()
        {
            return true;
        }

        public override int GetMana()
        {
            return 0;
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = 0;
            max = 0;
        }

        public int ComputePowerValue(int div)
        {
            return ComputePowerValue(Caster, div);
        }

        public static int ComputePowerValue(Mobile from, int div)
        {
            if (from == null)
                return 0;

            int v = (int)Math.Sqrt(30000);

            return v / div;
        }
    }
}
