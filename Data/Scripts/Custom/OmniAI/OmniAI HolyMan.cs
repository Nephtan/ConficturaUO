// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.HolyMan;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void HolyManPower()
        {
            Spell spell = null;

            if (m_Mobile.Poisoned)
            {
                spell = new PurgeSpell(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 70 / 100)
            {
                spell = new TouchOfLifeSpell(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null && m_Mobile.Hits < m_Mobile.HitsMax * 90 / 100)
            {
                spell = new SacredBoonSpell(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                spell = new SmiteSpell(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}