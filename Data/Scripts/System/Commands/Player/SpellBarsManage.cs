using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.DeathKnight;
using Server.Spells.Eighth;
using Server.Spells.Elementalism;
using Server.Spells.Fifth;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.HolyMan;
using Server.Spells.Mystic;
using Server.Spells.Necromancy;
using Server.Spells.Research;
using Server.Spells.Second;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Song;
using Server.Spells.Third;

namespace Server.Gumps
{
    public class SetupBarsArch1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "archspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("archspell1")]
        [Description("Opens Spell Bar Editor For Archmages - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsArch1));
            from.SendGump(new SetupBarsArch1(from, 0));
        }

        public SetupBarsArch1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - ANCIENT - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsArch1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsArch1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsArch1", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsArch1", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 1)
            {
                if (Server.Misc.ResearchSettings.ResearchMaterials(from) != null)
                {
                    ResearchBag bag = (ResearchBag)(
                        Server.Misc.ResearchSettings.ResearchMaterials(from)
                    );
                    from.SendGump(new Server.Items.ResearchBag.ResearchGump(bag, from));
                    from.SendSound(0x55);
                }
            }
            else if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsArch1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsArch2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "archspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("archspell2")]
        [Description("Opens Spell Bar Editor For Archmages - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsArch2));
            from.SendGump(new SetupBarsArch2(from, 0));
        }

        public SetupBarsArch2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - ANCIENT - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsArch2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsArch2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsArch2", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsArch2", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 1)
            {
                if (Server.Misc.ResearchSettings.ResearchMaterials(from) != null)
                {
                    ResearchBag bag = (ResearchBag)(
                        Server.Misc.ResearchSettings.ResearchMaterials(from)
                    );
                    from.SendGump(new Server.Items.ResearchBag.ResearchGump(bag, from));
                    from.SendSound(0x55);
                }
            }
            else if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsArch2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsArch3 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "archspell3",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("archspell3")]
        [Description("Opens Spell Bar Editor For Archmages - 3.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsArch3));
            from.SendGump(new SetupBarsArch3(from, 0));
        }

        public SetupBarsArch3(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - ANCIENT - III</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsArch3");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsArch3;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsArch3", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsArch3", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 1)
            {
                if (Server.Misc.ResearchSettings.ResearchMaterials(from) != null)
                {
                    ResearchBag bag = (ResearchBag)(
                        Server.Misc.ResearchSettings.ResearchMaterials(from)
                    );
                    from.SendGump(new Server.Items.ResearchBag.ResearchGump(bag, from));
                    from.SendSound(0x55);
                }
            }
            else if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsArch3(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsArch4 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "archspell4",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("archspell4")]
        [Description("Opens Spell Bar Editor For Archmages - 4.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsArch4));
            from.SendGump(new SetupBarsArch4(from, 0));
        }

        public SetupBarsArch4(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - ANCIENT - IV</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsArch4");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsArch4;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, Int32.Parse(Server.Misc.Research.SpellInformation(rp + 1, 11)));
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsArch4", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsArch4", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 1)
            {
                if (Server.Misc.ResearchSettings.ResearchMaterials(from) != null)
                {
                    ResearchBag bag = (ResearchBag)(
                        Server.Misc.ResearchSettings.ResearchMaterials(from)
                    );
                    from.SendGump(new Server.Items.ResearchBag.ResearchGump(bag, from));
                    from.SendSound(0x55);
                }
            }
            else if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsArch4(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMage1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "magespell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("magespell1")]
        [Description("Opens Spell Bar Editor For Mages - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMage1));
            from.SendGump(new SetupBarsMage1(from, 0));
        }

        public SetupBarsMage1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - MAGERY - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMage1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMage1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, 2240);
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2241);
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2242);
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2243);
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2244);
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2245);
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2246);
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2247);
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2248);
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2249);
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2250);
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2251);
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2252);
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2253);
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2254);
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2255);
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2256);
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2257);
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2258);
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2259);
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2260);
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2261);
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2262);
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2263);
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2264);
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2265);
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2266);
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2267);
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2268);
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2269);
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2270);
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2271);
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2272);
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2273);
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2274);
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2275);
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2276);
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2277);
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2278);
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2279);
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2280);
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2281);
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2282);
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2283);
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2284);
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2285);
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2286);
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2287);
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2288);
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2289);
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2290);
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2291);
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2292);
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2293);
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2294);
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2295);
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2296);
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2297);
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2298);
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2299);
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2300);
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2301);
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2302);
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2303);
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsMage1", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsMage1", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMage1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMage2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "magespell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("magespell2")]
        [Description("Opens Spell Bar Editor For Mages - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMage2));
            from.SendGump(new SetupBarsMage2(from, 0));
        }

        public SetupBarsMage2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - MAGERY - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMage2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMage2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, 2240);
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2241);
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2242);
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2243);
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2244);
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2245);
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2246);
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2247);
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2248);
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2249);
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2250);
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2251);
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2252);
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2253);
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2254);
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2255);
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2256);
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2257);
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2258);
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2259);
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2260);
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2261);
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2262);
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2263);
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2264);
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2265);
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2266);
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2267);
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2268);
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2269);
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2270);
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2271);
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2272);
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2273);
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2274);
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2275);
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2276);
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2277);
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2278);
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2279);
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2280);
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2281);
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2282);
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2283);
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2284);
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2285);
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2286);
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2287);
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2288);
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2289);
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2290);
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2291);
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2292);
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2293);
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2294);
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2295);
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2296);
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2297);
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2298);
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2299);
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2300);
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2301);
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2302);
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2303);
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsMage2", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsMage2", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMage2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMage3 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "magespell3",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("magespell3")]
        [Description("Opens Spell Bar Editor For Mages - 3.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMage3));
            from.SendGump(new SetupBarsMage3(from, 0));
        }

        public SetupBarsMage3(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - MAGERY - III</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMage3");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMage3;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, 2240);
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2241);
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2242);
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2243);
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2244);
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2245);
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2246);
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2247);
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2248);
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2249);
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2250);
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2251);
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2252);
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2253);
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2254);
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2255);
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2256);
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2257);
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2258);
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2259);
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2260);
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2261);
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2262);
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2263);
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2264);
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2265);
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2266);
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2267);
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2268);
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2269);
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2270);
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2271);
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2272);
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2273);
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2274);
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2275);
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2276);
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2277);
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2278);
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2279);
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2280);
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2281);
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2282);
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2283);
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2284);
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2285);
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2286);
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2287);
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2288);
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2289);
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2290);
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2291);
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2292);
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2293);
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2294);
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2295);
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2296);
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2297);
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2298);
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2299);
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2300);
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2301);
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2302);
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2303);
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsMage3", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsMage3", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMage3(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMage4 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "magespell4",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("magespell4")]
        [Description("Opens Spell Bar Editor For Mages - 4.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMage4));
            from.SendGump(new SetupBarsMage4(from, 0));
        }

        public SetupBarsMage4(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - MAGERY - IV</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMage4");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMage4;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button33 = 3609;
            int button34 = 3609;
            int button35 = 3609;
            int button36 = 3609;
            int button37 = 3609;
            int button38 = 3609;
            int button39 = 3609;
            int button40 = 3609;
            int button41 = 3609;
            int button42 = 3609;
            int button43 = 3609;
            int button44 = 3609;
            int button45 = 3609;
            int button46 = 3609;
            int button47 = 3609;
            int button48 = 3609;
            int button49 = 3609;
            int button50 = 3609;
            int button51 = 3609;
            int button52 = 3609;
            int button53 = 3609;
            int button54 = 3609;
            int button55 = 3609;
            int button56 = 3609;
            int button57 = 3609;
            int button58 = 3609;
            int button59 = 3609;
            int button60 = 3609;
            int button61 = 3609;
            int button62 = 3609;
            int button63 = 3609;
            int button64 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }
                if (nLine == 33 && eachSpells == "1")
                {
                    button33 = 4018;
                }
                if (nLine == 34 && eachSpells == "1")
                {
                    button34 = 4018;
                }
                if (nLine == 35 && eachSpells == "1")
                {
                    button35 = 4018;
                }
                if (nLine == 36 && eachSpells == "1")
                {
                    button36 = 4018;
                }
                if (nLine == 37 && eachSpells == "1")
                {
                    button37 = 4018;
                }
                if (nLine == 38 && eachSpells == "1")
                {
                    button38 = 4018;
                }
                if (nLine == 39 && eachSpells == "1")
                {
                    button39 = 4018;
                }
                if (nLine == 40 && eachSpells == "1")
                {
                    button40 = 4018;
                }
                if (nLine == 41 && eachSpells == "1")
                {
                    button41 = 4018;
                }
                if (nLine == 42 && eachSpells == "1")
                {
                    button42 = 4018;
                }
                if (nLine == 43 && eachSpells == "1")
                {
                    button43 = 4018;
                }
                if (nLine == 44 && eachSpells == "1")
                {
                    button44 = 4018;
                }
                if (nLine == 45 && eachSpells == "1")
                {
                    button45 = 4018;
                }
                if (nLine == 46 && eachSpells == "1")
                {
                    button46 = 4018;
                }
                if (nLine == 47 && eachSpells == "1")
                {
                    button47 = 4018;
                }
                if (nLine == 48 && eachSpells == "1")
                {
                    button48 = 4018;
                }
                if (nLine == 49 && eachSpells == "1")
                {
                    button49 = 4018;
                }
                if (nLine == 50 && eachSpells == "1")
                {
                    button50 = 4018;
                }
                if (nLine == 51 && eachSpells == "1")
                {
                    button51 = 4018;
                }
                if (nLine == 52 && eachSpells == "1")
                {
                    button52 = 4018;
                }
                if (nLine == 53 && eachSpells == "1")
                {
                    button53 = 4018;
                }
                if (nLine == 54 && eachSpells == "1")
                {
                    button54 = 4018;
                }
                if (nLine == 55 && eachSpells == "1")
                {
                    button55 = 4018;
                }
                if (nLine == 56 && eachSpells == "1")
                {
                    button56 = 4018;
                }
                if (nLine == 57 && eachSpells == "1")
                {
                    button57 = 4018;
                }
                if (nLine == 58 && eachSpells == "1")
                {
                    button58 = 4018;
                }
                if (nLine == 59 && eachSpells == "1")
                {
                    button59 = 4018;
                }
                if (nLine == 60 && eachSpells == "1")
                {
                    button60 = 4018;
                }
                if (nLine == 61 && eachSpells == "1")
                {
                    button61 = 4018;
                }
                if (nLine == 62 && eachSpells == "1")
                {
                    button62 = 4018;
                }
                if (nLine == 63 && eachSpells == "1")
                {
                    button63 = 4018;
                }
                if (nLine == 64 && eachSpells == "1")
                {
                    button64 = 4018;
                }

                if (nLine == 65 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 66 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 66 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 66 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 66 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 0;

            AddImage(x1, y1, 2240);
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2241);
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2242);
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2243);
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2244);
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2245);
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2246);
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2247);
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2248);
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2249);
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2250);
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2251);
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2252);
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2253);
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2254);
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2255);
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2256);
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2257);
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2258);
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2259);
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2260);
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2261);
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2262);
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2263);
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2264);
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2265);
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2266);
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2267);
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2268);
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2269);
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2270);
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2271);
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2272);
            AddButton(x2, y2, button33, button33, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2273);
            AddButton(x2, y2, button34, button34, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2274);
            AddButton(x2, y2, button35, button35, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2275);
            AddButton(x2, y2, button36, button36, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2276);
            AddButton(x2, y2, button37, button37, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2277);
            AddButton(x2, y2, button38, button38, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2278);
            AddButton(x2, y2, button39, button39, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2279);
            AddButton(x2, y2, button40, button40, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2280);
            AddButton(x2, y2, button41, button41, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2281);
            AddButton(x2, y2, button42, button42, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2282);
            AddButton(x2, y2, button43, button43, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2283);
            AddButton(x2, y2, button44, button44, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2284);
            AddButton(x2, y2, button45, button45, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2285);
            AddButton(x2, y2, button46, button46, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2286);
            AddButton(x2, y2, button47, button47, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2287);
            AddButton(x2, y2, button48, button48, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2288);
            AddButton(x2, y2, button49, button49, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2289);
            AddButton(x2, y2, button50, button50, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2290);
            AddButton(x2, y2, button51, button51, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2291);
            AddButton(x2, y2, button52, button52, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2292);
            AddButton(x2, y2, button53, button53, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2293);
            AddButton(x2, y2, button54, button54, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2294);
            AddButton(x2, y2, button55, button55, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2295);
            AddButton(x2, y2, button56, button56, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, 2296);
            AddButton(x2, y2, button57, button57, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2297);
            AddButton(x2, y2, button58, button58, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2298);
            AddButton(x2, y2, button59, button59, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2299);
            AddButton(x2, y2, button60, button60, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2300);
            AddButton(x2, y2, button61, button61, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2301);
            AddButton(x2, y2, button62, button62, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2302);
            AddButton(x2, y2, button63, button63, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;

            AddImage(x1, y1, 2303);
            AddButton(x2, y2, button64, button64, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 45;
            y2 = y2 + 45;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 18)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 19)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 20)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 21)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 22)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 23)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 24)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 25)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 26)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 27)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 28)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 29)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 30)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 31)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 32)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 33)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 34)
            {
                ToolBarUpdates.UpdateToolBar(from, 35, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 35)
            {
                ToolBarUpdates.UpdateToolBar(from, 36, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 36)
            {
                ToolBarUpdates.UpdateToolBar(from, 37, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 37)
            {
                ToolBarUpdates.UpdateToolBar(from, 38, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 38)
            {
                ToolBarUpdates.UpdateToolBar(from, 39, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 39)
            {
                ToolBarUpdates.UpdateToolBar(from, 40, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 40)
            {
                ToolBarUpdates.UpdateToolBar(from, 41, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 41)
            {
                ToolBarUpdates.UpdateToolBar(from, 42, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 42)
            {
                ToolBarUpdates.UpdateToolBar(from, 43, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 43)
            {
                ToolBarUpdates.UpdateToolBar(from, 44, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 44)
            {
                ToolBarUpdates.UpdateToolBar(from, 45, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 45)
            {
                ToolBarUpdates.UpdateToolBar(from, 46, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 46)
            {
                ToolBarUpdates.UpdateToolBar(from, 47, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 47)
            {
                ToolBarUpdates.UpdateToolBar(from, 48, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 48)
            {
                ToolBarUpdates.UpdateToolBar(from, 49, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 49)
            {
                ToolBarUpdates.UpdateToolBar(from, 50, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 50)
            {
                ToolBarUpdates.UpdateToolBar(from, 51, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 51)
            {
                ToolBarUpdates.UpdateToolBar(from, 52, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 52)
            {
                ToolBarUpdates.UpdateToolBar(from, 53, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 53)
            {
                ToolBarUpdates.UpdateToolBar(from, 54, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 54)
            {
                ToolBarUpdates.UpdateToolBar(from, 55, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 55)
            {
                ToolBarUpdates.UpdateToolBar(from, 56, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 56)
            {
                ToolBarUpdates.UpdateToolBar(from, 57, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 57)
            {
                ToolBarUpdates.UpdateToolBar(from, 58, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 58)
            {
                ToolBarUpdates.UpdateToolBar(from, 59, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 59)
            {
                ToolBarUpdates.UpdateToolBar(from, 60, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 60)
            {
                ToolBarUpdates.UpdateToolBar(from, 61, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 61)
            {
                ToolBarUpdates.UpdateToolBar(from, 62, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 62)
            {
                ToolBarUpdates.UpdateToolBar(from, 63, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 63)
            {
                ToolBarUpdates.UpdateToolBar(from, 64, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 65, "SetupBarsMage4", 66);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 66, "SetupBarsMage4", 66);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMage4(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsElement1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "elementspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("elementspell1")]
        [Description("Opens Spell Bar Editor For Elementalists - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsElement1));
            from.SendGump(new SetupBarsElement1(from, 0));
        }

        public SetupBarsElement1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - ELEMENTALIST - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsElly1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsElly1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }

                if (nLine == 33 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 34 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 34 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 34 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 34 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int book = 0x6421;
            int spel = ((PlayerMobile)from).CharacterElement;

            if (spel == 1)
            {
                book = 0x641F;
            }
            else if (spel == 2)
            {
                book = 0x6422;
            }
            else if (spel == 3)
            {
                book = 0x6420;
            }

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 300;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 301)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 302)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 303)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 304)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 305)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 306)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 307)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 308)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 309)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 310)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 311)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 312)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 313)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 314)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 315)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 316)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 317)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 318)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 319)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 320)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 321)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 322)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 323)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 324)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 325)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 326)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 327)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 328)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 329)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 330)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 331)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsElly1", 34);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsElly1", 34);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsElement1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsElement2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "elementspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("elementspell2")]
        [Description("Opens Spell Bar Editor For Elementalists - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsElement2));
            from.SendGump(new SetupBarsElement2(from, 0));
        }

        public SetupBarsElement2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - ELEMENTALIST - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsElly2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsElly2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;
            int button21 = 3609;
            int button22 = 3609;
            int button23 = 3609;
            int button24 = 3609;
            int button25 = 3609;
            int button26 = 3609;
            int button27 = 3609;
            int button28 = 3609;
            int button29 = 3609;
            int button30 = 3609;
            int button31 = 3609;
            int button32 = 3609;
            int button65 = 3609;
            int button66 = 3609;
            int button67 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }
                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }
                if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }
                if (nLine == 20 && eachSpells == "1")
                {
                    button20 = 4018;
                }
                if (nLine == 21 && eachSpells == "1")
                {
                    button21 = 4018;
                }
                if (nLine == 22 && eachSpells == "1")
                {
                    button22 = 4018;
                }
                if (nLine == 23 && eachSpells == "1")
                {
                    button23 = 4018;
                }
                if (nLine == 24 && eachSpells == "1")
                {
                    button24 = 4018;
                }
                if (nLine == 25 && eachSpells == "1")
                {
                    button25 = 4018;
                }
                if (nLine == 26 && eachSpells == "1")
                {
                    button26 = 4018;
                }
                if (nLine == 27 && eachSpells == "1")
                {
                    button27 = 4018;
                }
                if (nLine == 28 && eachSpells == "1")
                {
                    button28 = 4018;
                }
                if (nLine == 29 && eachSpells == "1")
                {
                    button29 = 4018;
                }
                if (nLine == 30 && eachSpells == "1")
                {
                    button30 = 4018;
                }
                if (nLine == 31 && eachSpells == "1")
                {
                    button31 = 4018;
                }
                if (nLine == 32 && eachSpells == "1")
                {
                    button32 = 4018;
                }

                if (nLine == 33 && eachSpells == "1")
                {
                    button65 = 4018;
                }

                if (nLine == 34 && eachSpells == "0")
                {
                    button66 = 3609;
                }
                else if (nLine == 34 && eachSpells == "1")
                {
                    button66 = 4018;
                }

                if (nLine == 34 && eachSpells == "1")
                {
                    button67 = 3609;
                }
                else if (nLine == 34 && eachSpells == "0")
                {
                    button67 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button65, button65, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button66, button66, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button67, button67, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int book = 0x6421;
            int spel = ((PlayerMobile)from).CharacterElement;

            if (spel == 1)
            {
                book = 0x641F;
            }
            else if (spel == 2)
            {
                book = 0x6422;
            }
            else if (spel == 3)
            {
                book = 0x6420;
            }

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 120;
            int y2 = 130;
            int rp = 300;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button1, button1, 99, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button2, button2, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button3, button3, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button4, button4, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button5, button5, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button6, button6, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button7, button7, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button8, button8, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button9, button9, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button10, button10, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button11, button11, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button12, button12, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button13, button13, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button14, button14, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button15, button15, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button16, button16, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button17, button17, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button18, button18, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button19, button19, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button20, button20, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button21, button21, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button22, button22, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button23, button23, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button24, button24, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button25, button25, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button26, button26, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button27, button27, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button28, button28, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 120;
            y2 = 130;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button29, button29, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button30, button30, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button31, button31, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;

            AddImage(x1, y1, ElementalSpell.SpellIcon(book, rp));
            AddButton(x2, y2, button32, button32, rp, GumpButtonType.Reply, 0);
            y1 = y1 + 90;
            y2 = y2 + 90;
            rp++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 99)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 301)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 302)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 303)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 304)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 305)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 306)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 307)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 308)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 309)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 310)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 311)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 312)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 313)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 314)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 315)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 316)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 317)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 318)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 319)
            {
                ToolBarUpdates.UpdateToolBar(from, 20, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 320)
            {
                ToolBarUpdates.UpdateToolBar(from, 21, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 321)
            {
                ToolBarUpdates.UpdateToolBar(from, 22, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 322)
            {
                ToolBarUpdates.UpdateToolBar(from, 23, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 323)
            {
                ToolBarUpdates.UpdateToolBar(from, 24, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 324)
            {
                ToolBarUpdates.UpdateToolBar(from, 25, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 325)
            {
                ToolBarUpdates.UpdateToolBar(from, 26, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 326)
            {
                ToolBarUpdates.UpdateToolBar(from, 27, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 327)
            {
                ToolBarUpdates.UpdateToolBar(from, 28, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 328)
            {
                ToolBarUpdates.UpdateToolBar(from, 29, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 329)
            {
                ToolBarUpdates.UpdateToolBar(from, 30, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 330)
            {
                ToolBarUpdates.UpdateToolBar(from, 31, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 331)
            {
                ToolBarUpdates.UpdateToolBar(from, 32, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 33, "SetupBarsElly2", 34);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 34, "SetupBarsElly2", 34);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsElement2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsNecro1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "necrospell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("necrospell1")]
        [Description("Opens Spell Bar Editor For Necromancers - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsNecro1));
            from.SendGump(new SetupBarsNecro1(from, 0));
        }

        public SetupBarsNecro1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - NECROMANCER - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsNecro1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsNecro1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }

                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }

                if (nLine == 19 && eachSpells == "0")
                {
                    button19 = 3609;
                }
                else if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }

                if (nLine == 19 && eachSpells == "1")
                {
                    button20 = 3609;
                }
                else if (nLine == 19 && eachSpells == "0")
                {
                    button20 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button18, button18, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button19, button19, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button20, button20, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 20480);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20488);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20496);
            AddButton(x2, y2, button17, button17, 17, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20481);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20489);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20482);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20490);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20483);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20491);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20484);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20492);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20485);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20493);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20486);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20494);
            AddButton(x2, y2, button15, button15, 15, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20487);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20495);
            AddButton(x2, y2, button16, button16, 16, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsNecro1", 19);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsNecro1", 19);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsNecro1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsNecro2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "necrospell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("necrospell2")]
        [Description("Opens Spell Bar Editor For Necromancers - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsNecro2));
            from.SendGump(new SetupBarsNecro2(from, 0));
        }

        public SetupBarsNecro2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - NECROMANCER - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsNecro2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsNecro2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;
            int button20 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }
                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }

                if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }

                if (nLine == 19 && eachSpells == "0")
                {
                    button19 = 3609;
                }
                else if (nLine == 19 && eachSpells == "1")
                {
                    button19 = 4018;
                }

                if (nLine == 19 && eachSpells == "1")
                {
                    button20 = 3609;
                }
                else if (nLine == 19 && eachSpells == "0")
                {
                    button20 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button18, button18, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button19, button19, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button20, button20, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 20480);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20488);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20496);
            AddButton(x2, y2, button17, button17, 17, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20481);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20489);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20482);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20490);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20483);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20491);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20484);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20492);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20485);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20493);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20486);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20494);
            AddButton(x2, y2, button15, button15, 15, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20487);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20495);
            AddButton(x2, y2, button16, button16, 16, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 17)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsNecro2", 19);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 19, "SetupBarsNecro2", 19);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsNecro2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsKnight1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "knightspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("knightspell1")]
        [Description("Opens Spell Bar Editor For Knights - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsKnight1));
            from.SendGump(new SetupBarsKnight1(from, 0));
        }

        public SetupBarsKnight1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - KNIGHT - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsKnight1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsKnight1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }

                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }

                if (nLine == 12 && eachSpells == "0")
                {
                    button12 = 3609;
                }
                else if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }

                if (nLine == 12 && eachSpells == "1")
                {
                    button13 = 3609;
                }
                else if (nLine == 12 && eachSpells == "0")
                {
                    button13 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button11, button11, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button12, button12, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button13, button13, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 20736);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20741);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20737);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20742);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20738);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20743);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20739);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20744);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20740);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20745);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsKnight1", 12);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsKnight1", 12);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsKnight1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsKnight2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "knightspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("knightspell2")]
        [Description("Opens Spell Bar Editor For Knights - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsKnight2));
            from.SendGump(new SetupBarsKnight2(from, 0));
        }

        public SetupBarsKnight2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - KNIGHT - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsKnight2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsKnight2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }

                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }

                if (nLine == 12 && eachSpells == "0")
                {
                    button12 = 3609;
                }
                else if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }

                if (nLine == 12 && eachSpells == "1")
                {
                    button13 = 3609;
                }
                else if (nLine == 12 && eachSpells == "0")
                {
                    button13 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button11, button11, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button12, button12, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button13, button13, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 20736);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20741);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20737);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20742);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20738);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20743);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20739);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20744);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 20740);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 20745);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsKnight2", 12);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsKnight2", 12);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsKnight2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsBard1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "bardsong1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("bardsong1")]
        [Description("Opens Spell Bar Editor For Bards - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsBard1));
            from.SendGump(new SetupBarsBard1(from, 0));
        }

        public SetupBarsBard1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - BARD - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsBard1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsBard1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }

                if (nLine == 18 && eachSpells == "0")
                {
                    button18 = 3609;
                }
                else if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }

                if (nLine == 18 && eachSpells == "1")
                {
                    button19 = 3609;
                }
                else if (nLine == 18 && eachSpells == "0")
                {
                    button19 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button17, button17, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button18, button18, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button19, button19, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 1028);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1036);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1029);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1037);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1030);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1038);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1031);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1040);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1032);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1041);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1033);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1042);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1034);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1043);
            AddButton(x2, y2, button15, button15, 15, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1035);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1044);
            AddButton(x2, y2, button16, button16, 16, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsBard1", 18);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsBard1", 18);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsBard1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsBard2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "bardsong2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("bardsong2")]
        [Description("Opens Spell Bar Editor For Bards - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsBard2));
            from.SendGump(new SetupBarsBard2(from, 0));
        }

        public SetupBarsBard2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - BARD - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsBard2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsBard2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;
            int button18 = 3609;
            int button19 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }
                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }
                if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 17 && eachSpells == "1")
                {
                    button17 = 4018;
                }

                if (nLine == 18 && eachSpells == "0")
                {
                    button18 = 3609;
                }
                else if (nLine == 18 && eachSpells == "1")
                {
                    button18 = 4018;
                }

                if (nLine == 18 && eachSpells == "1")
                {
                    button19 = 3609;
                }
                else if (nLine == 18 && eachSpells == "0")
                {
                    button19 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button17, button17, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button18, button18, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button19, button19, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 1028);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1036);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1029);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1037);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1030);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1038);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1031);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1040);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1032);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1041);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1033);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1042);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1034);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1043);
            AddButton(x2, y2, button15, button15, 15, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 8

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 1035);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 1044);
            AddButton(x2, y2, button16, button16, 16, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 15)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 16)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 17, "SetupBarsBard2", 18);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 18, "SetupBarsBard2", 18);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsBard2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsDeath1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "deathspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("deathspell1")]
        [Description("Opens Spell Bar Editor For Death Knights - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsDeath1));
            from.SendGump(new SetupBarsDeath1(from, 0));
        }

        public SetupBarsDeath1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - DEATH KNIGHT - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsDeath1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsDeath1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }

                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }

                if (nLine == 16 && eachSpells == "0")
                {
                    button16 = 3609;
                }
                else if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 16 && eachSpells == "1")
                {
                    button17 = 3609;
                }
                else if (nLine == 16 && eachSpells == "0")
                {
                    button17 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button15, button15, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button16, button16, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button17, button17, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x5010, 2405);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x1B, 2405);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5009, 2405);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x3EE, 2405);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5005, 2405);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5006, 2405);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x402, 2405);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x2B, 2405);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5002, 2405);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x12, 2405);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x3E9, 2405);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x500C, 2405);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5DC0, 2405);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x2E, 2405);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsDeath1", 16);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsDeath1", 16);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsDeath1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsDeath2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "deathspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("deathspell2")]
        [Description("Opens Spell Bar Editor For Death Knights - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsDeath2));
            from.SendGump(new SetupBarsDeath2(from, 0));
        }

        public SetupBarsDeath2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">SPELL BAR - DEATH KNIGHT - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsDeath2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsDeath2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }

                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }

                if (nLine == 16 && eachSpells == "0")
                {
                    button16 = 3609;
                }
                else if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 16 && eachSpells == "1")
                {
                    button17 = 3609;
                }
                else if (nLine == 16 && eachSpells == "0")
                {
                    button17 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button15, button15, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button16, button16, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button17, button17, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x5010, 2405);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x1B, 2405);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5009, 2405);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x3EE, 2405);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5005, 2405);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5006, 2405);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x402, 2405);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x2B, 2405);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5002, 2405);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x12, 2405);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x3E9, 2405);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x500C, 2405);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x5DC0, 2405);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x2E, 2405);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsDeath2", 16);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsDeath2", 16);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsDeath2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsPriest1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "holyspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("holyspell1")]
        [Description("Opens Spell Bar Editor For Prayers - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsPriest1));
            from.SendGump(new SetupBarsPriest1(from, 0));
        }

        public SetupBarsPriest1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - PRIEST - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsPriest1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsPriest1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }

                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }

                if (nLine == 16 && eachSpells == "0")
                {
                    button16 = 3609;
                }
                else if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 16 && eachSpells == "1")
                {
                    button17 = 3609;
                }
                else if (nLine == 16 && eachSpells == "0")
                {
                    button17 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button15, button15, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button16, button16, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button17, button17, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x965, 1071);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96C, 1071);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x966, 1071);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96E, 1071);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x967, 1071);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96D, 1071);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x968, 1071);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96F, 1071);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x969, 1071);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x970, 1071);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x96A, 1071);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x971, 1071);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x96B, 1071);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x972, 1071);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsPriest1", 16);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsPriest1", 16);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsPriest1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsPriest2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "holyspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("holyspell2")]
        [Description("Opens Spell Bar Editor For Prayers - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsPriest2));
            from.SendGump(new SetupBarsPriest2(from, 0));
        }

        public SetupBarsPriest2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">SPELL BAR - PRIEST - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsPriest2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsPriest2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;
            int button14 = 3609;
            int button15 = 3609;
            int button16 = 3609;
            int button17 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }
                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }
                if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }
                if (nLine == 13 && eachSpells == "1")
                {
                    button13 = 4018;
                }
                if (nLine == 14 && eachSpells == "1")
                {
                    button14 = 4018;
                }

                if (nLine == 15 && eachSpells == "1")
                {
                    button15 = 4018;
                }

                if (nLine == 16 && eachSpells == "0")
                {
                    button16 = 3609;
                }
                else if (nLine == 16 && eachSpells == "1")
                {
                    button16 = 4018;
                }

                if (nLine == 16 && eachSpells == "1")
                {
                    button17 = 3609;
                }
                else if (nLine == 16 && eachSpells == "0")
                {
                    button17 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button15, button15, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button16, button16, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button17, button17, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x965, 1071);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96C, 1071);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x966, 1071);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96E, 1071);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x967, 1071);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96D, 1071);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x968, 1071);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96F, 1071);
            AddButton(x2, y2, button11, button11, 11, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x969, 1071);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x970, 1071);
            AddButton(x2, y2, button12, button12, 12, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 6

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x96A, 1071);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x971, 1071);
            AddButton(x2, y2, button13, button13, 13, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 7

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x96B, 1071);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x972, 1071);
            AddButton(x2, y2, button14, button14, 14, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 11)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 12)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 13)
            {
                ToolBarUpdates.UpdateToolBar(from, 13, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 14)
            {
                ToolBarUpdates.UpdateToolBar(from, 14, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 15, "SetupBarsPriest2", 16);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 16, "SetupBarsPriest2", 16);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsPriest2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMonk1 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "monkspell1",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("monkspell1")]
        [Description("Opens Spell Bar Editor For Monks - 1.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMonk1));
            from.SendGump(new SetupBarsMonk1(from, 0));
        }

        public SetupBarsMonk1(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">ABILITY BAR - MONKS - I</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMonk1");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMonk1;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }

                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }

                if (nLine == 12 && eachSpells == "0")
                {
                    button12 = 3609;
                }
                else if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }

                if (nLine == 12 && eachSpells == "1")
                {
                    button13 = 3609;
                }
                else if (nLine == 12 && eachSpells == "0")
                {
                    button13 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button11, button11, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button12, button12, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button13, button13, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x500E, 2422);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5DC2, 2422);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x410, 2422);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x1A, 2422);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x15, 2422);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96D, 2422);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x971, 2422);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5001, 2422);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x4B2, 2422);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x19, 2422);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMonk1", 12);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMonk1", 12);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMonk1(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Gumps
{
    public class SetupBarsMonk2 : Gump
    {
        public int m_Origin;

        public static void Initialize()
        {
            CommandSystem.Register(
                "monkspell2",
                AccessLevel.Player,
                new CommandEventHandler(ToolBar_OnCommand)
            );
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            CommandSystem.Register(command, access, handler);
        }

        [Usage("monkspell2")]
        [Description("Opens Spell Bar Editor For Monks - 2.")]
        public static void ToolBar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(SetupBarsMonk2));
            from.SendGump(new SetupBarsMonk2(from, 0));
        }

        public SetupBarsMonk2(Mobile from, int origin)
            : base(12, 50)
        {
            m_Origin = origin;
            string color = "#d05f5a";

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddImage(38, 0, 9578, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddButton(897, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
            AddHtml(
                52,
                14,
                400,
                20,
                @"<BODY><BASEFONT Color=" + color + ">ABILITY BAR - MONKS - II</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            ToolBarUpdates.InitializeToolBar(from, "SetupBarsMonk2");
            string MyServerSettings = ((PlayerMobile)from).SpellBarsMonk2;

            int button1 = 3609;
            int button2 = 3609;
            int button3 = 3609;
            int button4 = 3609;
            int button5 = 3609;
            int button6 = 3609;
            int button7 = 3609;
            int button8 = 3609;
            int button9 = 3609;
            int button10 = 3609;
            int button11 = 3609;
            int button12 = 3609;
            int button13 = 3609;

            string[] eachSpell = MyServerSettings.Split('#');
            int nLine = 1;
            foreach (string eachSpells in eachSpell)
            {
                if (nLine == 1 && eachSpells == "1")
                {
                    button1 = 4018;
                }
                if (nLine == 2 && eachSpells == "1")
                {
                    button2 = 4018;
                }
                if (nLine == 3 && eachSpells == "1")
                {
                    button3 = 4018;
                }
                if (nLine == 4 && eachSpells == "1")
                {
                    button4 = 4018;
                }
                if (nLine == 5 && eachSpells == "1")
                {
                    button5 = 4018;
                }
                if (nLine == 6 && eachSpells == "1")
                {
                    button6 = 4018;
                }
                if (nLine == 7 && eachSpells == "1")
                {
                    button7 = 4018;
                }
                if (nLine == 8 && eachSpells == "1")
                {
                    button8 = 4018;
                }
                if (nLine == 9 && eachSpells == "1")
                {
                    button9 = 4018;
                }
                if (nLine == 10 && eachSpells == "1")
                {
                    button10 = 4018;
                }

                if (nLine == 11 && eachSpells == "1")
                {
                    button11 = 4018;
                }

                if (nLine == 12 && eachSpells == "0")
                {
                    button12 = 3609;
                }
                else if (nLine == 12 && eachSpells == "1")
                {
                    button12 = 4018;
                }

                if (nLine == 12 && eachSpells == "1")
                {
                    button13 = 3609;
                }
                else if (nLine == 12 && eachSpells == "0")
                {
                    button13 = 4018;
                }

                nLine++;
            }

            AddButton(582, 82, button11, button11, 90, GumpButtonType.Reply, 0);
            AddHtml(
                624,
                81,
                261,
                21,
                @"<BODY><BASEFONT Color="
                    + color
                    + ">Show Spell Names When Vertical</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(377, 540, button12, button12, 91, GumpButtonType.Reply, 0);
            AddHtml(
                417,
                539,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Vertical Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            AddButton(681, 537, button13, button13, 91, GumpButtonType.Reply, 0);
            AddHtml(
                721,
                536,
                125,
                21,
                @"<BODY><BASEFONT Color=" + color + ">Horizontal Bar</BASEFONT></BODY>",
                (bool)false,
                (bool)false
            );

            // ------------------------------------------------------------------------------------ 1

            int x1 = 135;
            int x2 = 95;
            int y1 = 165;
            int y2 = 175;

            AddImage(x1, y1, 0x500E, 2422);
            AddButton(x2, y2, button1, button1, 1, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5DC2, 2422);
            AddButton(x2, y2, button6, button6, 6, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 2

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x410, 2422);
            AddButton(x2, y2, button2, button2, 2, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x1A, 2422);
            AddButton(x2, y2, button7, button7, 7, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 3

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x15, 2422);
            AddButton(x2, y2, button3, button3, 3, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x96D, 2422);
            AddButton(x2, y2, button8, button8, 8, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 4

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x971, 2422);
            AddButton(x2, y2, button4, button4, 4, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x5001, 2422);
            AddButton(x2, y2, button9, button9, 9, GumpButtonType.Reply, 0);

            // ------------------------------------------------------------------------------------ 5

            x1 = x1 + 100;
            x2 = x2 + 100;
            y1 = 165;
            y2 = 175;

            AddImage(x1, y1, 0x4B2, 2422);
            AddButton(x2, y2, button5, button5, 5, GumpButtonType.Reply, 0);
            y1 = y1 + 135;
            y2 = y2 + 135;

            AddImage(x1, y1, 0x19, 2422);
            AddButton(x2, y2, button10, button10, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                ToolBarUpdates.UpdateToolBar(from, 1, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 2)
            {
                ToolBarUpdates.UpdateToolBar(from, 2, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 3)
            {
                ToolBarUpdates.UpdateToolBar(from, 3, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 4)
            {
                ToolBarUpdates.UpdateToolBar(from, 4, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 5)
            {
                ToolBarUpdates.UpdateToolBar(from, 5, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 6)
            {
                ToolBarUpdates.UpdateToolBar(from, 6, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 7)
            {
                ToolBarUpdates.UpdateToolBar(from, 7, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 8)
            {
                ToolBarUpdates.UpdateToolBar(from, 8, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 9)
            {
                ToolBarUpdates.UpdateToolBar(from, 9, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 10)
            {
                ToolBarUpdates.UpdateToolBar(from, 10, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 90)
            {
                ToolBarUpdates.UpdateToolBar(from, 11, "SetupBarsMonk2", 12);
            }
            else if (info.ButtonID == 91)
            {
                ToolBarUpdates.UpdateToolBar(from, 12, "SetupBarsMonk2", 12);
            }

            if (info.ButtonID < 1 && m_Origin > 0)
            {
                from.SendGump(new Server.Engines.Help.HelpGump(from, 7));
                from.SendSound(0x4A);
            }
            else if (info.ButtonID < 1) { }
            else
            {
                from.SendGump(new SetupBarsMonk2(from, m_Origin));
                from.SendSound(0x4A);
            }
        }
    }
}
