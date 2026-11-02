using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Misc;
using Server.Custom.Confictura.Champions;

// Supplied map knowledge for native packet drivers, not autonomous route discovery.
// No saves, global script initialization, listeners or actor combat are involved.
public static class ArachnidTerrainNavigation
{
    public static int Main(string[] args)
    {
        try
        {
            if (!Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase)) throw new Exception("Private test directory required");
            int centerX=Int32.Parse(args[1]),centerY=Int32.Parse(args[2]),radius=Int32.Parse(args[3]);
            if(radius<24 || radius>400 || centerX-radius<0 || centerY-radius<0)throw new Exception("Bounded legal map rectangle required");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            ScriptCompiler.Assemblies=new System.Reflection.Assembly[]{typeof(ArachnidOutbreakController).Assembly};
            typeof(World).GetField("m_Items",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();Server.Movement.MovementImpl.Configure();
            Dictionary<Point2D,Point3D> points=new Dictionary<Point2D,Point3D>();
            for(int x=centerX-radius;x<=centerX+radius;++x)for(int y=centerY-radius;y<=centerY+radius;++y)
            {Point3D point;if(ArachnidInfestationTerrain.TryPoint(x,y,out point))points.Add(new Point2D(x,y),point);}
            Mobile walker=new Mobile();walker.Player=true;walker.Body=0x190;walker.Map=Map.Sosaria;
            int[] dx={0,1,1,1,0,-1,-1,-1},dy={-1,-1,0,1,1,1,0,-1};
            using(StreamWriter writer=new StreamWriter("navigation-grid.json"))
            {
                writer.Write("{\"map\":1,\"scope\":\"Supplied copied-map terrain route; actual movement and dynamic collision remain native\",\"nodes\":[");bool first=true;
                foreach(KeyValuePair<Point2D,Point3D> entry in points)
                {
                    int mask=0;
                    for(int i=0;i<8;++i)
                    {
                        Point3D next;int z;
                        if(points.TryGetValue(new Point2D(entry.Key.X+dx[i],entry.Key.Y+dy[i]),out next)
                            && Server.Movement.Movement.CheckMovement(walker,Map.Sosaria,entry.Value,(Direction)i,out z) && Math.Abs(z-next.Z)<=2)mask|=1<<i;
                    }
                    if(!first)writer.Write(',');first=false;
                    writer.Write("["+entry.Value.X+","+entry.Value.Y+","+entry.Value.Z+","+mask+"]");
                }
                writer.Write("]}");
            }
            walker.Delete();Console.WriteLine("Supplied terrain grid: "+points.Count+" nodes; no saved world loaded.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
