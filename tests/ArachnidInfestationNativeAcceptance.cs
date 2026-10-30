using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    public static class ArachnidInfestationNativeAcceptance
    {
        private static readonly string Evidence=Path.GetFullPath(Path.Combine(Core.BaseDirectory,"../evidence"));
        private static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        private static PlayerMobile Actor;
        private static ArachnidOutbreakController Controller;
        private static Timer Poll;
        private static DateTime NextTick,Deadline=DateTime.UtcNow.AddMinutes(40),Started;
        private static string Scene="survey",Failure=String.Empty;
        private static Point3D TownOrigin;
        private static int GuardDeaths,CitizenDeaths,EventDeaths;
        private static List<Item> Fixtures=new List<Item>();
        private static List<Mobile> TownNpcs=new List<Mobile>();
        private static object Get(object value,string field){return value.GetType().GetField(field,Private).GetValue(value);}
        private static void Call(object value,string name,params object[] args){value.GetType().GetMethod(name,Private).Invoke(value,args);}

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            string guard=Path.Combine(Evidence,"native-private-root.txt");
            if(!File.Exists(guard) || !String.Equals(Path.GetFullPath(File.ReadAllText(guard)).TrimEnd('\\'),Core.BaseDirectory.TrimEnd('\\'),StringComparison.OrdinalIgnoreCase)
                || ArachnidOutbreakController.DeliveryReady)throw new InvalidOperationException("Infestation acceptance requires its exact disabled-production copy.");
            AutoSave.SavesEnabled=false;EventSink.MobileDeath+=Died;Timer.DelayCall(TimeSpan.FromSeconds(1),Prepare);
        }
        private static void Prepare()
        {
            try
            {
                IAccount login=Accounts.GetAccount(File.ReadAllText(Path.Combine(Evidence,"account-name.private")));
                Actor=login==null?null:login[0] as PlayerMobile;
                if(Actor==null || Actor.AccessLevel!=AccessLevel.Player || !Actor.Alive)throw new Exception("Requires existing copied living Player login.");
                Survey();Poll=Timer.DelayCall(TimeSpan.Zero,TimeSpan.FromSeconds(1),Tick);File.WriteAllText(Path.Combine(Evidence,"infestation-ready.txt"),"Survey complete; no encounter started.");
            }
            catch(Exception error){Failure=error.ToString();Status();}
        }
        private static void Survey()
        {
            using(XmlWriter writer=XmlWriter.Create(Path.Combine(Evidence,"town-survey.private.xml"),new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("towns");int candidates=0;
                foreach(Mobile mobile in World.Mobiles.Values)
                {
                    if(!(mobile is Citizens) || mobile.Deleted || !mobile.Alive || mobile.Map!=Map.Sosaria)continue;
                    Point3D point;bool allowed=ArachnidInfestationTerrain.TryPoint(mobile.X,mobile.Y,out point);
                    writer.WriteStartElement("citizen");Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"region",mobile.Region.Name);
                    Attr(writer,"regionType",mobile.Region.GetType().Name);Attr(writer,"allowed",allowed);Attr(writer,"blessed",mobile.Blessed);
                    BaseCreature npc=mobile as BaseCreature;Attr(writer,"spawner",npc.Spawner==null?"none":npc.Spawner.GetType().FullName);writer.WriteEndElement();
                    if(!allowed || TownOrigin!=Point3D.Zero)continue;
                    IPooledEnumerable nearby=Map.Sosaria.GetMobilesInRange(mobile.Location,18);
                    try
                    {
                        foreach(Mobile other in nearby)
                            if(other is TownGuards && other.Alive && !other.Deleted)
                            {
                                TownOrigin=point;++candidates;break;
                            }
                    }
                    finally{nearby.Free();}
                }
                writer.WriteElementString("selectedCandidates",candidates.ToString());writer.WriteEndElement();
            }
            if(TownOrigin==Point3D.Zero)throw new Exception("No ordinary harmful-enabled town citizen/guard pair found in copied world; inspect survey.");
        }
        private static void Died(Mobile mobile)
        {
            if(Controller==null || mobile==null)return;
            IArachnidOutbreakOwned owned=mobile as IArachnidOutbreakOwned;
            if(owned!=null && owned.Controller==Controller && owned.AttemptId==Controller.AttemptId)++EventDeaths;
            if(TownNpcs.Contains(mobile))
            {
                if(mobile is TownGuards)++GuardDeaths;
                if(mobile is Citizens)++CitizenDeaths;
                using(StreamWriter log=new StreamWriter(Path.Combine(Evidence,"town-deaths.private.log"),true))log.WriteLine(DateTime.UtcNow.ToString("o")+"\t"+mobile.Serial+"\t"+mobile.GetType().FullName);
            }
        }
        private static void Cleanup()
        {
            if(Controller!=null){Controller.Delete();Controller=null;}
            foreach(Item item in Fixtures)if(item!=null && !item.Deleted)item.Delete();Fixtures.Clear();
            if(Actor!=null && !Actor.Deleted)Actor.Blessed=false;
        }
        private static void StartTown()
        {
            Cleanup();Scene="town";GuardDeaths=CitizenDeaths=EventDeaths=0;TownNpcs.Clear();
            IPooledEnumerable nearby=Map.Sosaria.GetMobilesInRange(TownOrigin,32);
            try{foreach(Mobile mobile in nearby)if(mobile is TownGuards || mobile is Citizens)TownNpcs.Add(mobile);}
            finally{nearby.Free();}
            Point3D origin=Point3D.Zero;
            for(int radius=2;radius<=10 && origin==Point3D.Zero;++radius)
                for(int x=-radius;x<=radius && origin==Point3D.Zero;++x)
                    for(int y=-radius;y<=radius;++y)
                    {
                        Point3D p;if(ArachnidInfestationTerrain.TryPoint(TownOrigin.X+x,TownOrigin.Y+y,out p) && Map.Sosaria.CanSpawnMobile(p)){origin=p;break;}
                    }
            if(origin==Point3D.Zero)throw new Exception("Town has no safe origin beside the selected NPCs.");
            Controller=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(Controller,"m_State")).SetEnabled(true);
            bool begun=(bool)typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Controller,new object[]{origin,DateTime.UtcNow});
            if(!begun)throw new Exception("Town infestation start refused.");
            // A blessed observer activates/render-checks the scene without contributing.
            Actor.Combatant=null;Actor.Warmode=false;Actor.Blessed=true;Actor.MoveToWorld(new Point3D(origin.X-8,origin.Y,origin.Z),Map.Sosaria);
            Started=DateTime.UtcNow;NextTick=DateTime.MinValue;
        }
        private static void Artwork()
        {
            Cleanup();Scene="artwork";Point3D center=new Point3D(2514,2088,0);
            Controller=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(Controller,"m_State")).SetEnabled(true);
            typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Controller,new object[]{center,DateTime.UtcNow});
            foreach(Mobile mobile in ((List<Mobile>)Get(Controller,"m_Creatures")).ToArray())mobile.Delete();
            ((List<Mobile>)Get(Controller,"m_Creatures")).Clear();
            int index=0;
            for(int tier=0;tier<3;++tier)
                for(int kind=0;kind<2;++kind)
                {
                    ArachnidInfestationCreature creature=new ArachnidInfestationCreature(Controller,0,tier,false,false);
                    typeof(ArachnidInfestationCreature).GetField("m_Kind",Private).SetValue(creature,kind);creature.ApplyTier(tier);
                    creature.Name="QA tier "+tier+" variant "+kind;creature.MoveToWorld(new Point3D(center.X-9+(index%3)*8,center.Y-8+(index/3)*8,0),Map.Sosaria);
                    ((List<Mobile>)Get(Controller,"m_Creatures")).Add(creature);if(creature.AIObject!=null)creature.AIObject.m_Timer.Stop();++index;
                }
            ArachnidInfestationCreature guardian=new ArachnidInfestationCreature(Controller,0,2,true,false);guardian.MoveToWorld(new Point3D(center.X,center.Y+8,0),Map.Sosaria);
            ((List<Mobile>)Get(Controller,"m_Creatures")).Add(guardian);if(guardian.AIObject!=null)guardian.AIObject.m_Timer.Stop();
            BaseAddon[] webs=new BaseAddon[]{new GiantWeb1(),new GiantWeb2(),new GiantWeb3(),new GiantWeb4(),new GiantWeb5(),new GiantWeb6()};
            for(int i=0;i<webs.Length;++i){webs[i].MoveToWorld(new Point3D(center.X-12+(i%3)*9,center.Y+13+(i/3)*9,0),Map.Sosaria);Fixtures.Add(webs[i]);}
            Actor.Combatant=null;Actor.Warmode=false;Actor.Blessed=true;Actor.MoveToWorld(center,Map.Sosaria);Started=DateTime.UtcNow;
        }
        private static void Tick()
        {
            try
            {
                string request=Path.Combine(Evidence,"infestation-command.txt");
                if(File.Exists(request))
                {
                    string command=File.ReadAllText(request).Trim();File.Delete(request);
                    if(command=="town")StartTown();else if(command=="artwork")Artwork();else if(command=="cleanup")Cleanup();else if(command!="survey")throw new Exception("Unknown finite fixture request.");
                }
                if(Controller!=null && Scene=="town" && DateTime.UtcNow>=NextTick)
                {
                    NextTick=DateTime.UtcNow.AddSeconds(5);Call(Controller,"Tick",DateTime.UtcNow);
                }
                Status();
                if(File.Exists(Path.Combine(Evidence,"native-stop.txt")) || DateTime.UtcNow>Deadline)
                {
                    Cleanup();Poll.Stop();AutoSave.SavesEnabled=false;File.WriteAllText(Path.Combine(Evidence,"native-stopped.txt"),"Owned infestation fixture stopped without saving.");
                }
            }
            catch(Exception error){Failure=error.ToString();Status();Cleanup();if(Poll!=null)Poll.Stop();}
        }
        private static void Attr(XmlWriter writer,string name,object value){writer.WriteAttributeString(name,Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture));}
        private static void Status()
        {
            string file=Path.Combine(Evidence,"infestation-state.xml");
            using(XmlWriter writer=XmlWriter.Create(file+".tmp",new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("acceptance");Attr(writer,"scene",Scene);Attr(writer,"failure",Failure);Attr(writer,"online",Actor!=null && Actor.NetState!=null);
                Attr(writer,"seconds",Started==DateTime.MinValue?0:(DateTime.UtcNow-Started).TotalSeconds);
                Attr(writer,"guardDeaths",GuardDeaths);Attr(writer,"citizenDeaths",CitizenDeaths);Attr(writer,"eventDeaths",EventDeaths);
                if(Controller!=null)
                {
                    Attr(writer,"phase",Controller.Phase);Attr(writer,"population",Controller.InfestationPopulation);
                    ArachnidInfestationState colony=Get(Controller,"m_Infestation") as ArachnidInfestationState;
                    if(colony!=null){Attr(writer,"patches",colony.Patches.Count);Attr(writer,"epoch",colony.Epoch);Attr(writer,"births",colony.Births);Attr(writer,"terrainProbes",colony.TerrainProbes);Attr(writer,"routeRequests",colony.RouteRequests);}
                    foreach(Mobile mobile in (List<Mobile>)Get(Controller,"m_Creatures"))
                        if(mobile!=null && !mobile.Deleted){writer.WriteStartElement("creature");Attr(writer,"type",mobile.GetType().Name);Attr(writer,"body",mobile.BodyValue);Attr(writer,"hits",mobile.Hits);Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"combatant",mobile.Combatant==null?"none":mobile.Combatant.GetType().Name);writer.WriteEndElement();}
                }
                foreach(Mobile npc in TownNpcs)
                {
                    writer.WriteStartElement("npc");Attr(writer,"serial",npc.Serial.Value);Attr(writer,"type",npc.GetType().Name);Attr(writer,"alive",npc.Alive && !npc.Deleted);Attr(writer,"hits",npc.Hits);Attr(writer,"combatant",npc.Combatant==null?"none":npc.Combatant.GetType().Name);writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            if(File.Exists(file))File.Replace(file+".tmp",file,null);else File.Move(file+".tmp",file);
        }
    }
}
