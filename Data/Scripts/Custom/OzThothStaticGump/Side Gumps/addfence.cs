using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps
{
    public class AddFenceGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddFence",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddFence_OnCommand)
            );
        }

        [Usage("AddFence")]
        [Description("Displays a menu from which you can interactively add Fences.")]
        public static void AddFence_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddFenceGump));
            e.Mobile.SendGump(new AddFenceGump());
        }

        public static FenceInfo[] m_Types = new FenceInfo[]
        {
            #region Fences

            new FenceInfo(2007),
            new FenceInfo(2008),
            new FenceInfo(2081),
            new FenceInfo(2082),
            new FenceInfo(2083),
            new FenceInfo(2101),
            new FenceInfo(2102),
            new FenceInfo(2103),
            new FenceInfo(2104),
            new FenceInfo(2121),
            new FenceInfo(2122),
            new FenceInfo(2123),
            new FenceInfo(2140),
            new FenceInfo(2141),
            new FenceInfo(2142),
            new FenceInfo(2143),
            new FenceInfo(2144),
            new FenceInfo(2145),
            new FenceInfo(2146),
            new FenceInfo(2147),
            new FenceInfo(2148),
            new FenceInfo(2149),
            new FenceInfo(2167),
            new FenceInfo(2168),
            new FenceInfo(2174),
            new FenceInfo(2175),
            new FenceInfo(2176),
            new FenceInfo(2177),
            new FenceInfo(2178),
            new FenceInfo(2179),
            new FenceInfo(2180),
            new FenceInfo(2181),
            new FenceInfo(2182),
            new FenceInfo(2183),
            new FenceInfo(2184),
            new FenceInfo(2185),
            new FenceInfo(2186),
            new FenceInfo(2187),
            new FenceInfo(2222),
            new FenceInfo(2223),
            new FenceInfo(2224),
            new FenceInfo(2225),
            new FenceInfo(2226),
            new FenceInfo(2227),
            new FenceInfo(2228),
            new FenceInfo(2229),
            new FenceInfo(2230),
            new FenceInfo(2231),
            new FenceInfo(2232),
            new FenceInfo(2233),
            new FenceInfo(2234),
            new FenceInfo(2235),
            new FenceInfo(2236),
            new FenceInfo(2237),
            new FenceInfo(2238),
            new FenceInfo(2239),
            new FenceInfo(2240),
            new FenceInfo(2241),
            new FenceInfo(2242),
            new FenceInfo(2243),
            new FenceInfo(2244),
            new FenceInfo(2245),
            new FenceInfo(2246),
            new FenceInfo(2247),
            new FenceInfo(2248),
            new FenceInfo(2249),
            new FenceInfo(2250),
            new FenceInfo(2251),
            new FenceInfo(2252),
            new FenceInfo(2253),
            new FenceInfo(2254),
            new FenceInfo(2283),
            new FenceInfo(2284),
            new FenceInfo(2285),
            new FenceInfo(2286),
            new FenceInfo(2287),
            new FenceInfo(2288),
            new FenceInfo(2289),
            new FenceInfo(2290),
            new FenceInfo(2291),
            new FenceInfo(2292),
            new FenceInfo(2293),
            new FenceInfo(2294),
            new FenceInfo(2295),
            new FenceInfo(2296),
            new FenceInfo(2297),
            new FenceInfo(2298),
            new FenceInfo(2299),
            new FenceInfo(2300),
            new FenceInfo(20316),
            new FenceInfo(20317),
            new FenceInfo(20318),
            new FenceInfo(20319),
            new FenceInfo(20320),
            new FenceInfo(20321)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddFenceGump()
            : this(0) { }

        public AddFenceGump(int page)
            : base(0, 0)
        {
            int pageCount = 1 + (m_Types.Length / 10);

            if (page >= pageCount)
                page = pageCount - 1;
            else if (page < 0)
                page = 0;

            m_Page = page;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(0, 0, 600, 180, 3500);

            AddHtmlLocalized(15, 15, 60, 20, 1042971, page.ToString(), 0x1, false, false); // #

            AddHtmlLocalized(20, 38, 60, 20, 1043353, 0x1, false, false); // Next

            if (page + 1 < pageCount)
                AddButton(15, 55, 0xFA5, 0xFA7, 10000 + (page + 1), GumpButtonType.Reply, 0);
            else
                AddButton(15, 55, 0xFA5, 0xFA7, 10000, GumpButtonType.Reply, 0);

            AddHtmlLocalized(20, 93, 60, 20, 1011393, 0x1, false, false); // Back

            if (page > 0)
                AddButton(15, 110, 0xFAE, 0xFB0, 10000 + (page - 1), GumpButtonType.Reply, 0);
            else
                AddButton(15, 110, 0xFAE, 0xFB0, 10000, GumpButtonType.Reply, 0);

            for (int i = 0; i < 10; ++i)
            {
                int index = (page * 10) + i;
                if (index >= m_Types.Length)
                    break;

                int button = 1000000 + index;
                int offset = (i + 1) * 50;

                if (m_Types[index].m_BaseID > 0)
                {
                    AddButton(
                        25 + offset,
                        20,
                        0x2624,
                        0x2625,
                        button,
                        GumpButtonType.Reply,
                        m_Types[index].m_BaseID
                    );
                    AddItem(15 + offset, 50, m_Types[index].m_BaseID);
                }
                else
                {
                    AddImage(25 + offset, 20, 0x2625, 900);
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int button = info.ButtonID;

            if (button <= 0)
                return;

            int page = m_Page;

            if (button >= 1000000)
            {
                button -= 1000000;

                CommandSystem.Handle(
                    from,
                    String.Format(
                        "{0}Tile Static {1}",
                        CommandSystem.Prefix,
                        m_Types[button].m_BaseID
                    )
                );
            }
            else if (button >= 10000)
            {
                button -= 10000;

                page = button;
            }

            from.SendGump(new AddFenceGump(page));
        }

        public class FenceInfo
        {
            public int m_BaseID;

            public FenceInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
