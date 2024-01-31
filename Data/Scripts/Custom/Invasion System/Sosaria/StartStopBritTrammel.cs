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

		// CreateEntry method is responsible for creating waypoints and spawners for invasions
		public void CreateEntry(List<Point3D> waypointCoordinates, string waypointName, int amount, int minDelay, int maxDelay, int team, int homeRange, string creatureToSpawn)
		{
			// Debugging
			Console.WriteLine("[CreateEntry] Called for: " + waypointName); // Debugging

			if (waypointCoordinates == null || waypointCoordinates.Count == 0)
			{
				Console.WriteLine("[CreateEntry] No coordinates provided for: " + waypointName); // Debugging
				return;
			}

			foreach (var coord in waypointCoordinates) // Debugging
			{
				Console.WriteLine("[CreateEntry] Coordinate: " + coord.ToString());
			}

			// Validate the waypointCoordinates list before proceeding
			if (waypointCoordinates == null || waypointCoordinates.Count == 0)
				return;

			List<WayPoint> waypoints = new List<WayPoint>();

			// Check if there are additional waypoints beyond the spawner location
			if (waypointCoordinates.Count > 1)
			{
				// Start from the second coordinate for WayPoints
				for (int i = 1; i < waypointCoordinates.Count; i++)
				{
					WayPoint wp = new WayPoint();
					wp.Name = waypointName;
					wp.MoveToWorld(waypointCoordinates[i], Map.Sosaria);
					waypoints.Add(wp);
				}

				// Linking each waypoint to the next to create a path
				for (int i = 0; i < waypoints.Count; i++)
				{
					waypoints[i].NextPoint = waypoints[(i + 1) % waypoints.Count];
				}
			}

			// Creating a spawner at the first coordinate
			Spawner spawner = new Spawner(amount, minDelay, maxDelay, team, homeRange, creatureToSpawn);
			spawner.MoveToWorld(waypointCoordinates[0], Map.Sosaria);
    
			// Link the spawner to the first waypoint if waypoints are available
			if (waypoints.Count > 0)
			{
				spawner.WayPoint = waypoints[0];
			}
    
			spawner.Name = waypointName;
			spawner.Respawn();

			// Debugging
			Console.WriteLine("[CreateEntry] Spawner created at " + waypointCoordinates[0].ToString() + " for " + creatureToSpawn);
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
				// Handling the 'Start an Invasion' button
				case 1:
					{
						// Define the central location for the invasion
						Point3D loc = new Point3D(2999, 1053, 0);

						#region Entry 1 - West Gate Overseers
						// Coordinates for this entry's invasion path
						List<Point3D> entry1Coordinates = new List<Point3D>
						{
							new Point3D(2914, 1068, 0), // Spawner Location
							new Point3D(2929, 1063, 0),
							new Point3D(2941, 1064, 0),
							new Point3D(2956, 1068, 0),
							new Point3D(2970, 1072, 0),
							new Point3D(2985, 1068, 0),
							new Point3D(3000, 1064, 0)
						};

						// Creating the invasion entry
						CreateEntry(entry1Coordinates, "BritInvasionSosaria", 2, 5, 10, 30, 20, "Overseer");
						#endregion

						#region Entry 2 - South Gate Overseers
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

						#region Entry 3 - East Gate Overseers
						// Coordinates for this entry's invasion path
						List<Point3D> entry3Coordinates = new List<Point3D>
						{
							new Point3D(3068, 1059, 0), // Spawner Location
							new Point3D(3052, 1060, 0),
							new Point3D(3035, 1061, 0),
							new Point3D(3018, 1062, 0),
							new Point3D(3003, 1061, 0)
						};

						// Creating the invasion entry
						CreateEntry(entry3Coordinates, "BritInvasionSosaria", 2, 5, 10, 30, 20, "Overseer");
						#endregion

						#region Entry 4 - Central Well Iron Dragons
						// Coordinates for this entry's spawner point
						List<Point3D> entry4Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry4Coordinates, "BritInvasionSosaria", 2, 5, 10, 30, 10, "IronDragon");
						#endregion


						#region Entry 5 - North Central IronDragon
						// Coordinates for this entry's spawner point
						List<Point3D> entry5Coordinates = new List<Point3D>
						{
							new Point3D(2996, 1002, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry5Coordinates, "BritInvasionSosaria", 1, 5, 10, 30, 10, "IronDragon");
						#endregion

						#region Entry 6 - West Central IronDragon
						// Coordinates for this entry's spawner point
						List<Point3D> entry6Coordinates = new List<Point3D>
						{
							new Point3D(2947, 1075, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry6Coordinates, "BritInvasionSosaria", 1, 5, 10, 30, 10, "IronDragon");
						#endregion

						#region Entry 7 - East Central IronDragon
						// Coordinates for this entry's spawner point
						List<Point3D> entry7Coordinates = new List<Point3D>
						{
							new Point3D(3041, 1068, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry7Coordinates, "BritInvasionSosaria", 1, 5, 10, 30, 10, "IronDragon");
						#endregion

						#region Entry 8 - Inside the Walls Overseers
						// Coordinates for this entry's spawner point
						List<Point3D> entry8Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry8Coordinates, "BritInvasionSosaria", 30, 5, 10, 30, 60, "Overseer");
						#endregion

						#region Entry 9 - Inside the Walls RunicGolems
						// Coordinates for this entry's spawner point
						List<Point3D> entry9Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry9Coordinates, "BritInvasionSosaria", 6, 5, 10, 30, 60, "RunicGolemInvader");
						#endregion

						#region Entry 10 - Inside the Walls MetalDaemons
						// Coordinates for this entry's spawner point
						List<Point3D> entry10Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry10Coordinates, "BritInvasionSosaria", 6, 5, 10, 30, 60, "MetalDaemon");
						#endregion

						#region Entry 11 - Inside the Walls MechGargoyles
						// Coordinates for this entry's spawner point
						List<Point3D> entry11Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry11Coordinates, "BritInvasionSosaria", 15, 5, 10, 30, 60, "MechGargoyle");
						#endregion

						#region Entry 12 - Boss Central Well
						// Coordinates for this entry's spawner point
						List<Point3D> entry12Coordinates = new List<Point3D>
						{
							new Point3D(2999, 1062, 0) // Spawner Location
						};

						// Creating the invasion entry
						CreateEntry(entry12Coordinates, "BritInvasionSosaria", 1, 1, 1, 30, 12, "LordBlackThornBot");
						#endregion

						World.Broadcast(33, true, "Britian Sosaria is under invasion.");
						from.SendGump(new CityInvasion(from));
						break;
					}
				// Handling the 'Stop an Invasion' button
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
