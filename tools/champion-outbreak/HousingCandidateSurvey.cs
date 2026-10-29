
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
class HousingCandidateSurvey
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
    const int step=8,nx=640,ny=391;
    int[,] houses=new int[nx+1,ny+1],walk=new int[nx+1,ny+1];
    SosariaOutbreakTerrain terrain=new SosariaOutbreakTerrain();
    for(int gx=0;gx<nx;++gx)for(int gy=0;gy<ny;++gy)
    {
     int x=gx*step+4,y=gy*step+4;Point3D point=new Point3D(x,y,Map.Sosaria.GetAverageZ(x,y));
     applicant.MoveToWorld(point,Map.Sosaria);ArrayList ignored;
     bool possible=HousePlacement.Check(applicant,0x64,point,out ignored)==HousePlacementResult.Valid;
     bool passable=terrain.TryGetPoint(x,y,out point);
     houses[gx+1,gy+1]=(possible?1:0)+houses[gx,gy+1]+houses[gx+1,gy]-houses[gx,gy];
     walk[gx+1,gy+1]=(passable?1:0)+walk[gx,gy+1]+walk[gx+1,gy]-walk[gx,gy];
    }
    List<int[]> ranked=new List<int[]>();
    for(int gx=10;gx<nx-10;++gx)for(int gy=10;gy<ny-10;++gy)
    {
     int hc=houses[gx+11,gy+11]-houses[gx-10,gy+11]-houses[gx+11,gy-10]+houses[gx-10,gy-10];
     int wc=walk[gx+8,gy+8]-walk[gx-7,gy+8]-walk[gx+8,gy-7]+walk[gx-7,gy-7];
     if(hc==0&&wc>=70)ranked.Add(new int[]{gx*step+4,gy*step+4,wc});
    }
    ranked.Sort(delegate(int[] a,int[] b){int c=b[2].CompareTo(a[2]);return c!=0?c:a[0].CompareTo(b[0]);});
    Console.WriteLine("GRID sampled="+(nx*ny)+" candidates="+ranked.Count+"; exploratory only, not approval.");
    List<Point2D> disqualifyingPlots=new List<Point2D>();int screened=0,clear=0,skipped=0,topologyRejected=0;
    foreach(int[] center in ranked)
    {
     bool eliminated=false;foreach(Point2D plot in disqualifyingPlots)if(Math.Abs(plot.X-center[0])<=80&&Math.Abs(plot.Y-center[1])<=80){eliminated=true;break;}
     if(eliminated){++skipped;continue;}
     ArachnidOutbreakSite site=new ArachnidOutbreakSite("survey",center[0],center[1],false);
     ArachnidOutbreakPlacement placement;string reason;
     if(!site.Validate(new TerrainOnly(terrain),out placement,out reason)){++topologyRejected;continue;}
     ++screened;bool rejected=false;
     foreach(int id in new int[]{0x64,0x70})
     {
      for(int dy=-80;dy<=80&&!rejected;++dy)for(int dx=-80;dx<=80&&!rejected;++dx)
      {
       int x=center[0]+dx,y=center[1]+dy;Point3D point=new Point3D(x,y,Map.Sosaria.GetAverageZ(x,y));
       applicant.MoveToWorld(point,Map.Sosaria);ArrayList ignored;
       if(HousePlacement.Check(applicant,id,point,out ignored)==HousePlacementResult.Valid)
       {
        disqualifyingPlots.Add(new Point2D(x,y));rejected=true;
        Console.WriteLine("REJECT "+center[0]+","+center[1]+" legal_plot="+x+","+y+" multi="+id);
       }
      }
      if(rejected)break;
     }
     if(!rejected)
     {
      ++clear;
      Console.WriteLine("COMMON_SHAPES_CLEAR "+center[0]+","+center[1]+" topology=True reason="+reason+"; all-shape proof and copied-world occupancy still required");
     }
     if(screened>=500)break;
    }
    Console.WriteLine("SUMMARY topology_rejected="+topologyRejected+" screened="+screened+" eliminated_by_known_plots="+skipped+" common_shapes_clear="+clear+" total_candidates="+ranked.Count);

   }finally{applicant.Delete();}
   return 0;
  }catch(Exception e){Console.Error.WriteLine(e);return 1;}
 }
 class TerrainOnly : IArachnidOutbreakTerrain
 {
  private readonly SosariaOutbreakTerrain terrain;
  public TerrainOnly(SosariaOutbreakTerrain value){terrain=value;}
  public bool HasPermanentHousingProhibition(Rectangle2D bounds){return true;}
  public bool IsReserved(Rectangle2D bounds){return terrain.IsReserved(bounds);}
  public bool TryGetPoint(int x,int y,out Point3D point){return terrain.TryGetPoint(x,y,out point);}
 }
}
