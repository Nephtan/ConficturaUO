using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class OzThothsStaticOtherGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "StaticsTool",
                AccessLevel.GameMaster,
                new CommandEventHandler(OzThothsStatic_OnCommand)
            );
            CommandSystem.Register(
                "STool",
                AccessLevel.GameMaster,
                new CommandEventHandler(OzThothsStatic_OnCommand)
            );
            CommandSystem.Register(
                "ST",
                AccessLevel.GameMaster,
                new CommandEventHandler(OzThothsStatic_OnCommand)
            );
        }

        [Usage("StaticTool")]
        [Description("Makes a call to an in game static tool")]
        private static void OzThothsStatic_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.CloseGump(typeof(OzThothsStaticOtherGump));
            from.SendGump(new OzThothsStaticOtherGump(from));
        }

        public OzThothsStaticOtherGump(Mobile from)
            : base(0, 0)
        {
            Dragable = true;
            Closable = false;
            Resizable = false;
            Disposable = false;

            AddPage(0);
            AddImageTiled(87, 220, 142, 362, 2624);
            AddImageTiled(337, 230, 21, 347, 2624);
            AddBackground(88, 80, 878, 137, 2620);
            AddBackground(87, 210, 241, 378, 2620);
            AddBackground(327, 210, 639, 379, 2620);
            AddImageTiled(92, 240, 869, 2, 2700);
            AddImageTiled(537, 480, 100, 3, 2700);
            AddImageTiled(847, 480, 116, 3, 2700);
            AddImageTiled(327, 330, 634, 6, 2700);
            AddImageTiled(327, 360, 634, 2, 2700);
            AddImageTiled(480, 110, 481, 2, 2700);
            AddImageTiled(847, 480, 6, 106, 2701);
            AddImageTiled(637, 210, 11, 372, 2701);
            AddImageTiled(480, 80, 5, 135, 2701);
            AddImageTiled(910, 86, 5, 29, 2701);
            AddImageTiled(240, 80, 5, 131, 2701);
            AddImageTiled(537, 480, 6, 106, 2701);
            AddImage(94, 90, 100);

            AddHtml(
                547,
                490,
                86,
                87,
                "<basefont color=#E6FBFF>[inc x # <br/>"
                    + "[inc x -# <br/>"
                    + "[inc y # <br/>"
                    + "[inc y -#",
                false,
                false
            );
            AddHtml(
                860,
                490,
                101,
                86,
                "<basefont color=#E6FBFF>[area inc x # <br/>"
                    + "[area inc x -# <br/>"
                    + "[area inc y # <br/>"
                    + "[area inc y -#",
                false,
                false
            );
            AddHtml(
                490,
                120,
                464,
                89,
                "<basefont size=5 color=#E6FBFF><center>ID# = Item ID.  R# = Number For Random.   <br/>"
                    + "[Tile Static ID# R# [TileZ # ID# For Static Tiling At Z Height  [Add Static # [Tile Static # [Dupe to copy a static [Dupe # to copy # of statics of type.  [Inc x # y # z # increases/decreases multiple increments.",
                false,
                false
            );
            AddHtml(
                490,
                90,
                417,
                19,
                "<basefont size=5 color=#E6FBFF><center>Helpful Tips",
                false,
                false
            );
            AddHtml(
                110,
                110,
                108,
                60,
                "<basefont size=5><center>Oz'Thoth's Static Tool (Other Gump)",
                false,
                false
            );
            AddHtml(
                330,
                220,
                298,
                18,
                "<basefont size=5 color=#E6FBFF><center>Single Height",
                false,
                false
            );
            AddHtml(
                650,
                220,
                298,
                18,
                "<basefont size=5 color=#E6FBFF><center>Area Height",
                false,
                false
            );
            AddHtml(
                330,
                340,
                298,
                18,
                "<basefont size=5 color=#E6FBFF><center>Single Direction",
                false,
                false
            );
            AddHtml(
                650,
                340,
                298,
                18,
                "<basefont size=5 color=#E6FBFF><center>Area Direction",
                false,
                false
            );

            //Directional Commands

            AddButton(357, 260, 5600, 5604, 1, GumpButtonType.Reply, 0);
            AddButton(357, 310, 5602, 5606, 2, GumpButtonType.Reply, 0);
            AddHtml(347, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>1", false, false);

            AddButton(417, 260, 5600, 5604, 3, GumpButtonType.Reply, 0);
            AddButton(417, 310, 5602, 5606, 4, GumpButtonType.Reply, 0);
            AddHtml(407, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>3", false, false);

            AddButton(477, 260, 5600, 5604, 5, GumpButtonType.Reply, 0);
            AddButton(477, 310, 5602, 5606, 6, GumpButtonType.Reply, 0);
            AddHtml(467, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>5", false, false);

            AddButton(537, 260, 5600, 5604, 7, GumpButtonType.Reply, 0);
            AddButton(537, 310, 5602, 5606, 8, GumpButtonType.Reply, 0);
            AddHtml(527, 280, 35, 26, "<basefont size=5 color=#E6FBFF>10", false, false);

            AddButton(597, 260, 5600, 5604, 9, GumpButtonType.Reply, 0);
            AddButton(597, 310, 5602, 5606, 10, GumpButtonType.Reply, 0);
            AddHtml(587, 280, 35, 26, "<basefont size=5 color=#E6FBFF>20", false, false);

            AddButton(677, 260, 5600, 5604, 11, GumpButtonType.Reply, 0);
            AddButton(677, 310, 5602, 5606, 12, GumpButtonType.Reply, 0);
            AddHtml(667, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>1", false, false);

            AddButton(737, 260, 5600, 5604, 13, GumpButtonType.Reply, 0);
            AddButton(737, 310, 5602, 5606, 14, GumpButtonType.Reply, 0);
            AddHtml(727, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>3", false, false);

            AddButton(797, 260, 5600, 5604, 15, GumpButtonType.Reply, 0);
            AddButton(797, 310, 5602, 5606, 16, GumpButtonType.Reply, 0);
            AddHtml(787, 280, 35, 26, "<basefont size=5 color=#E6FBFF><center>5", false, false);

            AddButton(857, 260, 5600, 5604, 17, GumpButtonType.Reply, 0);
            AddButton(857, 310, 5602, 5606, 18, GumpButtonType.Reply, 0);
            AddHtml(847, 280, 35, 26, "<basefont size=5 color=#E6FBFF>10", false, false);

            AddButton(917, 260, 5600, 5604, 19, GumpButtonType.Reply, 0);
            AddButton(917, 310, 5602, 5606, 20, GumpButtonType.Reply, 0);
            AddHtml(907, 280, 35, 26, "<basefont size=5 color=#E6FBFF>20", false, false);

            //Directional Commands

            AddHtml(367, 420, 19, 23, "<basefont size=5 color=#E6FBFF><center>1", false, false);
            AddButton(367, 390, 5600, 5604, 21, GumpButtonType.Reply, 0);
            AddButton(367, 450, 5602, 5606, 22, GumpButtonType.Reply, 0);
            AddButton(337, 420, 5603, 5607, 23, GumpButtonType.Reply, 0);
            AddButton(397, 420, 5601, 5605, 24, GumpButtonType.Reply, 0);

            AddHtml(477, 420, 19, 23, "<basefont size=5 color=#E6FBFF><center>3", false, false);
            AddButton(477, 390, 5600, 5604, 25, GumpButtonType.Reply, 0);
            AddButton(477, 450, 5602, 5606, 26, GumpButtonType.Reply, 0);
            AddButton(447, 420, 5603, 5607, 27, GumpButtonType.Reply, 0);
            AddButton(507, 420, 5601, 5605, 28, GumpButtonType.Reply, 0);

            AddHtml(577, 420, 19, 23, "<basefont size=5 color=#E6FBFF><center>5", false, false);
            AddButton(577, 390, 5600, 5604, 29, GumpButtonType.Reply, 0);
            AddButton(577, 450, 5602, 5606, 30, GumpButtonType.Reply, 0);
            AddButton(547, 420, 5603, 5607, 31, GumpButtonType.Reply, 0);
            AddButton(607, 420, 5601, 5605, 32, GumpButtonType.Reply, 0);

            AddHtml(367, 520, 33, 23, "<basefont size=5 color=#E6FBFF>10", false, false);
            AddButton(367, 490, 5600, 5604, 33, GumpButtonType.Reply, 0);
            AddButton(367, 550, 5602, 5606, 34, GumpButtonType.Reply, 0);
            AddButton(337, 520, 5603, 5607, 35, GumpButtonType.Reply, 0);
            AddButton(397, 520, 5601, 5605, 36, GumpButtonType.Reply, 0);

            AddHtml(477, 520, 33, 23, "<basefont size=5 color=#E6FBFF>20", false, false);
            AddButton(477, 490, 5600, 5604, 37, GumpButtonType.Reply, 0);
            AddButton(477, 550, 5602, 5606, 38, GumpButtonType.Reply, 0);
            AddButton(447, 520, 5603, 5607, 39, GumpButtonType.Reply, 0);
            AddButton(507, 520, 5601, 5605, 40, GumpButtonType.Reply, 0);

            AddHtml(687, 410, 19, 23, "<basefont size=5 color=#E6FBFF><center>1", false, false);
            AddButton(687, 380, 5600, 5604, 41, GumpButtonType.Reply, 0);
            AddButton(687, 440, 5602, 5606, 42, GumpButtonType.Reply, 0);
            AddButton(657, 410, 5603, 5607, 43, GumpButtonType.Reply, 0);
            AddButton(717, 410, 5601, 5605, 44, GumpButtonType.Reply, 0);

            AddHtml(797, 410, 19, 23, "<basefont size=5 color=#E6FBFF><center>3", false, false);
            AddButton(797, 380, 5600, 5604, 45, GumpButtonType.Reply, 0);
            AddButton(797, 440, 5602, 5606, 46, GumpButtonType.Reply, 0);
            AddButton(767, 410, 5603, 5607, 47, GumpButtonType.Reply, 0);
            AddButton(827, 410, 5601, 5605, 48, GumpButtonType.Reply, 0);

            AddHtml(897, 410, 19, 23, "<basefont size=5 color=#E6FBFF><center>5", false, false);
            AddButton(897, 380, 5600, 5604, 49, GumpButtonType.Reply, 0);
            AddButton(897, 440, 5602, 5606, 50, GumpButtonType.Reply, 0);
            AddButton(867, 410, 5603, 5607, 51, GumpButtonType.Reply, 0);
            AddButton(927, 410, 5601, 5605, 52, GumpButtonType.Reply, 0);

            AddHtml(687, 510, 33, 23, "<basefont size=5 color=#E6FBFF>10", false, false);
            AddButton(687, 480, 5600, 5604, 53, GumpButtonType.Reply, 0);
            AddButton(687, 540, 5602, 5606, 54, GumpButtonType.Reply, 0);
            AddButton(657, 510, 5603, 5607, 55, GumpButtonType.Reply, 0);
            AddButton(717, 510, 5601, 5605, 56, GumpButtonType.Reply, 0);

            AddHtml(797, 510, 33, 23, "<basefont size=5 color=#E6FBFF>20", false, false);
            AddButton(797, 480, 5600, 5604, 57, GumpButtonType.Reply, 0);
            AddButton(797, 540, 5602, 5606, 58, GumpButtonType.Reply, 0);
            AddButton(767, 510, 5603, 5607, 59, GumpButtonType.Reply, 0);
            AddButton(827, 510, 5601, 5605, 60, GumpButtonType.Reply, 0);

            //Static Commands

            AddButton(97, 250, 5601, 5605, 61, GumpButtonType.Reply, 0);
            AddHtml(
                117,
                250,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Arches",
                false,
                false
            );

            AddButton(97, 280, 5601, 5605, 62, GumpButtonType.Reply, 0);
            AddHtml(117, 280, 80, 25, "<basefont size=5 color=#E6FBFF><center>Doors", false, false);

            AddButton(97, 310, 5601, 5605, 63, GumpButtonType.Reply, 0);
            AddHtml(
                117,
                310,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Fences",
                false,
                false
            );

            AddButton(97, 340, 5601, 5605, 64, GumpButtonType.Reply, 0);
            AddHtml(
                117,
                340,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Floors",
                false,
                false
            );

            AddButton(97, 370, 5601, 5605, 65, GumpButtonType.Reply, 0);
            AddHtml(
                117,
                370,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Plants",
                false,
                false
            );

            AddButton(97, 400, 5601, 5605, 66, GumpButtonType.Reply, 0);
            AddHtml(117, 400, 80, 25, "<basefont size=5 color=#E6FBFF><center>Signs", false, false);

            AddButton(97, 430, 5601, 5605, 67, GumpButtonType.Reply, 0);
            AddHtml(
                117,
                430,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Stairs",
                false,
                false
            );

            AddButton(97, 460, 5601, 5605, 68, GumpButtonType.Reply, 0);
            AddHtml(117, 460, 80, 25, "<basefont size=5 color=#E6FBFF><center>Rocks", false, false);

            AddButton(97, 490, 5601, 5605, 69, GumpButtonType.Reply, 0);
            AddHtml(117, 490, 80, 25, "<basefont size=5 color=#E6FBFF><center>Roofs", false, false);

            AddButton(97, 520, 5601, 5605, 70, GumpButtonType.Reply, 0);
            AddHtml(117, 520, 80, 25, "<basefont size=5 color=#E6FBFF><center>Walls", false, false);

            AddButton(97, 550, 5601, 5605, 71, GumpButtonType.Reply, 0);
            AddHtml(117, 550, 80, 25, "<basefont size=5 color=#E6FBFF><center>Gear", false, false);

            AddButton(207, 250, 5601, 5605, 72, GumpButtonType.Reply, 0);
            AddHtml(227, 250, 80, 25, "<basefont size=5 color=#E6FBFF><center>Food", false, false);

            AddButton(207, 280, 5601, 5605, 73, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                280,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Furniture",
                false,
                false
            );

            AddButton(207, 310, 5601, 5605, 74, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                310,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Dungeon",
                false,
                false
            );

            AddButton(207, 340, 5601, 5605, 75, GumpButtonType.Reply, 0);
            AddHtml(227, 340, 80, 25, "<basefont size=5 color=#E6FBFF><center>Light", false, false);

            AddButton(207, 370, 5601, 5605, 76, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                370,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Ground",
                false,
                false
            );

            AddButton(207, 400, 5601, 5605, 77, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                400,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Sign Post",
                false,
                false
            );

            AddButton(207, 430, 5601, 5605, 78, GumpButtonType.Reply, 0);
            AddHtml(227, 430, 80, 25, "<basefont size=5 color=#E6FBFF><center>Deco", false, false);

            AddButton(207, 460, 5601, 5605, 79, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                460,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Nature",
                false,
                false
            );

            AddButton(207, 490, 5601, 5605, 80, GumpButtonType.Reply, 0);
            AddHtml(227, 490, 80, 25, "<basefont size=5 color=#E6FBFF><center>Misc", false, false);

            AddButton(207, 520, 5601, 5605, 81, GumpButtonType.Reply, 0);
            AddHtml(
                227,
                520,
                80,
                25,
                "<basefont size=5 color=#E6FBFF><center>Custom",
                false,
                false
            );

            AddButton(207, 550, 5601, 5605, 82, GumpButtonType.Reply, 0);
            AddHtml(227, 550, 80, 25, "<basefont size=5 color=#E6FBFF><center>Anim", false, false);

            //Upper Commands

            AddButton(253, 110, 22153, 22155, 83, GumpButtonType.Reply, 0);
            AddHtml(
                273,
                110,
                86,
                20,
                "<basefont size=5 color=#E6FBFF><center>ItemID",
                false,
                false
            );

            AddButton(253, 130, 22153, 22155, 84, GumpButtonType.Reply, 0);
            AddHtml(273, 130, 86, 20, "<basefont size=5 color=#E6FBFF><center>Props", false, false);

            AddButton(363, 110, 22150, 22155, 85, GumpButtonType.Reply, 0);
            AddHtml(383, 110, 86, 20, "<basefont size=5 color=#E6FBFF><center>Wipe", false, false);

            AddButton(363, 130, 22150, 22155, 86, GumpButtonType.Reply, 0);
            AddHtml(
                383,
                130,
                86,
                20,
                "<basefont size=5 color=#E6FBFF><center>Delete",
                false,
                false
            );

            AddButton(253, 170, 22150, 22155, 87, GumpButtonType.Reply, 0);
            AddHtml(
                273,
                170,
                86,
                20,
                "<basefont size=5 color=#E6FBFF><center>M Delete",
                false,
                false
            );

            AddButton(363, 170, 22150, 22155, 88, GumpButtonType.Reply, 0);
            AddHtml(
                383,
                170,
                96,
                20,
                "<basefont size=5 color=#E6FBFF><center>Area Delete",
                false,
                false
            );

            AddButton(940, 90, 2708, 2709, 90, GumpButtonType.Reply, 0); // Close
            AddButton(920, 90, 2710, 2711, 89, GumpButtonType.Reply, 0); // Minimize
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            string prefix = Server.Commands.CommandSystem.Prefix;

            switch (info.ButtonID)
            {
                case 1:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 2:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 3:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 4:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 5:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 6:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 7:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 8:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 9:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 10:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc z -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 11:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 12:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 13:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 14:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 15:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 16:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 17:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 18:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 19:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 20:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc z -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 21:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 22:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 23:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 24:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 25:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 26:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 27:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 28:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 29:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 30:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 31:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 32:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 33:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 34:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 35:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 36:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 37:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 38:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc y 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 39:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 40:
                {
                    CommandSystem.Handle(from, String.Format("{0}inc x 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 41:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 42:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 43:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x -1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 44:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x 1", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 45:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 46:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 47:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x -3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 48:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x 3", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 49:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 50:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 51:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x -5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 52:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x 5", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 53:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 54:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 55:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x -10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 56:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x 10", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 57:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 58:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc y 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 59:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x -20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 60:
                {
                    CommandSystem.Handle(from, String.Format("{0}area inc x 20", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 61:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddArch", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 62:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddDoor", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 63:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddFence", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 64:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddFloor", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 65:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddPlants", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 66:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddSign", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 67:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddStair", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 68:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddRock", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 69:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddRoof", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 70:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddWall", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 71:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddGearStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 72:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddCookingStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 73:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddFurniture", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 74:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddDungStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 75:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddLightsStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 76:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddGroundStStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 77:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddSignPStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 78:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddDecoStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 79:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddNatureStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 80:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddMiscStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 81:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddCustomStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 82:
                {
                    CommandSystem.Handle(from, String.Format("{0}AddAnimationStatic", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 83:
                {
                    CommandSystem.Handle(from, String.Format("{0}Get ItemID", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 84:
                {
                    CommandSystem.Handle(from, String.Format("{0}Props", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 85:
                {
                    CommandSystem.Handle(from, String.Format("{0}Wipe", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 86:
                {
                    CommandSystem.Handle(from, String.Format("{0}Delete", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 87:
                {
                    CommandSystem.Handle(from, String.Format("{0}M Delete", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 88:
                {
                    CommandSystem.Handle(from, String.Format("{0}Area Delete", prefix));
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    break;
                }
                case 89:
                {
                    from.SendGump(new MinStaticGump2());
                    from.CloseGump(typeof(OzThothsStaticOtherGump));
                    break;
                }
                case 90:
                {
                    from.CloseGump(typeof(OzThothsStaticOtherGump));
                    break;
                }
            }
        }
    }

    public class MinStaticGump2 : Gump
    {
        Mobile caller;

        public static void Initialize() { }

        public MinStaticGump2(Mobile from)
            : this()
        {
            caller = from;
        }

        public MinStaticGump2()
            : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            Dragable = true;
            Closable = true;
            Resizable = false;
            Disposable = false;

            AddPage(0);
            AddImageTiled(90, 230, 21, 21, 2624);
            AddBackground(80, 220, 167, 38, 2620);
            AddButton(230, 230, 2706, 2707, 1, GumpButtonType.Reply, 0);
            AddHtml(
                90,
                230,
                133,
                19,
                "<basefont size=5 color=#E6FBFF><center>Oz's Static Tool",
                false,
                false
            );
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1:
                {
                    from.SendGump(new OzThothsStaticOtherGump(from));
                    from.CloseGump(typeof(MinStaticGump2));
                    break;
                }
            }
        }
    }
}
