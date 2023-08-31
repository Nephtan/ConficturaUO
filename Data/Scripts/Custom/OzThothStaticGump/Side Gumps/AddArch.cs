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
    public class AddArchGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddArch",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddArch_OnCommand)
            );
        }

        [Usage("AddArch")]
        [Description("Displays a menu from which you can interactively add Arches.")]
        public static void AddArch_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddArchGump));
            e.Mobile.SendGump(new AddArchGump());
        }

        public static ArchInfo[] m_Types = new ArchInfo[]
        {
            #region Archs

            new ArchInfo(40),
            new ArchInfo(41),
            new ArchInfo(42),
            new ArchInfo(43),
            new ArchInfo(44),
            new ArchInfo(45),
            new ArchInfo(46),
            new ArchInfo(47),
            new ArchInfo(69),
            new ArchInfo(70),
            new ArchInfo(71),
            new ArchInfo(72),
            new ArchInfo(73),
            new ArchInfo(109),
            new ArchInfo(110),
            new ArchInfo(111),
            new ArchInfo(112),
            new ArchInfo(113),
            new ArchInfo(114),
            new ArchInfo(115),
            new ArchInfo(116),
            new ArchInfo(117),
            new ArchInfo(118),
            new ArchInfo(119),
            new ArchInfo(122),
            new ArchInfo(123),
            new ArchInfo(124),
            new ArchInfo(125),
            new ArchInfo(126),
            new ArchInfo(127),
            new ArchInfo(134),
            new ArchInfo(135),
            new ArchInfo(136),
            new ArchInfo(137),
            new ArchInfo(138),
            new ArchInfo(139),
            new ArchInfo(195),
            new ArchInfo(196),
            new ArchInfo(198),
            new ArchInfo(205),
            new ArchInfo(206),
            new ArchInfo(207),
            new ArchInfo(208),
            new ArchInfo(209),
            new ArchInfo(210),
            new ArchInfo(211),
            new ArchInfo(212),
            new ArchInfo(213),
            new ArchInfo(214),
            new ArchInfo(215),
            new ArchInfo(216),
            new ArchInfo(217),
            new ArchInfo(218),
            new ArchInfo(223),
            new ArchInfo(224),
            new ArchInfo(225),
            new ArchInfo(226),
            new ArchInfo(227),
            new ArchInfo(274),
            new ArchInfo(275),
            new ArchInfo(276),
            new ArchInfo(277),
            new ArchInfo(278),
            new ArchInfo(279),
            new ArchInfo(280),
            new ArchInfo(281),
            new ArchInfo(282),
            new ArchInfo(316),
            new ArchInfo(317),
            new ArchInfo(318),
            new ArchInfo(319),
            new ArchInfo(320),
            new ArchInfo(321),
            new ArchInfo(322),
            new ArchInfo(323),
            new ArchInfo(324),
            new ArchInfo(364),
            new ArchInfo(365),
            new ArchInfo(366),
            new ArchInfo(367),
            new ArchInfo(368),
            new ArchInfo(369),
            new ArchInfo(370),
            new ArchInfo(371),
            new ArchInfo(377),
            new ArchInfo(378),
            new ArchInfo(379),
            new ArchInfo(380),
            new ArchInfo(381),
            new ArchInfo(382),
            new ArchInfo(383),
            new ArchInfo(384),
            new ArchInfo(385),
            new ArchInfo(390),
            new ArchInfo(391),
            new ArchInfo(392),
            new ArchInfo(393),
            new ArchInfo(412),
            new ArchInfo(413),
            new ArchInfo(414),
            new ArchInfo(415),
            new ArchInfo(416),
            new ArchInfo(417),
            new ArchInfo(418),
            new ArchInfo(420),
            new ArchInfo(469),
            new ArchInfo(470),
            new ArchInfo(471),
            new ArchInfo(472),
            new ArchInfo(473),
            new ArchInfo(474),
            new ArchInfo(475),
            new ArchInfo(476),
            new ArchInfo(477),
            new ArchInfo(478),
            new ArchInfo(479),
            new ArchInfo(480),
            new ArchInfo(481),
            new ArchInfo(482),
            new ArchInfo(483),
            new ArchInfo(484),
            new ArchInfo(485),
            new ArchInfo(486),
            new ArchInfo(487),
            new ArchInfo(581),
            new ArchInfo(582),
            new ArchInfo(583),
            new ArchInfo(584),
            new ArchInfo(585),
            new ArchInfo(586),
            new ArchInfo(688),
            new ArchInfo(689),
            new ArchInfo(690),
            new ArchInfo(691),
            new ArchInfo(692),
            new ArchInfo(711),
            new ArchInfo(712),
            new ArchInfo(713),
            new ArchInfo(714),
            new ArchInfo(715),
            new ArchInfo(716),
            new ArchInfo(717),
            new ArchInfo(919),
            new ArchInfo(920),
            new ArchInfo(921),
            new ArchInfo(922),
            new ArchInfo(923),
            new ArchInfo(924),
            new ArchInfo(925),
            new ArchInfo(926),
            new ArchInfo(927),
            new ArchInfo(928),
            new ArchInfo(1080),
            new ArchInfo(1081),
            new ArchInfo(1082),
            new ArchInfo(1083),
            new ArchInfo(1084),
            new ArchInfo(1085),
            new ArchInfo(1086),
            new ArchInfo(1087),
            new ArchInfo(1088),
            new ArchInfo(1089),
            new ArchInfo(1094),
            new ArchInfo(1095),
            new ArchInfo(1096),
            new ArchInfo(1097),
            new ArchInfo(1098),
            new ArchInfo(1099),
            new ArchInfo(1100),
            new ArchInfo(1101),
            new ArchInfo(1102),
            new ArchInfo(1103),
            new ArchInfo(2006),
            new ArchInfo(2009),
            new ArchInfo(8701),
            new ArchInfo(8705),
            new ArchInfo(8706),
            new ArchInfo(10716),
            new ArchInfo(10717),
            new ArchInfo(10718),
            new ArchInfo(10719),
            new ArchInfo(10720),
            new ArchInfo(10721),
            new ArchInfo(10722),
            new ArchInfo(10723),
            new ArchInfo(10724),
            new ArchInfo(10725),
            new ArchInfo(11177),
            new ArchInfo(11178),
            new ArchInfo(11179),
            new ArchInfo(11180),
            new ArchInfo(11187),
            new ArchInfo(11188),
            new ArchInfo(11189),
            new ArchInfo(11190),
            new ArchInfo(11191),
            new ArchInfo(11192),
            new ArchInfo(11203),
            new ArchInfo(11204),
            new ArchInfo(11205),
            new ArchInfo(11206),
            new ArchInfo(11540),
            new ArchInfo(11541),
            new ArchInfo(11542),
            new ArchInfo(11543),
            new ArchInfo(11574),
            new ArchInfo(11575),
            new ArchInfo(11711),
            new ArchInfo(11712),
            new ArchInfo(11713),
            new ArchInfo(11714),
            new ArchInfo(11721),
            new ArchInfo(11722),
            new ArchInfo(12059),
            new ArchInfo(12060),
            new ArchInfo(12061),
            new ArchInfo(12062),
            new ArchInfo(13838),
            new ArchInfo(13839),
            new ArchInfo(13840),
            new ArchInfo(13841),
            new ArchInfo(13842),
            new ArchInfo(16667),
            new ArchInfo(16668),
            new ArchInfo(16669),
            new ArchInfo(16670),
            new ArchInfo(16671),
            new ArchInfo(16672),
            new ArchInfo(16673),
            new ArchInfo(16674),
            new ArchInfo(16726),
            new ArchInfo(16727),
            new ArchInfo(16728),
            new ArchInfo(16729),
            new ArchInfo(16752),
            new ArchInfo(16753),
            new ArchInfo(16754),
            new ArchInfo(16755),
            new ArchInfo(16800),
            new ArchInfo(16801),
            new ArchInfo(16803),
            new ArchInfo(16804),
            new ArchInfo(16809),
            new ArchInfo(16810),
            new ArchInfo(16811),
            new ArchInfo(16812),
            new ArchInfo(17220),
            new ArchInfo(17221),
            new ArchInfo(17222),
            new ArchInfo(17223),
            new ArchInfo(17336),
            new ArchInfo(17337),
            new ArchInfo(17338),
            new ArchInfo(17339),
            new ArchInfo(17340),
            new ArchInfo(17341),
            new ArchInfo(17342),
            new ArchInfo(17343),
            new ArchInfo(18088),
            new ArchInfo(18089),
            new ArchInfo(18092),
            new ArchInfo(18093),
            new ArchInfo(18176),
            new ArchInfo(18177),
            new ArchInfo(18178),
            new ArchInfo(18179),
            new ArchInfo(18401),
            new ArchInfo(18402),
            new ArchInfo(18403),
            new ArchInfo(18404),
            new ArchInfo(18407),
            new ArchInfo(18408),
            new ArchInfo(18409),
            new ArchInfo(19261),
            new ArchInfo(20728),
            new ArchInfo(20729),
            new ArchInfo(20730),
            new ArchInfo(20731),
            new ArchInfo(20732),
            new ArchInfo(20771),
            new ArchInfo(20772),
            new ArchInfo(20773),
            new ArchInfo(20774),
            new ArchInfo(30705),
            new ArchInfo(30706),
            new ArchInfo(30707),
            new ArchInfo(30708),
            new ArchInfo(30709)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddArchGump()
            : this(0) { }

        public AddArchGump(int page)
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

            from.SendGump(new AddArchGump(page));
        }

        public class ArchInfo
        {
            public int m_BaseID;

            public ArchInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
