using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Menus;
using Server.Menus.Questions;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;

namespace Server.Engines.Help
{
    public class ContainedMenu : QuestionMenu
    {
        private Mobile m_From;

        public ContainedMenu(Mobile from)
            : base(
                "You already have an open help request. We will have someone assist you as soon as possible.  What would you like to do?",
                new string[]
                {
                    "Leave my old help request like it is.",
                    "Remove my help request from the queue."
                }
            )
        {
            m_From = from;
        }

        public override void OnCancel(NetState state)
        {
            m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
        }

        public override void OnResponse(NetState state, int index)
        {
            m_From.SendSound(0x4A);
            if (index == 0)
            {
                m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
            }
            else if (index == 1)
            {
                PageEntry entry = PageQueue.GetEntry(m_From);

                if (entry != null && entry.Handler == null)
                {
                    m_From.SendLocalizedMessage(1005307, "", 0x35); // Removed help request.
                    entry.AddResponse(entry.Sender, "[Canceled]");
                    PageQueue.Remove(entry);
                }
                else
                {
                    m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
                }
            }
        }
    }

    public class HelpGump : Gump
    {
        public static void Initialize()
        {
            EventSink.HelpRequest += new HelpRequestEventHandler(EventSink_HelpRequest);
        }

        private static void EventSink_HelpRequest(HelpRequestEventArgs e)
        {
            foreach (Gump g in e.Mobile.NetState.Gumps)
            {
                if (g is HelpGump)
                    return;
            }

            if (!PageQueue.CheckAllowedToPage(e.Mobile))
                return;

            if (PageQueue.Contains(e.Mobile))
                e.Mobile.SendMenu(new ContainedMenu(e.Mobile));
            else
                e.Mobile.SendGump(new HelpGump(e.Mobile, 1));
        }

        private static bool IsYoung(Mobile m)
        {
            if (m is PlayerMobile)
                return ((PlayerMobile)m).Young;

            return false;
        }

        public static bool CheckCombat(Mobile m)
        {
            for (int i = 0; i < m.Aggressed.Count; ++i)
            {
                AggressorInfo info = m.Aggressed[i];

                if (DateTime.Now - info.LastCombatTime < TimeSpan.FromSeconds(30.0))
                    return true;
            }

            return false;
        }

        public HelpGump(Mobile from, int page)
            : base(50, 50)
        {
            string HelpText = MyHelp();
            string color = "#ddbc4b";
            int button = 4005;

            from.SendSound(0x4A);

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            int r = 40;
            int e = 30;

            AddImage(0, 0, 9548, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddHtml(
                12,
                12,
                300,
                20,
                @"<BODY><BASEFONT Color=" + color + ">HELP OPTIONS</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            AddButton(967, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 1)
            {
                button = 4006;
                AddHtml(
                    252,
                    71,
                    739,
                    630,
                    @"<BODY><BASEFONT Color=" + color + ">" + HelpText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }
            AddButton(15, r, button, button, 1, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Main</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 3609;
            HelpText = "Your 'Away From Keyboard' Settings Are Disabled.";
            if (Server.Commands.AFK.m_AFK.Contains(from.Serial.Value))
            {
                button = 4018;
                HelpText = "Your 'Away From Keyboard' Settings Are Enabled.";
            }
            if (page == 2)
            {
                AddHtml(
                    252,
                    71,
                    739,
                    630,
                    @"<BODY><BASEFONT Color=" + color + ">" + HelpText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }
            AddButton(15, r, button, button, 2, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">AFK</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 3)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 3, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Chat</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 18)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 18, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Conversations</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 4)
            {
                button = 4006;
                AddHtml(
                    252,
                    71,
                    739,
                    630,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Your empty corpses have been removed.</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }
            AddButton(15, r, button, button, 4, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Corpse Clear</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 5)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 5, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Corpse Search</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 6)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 6, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Emote</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 13)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 13, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Library</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 7)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 7, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Magic Toolbars</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;
            if (page == 7)
            {
                AddButton(904, 10, 3610, 3610, 95, GumpButtonType.Reply, 0);

                AddButton(245, 95, 4005, 4005, 66, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    95,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Bard Songs Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 95, 4005, 4005, 266, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    95,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 95, 4020, 4020, 366, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    95,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 125, 4005, 4005, 67, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    125,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Bard Songs Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 125, 4005, 4005, 267, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    125,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 125, 4020, 4020, 367, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    125,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 155, 4005, 4005, 68, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    155,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Knight Spell Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 155, 4005, 4005, 268, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    155,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 155, 4020, 4020, 368, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    155,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 185, 4005, 4005, 69, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    185,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Knight Spell Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 185, 4005, 4005, 269, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    185,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 185, 4020, 4020, 369, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    185,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 215, 4005, 4005, 70, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    215,
                    316,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Death Knight Spell Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 215, 4005, 4005, 270, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    215,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 215, 4020, 4020, 370, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    215,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 245, 4005, 4005, 71, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    245,
                    316,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Death Knight Spell Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 245, 4005, 4005, 271, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    245,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 245, 4020, 4020, 371, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    245,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 275, 4005, 4005, 978, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    275,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Elemental Spell Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 275, 4005, 4005, 282, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    275,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 275, 4020, 4020, 382, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    275,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 305, 4005, 4005, 979, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    305,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Elemental Spell Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 305, 4005, 4005, 283, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    305,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 305, 4020, 4020, 383, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    305,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 335, 4005, 4005, 72, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    335,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Magery Spell Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 335, 4005, 4005, 272, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    335,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 335, 4020, 4020, 372, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    335,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 365, 4005, 4005, 73, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    365,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Magery Spell Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 365, 4005, 4005, 273, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    365,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 365, 4020, 4020, 373, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    365,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 395, 4005, 4005, 74, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    395,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Magery Spell Bar III</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 395, 4005, 4005, 274, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    395,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 395, 4020, 4020, 374, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    395,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 425, 4005, 4005, 75, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    425,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Magery Spell Bar IV</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 425, 4005, 4005, 275, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    425,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 425, 4020, 4020, 375, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    425,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 455, 4005, 4005, 980, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    455,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Monk Ability Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 455, 4005, 4005, 280, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    455,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 455, 4020, 4020, 380, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    455,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 485, 4005, 4005, 981, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    485,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Monk Ability Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 485, 4005, 4005, 281, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    485,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 485, 4020, 4020, 381, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    485,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 515, 4005, 4005, 76, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    515,
                    316,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Necromancer Spell Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 515, 4005, 4005, 276, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    515,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 515, 4020, 4020, 376, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    515,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 545, 4005, 4005, 77, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    545,
                    316,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Necromancer Spell Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 545, 4005, 4005, 277, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    545,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 545, 4020, 4020, 377, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    545,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 575, 4005, 4005, 78, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    575,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Priest Prayer Bar I</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 575, 4005, 4005, 278, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    575,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 575, 4020, 4020, 378, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    575,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(245, 605, 4005, 4005, 79, GumpButtonType.Reply, 0);
                AddHtml(
                    280,
                    605,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Priest Prayer Bar II</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(605, 605, 4005, 4005, 279, GumpButtonType.Reply, 0);
                AddHtml(
                    640,
                    605,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Open Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(800, 605, 4020, 4020, 379, GumpButtonType.Reply, 0);
                AddHtml(
                    835,
                    605,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Close Toolbar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 8)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 8, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Moongate Search</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            button = 4005;
            if (page == 9)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 9, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">MOTD</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 10)
            {
                button = 4006;
                AddHtml(
                    252,
                    71,
                    739,
                    630,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Throughout your journey, you may come across particular events that appear in your quest log. They may be a simple achievement of finding a strange land, or they may reference an item you must find. Quests are handled in a 'virtual' manner. What this means is that any achievements are real, but any references to items found are not. If your quest log states that you found an ebony key, you will not have an ebony key in your backpack...but you will 'virtually' have the item. The quest will keep track of this fact for you. Because of this, you will never lose that ebony key and it remains unique to your character's questing. The quest knows you found it and have it. You may be tasked to find an item in a dungeon. When there is an indication you found it, it will be 'virtually' in your possession. You will often hear a sound of victory when a quest event is reached, along with a message about it. You still may miss it, however. So check your quest log from time to time. One way to get quests is to visit taverns or inns. If you see a bulletin board called 'Seeking Brave Adventurers', single click on it to begin your life questing for fame and fortune.<BR><BR>"
                        + MyQuests(from)
                        + "<BR><BR></BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }
            AddButton(15, r, button, button, 10, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Quests</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 11)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 11, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Quick Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            button = 4005;
            if (page == 62)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 62, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Reagent Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            button = 4005;
            if (page == 12)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 12, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Settings</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;
            if (page == 12)
            {
                int g = 70;
                int j = 30;
                int setB = 3609;
                int xm = 245;
                int xo = 700;
                int xr = 0;
                int xs = 245;

                if (!from.NoAutoAttack)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 61, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 89, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Auto Attack</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (((PlayerMobile)from).CharacterSheath == 1)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 52, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 100, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Auto Sheath</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (((PlayerMobile)from).ClassicPoisoning == 1)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 64, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 86, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Classic Poisoning</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (
                    from.RaceID > 0
                    && (from.Region).Name == "the Tavern"
                    && Server.Items.BaseRace.GetMonsterMage(from.RaceID)
                )
                {
                    string magic = "Default";
                    if (from.RaceMagicSchool == 1)
                    {
                        magic = "Magery";
                    }
                    else if (from.RaceMagicSchool == 2)
                    {
                        magic = "Necromancy";
                    }
                    else if (from.RaceMagicSchool == 3)
                    {
                        magic = "Elementalism";
                    }
                    AddButton(xs, g, 4005, 4005, 989, GumpButtonType.Reply, 0);
                    AddButton(xs + 40, g, 4011, 4011, 103, GumpButtonType.Reply, 0);
                    AddHtml(
                        xs + 80,
                        g,
                        316,
                        20,
                        @"<BODY><BASEFONT Color="
                            + color
                            + ">Creature Magic ("
                            + magic
                            + ")</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );
                    if (xr == 1)
                    {
                        g = g + j;
                        xr = 0;
                        xs = xm;
                    }
                    else
                    {
                        xr = 1;
                        xs = xo;
                    }
                }

                if (from.RaceID > 0)
                {
                    if (from.RaceMakeSounds)
                    {
                        setB = 4018;
                    }
                    else
                    {
                        setB = 3609;
                    }
                    AddButton(xs, g, setB, setB, 991, GumpButtonType.Reply, 0);
                    AddButton(xs + 40, g, 4011, 4011, 105, GumpButtonType.Reply, 0);
                    AddHtml(
                        xs + 80,
                        g,
                        316,
                        20,
                        @"<BODY><BASEFONT Color=" + color + ">Creature Sounds</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );

                    if (xr == 1)
                    {
                        g = g + j;
                        xr = 0;
                        xs = xm;
                    }
                    else
                    {
                        xr = 1;
                        xs = xo;
                    }
                }

                if (
                    from.RaceID > 0
                    && (
                        (from.Region).Name == "the Tavern"
                        || (
                            from.Map == Map.Sosaria
                            && from.X >= 6982
                            && from.Y >= 694
                            && from.X <= 6999
                            && from.Y <= 713
                        )
                    )
                )
                {
                    AddButton(xs, g, 4005, 4005, 990, GumpButtonType.Reply, 0);
                    AddButton(xs + 40, g, 4011, 4011, 104, GumpButtonType.Reply, 0);
                    AddHtml(
                        xs + 80,
                        g,
                        316,
                        20,
                        @"<BODY><BASEFONT Color=" + color + ">Creature Type</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );

                    if (xr == 1)
                    {
                        g = g + j;
                        xr = 0;
                        xs = xm;
                    }
                    else
                    {
                        xr = 1;
                        xs = xo;
                    }
                }

                if (MyServerSettings.AllowCustomTitles())
                {
                    AddButton(xs, g, 4005, 4005, 80, GumpButtonType.Reply, 0);
                    AddButton(xs + 40, g, 4011, 4011, 97, GumpButtonType.Reply, 0);
                    AddHtml(
                        xs + 80,
                        g,
                        316,
                        20,
                        @"<BODY><BASEFONT Color=" + color + ">Custom Title</BASEFONT></BODY>",
                        (bool)false,
                        (bool)false
                    );

                    if (xr == 1)
                    {
                        g = g + j;
                        xr = 0;
                        xs = xm;
                    }
                    else
                    {
                        xr = 1;
                        xs = xo;
                    }
                }

                if (((PlayerMobile)from).GumpHue > 0)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 985, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 101, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Gump Images</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                AddButton(xs, g, 4005, 4005, 55, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 85, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Loot Options</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (from.RainbowMsg)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 60, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 88, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Message Colors</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                AddButton(xs, g, 4005, 4005, 65, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 83, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Music Playlist</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (((PlayerMobile)from).CharMusical == "Forest")
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 53, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 82, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Music Tone</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                if (((PlayerMobile)from).PublicMyRunUO == false)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(xs, g, setB, setB, 54, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 84, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Private Play</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                AddButton(xs, g, 4005, 4005, 56, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 87, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Skill Title</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (xr == 1)
                {
                    g = g + j;
                    xr = 0;
                    xs = xm;
                }
                else
                {
                    xr = 1;
                    xs = xo;
                }

                string skillLocks = "Skill List (Show Up)";
                if (((PlayerMobile)from).SkillDisplay == 1)
                {
                    setB = 4018;
                    skillLocks = "Skill List (Show Up and Locked)";
                }
                else
                {
                    setB = 4017;
                }
                AddButton(xs, g, setB, setB, 982, GumpButtonType.Reply, 0);
                AddButton(xs + 40, g, 4011, 4011, 199, GumpButtonType.Reply, 0);
                AddHtml(
                    xs + 80,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">" + skillLocks + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                g = g + j;
                g = g + j;

                AddHtml(
                    325,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Play Styles</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                g = g + j;

                if (
                    ((PlayerMobile)from).CharacterEvil == 0
                    && ((PlayerMobile)from).CharacterOriental == 0
                    && ((PlayerMobile)from).CharacterBarbaric == 0
                )
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(325, g, setB, setB, 57, GumpButtonType.Reply, 0);
                AddButton(370, g, 4011, 4011, 92, GumpButtonType.Reply, 0);
                AddHtml(
                    410,
                    g,
                    65,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Normal</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (((PlayerMobile)from).CharacterEvil == 1)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(535, g, setB, setB, 58, GumpButtonType.Reply, 0);
                AddButton(575, g, 4011, 4011, 93, GumpButtonType.Reply, 0);
                AddHtml(
                    620,
                    g,
                    65,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Evil</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (((PlayerMobile)from).CharacterOriental == 1)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(745, g, setB, setB, 59, GumpButtonType.Reply, 0);
                AddButton(785, g, 4011, 4011, 94, GumpButtonType.Reply, 0);
                AddHtml(
                    830,
                    g,
                    65,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Oriental</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                g = g + j;

                string amazon = "";
                if (((PlayerMobile)from).CharacterBarbaric == 1)
                {
                    setB = 4018;
                }
                else if (((PlayerMobile)from).CharacterBarbaric == 2)
                {
                    setB = 4003;
                    amazon = " with Amazon Fighting Titles";
                }
                else
                {
                    setB = 3609;
                }
                AddButton(325, g, setB, setB, 984, GumpButtonType.Reply, 0);
                AddButton(370, g, 4011, 4011, 198, GumpButtonType.Reply, 0);
                AddHtml(
                    410,
                    g,
                    300,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Barbaric" + amazon + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                g = g + j;
                g = g + j;

                AddButton(285, g, 4011, 4011, 96, GumpButtonType.Reply, 0);
                AddHtml(
                    325,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Magery Spell Color</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                if (((PlayerMobile)from).MagerySpellHue == 0x47E)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    565,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">White</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(523, g, setB, setB, 500, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0x94E)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    685,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Black</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(643, g, setB, setB, 501, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0x48D)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    805,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Blue</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(764, g, setB, setB, 502, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0x48E)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    925,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Red</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(883, g, setB, setB, 503, GumpButtonType.Reply, 0);

                g = g + j;

                if (((PlayerMobile)from).MagerySpellHue == 0x48F)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    565,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Green</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(523, g, setB, setB, 504, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0x490)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    685,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Purple</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(643, g, setB, setB, 505, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0x491)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    805,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Yellow</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(764, g, setB, setB, 506, GumpButtonType.Reply, 0);

                if (((PlayerMobile)from).MagerySpellHue == 0)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddHtml(
                    925,
                    g,
                    61,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Default</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddButton(883, g, setB, setB, 507, GumpButtonType.Reply, 0);

                g = g + j;
                g = g + j;

                if (((PlayerMobile)from).WeaponBarOpen > 0)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(245, g, setB, setB, 986, GumpButtonType.Reply, 0);
                AddButton(285, g, 4011, 4011, 102, GumpButtonType.Reply, 0);
                AddHtml(
                    325,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Weapon Ability Bar</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                g = g + j;

                if (((PlayerMobile)from).CharacterWepAbNames == 1)
                {
                    setB = 4018;
                }
                else
                {
                    setB = 3609;
                }
                AddButton(245, g, setB, setB, 51, GumpButtonType.Reply, 0);
                AddButton(285, g, 4011, 4011, 99, GumpButtonType.Reply, 0);
                AddHtml(
                    325,
                    g,
                    316,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Weapon Ability Names</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                g = g + j;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 983)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 983, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Skill List</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 14)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 14, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Statistics</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool house = false;
            if (from.Region is HouseRegion)
                if (((HouseRegion)from.Region).House.IsOwner(from))
                    house = true;
            if (
                from.Region.GetLogoutDelay(from) != TimeSpan.Zero
                && house == false
                && !(from.Region is PrisonArea)
                && !(from.Region is GargoyleRegion)
                && !(from.Region is SafeRegion)
            )
            {
                button = 4005;
                if (page == 15)
                {
                    button = 4006;
                }
                AddButton(15, r, button, button, 15, GumpButtonType.Reply, 0);
                AddHtml(
                    50,
                    r,
                    148,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Stuck in World</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                r = r + e;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            HelpText = Server.Misc.MyServerSettings.Versions();
            button = 4005;
            if (page == 19)
            {
                button = 4006;
                AddHtml(
                    252,
                    71,
                    739,
                    630,
                    @"<BODY><BASEFONT Color=" + color + ">" + HelpText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }
            AddButton(15, r, button, button, 19, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Version</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 17)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 17, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Wealth Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            button = 4005;
            if (page == 16)
            {
                button = 4006;
            }
            AddButton(15, r, button, button, 16, GumpButtonType.Reply, 0);
            AddHtml(
                50,
                r,
                148,
                20,
                @"<BODY><BASEFONT Color=" + color + ">Weapon Abilities</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            r = r + e;
        }

        public void InvokeCommand(string c, Mobile from)
        {
            CommandSystem.Handle(from, String.Format("{0}{1}", CommandSystem.Prefix, c));
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            from.SendSound(0x4A);

            from.CloseGump(typeof(Server.Engines.Help.HelpGump));

            if (info.ButtonID > 81 && info.ButtonID < 200) // SMALL INFO HELP WINDOWS
            {
                from.CloseGump(typeof(InfoHelpGump));
                from.SendGump(new InfoHelpGump(from, info.ButtonID, 12));
            }
            if (info.ButtonID >= 200 && info.ButtonID <= 400) // MAGIC BARS OPEN AND CLOSE
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));

                if (info.ButtonID == 266)
                {
                    InvokeCommand("bardtool1", from);
                }
                else if (info.ButtonID == 366)
                {
                    InvokeCommand("bardclose1", from);
                }
                else if (info.ButtonID == 267)
                {
                    InvokeCommand("bardtool2", from);
                }
                else if (info.ButtonID == 367)
                {
                    InvokeCommand("bardclose2", from);
                }
                else if (info.ButtonID == 268)
                {
                    InvokeCommand("knighttool1", from);
                }
                else if (info.ButtonID == 368)
                {
                    InvokeCommand("knightclose1", from);
                }
                else if (info.ButtonID == 269)
                {
                    InvokeCommand("knighttool2", from);
                }
                else if (info.ButtonID == 369)
                {
                    InvokeCommand("knightclose2", from);
                }
                else if (info.ButtonID == 270)
                {
                    InvokeCommand("deathtool1", from);
                }
                else if (info.ButtonID == 370)
                {
                    InvokeCommand("deathclose1", from);
                }
                else if (info.ButtonID == 271)
                {
                    InvokeCommand("deathtool2", from);
                }
                else if (info.ButtonID == 371)
                {
                    InvokeCommand("deathclose2", from);
                }
                else if (info.ButtonID == 272)
                {
                    InvokeCommand("magetool1", from);
                }
                else if (info.ButtonID == 372)
                {
                    InvokeCommand("mageclose1", from);
                }
                else if (info.ButtonID == 273)
                {
                    InvokeCommand("magetool2", from);
                }
                else if (info.ButtonID == 373)
                {
                    InvokeCommand("mageclose2", from);
                }
                else if (info.ButtonID == 274)
                {
                    InvokeCommand("magetool3", from);
                }
                else if (info.ButtonID == 374)
                {
                    InvokeCommand("mageclose3", from);
                }
                else if (info.ButtonID == 275)
                {
                    InvokeCommand("magetool4", from);
                }
                else if (info.ButtonID == 375)
                {
                    InvokeCommand("mageclose4", from);
                }
                else if (info.ButtonID == 276)
                {
                    InvokeCommand("necrotool1", from);
                }
                else if (info.ButtonID == 376)
                {
                    InvokeCommand("necroclose1", from);
                }
                else if (info.ButtonID == 277)
                {
                    InvokeCommand("necrotool2", from);
                }
                else if (info.ButtonID == 377)
                {
                    InvokeCommand("necroclose2", from);
                }
                else if (info.ButtonID == 278)
                {
                    InvokeCommand("holytool1", from);
                }
                else if (info.ButtonID == 378)
                {
                    InvokeCommand("holyclose1", from);
                }
                else if (info.ButtonID == 279)
                {
                    InvokeCommand("holytool2", from);
                }
                else if (info.ButtonID == 379)
                {
                    InvokeCommand("holyclose2", from);
                }
                else if (info.ButtonID == 280)
                {
                    InvokeCommand("monktool1", from);
                }
                else if (info.ButtonID == 380)
                {
                    InvokeCommand("monkclose1", from);
                }
                else if (info.ButtonID == 281)
                {
                    InvokeCommand("monktool2", from);
                }
                else if (info.ButtonID == 381)
                {
                    InvokeCommand("monkclose2", from);
                }
                else if (info.ButtonID == 282)
                {
                    InvokeCommand("elementtool1", from);
                }
                else if (info.ButtonID == 382)
                {
                    InvokeCommand("elementclose1", from);
                }
                else if (info.ButtonID == 283)
                {
                    InvokeCommand("elementtool2", from);
                }
                else if (info.ButtonID == 383)
                {
                    InvokeCommand("elementclose2", from);
                }
            }
            else
            {
                switch (info.ButtonID)
                {
                    case 0: // Close/Cancel
                    {
                        //from.SendLocalizedMessage( 501235, "", 0x35 ); // Help request aborted.
                        break;
                    }
                    case 1: // MAIN
                    {
                        from.SendGump(new Server.Engines.Help.HelpGump(from, info.ButtonID));
                        break;
                    }
                    case 2: // AFK
                    {
                        InvokeCommand("afk", from);
                        from.SendGump(new Server.Engines.Help.HelpGump(from, info.ButtonID));
                        break;
                    }
                    case 3: // Chat
                    {
                        InvokeCommand("c", from);
                        break;
                    }
                    case 4: // Corpse Clear
                    {
                        InvokeCommand("corpseclear", from);
                        from.SendGump(new Server.Engines.Help.HelpGump(from, info.ButtonID));
                        break;
                    }
                    case 5: // Corpse Search
                    {
                        InvokeCommand("corpse", from);
                        break;
                    }
                    case 6: // Emote
                    {
                        InvokeCommand("emote", from);
                        break;
                    }
                    case 7: // Magic
                    {
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                        break;
                    }
                    case 8: // Moongate
                    {
                        InvokeCommand("magicgate", from);
                        break;
                    }
                    case 9: // MOTD
                    {
                        from.CloseGump(typeof(Joeku.MOTD.MOTD_Gump));
                        Joeku.MOTD.MOTD_Utility.SendGump(from, false, 0, 1);
                        break;
                    }
                    case 10: // Quests
                    {
                        from.SendGump(new Server.Engines.Help.HelpGump(from, info.ButtonID));
                        break;
                    }
                    case 11: // Quick Bar
                    {
                        from.CloseGump(typeof(QuickBar));
                        from.SendGump(new QuickBar(from));
                        break;
                    }
                    case 62: // Reagent Bar
                    {
                        from.CloseGump(typeof(RegBar));
                        from.SendGump(new RegBar(from));
                        break;
                    }
                    case 12: // Settings
                    {
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 13: // Library
                    {
                        from.CloseGump(typeof(MyLibrary));
                        from.SendSound(0x4A);
                        from.SendGump(new MyLibrary(from, 1));
                        break;
                    }
                    case 14: // Statistics
                    {
                        from.CloseGump(typeof(Server.Statistics.StatisticsGump));
                        from.SendGump(new Server.Statistics.StatisticsGump(from, 1));
                        break;
                    }
                    case 15: // Stuck
                    {
                        BaseHouse house = BaseHouse.FindHouseAt(from);

                        if (house != null && house.IsAosRules)
                        {
                            from.Location = house.BanLocation;
                        }
                        else if (from.Region.IsPartOf(typeof(Server.Regions.Jail)))
                        {
                            from.SendLocalizedMessage(1041530, "", 0x35); // You'll need a better jailbreak plan then that!
                        }
                        else if (
                            from.CanUseStuckMenu()
                            && from.Region.CanUseStuckMenu(from)
                            && !CheckCombat(from)
                            && !from.Frozen
                            && !from.Criminal
                            && (Core.AOS || from.Kills < 5)
                        )
                        {
                            StuckMenu menu = new StuckMenu(from, from, true);

                            menu.BeginClose();

                            from.SendGump(menu);
                        }

                        break;
                    }
                    case 16: // Weapon Abilities
                    {
                        InvokeCommand("sad", from);
                        break;
                    }
                    case 17: // Wealth Bar
                    {
                        from.CloseGump(typeof(WealthBar));
                        from.SendGump(new WealthBar(from));
                        break;
                    }
                    case 18: // Conversations
                    {
                        from.CloseGump(typeof(MyChat));
                        from.SendSound(0x4A);
                        from.SendGump(new MyChat(from, 1));
                        break;
                    }
                    case 19: // Versions
                    {
                        from.SendGump(new Server.Engines.Help.HelpGump(from, info.ButtonID));
                        break;
                    }
                    case 51: // Weapon Ability Names
                    {
                        if (((PlayerMobile)from).CharacterWepAbNames != 1)
                        {
                            ((PlayerMobile)from).CharacterWepAbNames = 1;
                        }
                        else
                        {
                            ((PlayerMobile)from).CharacterWepAbNames = 0;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 52: // Auto Sheathe
                    {
                        if (((PlayerMobile)from).CharacterSheath == 1)
                        {
                            ((PlayerMobile)from).CharacterSheath = 0;
                        }
                        else
                        {
                            ((PlayerMobile)from).CharacterSheath = 1;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 53: // Musical
                    {
                        string tunes = ((PlayerMobile)from).CharMusical;

                        if (tunes == "Forest")
                        {
                            ((PlayerMobile)from).CharMusical = "Dungeon";
                        }
                        else
                        {
                            ((PlayerMobile)from).CharMusical = "Forest";
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 54: // Private
                    {
                        PlayerMobile pm = (PlayerMobile)from;

                        if (pm.PublicMyRunUO == false)
                        {
                            pm.PublicMyRunUO = true;
                        }
                        else
                        {
                            pm.PublicMyRunUO = false;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 55: // Loot
                    {
                        from.CloseGump(typeof(LootChoices));
                        from.SendGump(new LootChoices(from, 1));
                        break;
                    }
                    case 56: // Skill Titles
                    {
                        from.CloseGump(typeof(SkillTitleGump));
                        from.SendGump(new SkillTitleGump(from));
                        break;
                    }
                    case 982: // Skill List
                    {
                        if (((PlayerMobile)from).SkillDisplay > 0)
                        {
                            ((PlayerMobile)from).SkillDisplay = 0;
                        }
                        else
                        {
                            ((PlayerMobile)from).SkillDisplay = 1;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        Server.Gumps.SkillListingGump.RefreshSkillList(from);
                        break;
                    }
                    case 983: // Open Skill List
                    {
                        Server.Gumps.SkillListingGump.OpenSkillList(from);
                        break;
                    }
                    case 985: // Gump Images
                    {
                        int gump = ((PlayerMobile)from).GumpHue;

                        if (gump > 0)
                        {
                            ((PlayerMobile)from).GumpHue = 0;
                        }
                        else
                        {
                            ((PlayerMobile)from).GumpHue = 1;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 986: // Weapon Ability Auto-Open
                    {
                        int wep = ((PlayerMobile)from).WeaponBarOpen;

                        if (wep > 0)
                        {
                            ((PlayerMobile)from).WeaponBarOpen = 0;
                        }
                        else
                        {
                            ((PlayerMobile)from).WeaponBarOpen = 1;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 989: // Creature Magic Focus
                    {
                        if (from.RaceMagicSchool == 0)
                            from.RaceMagicSchool = 1;
                        else if (from.RaceMagicSchool == 1)
                            from.RaceMagicSchool = 2;
                        else if (from.RaceMagicSchool == 2)
                            from.RaceMagicSchool = 3;
                        else
                            from.RaceMagicSchool = 0;

                        if (from.FindItemOnLayer(Layer.Special) != null)
                        {
                            if (from.FindItemOnLayer(Layer.Special) is BaseRace)
                                Server.Items.BaseRace.SetMonsterMagic(
                                    from,
                                    (BaseRace)(from.FindItemOnLayer(Layer.Special))
                                );
                        }

                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 990: // Creature Type Choice
                    {
                        from.RaceSection = 1;
                        from.SendGump(new Server.Items.RacePotions.RacePotionsGump(from, 1));
                        break;
                    }
                    case 991: // Creature Sounds
                    {
                        if (!from.RaceMakeSounds)
                            from.RaceMakeSounds = true;
                        else
                            from.RaceMakeSounds = false;

                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 57: // Normal Play
                    {
                        ((PlayerMobile)from).CharacterEvil = 0;
                        ((PlayerMobile)from).CharacterOriental = 0;
                        ((PlayerMobile)from).CharacterBarbaric = 0;
                        Server.Items.BarbaricSatchel.GetRidOf(from);
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 58: // Evil Play
                    {
                        ((PlayerMobile)from).CharacterEvil = 1;
                        ((PlayerMobile)from).CharacterOriental = 0;
                        ((PlayerMobile)from).CharacterBarbaric = 0;
                        Server.Items.BarbaricSatchel.GetRidOf(from);
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 59: // Oriental Play
                    {
                        ((PlayerMobile)from).CharacterEvil = 0;
                        ((PlayerMobile)from).CharacterOriental = 1;
                        ((PlayerMobile)from).CharacterBarbaric = 0;
                        Server.Items.BarbaricSatchel.GetRidOf(from);
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 60: // Message Color
                    {
                        if (from.RainbowMsg)
                        {
                            from.RainbowMsg = false;
                        }
                        else
                        {
                            from.RainbowMsg = true;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 61: // Auto Attack
                    {
                        if (from.NoAutoAttack)
                        {
                            from.NoAutoAttack = false;
                        }
                        else
                        {
                            from.NoAutoAttack = true;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 984: // Barbaric Play
                    {
                        if (((PlayerMobile)from).CharacterBarbaric == 1 && from.Female)
                        {
                            ((PlayerMobile)from).CharacterBarbaric = 2;
                        }
                        else if (((PlayerMobile)from).CharacterBarbaric > 0)
                        {
                            ((PlayerMobile)from).CharacterBarbaric = 0;
                            Server.Items.BarbaricSatchel.GetRidOf(from);
                        }
                        else
                        {
                            ((PlayerMobile)from).CharacterEvil = 0;
                            ((PlayerMobile)from).CharacterOriental = 0;
                            ((PlayerMobile)from).CharacterBarbaric = 1;
                            Server.Items.BarbaricSatchel.GivePack(from);
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 64: // Poisoning
                    {
                        if (((PlayerMobile)from).ClassicPoisoning == 1)
                        {
                            ((PlayerMobile)from).ClassicPoisoning = 0;
                        }
                        else
                        {
                            ((PlayerMobile)from).ClassicPoisoning = 1;
                        }
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 65: // Music Playlist
                    {
                        from.CloseGump(typeof(MusicPlaylist));
                        from.SendGump(new MusicPlaylist(from, 1));
                        break;
                    }
                    case 66: // SPELL BARS BELOW ---------------------------------------
                    {
                        from.CloseGump(typeof(SetupBarsBard1));
                        from.SendGump(new SetupBarsBard1(from, 1));
                        break;
                    }
                    case 67:
                    {
                        from.CloseGump(typeof(SetupBarsBard2));
                        from.SendGump(new SetupBarsBard2(from, 1));
                        break;
                    }
                    case 68:
                    {
                        from.CloseGump(typeof(SetupBarsKnight1));
                        from.SendGump(new SetupBarsKnight1(from, 1));
                        break;
                    }
                    case 69:
                    {
                        from.CloseGump(typeof(SetupBarsKnight2));
                        from.SendGump(new SetupBarsKnight2(from, 1));
                        break;
                    }
                    case 70:
                    {
                        from.CloseGump(typeof(SetupBarsDeath1));
                        from.SendGump(new SetupBarsDeath1(from, 1));
                        break;
                    }
                    case 71:
                    {
                        from.CloseGump(typeof(SetupBarsDeath2));
                        from.SendGump(new SetupBarsDeath2(from, 1));
                        break;
                    }
                    case 72:
                    {
                        from.CloseGump(typeof(SetupBarsMage1));
                        from.SendGump(new SetupBarsMage1(from, 1));
                        break;
                    }
                    case 73:
                    {
                        from.CloseGump(typeof(SetupBarsMage2));
                        from.SendGump(new SetupBarsMage2(from, 1));
                        break;
                    }
                    case 74:
                    {
                        from.CloseGump(typeof(SetupBarsMage3));
                        from.SendGump(new SetupBarsMage3(from, 1));
                        break;
                    }
                    case 75:
                    {
                        from.CloseGump(typeof(SetupBarsMage4));
                        from.SendGump(new SetupBarsMage4(from, 1));
                        break;
                    }
                    case 76:
                    {
                        from.CloseGump(typeof(SetupBarsNecro1));
                        from.SendGump(new SetupBarsNecro1(from, 1));
                        break;
                    }
                    case 77:
                    {
                        from.CloseGump(typeof(SetupBarsNecro2));
                        from.SendGump(new SetupBarsNecro2(from, 1));
                        break;
                    }
                    case 78:
                    {
                        from.CloseGump(typeof(SetupBarsPriest1));
                        from.SendGump(new SetupBarsPriest1(from, 1));
                        break;
                    }
                    case 79:
                    {
                        from.CloseGump(typeof(SetupBarsPriest2));
                        from.SendGump(new SetupBarsPriest2(from, 1));
                        break;
                    }
                    case 80:
                    {
                        from.SendGump(new CustomTitleGump(from));
                        break;
                    }
                    case 980:
                    {
                        from.CloseGump(typeof(SetupBarsMonk1));
                        from.SendGump(new SetupBarsMonk1(from, 1));
                        break;
                    }
                    case 981:
                    {
                        from.CloseGump(typeof(SetupBarsMonk2));
                        from.SendGump(new SetupBarsMonk2(from, 1));
                        break;
                    }
                    case 978:
                    {
                        from.CloseGump(typeof(SetupBarsElement1));
                        from.SendGump(new SetupBarsElement1(from, 1));
                        break;
                    }
                    case 979:
                    {
                        from.CloseGump(typeof(SetupBarsElement2));
                        from.SendGump(new SetupBarsElement2(from, 1));
                        break;
                    }
                    case 500:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x47E;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 501:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x94E;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 502:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x48D;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 503:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x48E;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 504:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x48F;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 505:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x490;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 506:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0x491;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                    case 507:
                    {
                        ((PlayerMobile)from).MagerySpellHue = 0;
                        from.SendGump(new Server.Engines.Help.HelpGump(from, 12));
                        break;
                    }
                }
            }
        }

        public static string MyQuests(Mobile from)
        {
            PlayerMobile pm = (PlayerMobile)from;

            string sQuests =
                "Below is a brief list of current quests, along with achievements in specific discoveries. These are owned quests, which are specific to your character. Other quests (like messages in a bottle, treasure maps, or scribbled notes) are not listed here.<br><br>";

            string ContractQuest = PlayerSettings.GetQuestInfo(from, "StandardQuest");
            if (PlayerSettings.GetQuestState(from, "StandardQuest"))
            {
                string sAdventurer = StandardQuestFunctions.QuestStatus(from);
                sQuests = sQuests + "-" + sAdventurer + ".<br><br>";
            }

            string ContractKiller = PlayerSettings.GetQuestInfo(from, "AssassinQuest");
            if (PlayerSettings.GetQuestState(from, "AssassinQuest"))
            {
                string sAssassin = AssassinFunctions.QuestStatus(from);
                sQuests = sQuests + "-" + sAssassin + ".<br><br>";
            }

            string ContractSailor = PlayerSettings.GetQuestInfo(from, "FishingQuest");
            if (PlayerSettings.GetQuestState(from, "FishingQuest"))
            {
                string sSailor = FishingQuestFunctions.QuestStatus(from);
                sQuests = sQuests + "-" + sSailor + ".<br><br>";
            }

            sQuests = sQuests + OtherQuests(from);

            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleMadGodName"))
            {
                sQuests = sQuests + "-Learned about the Mad God Tarjan.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleCatacombKey"))
            {
                sQuests =
                    sQuests
                    + "-The priest from the Mad God Temple gave me the key to the Catacombs.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleSpectreEye"))
            {
                sQuests = sQuests + "-Found a mysterious eye from the Catacombs.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleHarkynKey"))
            {
                sQuests = sQuests + "-Found a key with a symbol of a dragon on it.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleDragonKey"))
            {
                sQuests = sQuests + "-Found a rusty key from around a gray dragon's neck.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleCrystalSword"))
            {
                sQuests = sQuests + "-Found a crystal sword.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleSilverSquare"))
            {
                sQuests = sQuests + "-Found a silver square.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleKylearanKey"))
            {
                sQuests = sQuests + "-Found a key with a symbol of a unicorn on it.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleBedroomKey"))
            {
                sQuests = sQuests + "-Found a key with a symbol of a tree on it.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleSilverTriangle"))
            {
                sQuests = sQuests + "-Found a silver triangle.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleCrystalGolem"))
            {
                sQuests = sQuests + "-Destroyed the crystal golem and found a golden key.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleEbonyKey"))
            {
                sQuests =
                    sQuests + "-Kylearan gave me an ebony key with a demon symbol on it.<br><br>";
            }
            if (PlayerSettings.GetBardsTaleQuest(from, "BardsTaleSilverCircle"))
            {
                sQuests = sQuests + "-Found a silver circle.<br><br>";
            }
            if (
                PlayerSettings.GetBardsTaleQuest(from, "BardsTaleWin")
                && ((PlayerMobile)from).Profession != 1
            )
            {
                sQuests =
                    sQuests + "-Defeated the evil wizard Mangar and escaped Skara Brae.<br><br>";
            }

            if (PlayerSettings.GetKeys(from, "UndermountainKey"))
            {
                sQuests = sQuests + "-Found a key made of dwarven steel.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "BlackKnightKey"))
            {
                sQuests = sQuests + "-Found the Black Knight's key.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "SkullGate"))
            {
                sQuests =
                    sQuests
                    + "-Discovered the secret of Skull Gate.<br>   One is in the Undercity of Umbra in Sosaria.<br>   The other is in the Ravendark Woods.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "SerpentPillars"))
            {
                sQuests =
                    sQuests
                    + "-Discovered the secret of the Serpent Pillars.<br>   Sosaria: 86° 41'S, 124° 39'E<br>   Lodoria: 35° 36'S, 65° 2'E<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "RangerOutpost"))
            {
                sQuests = sQuests + "-Discovered the Ranger Outpost.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "DragonRiding"))
            {
                sQuests = sQuests + "-Learned the secrets of riding draconic creatures.<br><br>";
            }

            if (PlayerSettings.GetDiscovered(from, "the Land of Sosaria"))
            {
                sQuests = sQuests + "-Discovered the World of Sosaria.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Island of Umber Veil"))
            {
                sQuests = sQuests + "-Discovered Umber Veil.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Land of Ambrosia"))
            {
                sQuests = sQuests + "-Discovered Ambrosia.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Land of Lodoria"))
            {
                sQuests = sQuests + "-Discovered the Elven World of Lodoria.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Serpent Island"))
            {
                sQuests = sQuests + "-Discovered the Serpent Island.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Isles of Dread"))
            {
                sQuests = sQuests + "-Discovered the Isles of Dread.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Savaged Empire"))
            {
                sQuests = sQuests + "-Discovered the Valley of the Savaged Empire.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Bottle World of Kuldar"))
            {
                sQuests = sQuests + "-Discovered the Bottle World of Kuldar.<br><br>";
            }
            if (PlayerSettings.GetDiscovered(from, "the Underworld"))
            {
                sQuests = sQuests + "-Discovered the Underworld.<br><br>";
            }

            return "Quests For " + from.Name + "<br><br>" + sQuests;
        }

        public static string OtherQuests(Mobile from)
        {
            string quests = "";

            ArrayList targets = new ArrayList();
            foreach (Item item in World.Items.Values)
            {
                if (item is ThiefNote)
                {
                    if (((ThiefNote)item).NoteOwner == from)
                    {
                        if (Server.Items.ThiefNote.ThiefAllowed(from) == null)
                        {
                            quests = quests + "-" + ((ThiefNote)item).NoteStory + "<br><br>";
                        }
                        else
                        {
                            quests =
                                quests
                                + "-You have a secret note instructing you to steal something, but you will take a break from thieving and read it in about "
                                + Server.Items.ThiefNote.ThiefAllowed(from)
                                + " minutes.<br><br>";
                        }
                    }
                }
                else if (item is CourierMail)
                {
                    if (((CourierMail)item).Owner == from)
                    {
                        quests =
                            quests
                            + "-You need to find "
                            + ((CourierMail)item).SearchItem
                            + " for "
                            + ((CourierMail)item).ForWho
                            + ". They said in their letter that you should search in "
                            + ((CourierMail)item).SearchDungeon
                            + " in "
                            + ((CourierMail)item).SearchWorld
                            + ".<br><br>";
                    }
                }
                else if (item is SearchPage)
                {
                    if (((SearchPage)item).Owner == from)
                    {
                        quests =
                            quests
                            + "-You want to find "
                            + ((SearchPage)item).SearchItem
                            + " in "
                            + ((SearchPage)item).SearchDungeon
                            + " in "
                            + ((SearchPage)item).SearchWorld
                            + ".<br><br>";
                    }
                }
                else if (item is SummonPrison)
                {
                    if (((SummonPrison)item).owner == from)
                    {
                        quests =
                            quests
                            + "-You currently have "
                            + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(
                                ((SummonPrison)item).Prisoner.ToLower()
                            )
                            + " in a Magical Prison.<br><br>";
                    }
                }
                else if (item is FrankenJournal)
                {
                    if (((FrankenJournal)item).JournalOwner == from)
                    {
                        int parts = 0;
                        if (((FrankenJournal)item).HasArmRight > 0)
                        {
                            parts++;
                        }
                        if (((FrankenJournal)item).HasArmLeft > 0)
                        {
                            parts++;
                        }
                        if (((FrankenJournal)item).HasLegRight > 0)
                        {
                            parts++;
                        }
                        if (((FrankenJournal)item).HasLegLeft > 0)
                        {
                            parts++;
                        }
                        if (((FrankenJournal)item).HasTorso > 0)
                        {
                            parts++;
                        }
                        if (((FrankenJournal)item).HasHead > 0)
                        {
                            parts++;
                        }

                        quests =
                            quests
                            + "-You currently have "
                            + parts
                            + " out of 6 body parts needed to create a flesh golem.<br><br>";
                    }
                }
                else if (item is RuneBox)
                {
                    if (((RuneBox)item).RuneBoxOwner == from)
                    {
                        int runes = 0;
                        if (((RuneBox)item).HasCompassion > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasHonesty > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasHonor > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasHumility > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasJustice > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasSacrifice > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasSpirituality > 0)
                        {
                            runes++;
                        }
                        if (((RuneBox)item).HasValor > 0)
                        {
                            runes++;
                        }

                        quests =
                            quests
                            + "-You currently have "
                            + runes
                            + " out of 8 runes of virtue.<br><br>";
                    }
                }
                else if (item is SearchPage)
                {
                    if (((SearchPage)item).owner == from)
                    {
                        quests =
                            quests
                            + "-You are on a quest to obtain the "
                            + ((SearchPage)item).SearchItem
                            + ".<br><br>";
                    }
                }
                else if (item is VortexCube)
                {
                    if (((VortexCube)item).CubeOwner == from)
                    {
                        VortexCube cube = (VortexCube)item;
                        quests =
                            quests + "-You are searching for the Codex of Ultimate Wisdom.<br>";

                        if (cube.HasConvexLense > 0)
                        {
                            quests = quests + "   -You have the Convex Lense.<br>";
                        }
                        if (cube.HasConcaveLense > 0)
                        {
                            quests = quests + "   -You have the Concave Lense.<br>";
                        }

                        if (cube.HasKeyLaw > 0)
                        {
                            quests = quests + "   -You have the Key of Law.<br>";
                        }
                        if (cube.HasKeyBalance > 0)
                        {
                            quests = quests + "   -You have the Key of Balance.<br>";
                        }
                        if (cube.HasKeyChaos > 0)
                        {
                            quests = quests + "   -You have the Key of Chaos.<br>";
                        }

                        if (cube.HasCrystalRed > 0)
                        {
                            quests = quests + "   -You have the Red Void Crystal.<br>";
                        }
                        if (cube.HasCrystalBlue > 0)
                        {
                            quests = quests + "   -You have the Blue Void Crystal.<br>";
                        }
                        if (cube.HasCrystalGreen > 0)
                        {
                            quests = quests + "   -You have the Green Void Crystal.<br>";
                        }
                        if (cube.HasCrystalYellow > 0)
                        {
                            quests = quests + "   -You have the Yellow Void Crystal.<br>";
                        }
                        if (cube.HasCrystalWhite > 0)
                        {
                            quests = quests + "   -You have the White Void Crystal.<br>";
                        }
                        if (cube.HasCrystalPurple > 0)
                        {
                            quests = quests + "   -You have the Purple Void Crystal.<br>";
                        }

                        quests = quests + "<br>";
                    }
                }
                else if (item is ObeliskTip)
                {
                    if (((ObeliskTip)item).ObeliskOwner == from)
                    {
                        ObeliskTip obelisk = (ObeliskTip)item;
                        quests = quests + "-You are trying to become a Titan of Ether.<br>";
                        quests = quests + "   -You have the Obelisk Tip.<br>";

                        if (obelisk.WonAir > 0)
                        {
                            quests =
                                quests + "   -You have defeated Stratos, the Titan of Air.<br>";
                        }
                        else if (obelisk.HasAir > 0)
                        {
                            quests = quests + "   -You have the Breath of Air.<br>";
                        }
                        if (obelisk.WonFire > 0)
                        {
                            quests = quests + "   -You have defeated Pyros, the Titan of Fire.<br>";
                        }
                        else if (obelisk.HasFire > 0)
                        {
                            quests = quests + "   -You have the Tongue of Flame.<br>";
                        }
                        if (obelisk.WonEarth > 0)
                        {
                            quests =
                                quests + "   -You have defeated Lithos, the Titan of Earth.<br>";
                        }
                        else if (obelisk.HasEarth > 0)
                        {
                            quests = quests + "   -You have the Heart of Earth.<br>";
                        }
                        if (obelisk.WonWater > 0)
                        {
                            quests =
                                quests + "   -You have defeated Hydros, the Titan of Water.<br>";
                        }
                        else if (obelisk.HasWater > 0)
                        {
                            quests = quests + "   -You have the Tear of the Seas.<br>";
                        }

                        quests = quests + "<br>";
                    }
                }
                else if (item is MuseumBook)
                {
                    if (((MuseumBook)item).ArtOwner == from)
                    {
                        quests =
                            quests
                            + "-You have found "
                            + MuseumBook.GetTotal((MuseumBook)item)
                            + " out of 60 antiques for the museum.<br><br>";
                    }
                }
                else if (item is RuneBox)
                {
                    if (((RuneBox)item).RuneBoxOwner == from)
                    {
                        int runes =
                            ((RuneBox)item).HasCompassion
                            + ((RuneBox)item).HasHonesty
                            + ((RuneBox)item).HasHonor
                            + ((RuneBox)item).HasHumility
                            + ((RuneBox)item).HasJustice
                            + ((RuneBox)item).HasSacrifice
                            + ((RuneBox)item).HasSpirituality
                            + ((RuneBox)item).HasValor;
                        quests =
                            quests
                            + "-You have found "
                            + runes
                            + " out of 8 runes of virtue.<br><br>";
                    }
                }
                else if (item is QuestTome)
                {
                    if (((QuestTome)item).QuestTomeOwner == from)
                    {
                        quests =
                            quests
                            + "-You are on a quest to find "
                            + ((QuestTome)item).GoalItem4
                            + ".<br><br>";
                    }
                }
            }

            if (
                from.Backpack.FindItemByType(typeof(ScalesOfEthicality)) != null
                || from.Backpack.FindItemByType(typeof(OrbOfLogic)) != null
                || from.Backpack.FindItemByType(typeof(LanternOfDiscipline)) != null
                || from.Backpack.FindItemByType(typeof(BlackrockSerpentOrder)) != null
                || from.Backpack.FindItemByType(typeof(BlackrockSerpentChaos)) != null
                || from.Backpack.FindItemByType(typeof(BlackrockSerpentBalance)) != null
            )
            {
                quests =
                    quests + "-You are on a quest to bring the Serpents back into balance.<br><br>";
            }

            if (
                from.Backpack.FindItemByType(typeof(ShardOfFalsehood)) != null
                || from.Backpack.FindItemByType(typeof(ShardOfCowardice)) != null
                || from.Backpack.FindItemByType(typeof(ShardOfHatred)) != null
                || from.Backpack.FindItemByType(typeof(CandleOfLove)) != null
                || from.Backpack.FindItemByType(typeof(BookOfTruth)) != null
                || from.Backpack.FindItemByType(typeof(BellOfCourage)) != null
            )
            {
                quests =
                    quests
                    + "-You are on a quest to destroy the Shadowlords and construct a Gem of Immortality.<br><br>";
            }
            else if (from.Backpack.FindItemByType(typeof(GemImmortality)) != null)
            {
                quests = quests + "-You have constructed a Gem of Immortality.<br><br>";
            }

            if (PlayerSettings.GetKeys(from, "Museums"))
            {
                quests = quests + "-You have found all of the antiques for the Museum.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Gygax"))
            {
                quests = quests + "-You have obtained the Statue of Gygax.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Virtues"))
            {
                quests = quests + "-You have cleansed all of the Runes of Virtue.<br><br>";
            }
            else if (PlayerSettings.GetKeys(from, "Corrupt"))
            {
                quests = quests + "-You have corrupted all of the Runes of Virtue.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Exodus"))
            {
                quests = quests + "-You have destroyed the Core of Exodus.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "BlackGateDemon"))
            {
                quests =
                    quests
                    + "-You have defeated the Black Gate Demon and found a portal to the Ethereal Plane.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Jormungandr"))
            {
                quests =
                    quests
                    + "-You have defeated the legendary serpent known as Jormungandr.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Dracula"))
            {
                quests = quests + "-You have destroyed Dracula, the ruler of all vampires.<br><br>";
            }
            if (
                from.Backpack.FindItemByType(typeof(StaffPartVenom)) != null
                || from.Backpack.FindItemByType(typeof(StaffPartCaddellite)) != null
                || from.Backpack.FindItemByType(typeof(StaffPartFire)) != null
                || from.Backpack.FindItemByType(typeof(StaffPartLight)) != null
                || from.Backpack.FindItemByType(typeof(StaffPartEnergy)) != null
            )
            {
                quests =
                    quests + "-You are seeking to assemble the Staff of Ultimate Power.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Arachnar"))
            {
                quests = quests + "-You have defeated Arachnar, the guardian of the staff.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Surtaz"))
            {
                quests = quests + "-You have defeated Surtaz, the guardian of the staff.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Vordinax"))
            {
                quests = quests + "-You have defeated Vordinax, the guardian of the staff.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Vulcrum"))
            {
                quests = quests + "-You have defeated Vulcrum, the guardian of the staff.<br><br>";
            }
            if (PlayerSettings.GetKeys(from, "Xurtzar"))
            {
                quests = quests + "-You have defeated Xurtzar, the guardian of the staff.<br><br>";
            }

            return quests;
        }

        public static string MyHelp()
        {
            string HelpText =
                "If you are looking for help exploring this world, you can learn about almost anything within the game world you travel. Some merchants sell scrolls or books that will explain how some skills can be performed, resources gathered, and even how elements of the world can be manipulated. A sage often sells many tomes of useful information on skills, weapon abilities, or various types of magics available. If you are totally new to this game, buy yourself a Guide to Adventure book from a sage if you lost the one you started with. This book explains how to navigate and play the game. You will also learn some things about how the world behaves such as merchant interactions, how to use items, and what to do when your character dies. Talk to the townsfolk to learn whatever you can. On this screen there are many options, information, and settings that can assist in your journey. Many of the options here have keyboard commands that are listed below. Make sure to check out the 'Info' section on your character's paperdoll as it has some vital information about your character.<br><br>"
                + "Common Commands: Below are the commands you can use for various things in the game.<br><br>"
                + "[abilitynames - Turns on/off the special weapon ability names next to the appropriate icons.<BR><BR>"
                + "[afk - Turns on/off the notification to others that you are away from keyboard.<BR><BR>"
                + "[autoattack - Turns on/off whether you auto attack when attacked.<BR><BR>"
                + "[bandother - Bandage other command.<BR><BR>"
                + "[bandself - Bandage self command.<BR><BR>"
                + "[barbaric - Turns on/off the barbaric flavor the game provides (see end).<BR><BR>"
                + "[c - Initiates the chat system.<BR><BR>"
                + "[corpse - Helps one find their remains.<BR><BR>"
                + "[cleardeck - Removes npc corpses and empty player corpses from your boat deck.<BR><BR>"
                + "[e - Opens the emote mini window.<BR><BR>"
                + "[emote - Opens the emote window.<BR><BR>"
                + "[evil - Turns on/off the evil flavor the game provides (see end).<BR><BR>"
                + "[loot - Automatically take certain items from common dungeon chests or corpses and put them in your backpack. The unknown items are those that will need identification, but you may decide to take them anyway. The reagent options have a few categories. Magery and necromancer reagents are those used specifically by those characters, where witches brew reagents mainly fall into the necromancer category. Alchemic reagents are those that fall outside the category of magery and necromancer reagents, and only alchemists use them. Herbalist reagents are useful in druidic herbalism.<BR><BR>"
                + "[magicgate - Helps one find the nearest magical gate.<BR><BR>"
                + "[motd - Opens the message of the day.<BR><BR>"
                + "[oriental - Turns on/off the oriental flavor the game provides (see end).<BR><BR>"
                + "[password - Change your account password.<BR><BR>"
                + "[poisons - This changes how poisoned weapons work, which can be for either precise control with special weapon infectious strikes (default) or with hits of a one-handed slashing or piercing weapon.<BR><BR>"
                + "[private - Turns on/off detailed messages of your journey for the town crier and local citizen chatter.<BR><BR>"
                + "[quests - Opens a scroll to show certain quest events.<BR><BR>"
                + "[quickbar - Opens a small, vertical bar with common game functions for easier use.<br><br>"
                + "[sad - Opens the weapon's special abilities.<BR><BR>"
                + "[set1 - Sets your weapon's first ability to active.<BR><BR>"
                + "[set2 - Sets your weapon's second ability to active.<BR><BR>"
                + "[set3 - Sets your weapon's third ability to active.<BR><BR>"
                + "[set4 - Sets your weapon's fourth ability to active.<BR><BR>"
                + "[set5 - Sets your weapon's fifth ability to active.<BR><BR>"
                + "[sheathe - Turns on/off the feature to sheathe your weapon when not in battle.<BR><BR>"
                + "[skill - Shows you what each skill is used for.<BR><BR>"
                + "[skilllist - Displays a more condensed list of skills you have set to 'up' and perhaps 'locked'.<BR><BR>"
                + "[spellhue ## - This command, following by a color reference hue number, will change all of your magery spell effects to that color. A value of '1' will normally render as '0' so avoid that setting as it will not produce the result you may want.<BR><BR>"
                + "[statistics - Shows you some statistics of the server.<BR><BR>"
                + "[wealth - Opens a small, horizontal bar showing your gold value for the various forms of currency and gold in your bank and backpack. Currency are items you would have a banker convert to gold for you (silver, copper, xormite, jewels, and crystals). If you put these items in your bank, you can update the values on the wealth bar by right clicking on it.<BR><BR>"
                + "<br><br>"
                + "Area Difficulty Levels: When you enter dangerous areas, you will receive a message indicating the specific area you have entered. In addition, there may be a difficulty level indicated in parentheses to provide an indication of the area's difficulty. Below are descriptions for each level.<br><br>"
                + " - Easy (Not much of a challenge)<br><br>"
                + " - Normal (An average level of<br>"
                + "        challenge)<br><br>"
                + " - Difficult (A tad more difficult)<br><br>"
                + " - Challenging (You will probably<br>"
                + "        run away alot)<br><br>"
                + " - Hard (You will probably die alot)<br><br>"
                + " - Deadly (I dare you)<br><br>"
                + " - Epic (For Titans of Ether)<br><br>"
                + "<br><br>"
                + "Skill Titles: You can set your default title for your character. Although you may be a Grandmaster Driven, you may want your title to reflect your Apprentice Wizard title instead. This is how you set it...<br><br>"
                + "Type the '[SkillName' command followed by the name of the skill you want to set as your default. Make sure you surround the skill name in quotes and all lowercase. Example...<br>"
                + "  [SkillName \"taming\"<br><br>"
                + "If you want the game to manage your character's title, simply use the same command with a skill name of \"clear\".<br><br>"
                + "<br><br>"
                + "Reagent Bars: Below are the commands you can use to monitor your reagent quantities while casting spells or creating potions. These customizable bars display the quantities of the reagents you possess, updating them whenever you cast a spell or make a potion that utilizes them. Alternatively, you can create a macro for these commands and manually refresh the amounts.<br><br>"
                + "[regbar - Opens the reagent bar.<br><br>"
                + "[regclose - Closes the reagent bar.<br><br>"
                + "<br><br>"
                + "Magic Toolbars: Here are the commands you can utilize to manage magic toolbars, which can enhance your gameplay experience.<br><br>"
                + "[bardsong1 - Opens the 1st bard song bar editor.<br><br>"
                + "[bardsong2 - Opens the 2nd bard song bar editor.<br><br>"
                + "<br><br>"
                + "[knightspell1 - Opens the 1st knight spell bar editor.<br><br>"
                + "[knightspell2 - Opens the 2nd knight spell bar editor.<br><br>"
                + "<br><br>"
                + "[deathspell1 - Opens the 1st death knight spell bar editor.<br><br>"
                + "[deathspell2 - Opens the 2nd death knight spell bar editor.<br><br>"
                + "<br><br>"
                + "[elementspell1 - Opens the 1st elemental spell bar editor.<br><br>"
                + "[elementspell2 - Opens the 2nd elemental spell bar editor.<br><br>"
                + "<br><br>"
                + "[holyspell1 - Opens the 1st priest prayer bar editor.<br><br>"
                + "[holyspell2 - Opens the 2nd priest prayer bar editor.<br><br>"
                + "<br><br>"
                + "[magespell1 - Opens the 1st mage spell bar editor.<br><br>"
                + "[magespell2 - Opens the 2nd mage spell bar editor.<br><br>"
                + "[magespell3 - Opens the 3rd mage spell bar editor.<br><br>"
                + "[magespell4 - Opens the 4th mage spell bar editor.<br><br>"
                + "<br><br>"
                + "[monkspell1 - Opens the 1st monk ability bar editor.<br><br>"
                + "[monkspell2 - Opens the 2nd monk ability bar editor.<br><br>"
                + "<br><br>"
                + "[necrospell1 - Opens the 1st necromancer spell bar editor.<br><br>"
                + "[necrospell2 - Opens the 2nd necromancer spell bar editor.<br><br>"
                + "<br><br>"
                + "<br><br>"
                + "[bardtool1 - Opens the 1st bard song bar.<br><br>"
                + "[bardtool2 - Opens the 2nd bard song bar.<br><br>"
                + "<br><br>"
                + "[knighttool1 - Opens the 1st knight spell bar.<br><br>"
                + "[knighttool2 - Opens the 2nd knight spell bar.<br><br>"
                + "<br><br>"
                + "[deathtool1 - Opens the 1st death knight spell bar.<br><br>"
                + "[deathtool2 - Opens the 2nd death knight spell bar.<br><br>"
                + "<br><br>"
                + "[elementtool1 - Opens the 1st elemental spell bar.<br><br>"
                + "[elementtool2 - Opens the 2nd elemental spell bar.<br><br>"
                + "<br><br>"
                + "[holytool1 - Opens the 1st priest prayer bar.<br><br>"
                + "[holytool2 - Opens the 2nd priest prayer bar.<br><br>"
                + "<br><br>"
                + "[magetool1 - Opens the 1st mage spell bar.<br><br>"
                + "[magetool2 - Opens the 2nd mage spell bar.<br><br>"
                + "[magetool3 - Opens the 3rd mage spell bar.<br><br>"
                + "[magetool4 - Opens the 4th mage spell bar.<br><br>"
                + "<br><br>"
                + "[monktool1 - Opens the 1st monk ability bar.<br><br>"
                + "[monktool2 - Opens the 2nd monk ability bar.<br><br>"
                + "<br><br>"
                + "[necrotool1 - Opens the 1st necromancer spell bar.<br><br>"
                + "[necrotool2 - Opens the 2nd necromancer spell bar.<br><br>"
                + "<br><br>"
                + "<br><br>"
                + "[bardclose1 - Closes the 1st bard song bar.<br><br>"
                + "[bardclose2 - Closes the 2nd bard song bar.<br><br>"
                + "<br><br>"
                + "[knightclose1 - Closes the 1st knight spell bar.<br><br>"
                + "[knightclose2 - Closes the 2nd knight spell bar.<br><br>"
                + "<br><br>"
                + "[deathclose1 - Closes the 1st death knight spell bar.<br><br>"
                + "[deathclose2 - Closes the 2nd death knight spell bar.<br><br>"
                + "<br><br>"
                + "[elementclose1 - Closes the 1st elemental spell bar.<br><br>"
                + "[elementclose2 - Closes the 2nd elemental spell bar.<br><br>"
                + "<br><br>"
                + "[holyclose1 - Closes the 1st priest prayer bar.<br><br>"
                + "[holyclose2 - Closes the 2nd priest prayer bar.<br><br>"
                + "<br><br>"
                + "[mageclose1 - Closes the 1st mage spell bar.<br><br>"
                + "[mageclose2 - Closes the 2nd mage spell bar.<br><br>"
                + "[mageclose3 - Closes the 3rd mage spell bar.<br><br>"
                + "[mageclose4 - Closes the 4th mage spell bar.<br><br>"
                + "<br><br>"
                + "[monkclose1 - Closes the 1st monk ability bar.<br><br>"
                + "[monkclose2 - Closes the 2nd monk ability bar.<br><br>"
                + "<br><br>"
                + "[necroclose1 - Closes the 1st necromancer spell bar.<br><br>"
                + "[necroclose2 - Closes the 2nd necromancer spell bar.<br><br>"
                + "<br><br>"
                + "<br><br>"
                + "Music: There are many different pieces of classical music in the game, and they play depending on the areas you visit. Some of the music is from the original game, but there are also some pieces from older games. Additionally, there are some pieces from computer games in the 1990s that really fit the theme when traveling the land. You can choose to listen to them or change the music you are listening to when exploring the world. However, please note that when you change the music and enter a new area, the default music for that area will play, and you may have to change your music again. Also, keep in mind that your game client will play the current song for a few seconds before allowing a switch to new music. You can use the below command to open a window that allows you to choose a song to play. Most of them play in a loop, but there are three that do not, and they are marked with an asterisk. There are two pages of songs to choose from, so use the top arrow to navigate between each screen. Once your chosen music begins to play, press the OKAY button to exit the screen. Although it is an unnecessary function, it does give you some control over the music in the game.<br><br>"
                + "[music - Opens the music playlist and player.<br><br>"
                + "The following command will toggle your music preference, allowing you to play a different set of music in the dungeons. When enabled, it will play the music typically heard while traveling the land, rather than the usual dungeon music.<br><br>"
                + "[musical - Sets the default dungeon music.<br><br>"
                + "<br><br>"
                + "Evil Style: Some players are drawn to the dark side of the game, where they can indulge in evil activities. With classes like Necromancers, players can immerse themselves in a world that embraces this sinister flavor. This unique setting allows players to switch between regular and evil modes. In the evil mode, the treasures they discover often bear names that suit the malevolent theme. Moreover, if players maintain negative karma, some of their skill titles will change, although not all of them. To determine which titles will alter based on karma, players can consult the book of skill titles within the game world. Additionally, players may stumble upon relics that boast this distinctive style, perfect for decorating their homes in a dark fashion. This option can be toggled on and off at any time, but players can only have one play style active at a time.<br><br>"
                + "[evil - Turns on/off the evil flavor the game provides.<br><br>"
                + "<br><br>"
                + "Oriental Style: There is an Oriental element in the game that not all players want to experience. With classes like Ninja and Samurai, some players may prefer a world that incorporates this flavor. This specific setting allows you to switch between fantasy and Oriental themes. When in Oriental mode, a significant portion of the treasure you discover will have Chinese or Japanese historical origins. These items are often named to match the style and possessions that once belonged to someone will frequently have an Oriental-sounding name. Some skill titles will also change based on this play style, although not all of them. Take a look at the book of skill titles (found within the game world) to see which titles are affected by this play style. Additionally, you may come across relics and artwork that follow this style, which can be used to decorate your in-game home. This option can be toggled on and off at any time, but only one play style can be active at a time.<br><br>"
                + "[oriental - Turns on/off the oriental flavor the game provides.<br><br>"
                + "<br><br>"
                + "Barbaric Style: The default game does not lend itself to a sword and sorcery experience, making it suboptimal for players who want to embody a loin cloth-wearing barbarian wielding a massive axe. In order to maximize survivability, characters typically strive to acquire as much equipment as possible. However, this particular play style can be advantageous in that it introduces a satchel into the main pack. While the satchel cannot be used for storage, its purpose is to alter specific pieces of equipment that are placed inside. Shields, hats, helms, tunics, sleeves, leggings, boots, gorgets, gloves, necklaces, cloaks, and robes can all be transformed into visually distinct items that maintain the same functionality as their original counterparts. It's worth noting that when wearing robes, they will cover both the tunics and sleeves of the character. Similarly, if a sword and sorcery robe is worn, it will have the same effect, necessitating the removal of the robe to access the sleeves and/or tunic. This play style also introduces its own unique set of skill titles for various abilities. Female characters have the additional option of converting any 'Barbarian' titles to 'Amazon' by pressing a specific button. To learn more about this play style, players can open their satchel. Furthermore, this option can be toggled on or off at any time, although only one play style can be active simultaneously.<br><br>"
                + "[barbaric - Turns on/off the barbaric flavor the game provides.<br><br>"
                + "";

            return HelpText;
        }
    }
}

namespace Server.Gumps
{
    public class InfoHelpGump : Gump
    {
        public int m_Origin;

        public InfoHelpGump(Mobile from, int page, int origin)
            : base(50, 50)
        {
            m_Origin = origin;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            string title = "";
            string info = "";
            bool scrollbar = true;

            if (page == 82)
            {
                scrollbar = false;
                title = "Music Tone";
                info =
                    "This option simply toggles your music preference to play a different set of music in the dungeons. When turned on, it will play the music you normally hear when traveling the land, instead of the music commonly played in dungeons.";
            }
            else if (page == 83)
            {
                title = "Music Playlist";
                info =
                    "This provides a comprehensive list of the in-game music. You can choose the music you prefer, and those selections will play randomly as you move between regions. To listen to a song for review, click on the blue gem icon. Please be aware that there is a delay before you can start another song, so if you clicked on the blue gem too quickly, it may not respond. Wait a few seconds and try clicking on the blue gem again to see if the song starts playing. Playlists are disabled by default, so if you want your playlist to work, remember to enable it.";
            }
            else if (page == 84)
            {
                scrollbar = false;
                title = "Private Play";
                info =
                    "This option allows you to enable or disable the detailed messages regarding your journey for the town crier and local citizen chatter. By doing so, you can keep your activities private, ensuring that others will not be able to see the specific locations you are traveling to in the world.";
            }
            else if (page == 85)
            {
                title = "Loot Options";
                info =
                    "This feature allows you to choose from a variety of categories. When selected, items from these categories will be automatically transferred from common dungeon chests or corpses into your backpack. If you choose the 'coins' category, you will acquire wealth in the form of currency or gold nuggets. The 'gems and jewels' category includes gems, gemstones, jewelry, jewels, and crystals. The 'unknown items' category refers to items that require identification, but you have the option to take them regardless. The 'reagent' options consist of several categories. 'Magery' and 'necromancer' reagents are specifically used by characters with those abilities, while 'witches brew' reagents fall under the necromancer category. 'Alchemic' reagents are distinct from magery and necromancer reagents and are exclusively used by alchemists. Lastly, 'herbalist' reagents are beneficial for druidic herbalism.";
            }
            else if (page == 86)
            {
                title = "Classic Poisoning";
                info =
                    "There are two methods that assassins use to handle poisoned weapons. One is the simple method of soaking the blade and having it poison whenever it strikes their opponent. With this method, known as classic poisoning, there is little control over the dosage given, but it is easier to maneuver. When this option is turned off, there is the newer and more tactical method, where only certain weapons can be poisoned, and the assassin can control when the poison is administered with each hit. Although the tactical method requires more thought, it does have the potential to allow an assassin to poison specific arrows, for example. The choice of methods can be switched at any time, but only one method can be in use at a given time.";
            }
            else if (page == 87)
            {
                title = "Skill Title";
                info =
                    "If you do not specify your skill title here, the game will automatically assign your highest skill as your character's title. Selecting a skill here will make that profession your title. For example, if you always want to be known as a wizard, choose the 'Magery' option. You can let the game handle this by setting it to 'Auto Title' anytime. However, be cautious when selecting a skill. If you have no skill points in it, you will be titled 'the Village Idiot'. If you have at least 0.1 skill points, you will be titled 'Aspiring'.";
            }
            else if (page == 88)
            {
                scrollbar = false;
                title = "Message Color";
                info =
                    "By default, most of the messages appearing on the lower left of the screen are gray in color. Enabling this option will change those messages to have a random color whenever a new message appears. This feature can help some people more easily see such messages, and the varying colors can also help distinguish individual messages that may be scrolling by.";
            }
            else if (page == 89)
            {
                scrollbar = false;
                title = "Auto Attack";
                info =
                    "By default, when you are attacked, you will automatically retaliate. However, if you prefer to have control over when or if you retaliate, you can disable this option. Disabling it can be beneficial if you wish to avoid accidentally harming innocent beings or if you are attempting to tame an aggressive creature.";
            }
            else if (page == 92)
            {
                title = "Play Style - Normal";
                info =
                    "This is the default play style for "
                    + Server.Misc.ServerList.ServerName
                    + ". It is designed to provide players with a classic fantasy world experience. There are two additional play styles available: evil and oriental. These play styles do not alter the mechanics of the gameplay, but they do affect the types of treasure you discover and the henchmen you can hire. For instance, if you choose the 'evil' play style, you will come across treasure that is specifically tailored to that style. Instead of finding a blue 'mace of might', you would find a black 'mace of ghostly death'. These play styles serve as a means to customize your character's experience within the game.";
            }
            else if (page == 93)
            {
                title = "Play Style - Evil";
                info =
                    "There is an element of evil in the game that some players want to engage with. With classes like Necromancers, players may choose to explore a world with this added flavor. This particular setting allows you to switch between regular and evil modes. In the evil mode, the treasure you find often has names that suit the evil style. When you maintain negative karma, some of your skill titles will change, although not all of them. You can check the book of skill titles in the game world to see which ones are affected by karma. Additionally, some of the relics you find may also follow this style, allowing you to decorate your home in an evil fashion. This option can be toggled on and off at any time. However, you can only have one play style active at a time.<br><br>"
                    + "[evil - Turns on/off the evil flavor the game provides.";
            }
            else if (page == 94)
            {
                title = "Play Style - Oriental";
                info =
                    "There is an Oriental element in the game that many players do not wish to engage with. With classes like Ninja and Samurai, some players may be interested in exploring a world with this added flavor. This specific setting allows players to toggle between fantasy and Oriental themes. In Oriental mode, a significant portion of the treasure you find will have Chinese or Japanese historical origins. These items will often be named to match the Oriental style. Items that once belonged to someone will frequently have a name that aligns with the Oriental style as well. Some of the skill titles will also change based on this play style, although not all of them. You can review the book of skill titles (located within the game world) to see which titles will change when playing with this style. Additionally, some of the relics and artwork you discover will also have an Oriental style, allowing players to decorate their homes accordingly. This option can be enabled or disabled at any time, but you can only have one play style active at a time.";
            }
            else if (page == 95)
            {
                m_Origin = 7;
                title = "Magic Toolbars";
                info =
                    "These toolbars can be configured for all areas of magical-style spells in the game. Each school of magic has two separate toolbars that you can customize, except for magery, which has four available. The large number of spells for magery benefits from the extra two toolbars. These toolbars allow you to select spells that you like to cast often and set whether the bar will appear vertically or horizontally. If you choose to have the toolbar appear vertically, you have the additional option of showing the spell names next to the icons. These toolbars can be moved around, and you only need to single-click the appropriate icon to cast the spell. If you have spells selected for a toolbar but lack the spell in your spellbook, the icon will not appear when you open the toolbar. These toolbars cannot be closed by normal means to avoid the chance of accidentally closing them during combat. You can either use the command button available in the 'Help' section or the appropriate typed keyboard command.";
            }
            else if (page == 96)
            {
                scrollbar = false;
                title = "Magery Spell Color";
                info =
                    "You can modify the color of all your magery spell effects here. There are a limited number of options provided. Once set, your spells will display that color for every effect. To revert to the original color, select the 'Default' option. Alternatively, you can use the '[spellhue' command followed by a number to set a custom color of your choice.";
            }
            else if (page == 97)
            {
                scrollbar = false;
                title = "Custom Title";
                info =
                    "This feature enables you to input a personalized title for your character, instead of depending on the game to assign one based on your highest skill or the skill you choose to represent you. To reset a custom title you have set using this option, simply enter the word 'clear' to remove it.";
            }
            else if (page == 99)
            {
                scrollbar = false;
                title = "Weapon Ability Names";
                info =
                    "Once you have mastered tactics and a specific weapon type, you will unlock special abilities associated with them. These abilities are represented by simple icons that you can select to perform certain actions. However, you also have the option to enable or disable the display of the names of these special weapon abilities next to their respective icons.";
            }
            else if (page == 100)
            {
                scrollbar = false;
                title = "Auto Sheath";
                info =
                    "This option allows you to enable or disable the feature of sheathing your weapon when you are not in battle. When you switch your character back to war mode, they will automatically draw their weapon.";
            }
            else if (page == 101)
            {
                scrollbar = false;
                title = "Gump Images";
                info =
                    "Many window dumps have a faded image in the background. Disabling this option will make those windows appear only in black color, without any background image.";
            }
            else if (page == 102)
            {
                scrollbar = false;
                title = "Weapon Ability Bar";
                info =
                    "This option turns on or off the auto-opening of the weapon ability icon bar, meaning you will have to do it manually if you turn it off.";
            }
            else if (page == 103)
            {
                scrollbar = false;
                title = "Creature Magic";
                info =
                    "Some creatures possess an inherent magical aptitude. In this particular scenario, you have the option to specialize in one of three schools of magic: magery, necromancy, or elementalism. This flexibility enables creatures with a proficiency in magery or necromancy to redirect their attention towards elementalism, or alternate between magery and necromancy as desired.";
            }
            else if (page == 104)
            {
                scrollbar = false;
                title = "Creature Type";
                info =
                    "Some species of creatures have multiple options for their appearance. This feature allows you to switch to a different appearance within the same species, if another option is available. Additionally, you have the ability to transform into a human. Once you make this choice, you will remain in human form indefinitely.";
            }
            else if (page == 105)
            {
                scrollbar = false;
                title = "Creature Sounds";
                info =
                    "As a creature, you may emit sounds when engaging in combat or sustaining injuries. You have the option to toggle these sounds on or off in this menu.";
            }
            else if (page == 198)
            {
                title = "Play Style - Barbaric";
                info =
                    "The default game is not suited for a sword and sorcery experience, making it less than optimal for players who want to be a barbarian wearing a loin cloth and wielding a huge axe. In order to maximize their chances of survival, characters typically gather as much equipment as possible. However, this particular play style can help in that regard. By choosing to play in this style, a satchel will appear in your main pack. It is important to note that you cannot store anything in this satchel; its purpose is to change specific pieces of equipment that you place into it. These changes apply to shields, hats, helms, tunics, sleeves, leggings, boots, gorgets, gloves, necklaces, cloaks, and robes. The altered items will have a different appearance but function in the same way as the original items. These different items can be equipped, although they may not be visible on your character. Additionally, when you wear robes, they will cover your character's tunics and sleeves. Wearing a sword and sorcery robe will have the same effect, so you will need to remove the robe in order to access the sleeves and/or tunic. This play style also introduces its own set of skill titles for many skills. If you are playing as a female character, pressing the button further will change any 'Barbarian' titles to 'Amazon'. You can open your satchel to learn more about this play style. This option can be toggled on or off at any time, but keep in mind that only one play style can be active at a time.";
            }
            else if (page == 199)
            {
                title = "Skill Lists";
                info =
                    "Skill lists are an alternative to the standard skill lists accessible by clicking the appropriate button on the paper doll. While you still need to use the standard skill lists for skill management (up, down, lock), the alternate skill lists have a more condensed appearance during gameplay. To have skills appear in this alternate list, they must be set to 'up' or 'locked'. 'Locked' skills will only display in this list if you adjust your settings accordingly. The list does not update in real time, but it will often refresh to reflect your current skill status in both real and enhanced values. Any skill appearing in orange indicates that it is locked. You can access this list using the '[skilllist' command or the designated button on the main screen.";
            }
            else if (page == 1000)
            {
                title = "Flip Deed";
                info =
                    "This option enables you to rotate certain items that have two possible facing directions. For instance, if a deed indicates that furniture faces east, you can place the deed on the floor of your house and rotate it to face south instead. This functionality can be applied to various items that resemble deeds, as well as other items that function similarly. For instance, tents or bear rugs have a specific facing direction that can be altered using this command.";
            }

            AddPage(0);

            string color = "#ddbc4b";

            AddImage(0, 0, 9577, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddHtml(
                12,
                12,
                239,
                20,
                @"<BODY><BASEFONT Color=" + color + ">" + title + "</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );
            AddHtml(
                12,
                43,
                278,
                212,
                @"<BODY><BASEFONT Color=" + color + ">" + info + "</BASEFONT></BODY>",
                (bool)false,
                (bool)scrollbar
            );
            AddButton(268, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendSound(0x4A);
            from.CloseGump(typeof(Server.Engines.Help.HelpGump));
            if (m_Origin != 999)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, m_Origin));
            }
        }
    }
}
