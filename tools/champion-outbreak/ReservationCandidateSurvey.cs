
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
class ReservationCandidateSurvey
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
    SosariaOutbreakTerrain terrain=new SosariaOutbreakTerrain();
    List<string> vendors=new List<string>();
    foreach(Type type in typeof(BaseRegion).Assembly.GetTypes())
     {
      Type ancestor=type;
      while(ancestor!=null)
      {
       if(ancestor.Name=="BaseVendor"||ancestor.Name=="BaseEscortable"||ancestor.Name=="PlayerVendor"||ancestor.Name=="PlayerBarkeeper"||ancestor.Name=="TownCrier"||ancestor.Name=="MondainQuester"||ancestor.Name=="BaseQuestGiver")
       {vendors.Add(type.FullName);break;}
       ancestor=ancestor.BaseType;
      }
     }
    File.WriteAllLines(args[2]+".vendor-types",vendors.ToArray());
    int passed=0,failed=0;
    foreach(string line in File.ReadAllLines(args[2]))
    {
     string[] values=line.Split(',');int x=Int32.Parse(values[1]),y=Int32.Parse(values[2]);
     ArachnidOutbreakSite site=new ArachnidOutbreakSite(values[0],x,y,false);
     ArachnidOutbreakPlacement placement;string reason;
     if(site.Validate(new TerrainOnly(terrain),out placement,out reason))
     {
      int roofTiles=0;
      for(int tx=x-80;tx<=x+80;++tx)for(int ty=y-80;ty<=y+80;++ty)
       foreach(StaticTile tile in Map.Sosaria.Tiles.GetStaticTiles(tx,ty,true))
        if((TileData.ItemTable[tile.ID&TileData.MaxItemValue].Flags&TileFlag.Roof)!=0)++roofTiles;
      ++passed;Console.WriteLine("TERRAIN_PASS\t"+values[0]+"\t"+x+"\t"+y+"\t"+placement.WalkableCount+"\t"+placement.Boss+"\t"+placement.Brood(0)+"\t"+placement.Brood(1)+"\t"+placement.Brood(2)+"\t"+roofTiles);

     }
     else{++failed;Console.WriteLine("REJECT\t"+values[0]+"\t"+x+"\t"+y+"\t"+reason);}
    }
    Console.WriteLine("SUMMARY terrain_pass="+passed+" failed="+failed+"; permanent reservation not created or approved by this probe.");

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
