using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using Server;
using Server.Items;
using Server.Multis;
using Server.Engines.CannedEvil;
using Server.Custom.Confictura.Champions;

internal static class LoadedReservationPreflight
{
    private sealed class ProposedTerrain : IArachnidOutbreakTerrain
    {
        private readonly SosariaOutbreakTerrain m_Terrain = new SosariaOutbreakTerrain();
        public bool HasPermanentHousingProhibition(Rectangle2D bounds) { return true; }
        public bool IsReserved(Rectangle2D bounds) { return m_Terrain.IsReserved(bounds); }
        public bool TryGetPoint(int x, int y, out Point3D point) { return m_Terrain.TryGetPoint(x, y, out point); }
    }

    private static void CoreField(string name, object value)
    {
        FieldInfo field = typeof(Core).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null) throw new InvalidOperationException("Missing bootstrap field: " + name);
        field.SetValue(null, value);
    }

    private static bool Overlap(Rectangle2D a, Rectangle2D b)
    {
        return a.Width > 0 && a.Height > 0 && b.Width > 0 && b.Height > 0
            && a.X < b.X + b.Width && a.X + a.Width > b.X
            && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y;
    }

    private static void Attribute(XmlWriter writer, string name, object value)
    {
        writer.WriteAttributeString(name, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
    }

    public static int Main(string[] args)
    {
        try
        {
            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!root.StartsWith(@"D:\ConficturaUOTestTools\outbreak-reserve-preflight-", StringComparison.OrdinalIgnoreCase)
                || args.Length != 1 || !File.Exists(Path.Combine(root, "preflight-inputs.json"))
                || !Directory.Exists(Path.Combine(root, "Saves")))
                throw new InvalidOperationException("Requires a prepared, exact private preflight root.");
            string assets = Path.GetFullPath(args[0]);
            if (!assets.StartsWith(@"D:\ConficturaUOTestTools\", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Map assets must be in the private test workspace.");
            Directory.SetCurrentDirectory(root);
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs request)
            {
                string path = Path.Combine(root, new AssemblyName(request.Name).Name + ".dll");
                return File.Exists(path) ? Assembly.LoadFrom(path) : null;
            };
            Core.Assembly = typeof(Core).Assembly;
            CoreField("m_BaseDirectory", root);
            CoreField("m_ExePath", typeof(Core).Assembly.Location);
            CoreField("m_Process", Process.GetCurrentProcess());
            CoreField("m_Thread", Thread.CurrentThread);
            CoreField("m_MultiConOut", new MultiTextWriter(Console.Out));
            Core.DataDirectories.Insert(0, assets);
            ScriptCompiler.Assemblies = new Assembly[] { Assembly.LoadFrom(Path.Combine(root, "Data.dll")) };
            ScriptCompiler.Invoke("Configure");
            // Match core startup's Configure -> Region.Load -> World.Load order.
            // Initialize, listener startup, the timer thread and saves are never invoked.
            typeof(Region).GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            int staticRegions = 0;
            foreach (Region region in Region.Regions)
                if (region.Map == Map.Sosaria && !region.Dynamic) ++staticRegions;
            if (staticRegions == 0) throw new InvalidOperationException("Static Sosaria regions were not loaded.");
            World.Load();
            int malformed = 0, activeMalformed = 0, accepted = 0, total = 0;
            foreach (Item item in World.Items.Values)
            {
                ChampionSpawn champion = item as ChampionSpawn;
                if (champion != null && champion.Map == Map.Sosaria && champion.X < 5120 && champion.Y < 3128
                    && (champion.SpawnArea.Width <= 0 || champion.SpawnArea.Height <= 0))
                {
                    ++malformed;
                    if (champion.Active) ++activeMalformed;
                }
            }
            string output = Path.Combine(root, "preflight.xml");
            if (File.Exists(output)) throw new InvalidOperationException("Never overwrite a prior preflight result.");
            using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("preflight");
                Attribute(writer, "staticSosariaRegions", staticRegions);
                Attribute(writer, "malformedLegacyControllers", malformed);
                Attribute(writer, "activeMalformedLegacyControllers", activeMalformed);
                Attribute(writer, "hypotheticalReservationsOnly", true);
                Attribute(writer, "productionActivation", ArachnidOutbreakController.DeliveryReady);
                foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
                {
                    if (reserve.Reviewed) throw new InvalidOperationException("Preflight must not approve reservations.");
                    ++total;
                    int houses = 0, cities = 0, champions = 0, portals = 0;
                    foreach (Item item in World.Items.Values)
                    {
                        if (item.Deleted || item.Map != Map.Sosaria) continue;
                        BaseHouse house = item as BaseHouse;
                        if (house != null)
                            foreach (Rectangle2D area in house.Area)
                                if (Overlap(reserve.Bounds, new Rectangle2D(house.X + area.X, house.Y + area.Y, area.Width, area.Height))) ++houses;
                        CityManagementStone city = item as CityManagementStone;
                        if (city != null && Overlap(reserve.Bounds, PlayerGovernmentSystem.GetMaximumCityBounds(city.Center))) ++cities;
                        ChampionSpawn champion = item as ChampionSpawn;
                        if (champion != null && Overlap(reserve.Bounds, champion.SpawnArea)) ++champions;
                        if (item.Parent == null && SosariaOutbreakTerrain.IsPortal(item) && reserve.Bounds.Contains(new Point2D(item.X, item.Y))) ++portals;
                    }
                    ArachnidOutbreakPlacement placement;
                    string reason;
                    bool valid = new ArachnidOutbreakSite(reserve.Id, reserve.Center.X, reserve.Center.Y, false)
                        .Validate(new ProposedTerrain(), out placement, out reason);
                    bool clear = valid && houses == 0 && cities == 0 && champions == 0 && portals == 0;
                    if (clear) ++accepted;
                    writer.WriteStartElement("reserve");
                    Attribute(writer, "id", reserve.Id);
                    Attribute(writer, "layoutAndClearance", valid);
                    Attribute(writer, "housesAllElevations", houses);
                    Attribute(writer, "maximumCityGrowth", cities);
                    Attribute(writer, "validLegacyAreas", champions);
                    Attribute(writer, "portalItems", portals);
                    Attribute(writer, "connectedTiles", valid ? placement.WalkableCount : 0);
                    Attribute(writer, "reason", reason);
                    Attribute(writer, "passed", clear);
                    writer.WriteEndElement();
                    Console.WriteLine("RESERVE {0} passed={1} houses={2} cities={3} legacy={4} portals={5}", reserve.Id, clear, houses, cities, champions, portals);
                }
                writer.WriteEndElement();
            }
            Console.WriteLine("PREFLIGHT {0}/{1}; static regions={2}; malformed inactive={3}; active malformed={4}", accepted, total, staticRegions, malformed - activeMalformed, activeMalformed);
            return accepted == 6 && total == 6 && activeMalformed == 0 && !ArachnidOutbreakController.DeliveryReady ? 0 : 1;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 2;
        }
    }
}
