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
    public class AddLightsStaticGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddLightsStatic",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddLightsStatic_OnCommand)
            );
        }

        [Usage("AddLightsStatic")]
        [Description("Displays a menu from which you can interactively add Light statics.")]
        public static void AddLightsStatic_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddLightsStaticGump));
            e.Mobile.SendGump(new AddLightsStaticGump());
        }

        public static LightSTInfo[] m_Types = new LightSTInfo[]
        {
            #region LightStatics

            new LightSTInfo(575),
            new LightSTInfo(2188),
            new LightSTInfo(2555),
            new LightSTInfo(2556),
            new LightSTInfo(2557),
            new LightSTInfo(2558),
            new LightSTInfo(2559),
            new LightSTInfo(2560),
            new LightSTInfo(2561),
            new LightSTInfo(2562),
            new LightSTInfo(2563),
            new LightSTInfo(2564),
            new LightSTInfo(2565),
            new LightSTInfo(2566),
            new LightSTInfo(2567),
            new LightSTInfo(2568),
            new LightSTInfo(2569),
            new LightSTInfo(2570),
            new LightSTInfo(2571),
            new LightSTInfo(2572),
            new LightSTInfo(2573),
            new LightSTInfo(2574),
            new LightSTInfo(2575),
            new LightSTInfo(2576),
            new LightSTInfo(2577),
            new LightSTInfo(2578),
            new LightSTInfo(2579),
            new LightSTInfo(2580),
            new LightSTInfo(2581),
            new LightSTInfo(2582),
            new LightSTInfo(2583),
            new LightSTInfo(2584),
            new LightSTInfo(2586),
            new LightSTInfo(2587),
            new LightSTInfo(2588),
            new LightSTInfo(2589),
            new LightSTInfo(2591),
            new LightSTInfo(2592),
            new LightSTInfo(2594),
            new LightSTInfo(2595),
            new LightSTInfo(2596),
            new LightSTInfo(2597),
            new LightSTInfo(2598),
            new LightSTInfo(2599),
            new LightSTInfo(2600),
            new LightSTInfo(2601),
            new LightSTInfo(2842),
            new LightSTInfo(2843),
            new LightSTInfo(2844),
            new LightSTInfo(2845),
            new LightSTInfo(2846),
            new LightSTInfo(2847),
            new LightSTInfo(2848),
            new LightSTInfo(2849),
            new LightSTInfo(2850),
            new LightSTInfo(2851),
            new LightSTInfo(2852),
            new LightSTInfo(2853),
            new LightSTInfo(2854),
            new LightSTInfo(2855),
            new LightSTInfo(2856),
            new LightSTInfo(3553),
            new LightSTInfo(3554),
            new LightSTInfo(3555),
            new LightSTInfo(3556),
            new LightSTInfo(3557),
            new LightSTInfo(3558),
            new LightSTInfo(3559),
            new LightSTInfo(3560),
            new LightSTInfo(3561),
            new LightSTInfo(3562),
            new LightSTInfo(3633),
            new LightSTInfo(3634),
            new LightSTInfo(3635),
            new LightSTInfo(5164),
            new LightSTInfo(5165),
            new LightSTInfo(5166),
            new LightSTInfo(5167),
            new LightSTInfo(5168),
            new LightSTInfo(5169),
            new LightSTInfo(5170),
            new LightSTInfo(5171),
            new LightSTInfo(5172),
            new LightSTInfo(5173),
            new LightSTInfo(5174),
            new LightSTInfo(5175),
            new LightSTInfo(6570),
            new LightSTInfo(6571),
            new LightSTInfo(6572),
            new LightSTInfo(6573),
            new LightSTInfo(6574),
            new LightSTInfo(6575),
            new LightSTInfo(6576),
            new LightSTInfo(6577),
            new LightSTInfo(6578),
            new LightSTInfo(6579),
            new LightSTInfo(6580),
            new LightSTInfo(6581),
            new LightSTInfo(6582),
            new LightSTInfo(6587),
            new LightSTInfo(7885),
            new LightSTInfo(7886),
            new LightSTInfo(7887),
            new LightSTInfo(7888),
            new LightSTInfo(7889),
            new LightSTInfo(7890),
            new LightSTInfo(7979),
            new LightSTInfo(9403),
            new LightSTInfo(9404),
            new LightSTInfo(9405),
            new LightSTInfo(9406),
            new LightSTInfo(9409),
            new LightSTInfo(9410),
            new LightSTInfo(9411),
            new LightSTInfo(9412),
            new LightSTInfo(9413),
            new LightSTInfo(9414),
            new LightSTInfo(9415),
            new LightSTInfo(9416),
            new LightSTInfo(9417),
            new LightSTInfo(9418),
            new LightSTInfo(15764),
            new LightSTInfo(15765),
            new LightSTInfo(15766),
            new LightSTInfo(15767),
            new LightSTInfo(15768),
            new LightSTInfo(15769),
            new LightSTInfo(15770),
            new LightSTInfo(15771)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddLightsStaticGump()
            : this(0) { }

        public AddLightsStaticGump(int page)
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

            from.SendGump(new AddLightsStaticGump(page));
        }

        public class LightSTInfo
        {
            public int m_BaseID;

            public LightSTInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
