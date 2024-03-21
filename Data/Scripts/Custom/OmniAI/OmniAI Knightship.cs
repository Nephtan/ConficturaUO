// Created by Peoharen
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.DeathKnight;
using Server.Spells.Eighth;
using Server.Spells.Elementalism;
using Server.Spells.Fifth;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Herbalist;
using Server.Spells.HolyMan;
using Server.Spells.Jedi;
using Server.Spells.Jester;
using Server.Spells.Magical;
using Server.Spells.Mystic;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Research;
using Server.Spells.Second;
using Server.Spells.Seventh;
using Server.Spells.Shinobi;
using Server.Spells.Sixth;
using Server.Spells.Song;
using Server.Spells.Syth;
using Server.Spells.Third;
using Server.Spells.Undead;
using Server.Targeting;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void KnightshipPower()
        {
            if (Utility.Random(100) > 30)
            {
                Spell spell = null;

                spell = GetPaladinSpell();

                if (spell != null)
                    spell.Cast();
            }
            else
                UseWeaponStrike();

            return;
        }

        public Spell GetPaladinSpell()
        {
            if (CheckForRemoveCurse() == true && Utility.RandomDouble() > 0.25)
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(1154, "Casting Remove Curse");

                return new RemoveCurseSpell(m_Mobile, null);
            }

            int whichone = Utility.RandomMinMax(1, 4);

            if (
                whichone == 4
                && m_Mobile.Skills[SkillName.Knightship].Value >= 55.0
                && m_Mobile.Mana >= 10
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(1154, "Casting Holy Light");

                return new HolyLightSpell(m_Mobile, null);
            }
            else if (whichone >= 3 && CheckForDispelEvil())
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(1154, "Casting Dispel Evil");

                return new DispelEvilSpell(m_Mobile, null);
            }
            else if (
                whichone >= 2
                && !(DivineFurySpell.UnderEffect(m_Mobile))
                && m_Mobile.Skills[SkillName.Knightship].Value >= 35.0
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(1154, "Casting Divine Fury");

                return new DivineFurySpell(m_Mobile, null);
            }
            else if (CheckForConsecrateWeapon())
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(1154, "Casting Consecrate Weapon");

                return new ConsecrateWeaponSpell(m_Mobile, null);
            }
            else
                return null;
        }

        public bool CheckForConsecrateWeapon()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(1154, "Checking to bless my weapon");

            if (m_Mobile.Skills[SkillName.Knightship].Value < 15.0 || m_Mobile.Mana <= 9)
                return false;

            BaseWeapon weapon = m_Mobile.Weapon as BaseWeapon;

            if (weapon != null && !weapon.Consecrated)
                return true;
            else
                return false;
        }

        public bool CheckForDispelEvil()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(1154, "Checking to dispel evil");

            if (m_Mobile.Skills[SkillName.Knightship].Value < 35.0 || m_Mobile.Mana <= 9)
                return false;

            bool cast = false;

            foreach (Mobile m in m_Mobile.GetMobilesInRange(4))
            {
                if (m != null)
                {
                    if (
                        m is BaseCreature
                        && ((BaseCreature)m).Summoned
                        && !((BaseCreature)m).IsAnimatedDead
                    )
                        cast = true;
                    else if (
                        m is BaseCreature
                        && !((BaseCreature)m).Controlled
                        && ((BaseCreature)m).Karma < 0
                    )
                        cast = true;
                    else if (TransformationSpellHelper.GetContext(m) != null)
                        cast = true;
                }
                continue;
            }

            return cast;
        }

        public bool CheckForRemoveCurse()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(1154, "Checking for remove curse");

            if (m_Mobile.Skills[SkillName.Knightship].Value < 5.0 || m_Mobile.Mana <= 19)
                return false;

            StatMod mod;

            mod = m_Mobile.GetStatMod("[Magic] Str Offset");

            if (mod == null)
                mod = m_Mobile.GetStatMod("[Magic] Dex Offset");

            if (mod == null)
                mod = m_Mobile.GetStatMod("[Magic] Int Offset");

            if (mod != null && mod.Offset < 0)
                return true;

            Mobile foe = m_Mobile.Combatant;

            if (foe == null)
                return false;

            //There is no way to know if they are under blood oath or strangle without editing the spells so we just check for necro skills instead.
            if (foe.Skills[SkillName.Necromancy].Value > 20.0 && Utility.RandomDouble() > 0.6)
                return true;

            return false;
        }
    }
}
