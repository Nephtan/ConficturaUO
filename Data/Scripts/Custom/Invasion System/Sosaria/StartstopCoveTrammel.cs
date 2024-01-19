using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class StartStopCovetram : Gump
    {
        private Mobile m_Mobile;

        public StartStopCovetram(Mobile from)
            : base(0, 0)
        {
            m_Mobile = from;
            Closable = false;
            Dragable = true;

            AddPage(0);

            AddImage(112, 73, 39);
            AddButton(135, 123, 9804, 9806, 1, GumpButtonType.Reply, 1);
            AddButton(138, 194, 9804, 9806, 2, GumpButtonType.Reply, 2);
            AddButton(277, 311, 2453, 2455, 0, GumpButtonType.Reply, 0);
            AddLabel(216, 140, 0, "Start an Invasion");
            AddLabel(218, 208, 0, "Stop an Invasion");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            switch (info.ButtonID)
            {
                case 0:
                {
                    from.CloseGump(typeof(StartStopCovetram));
                    from.SendGump(new CityInvasion(from));
                    break;
                }
                case 1:
                {
                    Point3D loc = new Point3D(568, 1311, 0);

                    WayPoint point = new WayPoint();
                    WayPoint point1 = new WayPoint();
                    WayPoint point2 = new WayPoint();
                    WayPoint point3 = new WayPoint();
                    WayPoint point4 = new WayPoint();
                    WayPoint point5 = new WayPoint();
                    WayPoint point6 = new WayPoint();
                    WayPoint point7 = new WayPoint();
                    WayPoint point8 = new WayPoint();
                    WayPoint point9 = new WayPoint();

                    WayPoint point10 = new WayPoint();
                    WayPoint point11 = new WayPoint();
                    WayPoint point12 = new WayPoint();
                    WayPoint point13 = new WayPoint();
                    WayPoint point14 = new WayPoint();
                    WayPoint point15 = new WayPoint();
                    WayPoint point16 = new WayPoint();
                    WayPoint point17 = new WayPoint();
                    WayPoint point18 = new WayPoint();
                    WayPoint point19 = new WayPoint();

                    WayPoint point20 = new WayPoint();
                    WayPoint point21 = new WayPoint();
                    WayPoint point22 = new WayPoint();
                    WayPoint point23 = new WayPoint();
                    WayPoint point24 = new WayPoint();
                    WayPoint point25 = new WayPoint();
                    WayPoint point26 = new WayPoint();
                    WayPoint point27 = new WayPoint();
                    WayPoint point28 = new WayPoint();
                    WayPoint point29 = new WayPoint();

                    WayPoint point30 = new WayPoint();
                    WayPoint point31 = new WayPoint();
                    WayPoint point32 = new WayPoint();
                    WayPoint point33 = new WayPoint();
                    WayPoint point34 = new WayPoint();
                    WayPoint point35 = new WayPoint();
                    WayPoint point36 = new WayPoint();
                    WayPoint point37 = new WayPoint();

                    point.Name = "CoveInvasionSosaria";
                    point1.Name = "CoveInvasionSosaria";
                    point2.Name = "CoveInvasionSosaria";
                    point3.Name = "CoveInvasionSosaria";
                    point4.Name = "CoveInvasionSosaria";
                    point5.Name = "CoveInvasionSosaria";
                    point6.Name = "CoveInvasionSosaria";
                    point7.Name = "CoveInvasionSosaria";
                    point8.Name = "CoveInvasionSosaria";
                    point9.Name = "CoveInvasionSosaria";

                    point10.Name = "CoveInvasionSosaria";
                    point11.Name = "CoveInvasionSosaria";
                    point12.Name = "CoveInvasionSosaria";
                    point13.Name = "CoveInvasionSosaria";
                    point14.Name = "CoveInvasionSosaria";
                    point15.Name = "CoveInvasionSosaria";
                    point16.Name = "CoveInvasionSosaria";
                    point17.Name = "CoveInvasionSosaria";
                    point18.Name = "CoveInvasionSosaria";
                    point19.Name = "CoveInvasionSosaria";

                    point20.Name = "CoveInvasionSosaria";
                    point21.Name = "CoveInvasionSosaria";
                    point22.Name = "CoveInvasionSosaria";
                    point23.Name = "CoveInvasionSosaria";
                    point24.Name = "CoveInvasionSosaria";
                    point25.Name = "CoveInvasionSosaria";
                    point26.Name = "CoveInvasionSosaria";
                    point27.Name = "CoveInvasionSosaria";
                    point28.Name = "CoveInvasionSosaria";
                    point29.Name = "CoveInvasionSosaria";

                    point30.Name = "CoveInvasionSosaria";
                    point31.Name = "CoveInvasionSosaria";
                    point32.Name = "CoveInvasionSosaria";
                    point33.Name = "CoveInvasionSosaria";
                    point34.Name = "CoveInvasionSosaria";
                    point35.Name = "CoveInvasionSosaria";
                    point36.Name = "CoveInvasionSosaria";
                    point37.Name = "CoveInvasionSosaria";

                    Spawner spawner1 = new Spawner(4, 5, 15, 0, 4, "OrcBomber");
                    spawner1.MoveToWorld(new Point3D(2322, 1127, 0), Map.Sosaria);
                    spawner1.WayPoint = point;
                    point.MoveToWorld(new Point3D(2313, 1169, 0), Map.Sosaria);
                    point.NextPoint = point1;
                    point1.MoveToWorld(new Point3D(2285, 1210, 0), Map.Sosaria);
                    point1.NextPoint = point2;
                    point2.MoveToWorld(new Point3D(2253, 1210, 0), Map.Sosaria);
                    point2.NextPoint = point3;
                    point3.MoveToWorld(new Point3D(2236, 1216, 0), Map.Sosaria);
                    point3.NextPoint = point4;
                    point4.MoveToWorld(new Point3D(2236, 1196, 0), Map.Sosaria);
                    point4.NextPoint = point5;
                    point5.MoveToWorld(new Point3D(2243, 1196, 0), Map.Sosaria);
                    point5.NextPoint = point6;
                    point6.MoveToWorld(new Point3D(2243, 1182, 0), Map.Sosaria);
                    point6.NextPoint = point7;
                    point7.MoveToWorld(new Point3D(2227, 1166, 0), Map.Sosaria);
                    point7.NextPoint = point8;
                    point8.MoveToWorld(new Point3D(2221, 1205, 0), Map.Sosaria);
                    point8.NextPoint = point9;
                    point9.MoveToWorld(new Point3D(2237, 1210, 0), Map.Sosaria);
                    point9.NextPoint = point2;
                    spawner1.Name = "CoveInvasionSosaria";
                    spawner1.Respawn();

                    Spawner spawner2 = new Spawner(4, 5, 15, 0, 10, "Orc");
                    spawner2.MoveToWorld(new Point3D(2348, 1214, 0), Map.Sosaria);
                    spawner2.WayPoint = point10;
                    point10.MoveToWorld(new Point3D(2285, 1210, 0), Map.Sosaria);
                    point10.NextPoint = point11;
                    point11.MoveToWorld(new Point3D(2268, 1210, 0), Map.Sosaria);
                    point11.NextPoint = point12;
                    point12.MoveToWorld(new Point3D(2268, 1231, 0), Map.Sosaria);
                    point12.NextPoint = point13;
                    point13.MoveToWorld(new Point3D(2251, 1231, 0), Map.Sosaria);
                    point13.NextPoint = point14;
                    point14.MoveToWorld(new Point3D(2251, 1214, 0), Map.Sosaria);
                    point14.NextPoint = point15;
                    point15.MoveToWorld(new Point3D(2234, 1211, 0), Map.Sosaria);
                    point15.NextPoint = point16;
                    point16.MoveToWorld(new Point3D(2210, 1200, 0), Map.Sosaria);
                    point16.NextPoint = point17;
                    point17.MoveToWorld(new Point3D(2210, 1177, 0), Map.Sosaria);
                    point17.NextPoint = point18;
                    point18.MoveToWorld(new Point3D(2240, 1177, 0), Map.Sosaria);
                    point18.NextPoint = point19;
                    point19.MoveToWorld(new Point3D(2276, 1211, 0), Map.Sosaria);
                    point19.NextPoint = point11;
                    spawner2.Name = "CoveInvasionSosaria";
                    spawner2.Respawn();

                    Spawner spawner3 = new Spawner(6, 5, 15, 0, 0, "Orc");
                    spawner3.MoveToWorld(new Point3D(2210, 1274, 0), Map.Sosaria);
                    spawner3.WayPoint = point20;
                    point20.MoveToWorld(new Point3D(2284, 1268, 0), Map.Sosaria);
                    point20.NextPoint = point21;
                    point21.MoveToWorld(new Point3D(2294, 1207, 0), Map.Sosaria);
                    point21.NextPoint = point22;
                    point22.MoveToWorld(new Point3D(2286, 1186, 0), Map.Sosaria);
                    point22.NextPoint = point23;
                    point23.MoveToWorld(new Point3D(2286, 1218, 0), Map.Sosaria);
                    point23.NextPoint = point22;
                    spawner3.Name = "CoveInvasionSosaria";
                    spawner3.Respawn();

                    Spawner spawner4 = new Spawner(4, 5, 15, 0, 20, "Orc");
                    spawner4.MoveToWorld(new Point3D(2364, 1205, 5), Map.Sosaria);
                    spawner4.WayPoint = point24;
                    point24.MoveToWorld(new Point3D(2285, 1210, 0), Map.Sosaria);
                    point24.NextPoint = point25;
                    point25.MoveToWorld(new Point3D(2275, 1209, 0), Map.Sosaria);
                    point25.NextPoint = point26;
                    point26.MoveToWorld(new Point3D(2275, 1203, 0), Map.Sosaria);
                    point26.NextPoint = point27;
                    point27.MoveToWorld(new Point3D(2272, 1200, 2), Map.Sosaria);
                    point27.NextPoint = point28;
                    point28.MoveToWorld(new Point3D(2272, 1196, 20), Map.Sosaria);
                    point28.NextPoint = point29;
                    point29.MoveToWorld(new Point3D(2282, 1200, 20), Map.Sosaria);
                    point29.NextPoint = point30;
                    point30.MoveToWorld(new Point3D(2282, 1187, 20), Map.Sosaria);
                    point30.NextPoint = point31;
                    point31.MoveToWorld(new Point3D(2282, 1222, 20), Map.Sosaria);
                    point31.NextPoint = point32;
                    point32.MoveToWorld(new Point3D(2290, 1222, 20), Map.Sosaria);
                    point32.NextPoint = point33;
                    point33.MoveToWorld(new Point3D(2287, 1228, 20), Map.Sosaria);
                    point33.NextPoint = point34;
                    point34.MoveToWorld(new Point3D(2280, 1224, 20), Map.Sosaria);
                    point34.NextPoint = point35;
                    point35.MoveToWorld(new Point3D(2280, 1227, 7), Map.Sosaria);
                    point35.NextPoint = point36;
                    point36.MoveToWorld(new Point3D(2283, 1233, 0), Map.Sosaria);
                    point36.NextPoint = point37;
                    point37.MoveToWorld(new Point3D(2273, 1233, 0), Map.Sosaria);
                    point37.NextPoint = point26;
                    spawner4.Name = "CoveInvasionSosaria";
                    spawner4.Respawn();

                    Spawner spawner5 = new Spawner(4, 5, 15, 0, 8, "OrcishLord");
                    spawner5.MoveToWorld(new Point3D(2219, 1117, 17), Map.Sosaria);
                    spawner5.Name = "CoveInvasionSosaria";
                    spawner5.Respawn();

                    Spawner spawner6 = new Spawner(3, 5, 15, 0, 8, "OrcishMage");
                    spawner6.MoveToWorld(new Point3D(2322, 1127, 0), Map.Sosaria);
                    spawner6.Name = "CoveInvasionSosaria";
                    spawner6.Respawn();

                    Spawner spawner7 = new Spawner(3, 5, 15, 0, 8, "OrcishLord");
                    spawner7.MoveToWorld(new Point3D(2348, 1214, 0), Map.Sosaria);
                    spawner7.Name = "CoveInvasionSosaria";
                    spawner7.Respawn();

                    Spawner spawner8 = new Spawner(3, 5, 15, 0, 8, "OrcishMage");
                    spawner8.MoveToWorld(new Point3D(2210, 1274, 0), Map.Sosaria);
                    spawner8.Name = "CoveInvasionSosaria";
                    spawner8.Respawn();

                    Spawner spawner9 = new Spawner(3, 5, 15, 0, 8, "OrcishLord");
                    spawner9.MoveToWorld(new Point3D(2364, 1205, 5), Map.Sosaria);
                    spawner9.Name = "CoveInvasionSosaria";
                    spawner9.Respawn();

                    Spawner spawner10 = new Spawner(3, 5, 15, 0, 20, "OrcScout");
                    spawner10.MoveToWorld(new Point3D(2348, 1214, 0), Map.Sosaria);
                    spawner10.Name = "CoveInvasionSosaria";
                    spawner10.Respawn();

                    Spawner spawner11 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner11.MoveToWorld(new Point3D(2348, 1214, 0), Map.Sosaria);
                    spawner11.Name = "CoveInvasionSosaria";
                    spawner11.Respawn();

                    Spawner spawner12 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner12.MoveToWorld(new Point3D(2225, 1168, 0), Map.Sosaria);
                    spawner12.Name = "CoveInvasionSosaria";
                    spawner12.Respawn();

                    Spawner spawner13 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner13.MoveToWorld(new Point3D(2216, 1190, 0), Map.Sosaria);
                    spawner13.Name = "CoveInvasionSosaria";
                    spawner13.Respawn();

                    Spawner spawner14 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner14.MoveToWorld(new Point3D(2234, 1210, 0), Map.Sosaria);
                    spawner14.Name = "CoveInvasionSosaria";
                    spawner14.Respawn();

                    Spawner spawner15 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner15.MoveToWorld(new Point3D(2277, 1200, 20), Map.Sosaria);
                    spawner15.Name = "CoveInvasionSosaria";
                    spawner15.Respawn();

                    Spawner spawner16 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner16.MoveToWorld(new Point3D(2295, 1203, 0), Map.Sosaria);
                    spawner16.Name = "CoveInvasionSosaria";
                    spawner16.Respawn();

                    Spawner spawner17 = new Spawner(10, 5, 15, 0, 25, "Orc");
                    spawner17.MoveToWorld(new Point3D(2295, 1203, 0), Map.Sosaria);
                    spawner17.Name = "CoveInvasionSosaria";
                    spawner17.Respawn();

                    Spawner spawner18 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner18.MoveToWorld(new Point3D(2301, 1229, 0), Map.Sosaria);
                    spawner18.Name = "CoveInvasionSosaria";
                    spawner18.Respawn();

                    Spawner spawner19 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner19.MoveToWorld(new Point3D(2223, 1151, 0), Map.Sosaria);
                    spawner19.Name = "CoveInvasionSosaria";
                    spawner19.Respawn();

                    Spawner spawner20 = new Spawner(10, 5, 15, 0, 15, "Orc");
                    spawner20.MoveToWorld(new Point3D(2224, 1181, 0), Map.Sosaria);
                    spawner20.Name = "CoveInvasionSosaria";
                    spawner20.Respawn();

                    Spawner spawner21 = new Spawner(10, 5, 15, 0, 15, "Orc");
                    spawner21.MoveToWorld(new Point3D(2247, 1219, 0), Map.Sosaria);
                    spawner21.Name = "CoveInvasionSosaria";
                    spawner21.Respawn();

                    Spawner spawner22 = new Spawner(5, 5, 15, 0, 15, "Orc");
                    spawner22.MoveToWorld(new Point3D(2224, 1181, 0), Map.Sosaria);
                    spawner22.Name = "CoveInvasionSosaria";
                    spawner22.Respawn();

                    Spawner spawner23 = new Spawner(3, 5, 15, 0, 10, "OrcScout");
                    spawner23.MoveToWorld(new Point3D(2299, 1252, 0), Map.Sosaria);
                    spawner23.Name = "CoveInvasionSosaria";
                    spawner23.Respawn();

                    Spawner spawner24 = new Spawner(3, 5, 15, 0, 8, "OrcishMage");
                    spawner24.MoveToWorld(new Point3D(2257, 1204, 0), Map.Sosaria);
                    spawner24.Name = "CoveInvasionSosaria";
                    spawner24.Respawn();

                    Spawner spawner25 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner25.MoveToWorld(new Point3D(2222, 1129, 0), Map.Sosaria);
                    spawner25.Name = "CoveInvasionSosaria";
                    spawner25.Respawn();

                    Spawner spawner26 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner26.MoveToWorld(new Point3D(2271, 1218, 0), Map.Sosaria);
                    spawner26.Name = "CoveInvasionSosaria";
                    spawner26.Respawn();

                    Spawner spawner27 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner27.MoveToWorld(new Point3D(2233, 1240, 0), Map.Sosaria);
                    spawner27.Name = "CoveInvasionSosaria";
                    spawner27.Respawn();

                    Spawner spawner28 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner28.MoveToWorld(new Point3D(2289, 1222, 20), Map.Sosaria);
                    spawner28.Name = "CoveInvasionSosaria";
                    spawner28.Respawn();

                    Spawner spawner29 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner29.MoveToWorld(new Point3D(2281, 1192, 20), Map.Sosaria);
                    spawner29.Name = "CoveInvasionSosaria";
                    spawner29.Respawn();

                    Spawner spawner30 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner30.MoveToWorld(new Point3D(2303, 1131, 0), Map.Sosaria);
                    spawner30.Name = "CoveInvasionSosaria";
                    spawner30.Respawn();

                    Spawner spawner31 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner31.MoveToWorld(new Point3D(2292, 1110, 0), Map.Sosaria);
                    spawner31.Name = "CoveInvasionSosaria";
                    spawner31.Respawn();

                    Spawner spawner32 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner32.MoveToWorld(new Point3D(2347, 1151, 5), Map.Sosaria);
                    spawner32.Name = "CoveInvasionSosaria";
                    spawner32.Respawn();

                    Spawner spawner33 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner32.MoveToWorld(new Point3D(2361, 1183, 10), Map.Sosaria);
                    spawner33.Name = "CoveInvasionSosaria";
                    spawner33.Respawn();

                    Spawner spawner34 = new Spawner(5, 5, 15, 0, 10, "Orc");
                    spawner34.MoveToWorld(new Point3D(2351, 1245, 10), Map.Sosaria);
                    spawner34.Name = "CoveInvasionSosaria";
                    spawner34.Respawn();

                    Spawner spawner35 = new Spawner(1, 2, 2, 0, 2, "LordOrcalis");
                    spawner35.MoveToWorld(new Point3D(2213, 1115, 40), Map.Sosaria);
                    spawner35.Name = "CoveInvasionSosaria";
                    spawner35.Respawn();

                    Spawner spawner36 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner36.MoveToWorld(new Point3D(2270, 1127, 40), Map.Sosaria);
                    spawner36.Name = "CoveInvasionSosaria";
                    spawner36.Respawn();

                    Spawner spawner37 = new Spawner(3, 5, 15, 0, 10, "OrcScout");
                    spawner37.MoveToWorld(new Point3D(2270, 1127, 40), Map.Sosaria);
                    spawner37.Name = "CoveInvasionSosaria";
                    spawner37.Respawn();

                    Spawner spawner38 = new Spawner(10, 20, 20, 0, 20, "OrcBomber");
                    spawner38.MoveToWorld(new Point3D(2253, 1193, -2), Map.Sosaria);
                    spawner38.Name = "CoveInvasionSosaria";
                    spawner38.Respawn();

                    Spawner spawner39 = new Spawner(10, 20, 20, 0, 20, "OrcBomber");
                    spawner39.MoveToWorld(new Point3D(2225, 1141, 0), Map.Sosaria);
                    spawner39.Name = "CoveInvasionSosaria";
                    spawner39.Respawn();

                    World.Broadcast(33, true, "Cove Sosaria is under invasion.");
                    from.SendGump(new CityInvasion(from));
                    break;
                }
                case 2:
                {
                    CoveInvasionStone covetram = new CoveInvasionStone();
                    covetram.StopCoveSosaria();
                    World.Broadcast(
                        33,
                        true,
                        "Cove Sosaria's invasion was successfully beaten back. No more invaders are left in the city."
                    );
                    from.SendGump(new CityInvasion(from));
                    break;
                }
            }
        }
    }
}
