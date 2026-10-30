using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Regions;
using Server.Custom.Confictura.Champions;

internal static class ArachnidInfestationTownRegression
{
    private static int Checks;
    private static void Check(bool ok,string text){if(!ok)throw new Exception("FAILED: "+text);++Checks;}
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();
            ArachnidOutbreakController c=new ArachnidOutbreakController();
            ArachnidInfestationCreature frontier=new ArachnidInfestationCreature(c,0,0,false,false);
            TownGuards guard=new TownGuards();Citizens citizen=new Citizens();
            try
            {
                Point3D point=new Point3D(2514,2088,0);frontier.MoveToWorld(point,Map.Sosaria);
                guard.MoveToWorld(new Point3D(point.X+1,point.Y,point.Z),Map.Sosaria);
                citizen.MoveToWorld(new Point3D(point.X,point.Y+1,point.Z),Map.Sosaria);
                Check(frontier.IsEnemy(guard) && frontier.CanBeHarmful(guard,false),"event-local targeting permits attacking an ordinary guard");
                Check(guard.IsEnemy(frontier) && guard.CanBeHarmful(frontier,false),"ordinary guard rules identify the frontier invader as an enemy");
                Check(frontier.IsEnemy(citizen) && frontier.CanBeHarmful(citizen,false),"citizens can be attacked under ordinary harmful rules");
                Check(!citizen.IsEnemy(frontier),"passive citizens are not silently converted into combat guards");
                Check(guard.HitsMax>=1500 && guard.HitsMax<=2000 && frontier.HitsMax==30,"real guard stats substantially exceed the weakest frontier creature");
                XmlDocument xml=new XmlDocument();xml.LoadXml("<region name='Infestation protected QA' priority='200'><rect x='2500' y='2070' width='32' height='32'/></region>");
                ProtectedRegion protectedRegion=new ProtectedRegion(xml.DocumentElement,Map.Sosaria,null);protectedRegion.Register();
                try
                {
                    Check(!protectedRegion.AllowHarmful(frontier,citizen),"protected-region harmful policy remains authoritative");
                    Point3D blocked;Check(!ArachnidInfestationTerrain.TryPoint(point.X,point.Y,out blocked),"infestation terrain does not silently override protected-town rules");
                }
                finally {protectedRegion.Unregister();}
            }
            finally {c.Delete();frontier.Delete();guard.Delete();citizen.Delete();}
            Console.WriteLine("PASS infestation town targeting: "+Checks+" real NPC/region method assertions; no AI combat/respawn claim.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
