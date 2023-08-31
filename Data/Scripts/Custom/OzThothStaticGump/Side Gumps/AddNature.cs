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
    public class AddNatureStaticGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddNatureStatic",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddNatureStatic_OnCommand)
            );
        }

        [Usage("AddNatureStatic")]
        [Description("Displays a menu from which you can interactively add Nature Statics.")]
        public static void AddNatureStatic_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddNatureStaticGump));
            e.Mobile.SendGump(new AddNatureStaticGump());
        }

        public static NatureInfo[] m_Types = new NatureInfo[]
        {
            #region Nature

            new NatureInfo(730),
            new NatureInfo(731),
            new NatureInfo(6361),
            new NatureInfo(6362),
            new NatureInfo(6365),
            new NatureInfo(6366),
            new NatureInfo(6375),
            new NatureInfo(6376),
            new NatureInfo(6379),
            new NatureInfo(6380),
            new NatureInfo(6681),
            new NatureInfo(6682),
            new NatureInfo(6683),
            new NatureInfo(6684),
            new NatureInfo(6685),
            new NatureInfo(6686),
            new NatureInfo(6687),
            new NatureInfo(6688),
            new NatureInfo(6689),
            new NatureInfo(6690),
            new NatureInfo(6691),
            new NatureInfo(6692),
            new NatureInfo(6693),
            new NatureInfo(6694),
            new NatureInfo(6695),
            new NatureInfo(6696),
            new NatureInfo(6697),
            new NatureInfo(6698),
            new NatureInfo(6699),
            new NatureInfo(6700),
            new NatureInfo(6701),
            new NatureInfo(6702),
            new NatureInfo(6703),
            new NatureInfo(6704),
            new NatureInfo(6705),
            new NatureInfo(6706),
            new NatureInfo(6707),
            new NatureInfo(6708),
            new NatureInfo(6709),
            new NatureInfo(6710),
            new NatureInfo(6711),
            new NatureInfo(6712),
            new NatureInfo(6713),
            new NatureInfo(6714),
            new NatureInfo(6715),
            new NatureInfo(6716),
            new NatureInfo(6717),
            new NatureInfo(6718),
            new NatureInfo(6719),
            new NatureInfo(6720),
            new NatureInfo(6721),
            new NatureInfo(6722),
            new NatureInfo(6723),
            new NatureInfo(6724),
            new NatureInfo(6725),
            new NatureInfo(6726),
            new NatureInfo(6727),
            new NatureInfo(6728),
            new NatureInfo(6729),
            new NatureInfo(6730),
            new NatureInfo(6731),
            new NatureInfo(6732),
            new NatureInfo(6733),
            new NatureInfo(6734),
            new NatureInfo(6735),
            new NatureInfo(6736),
            new NatureInfo(6737),
            new NatureInfo(6738),
            new NatureInfo(6739),
            new NatureInfo(6740),
            new NatureInfo(6741),
            new NatureInfo(6742),
            new NatureInfo(6743),
            new NatureInfo(6744),
            new NatureInfo(6745),
            new NatureInfo(6746),
            new NatureInfo(6747),
            new NatureInfo(6748),
            new NatureInfo(6749),
            new NatureInfo(6750),
            new NatureInfo(6751),
            new NatureInfo(6752),
            new NatureInfo(6753),
            new NatureInfo(6754),
            new NatureInfo(6755),
            new NatureInfo(6756),
            new NatureInfo(6757),
            new NatureInfo(6758),
            new NatureInfo(6759),
            new NatureInfo(6760),
            new NatureInfo(6761),
            new NatureInfo(6762),
            new NatureInfo(6763),
            new NatureInfo(6764),
            new NatureInfo(6765),
            new NatureInfo(6766),
            new NatureInfo(6767),
            new NatureInfo(6768),
            new NatureInfo(6769),
            new NatureInfo(6770),
            new NatureInfo(6771),
            new NatureInfo(6772),
            new NatureInfo(6773),
            new NatureInfo(6774),
            new NatureInfo(6775),
            new NatureInfo(6776),
            new NatureInfo(6777),
            new NatureInfo(6778),
            new NatureInfo(6779),
            new NatureInfo(6780),
            new NatureInfo(6781),
            new NatureInfo(6782),
            new NatureInfo(8099),
            new NatureInfo(8100),
            new NatureInfo(8101),
            new NatureInfo(8102),
            new NatureInfo(8103),
            new NatureInfo(8104),
            new NatureInfo(8105),
            new NatureInfo(8106),
            new NatureInfo(8107),
            new NatureInfo(8108),
            new NatureInfo(8109),
            new NatureInfo(8110),
            new NatureInfo(8111),
            new NatureInfo(8112),
            new NatureInfo(8113),
            new NatureInfo(8114),
            new NatureInfo(8115),
            new NatureInfo(8116),
            new NatureInfo(8117),
            new NatureInfo(8118),
            new NatureInfo(8119),
            new NatureInfo(8120),
            new NatureInfo(8121),
            new NatureInfo(8122),
            new NatureInfo(8123),
            new NatureInfo(8124),
            new NatureInfo(8125),
            new NatureInfo(8126),
            new NatureInfo(8127),
            new NatureInfo(8128),
            new NatureInfo(8129),
            new NatureInfo(8130),
            new NatureInfo(8131),
            new NatureInfo(8132),
            new NatureInfo(8133),
            new NatureInfo(8134),
            new NatureInfo(8135),
            new NatureInfo(8136),
            new NatureInfo(8137),
            new NatureInfo(8138),
            new NatureInfo(8710),
            new NatureInfo(8711),
            new NatureInfo(8712),
            new NatureInfo(8713),
            new NatureInfo(8714),
            new NatureInfo(8715),
            new NatureInfo(8716),
            new NatureInfo(8717),
            new NatureInfo(8718),
            new NatureInfo(8719),
            new NatureInfo(8720),
            new NatureInfo(8721),
            new NatureInfo(8722),
            new NatureInfo(8723),
            new NatureInfo(8724),
            new NatureInfo(8725),
            new NatureInfo(8726),
            new NatureInfo(8727),
            new NatureInfo(8728),
            new NatureInfo(8729),
            new NatureInfo(8730),
            new NatureInfo(8731),
            new NatureInfo(8732),
            new NatureInfo(8733),
            new NatureInfo(8734),
            new NatureInfo(8735),
            new NatureInfo(8736),
            new NatureInfo(8737),
            new NatureInfo(8738),
            new NatureInfo(8739),
            new NatureInfo(8740),
            new NatureInfo(8741),
            new NatureInfo(8742),
            new NatureInfo(8743),
            new NatureInfo(8744),
            new NatureInfo(8745),
            new NatureInfo(8746),
            new NatureInfo(8747),
            new NatureInfo(8748),
            new NatureInfo(8749),
            new NatureInfo(8750),
            new NatureInfo(8751),
            new NatureInfo(8752),
            new NatureInfo(8753),
            new NatureInfo(8754),
            new NatureInfo(8755),
            new NatureInfo(8762),
            new NatureInfo(8763),
            new NatureInfo(8764),
            new NatureInfo(8765),
            new NatureInfo(8766),
            new NatureInfo(8767),
            new NatureInfo(8768),
            new NatureInfo(8769),
            new NatureInfo(8770),
            new NatureInfo(8771),
            new NatureInfo(8772),
            new NatureInfo(8773),
            new NatureInfo(8774),
            new NatureInfo(8775),
            new NatureInfo(8776),
            new NatureInfo(8777),
            new NatureInfo(12244),
            new NatureInfo(12252),
            new NatureInfo(12253),
            new NatureInfo(12262),
            new NatureInfo(12263),
            new NatureInfo(13549),
            new NatureInfo(13550),
            new NatureInfo(13551),
            new NatureInfo(13552),
            new NatureInfo(13553),
            new NatureInfo(13554),
            new NatureInfo(13555),
            new NatureInfo(13556),
            new NatureInfo(13557),
            new NatureInfo(13558),
            new NatureInfo(13559),
            new NatureInfo(13560),
            new NatureInfo(13561),
            new NatureInfo(13562),
            new NatureInfo(13563),
            new NatureInfo(13564),
            new NatureInfo(13565),
            new NatureInfo(13566),
            new NatureInfo(13567),
            new NatureInfo(13568),
            new NatureInfo(13569),
            new NatureInfo(13570),
            new NatureInfo(13571),
            new NatureInfo(13572),
            new NatureInfo(13573),
            new NatureInfo(13574),
            new NatureInfo(13575),
            new NatureInfo(13576),
            new NatureInfo(13577),
            new NatureInfo(13578),
            new NatureInfo(13579),
            new NatureInfo(13580),
            new NatureInfo(13581),
            new NatureInfo(13582),
            new NatureInfo(13583),
            new NatureInfo(13584),
            new NatureInfo(13585),
            new NatureInfo(13586),
            new NatureInfo(13587),
            new NatureInfo(13588),
            new NatureInfo(13589),
            new NatureInfo(13590),
            new NatureInfo(13591),
            new NatureInfo(13592),
            new NatureInfo(13593),
            new NatureInfo(13594),
            new NatureInfo(13595),
            new NatureInfo(13596),
            new NatureInfo(13597),
            new NatureInfo(13598),
            new NatureInfo(13599),
            new NatureInfo(13600),
            new NatureInfo(13601),
            new NatureInfo(13602),
            new NatureInfo(13603),
            new NatureInfo(13604),
            new NatureInfo(13605),
            new NatureInfo(13606),
            new NatureInfo(13607),
            new NatureInfo(13608),
            new NatureInfo(13613),
            new NatureInfo(13614),
            new NatureInfo(13615),
            new NatureInfo(13616),
            new NatureInfo(15097),
            new NatureInfo(15098),
            new NatureInfo(15099),
            new NatureInfo(15100),
            new NatureInfo(15101),
            new NatureInfo(15102),
            new NatureInfo(15103),
            new NatureInfo(15104),
            new NatureInfo(15105),
            new NatureInfo(15106),
            new NatureInfo(15107),
            new NatureInfo(15108),
            new NatureInfo(15109),
            new NatureInfo(15110),
            new NatureInfo(15111),
            new NatureInfo(15112),
            new NatureInfo(15113),
            new NatureInfo(15114),
            new NatureInfo(15115),
            new NatureInfo(15116),
            new NatureInfo(15117),
            new NatureInfo(15118),
            new NatureInfo(15119),
            new NatureInfo(15120),
            new NatureInfo(15121),
            new NatureInfo(15122),
            new NatureInfo(15123),
            new NatureInfo(15124),
            new NatureInfo(15125),
            new NatureInfo(15126),
            new NatureInfo(15127),
            new NatureInfo(15128),
            new NatureInfo(15129),
            new NatureInfo(17154),
            new NatureInfo(17155),
            new NatureInfo(17156),
            new NatureInfo(17157),
            new NatureInfo(17158),
            new NatureInfo(17159),
            new NatureInfo(17603),
            new NatureInfo(17604),
            new NatureInfo(17605),
            new NatureInfo(17606),
            new NatureInfo(17617),
            new NatureInfo(17618),
            new NatureInfo(17619),
            new NatureInfo(17620),
            new NatureInfo(17637),
            new NatureInfo(17638),
            new NatureInfo(17639),
            new NatureInfo(17640),
            new NatureInfo(17641),
            new NatureInfo(17642),
            new NatureInfo(17643),
            new NatureInfo(17644),
            new NatureInfo(17645),
            new NatureInfo(17646),
            new NatureInfo(17778),
            new NatureInfo(17779),
            new NatureInfo(17780),
            new NatureInfo(17781),
            new NatureInfo(17782),
            new NatureInfo(17783)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddNatureStaticGump()
            : this(0) { }

        public AddNatureStaticGump(int page)
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

            from.SendGump(new AddNatureStaticGump(page));
        }

        public class NatureInfo
        {
            public int m_BaseID;

            public NatureInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
