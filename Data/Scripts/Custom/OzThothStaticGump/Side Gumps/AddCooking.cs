/* Credit is given where it is due.  With Vorspire's help, these gumps stay on the page you are on when you select an item.  For the life of me I couldn't figure it out.
Espcevan created the original AddWall and AddStair gump which helped with creating the other gumps.  AddDoor and AddSign are part of ServUO's scripts but are added into this
handy little tool.

TheArt came up with the idea to put make a tool that can help those who can't use Pandora or other third party tools due to various reasons.*/
using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps
{
    public class AddCookingStaticGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddCookingStatic",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddCookingStatic_OnCommand)
            );
        }

        [Usage("AddCookingStatic")]
        [Description("Displays a menu from which you can interactively add Cooking Statics.")]
        public static void AddCookingStatic_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddCookingStaticGump));
            e.Mobile.SendGump(new AddCookingStaticGump());
        }

        public static CookingStaticInfo[] m_Types = new CookingStaticInfo[]
        {
            #region CookingStatics

            new CookingStaticInfo(539),
            new CookingStaticInfo(2416),
            new CookingStaticInfo(2417),
            new CookingStaticInfo(2418),
            new CookingStaticInfo(2419),
            new CookingStaticInfo(2420),
            new CookingStaticInfo(2421),
            new CookingStaticInfo(2422),
            new CookingStaticInfo(2423),
            new CookingStaticInfo(2424),
            new CookingStaticInfo(2425),
            new CookingStaticInfo(2426),
            new CookingStaticInfo(2427),
            new CookingStaticInfo(2428),
            new CookingStaticInfo(2429),
            new CookingStaticInfo(2430),
            new CookingStaticInfo(2431),
            new CookingStaticInfo(2444),
            new CookingStaticInfo(2445),
            new CookingStaticInfo(2446),
            new CookingStaticInfo(2448),
            new CookingStaticInfo(2449),
            new CookingStaticInfo(2450),
            new CookingStaticInfo(2451),
            new CookingStaticInfo(2452),
            new CookingStaticInfo(2453),
            new CookingStaticInfo(2454),
            new CookingStaticInfo(2456),
            new CookingStaticInfo(2457),
            new CookingStaticInfo(2458),
            new CookingStaticInfo(2459),
            new CookingStaticInfo(2460),
            new CookingStaticInfo(2461),
            new CookingStaticInfo(2462),
            new CookingStaticInfo(2463),
            new CookingStaticInfo(2464),
            new CookingStaticInfo(2465),
            new CookingStaticInfo(2466),
            new CookingStaticInfo(2467),
            new CookingStaticInfo(2468),
            new CookingStaticInfo(2469),
            new CookingStaticInfo(2470),
            new CookingStaticInfo(2471),
            new CookingStaticInfo(2476),
            new CookingStaticInfo(2477),
            new CookingStaticInfo(2478),
            new CookingStaticInfo(2479),
            new CookingStaticInfo(2480),
            new CookingStaticInfo(2481),
            new CookingStaticInfo(2483),
            new CookingStaticInfo(2484),
            new CookingStaticInfo(2485),
            new CookingStaticInfo(2486),
            new CookingStaticInfo(2487),
            new CookingStaticInfo(2488),
            new CookingStaticInfo(2489),
            new CookingStaticInfo(2489),
            new CookingStaticInfo(2490),
            new CookingStaticInfo(2491),
            new CookingStaticInfo(2492),
            new CookingStaticInfo(2493),
            new CookingStaticInfo(2494),
            new CookingStaticInfo(2495),
            new CookingStaticInfo(2496),
            new CookingStaticInfo(2497),
            new CookingStaticInfo(2498),
            new CookingStaticInfo(2499),
            new CookingStaticInfo(2500),
            new CookingStaticInfo(2501),
            new CookingStaticInfo(2502),
            new CookingStaticInfo(2503),
            new CookingStaticInfo(2504),
            new CookingStaticInfo(2505),
            new CookingStaticInfo(2506),
            new CookingStaticInfo(2507),
            new CookingStaticInfo(2508),
            new CookingStaticInfo(2509),
            new CookingStaticInfo(2510),
            new CookingStaticInfo(2511),
            new CookingStaticInfo(2512),
            new CookingStaticInfo(2513),
            new CookingStaticInfo(2514),
            new CookingStaticInfo(2515),
            new CookingStaticInfo(2516),
            new CookingStaticInfo(2517),
            new CookingStaticInfo(2518),
            new CookingStaticInfo(2519),
            new CookingStaticInfo(2520),
            new CookingStaticInfo(2521),
            new CookingStaticInfo(2522),
            new CookingStaticInfo(2523),
            new CookingStaticInfo(2524),
            new CookingStaticInfo(2525),
            new CookingStaticInfo(2526),
            new CookingStaticInfo(2527),
            new CookingStaticInfo(2528),
            new CookingStaticInfo(2529),
            new CookingStaticInfo(2530),
            new CookingStaticInfo(2531),
            new CookingStaticInfo(2532),
            new CookingStaticInfo(2533),
            new CookingStaticInfo(2534),
            new CookingStaticInfo(2535),
            new CookingStaticInfo(2536),
            new CookingStaticInfo(2537),
            new CookingStaticInfo(2538),
            new CookingStaticInfo(2539),
            new CookingStaticInfo(2540),
            new CookingStaticInfo(2541),
            new CookingStaticInfo(2542),
            new CookingStaticInfo(2543),
            new CookingStaticInfo(2544),
            new CookingStaticInfo(2545),
            new CookingStaticInfo(2546),
            new CookingStaticInfo(2547),
            new CookingStaticInfo(2548),
            new CookingStaticInfo(2549),
            new CookingStaticInfo(2550),
            new CookingStaticInfo(2551),
            new CookingStaticInfo(2552),
            new CookingStaticInfo(2553),
            new CookingStaticInfo(2554),
            new CookingStaticInfo(2585),
            new CookingStaticInfo(2590),
            new CookingStaticInfo(3652),
            new CookingStaticInfo(3653),
            new CookingStaticInfo(3654),
            new CookingStaticInfo(3655),
            new CookingStaticInfo(3656),
            new CookingStaticInfo(3657),
            new CookingStaticInfo(3658),
            new CookingStaticInfo(3659),
            new CookingStaticInfo(3660),
            new CookingStaticInfo(3661),
            new CookingStaticInfo(3662),
            new CookingStaticInfo(3663),
            new CookingStaticInfo(3704),
            new CookingStaticInfo(4153),
            new CookingStaticInfo(4154),
            new CookingStaticInfo(4155),
            new CookingStaticInfo(4156),
            new CookingStaticInfo(4157),
            new CookingStaticInfo(4158),
            new CookingStaticInfo(4159),
            new CookingStaticInfo(4160),
            new CookingStaticInfo(4161),
            new CookingStaticInfo(4162),
            new CookingStaticInfo(4163),
            new CookingStaticInfo(4164),
            new CookingStaticInfo(4165),
            new CookingStaticInfo(4166),
            new CookingStaticInfo(5624),
            new CookingStaticInfo(5625),
            new CookingStaticInfo(5626),
            new CookingStaticInfo(5627),
            new CookingStaticInfo(5628),
            new CookingStaticInfo(5629),
            new CookingStaticInfo(5630),
            new CookingStaticInfo(5631),
            new CookingStaticInfo(5632),
            new CookingStaticInfo(5633),
            new CookingStaticInfo(5634),
            new CookingStaticInfo(5635),
            new CookingStaticInfo(5636),
            new CookingStaticInfo(5637),
            new CookingStaticInfo(5638),
            new CookingStaticInfo(5639),
            new CookingStaticInfo(5640),
            new CookingStaticInfo(5641),
            new CookingStaticInfo(5642),
            new CookingStaticInfo(5643),
            new CookingStaticInfo(5644),
            new CookingStaticInfo(5917),
            new CookingStaticInfo(5918),
            new CookingStaticInfo(5919),
            new CookingStaticInfo(5920),
            new CookingStaticInfo(5921),
            new CookingStaticInfo(5922),
            new CookingStaticInfo(5923),
            new CookingStaticInfo(5924),
            new CookingStaticInfo(5925),
            new CookingStaticInfo(5926),
            new CookingStaticInfo(5927),
            new CookingStaticInfo(5928),
            new CookingStaticInfo(5929),
            new CookingStaticInfo(5930),
            new CookingStaticInfo(5931),
            new CookingStaticInfo(5932),
            new CookingStaticInfo(5933),
            new CookingStaticInfo(6465),
            new CookingStaticInfo(6466),
            new CookingStaticInfo(6467),
            new CookingStaticInfo(7701),
            new CookingStaticInfo(7702),
            new CookingStaticInfo(7703),
            new CookingStaticInfo(7704),
            new CookingStaticInfo(7705),
            new CookingStaticInfo(7706),
            new CookingStaticInfo(7707),
            new CookingStaticInfo(7708),
            new CookingStaticInfo(7709),
            new CookingStaticInfo(7710),
            new CookingStaticInfo(7711),
            new CookingStaticInfo(7811),
            new CookingStaticInfo(7812),
            new CookingStaticInfo(7813),
            new CookingStaticInfo(7814),
            new CookingStaticInfo(7815),
            new CookingStaticInfo(7816),
            new CookingStaticInfo(7817),
            new CookingStaticInfo(7818),
            new CookingStaticInfo(7819),
            new CookingStaticInfo(7820),
            new CookingStaticInfo(7821),
            new CookingStaticInfo(7822),
            new CookingStaticInfo(7823),
            new CookingStaticInfo(7824),
            new CookingStaticInfo(7825),
            new CookingStaticInfo(7826),
            new CookingStaticInfo(7827),
            new CookingStaticInfo(7828),
            new CookingStaticInfo(7829),
            new CookingStaticInfo(7830),
            new CookingStaticInfo(7831),
            new CookingStaticInfo(7832),
            new CookingStaticInfo(7833),
            new CookingStaticInfo(8061),
            new CookingStaticInfo(8062),
            new CookingStaticInfo(8063),
            new CookingStaticInfo(8064),
            new CookingStaticInfo(8065),
            new CookingStaticInfo(8066),
            new CookingStaticInfo(8067),
            new CookingStaticInfo(8068),
            new CookingStaticInfo(8069),
            new CookingStaticInfo(8070),
            new CookingStaticInfo(8071),
            new CookingStaticInfo(8072),
            new CookingStaticInfo(8073),
            new CookingStaticInfo(8074),
            new CookingStaticInfo(8075),
            new CookingStaticInfo(8076),
            new CookingStaticInfo(8077),
            new CookingStaticInfo(8078),
            new CookingStaticInfo(8079),
            new CookingStaticInfo(8080),
            new CookingStaticInfo(8081),
            new CookingStaticInfo(8082),
            new CookingStaticInfo(8083),
            new CookingStaticInfo(8084),
            new CookingStaticInfo(8085),
            new CookingStaticInfo(8086),
            new CookingStaticInfo(8087),
            new CookingStaticInfo(8088),
            new CookingStaticInfo(8089),
            new CookingStaticInfo(8090),
            new CookingStaticInfo(8091),
            new CookingStaticInfo(8092),
            new CookingStaticInfo(8093),
            new CookingStaticInfo(8094),
            new CookingStaticInfo(8314),
            new CookingStaticInfo(8315),
            new CookingStaticInfo(9438),
            new CookingStaticInfo(9439),
            new CookingStaticInfo(9440),
            new CookingStaticInfo(9441),
            new CookingStaticInfo(9446),
            new CookingStaticInfo(9447),
            new CookingStaticInfo(9448),
            new CookingStaticInfo(9449),
            new CookingStaticInfo(9450),
            new CookingStaticInfo(9451),
            new CookingStaticInfo(10302),
            new CookingStaticInfo(10303),
            new CookingStaticInfo(10304),
            new CookingStaticInfo(10305),
            new CookingStaticInfo(10316),
            new CookingStaticInfo(10317),
            new CookingStaticInfo(10318),
            new CookingStaticInfo(10319),
            new CookingStaticInfo(10320),
            new CookingStaticInfo(11599),
            new CookingStaticInfo(11705),
            new CookingStaticInfo(11706),
            new CookingStaticInfo(12247),
            new CookingStaticInfo(12248),
            new CookingStaticInfo(15641),
            new CookingStaticInfo(15642),
            new CookingStaticInfo(15643),
            new CookingStaticInfo(15644),
            new CookingStaticInfo(15793),
            new CookingStaticInfo(18077),
            new CookingStaticInfo(18078),
            new CookingStaticInfo(18079),
            new CookingStaticInfo(18080),
            new CookingStaticInfo(18081),
            new CookingStaticInfo(18824),
            new CookingStaticInfo(18831),
            new CookingStaticInfo(19363),
            new CookingStaticInfo(19364),
            new CookingStaticInfo(19371),
            new CookingStaticInfo(19372),
            new CookingStaticInfo(19417),
            new CookingStaticInfo(19418),
            new CookingStaticInfo(19458),
            new CookingStaticInfo(19459)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddCookingStaticGump()
            : this(0) { }

        public AddCookingStaticGump(int page)
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
                        "{0}M Add Static {1}",
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

            from.SendGump(new AddCookingStaticGump(page));
        }

        public class CookingStaticInfo
        {
            public int m_BaseID;

            public CookingStaticInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
