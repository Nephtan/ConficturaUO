// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Shinobi;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void ShinobiPower()
        {
            Spell spell = null;

            if (m_Mobile.Hidden)
            {
                spell = new Deception(m_Mobile, null);
            }
            else if (m_Mobile.Hits < m_Mobile.HitsMax * 60 / 100)
            {
                spell = new FerretFlee(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null && m_Mobile.Skills[SkillName.Ninjitsu].Value > 70.0)
            {
                spell = new TigerStrength(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                spell = new MysticShuriken(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}