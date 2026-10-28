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
class HousingEligibilityProbe
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
    foreach(int[] center in new int[][]{new int[]{914,716},new int[]{2258,588},new int[]{3794,844},new int[]{1106,1768},new int[]{2514,2152},new int[]{3858,2600}})
    {
     bool found=false;int tested=0;
     for(int dy=-56;dy<=56&&!found;++dy) for(int dx=-56;dx<=56&&!found;++dx)
     {
      int x=center[0]+dx,y=center[1]+dy;Point3D point=new Point3D(x,y,Map.Sosaria.GetAverageZ(x,y));
      applicant.MoveToWorld(point,Map.Sosaria);ArrayList ignored;++tested;
      if(HousePlacement.Check(applicant,0x64,point,out ignored)==HousePlacementResult.Valid)
      {Console.WriteLine("DISQUALIFIED "+center[0]+","+center[1]+" legal small house at "+point+" tested="+tested);found=true;}
     }
     if(!found)Console.WriteLine("INCONCLUSIVE "+center[0]+","+center[1]+" no 0x64 placement found; other supported shapes still require testing; tested="+tested);
    }
   }finally{applicant.Delete();}
   return 0;
  }catch(Exception e){Console.Error.WriteLine(e);return 1;}
 }
}
