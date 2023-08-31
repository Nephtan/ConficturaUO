// Created by Peoharen
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.DeathKnight;
using Server.Spells.Elementalism;
using Server.Spells.Herbalist;
using Server.Spells.HolyMan;
using Server.Spells.Jedi;
using Server.Spells.Syth;
using Server.Spells.Jester;
using Server.Spells.Magical;
using Server.Spells.Mystic;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Research;
using Server.Spells.Shinobi;
using Server.Spells.Song;
using Server.Spells.Undead;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;
using Server.Targeting;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public DateTime m_NextShurikenThrow;
        public int m_SmokeBombs;
        public bool m_HasSetSmokeBombs;

        public void NinjitsuPower()
        {
            if (!m_HasSetSmokeBombs)
            {
                m_HasSetSmokeBombs = true;
                m_SmokeBombs = Utility.RandomMinMax(3, 5);
            }

            Spell spell = null;

            if (m_Mobile.Hidden)
                GetHiddenNinjaMove();
            else if (0.2 > Utility.RandomDouble())
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(995, "Casting Mirror Image");

                spell = new MirrorImage(m_Mobile, null);
            }
            else
                GetNinjaMove();

            if (spell != null)
                spell.Cast();

            if (
                DateTime.Now > m_NextShurikenThrow
                && m_Mobile.Combatant != null
                && m_Mobile.InRange(m_Mobile.Combatant, 12)
            )
            {
                if (m_Mobile.Debug)
                    m_Mobile.Say(995, "Throwing a shuriken");

                m_Mobile.Direction = m_Mobile.GetDirectionTo(m_Mobile.Combatant);

                if (m_Mobile.Body.IsHuman)
                    m_Mobile.Animate(m_Mobile.Mounted ? 26 : 9, 7, 1, true, false, 0);

                m_Mobile.PlaySound(0x23A);
                m_Mobile.MovingEffect(m_Mobile.Combatant, 0x27AC, 1, 0, false, false);

                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(ShurikenDamage));

                m_NextShurikenThrow =
                    DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
            }
        }

        public void GetHiddenNinjaMove()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(995, "Using a hidden ninja strike");

            int whichone = Utility.RandomMinMax(1, 3);

            if (whichone == 3 && m_Mobile.Skills[SkillName.Ninjitsu].Value >= 80.0)
                NinjaMove.SetCurrentMove(m_Mobile, new KiAttack());
            else if (whichone >= 2 && m_Mobile.Skills[SkillName.Ninjitsu].Value >= 30.0)
                NinjaMove.SetCurrentMove(m_Mobile, new SurpriseAttack());
            else if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 20.0)
                NinjaMove.SetCurrentMove(m_Mobile, new Backstab());
        }

        public void GetNinjaMove()
        {
            if (m_Mobile.Debug)
                m_Mobile.Say(995, "Using a ninja strike");

            int whichone = Utility.RandomMinMax(1, 3);

            if (whichone == 3 && m_Mobile.Skills[SkillName.Ninjitsu].Value >= 85.0)
                NinjaMove.SetCurrentMove(m_Mobile, new DeathStrike());
            else if (whichone >= 2 && m_Mobile.Skills[SkillName.Ninjitsu].Value >= 60.0)
                NinjaMove.SetCurrentMove(m_Mobile, new FocusAttack());
            else
                UseWeaponStrike();
        }

        public virtual void ShurikenDamage()
        {
            Mobile target = m_Mobile.Combatant;

            if (target != null)
            {
                m_Mobile.DoHarmful(target);
                AOS.Damage(target, m_Mobile, Utility.RandomMinMax(3, 5), 100, 0, 0, 0, 0);

                if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 120.0)
                    target.ApplyPoison(m_Mobile, Poison.Lethal);
                else if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 101.0)
                    target.ApplyPoison(m_Mobile, Poison.Deadly);
                else if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 100.0)
                    target.ApplyPoison(m_Mobile, Poison.Greater);
                else if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 70.0)
                    target.ApplyPoison(m_Mobile, Poison.Regular);
                else if (m_Mobile.Skills[SkillName.Ninjitsu].Value >= 50.0)
                    target.ApplyPoison(m_Mobile, Poison.Lesser);
            }
        }
    }
}
