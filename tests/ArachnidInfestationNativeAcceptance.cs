using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server.Multis;
using Server.Engines.XmlSpawner2;
using Server.Custom.Confictura.CloneOfflinePlayerCharacters;
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
        private static DateTime Deadline=DateTime.UtcNow.AddMinutes(75),Started;
        private static string Scene="survey",Failure=String.Empty;
        private static Point3D TownOrigin;
        private static int GuardDeaths,CitizenDeaths,EventDeaths;
        private static List<Item> Fixtures=new List<Item>();
        private static List<Mobile> TownNpcs=new List<Mobile>();
        private static List<Mobile> FixtureNpcs=new List<Mobile>();
        private static List<PlayerMobile> Players=new List<PlayerMobile>();
        private static List<CharacterClone> SelectedClones=new List<CharacterClone>();
        private static List<ArachnidOutbreakController> AggregateControllers=new List<ArachnidOutbreakController>();
        private static int PendingDirectors;
        private static Mobile FrontierFounder;
        private static TownGuards FrontierGuard;
        private static XmlSpawner CitizenSpawner;
        private static Dictionary<int,bool> SeenCitizens=new Dictionary<int,bool>();
        private static int RespawnGenerations;
        private static string CheckResult=String.Empty;
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
                InventoryPlayers();Survey();Poll=Timer.DelayCall(TimeSpan.Zero,TimeSpan.FromSeconds(1),Tick);File.WriteAllText(Path.Combine(Evidence,"infestation-ready.txt"),"Survey complete; no encounter started.");
            }
            catch(Exception error){Failure=error.ToString();Status();}
        }
        private static void Survey()
        {
            using(XmlWriter writer=XmlWriter.Create(Path.Combine(Evidence,"town-survey.private.xml"),new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("towns");int candidates=0,best=-1;
                foreach(Mobile mobile in World.Mobiles.Values)
                {
                    if(!(mobile is Citizens) || mobile.Deleted || !mobile.Alive || mobile.Map!=Map.Sosaria)continue;
                    Point3D point;bool allowed=ArachnidInfestationTerrain.TryPoint(mobile.X,mobile.Y,out point);
                    writer.WriteStartElement("citizen");Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"region",mobile.Region.Name);
                    Attr(writer,"regionType",mobile.Region.GetType().Name);Attr(writer,"allowed",allowed);Attr(writer,"blessed",mobile.Blessed);Attr(writer,"z",mobile.Z);Attr(writer,"groundZ",point.Z);
                    BaseCreature npc=mobile as BaseCreature;Attr(writer,"spawner",npc.Spawner==null?"none":npc.Spawner.GetType().FullName);writer.WriteEndElement();
                    if(!allowed || Math.Abs(mobile.Z-point.Z)>2 || mobile.GetType()!=typeof(Citizens))continue;
                    int quality=ArachnidInfestationTerrain.LootPlacement(point).WalkableCount;
                    if(quality<=best || quality<200)continue;
                    IPooledEnumerable nearby=Map.Sosaria.GetMobilesInRange(mobile.Location,12);
                    try
                    {
                        foreach(Mobile other in nearby)
                            if(other is TownGuards && other.Alive && !other.Deleted && Math.Abs(other.Z-point.Z)<=2
                                && ArachnidInfestationTerrain.Connected(other,point,other.Location))
                            {
                                TownOrigin=point;best=quality;++candidates;break;
                            }
                    }
                    finally{nearby.Free();}
                }
                writer.WriteElementString("selectedCandidates",candidates.ToString());writer.WriteEndElement();
            }
            if(TownOrigin==Point3D.Zero)CheckResult="No broad ordinary harmful-enabled town citizen/guard pair; town case must report this limit.";
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
            foreach(ArachnidOutbreakController owner in AggregateControllers)if(!owner.Deleted)owner.Delete();AggregateControllers.Clear();PendingDirectors=0;
            foreach(Item item in Fixtures)if(item!=null && !item.Deleted)item.Delete();Fixtures.Clear();
            foreach(Mobile mobile in FixtureNpcs)if(mobile!=null && !mobile.Deleted)mobile.Delete();FixtureNpcs.Clear();SelectedClones.Clear();
            FrontierFounder=null;FrontierGuard=null;CitizenSpawner=null;SeenCitizens.Clear();RespawnGenerations=0;
            if(Actor!=null && !Actor.Deleted)Actor.Blessed=false;
        }
        private static void StartTown()
        {
            Cleanup();Scene="town";GuardDeaths=CitizenDeaths=EventDeaths=0;TownNpcs.Clear();
            if(TownOrigin==Point3D.Zero)throw new Exception("No suitable town origin in survey.");
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
            Call(Controller,"StartTimer");Started=DateTime.UtcNow;
        }
        private static void Artwork()
        {
            Cleanup();Scene="artwork";Point3D center=new Point3D(2514,2088,0);
            Controller=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(Controller,"m_State")).SetEnabled(true);
            typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Controller,new object[]{center,DateTime.UtcNow});
            foreach(Mobile mobile in ((List<Mobile>)Get(Controller,"m_Creatures")).ToArray())mobile.Delete();
            ((List<Mobile>)Get(Controller,"m_Creatures")).Clear();
            int index=0;
            for(int tier=0;tier<5;++tier)
                for(int kind=0;kind<2;++kind)
                {
                    ArachnidInfestationCreature creature=new ArachnidInfestationCreature(Controller,0,tier,false,false);
                    typeof(ArachnidInfestationCreature).GetField("m_Kind",Private).SetValue(creature,kind);creature.ApplyTier(tier);
                    creature.Name="QA tier "+tier+" variant "+kind;creature.MoveToWorld(new Point3D(center.X-3+(index%2)*6,center.Y-10+(index/2)*5,0),Map.Sosaria);
                    ((List<Mobile>)Get(Controller,"m_Creatures")).Add(creature);if(creature.AIObject!=null)creature.AIObject.m_Timer.Stop();++index;
                }
            ArachnidInfestationCreature guardian=new ArachnidInfestationCreature(Controller,0,2,true,false);guardian.MoveToWorld(new Point3D(center.X,center.Y+8,0),Map.Sosaria);
            ((List<Mobile>)Get(Controller,"m_Creatures")).Add(guardian);if(guardian.AIObject!=null)guardian.AIObject.m_Timer.Stop();
            ArachnidOutbreakChampion queen=new ArachnidOutbreakChampion(Controller);queen.MoveToWorld(new Point3D(center.X+8,center.Y+8,0),Map.Sosaria);
            ((List<Mobile>)Get(Controller,"m_Creatures")).Add(queen);queen.AIObject.m_Timer.Stop();
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
                    if(command=="town")StartTown();else if(command=="artwork")Artwork();else if(command=="cleanup")Cleanup();
                    else if(command=="aggregate")
                    {
                        Cleanup();Scene="aggregate";Started=DateTime.UtcNow;PendingDirectors=16;
                        Actor.Blessed=true;Actor.Combatant=null;Actor.Warmode=false;Actor.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria);
                    }
                    else if(command=="withdraw")
                    { Actor.Combatant=null;Actor.Warmode=false;Actor.Blessed=true;Actor.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria); }
                    else if(command=="return" && Controller!=null)
                    { ArachnidInfestationState colony=(ArachnidInfestationState)Get(Controller,"m_Infestation");Actor.MoveToWorld(new Point3D(colony.Origin.X-8,colony.Origin.Y,colony.Origin.Z),Map.Sosaria); }
                    else if(command=="growth" || command=="growth-offsite" || command=="frontier" || command=="respawn" || command=="house" || command=="door" || command=="portal" || command.StartsWith("clone-"))StartCase(command);
                    else if(command!="survey")throw new Exception("Unknown finite fixture request.");
                }
                if(PendingDirectors>0)
                {
                    ArachnidOutbreakController owner=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(owner,"m_State")).SetEnabled(true);
                    AggregateControllers.Add(owner);if(Controller==null)Controller=owner;--PendingDirectors;
                    Call(owner,"StartTimer");
                }
                CaseWork();
                Status();
                if(File.Exists(Path.Combine(Evidence,"native-stop.txt")) || DateTime.UtcNow>Deadline)
                {
                    Cleanup();Poll.Stop();AutoSave.SavesEnabled=false;File.WriteAllText(Path.Combine(Evidence,"native-stopped.txt"),"Owned infestation fixture stopped without saving.");
                }
            }
            catch(Exception error)
            {
                Failure=error.ToString();File.WriteAllText(Path.Combine(Evidence,"fixture-failure.txt"),Failure);
                try{Status();}catch(Exception statusError){File.AppendAllText(Path.Combine(Evidence,"fixture-failure.txt"),"\nStatus: "+statusError);}
                Cleanup();if(Poll!=null)Poll.Stop();
            }
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
                Attr(writer,"check",CheckResult);Attr(writer,"respawnGenerations",RespawnGenerations);Attr(writer,"selectedClones",SelectedClones.Count);
                Attr(writer,"aggregateActors",SpreadingEncounterScheduler.ActorCount);Attr(writer,"aggregateScenery",SpreadingEncounterScheduler.SceneryCount);
                Attr(writer,"schedulerTurns",SpreadingEncounterScheduler.Turns);Attr(writer,"deferredTurns",SpreadingEncounterScheduler.DeferredTurns);
                using(System.Diagnostics.Process process=System.Diagnostics.Process.GetCurrentProcess())
                {Attr(writer,"privateMemory",process.PrivateMemorySize64);Attr(writer,"workingSet",process.WorkingSet64);Attr(writer,"cpuSeconds",process.TotalProcessorTime.TotalSeconds);}
                if(Controller!=null)
                {
                    Attr(writer,"phase",Controller.Phase);Attr(writer,"population",Controller.InfestationPopulation);
                    ArachnidInfestationState colony=Get(Controller,"m_Infestation") as ArachnidInfestationState;
                    if(colony!=null)
                    {
                        Attr(writer,"patches",colony.Patches.Count);Attr(writer,"epoch",colony.Epoch);Attr(writer,"births",colony.Births);Attr(writer,"terrainProbes",colony.TerrainProbes);Attr(writer,"routeRequests",colony.RouteRequests);
                        foreach(ArachnidInfestationPatch patch in colony.Patches)
                        {writer.WriteStartElement("patch");Attr(writer,"id",patch.Id);Attr(writer,"parent",patch.Parent);Attr(writer,"x",patch.Center.X);Attr(writer,"y",patch.Center.Y);Attr(writer,"established",patch.Established);Attr(writer,"retired",patch.Retired);Attr(writer,"tier",colony.Tier(patch));writer.WriteEndElement();}
                    }
                    foreach(Mobile mobile in (List<Mobile>)Get(Controller,"m_Creatures"))
                        if(mobile!=null && !mobile.Deleted){writer.WriteStartElement("creature");Describe(writer,mobile);writer.WriteEndElement();}
                }
                foreach(ArachnidOutbreakController owner in AggregateControllers)
                {
                    writer.WriteStartElement("director");Attr(writer,"serial",owner.Serial);Attr(writer,"phase",owner.Phase);Attr(writer,"population",owner.InfestationPopulation);Attr(writer,"status",owner.Status);
                    int running=0;
                    foreach(Mobile mobile in (List<Mobile>)Get(owner,"m_Creatures"))
                    {BaseCreature creature=mobile as BaseCreature;if(creature!=null && !creature.Deleted && creature.AIObject!=null && creature.AIObject.m_Timer.Running)++running;}
                    Attr(writer,"runningAi",running);
                    Attr(writer,"terrainGrants",SpreadingEncounterScheduler.WorkGrants(owner,0));Attr(writer,"routeGrants",SpreadingEncounterScheduler.WorkGrants(owner,1));
                    Attr(writer,"actorGrants",SpreadingEncounterScheduler.WorkGrants(owner,2));Attr(writer,"sceneryGrants",SpreadingEncounterScheduler.WorkGrants(owner,3));
                    ArachnidInfestationState state=Get(owner,"m_Infestation") as ArachnidInfestationState;
                    if(state!=null)
                    {
                        int dormant=0,residents=0;foreach(ArachnidInfestationPatch patch in state.Patches)
                            foreach(ArachnidInfestationResident resident in patch.Residents){++residents;if(resident.Actor==null && resident.Hits>0)++dormant;}
                        Attr(writer,"epoch",state.Epoch);Attr(writer,"patches",state.Patches.Count);Attr(writer,"residents",residents);Attr(writer,"dormant",dormant);
                        Attr(writer,"lastTick",state.LastTick.ToString("o"));Attr(writer,"births",state.Births);
                    }
                    writer.WriteEndElement();
                }
                foreach(Mobile npc in TownNpcs)
                {
                    writer.WriteStartElement("npc");Describe(writer,npc);writer.WriteEndElement();
                }
                foreach(CharacterClone clone in SelectedClones){writer.WriteStartElement("selectedClone");Attr(writer,"alias","P"+Players.IndexOf(clone.Original as PlayerMobile));Describe(writer,clone);writer.WriteEndElement();}
                writer.WriteEndElement();
            }
            if(File.Exists(file))File.Replace(file+".tmp",file,null);else File.Move(file+".tmp",file);
        }

        private static void Describe(XmlWriter writer,Mobile mobile)
        {
            Attr(writer,"serial",mobile.Serial.Value);Attr(writer,"type",mobile.GetType().Name);Attr(writer,"alive",mobile.Alive && !mobile.Deleted);
            Attr(writer,"body",mobile.BodyValue);Attr(writer,"hits",mobile.Hits);Attr(writer,"hitsMax",mobile.HitsMax);Attr(writer,"mana",mobile.Mana);Attr(writer,"stam",mobile.Stam);
            Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"z",mobile.Z);Attr(writer,"combatant",mobile.Combatant==null?"none":mobile.Combatant.GetType().Name);
            Attr(writer,"frozen",mobile.Frozen);Attr(writer,"paralyzed",mobile.Paralyzed);
            BaseCreature creature=mobile as BaseCreature;
            ArachnidInfestationCreature survivor=mobile as ArachnidInfestationCreature;
            if(survivor!=null){Attr(writer,"identity",survivor.Identity);Attr(writer,"slot",survivor.Slot);Attr(writer,"tier",survivor.Tier);Attr(writer,"kind",survivor.Kind);}
            if(creature!=null)
            {
                Attr(writer,"focus",creature.FocusMob==null?"none":creature.FocusMob.GetType().Name);
                Attr(writer,"statMods",creature.StatMods.Count);Attr(writer,"skillMods",creature.SkillMods.Count);
                Attr(writer,"interested",SpreadingEncounterScheduler.Interested(creature));Attr(writer,"busy",SpreadingEncounterScheduler.Busy(creature));
            }
            if(creature!=null){Attr(writer,"ai",creature.AIObject==null?"none":creature.AIObject.GetType().Name);Attr(writer,"action",creature.AIObject==null?"none":creature.AIObject.Action.ToString());Attr(writer,"aiRunning",creature.AIObject!=null && creature.AIObject.m_Timer.Running);}
        }

        private static void StartCase(string name)
        {
            Cleanup();Scene=name;CheckResult=String.Empty;GuardDeaths=CitizenDeaths=EventDeaths=0;TownNpcs.Clear();
            Point3D center=new Point3D(2514,2088,0);
            Controller=new ArachnidOutbreakController();((ArachnidOutbreakState)Get(Controller,"m_State")).SetEnabled(true);
            if(!(bool)typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Controller,new object[]{center,DateTime.UtcNow}))throw new Exception("Case origin rejected.");
            Actor.Combatant=null;Actor.Warmode=false;Actor.Blessed=true;Actor.MoveToWorld(new Point3D(center.X-8,center.Y,center.Z),Map.Sosaria);
            Call(Controller,"StartTimer");Started=DateTime.UtcNow;
            if(name=="growth-offsite")Actor.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria);
            if(name=="respawn")
            {
                CitizenSpawner=new XmlSpawner(1,1,1,0,0,"Citizens");CitizenSpawner.MoveToWorld(new Point3D(center.X+3,center.Y+2,center.Z),Map.Sosaria);
                CitizenSpawner.MinDelay=TimeSpan.FromSeconds(5);CitizenSpawner.MaxDelay=TimeSpan.FromSeconds(5);CitizenSpawner.NextSpawn=TimeSpan.FromSeconds(1);Fixtures.Add(CitizenSpawner);
            }
            else if(name=="house")
            {
                SmallOldHouse house=new SmallOldHouse(Actor,0x64);house.MoveToWorld(center,Map.Sosaria);Fixtures.Add(house);
            }
            else if(name=="door") {BaseDoor door=new DarkWoodDoor(DoorFacing.NorthCW);door.MoveToWorld(center,Map.Sosaria);Fixtures.Add(door);}
            else if(name=="portal") {Moongate gate=new Moongate();gate.MoveToWorld(center,Map.Sosaria);Fixtures.Add(gate);}
            else if(name.StartsWith("clone-"))SelectClones(name.Substring(6),center);
        }

        private static void CaseWork()
        {
            if(Controller==null)return;
            ArachnidInfestationState colony=Get(Controller,"m_Infestation") as ArachnidInfestationState;
            if(colony==null)return;
            if(Scene=="frontier" && FrontierFounder==null)
                foreach(ArachnidInfestationPatch patch in colony.Patches)
                    if(!patch.Established && !patch.Retired && patch.Founder!=null)
                    {
                        Point3D point;
                        for(int x=-1;x<=1 && FrontierGuard==null;++x)for(int y=-1;y<=1;++y)
                            if((x!=0 || y!=0) && ArachnidInfestationTerrain.TryPoint(patch.Founder.X+x,patch.Founder.Y+y,out point) && Map.Sosaria.CanSpawnMobile(point))
                            {FrontierFounder=patch.Founder;FrontierGuard=new TownGuards();FrontierGuard.MoveToWorld(point,Map.Sosaria);FixtureNpcs.Add(FrontierGuard);TownNpcs.Add(FrontierGuard);break;}
                        break;
                    }
            if(Scene=="frontier" && FrontierFounder!=null && (!FrontierFounder.Alive || FrontierFounder.Deleted))
                CheckResult="Founder removed during real guard AI case; epoch="+colony.Epoch+"; event deaths="+EventDeaths;
            if(Scene=="house" || Scene=="door" || Scene=="portal")
                CheckResult="root retired="+colony.Patches[0].Retired+"; queen population="+Controller.InfestationPopulation+"; external fixture retained="+(Fixtures.Count>0 && !Fixtures[0].Deleted);
            if(CitizenSpawner!=null)
            {
                IPooledEnumerable near=Map.Sosaria.GetMobilesInRange(CitizenSpawner.Location,2);
                try{foreach(Mobile mobile in near)if(mobile.GetType()==typeof(Citizens) && mobile.Spawner==CitizenSpawner && !SeenCitizens.ContainsKey(mobile.Serial.Value)){SeenCitizens.Add(mobile.Serial.Value,true);++RespawnGenerations;TownNpcs.Add(mobile);}}
                finally{near.Free();}
            }
        }

        private static double SkillScore(PlayerMobile player,bool caster)
        {
            double result=0;
            for(int i=0;i<player.Skills.Length;++i)
            {
                Skill skill=player.Skills[i];
                if(caster ? skill.SkillName==SkillName.Magery || skill.SkillName==SkillName.Necromancy || skill.SkillName==SkillName.Psychology
                    : skill.SkillName==SkillName.Swords || skill.SkillName==SkillName.Fencing || skill.SkillName==SkillName.Bludgeoning || skill.SkillName==SkillName.Tactics || skill.SkillName==SkillName.Marksmanship)result+=skill.Value;
            }
            return result;
        }
        private static void SelectClones(string role,Point3D center)
        {
            List<PlayerMobile> choices=new List<PlayerMobile>(Players);choices.RemoveAll(delegate(PlayerMobile p){return p==Actor || !p.Alive || p.Blessed;});
            if(role=="beginner")choices.Sort(delegate(PlayerMobile a,PlayerMobile b){return a.Skills.Total.CompareTo(b.Skills.Total);});
            else if(role=="hybrid")choices.Sort(delegate(PlayerMobile a,PlayerMobile b){return Math.Min(SkillScore(b,true),SkillScore(b,false)).CompareTo(Math.Min(SkillScore(a,true),SkillScore(a,false)));});
            else choices.Sort(delegate(PlayerMobile a,PlayerMobile b){return SkillScore(b,role=="caster").CompareTo(SkillScore(a,role=="caster"));});
            if(role=="group" && choices.Count>1)
            {
                PlayerMobile martial=choices[0];choices.RemoveAt(0);
                choices.Sort(delegate(PlayerMobile a,PlayerMobile b){return SkillScore(b,true).CompareTo(SkillScore(a,true));});
                PlayerMobile caster=choices[0];choices.RemoveAt(0);choices.Sort(delegate(PlayerMobile a,PlayerMobile b){return a.Skills.Total.CompareTo(b.Skills.Total);});
                choices.Insert(0,caster);choices.Insert(0,martial);
            }
            int maximum=role=="group"?Math.Min(3,choices.Count):Math.Min(1,choices.Count);
            if(maximum==0)throw new Exception("No eligible saved Player for selected clone case.");
            for(int i=0;i<maximum;++i)
            {
                PlayerMobile original=choices[i];CharacterClone clone=CloneThings.CreateClone(original);CloneThings.CloneMobileProperties(original,clone);
                CloneThings.CloneMobileItems(original,clone);CloneThings.CloneMobileBackpack(original,clone);CloneThings.CloneMobileMount(original,clone);
                clone.Team=190;clone.FightMode=FightMode.Closest;clone.Home=center;clone.RangeHome=24;
                clone.MoveToWorld(new Point3D(center.X-5,center.Y+i*2,center.Z),Map.Sosaria);SelectedClones.Add(clone);FixtureNpcs.Add(clone);
            }
            CheckResult="Selected "+maximum+" copied offline build(s); existing OmniAI, Team/FightMode/staging fixture changes; source-normalized saved stats; spell/toolkit behavior and missing companions remain separate fidelity checks.";
        }

        private static void InventoryPlayers()
        {
            foreach(Mobile mobile in World.Mobiles.Values)if(mobile is PlayerMobile && mobile.AccessLevel==AccessLevel.Player)Players.Add((PlayerMobile)mobile);
            Players.Sort(delegate(PlayerMobile a,PlayerMobile b){return a.Serial.Value.CompareTo(b.Serial.Value);});
            using(XmlWriter writer=XmlWriter.Create(Path.Combine(Evidence,"saved-builds.private.xml"),new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("copiedHistoricalBuilds");Attr(writer,"scope","Matching deserialization of October 17 copied world; no account data; no current human-playstyle claim.");
                for(int i=0;i<Players.Count;++i)
                {
                    PlayerMobile player=Players[i];writer.WriteStartElement("build");Attr(writer,"alias","P"+i);Attr(writer,"sourceSerial",player.Serial.Value);
                    Attr(writer,"str",player.Str);Attr(writer,"dex",player.Dex);Attr(writer,"int",player.Int);Attr(writer,"hitsMax",player.HitsMax);Attr(writer,"manaMax",player.ManaMax);Attr(writer,"skillTotal",player.Skills.Total);
                    Attr(writer,"physical",player.PhysicalResistance);Attr(writer,"fire",player.FireResistance);Attr(writer,"cold",player.ColdResistance);Attr(writer,"poison",player.PoisonResistance);Attr(writer,"energy",player.EnergyResistance);Attr(writer,"followersMax",player.FollowersMax);
                    Attr(writer,"casterScore",SkillScore(player,true));Attr(writer,"martialScore",SkillScore(player,false));
                    for(int skillIndex=0;skillIndex<player.Skills.Length;++skillIndex){Skill skill=player.Skills[skillIndex];if(skill.Base>0){writer.WriteStartElement("skill");Attr(writer,"name",skill.SkillName);Attr(writer,"base",skill.Base);Attr(writer,"effective",skill.Value);writer.WriteEndElement();}}
                    foreach(Item item in player.Items)if(item.Layer!=Layer.Bank)InventoryItem(writer,item,0);
                    if(String.Equals(player.Name,"Sargar",StringComparison.OrdinalIgnoreCase))
                        foreach(Mobile mobile in player.Stabled)
                        {
                            BaseCreature pet=mobile as BaseCreature;if(pet==null)continue;
                            writer.WriteStartElement("stabledPet");Attr(writer,"type",pet.GetType().FullName);Attr(writer,"sourceSerial",pet.Serial.Value);
                            Attr(writer,"name",pet.Name);Attr(writer,"slots",pet.ControlSlots);Attr(writer,"loyalty",pet.Loyalty);Attr(writer,"minTame",pet.MinTameSkill);Attr(writer,"bonded",pet.IsBonded);
                            Attr(writer,"hits",pet.Hits);Attr(writer,"hitsMax",pet.HitsMax);Attr(writer,"damageMin",pet.DamageMin);Attr(writer,"damageMax",pet.DamageMax);Attr(writer,"ability",pet.GetWeaponAbility()==null?"none":pet.GetWeaponAbility().GetType().Name);
                            writer.WriteEndElement();
                        }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
        private static void InventoryItem(XmlWriter writer,Item item,int depth)
        {
            if(depth>8)return;
            writer.WriteStartElement("item");Attr(writer,"type",item.GetType().FullName);Attr(writer,"layer",item.Layer);Attr(writer,"amount",item.Amount);
            Spellbook book=item as Spellbook;if(book!=null)Attr(writer,"spellContent",book.Content);
            foreach(string propertyName in new string[]{"Attributes","WeaponAttributes","ArmorAttributes","AbsorptionAttributes"})
            {
                PropertyInfo property=item.GetType().GetProperty(propertyName);if(property==null)continue;object attributes=property.GetValue(item,null);if(attributes==null)continue;
                foreach(PropertyInfo valueProperty in attributes.GetType().GetProperties())
                    if(valueProperty.CanRead && valueProperty.GetIndexParameters().Length==0 && valueProperty.PropertyType==typeof(int))
                    {int value=(int)valueProperty.GetValue(attributes,null);if(value!=0){writer.WriteStartElement("attribute");Attr(writer,"group",propertyName);Attr(writer,"name",valueProperty.Name);Attr(writer,"value",value);writer.WriteEndElement();}}
            }
            if(item is ResearchBag || item is AncientSpellbook)
                foreach(FieldInfo field in item.GetType().GetFields(BindingFlags.Instance|BindingFlags.Public))if(field.FieldType==typeof(int))
                {writer.WriteStartElement("research");Attr(writer,"field",field.Name);Attr(writer,"value",field.GetValue(item));writer.WriteEndElement();}
            Container container=item as Container;if(container!=null)foreach(Item child in container.Items)InventoryItem(writer,child,depth+1);
            writer.WriteEndElement();
        }
    }
}
