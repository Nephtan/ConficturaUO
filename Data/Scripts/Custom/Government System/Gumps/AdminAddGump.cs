﻿using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Regions;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Gumps
{
    public class AdminAddGump : Gump
    {
        private ArrayList m_List;
        private int m_ListPage;
        private Mobile m_mobile;
        private ArrayList m_CountList;

        public AdminAddGump(int listPage, Mobile m, ArrayList list, ArrayList count)
            : base(0, 0)
        {
            m_List = list;
            m_ListPage = listPage;
            m_CountList = count;
            m_mobile = m;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            ArrayList cities = new ArrayList();
            ArrayList fell = new ArrayList();
            ArrayList tram = new ArrayList();
            ArrayList ilsh = new ArrayList();
            ArrayList mala = new ArrayList();
            ArrayList toku = new ArrayList();

            if (m_List == null)
            {
                foreach (Item i in World.Items.Values)
                {
                    if (i is CityManagementStone)
                    {
                        CityManagementStone s = (CityManagementStone)i;

                        cities.Add(s);

                        if (s.Map == Map.Lodor)
                            fell.Add(s);

                        if (s.Map == Map.Sosaria)
                            tram.Add(s);

                        if (s.Map == Map.Underworld)
                            ilsh.Add(s);

                        if (s.Map == Map.SerpentIsland)
                            mala.Add(s);

                        if (s.Map == Map.IslesDread)
                            toku.Add(s);
                    }
                }

                m_List = cities;
            }

            AddBackground(12, 10, 389, 365, 5120);
            AddImageTiled(24, 19, 368, 179, 3504);
            AddAlphaRegion(24, 19, 368, 179);
            AddImageTiled(16, 234, 384, 8, 5121);
            AddLabel(25, 245, 1149, @"Total Cities: " + cities.Count.ToString());
            AddLabel(25, 265, 1149, @"Lodor: " + fell.Count.ToString());
            AddLabel(25, 285, 1149, @"Sosaria: " + tram.Count.ToString());
            AddLabel(25, 305, 1149, @"Underworld: " + ilsh.Count.ToString());
            AddLabel(25, 325, 1149, @"SerpentIsland: " + mala.Count.ToString());
            AddLabel(25, 345, 1149, @"IslesDread: " + toku.Count.ToString());

            if (listPage > 0)
            {
                AddButton(35, 205, 4014, 4015, 1, GumpButtonType.Reply, 0);
                AddLabel(75, 205, 1149, @"Last Page");
            }

            if ((listPage + 1) * 7 < m_List.Count)
            {
                AddButton(349, 205, 4005, 4006, 2, GumpButtonType.Reply, 0);
                AddLabel(277, 205, 1149, @"Next Page");
            }

            int k = 0;

            for (
                int i = 0, j = 0, index = ((listPage * 7) + k);
                i < 7 && index >= 0 && index < m_List.Count && j >= 0;
                ++i, ++j, ++index
            )
            {
                Item item = m_List[index] as Item;

                if (item is CityManagementStone)
                {
                    CityManagementStone citystone = (CityManagementStone)item;

                    int offset = 30 + (i * 25);

                    AddButton(35, offset, 4005, 4006, 100 + index, GumpButtonType.Reply, 0);
                    AddLabel(75, offset, 1149, citystone.CityName.ToString());
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (from == null)
                return;

            if (info.ButtonID == 1) // Previous page
            {
                if (m_ListPage > 0)
                    from.SendGump(new AdminAddGump(m_ListPage - 1, m_mobile, m_List, m_CountList));
            }

            if (info.ButtonID == 2) // Next page
            {
                if ((m_ListPage + 1) * 7 < m_List.Count)
                    from.SendGump(new AdminAddGump(m_ListPage + 1, m_mobile, m_List, m_CountList));
            }

            if (info.ButtonID >= 100) // Goto City
            {
                CityManagementStone city = m_List[info.ButtonID - 100] as CityManagementStone;
                city.AdminDoJoin(from, m_mobile);
            }
        }
    }
}
