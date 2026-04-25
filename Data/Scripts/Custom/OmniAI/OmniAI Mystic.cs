// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Mystic;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void MysticPower()
        {
            Spell spell = null;

            if (m_Mobile.Poisoned)
            {
                spell = new PurityOfBody(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 65 / 100)
            {
                spell = new GentleTouch(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null && m_Mobile.Hits < m_Mobile.HitsMax * 85 / 100)
            {
                spell = new PsychicWall(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                if (m_Mobile.Skills[SkillName.FistFighting].Value > 80.0)
                    spell = new QuiveringPalm(m_Mobile, null);
                else
                    spell = new PsionicBlast(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}