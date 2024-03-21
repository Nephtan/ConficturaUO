using System;
using Server;
using Server.Commands;
using Server.Network;

namespace Server.Gumps
{
    public class CityInvasion : Gump
    {
        private Mobile m_Mobile;

        public static void Initialize()
        {
            CommandSystem.Register(
                "invasion",
                AccessLevel.Administrator,
                new CommandEventHandler(CityInvasion_OnCommand)
            );
        }

        private static void CityInvasion_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new CityInvasion(e.Mobile));
        }

        public CityInvasion(Mobile from)
            : base(0, 0)
        {
            m_Mobile = from;
            Closable = false;
            Dragable = true;

            AddPage(2);

            AddImage(83, 32, 9001);
            AddButton(215, 321, 9804, 9806, 1, GumpButtonType.Reply, 1);
            AddLabel(288, 332, 1152, "Britian");
            AddLabel(287, 347, 1152, "Lodor");
            AddButton(214, 375, 9804, 9806, 2, GumpButtonType.Reply, 2);
            AddLabel(286, 393, 1152, "Minoc");
            AddLabel(285, 406, 1152, "Lodor");
            AddButton(215, 426, 9804, 9806, 3, GumpButtonType.Reply, 3);
            AddLabel(288, 442, 1152, "Cove");
            AddLabel(287, 456, 1152, "Lodor");
            AddButton(349, 321, 9804, 9806, 4, GumpButtonType.Reply, 4);
            AddLabel(422, 333, 1152, "Buc's Den");
            AddLabel(421, 347, 1152, "Lodor");
            AddButton(349, 376, 9804, 9806, 5, GumpButtonType.Reply, 5);
            AddLabel(423, 390, 1152, "Trinsic");
            AddLabel(422, 404, 1152, "Lodor");
            AddButton(349, 427, 9804, 9806, 6, GumpButtonType.Reply, 6);
            AddLabel(425, 438, 1152, "Vesper");
            AddLabel(424, 456, 1152, "Lodor");
            AddButton(349, 478, 9804, 9806, 24, GumpButtonType.Reply, 24);
            AddLabel(428, 492, 1152, "Nu'Jelm");
            AddLabel(427, 510, 1152, "Lodor");
            AddButton(91, 530, 9804, 9806, 7, GumpButtonType.Page, 1);
            AddLabel(173, 553, 1152, "Sosaria");
            AddButton(256, 532, 9804, 9806, 8, GumpButtonType.Page, 2);
            AddLabel(332, 553, 1152, "Lodor");
            AddButton(430, 532, 9804, 9806, 9, GumpButtonType.Page, 3);
            AddLabel(508, 553, 1152, "Underworld");
            AddButton(608, 531, 9804, 9806, 10, GumpButtonType.Page, 4);
            AddLabel(682, 553, 1152, "SerpentIsland");
            AddButton(487, 322, 9804, 9806, 11, GumpButtonType.Reply, 11);
            AddLabel(562, 336, 1152, "Skara Brae");
            AddLabel(563, 351, 1152, "Lodor");
            AddButton(485, 376, 9804, 9806, 12, GumpButtonType.Reply, 12);
            AddLabel(561, 390, 1152, "Magincia");
            AddLabel(561, 405, 1152, "Lodor");
            AddButton(484, 427, 9804, 9806, 13, GumpButtonType.Reply, 13);
            AddLabel(560, 441, 1152, "Yew");
            AddLabel(559, 456, 1152, "Lodor");
            AddImage(162, 219, 10400);
            AddImage(160, 439, 10402);
            AddImage(629, 212, 10410);
            AddImage(631, 439, 10412);
            AddImage(33, 0, 10440);
            AddImage(692, 0, 10441);
            AddButton(628, 90, 241, 243, 0, GumpButtonType.Reply, 0);
            AddLabel(380, 73, 32, "City Invasion");

            AddPage(1);

            AddImage(83, 32, 9001);
            AddButton(215, 321, 9804, 9806, 14, GumpButtonType.Reply, 14);
            AddLabel(288, 332, 1152, "Britian");
            AddLabel(287, 347, 1152, "Sosaria");
            AddButton(214, 375, 9804, 9806, 15, GumpButtonType.Reply, 15);
            AddLabel(286, 390, 1152, "Minoc");
            AddLabel(285, 403, 1152, "Sosaria");
            AddButton(215, 426, 9804, 9806, 16, GumpButtonType.Reply, 16);
            AddLabel(288, 439, 1152, "Cove");
            AddLabel(287, 453, 1152, "Sosaria");
            AddButton(349, 321, 9804, 9806, 17, GumpButtonType.Reply, 17);
            AddLabel(422, 333, 1152, "Buc's Den");
            AddLabel(421, 347, 1152, "Sosaria");
            AddButton(349, 376, 9804, 9806, 18, GumpButtonType.Reply, 18);
            AddLabel(422, 390, 1152, "Trinsic");
            AddLabel(421, 403, 1152, "Sosaria");
            AddButton(349, 427, 9804, 9806, 19, GumpButtonType.Reply, 19);
            AddLabel(425, 438, 1152, "Vesper");
            AddLabel(424, 456, 1152, "Sosaria");
            AddButton(349, 478, 9804, 9806, 23, GumpButtonType.Reply, 23);
            AddLabel(428, 492, 1152, "Nu'Jelm");
            AddLabel(427, 510, 1152, "Sosaria");
            AddButton(91, 530, 9804, 9806, 7, GumpButtonType.Page, 1);
            AddLabel(173, 553, 1152, "Sosaria");
            AddButton(256, 532, 9804, 9806, 8, GumpButtonType.Page, 2);
            AddLabel(332, 553, 1152, "Lodor");
            AddButton(430, 532, 9804, 9806, 9, GumpButtonType.Page, 3);
            AddLabel(508, 553, 1152, "Underworld");
            AddButton(608, 531, 9804, 9806, 10, GumpButtonType.Page, 4);
            AddLabel(682, 553, 1152, "SerpentIsland");
            AddButton(487, 322, 9804, 9806, 20, GumpButtonType.Reply, 20);
            AddLabel(562, 336, 1152, "Skara Brae");
            AddLabel(563, 351, 1152, "Sosaria");
            AddButton(485, 376, 9804, 9806, 21, GumpButtonType.Reply, 21);
            AddLabel(561, 390, 1152, "Magincia");
            AddLabel(561, 405, 1152, "Sosaria");
            AddButton(484, 427, 9804, 9806, 22, GumpButtonType.Reply, 22);
            AddLabel(560, 441, 1152, "Yew");
            AddLabel(559, 456, 1152, "Sosaria");
            AddImage(162, 219, 10400);
            AddImage(160, 439, 10402);
            AddImage(629, 212, 10410);
            AddImage(631, 439, 10412);
            AddImage(33, 0, 10440);
            AddImage(692, 0, 10441);
            AddButton(628, 90, 241, 243, 55, GumpButtonType.Reply, 0);
            AddLabel(379, 74, 32, "City Invasion");

            AddPage(3);

            AddImage(83, 32, 9001);
            AddButton(215, 321, 9804, 9806, 25, GumpButtonType.Reply, 25);
            AddLabel(287, 345, 1152, "Mistas");
            AddButton(214, 375, 9804, 9806, 26, GumpButtonType.Reply, 26);
            AddLabel(285, 398, 1152, "Montor");
            AddButton(215, 426, 9804, 9806, 27, GumpButtonType.Reply, 27);
            AddLabel(282, 448, 1152, "Reg Volon");
            AddButton(349, 321, 9804, 9806, 28, GumpButtonType.Reply, 28);
            AddLabel(421, 344, 1152, "Savage Camp");
            AddButton(349, 376, 9804, 9806, 29, GumpButtonType.Reply, 29);
            AddLabel(422, 400, 1152, "Gargoyle City");
            AddButton(349, 427, 9804, 9806, 30, GumpButtonType.Reply, 30);
            AddLabel(421, 449, 1152, "Lakeshire/Mireg");
            AddButton(91, 530, 9804, 9806, 7, GumpButtonType.Page, 1);
            AddLabel(173, 553, 1152, "Sosaria");
            AddButton(256, 532, 9804, 9806, 8, GumpButtonType.Page, 2);
            AddLabel(332, 553, 1152, "Lodor");
            AddButton(430, 532, 9804, 9806, 9, GumpButtonType.Page, 3);
            AddLabel(508, 553, 1152, "Underworld");
            AddButton(608, 531, 9804, 9806, 10, GumpButtonType.Page, 4);
            AddLabel(682, 553, 1152, "SerpentIsland");
            AddButton(509, 323, 9804, 9806, 31, GumpButtonType.Reply, 31);
            AddButton(509, 378, 9804, 9806, 32, GumpButtonType.Reply, 32);
            AddLabel(574, 345, 1152, "Ancient Citadel");
            AddLabel(578, 401, 1152, "Terort Skitas");
            AddImage(162, 219, 10400);
            AddImage(160, 439, 10402);
            AddImage(629, 212, 10410);
            AddImage(631, 439, 10412);
            AddImage(33, 0, 10440);
            AddImage(692, 0, 10441);
            AddButton(628, 90, 241, 243, 55, GumpButtonType.Reply, 0);
            AddLabel(379, 74, 32, "City Invasion");

            AddPage(4);

            AddImage(83, 32, 9001);
            AddButton(219, 341, 9804, 9806, 33, GumpButtonType.Reply, 33);
            AddLabel(296, 362, 1152, "Luna");
            AddButton(218, 409, 9804, 9806, 34, GumpButtonType.Reply, 34);
            AddLabel(294, 431, 1152, "Umbra");
            AddButton(91, 530, 9804, 9806, 7, GumpButtonType.Page, 1);
            AddLabel(173, 553, 1152, "Sosaria");
            AddButton(256, 532, 9804, 9806, 8, GumpButtonType.Page, 2);
            AddLabel(332, 553, 1152, "Lodor");
            AddButton(430, 532, 9804, 9806, 9, GumpButtonType.Page, 3);
            AddLabel(508, 553, 1152, "Underworld");
            AddButton(608, 531, 9804, 9806, 10, GumpButtonType.Page, 4);
            AddLabel(682, 553, 1152, "SerpentIsland");
            AddImage(162, 219, 10400);
            AddImage(160, 439, 10402);
            AddImage(629, 212, 10410);
            AddImage(631, 439, 10412);
            AddImage(33, 0, 10440);
            AddImage(692, 0, 10441);
            AddButton(628, 90, 241, 243, 55, GumpButtonType.Reply, 0);
            AddLabel(380, 73, 32, "City Invasion");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            switch (info.ButtonID)
            {
                case 0:
                {
                    from.CloseGump(typeof(CityInvasion));
                    break;
                }
                case 1:
                {
                    from.SendGump(new StartStopBritfel(from));
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(1430, 1688, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Britain."
                    );
                    break;
                }
                case 2:
                {
                    from.SendGump(new StartStopMinocfel(from));
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(2503, 563, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Minoc."
                    );
                    break;
                }
                case 3:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(2285, 1209, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Cove."
                    );
                    from.SendGump(new StartStopCovefel(from));
                    break;
                }
                case 4:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(2685, 2159, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Buccaneers Den."
                    );
                    from.SendGump(new StartStopBuccaneersDenfel(from));
                    break;
                }
                case 5:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(1927, 2779, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Trinsic."
                    );
                    from.SendGump(new StartStopTrinsicfel(from));
                    break;
                }
                case 6:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(2882, 788, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Vesper."
                    );
                    from.SendGump(new StartStopVesperfel(from));
                    break;
                }
                case 11:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(734, 2236, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Skara Brae."
                    );
                    from.SendGump(new StartStopSkaraBraefel(from));
                    break;
                }
                case 12:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(3709, 2074, 5);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Magincia."
                    );
                    from.SendGump(new StartStopMaginciafel(from));
                    break;
                }
                case 13:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(535, 992, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Yew."
                    );
                    from.SendGump(new StartStopYewfel(from));
                    break;
                }
                case 14:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(2999, 1053, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Britain."
                    );
                    from.SendGump(new StartStopBrittram(from));
                    break;
                }
                case 15:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(2503, 563, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Minoc."
                    );
                    from.SendGump(new StartStopMinoctram(from));
                    break;
                }
                case 16:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(2285, 1209, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Cove."
                    );
                    from.SendGump(new StartStopCovetram(from));
                    break;
                }
                case 17:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(2691, 2115, 5);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Buccaneers Den."
                    );
                    from.SendGump(new StartStopBuccaneersDentram(from));

                    break;
                }
                case 18:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(1927, 2779, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Trinsic."
                    );
                    from.SendGump(new StartStopTrinsictram(from));
                    break;
                }
                case 19:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(2882, 788, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Vesper."
                    );
                    from.SendGump(new StartStopVespertram(from));
                    break;
                }
                case 20:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(734, 2236, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Skara Brae."
                    );
                    from.SendGump(new StartStopSkaraBraetram(from));
                    break;
                }
                case 21:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(3709, 2074, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Magincia."
                    );
                    from.SendGump(new StartStopMaginciatram(from));
                    break;
                }
                case 22:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(535, 992, 0);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Yew."
                    );
                    from.SendGump(new StartStopYewtram(from));

                    break;
                }
                case 23:
                {
                    from.Map = Map.Sosaria;
                    from.Location = new Point3D(3728, 1360, 5);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Nu'jelm."
                    );
                    from.SendGump(new StartStopNujelmtram(from));
                    break;
                }
                case 24:
                {
                    from.Map = Map.Lodor;
                    from.Location = new Point3D(3728, 1360, 5);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Nu'jelm."
                    );
                    from.SendGump(new StartStopNujelmtram(from));
                    break;
                }
                case 25:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(820, 1073, -30);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Mistas."
                    );
                    from.SendGump(new StartStopMistasilsh(from));
                    break;
                }
                case 26:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(1643, 310, 48);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Montor."
                    );
                    from.SendGump(new StartStopMontorilsh(from));
                    break;
                }
                case 27:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(1362, 1073, -13);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Reg Volon."
                    );
                    from.SendGump(new StartStopRegVolonilsh(from));
                    break;
                }
                case 28:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(1251, 743, -80);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Savage Camp."
                    );
                    from.SendGump(new StartStopSavageCampilsh(from));
                    break;
                }
                case 29:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(834, 642, -40);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Gargoyle City."
                    );
                    from.SendGump(new StartStopGargoyleCityilsh(from));
                    break;
                }
                case 30:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(1203, 1124, -25);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Lakeshire."
                    );
                    from.SendGump(new StartStopLakeShireMiregilsh(from));
                    break;
                }
                case 31:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(1518, 567, -15);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Ancient Citadel."
                    );
                    from.SendGump(new StartStopAncientCitadelilsh(from));
                    break;
                }
                case 32:
                {
                    from.Map = Map.Underworld;
                    from.Location = new Point3D(567, 437, 21);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Terort Skitas."
                    );
                    from.SendGump(new StartStopTerortSkitasilsh(from));
                    break;
                }
                case 33:
                {
                    from.Map = Map.SerpentIsland;
                    from.Location = new Point3D(1028, 520, -53);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Luna."
                    );
                    from.SendGump(new StartStopLunaSerpentIsland(from));
                    break;
                }
                case 34:
                {
                    from.Map = Map.SerpentIsland;
                    from.Location = new Point3D(1949, 1320, -80);
                    from.PlaySound(0x1FA);
                    Effects.SendLocationEffect(from, from.Map, 14201, 16);
                    from.SendAsciiMessage(
                        "The world around you twists and fades. Suddenly, you are in Umbra."
                    );
                    from.SendGump(new StartStopUmbraSerpentIsland(from));
                    break;
                }
            }
        }
    }
}
