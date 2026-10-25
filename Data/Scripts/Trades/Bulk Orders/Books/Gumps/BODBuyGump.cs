using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
    public class BODBuyGump : Gump
    {
        private PlayerMobile m_From;
        private BulkOrderBook m_Book;
        private object m_Object;
        private int m_Price;
        private int m_Page;
        private bool m_Responded;

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (m_Responded || sender == null || info == null || sender.Mobile != m_From
                || m_From == null || m_From.Deleted || m_Book == null || m_Book.Deleted)
                return;

            m_Responded = true;
            if (info.ButtonID != 2)
            {
                m_From.SendLocalizedMessage(503207); // Cancelled purchase.
                return;
            }

            PlayerVendor pv = m_Book.RootParent as PlayerVendor;
            if (pv == null || pv.Deleted || !m_Book.Entries.Contains(m_Object))
            {
                m_From.SendLocalizedMessage(1062382);
                return;
            }

            VendorItem vi = pv.GetVendorItem(m_Book);
            int price = 0;
            if (vi != null && !vi.IsForSale)
            {
                if (m_Object is BOBLargeEntry)
                    price = ((BOBLargeEntry)m_Object).Price;
                else if (m_Object is BOBSmallEntry)
                    price = ((BOBSmallEntry)m_Object).Price;
            }
            if (price != m_Price)
            {
                pv.SayTo(m_From, "The price has been changed. You may offer to purchase the item again.");
                return;
            }
            if (price <= 0 || (long)pv.HoldGold + price > Int32.MaxValue)
            {
                pv.SayTo(m_From, 1062382);
                return;
            }

            Item item = null;
            bool purchased = false;
            try
            {
                if (m_Object is BOBLargeEntry)
                    item = ((BOBLargeEntry)m_Object).Reconstruct();
                else if (m_Object is BOBSmallEntry)
                    item = ((BOBSmallEntry)m_Object).Reconstruct();

                if (item == null)
                {
                    m_From.SendMessage("Internal error. The bulk order deed could not be reconstructed.");
                    return;
                }

                Container pack = m_From.Backpack;
                if (pack == null || !pack.CheckHold(m_From, item, true, true, 0, item.PileWeight + item.TotalWeight))
                {
                    pv.SayTo(m_From, 503204);
                    return;
                }
                if (pack.GetAmount(typeof(Gold)) < price && Banker.GetBalance(m_From) < price)
                {
                    pv.SayTo(m_From, 503205);
                    return;
                }

                // This newly reconstructed, nonstackable deed is owned here until both steps succeed.
                if (!pack.TryDropItem(m_From, item, false) || item.Deleted || !item.IsChildOf(pack))
                {
                    pv.SayTo(m_From, 503204);
                    return;
                }
                if (!pack.ConsumeTotal(typeof(Gold), price) && !Banker.Withdraw(m_From, price))
                {
                    pv.SayTo(m_From, 503205);
                    return;
                }

                purchased = true;
                m_Book.Entries.Remove(m_Object);
                m_Book.InvalidateProperties();
                pv.HoldGold += price;
                m_From.SendLocalizedMessage(1045152);
                if (m_Book.Entries.Count / 5 < m_Book.ItemCount)
                {
                    m_Book.ItemCount--;
                    m_Book.InvalidateItems();
                }
            }
            finally
            {
                if (!purchased && item != null && !item.Deleted)
                    item.Delete();
            }

            if (m_Book.Entries.Count > 0)
                m_From.SendGump(new BOBGump(m_From, m_Book, m_Page, null));
            else
                m_From.SendLocalizedMessage(1062381);
        }

        public BODBuyGump(PlayerMobile from, BulkOrderBook book, object obj, int page, int price)
            : base(100, 200)
        {
            m_From = from;
            m_Book = book;
            m_Object = obj;
            m_Price = price;
            m_Page = page;

            AddPage(0);

            AddBackground(100, 10, 300, 150, 5054);

            AddHtmlLocalized(125, 20, 250, 24, 1019070, false, false); // You have agreed to purchase:
            AddHtmlLocalized(125, 45, 250, 24, 1045151, false, false); // a bulk order deed

            AddHtmlLocalized(125, 70, 250, 24, 1019071, false, false); // for the amount of:
            AddLabel(125, 95, 0, price.ToString());

            AddButton(250, 130, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(282, 130, 100, 24, 1011012, false, false); // CANCEL

            AddButton(120, 130, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddHtmlLocalized(152, 130, 100, 24, 1011036, false, false); // OKAY
        }
    }
}
