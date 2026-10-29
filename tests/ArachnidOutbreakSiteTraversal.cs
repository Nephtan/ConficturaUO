using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    // Disposable copied-world travel fixture. It never moves the actor along a route.
    public static class ArachnidOutbreakSiteTraversal
    {
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "../evidence"));
        private static readonly List<Route> Routes = new List<Route>();
        private static readonly List<Item> Markers = new List<Item>();
        private static PlayerMobile Actor;
        private static Timer Poll;
        private static DateTime Deadline;
        private static Route Active;
        private static Point3D Last;
        private static int Progress;
        private static int Deviations;
        private static bool Stopping;

        private sealed class Route
        {
            public string Id;
            public ArachnidOutbreakSite Site;
            public List<Point3D> Points = new List<Point3D>();
            public Dictionary<int, string> Checkpoints = new Dictionary<int, string>();
        }

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard = Path.Combine(Evidence, "native-private-root.txt");
            if (!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'), Core.BaseDirectory.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Site traversal requires its exact disposable-root guard.");
            AutoSave.SavesEnabled = false;
            Deadline = DateTime.UtcNow.AddMinutes(45);
            Timer.DelayCall(TimeSpan.FromSeconds(1), Start);
        }

        private static void Start()
        {
            IAccount account = Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence, "account-name.private")));
            if (account == null) throw new InvalidOperationException("Copied account missing.");
            for (int i = 0; i < 7; ++i)
                if (account[i] is PlayerMobile) { Actor = (PlayerMobile)account[i]; break; }
            if (Actor == null || !Actor.Alive || Actor.Mounted || Actor.CanSwim)
                throw new InvalidOperationException("Survey requires a living unmounted ordinary walking Player.");
            Actor.AccessLevel = AccessLevel.Player;
            Actor.Blessed = true;
            Actor.Hidden = false;
            Actor.Warmode = false;
            Actor.Combatant = null;
            SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
            string plans = "site,step,x,y,z,direction,checkpoint\n";
            foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
            {
                if (reserve.Reviewed) throw new InvalidOperationException("This fixture expects inactive proposals.");
                ArachnidOutbreakSite site = new ArachnidOutbreakSite(reserve.Id, reserve.Center.X, reserve.Center.Y, false);
                if (terrain.HasStructuralConflict(site.ClearanceBounds))
                    throw new InvalidOperationException("Copied-world structural conflict at " + reserve.Id);
                Route route = Build(site, terrain);
                Routes.Add(route);
                for (int i = 0; i < route.Points.Count; ++i)
                {
                    Point3D p = route.Points[i];
                    string checkpoint;
                    route.Checkpoints.TryGetValue(i, out checkpoint);
                    int direction = i == 0 ? 0 : (int)DirectionBetween(route.Points[i - 1], p);
                    plans += reserve.Id + "," + i + "," + p.X + "," + p.Y + "," + p.Z + "," + direction + "," + checkpoint + "\n";
                }
            }
            File.WriteAllText(Path.Combine(Evidence, "site-routes.csv"), plans);
            File.WriteAllText(Path.Combine(Evidence, "site-events.csv"), "utc,site,event,step,x,y,z,connected\n");
            File.WriteAllText(Path.Combine(Evidence, "native-ready.txt"), "Six planned walking routes; travel-only Player is blessed. No encounter, production activation or site approval.");
            Poll = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromMilliseconds(50), Tick);
            Select(0);
        }

        private static Route Build(ArachnidOutbreakSite site, SosariaOutbreakTerrain terrain)
        {
            Dictionary<Point2D, Point3D> ground = new Dictionary<Point2D, Point3D>();
            Rectangle2D bounds = site.Bounds;
            for (int x = bounds.X; x < bounds.X + bounds.Width; ++x)
                for (int y = bounds.Y; y < bounds.Y + bounds.Height; ++y)
                {
                    Point3D p;
                    if (terrain.TryGetPoint(x, y, out p)) ground.Add(new Point2D(x, y), p);
                }
            Point3D center = ground[site.Center];
            Dictionary<Point2D, Point2D> connected = Search(center, ground);
            if (connected.Count < 4000) throw new InvalidOperationException("Too little actual movement connectivity at " + site.Id);
            Point3D north = Exit(site, -ArachnidOutbreakSite.Radius, ground, connected);
            Point3D south = Exit(site, ArachnidOutbreakSite.Radius, ground, connected);
            Point3D[] goals = new Point3D[] {
                north, ground[new Point2D(center.X - 28, center.Y - 20)],
                ground[new Point2D(center.X + 28, center.Y - 20)], center,
                ground[new Point2D(center.X, center.Y + 30)], south };
            string[] names = new string[] { "north-entrance", "brood-0", "brood-1", "boss", "brood-2", "south-exit" };
            Route route = new Route(); route.Id = site.Id; route.Site = site;
            route.Points.Add(north); route.Checkpoints.Add(0, names[0]);
            for (int i = 1; i < goals.Length; ++i)
            {
                List<Point3D> leg = FindPath(goals[i - 1], goals[i], ground);
                route.Points.AddRange(leg);
                route.Checkpoints.Add(route.Points.Count - 1, names[i]);
            }
            File.AppendAllText(Path.Combine(Evidence, "site-planning.log"), site.Id + " movement_connected=" + connected.Count + " steps=" + (route.Points.Count - 1) + "\n");
            return route;
        }

        private static Dictionary<Point2D, Point2D> Search(Point3D start, Dictionary<Point2D, Point3D> ground)
        {
            Dictionary<Point2D, Point2D> previous = new Dictionary<Point2D, Point2D>();
            Queue<Point3D> queue = new Queue<Point3D>();
            Point2D first = new Point2D(start.X, start.Y);
            previous.Add(first, first); queue.Enqueue(start);
            while (queue.Count != 0)
            {
                Point3D current = queue.Dequeue();
                for (int d = 0; d < 8; d += 2)
                {
                    int x = current.X, y = current.Y, z;
                    Server.Movement.Movement.Offset((Direction)d, ref x, ref y);
                    Point2D key = new Point2D(x, y);
                    Point3D next;
                    if (previous.ContainsKey(key) || !ground.TryGetValue(key, out next)
                        || !Server.Movement.Movement.CheckMovement(Actor, Map.Sosaria, current, (Direction)d, out z) || z != next.Z)
                        continue;
                    previous.Add(key, new Point2D(current.X, current.Y)); queue.Enqueue(next);
                }
            }
            return previous;
        }

        private static Point3D Exit(ArachnidOutbreakSite site, int offset, Dictionary<Point2D, Point3D> ground, Dictionary<Point2D, Point2D> connected)
        {
            for (int distance = 0; distance <= ArachnidOutbreakSite.Radius; ++distance)
                for (int side = -1; side <= 1; side += 2)
                {
                    Point2D key = new Point2D(site.Center.X + distance * side, site.Center.Y + offset);
                    if (connected.ContainsKey(key)) return ground[key];
                }
            throw new InvalidOperationException("No reachable north/south exit at " + site.Id);
        }

        private static List<Point3D> FindPath(Point3D start, Point3D end, Dictionary<Point2D, Point3D> ground)
        {
            Dictionary<Point2D, Point2D> previous = Search(start, ground);
            Point2D key = new Point2D(end.X, end.Y), first = new Point2D(start.X, start.Y);
            if (!previous.ContainsKey(key)) throw new InvalidOperationException("No bidirectionally usable walking leg.");
            List<Point3D> result = new List<Point3D>();
            while (key != first) { result.Add(ground[key]); key = previous[key]; }
            result.Reverse(); return result;
        }

        private static Direction DirectionBetween(Point3D from, Point3D to)
        {
            if (to.X == from.X && to.Y == from.Y - 1) return Direction.North;
            if (to.X == from.X + 1 && to.Y == from.Y) return Direction.East;
            if (to.X == from.X && to.Y == from.Y + 1) return Direction.South;
            if (to.X == from.X - 1 && to.Y == from.Y) return Direction.West;
            throw new InvalidOperationException("Walking route contains a non-cardinal jump.");
        }

        private static void Select(int index)
        {
            if (index < 0 || index >= Routes.Count) throw new ArgumentOutOfRangeException("index");
            foreach (Item marker in Markers) marker.Delete(); Markers.Clear();
            Active = Routes[index]; Progress = 0; Deviations = 0;
            // Relocation is setup only. There is no route-following Move/MoveToWorld callback.
            Actor.Direction = Direction.North;
            Actor.MoveToWorld(Active.Points[0], Map.Sosaria); Last = Actor.Location;
            foreach (KeyValuePair<int, string> checkpoint in Active.Checkpoints)
            {
                Item marker = new Item(0x1F14); marker.Name = "PRIVATE " + checkpoint.Value; marker.Movable = false;
                marker.MoveToWorld(Active.Points[checkpoint.Key], Map.Sosaria); Markers.Add(marker);
            }
            Record("scripted-site-start");
        }

        private static void Record(string kind)
        {
            File.AppendAllText(Path.Combine(Evidence, "site-events.csv"), DateTime.UtcNow.ToString("o") + "," + Active.Id + "," + kind + "," + Progress + "," + Actor.X + "," + Actor.Y + "," + Actor.Z + "," + (Actor.NetState != null) + "\n");
        }

        private static void Stop()
        {
            Stopping = true; Poll.Stop();
            foreach (Item marker in Markers) marker.Delete();
            File.WriteAllText(Path.Combine(Evidence, "site-forced-stop.txt"), DateTime.UtcNow.ToString("o") + " forced no-save teardown");
            Core.Kill(false);
        }

        private static void Tick()
        {
            if (Stopping) return;
            if (DateTime.UtcNow >= Deadline) { Stop(); return; }
            string request = Path.Combine(Evidence, "native-request.txt");
            if (File.Exists(request))
            {
                string command;
                try { command = File.ReadAllText(request).Trim(); File.Delete(request); }
                catch (IOException) { return; }
                if (command == "stop") { Stop(); return; }
                if (command.StartsWith("site-")) Select(Int32.Parse(command.Substring(5)));
            }
            if (Actor.Location != Last)
            {
                if (Progress + 1 < Active.Points.Count && Actor.Location == Active.Points[Progress + 1])
                {
                    ++Progress;
                    string checkpoint;
                    Record(Active.Checkpoints.TryGetValue(Progress, out checkpoint) ? checkpoint : "walk-step");
                }
                else { ++Deviations; Record("off-planned-route"); }
                Last = Actor.Location;
            }
            File.WriteAllText(Path.Combine(Evidence, "site-state.json"), "{\"utc\":\"" + DateTime.UtcNow.ToString("o") + "\",\"site\":\"" + Active.Id + "\",\"progress\":" + Progress + ",\"steps\":" + (Active.Points.Count - 1) + ",\"deviations\":" + Deviations + ",\"x\":" + Actor.X + ",\"y\":" + Actor.Y + ",\"z\":" + Actor.Z + ",\"connected\":" + (Actor.NetState != null).ToString().ToLowerInvariant() + ",\"playerAccess\":" + (Actor.AccessLevel == AccessLevel.Player).ToString().ToLowerInvariant() + ",\"blessedTravelOnly\":" + Actor.Blessed.ToString().ToLowerInvariant() + "}");
        }
    }
}
