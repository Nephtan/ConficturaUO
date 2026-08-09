using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public interface IGiftable
    {
        string Gifter { get; set; }
        string How { get; set; }
        Mobile Owner { get; set; }
        int Points { get; set; }
    }
}

namespace Server.ContextMenus
{
    public class GiftInfoEntry : ContextMenuEntry
    {
        private Item m_Item;
        private Mobile m_From;
        private GiftAttributeCategory m_Cat;

        public GiftInfoEntry(Mobile from, Item item, GiftAttributeCategory cat)
            : base(132, 3)
        {
            m_From = from;
            m_Item = item;
            m_Cat = cat;
        }

        public override void OnClick()
        {
            if (m_From == null || m_From.Deleted || m_Item == null || m_Item.Deleted)
                return;

            m_From.CloseGump(typeof(GiftGump));
            m_From.SendGump(new GiftGump(m_From, m_Item, m_Cat));
        }
    }
}
