using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
    public class StartStopMinoctram : Gump
    {
        public static readonly bool DummyMessage = false;

        private Mobile m_Mobile;

        public StartStopMinoctram(Mobile from)
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
                    from.CloseGump(typeof(StartStopMinoctram));
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

                    point.Name = "MinocInvasionSosaria";
                    point1.Name = "MinocInvasionSosaria";
                    point2.Name = "MinocInvasionSosaria";
                    point3.Name = "MinocInvasionSosaria";
                    point4.Name = "MinocInvasionSosaria";
                    point5.Name = "MinocInvasionSosaria";
                    point6.Name = "MinocInvasionSosaria";
                    point7.Name = "MinocInvasionSosaria";
                    point8.Name = "MinocInvasionSosaria";
                    point9.Name = "MinocInvasionSosaria";

                    point10.Name = "MinocInvasionSosaria";
                    point11.Name = "MinocInvasionSosaria";
                    point12.Name = "MinocInvasionSosaria";
                    point13.Name = "MinocInvasionSosaria";
                    point14.Name = "MinocInvasionSosaria";
                    point15.Name = "MinocInvasionSosaria";
                    point16.Name = "MinocInvasionSosaria";
                    point17.Name = "MinocInvasionSosaria";
                    point18.Name = "MinocInvasionSosaria";
                    point19.Name = "MinocInvasionSosaria";

                    point20.Name = "MinocInvasionSosaria";
                    point21.Name = "MinocInvasionSosaria";
                    point22.Name = "MinocInvasionSosaria";
                    point23.Name = "MinocInvasionSosaria";
                    point24.Name = "MinocInvasionSosaria";
                    point25.Name = "MinocInvasionSosaria";
                    point26.Name = "MinocInvasionSosaria";
                    point27.Name = "MinocInvasionSosaria";
                    point28.Name = "MinocInvasionSosaria";
                    point29.Name = "MinocInvasionSosaria";

                    point30.Name = "MinocInvasionSosaria";
                    point31.Name = "MinocInvasionSosaria";
                    point32.Name = "MinocInvasionSosaria";
                    point33.Name = "MinocInvasionSosaria";
                    point34.Name = "MinocInvasionSosaria";
                    point35.Name = "MinocInvasionSosaria";
                    point36.Name = "MinocInvasionSosaria";
                    point37.Name = "MinocInvasionSosaria";
                    point38.Name = "MinocInvasionSosaria";
                    point39.Name = "MinocInvasionSosaria";

                    point40.Name = "MinocInvasionSosaria";
                    point41.Name = "MinocInvasionSosaria";
                    point42.Name = "MinocInvasionSosaria";
                    point43.Name = "MinocInvasionSosaria";
                    point44.Name = "MinocInvasionSosaria";
                    point45.Name = "MinocInvasionSosaria";
                    point46.Name = "MinocInvasionSosaria";
                    point47.Name = "MinocInvasionSosaria";
                    point48.Name = "MinocInvasionSosaria";
                    point49.Name = "MinocInvasionSosaria";

                    point50.Name = "MinocInvasionSosaria";
                    point51.Name = "MinocInvasionSosaria";
                    point52.Name = "MinocInvasionSosaria";
                    point53.Name = "MinocInvasionSosaria";
                    point54.Name = "MinocInvasionSosaria";
                    point55.Name = "MinocInvasionSosaria";
                    point56.Name = "MinocInvasionSosaria";
                    point57.Name = "MinocInvasionSosaria";
                    point58.Name = "MinocInvasionSosaria";
                    point59.Name = "MinocInvasionSosaria";

                    point60.Name = "MinocInvasionSosaria";
                    point61.Name = "MinocInvasionSosaria";
                    point62.Name = "MinocInvasionSosaria";
                    point63.Name = "MinocInvasionSosaria";
                    point64.Name = "MinocInvasionSosaria";
                    point65.Name = "MinocInvasionSosaria";
                    point66.Name = "MinocInvasionSosaria";
                    point67.Name = "MinocInvasionSosaria";
                    point68.Name = "MinocInvasionSosaria";
                    point69.Name = "MinocInvasionSosaria";

                    point70.Name = "MinocInvasionSosaria";
                    point71.Name = "MinocInvasionSosaria";
                    point72.Name = "MinocInvasionSosaria";
                    point73.Name = "MinocInvasionSosaria";
                    point74.Name = "MinocInvasionSosaria";
                    point75.Name = "MinocInvasionSosaria";
                    point76.Name = "MinocInvasionSosaria";
                    point77.Name = "MinocInvasionSosaria";
                    point78.Name = "MinocInvasionSosaria";
                    point79.Name = "MinocInvasionSosaria";
                    point80.Name = "MinocInvasionSosaria";
                    point81.Name = "MinocInvasionSosaria";

                    point82.Name = "MinocInvasionSosaria";
                    point83.Name = "MinocInvasionSosaria";
                    point84.Name = "MinocInvasionSosaria";
                    point85.Name = "MinocInvasionSosaria";
                    point86.Name = "MinocInvasionSosaria";
                    point87.Name = "MinocInvasionSosaria";
                    point88.Name = "MinocInvasionSosaria";
                    point89.Name = "MinocInvasionSosaria";
                    point90.Name = "MinocInvasionSosaria";
                    point91.Name = "MinocInvasionSosaria";
                    point92.Name = "MinocInvasionSosaria";
                    point93.Name = "MinocInvasionSosaria";

                    point94.Name = "MinocInvasionSosaria";
                    point95.Name = "MinocInvasionSosaria";
                    point96.Name = "MinocInvasionSosaria";
                    point97.Name = "MinocInvasionSosaria";
                    point98.Name = "MinocInvasionSosaria";
                    point99.Name = "MinocInvasionSosaria";
                    point100.Name = "MinocInvasionSosaria";
                    point101.Name = "MinocInvasionSosaria";
                    point102.Name = "MinocInvasionSosaria";
                    point103.Name = "MinocInvasionSosaria";
                    point104.Name = "MinocInvasionSosaria";
                    point105.Name = "MinocInvasionSosaria";

                    point106.Name = "MinocInvasionSosaria";
                    point107.Name = "MinocInvasionSosaria";
                    point108.Name = "MinocInvasionSosaria";
                    point109.Name = "MinocInvasionSosaria";
                    point110.Name = "MinocInvasionSosaria";
                    point111.Name = "MinocInvasionSosaria";
                    point112.Name = "MinocInvasionSosaria";
                    point113.Name = "MinocInvasionSosaria";
                    point114.Name = "MinocInvasionSosaria";
                    point115.Name = "MinocInvasionSosaria";
                    point116.Name = "MinocInvasionSosaria";
                    point117.Name = "MinocInvasionSosaria";
                    point118.Name = "MinocInvasionSosaria";
                    point119.Name = "MinocInvasionSosaria";
                    point120.Name = "MinocInvasionSosaria";

                    GuardedRegion reg = from.Region as GuardedRegion;

                    if (reg == null)
                    {
                        from.SendMessage(33, "You are not in the guarded part of Minoc, Sosaria.");
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
                            "If you are not in the guarded part of Magincia, Sosaria."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards off."
                        );
                    }
                    Spawner spawner1 = new Spawner(4, 5, 15, 20, 10, "JukaLord");
                    spawner1.MoveToWorld(new Point3D(2575, 647, 0), Map.Sosaria);
                    spawner1.WayPoint = point;
                    point.MoveToWorld(new Point3D(2518, 581, 0), Map.Sosaria);
                    point.NextPoint = point1;
                    point1.MoveToWorld(new Point3D(2518, 530, 0), Map.Sosaria);
                    point1.NextPoint = point2;
                    point2.MoveToWorld(new Point3D(2510, 530, 0), Map.Sosaria);
                    point2.NextPoint = point3;
                    point3.MoveToWorld(new Point3D(2510, 518, 0), Map.Sosaria);
                    point3.NextPoint = point4;
                    point4.MoveToWorld(new Point3D(2490, 518, 0), Map.Sosaria);
                    point4.NextPoint = point5;
                    point5.MoveToWorld(new Point3D(2489, 545, 0), Map.Sosaria);
                    point5.NextPoint = point6;
                    point6.MoveToWorld(new Point3D(2433, 545, 0), Map.Sosaria);
                    point6.NextPoint = point7;
                    point7.MoveToWorld(new Point3D(2489, 545, 0), Map.Sosaria);
                    point7.NextPoint = point8;
                    point8.MoveToWorld(new Point3D(2489, 482, 15), Map.Sosaria);
                    point8.NextPoint = point9;
                    point9.MoveToWorld(new Point3D(2490, 518, 0), Map.Sosaria);
                    point9.NextPoint = point10;
                    point10.MoveToWorld(new Point3D(2505, 518, 0), Map.Sosaria);
                    point10.NextPoint = point11;
                    point11.MoveToWorld(new Point3D(2518, 530, 0), Map.Sosaria);
                    point11.NextPoint = point;
                    spawner1.Name = "MinocInvasionSosaria";
                    spawner1.Respawn();

                    Spawner spawner9 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner9.MoveToWorld(new Point3D(2575, 647, 0), Map.Sosaria);
                    spawner9.Name = "MinocInvasionSosaria";
                    spawner9.Respawn();

                    Spawner spawner2 = new Spawner(4, 5, 15, 20, 15, "JukaWarrior");
                    spawner2.MoveToWorld(new Point3D(2694, 466, 18), Map.Sosaria);
                    spawner2.WayPoint = point12;
                    point12.MoveToWorld(new Point3D(2662, 469, 15), Map.Sosaria);
                    point12.NextPoint = point13;
                    point13.MoveToWorld(new Point3D(2625, 469, 15), Map.Sosaria);
                    point13.NextPoint = point14;
                    point14.MoveToWorld(new Point3D(2613, 507, 15), Map.Sosaria);
                    point14.NextPoint = point15;
                    point15.MoveToWorld(new Point3D(2558, 513, 15), Map.Sosaria);
                    point15.NextPoint = point16;
                    point16.MoveToWorld(new Point3D(2558, 496, 0), Map.Sosaria);
                    point16.NextPoint = point17;
                    point17.MoveToWorld(new Point3D(2576, 479, 0), Map.Sosaria);
                    point17.NextPoint = point18;
                    point18.MoveToWorld(new Point3D(2558, 496, 0), Map.Sosaria);
                    point18.NextPoint = point19;
                    point19.MoveToWorld(new Point3D(2558, 528, 15), Map.Sosaria);
                    point19.NextPoint = point20;
                    point20.MoveToWorld(new Point3D(2569, 537, 15), Map.Sosaria);
                    point20.NextPoint = point21;
                    point21.MoveToWorld(new Point3D(2599, 531, 15), Map.Sosaria);
                    point21.NextPoint = point22;
                    point22.MoveToWorld(new Point3D(2599, 504, 0), Map.Sosaria);
                    point22.NextPoint = point23;
                    point23.MoveToWorld(new Point3D(2606, 502, 0), Map.Sosaria);
                    point23.NextPoint = point24;
                    point24.MoveToWorld(new Point3D(2604, 496, 20), Map.Sosaria);
                    point24.NextPoint = point25;
                    point25.MoveToWorld(new Point3D(2578, 500, 22), Map.Sosaria);
                    point25.NextPoint = point26;
                    point26.MoveToWorld(new Point3D(2582, 493, 40), Map.Sosaria);
                    point26.NextPoint = point27;
                    point27.MoveToWorld(new Point3D(2609, 469, 40), Map.Sosaria);
                    point27.NextPoint = point28;
                    point28.MoveToWorld(new Point3D(2602, 466, 60), Map.Sosaria);
                    point28.NextPoint = point29;
                    point29.MoveToWorld(new Point3D(2604, 453, 60), Map.Sosaria);
                    point29.NextPoint = point30;
                    point30.MoveToWorld(new Point3D(2591, 457, 60), Map.Sosaria);
                    point30.NextPoint = point31;
                    point31.MoveToWorld(new Point3D(2604, 453, 60), Map.Sosaria);
                    point31.NextPoint = point32;
                    point32.MoveToWorld(new Point3D(2602, 466, 60), Map.Sosaria);
                    point32.NextPoint = point33;
                    point33.MoveToWorld(new Point3D(2609, 469, 40), Map.Sosaria);
                    point33.NextPoint = point34;
                    point34.MoveToWorld(new Point3D(2582, 493, 40), Map.Sosaria);
                    point34.NextPoint = point35;
                    point35.MoveToWorld(new Point3D(2578, 500, 22), Map.Sosaria);
                    point35.NextPoint = point36;
                    point36.MoveToWorld(new Point3D(2604, 496, 20), Map.Sosaria);
                    point36.NextPoint = point37;
                    point37.MoveToWorld(new Point3D(2606, 502, 0), Map.Sosaria);
                    point37.NextPoint = point14;
                    spawner2.Name = "MinocInvasionSosaria";
                    spawner2.Respawn();

                    Spawner spawner10 = new Spawner(6, 20, 20, 20, 20, "JukaLord");
                    spawner10.MoveToWorld(new Point3D(2694, 466, 18), Map.Sosaria);
                    spawner10.Name = "MinocInvasionSosaria";
                    spawner10.Respawn();

                    Spawner spawner3 = new Spawner(6, 5, 15, 20, 10, "JukaWarrior");
                    spawner3.MoveToWorld(new Point3D(2555, 370, 15), Map.Sosaria);
                    spawner3.WayPoint = point38;
                    point38.MoveToWorld(new Point3D(2532, 389, 15), Map.Sosaria);
                    point38.NextPoint = point39;
                    point39.MoveToWorld(new Point3D(2510, 386, 15), Map.Sosaria);
                    point39.NextPoint = point40;
                    point40.MoveToWorld(new Point3D(2500, 419, 15), Map.Sosaria);
                    point40.NextPoint = point41;
                    point41.MoveToWorld(new Point3D(2445, 419, 15), Map.Sosaria);
                    point41.NextPoint = point42;
                    point42.MoveToWorld(new Point3D(2445, 447, 15), Map.Sosaria);
                    point42.NextPoint = point43;
                    point43.MoveToWorld(new Point3D(2501, 444, 15), Map.Sosaria);
                    point43.NextPoint = point44;
                    point44.MoveToWorld(new Point3D(2501, 485, 15), Map.Sosaria);
                    point44.NextPoint = point45;
                    point45.MoveToWorld(new Point3D(2469, 483, 15), Map.Sosaria);
                    point45.NextPoint = point46;
                    point46.MoveToWorld(new Point3D(2469, 461, 15), Map.Sosaria);
                    point46.NextPoint = point47;
                    point47.MoveToWorld(new Point3D(2476, 461, 15), Map.Sosaria);
                    point47.NextPoint = point48;
                    point48.MoveToWorld(new Point3D(2476, 435, 15), Map.Sosaria);
                    point48.NextPoint = point49;
                    point49.MoveToWorld(new Point3D(2467, 435, 15), Map.Sosaria);
                    point49.NextPoint = point50;
                    point50.MoveToWorld(new Point3D(2467, 418, 15), Map.Sosaria);
                    point50.NextPoint = point51;
                    point51.MoveToWorld(new Point3D(2500, 419, 15), Map.Sosaria);
                    point51.NextPoint = point39;
                    spawner3.Name = "MinocInvasionSosaria";
                    spawner3.Respawn();

                    Spawner spawner11 = new Spawner(8, 20, 20, 20, 20, "JukaMage");
                    spawner11.MoveToWorld(new Point3D(2555, 370, 15), Map.Sosaria);
                    spawner11.Name = "MinocInvasionSosaria";
                    spawner11.Respawn();

                    Spawner spawner4 = new Spawner(4, 5, 15, 20, 50, "JukaMage");
                    spawner4.MoveToWorld(new Point3D(2598, 747, 0), Map.Sosaria);
                    spawner4.WayPoint = point52;
                    point52.MoveToWorld(new Point3D(2579, 690, 0), Map.Sosaria);
                    point52.NextPoint = point53;
                    point53.MoveToWorld(new Point3D(2561, 623, 0), Map.Sosaria);
                    point53.NextPoint = point54;
                    point54.MoveToWorld(new Point3D(2513, 620, 0), Map.Sosaria);
                    point54.NextPoint = point55;
                    point55.MoveToWorld(new Point3D(2517, 562, 0), Map.Sosaria);
                    point55.NextPoint = point56;
                    point56.MoveToWorld(new Point3D(2486, 564, 5), Map.Sosaria);
                    point56.NextPoint = point57;
                    point57.MoveToWorld(new Point3D(2486, 544, 0), Map.Sosaria);
                    point57.NextPoint = point58;
                    point58.MoveToWorld(new Point3D(2465, 543, 0), Map.Sosaria);
                    point58.NextPoint = point59;
                    point59.MoveToWorld(new Point3D(2465, 528, 15), Map.Sosaria);
                    point59.NextPoint = point60;
                    point60.MoveToWorld(new Point3D(2455, 528, 15), Map.Sosaria);
                    point60.NextPoint = point61;
                    point61.MoveToWorld(new Point3D(2455, 513, 15), Map.Sosaria);
                    point61.NextPoint = point62;
                    point62.MoveToWorld(new Point3D(2475, 513, 15), Map.Sosaria);
                    point62.NextPoint = point63;
                    point63.MoveToWorld(new Point3D(2475, 528, 15), Map.Sosaria);
                    point63.NextPoint = point60;
                    spawner4.Name = "MinocInvasionSosaria";
                    spawner4.Respawn();

                    Spawner spawner12 = new Spawner(6, 20, 20, 20, 20, "JukaLord");
                    spawner12.MoveToWorld(new Point3D(2598, 747, 0), Map.Sosaria);
                    spawner12.Name = "MinocInvasionSosaria";
                    spawner12.Respawn();

                    Spawner spawner5 = new Spawner(6, 5, 15, 20, 4, "JukaWarrior");
                    spawner5.MoveToWorld(new Point3D(2579, 376, 5), Map.Sosaria);
                    spawner5.WayPoint = point65;
                    point64.MoveToWorld(new Point3D(2579, 398, 15), Map.Sosaria);
                    point64.NextPoint = point65;
                    point65.MoveToWorld(new Point3D(2623, 437, 15), Map.Sosaria);
                    point65.NextPoint = point66;
                    point66.MoveToWorld(new Point3D(2617, 506, 15), Map.Sosaria);
                    point66.NextPoint = point67;
                    point67.MoveToWorld(new Point3D(2562, 513, 15), Map.Sosaria);
                    point67.NextPoint = point68;
                    point68.MoveToWorld(new Point3D(2551, 501, 15), Map.Sosaria);
                    point68.NextPoint = point69;
                    point69.MoveToWorld(new Point3D(2525, 501, 15), Map.Sosaria);
                    point69.NextPoint = point70;
                    point70.MoveToWorld(new Point3D(2525, 516, 0), Map.Sosaria);
                    point70.NextPoint = point71;
                    point71.MoveToWorld(new Point3D(2489, 516, 0), Map.Sosaria);
                    point71.NextPoint = point72;
                    point72.MoveToWorld(new Point3D(2489, 482, 15), Map.Sosaria);
                    point72.NextPoint = point73;
                    point73.MoveToWorld(new Point3D(2500, 484, 15), Map.Sosaria);
                    point73.NextPoint = point74;
                    point74.MoveToWorld(new Point3D(2500, 442, 15), Map.Sosaria);
                    point74.NextPoint = point75;
                    point75.MoveToWorld(new Point3D(2514, 442, 15), Map.Sosaria);
                    point75.NextPoint = point76;
                    point76.MoveToWorld(new Point3D(2514, 419, 15), Map.Sosaria);
                    point76.NextPoint = point77;
                    point77.MoveToWorld(new Point3D(2445, 419, 15), Map.Sosaria);
                    point77.NextPoint = point78;
                    point78.MoveToWorld(new Point3D(2444, 444, 15), Map.Sosaria);
                    point78.NextPoint = point79;
                    point79.MoveToWorld(new Point3D(2531, 444, 15), Map.Sosaria);
                    point79.NextPoint = point69;
                    spawner5.Name = "MinocInvasionSosaria";
                    spawner5.Respawn();

                    Spawner spawner13 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner13.MoveToWorld(new Point3D(2579, 376, 5), Map.Sosaria);
                    spawner13.Name = "MinocInvasionSosaria";
                    spawner13.Respawn();

                    Spawner spawner6 = new Spawner(1, 5, 15, 20, 0, "JukaLord");
                    spawner6.MoveToWorld(new Point3D(2420, 420, 15), Map.Sosaria);
                    spawner6.WayPoint = point80;
                    point80.MoveToWorld(new Point3D(2489, 419, 15), Map.Sosaria);
                    point80.NextPoint = point81;
                    point81.MoveToWorld(new Point3D(2491, 442, 15), Map.Sosaria);
                    point81.NextPoint = point82;
                    point82.MoveToWorld(new Point3D(2476, 442, 15), Map.Sosaria);
                    point82.NextPoint = point83;
                    point83.MoveToWorld(new Point3D(2475, 460, 15), Map.Sosaria);
                    point83.NextPoint = point84;
                    point84.MoveToWorld(new Point3D(2467, 460, 15), Map.Sosaria);
                    point84.NextPoint = point85;
                    point85.MoveToWorld(new Point3D(2469, 481, 15), Map.Sosaria);
                    point85.NextPoint = point86;
                    point86.MoveToWorld(new Point3D(2491, 481, 15), Map.Sosaria);
                    point86.NextPoint = point87;
                    point87.MoveToWorld(new Point3D(2488, 564, 5), Map.Sosaria);
                    point87.NextPoint = point88;
                    point88.MoveToWorld(new Point3D(2514, 561, 0), Map.Sosaria);
                    point88.NextPoint = point89;
                    point89.MoveToWorld(new Point3D(2516, 529, 0), Map.Sosaria);
                    point89.NextPoint = point90;
                    point90.MoveToWorld(new Point3D(2489, 529, 0), Map.Sosaria);
                    point90.NextPoint = point91;
                    point91.MoveToWorld(new Point3D(2489, 493, 15), Map.Sosaria);
                    point91.NextPoint = point92;
                    point92.MoveToWorld(new Point3D(2504, 482, 15), Map.Sosaria);
                    point92.NextPoint = point80;
                    spawner6.Name = "MinocInvasionSosaria";
                    spawner6.Respawn();

                    Spawner spawner14 = new Spawner(6, 20, 20, 20, 20, "Swampdragon");
                    spawner14.MoveToWorld(new Point3D(2420, 420, 15), Map.Sosaria);
                    spawner14.Name = "MinocInvasionSosaria";
                    spawner14.Respawn();

                    Spawner spawner7 = new Spawner(6, 5, 15, 20, 0, "JukaMage");
                    spawner7.MoveToWorld(new Point3D(2534, 624, 0), Map.Sosaria);
                    spawner7.WayPoint = point93;
                    point93.MoveToWorld(new Point3D(2513, 620, 0), Map.Sosaria);
                    point93.NextPoint = point94;
                    point94.MoveToWorld(new Point3D(2517, 562, 0), Map.Sosaria);
                    point94.NextPoint = point95;
                    point95.MoveToWorld(new Point3D(2504, 562, 0), Map.Sosaria);
                    point95.NextPoint = point96;
                    point96.MoveToWorld(new Point3D(2504, 547, 0), Map.Sosaria);
                    point96.NextPoint = point97;
                    point97.MoveToWorld(new Point3D(2501, 547, 0), Map.Sosaria);
                    point97.NextPoint = point98;
                    point98.MoveToWorld(new Point3D(2504, 547, 0), Map.Sosaria);
                    point98.NextPoint = point99;
                    point99.MoveToWorld(new Point3D(2504, 562, 0), Map.Sosaria);
                    point99.NextPoint = point100;
                    point100.MoveToWorld(new Point3D(2492, 562, 0), Map.Sosaria);
                    point100.NextPoint = point101;
                    point101.MoveToWorld(new Point3D(2491, 540, 0), Map.Sosaria);
                    point101.NextPoint = point102;
                    point102.MoveToWorld(new Point3D(2517, 539, 0), Map.Sosaria);
                    point102.NextPoint = point94;
                    spawner7.Name = "MinocInvasionSosaria";
                    spawner7.Respawn();

                    Spawner spawner15 = new Spawner(5, 20, 20, 20, 20, "JukaMage");
                    spawner15.MoveToWorld(new Point3D(2534, 624, 0), Map.Sosaria);
                    spawner15.Name = "MinocInvasionSosaria";
                    spawner15.Respawn();

                    Spawner spawner18 = new Spawner(5, 20, 20, 20, 20, "JukaLord");
                    spawner18.MoveToWorld(new Point3D(2534, 624, 0), Map.Sosaria);
                    spawner18.Name = "MinocInvasionSosaria";
                    spawner18.Respawn();

                    Spawner spawner8 = new Spawner(3, 10, 20, 20, 10, "JukaLord");
                    spawner8.MoveToWorld(new Point3D(2638, 652, 0), Map.Sosaria);
                    spawner8.WayPoint = point103;
                    point103.MoveToWorld(new Point3D(2591, 647, 0), Map.Sosaria);
                    point103.NextPoint = point104;
                    point104.MoveToWorld(new Point3D(2586, 641, 0), Map.Sosaria);
                    point104.NextPoint = point105;
                    point105.MoveToWorld(new Point3D(2584, 622, 0), Map.Sosaria);
                    point105.NextPoint = point106;
                    point106.MoveToWorld(new Point3D(2513, 620, 0), Map.Sosaria);
                    point106.NextPoint = point107;
                    point107.MoveToWorld(new Point3D(2517, 562, 0), Map.Sosaria);
                    point107.NextPoint = point108;
                    point108.MoveToWorld(new Point3D(2489, 563, 1), Map.Sosaria);
                    point108.NextPoint = point109;
                    point109.MoveToWorld(new Point3D(2489, 535, 0), Map.Sosaria);
                    point109.NextPoint = point110;
                    point110.MoveToWorld(new Point3D(2517, 535, 0), Map.Sosaria);
                    point110.NextPoint = point111;
                    point111.MoveToWorld(new Point3D(2514, 561, 0), Map.Sosaria);
                    point111.NextPoint = point112;
                    point112.MoveToWorld(new Point3D(2504, 562, 0), Map.Sosaria);
                    point112.NextPoint = point113;
                    point113.MoveToWorld(new Point3D(2504, 555, 0), Map.Sosaria);
                    point113.NextPoint = point114;
                    point114.MoveToWorld(new Point3D(2504, 562, 0), Map.Sosaria);
                    point114.NextPoint = point115;
                    point115.MoveToWorld(new Point3D(2540, 562, 0), Map.Sosaria);
                    point115.NextPoint = point116;
                    point116.MoveToWorld(new Point3D(2487, 570, 5), Map.Sosaria);
                    point116.NextPoint = point117;
                    point117.MoveToWorld(new Point3D(2473, 570, 5), Map.Sosaria);
                    point117.NextPoint = point118;
                    point118.MoveToWorld(new Point3D(2473, 565, 5), Map.Sosaria);
                    point118.NextPoint = point119;
                    point119.MoveToWorld(new Point3D(2473, 570, 5), Map.Sosaria);
                    point119.NextPoint = point120;
                    point120.MoveToWorld(new Point3D(2490, 570, 5), Map.Sosaria);
                    point120.NextPoint = point108;
                    spawner8.Name = "MinocInvasionSosaria";
                    spawner8.Respawn();

                    Spawner spawner16 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner16.MoveToWorld(new Point3D(2638, 652, 0), Map.Sosaria);
                    spawner16.Name = "MinocInvasionSosaria";
                    spawner16.Respawn();

                    Spawner spawner17 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner17.MoveToWorld(new Point3D(2476, 416, 15), Map.Sosaria);
                    spawner17.Name = "MinocInvasionSosaria";
                    spawner17.Respawn();

                    Spawner spawner19 = new Spawner(5, 20, 20, 20, 20, "JukaMage");
                    spawner19.MoveToWorld(new Point3D(2500, 463, 15), Map.Sosaria);
                    spawner19.Name = "MinocInvasionSosaria";
                    spawner19.Respawn();

                    Spawner spawner20 = new Spawner(3, 20, 20, 20, 20, "JukaLord");
                    spawner20.MoveToWorld(new Point3D(2500, 463, 15), Map.Sosaria);
                    spawner20.Name = "MinocInvasionSosaria";
                    spawner20.Respawn();

                    Spawner spawner21 = new Spawner(5, 20, 20, 20, 20, "JukaWarrior");
                    spawner21.MoveToWorld(new Point3D(2525, 479, 15), Map.Sosaria);
                    spawner21.Name = "MinocInvasionSosaria";
                    spawner21.Respawn();

                    Spawner spawner22 = new Spawner(2, 2, 2, 20, 2, "JukaLord");
                    spawner22.MoveToWorld(new Point3D(2539, 501, 30), Map.Sosaria);
                    spawner22.Name = "MinocInvasionSosaria";
                    spawner22.Respawn();

                    Spawner spawner23 = new Spawner(4, 20, 20, 20, 20, "JukaLord");
                    spawner23.MoveToWorld(new Point3D(2487, 518, 0), Map.Sosaria);
                    spawner23.Name = "MinocInvasionSosaria";
                    spawner23.Respawn();

                    Spawner spawner24 = new Spawner(6, 20, 20, 20, 20, "JukaWarrior");
                    spawner24.MoveToWorld(new Point3D(2526, 512, 7), Map.Sosaria);
                    spawner24.Name = "MinocInvasionSosaria";
                    spawner24.Respawn();

                    Spawner spawner25 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner25.MoveToWorld(new Point3D(2526, 512, 7), Map.Sosaria);
                    spawner25.Name = "MinocInvasionSosaria";
                    spawner25.Respawn();

                    Spawner spawner26 = new Spawner(4, 20, 20, 20, 20, "JukaLord");
                    spawner26.MoveToWorld(new Point3D(2508, 379, 0), Map.Sosaria);
                    spawner26.Name = "MinocInvasionSosaria";
                    spawner26.Respawn();

                    Spawner spawner27 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner27.MoveToWorld(new Point3D(2556, 342, 15), Map.Sosaria);
                    spawner27.Name = "MinocInvasionSosaria";
                    spawner27.Respawn();

                    Spawner spawner28 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner28.MoveToWorld(new Point3D(2527, 440, 15), Map.Sosaria);
                    spawner28.Name = "MinocInvasionSosaria";
                    spawner28.Respawn();

                    Spawner spawner29 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner29.MoveToWorld(new Point3D(2496, 572, 3), Map.Sosaria);
                    spawner29.Name = "MinocInvasionSosaria";
                    spawner29.Respawn();

                    Spawner spawner30 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner30.MoveToWorld(new Point3D(2496, 572, 3), Map.Sosaria);
                    spawner30.Name = "MinocInvasionSosaria";
                    spawner30.Respawn();

                    Spawner spawner31 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner31.MoveToWorld(new Point3D(2501, 619, 0), Map.Sosaria);
                    spawner31.Name = "MinocInvasionSosaria";
                    spawner31.Respawn();

                    Spawner spawner32 = new Spawner(7, 20, 20, 20, 20, "JukaWarrior");
                    spawner32.MoveToWorld(new Point3D(2519, 672, 0), Map.Sosaria);
                    spawner32.Name = "MinocInvasionSosaria";
                    spawner32.Respawn();

                    Spawner spawner33 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner33.MoveToWorld(new Point3D(2553, 706, 0), Map.Sosaria);
                    spawner33.Name = "MinocInvasionSosaria";
                    spawner33.Respawn();

                    Spawner spawner34 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner34.MoveToWorld(new Point3D(2553, 706, 0), Map.Sosaria);
                    spawner34.Name = "MinocInvasionSosaria";
                    spawner34.Respawn();

                    Spawner spawner35 = new Spawner(10, 20, 20, 20, 20, "JukaWarrior");
                    spawner35.MoveToWorld(new Point3D(2587, 685, 0), Map.Sosaria);
                    spawner35.Name = "MinocInvasionSosaria";
                    spawner35.Respawn();

                    Spawner spawner36 = new Spawner(5, 20, 20, 20, 20, "Swampdragon");
                    spawner36.MoveToWorld(new Point3D(2607, 669, 0), Map.Sosaria);
                    spawner36.Name = "MinocInvasionSosaria";
                    spawner36.Respawn();

                    Spawner spawner37 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner37.MoveToWorld(new Point3D(2453, 544, 0), Map.Sosaria);
                    spawner37.Name = "MinocInvasionSosaria";
                    spawner37.Respawn();

                    Spawner spawner38 = new Spawner(8, 20, 20, 20, 10, "JukaWarrior");
                    spawner38.MoveToWorld(new Point3D(2417, 506, 15), Map.Sosaria);
                    spawner38.Name = "MinocInvasionSosaria";
                    spawner38.Respawn();

                    Spawner spawner39 = new Spawner(3, 20, 20, 20, 10, "JukaLord");
                    spawner39.MoveToWorld(new Point3D(2426, 456, 15), Map.Sosaria);
                    spawner39.Name = "MinocInvasionSosaria";
                    spawner39.Respawn();

                    Spawner spawner40 = new Spawner(3, 20, 20, 20, 10, "JukaMage");
                    spawner40.MoveToWorld(new Point3D(2426, 456, 15), Map.Sosaria);
                    spawner40.Name = "MinocInvasionSosaria";
                    spawner40.Respawn();

                    Spawner spawner41 = new Spawner(7, 20, 20, 20, 20, "JukaWarrior");
                    spawner41.MoveToWorld(new Point3D(2446, 400, 15), Map.Sosaria);
                    spawner41.Name = "MinocInvasionSosaria";
                    spawner41.Respawn();

                    Spawner spawner42 = new Spawner(7, 20, 20, 20, 10, "JukaWarrior");
                    spawner42.MoveToWorld(new Point3D(2451, 469, 15), Map.Sosaria);
                    spawner42.Name = "MinocInvasionSosaria";
                    spawner42.Respawn();

                    Spawner spawner43 = new Spawner(5, 20, 20, 20, 10, "JukaWarrior");
                    spawner43.MoveToWorld(new Point3D(2511, 387, 15), Map.Sosaria);
                    spawner43.Name = "MinocInvasionSosaria";
                    spawner43.Respawn();

                    Spawner spawner44 = new Spawner(1, 2, 2, 20, 1, "TheSavageGeneral");
                    spawner44.MoveToWorld(new Point3D(2454, 477, 15), Map.Sosaria);
                    spawner44.Name = "MinocInvasionSosaria";
                    spawner44.Respawn();

                    Spawner spawner45 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner45.MoveToWorld(new Point3D(2449, 486, 15), Map.Sosaria);
                    spawner45.Name = "MinocInvasionSosaria";
                    spawner45.Respawn();

                    Spawner spawner46 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner46.MoveToWorld(new Point3D(2455, 493, 15), Map.Sosaria);
                    spawner46.Name = "MinocInvasionSosaria";
                    spawner46.Respawn();

                    Spawner spawner47 = new Spawner(1, 1, 1, 20, 1, "JukaMage");
                    spawner47.MoveToWorld(new Point3D(2458, 487, 15), Map.Sosaria);
                    spawner47.Name = "MinocInvasionSosaria";
                    spawner47.Respawn();

                    Spawner spawner48 = new Spawner(2, 2, 2, 20, 2, "JukaLord");
                    spawner48.MoveToWorld(new Point3D(2458, 487, 15), Map.Sosaria);
                    spawner48.Name = "MinocInvasionSosaria";
                    spawner48.Respawn();

                    World.Broadcast(33, true, "Minoc Sosaria is under invasion.");
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
                            "You are not in a The guarded part of Minoc, Sosaria."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards on."
                        );
                    }
                    else if (DummyMessage)
                    {
                        from.SendMessage(
                            33,
                            "If you are not in a The guarded part of Minoc, Sosaria."
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
                            "If you are not in the guarded part of Magincia, Sosaria."
                        );
                        from.SendMessage(
                            33,
                            "You will have to go there and use [toggleguarded to turn the guards off."
                        );
                    }
                    MinocInvasionStone minoctram = new MinocInvasionStone();
                    minoctram.StopMinocSosaria();
                    World.Broadcast(
                        33,
                        true,
                        "Minoc Sosaria's invasion was successfully beaten back. No more invaders are left in the city."
                    );
                    from.SendGump(new CityInvasion(from));
                    break;
                }
            }
        }
    }
}
