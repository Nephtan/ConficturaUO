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
    public class AddRockGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddRock",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddRock_OnCommand)
            );
        }

        [Usage("AddRock")]
        [Description("Displays a menu from which you can interactively add Rock Statics.")]
        public static void AddRock_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddRockGump));
            e.Mobile.SendGump(new AddRockGump());
        }

        public static RockInfo[] m_Types = new RockInfo[]
        {
            #region Rock

            new RockInfo(1788),
            new RockInfo(1793),
            new RockInfo(1794),
            new RockInfo(1795),
            new RockInfo(1796),
            new RockInfo(1797),
            new RockInfo(1798),
            new RockInfo(1799),
            new RockInfo(1800),
            new RockInfo(2001),
            new RockInfo(2002),
            new RockInfo(2272),
            new RockInfo(2273),
            new RockInfo(2274),
            new RockInfo(2275),
            new RockInfo(2276),
            new RockInfo(2277),
            new RockInfo(2278),
            new RockInfo(2279),
            new RockInfo(2280),
            new RockInfo(2281),
            new RockInfo(2282),
            new RockInfo(4943),
            new RockInfo(4944),
            new RockInfo(4945),
            new RockInfo(4946),
            new RockInfo(4947),
            new RockInfo(4948),
            new RockInfo(4949),
            new RockInfo(4950),
            new RockInfo(4951),
            new RockInfo(4952),
            new RockInfo(4953),
            new RockInfo(4954),
            new RockInfo(4955),
            new RockInfo(4956),
            new RockInfo(4957),
            new RockInfo(4958),
            new RockInfo(4959),
            new RockInfo(4960),
            new RockInfo(4961),
            new RockInfo(4962),
            new RockInfo(4963),
            new RockInfo(4964),
            new RockInfo(4965),
            new RockInfo(4966),
            new RockInfo(4967),
            new RockInfo(4968),
            new RockInfo(4969),
            new RockInfo(4970),
            new RockInfo(4971),
            new RockInfo(4972),
            new RockInfo(4973),
            new RockInfo(6001),
            new RockInfo(6002),
            new RockInfo(6003),
            new RockInfo(6004),
            new RockInfo(6005),
            new RockInfo(6006),
            new RockInfo(6007),
            new RockInfo(6008),
            new RockInfo(6009),
            new RockInfo(6010),
            new RockInfo(6011),
            new RockInfo(6012),
            new RockInfo(13345),
            new RockInfo(13346),
            new RockInfo(13347),
            new RockInfo(13348),
            new RockInfo(13350),
            new RockInfo(13351),
            new RockInfo(13352),
            new RockInfo(13353),
            new RockInfo(13446),
            new RockInfo(13447),
            new RockInfo(13448),
            new RockInfo(13449),
            new RockInfo(13450),
            new RockInfo(13451),
            new RockInfo(13452),
            new RockInfo(13453),
            new RockInfo(13454),
            new RockInfo(13455),
            new RockInfo(13484),
            new RockInfo(13485),
            new RockInfo(13486),
            new RockInfo(13487),
            new RockInfo(13488),
            new RockInfo(13489),
            new RockInfo(13490),
            new RockInfo(13491),
            new RockInfo(13492),
            new RockInfo(22024),
            new RockInfo(22025),
            new RockInfo(22026),
            new RockInfo(22027),
            new RockInfo(22028),
            new RockInfo(22029),
            new RockInfo(22030),
            new RockInfo(22031),
            new RockInfo(22032),
            new RockInfo(22033),
            new RockInfo(22062),
            new RockInfo(22063),
            new RockInfo(22064),
            new RockInfo(22065),
            new RockInfo(22066),
            new RockInfo(22067),
            new RockInfo(22068),
            new RockInfo(22069),
            new RockInfo(22070)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddRockGump()
            : this(0) { }

        public AddRockGump(int page)
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

            from.SendGump(new AddRockGump(page));
        }

        public class RockInfo
        {
            public int m_BaseID;

            public RockInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
