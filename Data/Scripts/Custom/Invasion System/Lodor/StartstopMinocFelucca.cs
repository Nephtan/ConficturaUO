using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
    public class StartStopMinocfel : Gump
    {
        public static readonly bool DummyMessage = false;

        private Mobile m_Mobile;

        public StartStopMinocfel(Mobile from)
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
                    from.CloseGump(typeof(StartStopMinocfel));
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
                    WayPoint point120 = new WayPoint();

                    point.Name = "MinocInvasionLodor";
                    point1.Name = "MinocInvasionLodor";
                    point2.Name = "MinocInvasionLodor";
                    point3.Name = "MinocInvasionLodor";
                    point4.Name = "MinocInvasionLodor";
                    point5.Name = "MinocInvasionLodor";
                    point6.Name = "MinocInvasionLodor";
                    point7.Name = "MinocInvasionLodor";
                    point8.Name = "MinocInvasionLodor";
                    point9.Name = "MinocInvasionLodor";

                    point10.Name = "MinocInvasionLodor";
                    point11.Name = "MinocInvasionLodor";
                    point12.Name = "MinocInvasionLodor";
                    point13.Name = "MinocInvasionLodor";
                    point14.Name = "MinocInvasionLodor";
                    point15.Name = "MinocInvasionLodor";
                    point16.Name = "MinocInvasionLodor";
                    point17.Name = "MinocInvasionLodor";
                    point18.Name = "MinocInvasionLodor";
                    point19.Name = "MinocInvasionLodor";

                    point20.Name = "MinocInvasionLodor";
                    point21.Name = "MinocInvasionLodor";
                    point22.Name = "MinocInvasionLodor";
                    point23.Name = "MinocInvasionLodor";
                    point24.Name = "MinocInvasionLodor";
                    point25.Name = "MinocInvasionLodor";
                    point26.Name = "MinocInvasionLodor";
                    point27.Name = "MinocInvasionLodor";
                    point28.Name = "MinocInvasionLodor";
                    point29.Name = "MinocInvasionLodor";

                    point30.Name = "MinocInvasionLodor";
                    point31.Name = "MinocInvasionLodor";
                    point32.Name = "MinocInvasionLodor";
                    point33.Name = "MinocInvasionLodor";
                    point34.Name = "MinocInvasionLodor";
                    point35.Name = "MinocInvasionLodor";
                    point36.Name = "MinocInvasionLodor";
                    point37.Name = "MinocInvasionLodor";
                    point38.Name = "MinocInvasionLodor";
                    point39.Name = "MinocInvasionLodor";

                    point40.Name = "MinocInvasionLodor";
                    point41.Name = "MinocInvasionLodor";
                    point42.Name = "MinocInvasionLodor";
                    point43.Name = "MinocInvasionLodor";
                    point44.Name = "MinocInvasionLodor";
                    point45.Name = "MinocInvasionLodor";
                    point46.Name = "MinocInvasionLodor";
                    point47.Name = "MinocInvasionLodor";
                    point48.Name = "MinocInvasionLodor";
                    point49.Name = "MinocInvasionLodor";

                    point50.Name = "MinocInvasionLodor";
                    point51.Name = "MinocInvasionLodor";
                    point52.Name = "MinocInvasionLodor";
                    point53.Name = "MinocInvasionLodor";
                    point54.Name = "MinocInvasionLodor";
                    point55.Name = "MinocInvasionLodor";
                    point56.Name = "MinocInvasionLodor";
                    point57.Name = "MinocInvasionLodor";
                    point58.Name = "MinocInvasionLodor";
                    point59.Name = "MinocInvasionLodor";

                    point60.Name = "MinocInvasionLodor";
                    point61.Name = "MinocInvasionLodor";
                    point62.Name = "MinocInvasionLodor";
                    point63.Name = "MinocInvasionLodor";
                    point64.Name = "MinocInvasionLodor";
                    point65.Name = "MinocInvasionLodor";
                    point66.Name = "MinocInvasionLodor";
                    point67.Name = "MinocInvasionLodor";
                    point68.Name = "MinocInvasionLodor";
                    point69.Name = "MinocInvasionLodor";

                    point70.Name = "MinocInvasionLodor";
                    point71.Name = "MinocInvasionLodor";
                    point72.Name = "MinocInvasionLodor";
                    point73.Name = "MinocInvasionLodor";
                    point74.Name = "MinocInvasionLodor";
                    point75.Name = "MinocInvasionLodor";
                    point76.Name = "MinocInvasionLodor";
                    point77.Name = "MinocInvasionLodor";
                    point78.Name = "MinocInvasionLodor";
                    point79.Name = "MinocInvasionLodor";
                    point80.Name = "MinocInvasionLodor";
                    point81.Name = "MinocInvasionLodor";

                    point82.Name = "MinocInvasionLodor";
                    point83.Name = "MinocInvasionLodor";
                    point84.Name = "MinocInvasionLodor";
                    point85.Name = "MinocInvasionLodor";
                    point86.Name = "MinocInvasionLodor";
                    point87.Name = "MinocInvasionLodor";
                    point88.Name = "MinocInvasionLodor";
                    point89.Name = "MinocInvasionLodor";
                    point90.Name = "MinocInvasionLodor";
                    point91.Name = "MinocInvasionLodor";
                    point92.Name = "MinocInvasionLodor";
                    point93.Name = "MinocInvasionLodor";

                    point94.Name = "MinocInvasionLodor";
                    point95.Name = "MinocInvasionLodor";
                    point96.Name = "MinocInvasionLodor";
                    point97.Name = "MinocInvasionLodor";
                    point98.Name = "MinocInvasionLodor";
                    point99.Name = "MinocInvasionLodor";
                    point100.Name = "MinocInvasionLodor";
                    point101.Name = "MinocInvasionLodor";
                    point102.Name = "MinocInvasionLodor";
                    point103.Name = "MinocInvasionLodor";
                    point104.Name = "MinocInvasionLodor";
                    point105.Name = "MinocInvasionLodor";

                    point106.Name = "MinocInvasionLodor";
                    point107.Name = "MinocInvasionLodor";
                    point108.Name = "MinocInvasionLodor";
                    point109.Name = "MinocInvasionLodor";
                    point110.Name = "MinocInvasionLodor";
                    point111.Name = "MinocInvasionLodor";
                    point112.Name = "MinocInvasionLodor";
                    point113.Name = "MinocInvasionLodor";
                    point114.Name = "MinocInvasionLodor";
                    point115.Name = "MinocInvasionLodor";
                    point116.Name = "MinocInvasionLodor";
                    point117.Name = "MinocInvasionLodor";
                    point118.Name = "MinocInvasionLodor";
                    point119.Name = "MinocInvasionLodor";
                    point120.Name = "MinocInvasionLodor";

                    GuardedRegion reg = from.Region as GuardedRegion;

                    if (reg == null)
                    {
                        from.SendMessage(33, "You are not in the guarded part of Minoc, Lodor.");
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
                    Spawner spawner1 = new Spawner(4, 5, 15, 20, 10, "JukaLord");
                    spawner1.MoveToWorld(new Point3D(2575, 647, 0), Map.Lodor);
                    spawner1.WayPoint = point;
                    point.MoveToWorld(new Point3D(2518, 581, 0), Map.Lodor);
                    point.NextPoint = point1;
                    point1.MoveToWorld(new Point3D(2518, 530, 0), Map.Lodor);
                    point1.NextPoint = point2;
                    point2.MoveToWorld(new Point3D(2510, 530, 0), Map.Lodor);
                    point2.NextPoint = point3;
                    point3.MoveToWorld(new Point3D(2510, 518, 0), Map.Lodor);
                    point3.NextPoint = point4;
                    point4.MoveToWorld(new Point3D(2490, 518, 0), Map.Lodor);
                    point4.NextPoint = point5;
                    point5.MoveToWorld(new Point3D(2489, 545, 0), Map.Lodor);
                    point5.NextPoint = point6;
                    point6.MoveToWorld(new Point3D(2433, 545, 0), Map.Lodor);
                    point6.NextPoint = point7;
                    point7.MoveToWorld(new Point3D(2489, 545, 0), Map.Lodor);
                    point7.NextPoint = point8;
                    point8.MoveToWorld(new Point3D(2489, 482, 15), Map.Lodor);
                    point8.NextPoint = point9;
                    point9.MoveToWorld(new Point3D(2490, 518, 0), Map.Lodor);
                    point9.NextPoint = point10;
                    point10.MoveToWorld(new Point3D(2505, 518, 0), Map.Lodor);
                    point10.NextPoint = point11;
                    point11.MoveToWorld(new Point3D(2518, 530, 0), Map.Lodor);
                    point11.NextPoint = point;
                    spawner1.Name = "MinocInvasionLodor";
                    spawner1.Respawn();

                    Spawner spawner9 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner9.MoveToWorld(new Point3D(2575, 647, 0), Map.Lodor);
                    spawner9.Name = "MinocInvasionLodor";
                    spawner9.Respawn();

                    Spawner spawner2 = new Spawner(4, 5, 15, 20, 15, "JukaWarrior");
                    spawner2.MoveToWorld(new Point3D(2694, 466, 18), Map.Lodor);
                    spawner2.WayPoint = point12;
                    point12.MoveToWorld(new Point3D(2662, 469, 15), Map.Lodor);
                    point12.NextPoint = point13;
                    point13.MoveToWorld(new Point3D(2625, 469, 15), Map.Lodor);
                    point13.NextPoint = point14;
                    point14.MoveToWorld(new Point3D(2613, 507, 15), Map.Lodor);
                    point14.NextPoint = point15;
                    point15.MoveToWorld(new Point3D(2558, 513, 15), Map.Lodor);
                    point15.NextPoint = point16;
                    point16.MoveToWorld(new Point3D(2558, 496, 0), Map.Lodor);
                    point16.NextPoint = point17;
                    point17.MoveToWorld(new Point3D(2576, 479, 0), Map.Lodor);
                    point17.NextPoint = point18;
                    point18.MoveToWorld(new Point3D(2558, 496, 0), Map.Lodor);
                    point18.NextPoint = point19;
                    point19.MoveToWorld(new Point3D(2558, 528, 15), Map.Lodor);
                    point19.NextPoint = point20;
                    point20.MoveToWorld(new Point3D(2569, 537, 15), Map.Lodor);
                    point20.NextPoint = point21;
                    point21.MoveToWorld(new Point3D(2599, 531, 15), Map.Lodor);
                    point21.NextPoint = point22;
                    point22.MoveToWorld(new Point3D(2599, 504, 0), Map.Lodor);
                    point22.NextPoint = point23;
                    point23.MoveToWorld(new Point3D(2606, 502, 0), Map.Lodor);
                    point23.NextPoint = point24;
                    point24.MoveToWorld(new Point3D(2604, 496, 20), Map.Lodor);
                    point24.NextPoint = point25;
                    point25.MoveToWorld(new Point3D(2578, 500, 22), Map.Lodor);
                    point25.NextPoint = point26;
                    point26.MoveToWorld(new Point3D(2582, 493, 40), Map.Lodor);
                    point26.NextPoint = point27;
                    point27.MoveToWorld(new Point3D(2609, 469, 40), Map.Lodor);
                    point27.NextPoint = point28;
                    point28.MoveToWorld(new Point3D(2602, 466, 60), Map.Lodor);
                    point28.NextPoint = point29;
                    point29.MoveToWorld(new Point3D(2604, 453, 60), Map.Lodor);
                    point29.NextPoint = point30;
                    point30.MoveToWorld(new Point3D(2591, 457, 60), Map.Lodor);
                    point30.NextPoint = point31;
                    point31.MoveToWorld(new Point3D(2604, 453, 60), Map.Lodor);
                    point31.NextPoint = point32;
                    point32.MoveToWorld(new Point3D(2602, 466, 60), Map.Lodor);
                    point32.NextPoint = point33;
                    point33.MoveToWorld(new Point3D(2609, 469, 40), Map.Lodor);
                    point33.NextPoint = point34;
                    point34.MoveToWorld(new Point3D(2582, 493, 40), Map.Lodor);
                    point34.NextPoint = point35;
                    point35.MoveToWorld(new Point3D(2578, 500, 22), Map.Lodor);
                    point35.NextPoint = point36;
                    point36.MoveToWorld(new Point3D(2604, 496, 20), Map.Lodor);
                    point36.NextPoint = point37;
                    point37.MoveToWorld(new Point3D(2606, 502, 0), Map.Lodor);
                    point37.NextPoint = point14;
                    spawner2.Name = "MinocInvasionLodor";
                    spawner2.Respawn();

                    Spawner spawner10 = new Spawner(6, 20, 20, 20, 20, "JukaLord");
                    spawner10.MoveToWorld(new Point3D(2694, 466, 18), Map.Lodor);
                    spawner10.Name = "MinocInvasionLodor";
                    spawner10.Respawn();

                    Spawner spawner3 = new Spawner(6, 5, 15, 20, 10, "JukaWarrior");
                    spawner3.MoveToWorld(new Point3D(2555, 370, 15), Map.Lodor);
                    spawner3.WayPoint = point38;
                    point38.MoveToWorld(new Point3D(2532, 389, 15), Map.Lodor);
                    point38.NextPoint = point39;
                    point39.MoveToWorld(new Point3D(2510, 386, 15), Map.Lodor);
                    point39.NextPoint = point40;
                    point40.MoveToWorld(new Point3D(2500, 419, 15), Map.Lodor);
                    point40.NextPoint = point41;
                    point41.MoveToWorld(new Point3D(2445, 419, 15), Map.Lodor);
                    point41.NextPoint = point42;
                    point42.MoveToWorld(new Point3D(2445, 447, 15), Map.Lodor);
                    point42.NextPoint = point43;
                    point43.MoveToWorld(new Point3D(2501, 444, 15), Map.Lodor);
                    point43.NextPoint = point44;
                    point44.MoveToWorld(new Point3D(2501, 485, 15), Map.Lodor);
                    point44.NextPoint = point45;
                    point45.MoveToWorld(new Point3D(2469, 483, 15), Map.Lodor);
                    point45.NextPoint = point46;
                    point46.MoveToWorld(new Point3D(2469, 461, 15), Map.Lodor);
                    point46.NextPoint = point47;
                    point47.MoveToWorld(new Point3D(2476, 461, 15), Map.Lodor);
                    point47.NextPoint = point48;
                    point48.MoveToWorld(new Point3D(2476, 435, 15), Map.Lodor);
                    point48.NextPoint = point49;
                    point49.MoveToWorld(new Point3D(2467, 435, 15), Map.Lodor);
                    point49.NextPoint = point50;
                    point50.MoveToWorld(new Point3D(2467, 418, 15), Map.Lodor);
                    point50.NextPoint = point51;
                    point51.MoveToWorld(new Point3D(2500, 419, 15), Map.Lodor);
                    point51.NextPoint = point39;
                    spawner3.Name = "MinocInvasionLodor";
                    spawner3.Respawn();

                    Spawner spawner11 = new Spawner(8, 20, 20, 20, 20, "JukaMage");
                    spawner11.MoveToWorld(new Point3D(2555, 370, 15), Map.Lodor);
                    spawner11.Name = "MinocInvasionLodor";
                    spawner11.Respawn();

                    Spawner spawner4 = new Spawner(4, 5, 15, 20, 50, "JukaMage");
                    spawner4.MoveToWorld(new Point3D(2598, 747, 0), Map.Lodor);
                    spawner4.WayPoint = point52;
                    point52.MoveToWorld(new Point3D(2579, 690, 0), Map.Lodor);
                    point52.NextPoint = point53;
                    point53.MoveToWorld(new Point3D(2561, 623, 0), Map.Lodor);
                    point53.NextPoint = point54;
                    point54.MoveToWorld(new Point3D(2513, 620, 0), Map.Lodor);
                    point54.NextPoint = point55;
                    point55.MoveToWorld(new Point3D(2517, 562, 0), Map.Lodor);
                    point55.NextPoint = point56;
                    point56.MoveToWorld(new Point3D(2486, 564, 5), Map.Lodor);
                    point56.NextPoint = point57;
                    point57.MoveToWorld(new Point3D(2486, 544, 0), Map.Lodor);
                    point57.NextPoint = point58;
                    point58.MoveToWorld(new Point3D(2465, 543, 0), Map.Lodor);
                    point58.NextPoint = point59;
                    point59.MoveToWorld(new Point3D(2465, 528, 15), Map.Lodor);
                    point59.NextPoint = point60;
                    point60.MoveToWorld(new Point3D(2455, 528, 15), Map.Lodor);
                    point60.NextPoint = point61;
                    point61.MoveToWorld(new Point3D(2455, 513, 15), Map.Lodor);
                    point61.NextPoint = point62;
                    point62.MoveToWorld(new Point3D(2475, 513, 15), Map.Lodor);
                    point62.NextPoint = point63;
                    point63.MoveToWorld(new Point3D(2475, 528, 15), Map.Lodor);
                    point63.NextPoint = point60;
                    spawner4.Name = "MinocInvasionLodor";
                    spawner4.Respawn();

                    Spawner spawner12 = new Spawner(6, 20, 20, 20, 20, "JukaLord");
                    spawner12.MoveToWorld(new Point3D(2598, 747, 0), Map.Lodor);
                    spawner12.Name = "MinocInvasionLodor";
                    spawner12.Respawn();

                    Spawner spawner5 = new Spawner(6, 5, 15, 20, 4, "JukaWarrior");
                    spawner5.MoveToWorld(new Point3D(2579, 376, 5), Map.Lodor);
                    spawner5.WayPoint = point65;
                    point64.MoveToWorld(new Point3D(2579, 398, 15), Map.Lodor);
                    point64.NextPoint = point65;
                    point65.MoveToWorld(new Point3D(2623, 437, 15), Map.Lodor);
                    point65.NextPoint = point66;
                    point66.MoveToWorld(new Point3D(2617, 506, 15), Map.Lodor);
                    point66.NextPoint = point67;
                    point67.MoveToWorld(new Point3D(2562, 513, 15), Map.Lodor);
                    point67.NextPoint = point68;
                    point68.MoveToWorld(new Point3D(2551, 501, 15), Map.Lodor);
                    point68.NextPoint = point69;
                    point69.MoveToWorld(new Point3D(2525, 501, 15), Map.Lodor);
                    point69.NextPoint = point70;
                    point70.MoveToWorld(new Point3D(2525, 516, 0), Map.Lodor);
                    point70.NextPoint = point71;
                    point71.MoveToWorld(new Point3D(2489, 516, 0), Map.Lodor);
                    point71.NextPoint = point72;
                    point72.MoveToWorld(new Point3D(2489, 482, 15), Map.Lodor);
                    point72.NextPoint = point73;
                    point73.MoveToWorld(new Point3D(2500, 484, 15), Map.Lodor);
                    point73.NextPoint = point74;
                    point74.MoveToWorld(new Point3D(2500, 442, 15), Map.Lodor);
                    point74.NextPoint = point75;
                    point75.MoveToWorld(new Point3D(2514, 442, 15), Map.Lodor);
                    point75.NextPoint = point76;
                    point76.MoveToWorld(new Point3D(2514, 419, 15), Map.Lodor);
                    point76.NextPoint = point77;
                    point77.MoveToWorld(new Point3D(2445, 419, 15), Map.Lodor);
                    point77.NextPoint = point78;
                    point78.MoveToWorld(new Point3D(2444, 444, 15), Map.Lodor);
                    point78.NextPoint = point79;
                    point79.MoveToWorld(new Point3D(2531, 444, 15), Map.Lodor);
                    point79.NextPoint = point69;
                    spawner5.Name = "MinocInvasionLodor";
                    spawner5.Respawn();

                    Spawner spawner13 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner13.MoveToWorld(new Point3D(2579, 376, 5), Map.Lodor);
                    spawner13.Name = "MinocInvasionLodor";
                    spawner13.Respawn();

                    Spawner spawner6 = new Spawner(1, 5, 15, 20, 0, "JukaLord");
                    spawner6.MoveToWorld(new Point3D(2420, 420, 15), Map.Lodor);
                    spawner6.WayPoint = point80;
                    point80.MoveToWorld(new Point3D(2489, 419, 15), Map.Lodor);
                    point80.NextPoint = point81;
                    point81.MoveToWorld(new Point3D(2491, 442, 15), Map.Lodor);
                    point81.NextPoint = point82;
                    point82.MoveToWorld(new Point3D(2476, 442, 15), Map.Lodor);
                    point82.NextPoint = point83;
                    point83.MoveToWorld(new Point3D(2475, 460, 15), Map.Lodor);
                    point83.NextPoint = point84;
                    point84.MoveToWorld(new Point3D(2467, 460, 15), Map.Lodor);
                    point84.NextPoint = point85;
                    point85.MoveToWorld(new Point3D(2469, 481, 15), Map.Lodor);
                    point85.NextPoint = point86;
                    point86.MoveToWorld(new Point3D(2491, 481, 15), Map.Lodor);
                    point86.NextPoint = point87;
                    point87.MoveToWorld(new Point3D(2488, 564, 5), Map.Lodor);
                    point87.NextPoint = point88;
                    point88.MoveToWorld(new Point3D(2514, 561, 0), Map.Lodor);
                    point88.NextPoint = point89;
                    point89.MoveToWorld(new Point3D(2516, 529, 0), Map.Lodor);
                    point89.NextPoint = point90;
                    point90.MoveToWorld(new Point3D(2489, 529, 0), Map.Lodor);
                    point90.NextPoint = point91;
                    point91.MoveToWorld(new Point3D(2489, 493, 15), Map.Lodor);
                    point91.NextPoint = point92;
                    point92.MoveToWorld(new Point3D(2504, 482, 15), Map.Lodor);
                    point92.NextPoint = point80;
                    spawner6.Name = "MinocInvasionLodor";
                    spawner6.Respawn();

                    Spawner spawner14 = new Spawner(6, 20, 20, 20, 20, "Swampdragon");
                    spawner14.MoveToWorld(new Point3D(2420, 420, 15), Map.Lodor);
                    spawner14.Name = "MinocInvasionLodor";
                    spawner14.Respawn();

                    Spawner spawner7 = new Spawner(6, 5, 15, 20, 0, "JukaMage");
                    spawner7.MoveToWorld(new Point3D(2534, 624, 0), Map.Lodor);
                    spawner7.WayPoint = point93;
                    point93.MoveToWorld(new Point3D(2513, 620, 0), Map.Lodor);
                    point93.NextPoint = point94;
                    point94.MoveToWorld(new Point3D(2517, 562, 0), Map.Lodor);
                    point94.NextPoint = point95;
                    point95.MoveToWorld(new Point3D(2504, 562, 0), Map.Lodor);
                    point95.NextPoint = point96;
                    point96.MoveToWorld(new Point3D(2504, 547, 0), Map.Lodor);
                    point96.NextPoint = point97;
                    point97.MoveToWorld(new Point3D(2501, 547, 0), Map.Lodor);
                    point97.NextPoint = point98;
                    point98.MoveToWorld(new Point3D(2504, 547, 0), Map.Lodor);
                    point98.NextPoint = point99;
                    point99.MoveToWorld(new Point3D(2504, 562, 0), Map.Lodor);
                    point99.NextPoint = point100;
                    point100.MoveToWorld(new Point3D(2492, 562, 0), Map.Lodor);
                    point100.NextPoint = point101;
                    point101.MoveToWorld(new Point3D(2491, 540, 0), Map.Lodor);
                    point101.NextPoint = point102;
                    point102.MoveToWorld(new Point3D(2517, 539, 0), Map.Lodor);
                    point102.NextPoint = point94;
                    spawner7.Name = "MinocInvasionLodor";
                    spawner7.Respawn();

                    Spawner spawner15 = new Spawner(5, 20, 20, 20, 20, "JukaMage");
                    spawner15.MoveToWorld(new Point3D(2534, 624, 0), Map.Lodor);
                    spawner15.Name = "MinocInvasionLodor";
                    spawner15.Respawn();

                    Spawner spawner18 = new Spawner(5, 20, 20, 20, 20, "JukaLord");
                    spawner18.MoveToWorld(new Point3D(2534, 624, 0), Map.Lodor);
                    spawner18.Name = "MinocInvasionLodor";
                    spawner18.Respawn();

                    Spawner spawner8 = new Spawner(3, 10, 20, 20, 10, "JukaLord");
                    spawner8.MoveToWorld(new Point3D(2638, 652, 0), Map.Lodor);
                    spawner8.WayPoint = point103;
                    point103.MoveToWorld(new Point3D(2591, 647, 0), Map.Lodor);
                    point103.NextPoint = point104;
                    point104.MoveToWorld(new Point3D(2586, 641, 0), Map.Lodor);
                    point104.NextPoint = point105;
                    point105.MoveToWorld(new Point3D(2584, 622, 0), Map.Lodor);
                    point105.NextPoint = point106;
                    point106.MoveToWorld(new Point3D(2513, 620, 0), Map.Lodor);
                    point106.NextPoint = point107;
                    point107.MoveToWorld(new Point3D(2517, 562, 0), Map.Lodor);
                    point107.NextPoint = point108;
                    point108.MoveToWorld(new Point3D(2489, 563, 1), Map.Lodor);
                    point108.NextPoint = point109;
                    point109.MoveToWorld(new Point3D(2489, 535, 0), Map.Lodor);
                    point109.NextPoint = point110;
                    point110.MoveToWorld(new Point3D(2517, 535, 0), Map.Lodor);
                    point110.NextPoint = point111;
                    point111.MoveToWorld(new Point3D(2514, 561, 0), Map.Lodor);
                    point111.NextPoint = point112;
                    point112.MoveToWorld(new Point3D(2504, 562, 0), Map.Lodor);
                    point112.NextPoint = point113;
                    point113.MoveToWorld(new Point3D(2504, 555, 0), Map.Lodor);
                    point113.NextPoint = point114;
                    point114.MoveToWorld(new Point3D(2504, 562, 0), Map.Lodor);
                    point114.NextPoint = point115;
                    point115.MoveToWorld(new Point3D(2540, 562, 0), Map.Lodor);
                    point115.NextPoint = point116;
                    point116.MoveToWorld(new Point3D(2487, 570, 5), Map.Lodor);
                    point116.NextPoint = point117;
                    point117.MoveToWorld(new Point3D(2473, 570, 5), Map.Lodor);
                    point117.NextPoint = point118;
                    point118.MoveToWorld(new Point3D(2473, 565, 5), Map.Lodor);
                    point118.NextPoint = point119;
                    point119.MoveToWorld(new Point3D(2473, 570, 5), Map.Lodor);
                    point119.NextPoint = point120;
                    point120.MoveToWorld(new Point3D(2490, 570, 5), Map.Lodor);
                    point120.NextPoint = point108;
                    spawner8.Name = "MinocInvasionLodor";
                    spawner8.Respawn();

                    Spawner spawner16 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner16.MoveToWorld(new Point3D(2638, 652, 0), Map.Lodor);
                    spawner16.Name = "MinocInvasionLodor";
                    spawner16.Respawn();

                    Spawner spawner17 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner17.MoveToWorld(new Point3D(2476, 416, 15), Map.Lodor);
                    spawner17.Name = "MinocInvasionLodor";
                    spawner17.Respawn();

                    Spawner spawner19 = new Spawner(5, 20, 20, 20, 20, "JukaMage");
                    spawner19.MoveToWorld(new Point3D(2500, 463, 15), Map.Lodor);
                    spawner19.Name = "MinocInvasionLodor";
                    spawner19.Respawn();

                    Spawner spawner20 = new Spawner(3, 20, 20, 20, 20, "JukaLord");
                    spawner20.MoveToWorld(new Point3D(2500, 463, 15), Map.Lodor);
                    spawner20.Name = "MinocInvasionLodor";
                    spawner20.Respawn();

                    Spawner spawner21 = new Spawner(5, 20, 20, 20, 20, "JukaWarrior");
                    spawner21.MoveToWorld(new Point3D(2525, 479, 15), Map.Lodor);
                    spawner21.Name = "MinocInvasionLodor";
                    spawner21.Respawn();

                    Spawner spawner22 = new Spawner(2, 2, 2, 20, 2, "JukaLord");
                    spawner22.MoveToWorld(new Point3D(2539, 501, 30), Map.Lodor);
                    spawner22.Name = "MinocInvasionLodor";
                    spawner22.Respawn();

                    Spawner spawner23 = new Spawner(4, 20, 20, 20, 20, "JukaLord");
                    spawner23.MoveToWorld(new Point3D(2487, 518, 0), Map.Lodor);
                    spawner23.Name = "MinocInvasionLodor";
                    spawner23.Respawn();

                    Spawner spawner24 = new Spawner(6, 20, 20, 20, 20, "JukaWarrior");
                    spawner24.MoveToWorld(new Point3D(2526, 512, 7), Map.Lodor);
                    spawner24.Name = "MinocInvasionLodor";
                    spawner24.Respawn();

                    Spawner spawner25 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner25.MoveToWorld(new Point3D(2526, 512, 7), Map.Lodor);
                    spawner25.Name = "MinocInvasionLodor";
                    spawner25.Respawn();

                    Spawner spawner26 = new Spawner(4, 20, 20, 20, 20, "JukaLord");
                    spawner26.MoveToWorld(new Point3D(2508, 379, 0), Map.Lodor);
                    spawner26.Name = "MinocInvasionLodor";
                    spawner26.Respawn();

                    Spawner spawner27 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner27.MoveToWorld(new Point3D(2556, 342, 15), Map.Lodor);
                    spawner27.Name = "MinocInvasionLodor";
                    spawner27.Respawn();

                    Spawner spawner28 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner28.MoveToWorld(new Point3D(2527, 440, 15), Map.Lodor);
                    spawner28.Name = "MinocInvasionLodor";
                    spawner28.Respawn();

                    Spawner spawner29 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner29.MoveToWorld(new Point3D(2496, 572, 3), Map.Lodor);
                    spawner29.Name = "MinocInvasionLodor";
                    spawner29.Respawn();

                    Spawner spawner30 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner30.MoveToWorld(new Point3D(2496, 572, 3), Map.Lodor);
                    spawner30.Name = "MinocInvasionLodor";
                    spawner30.Respawn();

                    Spawner spawner31 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner31.MoveToWorld(new Point3D(2501, 619, 0), Map.Lodor);
                    spawner31.Name = "MinocInvasionLodor";
                    spawner31.Respawn();

                    Spawner spawner32 = new Spawner(7, 20, 20, 20, 20, "JukaWarrior");
                    spawner32.MoveToWorld(new Point3D(2519, 672, 0), Map.Lodor);
                    spawner32.Name = "MinocInvasionLodor";
                    spawner32.Respawn();

                    Spawner spawner33 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner33.MoveToWorld(new Point3D(2553, 706, 0), Map.Lodor);
                    spawner33.Name = "MinocInvasionLodor";
                    spawner33.Respawn();

                    Spawner spawner34 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner34.MoveToWorld(new Point3D(2553, 706, 0), Map.Lodor);
                    spawner34.Name = "MinocInvasionLodor";
                    spawner34.Respawn();

                    Spawner spawner35 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner35.MoveToWorld(new Point3D(2587, 685, 0), Map.Lodor);
                    spawner35.Name = "MinocInvasionLodor";
                    spawner35.Respawn();

                    Spawner spawner36 = new Spawner(5, 20, 20, 20, 20, "Swampdragon");
                    spawner36.MoveToWorld(new Point3D(2607, 669, 0), Map.Lodor);
                    spawner36.Name = "MinocInvasionLodor";
                    spawner36.Respawn();

                    Spawner spawner37 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner37.MoveToWorld(new Point3D(2453, 544, 0), Map.Lodor);
                    spawner37.Name = "MinocInvasionLodor";
                    spawner37.Respawn();

                    Spawner spawner38 = new Spawner(8, 20, 20, 20, 10, "JukaWarrior");
                    spawner38.MoveToWorld(new Point3D(2417, 506, 15), Map.Lodor);
                    spawner38.Name = "MinocInvasionLodor";
                    spawner38.Respawn();

                    Spawner spawner39 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner39.MoveToWorld(new Point3D(2426, 456, 15), Map.Lodor);
                    spawner39.Name = "MinocInvasionLodor";
                    spawner39.Respawn();

                    Spawner spawner40 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner40.MoveToWorld(new Point3D(2426, 456, 15), Map.Lodor);
                    spawner40.Name = "MinocInvasionLodor";
                    spawner40.Respawn();

                    Spawner spawner41 = new Spawner(7, 20, 20, 20, 20, "JukaWarrior");
                    spawner41.MoveToWorld(new Point3D(2446, 400, 15), Map.Lodor);
                    spawner41.Name = "MinocInvasionLodor";
                    spawner41.Respawn();

                    Spawner spawner42 = new Spawner(7, 20, 20, 20, 10, "JukaWarrior");
                    spawner42.MoveToWorld(new Point3D(2451, 469, 15), Map.Lodor);
                    spawner42.Name = "MinocInvasionLodor";
                    spawner42.Respawn();

                    Spawner spawner43 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner43.MoveToWorld(new Point3D(2511, 387, 15), Map.Lodor);
                    spawner43.Name = "MinocInvasionLodor";
                    spawner43.Respawn();

                    Spawner spawner44 = new Spawner(1, 2, 2, 20, 1, "TheSavageGeneral");
                    spawner44.MoveToWorld(new Point3D(2454, 477, 15), Map.Lodor);
                    spawner44.Name = "MinocInvasionLodor";
                    spawner44.Respawn();

                    Spawner spawner45 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner45.MoveToWorld(new Point3D(2449, 486, 15), Map.Lodor);
                    spawner45.Name = "MinocInvasionLodor";
                    spawner45.Respawn();

                    Spawner spawner46 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner46.MoveToWorld(new Point3D(2455, 493, 15), Map.Lodor);
                    spawner46.Name = "MinocInvasionLodor";
                    spawner46.Respawn();

                    Spawner spawner47 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner47.MoveToWorld(new Point3D(2458, 487, 15), Map.Lodor);
                    spawner47.Name = "MinocInvasionLodor";
                    spawner47.Respawn();

                    Spawner spawner48 = new Spawner(2, 2, 2, 20, 2, "JukaLord");
                    spawner48.MoveToWorld(new Point3D(2458, 487, 15), Map.Lodor);
                    spawner48.Name = "MinocInvasionLodor";
                    spawner48.Respawn();

                    World.Broadcast(33, true, "Minoc Lodor is under invasion.");
                    from.SendGump(new CityInvasion(from));
                    break;
                }
                case 2:
                {
                    GuardedRegion reg = from.Region as GuardedRegion;

                    if (reg == null)
                    {
                        from.SendMessage(33, "You are not in a The guarded part of Minoc, Lodor.");
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards on."
                        );
                    }
                    else if (DummyMessage)
                    {
                        from.SendMessage(
                            33,
                            "If you are not in a The guarded part of Minoc, Lodor."
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
                            "If you are not in the guarded part of Magincia, Lodor."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards off."
                        );
                    }
                    MinocInvasionStone minocfel = new MinocInvasionStone();
                    minocfel.StopMinocLodor();
                    World.Broadcast(
                        33,
                        true,
                        "Minoc Lodor's invasion was successfully beaten back. No more invaders are left in the city."
                    );
                    from.SendGump(new CityInvasion(from));
                    break;
                }
            }
        }
    }
}
