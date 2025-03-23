using System;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class ConfirmRemoveGump : Gump
    {
        private Mobile m_From;
        private Item m_Item;

        public ConfirmRemoveGump(Mobile from, Item item) : base(100, 100)
        {
            m_From = from;
            m_Item = item;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(0, 0, 300, 200, 0xA28);

            AddHtml(20, 20, 260, 80,
                "Are you sure you want to remove this item?",
                true, true);

            AddButton(50, 110, 0xFB1, 0xFB2, 1, GumpButtonType.Reply, 0);
            AddHtml(90, 110, 70, 20, "Yes", false, false);

            AddButton(180, 110, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
            AddHtml(220, 110, 70, 20, "No", false, false);

            AddCheck(50, 140, 0xD2, 0xD3, false, 1);
            AddHtml(80, 140, 200, 20, "Do not ask again for 10 minutes", false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1) // Yes
            {
                if (info.IsSwitched(1)) // "Do not ask again" is checked
                {
                    PlayerMobile pm = m_From as PlayerMobile;

                    if (m_Item is LawnItem)
                    {
                        ((LawnItem)m_Item).LastConfirmTime = DateTime.Now;
                        if (pm != null)
                        {
                            pm.LastDecoConfirmTime = DateTime.Now;
                        } // Update the player's global time
                    }
                    else if (m_Item is ShantyItem)
                    {
                        ((ShantyItem)m_Item).LastConfirmTime = DateTime.Now;
                        if (pm != null)
                        {
                            pm.LastDecoConfirmTime = DateTime.Now;
                        }
                    }
                }

                m_Item.Delete();
            }
        }
    }
}