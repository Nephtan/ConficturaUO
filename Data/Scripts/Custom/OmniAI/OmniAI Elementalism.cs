// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Elementalism;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void ElementalismPower()
        {
            Spell spell = null;

            if (m_Mobile.Poisoned)
            {
                spell = new Elemental_Sanctuary(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 70 / 100)
            {
                spell = new Elemental_Mend(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 90 / 100)
            {
                spell = new Elemental_Armor(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                spell = new Elemental_Bolt(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}