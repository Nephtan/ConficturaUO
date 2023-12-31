using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Spells.HolyMan
{
    public abstract class HolyManSpell : Spell
    {
        public abstract int RequiredTithing { get; }
        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public override bool ClearHandsOnCast
        {
            get { return false; }
        }
        public override SkillName CastSkill
        {
            get { return SkillName.Spiritualism; }
        }
        public override SkillName DamageSkill
        {
            get { return SkillName.Healing; }
        }
        public override int CastRecoveryBase
        {
            get { return 7; }
        }

        public HolyManSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info) { }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if (Caster.Karma < 2500)
            {
                Caster.SendMessage("You have too little Karma to invoke this prayer.");
                return false;
            }
            else if (Caster.Skills[CastSkill].Value < RequiredSkill)
            {
                Caster.SendMessage(
                    "You must have at least "
                        + RequiredSkill
                        + " Spiritualism to invoke this prayer."
                );
                return false;
            }
            else if (GetSoulsInSymbol(Caster) < RequiredTithing)
            {
                Caster.SendMessage(
                    "You must have at least " + RequiredTithing + " piety to invoke this prayer."
                );
                return false;
            }
            else if (Caster.Mana < GetMana())
            {
                Caster.SendMessage(
                    "You must have at least " + GetMana() + " Mana to invoke this prayer."
                );
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            int requiredTithing = GetTithing(Caster, this);
            int mana = GetMana();

            if (Caster.Karma < 2500)
            {
                Caster.SendMessage("You have too little Karma to invoke this prayer.");
                return false;
            }
            else if (Caster.Skills[CastSkill].Value < RequiredSkill)
            {
                Caster.SendMessage(
                    "You must have at least "
                        + RequiredSkill
                        + " Spiritualism to invoke this prayer"
                );
                return false;
            }
            else if (GetSoulsInSymbol(Caster) < requiredTithing)
            {
                Caster.SendMessage(
                    "You must have at least " + requiredTithing + " piety to invoke this prayer."
                );
                return false;
            }
            else if (Caster.Mana < mana)
            {
                Caster.SendMessage(
                    "You must have at least " + mana + " Mana to invoke this prayer."
                );
                return false;
            }

            return true;
        }

        public override void FinishSequence()
        {
            DrainSoulsInSymbol(Caster, RequiredTithing);
            base.FinishSequence();
        }

        public override void DoFizzle()
        {
            Caster.PrivateOverheadMessage(
                MessageType.Regular,
                0x3B2,
                false,
                "You fail to invoke the power.",
                Caster.NetState
            );
            Caster.FixedParticles(0x3735, 1, 30, 9503, EffectLayer.Waist);
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = DateTime.Now;
        }

        public override int GetMana()
        {
            return ScaleMana(RequiredMana);
        }

        public static int GetTithing(Mobile Caster, HolyManSpell spell)
        {
            if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                return 0;

            return spell.RequiredTithing;
        }

        public override void SayMantra()
        {
            Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, Info.Mantra);
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
                Caster.PlaySound(0x1D6);
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            Caster.FixedEffect(0x37C4, 10, 42, 4, 3);
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 40.0;
        }

        public int ComputePowerValue(int div)
        {
            return ComputePowerValue(Caster, div);
        }

        public static int ComputePowerValue(Mobile from, int div)
        {
            if (from == null)
                return 0;

            int v = (int)
                Math.Sqrt((from.Karma * -1) + 20000 + (from.Skills.Spiritualism.Fixed * 10));

            return v / div;
        }

        public static void DrainSoulsInSymbol(Mobile from, int tithing)
        {
            if (AosAttributes.GetValue(from, AosAttribute.LowerRegCost) > Utility.Random(100))
                tithing = 0;

            if (tithing > 0)
            {
                ArrayList targets = new ArrayList();
                foreach (Item item in World.Items.Values)
                {
                    if (item is HolySymbol)
                    {
                        HolySymbol symbol = (HolySymbol)item;
                        if (symbol.owner == from)
                        {
                            symbol.BanishedEvil = symbol.BanishedEvil - tithing;
                            if (symbol.BanishedEvil < 1)
                            {
                                symbol.BanishedEvil = 0;
                            }
                            symbol.InvalidateProperties();
                        }
                    }
                }
            }
        }

        public static int GetSoulsInSymbol(Mobile from)
        {
            int souls = 0;

            ArrayList targets = new ArrayList();
            foreach (Item item in World.Items.Values)
            {
                if (item is HolySymbol)
                {
                    HolySymbol symbol = (HolySymbol)item;
                    if (symbol.owner == from)
                    {
                        souls = symbol.BanishedEvil;
                    }
                }
            }

            return souls;
        }
    }
}
