using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

// Define a namespace for organization and to avoid naming conflicts
namespace Server.Gumps
{
	// The StartStopBrittram class, inheriting from Gump, is used to create a graphical interface
	public class StartStopBrittram : Gump
	{
		// Mobile is a base class for all players and NPCs in the game
		private Mobile m_Mobile;

		// Constructor for the StartStopBrittram class
		public StartStopBrittram(Mobile from)
			: base(0, 0) // Initialize the base Gump class
		{
			// Storing the reference to the mobile who opened this Gump
			m_Mobile = from;
			// This Gump cannot be closed using the standard close button
			Closable = false;
			// This Gump can be moved around the screen
			Dragable = true;

			// Add a new page to the Gump
			AddPage(0);

			// GUI elements are added here, such as images and buttons
			AddImage(112, 73, 39);
			AddButton(135, 123, 9804, 9806, 1, GumpButtonType.Reply, 1);
			AddButton(138, 194, 9804, 9806, 2, GumpButtonType.Reply, 2);
			AddButton(277, 311, 2453, 2455, 0, GumpButtonType.Reply, 0);
			AddLabel(216, 140, 0, "Start an Invasion");
			AddLabel(218, 208, 0, "Stop an Invasion");
		}

		// The OnResponse method is called when the player interacts with the Gump
		public override void OnResponse(NetState state, RelayInfo info)
		{
			// The player who interacted with the Gump
			Mobile from = state.Mobile;

			// Handling different button clicks
			switch (info.ButtonID)
			{
				// Case when the "Close" button is clicked
				case 0:
					{
						// Close the current Gump
						from.CloseGump(typeof(StartStopBrittram));
						// Open a new Gump for city invasion management
						from.SendGump(new CityInvasion(from));
						break;
					}

				// Case when the "Start an Invasion" button is clicked
				case 1:
					{
						// Logic for starting an invasion should be implemented here
						// This typically involves creating and positioning various NPCs and objects in the game world

						// Define a central point for the invasion to start
						Point3D loc = new Point3D(568, 1311, 0);

/*						EXAMPLE FOR SINGLE ENTRY WITH WAYPOINTS
						// Creating a few waypoints for the invasion path
						WayPoint point1 = new WayPoint();
						WayPoint point2 = new WayPoint();
						WayPoint point3 = new WayPoint();

						// Setting the name for waypoints for identification
						point1.Name = "BritInvasionSosaria";
						point2.Name = "BritInvasionSosaria";
						point3.Name = "BritInvasionSosaria";

						// Positioning waypoints in the world and linking them to create a path
						point1.MoveToWorld(new Point3D(1374, 1752, 1), Map.Sosaria);
						point1.NextPoint = point2;

						point2.MoveToWorld(new Point3D(1440, 1697, 0), Map.Sosaria);
						point2.NextPoint = point3;

						point3.MoveToWorld(new Point3D(1462, 1657, 10), Map.Sosaria);
						point3.NextPoint = point1;  // Looping back to the first point

						// Creating a spawner for the invasion with linked waypoints
						Spawner spawner = new Spawner(2, 5, 15, 30, 4, "Overseer");
						spawner.MoveToWorld(new Point3D(1318, 1756, 10), Map.Sosaria);
						spawner.WayPoint = point1;
						spawner.Name = "BritInvasionSosaria";
						spawner.Respawn();
*/
						// Note: The actual implementation will involve creating multiple waypoints and spawners
						// based on the desired complexity and scale of the invasion

						// Creating multiple waypoints. Waypoints guide the path of NPCs during the invasion.
						// These are strategically placed throughout the map.
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

						// Setting the name for waypoints. This could be used for identification or triggering specific behavior.
						point.Name = "BritInvasionSosaria";
						point1.Name = "BritInvasionSosaria";
						point2.Name = "BritInvasionSosaria";
						point3.Name = "BritInvasionSosaria";
						point4.Name = "BritInvasionSosaria";
						point5.Name = "BritInvasionSosaria";
						point6.Name = "BritInvasionSosaria";
						point7.Name = "BritInvasionSosaria";
						point8.Name = "BritInvasionSosaria";
						point9.Name = "BritInvasionSosaria";

						point10.Name = "BritInvasionSosaria";
						point11.Name = "BritInvasionSosaria";
						point12.Name = "BritInvasionSosaria";
						point13.Name = "BritInvasionSosaria";
						point14.Name = "BritInvasionSosaria";
						point15.Name = "BritInvasionSosaria";
						point16.Name = "BritInvasionSosaria";
						point17.Name = "BritInvasionSosaria";
						point18.Name = "BritInvasionSosaria";
						point19.Name = "BritInvasionSosaria";

						point20.Name = "BritInvasionSosaria";
						point21.Name = "BritInvasionSosaria";
						point22.Name = "BritInvasionSosaria";
						point23.Name = "BritInvasionSosaria";
						point24.Name = "BritInvasionSosaria";
						point25.Name = "BritInvasionSosaria";
						point26.Name = "BritInvasionSosaria";
						point27.Name = "BritInvasionSosaria";
						point28.Name = "BritInvasionSosaria";
						point29.Name = "BritInvasionSosaria";

						point30.Name = "BritInvasionSosaria";
						point31.Name = "BritInvasionSosaria";
						point32.Name = "BritInvasionSosaria";
						point33.Name = "BritInvasionSosaria";
						point34.Name = "BritInvasionSosaria";
						point35.Name = "BritInvasionSosaria";
						point36.Name = "BritInvasionSosaria";
						point37.Name = "BritInvasionSosaria";
						point38.Name = "BritInvasionSosaria";
						point39.Name = "BritInvasionSosaria";

						point40.Name = "BritInvasionSosaria";
						point41.Name = "BritInvasionSosaria";
						point42.Name = "BritInvasionSosaria";
						point43.Name = "BritInvasionSosaria";
						point44.Name = "BritInvasionSosaria";
						point45.Name = "BritInvasionSosaria";
						point46.Name = "BritInvasionSosaria";
						point47.Name = "BritInvasionSosaria";
						point48.Name = "BritInvasionSosaria";
						point49.Name = "BritInvasionSosaria";

						point50.Name = "BritInvasionSosaria";
						point51.Name = "BritInvasionSosaria";
						point52.Name = "BritInvasionSosaria";
						point53.Name = "BritInvasionSosaria";
						point54.Name = "BritInvasionSosaria";
						point55.Name = "BritInvasionSosaria";
						point56.Name = "BritInvasionSosaria";
						point57.Name = "BritInvasionSosaria";
						point58.Name = "BritInvasionSosaria";
						point59.Name = "BritInvasionSosaria";

						point60.Name = "BritInvasionSosaria";
						point61.Name = "BritInvasionSosaria";
						point62.Name = "BritInvasionSosaria";
						point63.Name = "BritInvasionSosaria";
						point64.Name = "BritInvasionSosaria";
						point65.Name = "BritInvasionSosaria";
						point66.Name = "BritInvasionSosaria";
						point67.Name = "BritInvasionSosaria";
						point68.Name = "BritInvasionSosaria";
						point69.Name = "BritInvasionSosaria";

						point70.Name = "BritInvasionSosaria";
						point71.Name = "BritInvasionSosaria";
						point72.Name = "BritInvasionSosaria";
						point73.Name = "BritInvasionSosaria";
						point74.Name = "BritInvasionSosaria";
						point75.Name = "BritInvasionSosaria";
						point76.Name = "BritInvasionSosaria";
						point77.Name = "BritInvasionSosaria";
						point78.Name = "BritInvasionSosaria";
						point79.Name = "BritInvasionSosaria";
						point80.Name = "BritInvasionSosaria";
						point81.Name = "BritInvasionSosaria";

						// Creating spawner for the invasion. Spawner generate NPCs or creatures.
						// The parameters define the spawn details like number of creatures, their type, and spawn frequency.
						Spawner spawner1 = new Spawner(2, 5, 15, 30, 4, "Overseer"); // Spawner(amount, minDelay, maxDelay, team, homeRange, spawnName)

						// Positioning spawners and linking them to waypoints.
						// This determines where NPCs will appear and the path they will follow.
						spawner1.MoveToWorld(new Point3D(1318, 1756, 10), Map.Sosaria); // Placing spawner in the world
						spawner1.WayPoint = point; // Linking spawner to a waypoint

						// Setting up the route for each waypoint. This defines the path that NPCs will take during the invasion.
						point.MoveToWorld(new Point3D(1374, 1752, 1), Map.Sosaria); // Positioning the first waypoint
						point.NextPoint = point1; // Linking to the next waypoint in the path
						point1.MoveToWorld(new Point3D(1440, 1697, 0), Map.Sosaria);
						point1.NextPoint = point2;
						point2.MoveToWorld(new Point3D(1462, 1657, 10), Map.Sosaria);
						point2.NextPoint = point3;
						point3.MoveToWorld(new Point3D(1482, 1605, 20), Map.Sosaria);
						point3.NextPoint = point4;
						point4.MoveToWorld(new Point3D(1424, 1603, 20), Map.Sosaria);
						point4.NextPoint = point5;
						point5.MoveToWorld(new Point3D(1413, 1662, 10), Map.Sosaria);
						point5.NextPoint = point6;
						point6.MoveToWorld(new Point3D(1418, 1697, 0), Map.Sosaria);
						point6.NextPoint = point7;
						point7.MoveToWorld(new Point3D(1481, 1697, 0), Map.Sosaria);
						point7.NextPoint = point8;
						point8.MoveToWorld(new Point3D(1418, 1699, 0), Map.Sosaria);
						point8.NextPoint = point9;
						point9.MoveToWorld(new Point3D(1420, 1726, 20), Map.Sosaria);
						point9.NextPoint = point;

						// Naming spawner for identification or specific behavior.
						spawner1.Name = "BritInvasionSosaria";

						// Triggering the spawner to start generating NPCs.
						spawner1.Respawn();

						Spawner spawner2 = new Spawner(2, 5, 15, 30, 20, "Overseer");
						spawner2.MoveToWorld(new Point3D(1363, 1801, 0), Map.Sosaria);
						spawner2.WayPoint = point10;
						point10.MoveToWorld(new Point3D(1373, 1751, 2), Map.Sosaria);
						point10.NextPoint = point11;
						point11.MoveToWorld(new Point3D(1408, 1745, 5), Map.Sosaria);
						point11.NextPoint = point12;
						point12.MoveToWorld(new Point3D(1419, 1669, 10), Map.Sosaria);
						point12.NextPoint = point13;
						point13.MoveToWorld(new Point3D(1475, 1650, 10), Map.Sosaria);
						point13.NextPoint = point14;
						point14.MoveToWorld(new Point3D(1513, 1650, 20), Map.Sosaria);
						point14.NextPoint = point15;
						point15.MoveToWorld(new Point3D(1513, 1629, 10), Map.Sosaria);
						point15.NextPoint = point16;
						point16.MoveToWorld(new Point3D(1420, 1627, 20), Map.Sosaria);
						point16.NextPoint = point17;
						point17.MoveToWorld(new Point3D(1330, 1625, 50), Map.Sosaria);
						point17.NextPoint = point18;
						point18.MoveToWorld(new Point3D(1420, 1625, 20), Map.Sosaria);
						point18.NextPoint = point19;
						point19.MoveToWorld(new Point3D(1420, 1725, 20), Map.Sosaria);
						point19.NextPoint = point10;
						spawner2.Name = "BritInvasionSosaria";
						spawner2.Respawn();

						Spawner spawner3 = new Spawner(2, 5, 15, 30, 20, "Overseer");
						spawner3.MoveToWorld(new Point3D(1349, 1533, 0), Map.Sosaria);
						spawner3.WayPoint = point20;
						point20.MoveToWorld(new Point3D(1425, 1532, 32), Map.Sosaria);
						point20.NextPoint = point21;
						point21.MoveToWorld(new Point3D(1479, 1542, 30), Map.Sosaria);
						point21.NextPoint = point22;
						point22.MoveToWorld(new Point3D(1479, 1572, 30), Map.Sosaria);
						point22.NextPoint = point23;
						point23.MoveToWorld(new Point3D(1493, 1577, 30), Map.Sosaria);
						point23.NextPoint = point24;
						point24.MoveToWorld(new Point3D(1519, 1579, 27), Map.Sosaria);
						point24.NextPoint = point25;
						point25.MoveToWorld(new Point3D(1565, 1579, 20), Map.Sosaria);
						point25.NextPoint = point26;
						point26.MoveToWorld(new Point3D(1567, 1630, 6), Map.Sosaria);
						point26.NextPoint = point27;
						point27.MoveToWorld(new Point3D(1536, 1630, 2), Map.Sosaria);
						point27.NextPoint = point28;
						point28.MoveToWorld(new Point3D(1443, 1622, 20), Map.Sosaria);
						point28.NextPoint = point29;
						point29.MoveToWorld(new Point3D(1438, 1587, 20), Map.Sosaria);
						point29.NextPoint = point20;
						spawner3.Name = "BritInvasionSosaria";
						spawner3.Respawn();

						Spawner spawner4 = new Spawner(2, 5, 15, 30, 50, "Overseer");
						spawner4.MoveToWorld(new Point3D(1385, 1385, 0), Map.Sosaria);
						spawner4.WayPoint = point30;
						point30.MoveToWorld(new Point3D(1428, 1528, 32), Map.Sosaria);
						point30.NextPoint = point31;
						point31.MoveToWorld(new Point3D(1503, 1535, 31), Map.Sosaria);
						point31.NextPoint = point32;
						point32.MoveToWorld(new Point3D(1499, 1578, 30), Map.Sosaria);
						point32.NextPoint = point33;
						point33.MoveToWorld(new Point3D(1485, 1602, 17), Map.Sosaria);
						point33.NextPoint = point34;
						point34.MoveToWorld(new Point3D(1485, 1632, 20), Map.Sosaria);
						point34.NextPoint = point35;
						point35.MoveToWorld(new Point3D(1518, 1630, 10), Map.Sosaria);
						point35.NextPoint = point36;
						point36.MoveToWorld(new Point3D(1541, 1630, 0), Map.Sosaria);
						point36.NextPoint = point37;
						point37.MoveToWorld(new Point3D(1532, 1673, 20), Map.Sosaria);
						point37.NextPoint = point38;
						point38.MoveToWorld(new Point3D(1491, 1670, 10), Map.Sosaria);
						point38.NextPoint = point39;
						point39.MoveToWorld(new Point3D(1483, 1603, 19), Map.Sosaria);
						point39.NextPoint = point40;
						point40.MoveToWorld(new Point3D(1463, 1602, 20), Map.Sosaria);
						point40.NextPoint = point41;
						point41.MoveToWorld(new Point3D(1464, 1572, 30), Map.Sosaria);
						point41.NextPoint = point42;
						point42.MoveToWorld(new Point3D(1432, 1571, 30), Map.Sosaria);
						point42.NextPoint = point30;
						spawner4.Name = "BritInvasionSosaria";
						spawner4.Respawn();

						Spawner spawner5 = new Spawner(3, 5, 15, 30, 90, "Overseer");
						spawner5.MoveToWorld(new Point3D(1270, 2060, 0), Map.Sosaria);
						spawner5.WayPoint = point43;
						point43.MoveToWorld(new Point3D(1373, 1809, 0), Map.Sosaria);
						point43.NextPoint = point44;
						point44.MoveToWorld(new Point3D(1368, 1750, 5), Map.Sosaria);
						point44.NextPoint = point45;
						point45.MoveToWorld(new Point3D(1403, 1747, 3), Map.Sosaria);
						point45.NextPoint = point46;
						point46.MoveToWorld(new Point3D(1423, 1698, 0), Map.Sosaria);
						point46.NextPoint = point47;
						point47.MoveToWorld(new Point3D(1481, 1699, 0), Map.Sosaria);
						point47.NextPoint = point48;
						point48.MoveToWorld(new Point3D(1504, 1705, 20), Map.Sosaria);
						point48.NextPoint = point49;
						point49.MoveToWorld(new Point3D(1557, 1699, 30), Map.Sosaria);
						point49.NextPoint = point50;
						point50.MoveToWorld(new Point3D(1569, 1636, 5), Map.Sosaria);
						point50.NextPoint = point51;
						point51.MoveToWorld(new Point3D(1617, 1636, 35), Map.Sosaria);
						point51.NextPoint = point52;
						point52.MoveToWorld(new Point3D(1601, 1691, 10), Map.Sosaria);
						point52.NextPoint = point53;
						point53.MoveToWorld(new Point3D(1589, 1724, 20), Map.Sosaria);
						point53.NextPoint = point54;
						point54.MoveToWorld(new Point3D(1533, 1724, 20), Map.Sosaria);
						point54.NextPoint = point55;
						point55.MoveToWorld(new Point3D(1533, 1647, 20), Map.Sosaria);
						point55.NextPoint = point56;
						point56.MoveToWorld(new Point3D(1569, 1595, 23), Map.Sosaria);
						point56.NextPoint = point57;
						point57.MoveToWorld(new Point3D(1571, 1530, 39), Map.Sosaria);
						point57.NextPoint = point58;
						point58.MoveToWorld(new Point3D(1432, 1573, 30), Map.Sosaria);
						point58.NextPoint = point59;
						point59.MoveToWorld(new Point3D(1429, 1531, 35), Map.Sosaria);
						point59.NextPoint = point60;
						point60.MoveToWorld(new Point3D(1437, 1588, 20), Map.Sosaria);
						point60.NextPoint = point61;
						point61.MoveToWorld(new Point3D(1420, 1627, 20), Map.Sosaria);
						point61.NextPoint = point62;
						point62.MoveToWorld(new Point3D(1420, 1728, 20), Map.Sosaria);
						point62.NextPoint = point44;
						spawner5.Name = "BritInvasionSosaria";
						spawner5.Respawn();

						Spawner spawner6 = new Spawner(3, 5, 15, 0, 10, "Overseer");
						spawner6.MoveToWorld(new Point3D(1435, 1498, 34), Map.Sosaria);
						spawner6.WayPoint = point63;
						point63.MoveToWorld(new Point3D(1429, 1537, 29), Map.Sosaria);
						point63.NextPoint = point64;
						point64.MoveToWorld(new Point3D(1432, 1572, 30), Map.Sosaria);
						point64.NextPoint = point65;
						point65.MoveToWorld(new Point3D(1437, 1588, 20), Map.Sosaria);
						point65.NextPoint = point66;
						point66.MoveToWorld(new Point3D(1424, 1590, 20), Map.Sosaria);
						point66.NextPoint = point67;
						point67.MoveToWorld(new Point3D(1424, 1625, 20), Map.Sosaria);
						point67.NextPoint = point68;
						point68.MoveToWorld(new Point3D(1463, 1625, 20), Map.Sosaria);
						point68.NextPoint = point69;
						point69.MoveToWorld(new Point3D(1463, 1573, 30), Map.Sosaria);
						point69.NextPoint = point64;
						spawner6.Name = "BritInvasionSosaria";
						spawner6.Respawn();

						Spawner spawner7 = new Spawner(3, 5, 15, 30, 0, "Overseer");
						spawner7.MoveToWorld(new Point3D(1351, 1757, 17), Map.Sosaria);
						spawner7.WayPoint = point70;
						point70.MoveToWorld(new Point3D(1373, 1753, 3), Map.Sosaria);
						point70.NextPoint = point71;
						point71.MoveToWorld(new Point3D(1420, 1699, 0), Map.Sosaria);
						point71.NextPoint = point72;
						point72.MoveToWorld(new Point3D(1447, 1696, 0), Map.Sosaria);
						point72.NextPoint = point73;
						point73.MoveToWorld(new Point3D(1447, 1679, 0), Map.Sosaria);
						point73.NextPoint = point74;
						point74.MoveToWorld(new Point3D(1434, 1667, 10), Map.Sosaria);
						point74.NextPoint = point75;
						point75.MoveToWorld(new Point3D(1417, 1668, 10), Map.Sosaria);
						point75.NextPoint = point71;
						spawner7.Name = "BritInvasionSosaria";
						spawner7.Respawn();

						Spawner spawner8 = new Spawner(3, 10, 20, 30, 10, "Overseer");
						spawner8.MoveToWorld(new Point3D(1370, 1749, 3), Map.Sosaria);
						spawner8.WayPoint = point76;
						point76.MoveToWorld(new Point3D(1405, 1746, 5), Map.Sosaria);
						point76.NextPoint = point77;
						point77.MoveToWorld(new Point3D(1420, 1699, 0), Map.Sosaria);
						point77.NextPoint = point78;
						point78.MoveToWorld(new Point3D(1447, 1696, 0), Map.Sosaria);
						point78.NextPoint = point79;
						point79.MoveToWorld(new Point3D(1447, 1679, 0), Map.Sosaria);
						point79.NextPoint = point80;
						point80.MoveToWorld(new Point3D(1426, 1666, 10), Map.Sosaria);
						point80.NextPoint = point81;
						point81.MoveToWorld(new Point3D(1417, 1668, 10), Map.Sosaria);
						point81.NextPoint = point77;
						spawner8.Name = "BritInvasionSosaria";
						spawner8.Respawn();

						Spawner spawner9 = new Spawner(5, 10, 20, 30, 20, "RunicGolemInvader");
						spawner9.MoveToWorld(new Point3D(1374, 1793, 2), Map.Sosaria);
						spawner9.Name = "BritInvasionSosaria";
						spawner9.Respawn();

						Spawner spawner10 = new Spawner(5, 10, 20, 30, 20, "RunicGolemInvader");
						spawner10.MoveToWorld(new Point3D(1345, 1751, 20), Map.Sosaria);
						spawner10.Name = "BritInvasionSosaria";
						spawner10.Respawn();

						Spawner spawner11 = new Spawner(7, 10, 20, 30, 25, "RunicGolemInvader");
						spawner11.MoveToWorld(new Point3D(1436, 1534, 35), Map.Sosaria);
						spawner11.Name = "BritInvasionSosaria";
						spawner11.Respawn();

						Spawner spawner12 = new Spawner(2, 1, 21, 30, 1, "MetalDaemon");
						spawner12.MoveToWorld(new Point3D(1523, 1513, 8), Map.Sosaria);
						spawner12.Name = "BritInvasionSosaria";
						spawner12.Respawn();

						Spawner spawner13 = new Spawner(5, 10, 20, 30, 10, "RunicGolemInvader");
						spawner13.MoveToWorld(new Point3D(1576, 1529, 39), Map.Sosaria);
						spawner13.Name = "BritInvasionSosaria";
						spawner13.Respawn();

						Spawner spawner14 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner14.MoveToWorld(new Point3D(1546, 1579, 20), Map.Sosaria);
						spawner14.Name = "BritInvasionSosaria";
						spawner14.Respawn();

						Spawner spawner15 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner15.MoveToWorld(new Point3D(1539, 1630, 0), Map.Sosaria);
						spawner15.Name = "BritInvasionSosaria";
						spawner15.Respawn();

						Spawner spawner16 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner16.MoveToWorld(new Point3D(1535, 1673, 20), Map.Sosaria);
						spawner16.Name = "BritInvasionSosaria";
						spawner16.Respawn();

						Spawner spawner17 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner17.MoveToWorld(new Point3D(1524, 1707, 20), Map.Sosaria);
						spawner17.Name = "BritInvasionSosaria";
						spawner17.Respawn();

						Spawner spawner18 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner18.MoveToWorld(new Point3D(1477, 1758, -2), Map.Sosaria);
						spawner18.Name = "BritInvasionSosaria";
						spawner18.Respawn();

						Spawner spawner19 = new Spawner(4, 10, 20, 30, 10, "RunicGolemInvader");
						spawner19.MoveToWorld(new Point3D(1441, 1763, -2), Map.Sosaria);
						spawner19.Name = "BritInvasionSosaria";
						spawner19.Respawn();

						Spawner spawner20 = new Spawner(2, 10, 10, 30, 10, "IronDragon");
						spawner20.MoveToWorld(new Point3D(1386, 1626, 30), Map.Sosaria);
						spawner20.Name = "BritInvasionSosaria";
						spawner20.Respawn();

						Spawner spawner21 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner21.MoveToWorld(new Point3D(1401, 1745, 2), Map.Sosaria);
						spawner21.Name = "BritInvasionSosaria";
						spawner21.Respawn();

						Spawner spawner29 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner29.MoveToWorld(new Point3D(1429, 1567, 30), Map.Sosaria);
						spawner29.Name = "BritInvasionSosaria";
						spawner29.Respawn();

						Spawner spawner22 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner22.MoveToWorld(new Point3D(1476, 1537, 30), Map.Sosaria);
						spawner22.Name = "BritInvasionSosaria";
						spawner22.Respawn();

						Spawner spawner23 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner23.MoveToWorld(new Point3D(1532, 1530, 40), Map.Sosaria);
						spawner23.Name = "BritInvasionSosaria";
						spawner23.Respawn();

						Spawner spawner24 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner24.MoveToWorld(new Point3D(1513, 1582, 20), Map.Sosaria);
						spawner24.Name = "BritInvasionSosaria";
						spawner24.Respawn();

						Spawner spawner25 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner25.MoveToWorld(new Point3D(1513, 1628, 10), Map.Sosaria);
						spawner25.Name = "BritInvasionSosaria";
						spawner25.Respawn();

						Spawner spawner26 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner26.MoveToWorld(new Point3D(1512, 1673, 20), Map.Sosaria);
						spawner26.Name = "BritInvasionSosaria";
						spawner26.Respawn();

						Spawner spawner27 = new Spawner(2, 10, 10, 30, 10, "MechGargoyle");
						spawner27.MoveToWorld(new Point3D(1502, 1705, 20), Map.Sosaria);
						spawner27.Name = "BritInvasionSosaria";
						spawner27.Respawn();

						Spawner spawner28 = new Spawner(5, 20, 20, 30, 20, "MechGargoyle");
						spawner28.MoveToWorld(new Point3D(1455, 1734, 0), Map.Sosaria);
						spawner28.Name = "BritInvasionSosaria";
						spawner28.Respawn();

						Spawner spawner30 = new Spawner(1, 1, 1, 30, 1, "IronDragon");
						spawner30.MoveToWorld(new Point3D(1523, 1476, 20), Map.Sosaria);
						spawner30.Name = "BritInvasionSosaria";
						spawner30.Respawn();

						Spawner spawner31 = new Spawner(2, 10, 10, 30, 10, "ExodusOverseer");
						spawner31.MoveToWorld(new Point3D(1420, 1697, 0), Map.Sosaria);
						spawner31.Name = "BritInvasionSosaria";
						spawner31.Respawn();

						Spawner spawner32 = new Spawner(2, 10, 10, 30, 10, "ExodusOverseer");
						spawner32.MoveToWorld(new Point3D(1419, 1626, 20), Map.Sosaria);
						spawner32.Name = "BritInvasionSosaria";
						spawner32.Respawn();

						Spawner spawner33 = new Spawner(30, 150, 150, 30, 150, "Golem");
						spawner33.MoveToWorld(new Point3D(1475, 1651, 10), Map.Sosaria);
						spawner33.Name = "BritInvasionSosaria";
						spawner33.Respawn();

						Spawner spawner34 = new Spawner(2, 10, 10, 30, 10, "ExodusOverseer");
						spawner34.MoveToWorld(new Point3D(1461, 1583, 20), Map.Sosaria);
						spawner34.Name = "BritInvasionSosaria";
						spawner34.Respawn();

						Spawner spawner35 = new Spawner(2, 10, 10, 30, 10, "ExodusOverseer");
						spawner35.MoveToWorld(new Point3D(1484, 1613, 20), Map.Sosaria);
						spawner35.Name = "BritInvasionSosaria";
						spawner35.Respawn();

						Spawner spawner36 = new Spawner(30, 150, 150, 30, 150, "Overseer");
						spawner36.MoveToWorld(new Point3D(1475, 1657, 10), Map.Sosaria);
						spawner36.Name = "BritInvasionSosaria";
						spawner36.Respawn();

						Spawner spawner37 = new Spawner(2, 5, 5, 30, 5, "ExodusOverseer");
						spawner37.MoveToWorld(new Point3D(1478, 1704, 20), Map.Sosaria);
						spawner37.Name = "BritInvasionSosaria";
						spawner37.Respawn();

						Spawner spawner38 = new Spawner(2, 5, 5, 30, 5, "ExodusOverseer");
						spawner38.MoveToWorld(new Point3D(1454, 1698, 20), Map.Sosaria);
						spawner38.Name = "BritInvasionSosaria";
						spawner38.Respawn();

						Spawner spawner39 = new Spawner(2, 5, 5, 30, 5, "ExodusOverseer");
						spawner39.MoveToWorld(new Point3D(1512, 1641, 20), Map.Sosaria);
						spawner39.Name = "BritInvasionSosaria";
						spawner39.Respawn();

						Spawner spawner40 = new Spawner(4, 5, 5, 30, 5, "RunicGolemInvader");
						spawner40.MoveToWorld(new Point3D(1523, 1453, 15), Map.Sosaria);
						spawner40.Name = "BritInvasionSosaria";
						spawner40.Respawn();

						Spawner spawner41 = new Spawner(2, 10, 10, 30, 4, "MechGargoyle");
						spawner41.MoveToWorld(new Point3D(1524, 1441, 15), Map.Sosaria);
						spawner41.Name = "BritInvasionSosaria";
						spawner41.Respawn();

						Spawner spawner42 = new Spawner(8, 10, 10, 30, 20, "ExodusOverseer");
						spawner42.MoveToWorld(new Point3D(1524, 1446, 15), Map.Sosaria);
						spawner42.Name = "BritInvasionSosaria";
						spawner42.Respawn();

						Spawner spawner43 = new Spawner(8, 10, 10, 30, 20, "ExodusOverseer");
						spawner43.MoveToWorld(new Point3D(1525, 1427, 15), Map.Sosaria);
						spawner43.Name = "BritInvasionSosaria";
						spawner43.Respawn();

						Spawner spawner44 = new Spawner(1, 1, 1, 30, 1, "IronDragon");
						spawner44.MoveToWorld(new Point3D(1518, 1428, 15), Map.Sosaria);
						spawner44.Name = "BritInvasionSosaria";
						spawner44.Respawn();

						Spawner spawner45 = new Spawner(2, 1, 21, 30, 1, "MetalDaemon");
						spawner45.MoveToWorld(new Point3D(1516, 1434, 15), Map.Sosaria);
						spawner45.Name = "BritInvasionSosaria";
						spawner45.Respawn();

						Spawner spawner46 = new Spawner(8, 1, 1, 30, 10, "ExodusOverseer");
						spawner46.MoveToWorld(new Point3D(1529, 1427, 35), Map.Sosaria);
						spawner46.Name = "BritInvasionSosaria";
						spawner46.Respawn();

						Spawner spawner47 = new Spawner(2, 1, 1, 30, 10, "MetalDaemon");
						spawner47.MoveToWorld(new Point3D(1525, 1418, 35), Map.Sosaria);
						spawner47.Name = "BritInvasionSosaria";
						spawner47.Respawn();

						Spawner spawner48 = new Spawner(2, 5, 5, 30, 5, "RunicGolemInvader");
						spawner48.MoveToWorld(new Point3D(1526, 1417, 15), Map.Sosaria);
						spawner48.Name = "BritInvasionSosaria";
						spawner48.Respawn();

						Spawner spawner49 = new Spawner(1, 1, 1, 30, 1, "IronDragon");
						spawner49.MoveToWorld(new Point3D(1524, 1415, 56), Map.Sosaria);
						spawner49.Name = "BritInvasionSosaria";
						spawner49.Respawn();

						Spawner spawner50 = new Spawner(6, 1, 1, 30, 10, "RunicGolemInvader");
						spawner50.MoveToWorld(new Point3D(1526, 1417, 55), Map.Sosaria);
						spawner50.Name = "BritInvasionSosaria";
						spawner50.Respawn();

						Spawner spawner51 = new Spawner(6, 1, 1, 30, 10, "ExodusOverseer");
						spawner51.MoveToWorld(new Point3D(1529, 1427, 55), Map.Sosaria);
						spawner51.Name = "BritInvasionSosaria";
						spawner51.Respawn();

						Spawner spawner52 = new Spawner(1, 1, 1, 30, 1, "IronDragon");
						spawner52.MoveToWorld(new Point3D(1530, 1423, 55), Map.Sosaria);
						spawner52.Name = "BritInvasionSosaria";
						spawner52.Respawn();

						Spawner spawner53 = new Spawner(2, 1, 1, 30, 1, "MechGargoyle");
						spawner53.MoveToWorld(new Point3D(1533, 1415, 56), Map.Sosaria);
						spawner53.Name = "BritInvasionSosaria";
						spawner53.Respawn();

						Spawner spawner54 = new Spawner(1, 1, 1, 30, 1, "BlackthornClone");
						spawner54.MoveToWorld(new Point3D(1540, 1415, 55), Map.Sosaria);
						spawner54.Name = "BritInvasionSosaria";
						spawner54.Respawn();

						Spawner spawner55 = new Spawner(4, 5, 5, 30, 5, "RunicGolemInvader");
						spawner55.MoveToWorld(new Point3D(1537, 1516, 32), Map.Sosaria);
						spawner55.Name = "BritInvasionSosaria";
						spawner55.Respawn();
						//<---------
						Spawner spawner56 = new Spawner(1, 1, 1, 30, 1, "IronDragon");
						spawner56.MoveToWorld(new Point3D(1517, 1433, -25), Map.Sosaria);
						spawner56.Name = "BritInvasionSosaria";
						spawner56.Respawn();

						Spawner spawner57 = new Spawner(3, 2, 2, 30, 2, "ExodusOverseer");
						spawner57.MoveToWorld(new Point3D(1517, 1433, -25), Map.Sosaria);
						spawner57.Name = "BritInvasionSosaria";
						spawner57.Respawn();

						Spawner spawner58 = new Spawner(4, 10, 10, 30, 10, "MetalDaemon");
						spawner58.MoveToWorld(new Point3D(6045, 1400, 30), Map.Sosaria);
						spawner58.Name = "BritInvasionSosaria";
						spawner58.Respawn();

						// Broadcasting a message to all players about the invasion.
						World.Broadcast(33, true, "Britian Sosaria is under invasion.");

						// Sending a follow-up Gump to the player
						from.SendGump(new CityInvasion(from));
						break;
					}

				// Case when the "Stop an Invasion" button is clicked
				case 2:
					{
						// Creating a new BritInvasionStone object to manage the stopping of the invasion.
						BritInvasionStone brittram = new BritInvasionStone();

						// Calling the StopBritSosaria method on the brittram object.
						// This method is responsible for halting the invasion, likely by removing NPCs and other invasion-related objects.
						brittram.StopBritSosaria();

						// Broadcasting a message to all players indicating that the invasion has been successfully repelled.
						World.Broadcast(
							33,
							true,
							"Britian Sosaria's invasion was successfully beaten back. No more invaders are left in the city."
						);

						// Reopening the CityInvasion Gump for the player, allowing them to manage further invasion-related actions.
						from.SendGump(new CityInvasion(from));
						break;
					}
			}
		}
	}
}
