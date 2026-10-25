using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Prompts
{
    public class CityTaxesIncomePrompt : Prompt
    {
        private CityManagementStone m_Stone;
        private Mobile m_From;
        private bool m_Responded;

        public CityTaxesIncomePrompt(CityManagementStone stone, Mobile from)
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
            else if (amount < 0 || amount > 100)
                from.SendMessage("Income Tax can only range from 0 - 100");
            else
            {
                m_Stone.IncomeTax = amount;
                if (m_Stone.Vendors != null)
                {
                    foreach (Mobile mob in m_Stone.Vendors)
                    {
                        CityPlayerVendor vendor = mob as CityPlayerVendor;
                        if (vendor != null && !vendor.Deleted)
                            vendor.TaxRate = amount;
                    }
                }
                from.SendMessage("You have set the cities income tax to {0}.", amount);
                if (m_Stone.Citizens != null)
                {
                    foreach (Mobile citizen in m_Stone.Citizens)
                        if (citizen != null && !citizen.Deleted)
                            citizen.SendMessage(53, "The cities income tax has changed to {0}.", amount);
                }
            }
            Reopen(from);
        }
    }
}
