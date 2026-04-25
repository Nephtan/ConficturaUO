// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Research;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void ResearchPower()
        {
            Spell spell = null;

            if (m_Mobile.Poisoned)
            {
                spell = new ResearchIntervention(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 60 / 100)
            {
                spell = new ResearchHealingTouch(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                spell = new ResearchFlameBolt(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}