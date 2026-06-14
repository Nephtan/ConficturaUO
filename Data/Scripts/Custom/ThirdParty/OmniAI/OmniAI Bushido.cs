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
        public void BushidoPower()
        {
            if (
                0.5 > Utility.RandomDouble()
                && !(
                    Confidence.IsConfident(m_Mobile)
                    || CounterAttack.IsCountering(m_Mobile)
                    || Evasion.IsEvading(m_Mobile)
                )
            )
                UseBushidoStance();
            else
                UseBushidoMove();
        }

        public void UseBushidoStance()
        {
            Spell spell = null;

            if (m_Mobile.Debug)
                m_Mobile.Say(2117, "Using a samurai stance");

            BaseWeapon weapon = m_Mobile.Weapon as BaseWeapon;

            if (weapon == null)
                return;

            int whichone = Utility.RandomMinMax(1, 3);

            if (whichone == 3 && m_Mobile.Skills[SkillName.Bushido].Value >= 60.0)
                spell = new Evasion(m_Mobile, null);
            else if (whichone >= 2 && m_Mobile.Skills[SkillName.Bushido].Value >= 40.0)
                spell = new CounterAttack(m_Mobile, null);
            else if (whichone >= 1 && m_Mobile.Skills[SkillName.Bushido].Value >= 25.0)
                spell = new Confidence(m_Mobile, null);

            if (spell != null)
                spell.Cast();
        }

        public void UseBushidoMove()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(2117, "Using a samurai or special move strike");

            Mobile comb = m_Mobile.Combatant;

            if (comb == null)
                return;

            BaseWeapon weapon = m_Mobile.Weapon as BaseWeapon;

            if (weapon == null)
                return;

            int whichone = Utility.RandomMinMax(1, 4);

            if (whichone == 4 && m_Mobile.Skills[SkillName.Bushido].Value >= 70.0)
                SamuraiMove.SetCurrentMove(m_Mobile, new MomentumStrike());
            else if (whichone >= 3 && m_Mobile.Skills[SkillName.Bushido].Value >= 50.0)
                SamuraiMove.SetCurrentMove(m_Mobile, new LightningStrike());
            else if (
                whichone >= 2
                && m_Mobile.Skills[SkillName.Bushido].Value >= 25.0
                && comb.Hits <= m_Mobile.DamageMin
            )
                SamuraiMove.SetCurrentMove(m_Mobile, new HonorableExecution());
            else if (
                whichone >= 2
                && m_Mobile.Skills[SkillName.Tactics].Value >= 90.0
                && weapon != null
            )
                WeaponAbility.SetCurrentAbility(m_Mobile, weapon.PrimaryAbility);
            else if (m_Mobile.Skills[SkillName.Tactics].Value >= 60.0 && weapon != null)
                WeaponAbility.SetCurrentAbility(m_Mobile, weapon.SecondaryAbility);
        }
    }
}
