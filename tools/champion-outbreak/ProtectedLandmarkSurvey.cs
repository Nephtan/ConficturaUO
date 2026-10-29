
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Misc;
using Server.Multis;
using Server.Regions;
using Server.Custom.Confictura.Champions;
class ProtectedLandmarkSurvey
{
 static void Load(XmlElement root,Region parent)
 {
  foreach(XmlElement element in root.SelectNodes("region"))
  {
   Type type=typeof(BaseRegion);
   if(element.HasAttribute("type")&&!Region.ReadType(element,"type",ref type,true))throw new Exception("Unknown region type");
   Region region=(Region)Activator.CreateInstance(type,new object[]{element,Map.Sosaria,parent});region.Register();Load(element,region);
  }
 }
 static int Main(string[] args)
 {
  try
  {
   Core.Assembly=typeof(Core).Assembly;Core.DataDirectories.Add(args[0]);ScriptCompiler.Assemblies=new Assembly[]{typeof(BaseRegion).Assembly};
   typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
   typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
   MapDefinitions.Configure();BaseRegion.Configure();XmlDocument doc=new XmlDocument();doc.Load(args[1]);Load((XmlElement)doc.SelectSingleNode("/ServerRegions/Facet[@name='Sosaria']"),null);
   Mobile applicant=new Mobile();applicant.AccessLevel=AccessLevel.Player;
   try
   {

    applicant.Skills[SkillName.Camping].Base=120;applicant.Skills[SkillName.Tracking].Base=120;
    string[] names=new string[]{"the Orc Fort","the Ratmen Fort","the Swamp Ruins of the Dead","the Ratmen Ruins","the Lich Tower","Castle Exodus","the Brigand Fort"};
    SosariaOutbreakTerrain terrain=new SosariaOutbreakTerrain();
    foreach(XmlElement e in doc.SelectNodes("/ServerRegions/Facet[@name='Sosaria']//region"))
    {
     if(Array.IndexOf(names,e.GetAttribute("name"))<0)continue;
     foreach(XmlElement rect in e.SelectNodes("rect"))
     {
      int x0=Int32.Parse(rect.GetAttribute("x")),y0=Int32.Parse(rect.GetAttribute("y")),w=Int32.Parse(rect.GetAttribute("width")),h=Int32.Parse(rect.GetAttribute("height"));
      Dictionary<Point2D,Point3D> all=new Dictionary<Point2D,Point3D>();
      for(int x=x0;x<x0+w;++x)for(int y=y0;y<y0+h;++y)
      {
       Point3D p;if(terrain.TryGetPoint(x,y,out p))all.Add(new Point2D(x,y),p);
      }
      HashSet<Point2D> seen=new HashSet<Point2D>();List<Point3D> largest=new List<Point3D>();
      foreach(Point3D first in all.Values)
      {
       if(seen.Contains(new Point2D(first.X,first.Y)))continue;
       Queue<Point3D> queue=new Queue<Point3D>();List<Point3D> component=new List<Point3D>();queue.Enqueue(first);seen.Add(new Point2D(first.X,first.Y));
       int[] dx=new int[]{1,-1,0,0},dy=new int[]{0,0,1,-1};
       while(queue.Count>0)
       {
        Point3D p=queue.Dequeue();component.Add(p);
        for(int k=0;k<4;++k)
        {
         Point2D key=new Point2D(p.X+dx[k],p.Y+dy[k]);Point3D next;
         if(!seen.Contains(key)&&all.TryGetValue(key,out next)&&Math.Abs(p.Z-next.Z)<=2){seen.Add(key);queue.Enqueue(next);}
        }
       }
       if(component.Count>largest.Count)largest=component;
      }
      HashSet<Point2D> connected=new HashSet<Point2D>();foreach(Point3D p in largest)connected.Add(new Point2D(p.X,p.Y));
      List<Point3D> staging=new List<Point3D>();
      foreach(Point3D p in largest)
      {
       bool fits=true;for(int dx=-2;dx<=2&&fits;++dx)for(int dy=-2;dy<=2&&fits;++dy)if(!connected.Contains(new Point2D(p.X+dx,p.Y+dy)))fits=false;
       if(fits)staging.Add(p);
      }
      Console.WriteLine("LANDMARK "+e.GetAttribute("name")+" bounds="+x0+","+y0+","+w+","+h+" permanent="+terrain.HasPermanentHousingProhibition(new Rectangle2D(x0,y0,w,h))+" walkable="+all.Count+" largest_component="+largest.Count+" staging5x5="+staging.Count+"; NOT APPROVED: current layout and safety buffer do not fit");
     }
    }

   }finally{applicant.Delete();}
   return 0;
  }catch(Exception e){Console.Error.WriteLine(e);return 1;}
 }
}
