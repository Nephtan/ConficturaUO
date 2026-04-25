// Created by Peoharen, extended for additional spell schools
using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Song;

namespace Server.Mobiles
{
    public partial class OmniAI : BaseAI
    {
        public void BardPower()
        {
            Spell spell = null;

            if (m_Mobile.Hits < m_Mobile.HitsMax * 70 / 100)
            {
                spell = new ArmysPaeonSong(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null && m_Mobile.Combatant.Poisoned)
            {
                spell = new PoisonThrenodySong(m_Mobile, null);
            }
            else if (m_Mobile.Combatant != null)
            {
                spell = new FoeRequiemSong(m_Mobile, null);
            }
            else
            {
                spell = new MagesBalladSong(m_Mobile, null);
            }

            if (spell != null)
            {
                m_NextCastTime = DateTime.Now + spell.GetCastDelay() + spell.GetCastRecovery();
                spell.Cast();
            }
        }
    }
}