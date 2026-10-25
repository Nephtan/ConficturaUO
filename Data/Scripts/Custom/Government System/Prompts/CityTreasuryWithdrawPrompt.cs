using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Prompts;

namespace Server.Prompts
{
    public class CityTreasuryWithdrawPrompt : Prompt
    {
        private CityManagementStone m_Stone;
        private Mobile m_From;
        private bool m_Responded;

        public CityTreasuryWithdrawPrompt(CityManagementStone stone, Mobile from)
        {
            m_Stone = stone;
            m_From = from;
        }

        private void Reopen(Mobile from)
        {
            if (from == m_From && CityManagementGump.CanManage(m_Stone, from))
            {
                from.CloseGump(typeof(CityManagementGump));
                from.SendGump(new CityManagementGump(m_Stone, from));
            }
        }

        public override void OnCancel(Mobile from)
        {
            if (m_Responded)
                return;

            m_Responded = true;
            Reopen(from);
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (m_Responded)
                return;

            m_Responded = true;
            if (from != m_From || !CityManagementGump.CanManage(m_Stone, from))
                return;

            text = text == null ? String.Empty : text.Trim();
            if (text.Length > 40)
                text = text.Substring(0, 40);

            int amount;
            if (text.Length == 0)
            {
                Reopen(from);
                return;
            }
            if (!Int32.TryParse(text, out amount))
                from.SendMessage("You must enter a number amount.");
            else if (m_Stone.CityTreasury <= amount)
                from.SendMessage("The treasury lacks the amount you requested.");
            else if (amount <= 0)
                from.SendMessage("You must withdraw a minimal of 1 gold.");
            else if (amount > 1000000)
                from.SendMessage("You can only withdraw a maxium of 1000000 gold.");
            else
            {
                m_Stone.CityTreasury -= amount;
                from.AddToBackpack(amount <= 4999 ? (Item)new Gold(amount) : new BankCheck(amount));
                from.SendMessage("You have withdrawn {0} gold from the treasury.", amount);
                if (m_Stone.Citizens != null)
                {
                    foreach (Mobile citizen in m_Stone.Citizens)
                        if (citizen != null && !citizen.Deleted)
                            citizen.SendMessage(53, "The mayor has withdrawn {0} gold from the cities treasury.", amount);
                }
            }
            Reopen(from);
        }
    }
}
