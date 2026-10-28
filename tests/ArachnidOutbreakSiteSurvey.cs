using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml;
using Server;
using Server.Misc;
using Server.Regions;
using Server.Multis;
using System.Collections;
using Server.Custom.Confictura.Champions;

// Static map/region survey only. No saves, listeners, timers or client are started.
internal static class ArachnidOutbreakSiteSurvey
{
    private static int LoadRegions(XmlElement parentXml, Region parent)
    {
        int count = 0;
        foreach (XmlElement element in parentXml.SelectNodes("region"))
        {
            Type type = typeof(BaseRegion);
            if (element.HasAttribute("type") && !Region.ReadType(element, "type", ref type, true))
                throw new Exception("Unknown region type: " + element.GetAttribute("type"));
            Region region = (Region)Activator.CreateInstance(type, new object[] { element, Map.Sosaria, parent });
            region.Register();
            ++count;
            count += LoadRegions(element, region);
        }
        return count;
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            ScriptCompiler.Assemblies = new Assembly[] { typeof(BaseRegion).Assembly };
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            BaseRegion.Configure();
            byte[] xmlBytes = File.ReadAllBytes(args[1]);
            using (SHA256 sha = SHA256.Create())
                Console.WriteLine("REGIONS_SHA256 " + BitConverter.ToString(sha.ComputeHash(xmlBytes)).Replace("-", "").ToLowerInvariant());
            XmlDocument document = new XmlDocument();
            using (MemoryStream stream = new MemoryStream(xmlBytes))
                document.Load(stream);
            XmlElement facet = (XmlElement)document.SelectSingleNode("/ServerRegions/Facet[@name='Sosaria']");
            if (facet == null)
                throw new Exception("Missing Sosaria facet.");
            Console.WriteLine("REGIONS_LOADED " + LoadRegions(facet, null));

            SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
            if (args.Length > 2 && args[2] == "retired")
            {
                Point2D[] retired = new Point2D[]
                {
                    new Point2D(914, 716), new Point2D(2258, 588), new Point2D(3794, 844),
                    new Point2D(1106, 1768), new Point2D(2514, 2152), new Point2D(3858, 2600)
                };
                foreach (Point2D point in retired)
                {
                    ArachnidOutbreakSite site = new ArachnidOutbreakSite("retired", point.X, point.Y, false);
                    ArachnidOutbreakPlacement placement;
                    string reason;
                    if (site.Validate(terrain, out placement, out reason)
                        || reason != "No permanent housing prohibition across the footprint and buffer.")
                        throw new Exception("Housing-eligible retired site accepted or failed for the wrong reason: " + point);
                    Console.WriteLine("RETIRED " + point + " " + reason);
                }
                if (ArachnidOutbreakSites.Candidates.Count != 0)
                    throw new Exception("Replacement catalog needs reviewed non-housing candidates.");
                Console.WriteLine("PASS retired-site housing exclusion: all six candidates rejected; no replacement site is claimed.");
                return 0;
            }
            int found = 0;
            for (int sectorY = 0; sectorY < 2; ++sectorY)
                for (int sectorX = 0; sectorX < 3; ++sectorX)
                {
                    int midX = 850 + sectorX * 1600;
                    int midY = 780 + sectorY * 1500;
                    bool selected = false;
                    for (int radius = 0; radius <= 10 && !selected; ++radius)
                        for (int dy = -radius; dy <= radius && !selected; ++dy)
                            for (int dx = -radius; dx <= radius && !selected; ++dx)
                            {
                                if (radius > 0 && Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                                    continue;
                                int x = midX + dx * 64;
                                int y = midY + dy * 64;
                                Point3D candidate;
                                if (x < 81 || x > 5038 || y < 81 || y > 3046 || !terrain.TryGetPoint(x, y, out candidate))
                                    continue;
                                string id = "sosaria-" + sectorX + "-" + sectorY;
                                ArachnidOutbreakSite site = new ArachnidOutbreakSite(id, x, y, false);
                                ArachnidOutbreakPlacement placement;
                                string reason;
                                if (!site.Validate(terrain, out placement, out reason))
                                    continue;
                                Console.WriteLine("CANDIDATE " + id + " " + placement.Boss + " connected=" + placement.WalkableCount);
                                Mobile applicant = new Mobile();
                                try
                                {
                                    applicant.AccessLevel = AccessLevel.Player;
                                    applicant.MoveToWorld(placement.Boss, Map.Sosaria);
                                    ArrayList toMove;
                                    HousePlacementResult houseResult = HousePlacement.Check(applicant, 0x64, placement.Boss, out toMove);
                                    Console.WriteLine("HOUSING " + id + " region=" + applicant.Region.Name
                                        + " allows=" + applicant.Region.AllowHousing(applicant, placement.Boss)
                                        + " small-house-0x64=" + houseResult);
                                }
                                finally { applicant.Delete(); }
                                for (int stage = 0; stage < 3; ++stage)
                                    Console.WriteLine("BROOD " + stage + " " + placement.Brood(stage));
                                selected = true;
                                ++found;
                            }
                    if (!selected)
                        throw new Exception("No valid site in sector " + sectorX + "," + sectorY);
                }

            if (found != 6)
                throw new Exception("Expected six sites.");
            if (World.Items.Count != 0 || World.Mobiles.Count != 0)
                throw new Exception("Survey unexpectedly created world entities.");
            byte[] after = File.ReadAllBytes(args[1]);
            if (Convert.ToBase64String(after) != Convert.ToBase64String(xmlBytes))
                throw new Exception("Region XML changed during survey.");
            Console.WriteLine("PASS terrain survey: 6 candidate sites; client review and saved-world occupancy remain unverified.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
