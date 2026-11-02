using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Misc;
using Server.Custom.Confictura.Champions;

// Offline preflight of a retained copied world. No Initialize, listener, save or combat.
public static class ArachnidSafePreflight
{
    private static Point3D Point(XmlElement root,string name)
    { XmlElement node=(XmlElement)root.SelectSingleNode(name);return new Point3D(Int32.Parse(node.GetAttribute("x")),Int32.Parse(node.GetAttribute("y")),Int32.Parse(node.GetAttribute("z"))); }
    private static int Distance(Point3D a,Point3D b){return Math.Max(Math.Abs(a.X-b.X),Math.Abs(a.Y-b.Y));}
    private static void Check(bool pass,string reason){if(!pass)throw new Exception(reason);Console.WriteLine("PASS: "+reason);}
    private static void WritePoint(XmlWriter writer,string name,Point3D point)
    {writer.WriteStartElement(name);writer.WriteAttributeString("x",point.X.ToString());writer.WriteAttributeString("y",point.Y.ToString());writer.WriteAttributeString("z",point.Z.ToString());writer.WriteEndElement();}
    public static int Main(string[] args)
    {
        try
        {
            Check(Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase),"Owned private preflight directory");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(Core).GetField("m_Service",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,true);
            ScriptCompiler.Assemblies=new Assembly[]{typeof(ArachnidOutbreakController).Assembly};
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();World.Load();
            XmlDocument input=new XmlDocument();input.XmlResolver=null;input.Load(args[1]);XmlElement plan=input.DocumentElement;
            Point3D opening=Point(plan,"opening"),exit=Point(plan,"exit"),center=Point(plan,"center"),queen=Point(plan,"queen");
            Check(Distance(opening,queen)>Distance(center,queen) && Distance(exit,queen)>Distance(opening,queen),"Opening and exit both lead outward from the measured queen");
            Mobile proxy=new Mobile();proxy.Player=true;proxy.Body=0x190;proxy.AccessLevel=AccessLevel.Player;proxy.Blessed=true;proxy.MoveToWorld(opening,Map.Sosaria);
            PathFollower follower=new PathFollower(proxy,exit);int moves=0,unchanged=0;
            for(int attempt=0;attempt<512 && proxy.Location!=exit;++attempt)
            {
                Point3D before=proxy.Location;follower.Follow(true,0);
                if(proxy.Location!=before){++moves;unchanged=0;}else if(++unchanged>16)break;
            }
            Check(proxy.Location==exit && moves>0,"Protected disposable proxy follows the complete actual escape through ordinary server movement");
            proxy.Delete();
            PlayerMobile bard=World.FindMobile(142237) as PlayerMobile;
            Check(bard!=null && bard.Alive,"Actual saved bard remains alive before exposure");
            Point3D start=bard.Map==Map.Internal ? bard.LogoutLocation : bard.Location;
            Map map=bard.Map==Map.Internal ? bard.LogoutMap : bard.Map;
            Check(map!=null && map!=Map.Internal,"Saved safe-area map is available");
            int[] dx={0,1,1,1,0,-1,-1,-1},dy={-1,-1,0,1,1,1,0,-1};bool found=false;Point3D step=Point3D.Zero;
            for(int i=0;i<8 && !found;++i)
            {
                int z,back;
                if(!Server.Movement.Movement.CheckMovement(bard,map,start,(Direction)i,out z))continue;
                Point3D candidate=new Point3D(start.X+dx[i],start.Y+dy[i],z);
                if(Math.Abs(z-start.Z)>2 || !Server.Movement.Movement.CheckMovement(bard,map,candidate,(Direction)((i+4)%8),out back) || Math.Abs(back-start.Z)>2)continue;
                bool threat=false;IPooledEnumerable near=map.GetMobilesInRange(candidate,16);
                try{foreach(Mobile mobile in near){BaseCreature creature=mobile as BaseCreature;if(creature!=null && creature.Alive && !creature.Controlled && !creature.Summoned && (creature.FightMode&FightMode.Closest)!=0 && creature.IsEnemy(bard)){threat=true;break;}}}finally{near.Free();}
                if(!threat){step=candidate;found=true;}
            }
            Check(found,"Safe adjacent native-client step and return have checked terrain and no nearby hostile acquisition candidate");
            using(XmlWriter writer=XmlWriter.Create("safe-native-step.xml",new XmlWriterSettings{Indent=true}))
            {writer.WriteStartElement("safeStep");writer.WriteAttributeString("serial",bard.Serial.Value.ToString());writer.WriteAttributeString("map",map.Name);WritePoint(writer,"start",start);WritePoint(writer,"step",step);writer.WriteEndElement();}
            Console.WriteLine("Offline server movement preflight only; full native-client escape and combat remain separate. No world save written.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
