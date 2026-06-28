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
    public class AddStairGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddStair",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddStair_OnCommand)
            );
        }

        [Usage("AddStair")]
        [Description("Displays a menu from which you can interactively add Stairs.")]
        public static void AddStair_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddStairGump));
            e.Mobile.SendGump(new AddStairGump());
        }

        public static StairInfo[] m_Types = new StairInfo[]
        {
            #region Stair

            new StairInfo(1006),
            new StairInfo(1007),
            new StairInfo(1008),
            new StairInfo(1009),
            new StairInfo(1010),
            new StairInfo(1011),
            new StairInfo(1012),
            new StairInfo(1014),
            new StairInfo(1016),
            new StairInfo(1017),
            new StairInfo(1018),
            new StairInfo(1019),
            new StairInfo(1020),
            new StairInfo(1021),
            new StairInfo(1021),
            new StairInfo(1022),
            new StairInfo(1023),
            new StairInfo(1024),
            new StairInfo(1025),
            new StairInfo(1026),
            new StairInfo(1801),
            new StairInfo(1802),
            new StairInfo(1803),
            new StairInfo(1804),
            new StairInfo(1805),
            new StairInfo(1807),
            new StairInfo(1809),
            new StairInfo(1811),
            new StairInfo(1812),
            new StairInfo(1822),
            new StairInfo(1823),
            new StairInfo(1825),
            new StairInfo(1826),
            new StairInfo(1827),
            new StairInfo(1828),
            new StairInfo(1829),
            new StairInfo(1831),
            new StairInfo(1833),
            new StairInfo(1835),
            new StairInfo(1836),
            new StairInfo(1846),
            new StairInfo(1847),
            new StairInfo(1848),
            new StairInfo(1849),
            new StairInfo(1850),
            new StairInfo(1851),
            new StairInfo(1852),
            new StairInfo(1854),
            new StairInfo(1856),
            new StairInfo(1861),
            new StairInfo(1862),
            new StairInfo(1865),
            new StairInfo(1867),
            new StairInfo(1869),
            new StairInfo(1872),
            new StairInfo(1873),
            new StairInfo(1874),
            new StairInfo(1875),
            new StairInfo(1876),
            new StairInfo(1878),
            new StairInfo(1880),
            new StairInfo(1882),
            new StairInfo(1883),
            new StairInfo(1900),
            new StairInfo(1901),
            new StairInfo(1902),
            new StairInfo(1903),
            new StairInfo(1904),
            new StairInfo(1906),
            new StairInfo(1908),
            new StairInfo(1910),
            new StairInfo(1911),
            new StairInfo(1928),
            new StairInfo(1929),
            new StairInfo(1930),
            new StairInfo(1931),
            new StairInfo(1932),
            new StairInfo(1934),
            new StairInfo(1936),
            new StairInfo(1938),
            new StairInfo(1939),
            new StairInfo(1955),
            new StairInfo(1956),
            new StairInfo(1957),
            new StairInfo(1958),
            new StairInfo(1959),
            new StairInfo(1961),
            new StairInfo(1963),
            new StairInfo(1978),
            new StairInfo(1979),
            new StairInfo(1980),
            new StairInfo(1991),
            new StairInfo(1992),
            new StairInfo(2003),
            new StairInfo(2004),
            new StairInfo(2010),
            new StairInfo(2015),
            new StairInfo(2016),
            new StairInfo(2100),
            new StairInfo(2166),
            new StairInfo(2170),
            new StairInfo(2171),
            new StairInfo(2172),
            new StairInfo(2173),
            new StairInfo(2174),
            new StairInfo(2175),
            new StairInfo(2176),
            new StairInfo(2177),
            new StairInfo(2178),
            new StairInfo(2179),
            new StairInfo(2180),
            new StairInfo(2181),
            new StairInfo(2182),
            new StairInfo(2183),
            new StairInfo(2184),
            new StairInfo(2185),
            new StairInfo(2201),
            new StairInfo(2202),
            new StairInfo(2203),
            new StairInfo(2204),
            new StairInfo(2205),
            new StairInfo(2206),
            new StairInfo(2206),
            new StairInfo(2207),
            new StairInfo(2208),
            new StairInfo(2209),
            new StairInfo(2210),
            new StairInfo(2211),
            new StairInfo(2212),
            new StairInfo(2213),
            new StairInfo(2214),
            new StairInfo(2222),
            new StairInfo(2223),
            new StairInfo(2224),
            new StairInfo(2225),
            new StairInfo(2226),
            new StairInfo(2227),
            new StairInfo(2228),
            new StairInfo(2229),
            new StairInfo(2230),
            new StairInfo(2231),
            new StairInfo(2232),
            new StairInfo(2233),
            new StairInfo(2234),
            new StairInfo(2235),
            new StairInfo(2236),
            new StairInfo(2237),
            new StairInfo(2238),
            new StairInfo(2239),
            new StairInfo(2240),
            new StairInfo(2241),
            new StairInfo(2242),
            new StairInfo(2243),
            new StairInfo(2244),
            new StairInfo(2245),
            new StairInfo(2246),
            new StairInfo(2247),
            new StairInfo(2248),
            new StairInfo(2249),
            new StairInfo(2250),
            new StairInfo(2325),
            new StairInfo(2326),
            new StairInfo(2327),
            new StairInfo(2328),
            new StairInfo(4540),
            new StairInfo(4541),
            new StairInfo(4542),
            new StairInfo(4543),
            new StairInfo(12254),
            new StairInfo(12255),
            new StairInfo(13778),
            new StairInfo(13779),
            new StairInfo(13780),
            new StairInfo(13781),
            new StairInfo(13782),
            new StairInfo(13833),
            new StairInfo(13834),
            new StairInfo(13835),
            new StairInfo(13836),
            new StairInfo(13837),
            new StairInfo(13837),
            new StairInfo(13843),
            new StairInfo(16946),
            new StairInfo(16947),
            new StairInfo(16948),
            new StairInfo(16949),
            new StairInfo(16950),
            new StairInfo(16951),
            new StairInfo(16952),
            new StairInfo(16953),
            new StairInfo(17120),
            new StairInfo(17121),
            new StairInfo(17122),
            new StairInfo(17123),
            new StairInfo(17124),
            new StairInfo(17125),
            new StairInfo(17126),
            new StairInfo(17127),
            new StairInfo(17128),
            new StairInfo(17129),
            new StairInfo(17130),
            new StairInfo(17131),
            new StairInfo(17132),
            new StairInfo(17133),
            new StairInfo(17134),
            new StairInfo(17135),
            new StairInfo(17136),
            new StairInfo(17137),
            new StairInfo(17138),
            new StairInfo(17139),
            new StairInfo(17140),
            new StairInfo(17141),
            new StairInfo(17142),
            new StairInfo(17143),
            new StairInfo(17144),
            new StairInfo(17145),
            new StairInfo(17146),
            new StairInfo(17147),
            new StairInfo(17148),
            new StairInfo(17149),
            new StairInfo(17150),
            new StairInfo(17151),
            new StairInfo(17152),
            new StairInfo(17153),
            new StairInfo(17242),
            new StairInfo(17242),
            new StairInfo(17242),
            new StairInfo(17243),
            new StairInfo(17243),
            new StairInfo(17244),
            new StairInfo(17244),
            new StairInfo(17245),
            new StairInfo(17245),
            new StairInfo(17246),
            new StairInfo(17246),
            new StairInfo(17247),
            new StairInfo(17247),
            new StairInfo(17248),
            new StairInfo(17248),
            new StairInfo(17249),
            new StairInfo(17249),
            new StairInfo(17250),
            new StairInfo(17250),
            new StairInfo(17251),
            new StairInfo(17251),
            new StairInfo(17252),
            new StairInfo(17252),
            new StairInfo(17253),
            new StairInfo(17253),
            new StairInfo(17254),
            new StairInfo(17254),
            new StairInfo(17255),
            new StairInfo(17255),
            new StairInfo(17256),
            new StairInfo(17256),
            new StairInfo(17257),
            new StairInfo(17257),
            new StairInfo(17258),
            new StairInfo(17258),
            new StairInfo(17259),
            new StairInfo(17259),
            new StairInfo(17260),
            new StairInfo(17260),
            new StairInfo(17261),
            new StairInfo(17261),
            new StairInfo(17328),
            new StairInfo(17328),
            new StairInfo(17329),
            new StairInfo(17329),
            new StairInfo(17330),
            new StairInfo(17330),
            new StairInfo(17331),
            new StairInfo(17331),
            new StairInfo(17332),
            new StairInfo(17332),
            new StairInfo(17333),
            new StairInfo(17333),
            new StairInfo(17334),
            new StairInfo(17334),
            new StairInfo(17335),
            new StairInfo(17335),
            new StairInfo(17336),
            new StairInfo(18096),
            new StairInfo(18097),
            new StairInfo(18180),
            new StairInfo(18180),
            new StairInfo(18181),
            new StairInfo(18181),
            new StairInfo(19201),
            new StairInfo(19201),
            new StairInfo(19202),
            new StairInfo(19202),
            new StairInfo(19203),
            new StairInfo(19203),
            new StairInfo(19204),
            new StairInfo(19204),
            new StairInfo(19205),
            new StairInfo(19205),
            new StairInfo(19206),
            new StairInfo(19250),
            new StairInfo(19251),
            new StairInfo(19252),
            new StairInfo(19262),
            new StairInfo(19262),
            new StairInfo(19263),
            new StairInfo(19263),
            new StairInfo(19264),
            new StairInfo(19264),
            new StairInfo(19265),
            new StairInfo(19265),
            new StairInfo(19754),
            new StairInfo(19754),
            new StairInfo(19933),
            new StairInfo(19938),
            new StairInfo(19938),
            new StairInfo(19939),
            new StairInfo(19939),
            new StairInfo(19940),
            new StairInfo(19940),
            new StairInfo(19941),
            new StairInfo(19941),
            new StairInfo(20322),
            new StairInfo(20322),
            new StairInfo(20323),
            new StairInfo(20323),
            new StairInfo(20324),
            new StairInfo(20324),
            new StairInfo(20325)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddStairGump()
            : this(0) { }

        public AddStairGump(int page)
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

            from.SendGump(new AddStairGump(page));
        }

        public class StairInfo
        {
            public int m_BaseID;

            public StairInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
