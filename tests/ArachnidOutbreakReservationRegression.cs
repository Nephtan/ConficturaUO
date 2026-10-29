using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml;
using Server;
using Server.Misc;
using Server.Multis;
using Server.Regions;
using Server.Items;
using Server.Mobiles;
using Server.Engines.CannedEvil;
using Server.Custom.Confictura.Champions;

internal static class ArachnidOutbreakReservationRegression
{
    private static int s_Checks;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        ++s_Checks;
    }
    private static void Load(XmlElement root, Region parent)
    {
        foreach (XmlElement element in root.SelectNodes("region"))
        {
            Type type = typeof(BaseRegion);
            if (element.HasAttribute("type") && !Region.ReadType(element,"type",ref type,true))
                throw new Exception("Unknown region type");
            Region region = (Region)Activator.CreateInstance(type,new object[] { element,Map.Sosaria,parent });
            region.Register();
            Load(element,region);
        }
    }
    private static HousePlacementResult Place(Mobile from, Point3D point)
    {
        from.MoveToWorld(point,Map.Sosaria);
        ArrayList ignored;
        return HousePlacement.Check(from,0x64,point,out ignored);
    }
    private static void CityAndLegacyPlacement(Mobile applicant, WildernessEncounterReserve reserve)
    {
        FieldStoneCityHallDeed deed = new FieldStoneCityHallDeed();
        XmlSpawner spawner = new XmlSpawner();
        try
        {
            applicant.AccessLevel = AccessLevel.GameMaster;
            Point3D target = new Point3D(reserve.Bounds.X-50+deed.Offset.X,reserve.Center.Y+deed.Offset.Y,0);
            applicant.MoveToWorld(target,Map.Sosaria);
            Check(!WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,target.X-deed.Offset.X,target.Y-deed.Offset.Y),"city hall center itself is outside reserve");
            Check(PlayerGovernmentSystem.CheckPlacement(applicant,target,deed.MultiID,deed.Offset),"actual city hall terrain preflight succeeds before territory restriction");
            int before = World.Items.Count;
            PlayerGovernmentSystem.PlaceCityHall(applicant,target,deed);
            Check(!deed.Deleted && World.Items.Count==before,"future city growth overlap refuses placement before consuming deed or creating civic objects");
            applicant.AccessLevel = AccessLevel.Player;
            spawner.SpawnRange = 0;
            spawner.MoveToWorld(new Point3D(reserve.Center.X,reserve.Center.Y,0),Map.Sosaria);
            XmlSpawner.SpawnObject entry = new XmlSpawner.SpawnObject("ChampionSpawn",1);
            ChampionSpawn blocked = new ChampionSpawn();
            int originalWidth = blocked.SpawnArea.Width;
            int originalHeight = blocked.SpawnArea.Height;
            blocked.MoveToWorld(spawner.Location,Map.Sosaria);
            Check(blocked.SpawnArea.Width==originalWidth && blocked.SpawnArea.Height==originalHeight,"legacy relocation preserves whole rectangle dimensions");
            string status;
            BaseXmlSpawner.AddSpawnItem(spawner,entry,blocked,spawner.Location,Map.Sosaria,null,true,null,"ChampionSpawn",false,out status);
            Check(blocked.Deleted && !entry.SpawnedObjects.Contains(blocked) && status!=null,"fixed legacy controller cannot fall back into reservation; deleted="+blocked.Deleted+" location="+blocked.Location+" map="+blocked.Map+" area="+blocked.SpawnArea+" status="+status);
            spawner.MoveToWorld(new Point3D(reserve.Bounds.X-200,reserve.Center.Y,0),Map.Sosaria);
            ChampionSpawn safe = new ChampionSpawn();
            try
            {
                BaseXmlSpawner.AddSpawnItem(spawner,entry,safe,spawner.Location,Map.Sosaria,null,true,null,"ChampionSpawn",false,out status);
                Check(!safe.Deleted && entry.SpawnedObjects.Contains(safe),"ordinary legacy placement outside reservations remains available");
            }
            finally { safe.Delete(); }
            ChampionSpawn enlarged = new ChampionSpawn();
            BaseXmlSpawner.AddSpawnItem(spawner,entry,enlarged,spawner.Location,Map.Sosaria,null,true,null,"ChampionSpawn/SpawnRadius/300",false,out status);
            Check(enlarged.Deleted && !entry.SpawnedObjects.Contains(enlarged),"legacy final configured area checked after property-string enlargement");
        }
        finally { deed.Delete();spawner.Delete(); }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            ScriptCompiler.Assemblies = new Assembly[] { typeof(BaseRegion).Assembly };
            Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();
            BaseRegion.Configure();
            XmlDocument regions = new XmlDocument();
            regions.Load(args[1]);
            Load((XmlElement)regions.SelectSingleNode("/ServerRegions/Facet[@name='Sosaria']"),null);
            FieldInfo catalog = typeof(WildernessEncounterReserves).GetField("m_Proposals",BindingFlags.Static|BindingFlags.NonPublic);
            object original = catalog.GetValue(null);
            List<WildernessEncounterReserve> reviewed = new List<WildernessEncounterReserve>();
            Check(WildernessEncounterReserves.Proposals.Count == 6,"six reviewed-by-tool proposals, not runtime approvals");
            foreach (WildernessEncounterReserve reserve in WildernessEncounterReserves.Proposals)
            {
                Check(!reserve.Reviewed && !WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,reserve.Center.X,reserve.Center.Y),"proposal does not silently impose a housing ban");
                reviewed.Add(new WildernessEncounterReserve(reserve.Id,reserve.Center.X,reserve.Center.Y,true));
            }
            Mobile applicant = new Mobile();
            applicant.AccessLevel = AccessLevel.Player;
            applicant.Skills[SkillName.Camping].Base = 120;
            applicant.Skills[SkillName.Tracking].Base = 120;
            Point3D overlap = Point3D.Zero;
            bool foundOverlap = false;
            try
            {
                // Find a genuinely legal baseline plot crossing the west boundary;
                // its center is outside. This avoids a weak already-invalid test.
                foreach (WildernessEncounterReserve reserve in reviewed)
                {
                    for (int dy=-60;dy<=60 && !foundOverlap;++dy)
                    {
                        int x=reserve.Bounds.X-1,y=reserve.Center.Y+dy;
                        Point3D point=new Point3D(x,y,Map.Sosaria.GetAverageZ(x,y));
                        if (Place(applicant,point)==HousePlacementResult.Valid) { overlap=point;foundOverlap=true; }
                    }
                    if (foundOverlap) break;
                }
                Check(foundOverlap,"real legal boundary-overlapping house found before policy approval");
                catalog.SetValue(null,reviewed.AsReadOnly()); // Private process-only simulation of reviewed declarations.
                foreach (WildernessEncounterReserve reserve in reviewed)
                {
                    Rectangle2D box=reserve.Bounds;
                    Check(WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,box.X,box.Y),"inclusive northwest reservation boundary");
                    Check(!WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,box.X+box.Width,box.Y),"exclusive east boundary");
                    Check(!WildernessEncounterReserves.ContainsHousingTile(Map.Lodor,reserve.Center.X,reserve.Center.Y),"other facets unaffected");
                    Check(WildernessEncounterReserves.Intersects(Map.Sosaria,new Rectangle2D(box.X-1,box.Y,2,1)),"crossing territory intersects");
                    Check(!WildernessEncounterReserves.Intersects(Map.Sosaria,new Rectangle2D(box.X,box.Y,0,1)),"empty territory does not intersect");
                    Point3D center=new Point3D(reserve.Center.X,reserve.Center.Y,Map.Sosaria.GetAverageZ(reserve.Center.X,reserve.Center.Y));
                    Check(Place(applicant,center)==HousePlacementResult.BadRegion,"ordinary player center rejected by actual housing validator");
                    applicant.AccessLevel=AccessLevel.GameMaster;
                    Check(Place(applicant,center)==HousePlacementResult.Valid,"existing staff placement policy preserved");
                    applicant.AccessLevel=AccessLevel.Player;
                    ArachnidOutbreakPlacement placement;string reason;
                    Check(new ArachnidOutbreakSite(reserve.Id,reserve.Center.X,reserve.Center.Y,false).Validate(new SosariaOutbreakTerrain(),out placement,out reason),"full reserved terrain layout passes: "+reserve.Id+" "+reason);
                    Check(placement.WalkableCount>=4000,"combat-space floor preserved");
                }
                Check(!WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,overlap.X,overlap.Y),"overlap test center remains outside every reserve");
                Check(Place(applicant,overlap)==HousePlacementResult.BadRegion,"multi crossing reservation rejected despite outside center");
                ArachnidOutbreakController controller=new ArachnidOutbreakController();
                controller.Stop();controller.Delete();
                Check(Place(applicant,overlap)==HousePlacementResult.BadRegion,"encounter stop and deletion cannot reopen permanent housing");
                CityAndLegacyPlacement(applicant,reviewed[0]);
            }
            finally
            {
                catalog.SetValue(null,original);
                applicant.Delete();
            }
            Check(!WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,reviewed[0].Center.X,reviewed[0].Center.Y),"test review simulation restored");
            Console.WriteLine("PASS outbreak reservations: "+s_Checks+" assertions; actual housing and terrain, isolated review simulation; no new production bans.");
            return 0;
        }
        catch (Exception exception) { Console.Error.WriteLine(exception);return 1; }
    }
}
