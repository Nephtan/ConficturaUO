using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
namespace Server.Gumps
{
	public class StartStopBrittram : Gump
	{
		private Mobile m_Mobile;
		public StartStopBrittram(Mobile from)
			: base(0, 0)
		{
			m_Mobile = from;

			// Setting up the Gump properties
			Closable = false;
			Dragable = true;

			// Adding UI elements to the Gump
			AddPage(0);
			AddImage(112, 73, 39);
			AddButton(135, 123, 9804, 9806, 1, GumpButtonType.Reply, 1);
			AddButton(138, 194, 9804, 9806, 2, GumpButtonType.Reply, 2);
			AddButton(277, 311, 2453, 2455, 0, GumpButtonType.Reply, 0);
			AddLabel(216, 140, 0, "Start an Invasion");
			AddLabel(218, 208, 0, "Stop an Invasion");
		}

		// CreateEntry method is responsible for creating waypoints and a spawner for invasions
		public void CreateEntry(List<Point3D> waypointCoordinates, string waypointName, int amount, int minDelay, int maxDelay, int team, int homeRange, string creatureToSpawn)
		{
			// Validate the waypointCoordinates list before proceeding
			if (waypointCoordinates == null || waypointCoordinates.Count == 0)
				return;

			// Creating waypoints and setting their location and name
			List<WayPoint> waypoints = new List<WayPoint>();
			foreach (var coord in waypointCoordinates)
			{
				WayPoint wp = new WayPoint();
				wp.Name = waypointName;
				wp.MoveToWorld(coord, Map.Sosaria);
				waypoints.Add(wp);
			}

			// Linking each waypoint to the next to create a path
			for (int i = 0; i < waypoints.Count; i++)
			{
				waypoints[i].NextPoint = waypoints[(i + 1) % waypoints.Count];
			}

			// Creating a spawner at the first waypoint
			Spawner spawner = new Spawner(amount, minDelay, maxDelay, team, homeRange, creatureToSpawn);
			spawner.MoveToWorld(waypointCoordinates[0], Map.Sosaria);
			spawner.WayPoint = waypoints[0];
			spawner.Name = waypointName;
			spawner.Respawn();
		}

		// Handling responses from the Gump's buttons
		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;
			switch (info.ButtonID)
			{
				// Handling the 'Close' button
				case 0:
					{
						from.CloseGump(typeof(StartStopBrittram));
						from.SendGump(new CityInvasion(from));
						break;
					}
				// Handling the 'Start Invasion' button
				case 1:
					{
						// Define the central location for the invasion
						Point3D loc = new Point3D(2999, 1053, 0);

						#region Entry 1
						// Coordinates for this entry's invasion path
						List<Point3D> entry1Coordinates = new List<Point3D>
						{
							new Point3D(2914, 1068, 0), // Spawner Location
							new Point3D(2928, 1066, 0),
							new Point3D(2941, 1064, 0),
							new Point3D(2956, 1068, 0),
							new Point3D(2970, 1072, 0),
							new Point3D(2985, 1068, 0),
							new Point3D(3000, 1064, 0)
						};

						// Creating the invasion entry
						CreateEntry(entry1Coordinates, "BritInvasionSosaria", 2, 5, 10, 30, 20, "Overseer");
						#endregion

						#region Entry 2
						// Coordinates for this entry's invasion path
						List<Point3D> entry2Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1139, 0), // Spawner Location
							new Point3D(3000, 1126, 0),
							new Point3D(3000, 1113, 0),
							new Point3D(3001, 1100, 0),
							new Point3D(3000, 1083, 0),
							new Point3D(2999, 1066, 0),
							new Point3D(2999, 1054, 0),
							new Point3D(3000, 1041, 0),
							new Point3D(3000, 1029, 0)
						};

						// Creating the invasion entry
						CreateEntry(entry2Coordinates, "BritInvasionSosaria", 2, 5, 10, 30, 20, "Overseer");
						#endregion

						World.Broadcast(33, true, "Britian Sosaria is under invasion.");
						from.SendGump(new CityInvasion(from));
						break;
					}
				case 2:
					{
						BritInvasionStone brittram = new BritInvasionStone();
						brittram.StopBritSosaria();
						World.Broadcast(
							33,
							true,
							"Britian Sosaria's invasion was successfully beaten back. No more invaders are left in the city."
						);
						from.SendGump(new CityInvasion(from));
						break;
					}
			}
		}
	}
}
