// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Syth;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void SythPower()
        {
            Spell spell = null;

            if (m_Mobile.Poisoned || m_Mobile.Hits < m_Mobile.HitsMax * 60 / 100)
            {
                spell = new Absorption(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null && m_Mobile.Hits < m_Mobile.HitsMax * 80 / 100)
            {
                spell = new DrainLife(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                if (m_Mobile.Skills[SkillName.Psychology].Value > 80.0)
                    spell = new SythLightning(m_Mobile, null);
                else
                    spell = new PsychicBlast(m_Mobile, null);
            }
            else
            {
                spell = new Speed(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}