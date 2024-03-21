using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
    public class StartStopMaginciafel : Gump
    {
        public static readonly bool DummyMessage = false;

        private Mobile m_Mobile;

        public StartStopMaginciafel(Mobile from)
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
                    from.CloseGump(typeof(StartStopMaginciafel));
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
                    /*WayPoint point12 = new WayPoint();
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
                    WayPoint point38 = new WayPoint();
                    WayPoint point39 = new WayPoint();
        
                    WayPoint point40 = new WayPoint();
                    WayPoint point41 = new WayPoint();
                    WayPoint point42 = new WayPoint();
                    WayPoint point43 = new WayPoint();
                    WayPoint point44 = new WayPoint();
                    WayPoint point45 = new WayPoint();
                    WayPoint point46 = new WayPoint();
                    WayPoint point47 = new WayPoint();
                    WayPoint point48 = new WayPoint();
                    WayPoint point49 = new WayPoint();
        
                    WayPoint point50 = new WayPoint();
                    WayPoint point51 = new WayPoint();
                    WayPoint point52 = new WayPoint();
                    WayPoint point53 = new WayPoint();
                    WayPoint point54 = new WayPoint();
                    WayPoint point55 = new WayPoint();
                    WayPoint point56 = new WayPoint();
                    WayPoint point57 = new WayPoint();
                    WayPoint point58 = new WayPoint();
                    WayPoint point59 = new WayPoint();
        
                    WayPoint point60 = new WayPoint();
                    WayPoint point61 = new WayPoint();
                    WayPoint point62 = new WayPoint();
                    WayPoint point63 = new WayPoint();
                    WayPoint point64 = new WayPoint();
                    WayPoint point65 = new WayPoint();
                    WayPoint point66 = new WayPoint();
                    WayPoint point67 = new WayPoint();
                    WayPoint point68 = new WayPoint();
                    WayPoint point69 = new WayPoint();
        
                    WayPoint point70 = new WayPoint();
                    WayPoint point71 = new WayPoint();
                    WayPoint point72 = new WayPoint();
                    WayPoint point73 = new WayPoint();
                    WayPoint point74 = new WayPoint();
                    WayPoint point75 = new WayPoint();
                    WayPoint point76 = new WayPoint();
                    WayPoint point77 = new WayPoint();
                    WayPoint point78 = new WayPoint();
                    WayPoint point79 = new WayPoint();
                    WayPoint point80 = new WayPoint();
                    WayPoint point81 = new WayPoint();
        
                    WayPoint point82 = new WayPoint();
                    WayPoint point83 = new WayPoint();
                    WayPoint point84 = new WayPoint();
                    WayPoint point85 = new WayPoint();
                    WayPoint point86 = new WayPoint();
                    WayPoint point87 = new WayPoint();
                    WayPoint point88 = new WayPoint();
                    WayPoint point89 = new WayPoint();
                    WayPoint point90 = new WayPoint();
                    WayPoint point91 = new WayPoint();
                    WayPoint point92 = new WayPoint();
                    WayPoint point93 = new WayPoint();
        
                    WayPoint point94 = new WayPoint();
                    WayPoint point95 = new WayPoint();
                    WayPoint point96 = new WayPoint();
                    WayPoint point97 = new WayPoint();
                    WayPoint point98 = new WayPoint();
                    WayPoint point99 = new WayPoint();
                    WayPoint point100 = new WayPoint();
                    WayPoint point101 = new WayPoint();
                    WayPoint point102 = new WayPoint();
                    WayPoint point103 = new WayPoint();
                    WayPoint point104 = new WayPoint();
                    WayPoint point105 = new WayPoint();
        
                    WayPoint point106 = new WayPoint();
                    WayPoint point107 = new WayPoint();
                    WayPoint point108 = new WayPoint();
                    WayPoint point109 = new WayPoint();
                    WayPoint point110 = new WayPoint();
                    WayPoint point111 = new WayPoint();
                    WayPoint point112 = new WayPoint();
                    WayPoint point113 = new WayPoint();
                    WayPoint point114 = new WayPoint();
                    WayPoint point115 = new WayPoint();
                    WayPoint point116 = new WayPoint();
                    WayPoint point117 = new WayPoint();
                    WayPoint point118 = new WayPoint();
                    WayPoint point119 = new WayPoint();
                    WayPoint point120 = new WayPoint();*/

                    point.Name = "MaginciaInvasionLodor";
                    point1.Name = "MaginciaInvasionLodor";
                    point2.Name = "MaginciaInvasionLodor";
                    point3.Name = "MaginciaInvasionLodor";
                    point4.Name = "MaginciaInvasionLodor";
                    point5.Name = "MaginciaInvasionLodor";
                    point6.Name = "MaginciaInvasionLodor";
                    point7.Name = "MaginciaInvasionLodor";
                    point8.Name = "MaginciaInvasionLodor";
                    point9.Name = "MaginciaInvasionLodor";

                    point10.Name = "MaginciaInvasionLodor";
                    point11.Name = "MaginciaInvasionLodor";
                    /*point12.Name = "MaginciaInvasionLodor";
                    point13.Name = "MaginciaInvasionLodor";
                    point14.Name = "MaginciaInvasionLodor";
                    point15.Name = "MaginciaInvasionLodor";
                    point16.Name = "MaginciaInvasionLodor";
                    point17.Name = "MaginciaInvasionLodor";
                    point18.Name = "MaginciaInvasionLodor";
                    point19.Name = "MaginciaInvasionLodor";
        
                    point20.Name = "MaginciaInvasionLodor";
                    point21.Name = "MaginciaInvasionLodor";
                    point22.Name = "MaginciaInvasionLodor";
                    point23.Name = "MaginciaInvasionLodor";
                    point24.Name = "MaginciaInvasionLodor";
                    point25.Name = "MaginciaInvasionLodor";
                    point26.Name = "MaginciaInvasionLodor";
                    point27.Name = "MaginciaInvasionLodor";
                    point28.Name = "MaginciaInvasionLodor";
                    point29.Name = "MaginciaInvasionLodor";
        
                    point30.Name = "MaginciaInvasionLodor";
                    point31.Name = "MaginciaInvasionLodor";
                    point32.Name = "MaginciaInvasionLodor";
                    point33.Name = "MaginciaInvasionLodor";
                    point34.Name = "MaginciaInvasionLodor";
                    point35.Name = "MaginciaInvasionLodor";
                    point36.Name = "MaginciaInvasionLodor";
                    point37.Name = "MaginciaInvasionLodor";
                    point38.Name = "MaginciaInvasionLodor";
                    point39.Name = "MaginciaInvasionLodor";
        
                    point40.Name = "MaginciaInvasionLodor";
                    point41.Name = "MaginciaInvasionLodor";
                    point42.Name = "MaginciaInvasionLodor";
                    point43.Name = "MaginciaInvasionLodor";
                    point44.Name = "MaginciaInvasionLodor";
                    point45.Name = "MaginciaInvasionLodor";
                    point46.Name = "MaginciaInvasionLodor";
                    point47.Name = "MaginciaInvasionLodor";
                    point48.Name = "MaginciaInvasionLodor";
                    point49.Name = "MaginciaInvasionLodor";
        
                    point50.Name = "MaginciaInvasionLodor";
                    point51.Name = "MaginciaInvasionLodor";
                    point52.Name = "MaginciaInvasionLodor";
                    point53.Name = "MaginciaInvasionLodor";
                    point54.Name = "MaginciaInvasionLodor";
                    point55.Name = "MaginciaInvasionLodor";
                    point56.Name = "MaginciaInvasionLodor";
                    point57.Name = "MaginciaInvasionLodor";
                    point58.Name = "MaginciaInvasionLodor";
                    point59.Name = "MaginciaInvasionLodor";
        
                    point60.Name = "MaginciaInvasionLodor";
                    point61.Name = "MaginciaInvasionLodor";
                    point62.Name = "MaginciaInvasionLodor";
                    point63.Name = "MaginciaInvasionLodor";
                    point64.Name = "MaginciaInvasionLodor";
                    point65.Name = "MaginciaInvasionLodor";
                    point66.Name = "MaginciaInvasionLodor";
                    point67.Name = "MaginciaInvasionLodor";
                    point68.Name = "MaginciaInvasionLodor";
                    point69.Name = "MaginciaInvasionLodor";
        
                    point70.Name = "MaginciaInvasionLodor";
                    point71.Name = "MaginciaInvasionLodor";
                    point72.Name = "MaginciaInvasionLodor";
                    point73.Name = "MaginciaInvasionLodor";
                    point74.Name = "MaginciaInvasionLodor";
                    point75.Name = "MaginciaInvasionLodor";
                    point76.Name = "MaginciaInvasionLodor";
                    point77.Name = "MaginciaInvasionLodor";
                    point78.Name = "MaginciaInvasionLodor";
                    point79.Name = "MaginciaInvasionLodor";
                    point80.Name = "MaginciaInvasionLodor";
                    point81.Name = "MaginciaInvasionLodor";
        
                    point82.Name = "MaginciaInvasionLodor";
                    point83.Name = "MaginciaInvasionLodor";
                    point84.Name = "MaginciaInvasionLodor";
                    point85.Name = "MaginciaInvasionLodor";
                    point86.Name = "MaginciaInvasionLodor";
                    point87.Name = "MaginciaInvasionLodor";
                    point88.Name = "MaginciaInvasionLodor";
                    point89.Name = "MaginciaInvasionLodor";
                    point90.Name = "MaginciaInvasionLodor";
                    point91.Name = "MaginciaInvasionLodor";
                    point92.Name = "MaginciaInvasionLodor";
                    point93.Name = "MaginciaInvasionLodor";
        
                    point94.Name = "MaginciaInvasionLodor";
                    point95.Name = "MaginciaInvasionLodor";
                    point96.Name = "MaginciaInvasionLodor";
                    point97.Name = "MaginciaInvasionLodor";
                    point98.Name = "MaginciaInvasionLodor";
                    point99.Name = "MaginciaInvasionLodor";
                    point100.Name = "MaginciaInvasionLodor";
                    point101.Name = "MaginciaInvasionLodor";
                    point102.Name = "MaginciaInvasionLodor";
                    point103.Name = "MaginciaInvasionLodor";
                    point104.Name = "MaginciaInvasionLodor";
                    point105.Name = "MaginciaInvasionLodor";
        
                    point106.Name = "MaginciaInvasionLodor";
                    point107.Name = "MaginciaInvasionLodor";
                    point108.Name = "MaginciaInvasionLodor";
                    point109.Name = "MaginciaInvasionLodor";
                    point110.Name = "MaginciaInvasionLodor";
                    point111.Name = "MaginciaInvasionLodor";
                    point112.Name = "MaginciaInvasionLodor";
                    point113.Name = "MaginciaInvasionLodor";
                    point114.Name = "MaginciaInvasionLodor";
                    point115.Name = "MaginciaInvasionLodor";
                    point116.Name = "MaginciaInvasionLodor";
                    point117.Name = "MaginciaInvasionLodor";
                    point118.Name = "MaginciaInvasionLodor";
                    point119.Name = "MaginciaInvasionLodor";
                    point120.Name = "MaginciaInvasionLodor";*/

                    GuardedRegion reg = from.Region as GuardedRegion;

                    if (reg == null)
                    {
                        from.SendMessage(33, "You are not in the guarded part of Magincia, Lodor.");
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards off."
                        );
                    }
                    else if (reg.Disabled)
                    {
                        from.SendMessage(3, "The guards in this region have not changed.");
                    }
                    else if (!reg.Disabled)
                    {
                        reg.Disabled = !reg.Disabled;
                        from.SendMessage(3, "The guards in this region have been disabled.");
                    }
                    if (DummyMessage && reg != null)
                    {
                        from.SendMessage(
                            33,
                            "If you are not in the guarded part of Magincia, Lodor."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards off."
                        );
                    }
                    Spawner spawner1 = new Spawner(4, 5, 15, 0, 10, "OrcBomber");
                    spawner1.MoveToWorld(new Point3D(3654, 2070, 20), Map.Lodor);
                    spawner1.WayPoint = point;
                    point.MoveToWorld(new Point3D(3708, 2090, 5), Map.Lodor);
                    point.NextPoint = point1;
                    point1.MoveToWorld(new Point3D(3707, 2180, 20), Map.Lodor);
                    point1.NextPoint = point2;
                    point2.MoveToWorld(new Point3D(3675, 2180, 20), Map.Lodor);
                    point2.NextPoint = point3;
                    point3.MoveToWorld(new Point3D(3675, 2235, 20), Map.Lodor);
                    point3.NextPoint = point4;
                    point4.MoveToWorld(new Point3D(3741, 2235, 20), Map.Lodor);
                    point4.NextPoint = point5;
                    point5.MoveToWorld(new Point3D(3741, 2188, 20), Map.Lodor);
                    point5.NextPoint = point6;
                    point6.MoveToWorld(new Point3D(3707, 2188, 20), Map.Lodor);
                    point6.NextPoint = point7;
                    point7.MoveToWorld(new Point3D(3707, 2180, 20), Map.Lodor);
                    point7.NextPoint = point8;
                    point8.MoveToWorld(new Point3D(3675, 2180, 20), Map.Lodor);
                    point8.NextPoint = point9;
                    point9.MoveToWorld(new Point3D(3675, 2115, 20), Map.Lodor);
                    point9.NextPoint = point10;
                    point10.MoveToWorld(new Point3D(3754, 2115, 20), Map.Lodor);
                    point10.NextPoint = point11;
                    point11.MoveToWorld(new Point3D(3708, 2115, 20), Map.Lodor);
                    point11.NextPoint = point;
                    spawner1.Name = "MaginciaInvasionLodor";
                    spawner1.Respawn();

                    Spawner spawner2 = new Spawner(4, 5, 15, 0, 8, "OrcishLord");
                    spawner2.MoveToWorld(new Point3D(3797, 2263, 40), Map.Lodor);
                    spawner2.Name = "MaginciaInvasionLodor";
                    spawner2.Respawn();

                    Spawner spawner3 = new Spawner(4, 5, 15, 0, 8, "OrcCaptain");
                    spawner3.MoveToWorld(new Point3D(3796, 2262, 40), Map.Lodor);
                    spawner3.Name = "MaginciaInvasionLodor";
                    spawner3.Respawn();

                    Spawner spawner4 = new Spawner(4, 5, 15, 0, 8, "OrcSoldier");
                    spawner4.MoveToWorld(new Point3D(3797, 2262, 40), Map.Lodor);
                    spawner4.Name = "MaginciaInvasionLodor";
                    spawner4.Respawn();

                    Spawner spawner5 = new Spawner(4, 5, 15, 0, 8, "OrcishMage");
                    spawner5.MoveToWorld(new Point3D(3798, 2262, 40), Map.Lodor);
                    spawner5.Name = "MaginciaInvasionLodor";
                    spawner5.Respawn();

                    Spawner spawner6 = new Spawner(1, 2, 2, 0, 2, "OrcKing");
                    spawner6.MoveToWorld(new Point3D(3799, 2262, 40), Map.Lodor);
                    spawner6.Name = "MaginciaInvasionLodor";
                    spawner6.Respawn();

                    Spawner spawner7 = new Spawner(10, 5, 15, 0, 8, "Orc");
                    spawner7.MoveToWorld(new Point3D(3798, 2262, 40), Map.Lodor);
                    spawner7.Name = "MaginciaInvasionLodor";
                    spawner7.Respawn();

                    Spawner spawner8 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner8.MoveToWorld(new Point3D(3739, 2255, 20), Map.Lodor);
                    spawner8.Name = "MaginciaInvasionLodor";
                    spawner8.Respawn();

                    Spawner spawner41 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner41.MoveToWorld(new Point3D(3752, 2237, 20), Map.Lodor);
                    spawner41.Name = "MaginciaInvasionLodor";
                    spawner41.Respawn();

                    Spawner spawner9 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner9.MoveToWorld(new Point3D(3744, 2237, 20), Map.Lodor);
                    spawner9.Name = "MaginciaInvasionLodor";
                    spawner9.Respawn();

                    Spawner spawner10 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner10.MoveToWorld(new Point3D(3739, 2251, 20), Map.Lodor);
                    spawner10.Name = "MaginciaInvasionLodor";
                    spawner10.Respawn();

                    Spawner spawner11 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner11.MoveToWorld(new Point3D(3727, 2224, 20), Map.Lodor);
                    spawner11.Name = "MaginciaInvasionLodor";
                    spawner11.Respawn();

                    Spawner spawner12 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner12.MoveToWorld(new Point3D(3727, 2224, 20), Map.Lodor);
                    spawner12.Name = "MaginciaInvasionLodor";
                    spawner12.Respawn();

                    Spawner spawner13 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner13.MoveToWorld(new Point3D(3727, 2220, 20), Map.Lodor);
                    spawner13.Name = "MaginciaInvasionLodor";
                    spawner13.Respawn();

                    Spawner spawner14 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner14.MoveToWorld(new Point3D(3727, 2220, 20), Map.Lodor);
                    spawner14.Name = "MaginciaInvasionLodor";
                    spawner14.Respawn();

                    Spawner spawner15 = new Spawner(3, 5, 15, 0, 20, "OrcScout");
                    spawner15.MoveToWorld(new Point3D(3764, 2237, 30), Map.Lodor);
                    spawner15.Name = "MaginciaInvasionLodor";
                    spawner15.Respawn();

                    Spawner spawner16 = new Spawner(3, 5, 15, 0, 20, "OrcScout");
                    spawner16.MoveToWorld(new Point3D(3752, 2260, 30), Map.Lodor);
                    spawner16.Name = "MaginciaInvasionLodor";
                    spawner16.Respawn();

                    Spawner spawner17 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner17.MoveToWorld(new Point3D(3700, 2250, 20), Map.Lodor);
                    spawner17.Name = "MaginciaInvasionLodor";
                    spawner17.Respawn();

                    Spawner spawner18 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner18.MoveToWorld(new Point3D(3700, 2250, 20), Map.Lodor);
                    spawner18.Name = "MaginciaInvasionLodor";
                    spawner18.Respawn();

                    Spawner spawner19 = new Spawner(20, 5, 15, 0, 8, "Orc");
                    spawner19.MoveToWorld(new Point3D(3715, 2235, 20), Map.Lodor);
                    spawner19.Name = "MaginciaInvasionLodor";
                    spawner19.Respawn();

                    Spawner spawner20 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner20.MoveToWorld(new Point3D(3700, 2221, 41), Map.Lodor);
                    spawner20.Name = "MaginciaInvasionLodor";
                    spawner20.Respawn();

                    Spawner spawner21 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner21.MoveToWorld(new Point3D(3700, 2221, 41), Map.Lodor);
                    spawner21.Name = "MaginciaInvasionLodor";
                    spawner21.Respawn();

                    Spawner spawner22 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner22.MoveToWorld(new Point3D(3700, 2221, 20), Map.Lodor);
                    spawner22.Name = "MaginciaInvasionLodor";
                    spawner22.Respawn();

                    Spawner spawner23 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner23.MoveToWorld(new Point3D(3700, 2221, 20), Map.Lodor);
                    spawner23.Name = "MaginciaInvasionLodor";
                    spawner23.Respawn();

                    Spawner spawner24 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner24.MoveToWorld(new Point3D(3690, 2226, 20), Map.Lodor);
                    spawner24.Name = "MaginciaInvasionLodor";
                    spawner24.Respawn();

                    Spawner spawner25 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner25.MoveToWorld(new Point3D(3690, 2226, 20), Map.Lodor);
                    spawner25.Name = "MaginciaInvasionLodor";
                    spawner25.Respawn();

                    Spawner spawner26 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner26.MoveToWorld(new Point3D(3666, 2234, 20), Map.Lodor);
                    spawner26.Name = "MaginciaInvasionLodor";
                    spawner26.Respawn();

                    Spawner spawner27 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner27.MoveToWorld(new Point3D(3666, 2234, 20), Map.Lodor);
                    spawner27.Name = "MaginciaInvasionLodor";
                    spawner27.Respawn();

                    Spawner spawner28 = new Spawner(20, 5, 15, 0, 8, "Orc");
                    spawner28.MoveToWorld(new Point3D(3676, 2238, 20), Map.Lodor);
                    spawner28.Name = "MaginciaInvasionLodor";
                    spawner28.Respawn();

                    Spawner spawner29 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner29.MoveToWorld(new Point3D(3667, 2254, 20), Map.Lodor);
                    spawner29.Name = "MaginciaInvasionLodor";
                    spawner29.Respawn();

                    Spawner spawner30 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner30.MoveToWorld(new Point3D(3667, 2254, 20), Map.Lodor);
                    spawner30.Name = "MaginciaInvasionLodor";
                    spawner30.Respawn();

                    Spawner spawner31 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner31.MoveToWorld(new Point3D(3676, 2238, 20), Map.Lodor);
                    spawner31.Name = "MaginciaInvasionLodor";
                    spawner31.Respawn();

                    Spawner spawner32 = new Spawner(2, 5, 15, 0, 5, "OrcBomber");
                    spawner32.MoveToWorld(new Point3D(3715, 2235, 20), Map.Lodor);
                    spawner32.Name = "MaginciaInvasionLodor";
                    spawner32.Respawn();

                    Spawner spawner33 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner33.MoveToWorld(new Point3D(3684, 2252, 20), Map.Lodor);
                    spawner33.Name = "MaginciaInvasionLodor";
                    spawner33.Respawn();

                    Spawner spawner34 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner34.MoveToWorld(new Point3D(3684, 2252, 20), Map.Lodor);
                    spawner34.Name = "MaginciaInvasionLodor";
                    spawner34.Respawn();

                    Spawner spawner35 = new Spawner(20, 5, 15, 0, 8, "Orc");
                    spawner35.MoveToWorld(new Point3D(3674, 2286, -2), Map.Lodor);
                    spawner35.Name = "MaginciaInvasionLodor";
                    spawner35.Respawn();

                    Spawner spawner36 = new Spawner(3, 5, 15, 0, 20, "OrcScout");
                    spawner36.MoveToWorld(new Point3D(3674, 2286, -2), Map.Lodor);
                    spawner36.Name = "MaginciaInvasionLodor";
                    spawner36.Respawn();

                    Spawner spawner37 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner37.MoveToWorld(new Point3D(3660, 2188, 20), Map.Lodor);
                    spawner37.Name = "MaginciaInvasionLodor";
                    spawner37.Respawn();

                    Spawner spawner38 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner38.MoveToWorld(new Point3D(3660, 2188, 20), Map.Lodor);
                    spawner38.Name = "MaginciaInvasionLodor";
                    spawner38.Respawn();

                    Spawner spawner39 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner39.MoveToWorld(new Point3D(3694, 2163, 20), Map.Lodor);
                    spawner39.Name = "MaginciaInvasionLodor";
                    spawner39.Respawn();

                    Spawner spawner40 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner40.MoveToWorld(new Point3D(3694, 2163, 20), Map.Lodor);
                    spawner40.Name = "MaginciaInvasionLodor";
                    spawner40.Respawn();

                    Spawner spawner42 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner42.MoveToWorld(new Point3D(3731, 2149, 20), Map.Lodor);
                    spawner42.Name = "MaginciaInvasionLodor";
                    spawner42.Respawn();

                    Spawner spawner43 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner43.MoveToWorld(new Point3D(3731, 2149, 20), Map.Lodor);
                    spawner43.Name = "MaginciaInvasionLodor";
                    spawner43.Respawn();

                    Spawner spawner44 = new Spawner(20, 5, 15, 0, 8, "Orc");
                    spawner44.MoveToWorld(new Point3D(3710, 2162, 20), Map.Lodor);
                    spawner44.Name = "MaginciaInvasionLodor";
                    spawner44.Respawn();

                    Spawner spawner45 = new Spawner(1, 5, 15, 0, 1, "OrcCamp");
                    spawner45.MoveToWorld(new Point3D(3715, 2147, 20), Map.Lodor);
                    spawner45.Name = "MaginciaInvasionLodor";
                    spawner45.Respawn();

                    Spawner spawner46 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner46.MoveToWorld(new Point3D(3718, 2127, 20), Map.Lodor);
                    spawner46.Name = "MaginciaInvasionLodor";
                    spawner46.Respawn();

                    Spawner spawner47 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner47.MoveToWorld(new Point3D(3718, 2127, 20), Map.Lodor);
                    spawner47.Name = "MaginciaInvasionLodor";
                    spawner47.Respawn();

                    Spawner spawner48 = new Spawner(20, 5, 15, 0, 8, "Orc");
                    spawner48.MoveToWorld(new Point3D(3698, 2108, 20), Map.Lodor);
                    spawner48.Name = "MaginciaInvasionLodor";
                    spawner48.Respawn();

                    Spawner spawner49 = new Spawner(3, 5, 15, 0, 10, "OrcCamp");
                    spawner49.MoveToWorld(new Point3D(3667, 2107, 20), Map.Lodor);
                    spawner49.Name = "MaginciaInvasionLodor";
                    spawner49.Respawn();

                    /*Spawner spawner50 = new Spawner(2, 5, 15, 0, 8, "OrcishMage");
                    spawner50.MoveToWorld(new Point3D(3718, 2127, 20), Map.Lodor);
                    spawner50.Name = "MaginciaInvasionLodor";
                    spawner50.Respawn();
        
                    Spawner spawner51 = new Spawner(2, 5, 15, 0, 8, "OrcishLord");
                    spawner51.MoveToWorld(new Point3D(3718, 2127, 20), Map.Lodor);
                    spawner51.Name = "MaginciaInvasionLodor";
                    spawner51.Respawn();
        
                    /*Spawner spawner2 = new Spawner( 4, 5, 15, 0, 15, "Orc" );
                    spawner2.MoveToWorld( new Point3D(  2694, 466, 18  ), Map.Lodor );
                    spawner2.WayPoint = point12;
                    point12.MoveToWorld( new Point3D(  2662, 469, 15  ), Map.Lodor );
                    point12.NextPoint = point13;
                    point13.MoveToWorld( new Point3D(  2625, 469, 15  ), Map.Lodor );
                    point13.NextPoint = point14;
                    point14.MoveToWorld( new Point3D(  2613, 507, 15  ), Map.Lodor );
                    point14.NextPoint = point15;
                    point15.MoveToWorld( new Point3D(  2558, 513, 15  ), Map.Lodor );
                    point15.NextPoint = point16;
                    point16.MoveToWorld( new Point3D(  2558, 496, 0  ), Map.Lodor );
                    point16.NextPoint = point17;
                    point17.MoveToWorld( new Point3D(  2576, 479, 0  ), Map.Lodor );
                    point17.NextPoint = point18;
                    point18.MoveToWorld( new Point3D(  2558, 496, 0  ), Map.Lodor );
                    point18.NextPoint = point19;
                    point19.MoveToWorld( new Point3D(  2558, 528, 15  ), Map.Lodor );
                    point19.NextPoint = point20;
                    point20.MoveToWorld( new Point3D(  2569, 537, 15  ), Map.Lodor );
                    point20.NextPoint = point21;
                    point21.MoveToWorld( new Point3D(  2599, 531, 15  ), Map.Lodor );
                    point21.NextPoint = point22;
                    point22.MoveToWorld( new Point3D(  2599, 504, 0  ), Map.Lodor );
                    point22.NextPoint = point23;
                    point23.MoveToWorld( new Point3D(  2606, 502, 0  ), Map.Lodor );
                    point23.NextPoint = point24;
                    point24.MoveToWorld( new Point3D(  2604, 496, 20  ), Map.Lodor );
                    point24.NextPoint = point25;
                    point25.MoveToWorld( new Point3D(  2578, 500, 22  ), Map.Lodor );
                    point25.NextPoint = point26;
                    point26.MoveToWorld( new Point3D(  2582, 493, 40  ), Map.Lodor );
                    point26.NextPoint = point27;
                    point27.MoveToWorld( new Point3D(  2609, 469, 40  ), Map.Lodor );
                    point27.NextPoint = point28;
                    point28.MoveToWorld( new Point3D(  2602, 466, 60  ), Map.Lodor );
                    point28.NextPoint = point29;
                    point29.MoveToWorld( new Point3D(  2604, 453, 60  ), Map.Lodor );
                    point29.NextPoint = point30;
                    point30.MoveToWorld( new Point3D(  2591, 457, 60  ), Map.Lodor );
                    point30.NextPoint = point31;
                    point31.MoveToWorld( new Point3D(  2604, 453, 60  ), Map.Lodor );
                    point31.NextPoint = point32;
                    point32.MoveToWorld( new Point3D(  2602, 466, 60  ), Map.Lodor );
                    point32.NextPoint = point33;
                    point33.MoveToWorld( new Point3D(  2609, 469, 40  ), Map.Lodor );
                    point33.NextPoint = point34;
                    point34.MoveToWorld( new Point3D(  2582, 493, 40   ), Map.Lodor );
                    point34.NextPoint = point35;
                    point35.MoveToWorld( new Point3D(  2578, 500, 22  ), Map.Lodor );
                    point35.NextPoint = point36;
                    point36.MoveToWorld( new Point3D(  2604, 496, 20  ), Map.Lodor );
                    point36.NextPoint = point37;
                    point37.MoveToWorld( new Point3D(  2606, 502, 0  ), Map.Lodor );
                    point37.NextPoint = point14;
                    spawner2.Name = "MaginciaInvasionLodor";
                    spawner2.Respawn();
        
                    Spawner spawner3 = new Spawner( 6, 5, 15, 0, 10, "Orc" );
                    spawner3.MoveToWorld( new Point3D(  2555, 370, 15  ), Map.Lodor );
                    spawner3.WayPoint = point38;
                    point38.MoveToWorld( new Point3D(  2532, 389, 15  ), Map.Lodor );
                    point38.NextPoint = point39;
                    point39.MoveToWorld( new Point3D(  2510, 386, 15  ), Map.Lodor );
                    point39.NextPoint = point40;
                    point40.MoveToWorld( new Point3D(  2500, 419, 15  ), Map.Lodor );
                    point40.NextPoint = point41;
                    point41.MoveToWorld( new Point3D(  2445, 419, 15  ), Map.Lodor );
                    point41.NextPoint = point42;
                    point42.MoveToWorld( new Point3D(  2445, 447, 15  ), Map.Lodor );
                    point42.NextPoint = point43;
                    point43.MoveToWorld( new Point3D(  2501, 444, 15  ), Map.Lodor );
                    point43.NextPoint = point44;
                    point44.MoveToWorld( new Point3D(  2501, 485, 15  ), Map.Lodor );
                    point44.NextPoint = point45;
                    point45.MoveToWorld( new Point3D(  2469, 483, 15  ), Map.Lodor );
                    point45.NextPoint = point46;
                    point46.MoveToWorld( new Point3D(  2469, 461, 15  ), Map.Lodor );
                    point46.NextPoint = point47;
                    point47.MoveToWorld( new Point3D(  2476, 461, 15  ), Map.Lodor );
                    point47.NextPoint = point48;
                    point48.MoveToWorld( new Point3D(  2476, 435, 15  ), Map.Lodor );
                    point48.NextPoint = point49;
                    point49.MoveToWorld( new Point3D(  2467, 435, 15  ), Map.Lodor );
                    point49.NextPoint = point50;
                    point50.MoveToWorld( new Point3D(  2467, 418, 15  ), Map.Lodor );
                    point50.NextPoint = point51;
                    point51.MoveToWorld( new Point3D(  2500, 419, 15  ), Map.Lodor );
                    point51.NextPoint = point39;
                    spawner3.Name = "MaginciaInvasionLodor";
                    spawner3.Respawn();
        
                    Spawner spawner4 = new Spawner( 4, 5, 15, 0, 50, "Orc" );
                    spawner4.MoveToWorld( new Point3D(  2598, 747, 0  ), Map.Lodor );
                    spawner4.WayPoint = point52;
                    point52.MoveToWorld( new Point3D(  2579, 690, 0  ), Map.Lodor );
                    point52.NextPoint = point53;
                    point53.MoveToWorld( new Point3D(  2561, 623, 0  ), Map.Lodor );
                    point53.NextPoint = point54;
                    point54.MoveToWorld( new Point3D(  2513, 620, 0  ), Map.Lodor );
                    point54.NextPoint = point55;
                    point55.MoveToWorld( new Point3D(  2517, 562, 0  ), Map.Lodor );
                    point55.NextPoint = point56;
                    point56.MoveToWorld( new Point3D(  2486, 564, 5  ), Map.Lodor );
                    point56.NextPoint = point57;
                    point57.MoveToWorld( new Point3D(  2486, 544, 0  ), Map.Lodor );
                    point57.NextPoint = point58;
                    point58.MoveToWorld( new Point3D(  2465, 543, 0  ), Map.Lodor );
                    point58.NextPoint = point59;
                    point59.MoveToWorld( new Point3D(  2465, 528, 15  ), Map.Lodor );
                    point59.NextPoint = point60;
                    point60.MoveToWorld( new Point3D(  2455, 528, 15  ), Map.Lodor );
                    point60.NextPoint = point61;
                    point61.MoveToWorld( new Point3D(  2455, 513, 15  ), Map.Lodor );
                    point61.NextPoint = point62;
                    point62.MoveToWorld( new Point3D(  2475, 513, 15  ), Map.Lodor );
                    point62.NextPoint = point63;
                    point63.MoveToWorld( new Point3D(  2475, 528, 15  ), Map.Lodor );
                    point63.NextPoint = point60;
                    spawner4.Name = "MaginciaInvasionLodor";
                    spawner4.Respawn();
        
                    Spawner spawner5 = new Spawner( 6, 5, 15, 0, 4, "Orc" );
                    spawner5.MoveToWorld( new Point3D(  2579, 376, 5  ), Map.Lodor );
                    spawner5.WayPoint = point65;
                    point64.MoveToWorld( new Point3D(  2579, 398, 15  ), Map.Lodor );
                    point64.NextPoint = point65;
                    point65.MoveToWorld( new Point3D(  2623, 437, 15  ), Map.Lodor );
                    point65.NextPoint = point66;
                    point66.MoveToWorld( new Point3D(  2617, 506, 15  ), Map.Lodor );
                    point66.NextPoint = point67;
                    point67.MoveToWorld( new Point3D(  2562, 513, 15  ), Map.Lodor );
                    point67.NextPoint = point68;
                    point68.MoveToWorld( new Point3D(  2551, 501, 15  ), Map.Lodor );
                    point68.NextPoint = point69;
                    point69.MoveToWorld( new Point3D(  2525, 501, 15  ), Map.Lodor );
                    point69.NextPoint = point70;
                    point70.MoveToWorld( new Point3D(  2525, 516, 0  ), Map.Lodor );
                    point70.NextPoint = point71;
                    point71.MoveToWorld( new Point3D(  2489, 516, 0  ), Map.Lodor );
                    point71.NextPoint = point72;
                    point72.MoveToWorld( new Point3D(  2489, 482, 15  ), Map.Lodor );
                    point72.NextPoint = point73;
                    point73.MoveToWorld( new Point3D(  2500, 484, 15  ), Map.Lodor );
                    point73.NextPoint = point74;
                    point74.MoveToWorld( new Point3D(  2500, 442, 15  ), Map.Lodor );
                    point74.NextPoint = point75;
                    point75.MoveToWorld( new Point3D(  2514, 442, 15  ), Map.Lodor );
                    point75.NextPoint = point76;
                    point76.MoveToWorld( new Point3D(  2514, 419, 15  ), Map.Lodor );
                    point76.NextPoint = point77;
                    point77.MoveToWorld( new Point3D(  2445, 419, 15  ), Map.Lodor );
                    point77.NextPoint = point78;
                    point78.MoveToWorld( new Point3D(  2444, 444, 15  ), Map.Lodor );
                    point78.NextPoint = point79;
                    point79.MoveToWorld( new Point3D(  2531, 444, 15  ), Map.Lodor );
                    point79.NextPoint = point69;
                    spawner5.Name = "MaginciaInvasionLodor";
                    spawner5.Respawn();
        
                    Spawner spawner6 = new Spawner( 1, 5, 15, 0, 0, "OrcishLord" );
                    spawner6.MoveToWorld( new Point3D(  2420, 420, 15  ), Map.Lodor );
                    spawner6.WayPoint = point80;
                    point80.MoveToWorld( new Point3D(  2489, 419, 15  ), Map.Lodor );
                    point80.NextPoint = point81;
                    point81.MoveToWorld( new Point3D(  2491, 442, 15  ), Map.Lodor );
                    point81.NextPoint = point82;
                    point82.MoveToWorld( new Point3D(  2476, 442, 15  ), Map.Lodor );
                    point82.NextPoint = point83;
                    point83.MoveToWorld( new Point3D(  2475, 460, 15  ), Map.Lodor );
                    point83.NextPoint = point84;
                    point84.MoveToWorld( new Point3D(  2467, 460, 15  ), Map.Lodor );
                    point84.NextPoint = point85;
                    point85.MoveToWorld( new Point3D(  2469, 481, 15  ), Map.Lodor );
                    point85.NextPoint = point86;
                    point86.MoveToWorld( new Point3D(  2491, 481, 15  ), Map.Lodor );
                    point86.NextPoint = point87;
                    point87.MoveToWorld( new Point3D(  2488, 564, 5  ), Map.Lodor );
                    point87.NextPoint = point88;
                    point88.MoveToWorld( new Point3D(  2514, 561, 0  ), Map.Lodor );
                    point88.NextPoint = point89;
                    point89.MoveToWorld( new Point3D(  2516, 529, 0  ), Map.Lodor );
                    point89.NextPoint = point90;
                    point90.MoveToWorld( new Point3D(  2489, 529, 0  ), Map.Lodor );
                    point90.NextPoint = point91;
                    point91.MoveToWorld( new Point3D(  2489, 493, 15  ), Map.Lodor );
                    point91.NextPoint = point92;
                    point92.MoveToWorld( new Point3D(  2504, 482, 15  ), Map.Lodor );
                    point92.NextPoint = point80;
                    spawner6.Name = "MaginciaInvasionLodor";
                    spawner6.Respawn();
        
                    Spawner spawner7 = new Spawner( 1, 5, 15, 0, 0, "OrcCaptain" );
                    spawner7.MoveToWorld( new Point3D(  1351, 1757, 17  ), Map.Lodor );
                    spawner7.WayPoint = point93;
                    point93.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point93.NextPoint = point94;
                    point94.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point94.NextPoint = point95;
                    point95.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point95.NextPoint = point96;
                    point96.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point96.NextPoint = point97;
                    point97.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point97.NextPoint = point98;
                    point98.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point98.NextPoint = point99;
                    point99.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point99.NextPoint = point100;
                    point100.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point100.NextPoint = point101;
                    point101.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point101.NextPoint = point102;
                    point102.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point102.NextPoint = point103;
                    spawner7.Name = "MaginciaInvasionLodor";
                    spawner7.Respawn();
        
                    Spawner spawner8 = new Spawner( 1, 10, 20, 0, 10, "OrcBrute" );
                    spawner8.MoveToWorld( new Point3D(  1370, 1749, 3  ), Map.Lodor );
                    spawner8.WayPoint = point103;
                    point103.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point103.NextPoint = point104;
                    point104.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point104.NextPoint = point105;
                    point105.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point105.NextPoint = point106;
                    point106.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point106.NextPoint = point107;
                    point107.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point107.NextPoint = point108;
                    point108.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point108.NextPoint = point109;
                    point109.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point109.NextPoint = point110;
                    point110.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point110.NextPoint = point111;
                    point111.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point111.NextPoint = point112;
                    point112.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point112.NextPoint = point113;
                    point113.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point113.NextPoint = point114;
                    point114.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point114.NextPoint = point115;
                    point115.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point125.NextPoint = point116;
                    point116.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point116.NextPoint = point117;
                    point117.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point117.NextPoint = point118;
                    point118.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point118.NextPoint = point119;
                    point119.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point119.NextPoint = point120;
                    point120.MoveToWorld( new Point3D(  2491, 419, 15  ), Map.Lodor );
                    point120.NextPoint = point103;
                    spawner8.Name = "MaginciaInvasionLodor";
                    spawner8.Respawn();*/

                    World.Broadcast(33, true, "Magincia Lodor is under invasion.");
                    from.SendGump(new CityInvasion(from));
                    break;
                }
                case 2:
                {
                    GuardedRegion reg = from.Region as GuardedRegion;

                    if (reg == null)
                    {
                        from.SendMessage(
                            33,
                            "You are not in a The guarded part of Magincia, Lodor."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards on."
                        );
                    }
                    else if (!reg.Disabled)
                    {
                        from.SendMessage(3, "The guards in THIS region have not changed.");
                    }
                    else if (reg.Disabled)
                    {
                        reg.Disabled = !reg.Disabled;
                        from.SendMessage(3, "The guards in THIS region have been enabled.");
                    }
                    if (DummyMessage && reg != null)
                    {
                        from.SendMessage(
                            33,
                            "If you are not in a The guarded part of Magincia, Lodor."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards on."
                        );
                    }
                    MaginciaInvasionStone maginciafel = new MaginciaInvasionStone();
                    maginciafel.StopMaginciaLodor();
                    World.Broadcast(
                        33,
                        true,
                        "Magincia Lodor's invasion was successfully beaten back. No more invaders are left in the city."
                    );
                    from.SendGump(new CityInvasion(from));
                    break;
                }
            }
        }
    }
}
