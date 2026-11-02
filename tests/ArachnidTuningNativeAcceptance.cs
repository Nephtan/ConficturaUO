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
    public static class ArachnidTuningResumeCapture
    {
        [CallPriority(-1000000)]
        public static void Initialize() { ArachnidTuningNativeAcceptance.CaptureCopiedAttempt(); }
    }
    public static class ArachnidTuningNativeAcceptance
    {
        private const string ExpectedRoot = "__TUNING_NATIVE_ROOT__";
        private static readonly string Evidence = Path.GetFullPath(Path.Combine(ExpectedRoot,"../evidence"));
        private static readonly BindingFlags Hidden = BindingFlags.Instance|BindingFlags.NonPublic;
        private sealed class Session
        {
            public string Role, State = "intro", Mode = "idle";
            public PlayerMobile Actor;
            public DateTime Started;
            public int Deaths, Incoming, Direct, PetDamage, QueenDamage, ResponseDamage, StartWarnings, TargetSerial, TargetPatch = -1;
            public Point3D Opening;
            public Point3D SafePoint;
            public Map SafeMap;
            public readonly List<Point3D> Goals = new List<Point3D>();
        }
        private static readonly Dictionary<string,Session> Sessions = new Dictionary<string,Session>();
        private static ArachnidOutbreakController Owner;
        private static ArachnidInfestationState Colony;
        private static ArachnidOutbreakChampion Queen;
        private static ArachnidOutbreakController NaturalOwner;
        private static DateTime NaturalStarted;
        private static readonly Queue<Point3D> SeedQueue = new Queue<Point3D>();
        private static readonly Dictionary<Point2D,bool> SeedSeen = new Dictionary<Point2D,bool>();
        private static string State = "preparing", Failure = String.Empty;
        private static DateTime NextStatus, SeedStarted;
        private static bool SeedDone, Registered;
        private static int CitizensDied, GuardsDied;
        private static object Get(object owner,string name){return owner.GetType().GetField(name,Hidden).GetValue(owner);}
        private static object Call(object owner,string method,params object[] args){return owner.GetType().InvokeMember(method,Hidden|BindingFlags.InvokeMethod,null,owner,args);}


        // Test-only resume bridge: production activation remains disabled. Remove only the
        // one copied controller during its disabled-production Restore, then use its real
        // recovery and scheduler. No actor, clock, AI, equipment or resource substitution.
        private static ArachnidOutbreakController SavedOwner;
        public static void CaptureCopiedAttempt()
        {
            if (!String.Equals(Core.BaseDirectory.TrimEnd('\\'),ExpectedRoot,StringComparison.OrdinalIgnoreCase)
                || !ExpectedRoot.StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase)
                || !File.Exists(Path.Combine(Evidence,"native-private-root.txt")) || ArachnidOutbreakController.DeliveryReady)
                throw new Exception("Exact disabled-production copied resume root required");
            if(!File.Exists(Path.Combine(Evidence,"resume-copy.txt")))
            {
                if(!File.Exists(Path.Combine(Evidence,"declared-frontier-stage.txt")))throw new Exception("Explicit resume or constructed-geometry mode required");
                return;
            }
            List<ArachnidOutbreakController> controllers=(List<ArachnidOutbreakController>)typeof(ArachnidOutbreakController).GetField("m_Controllers",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
            foreach(ArachnidOutbreakController candidate in controllers.ToArray())
                if(Get(candidate,"m_Infestation")!=null)
                {
                    if(SavedOwner!=null)throw new Exception("Exactly one saved colony required");
                    SavedOwner=candidate;controllers.Remove(candidate);
                }
            if(SavedOwner==null)throw new Exception("No saved colony to resume");
        }
        private static void ResumeCopiedAttempt()
        {
            Owner=SavedOwner;Colony=(ArachnidInfestationState)Get(Owner,"m_Infestation");
            List<ArachnidOutbreakController> controllers=(List<ArachnidOutbreakController>)typeof(ArachnidOutbreakController).GetField("m_Controllers",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
            if(controllers.Contains(Owner))throw new Exception("Copied controller restored too early");
            controllers.Add(Owner);
            XmlDocument settings=new XmlDocument();settings.XmlResolver=null;settings.Load(Path.Combine(Evidence,"cycle-settings.xml"));
            ArachnidOutbreakTuning tuning=Owner.Tuning;
            tuning.InnerPopulationPercent=Int32.Parse(settings.DocumentElement.GetAttribute("population"));
            tuning.ReplacementSeconds=Int32.Parse(settings.DocumentElement.GetAttribute("replacement"));
            tuning.Validate();
            typeof(ArachnidOutbreakController).GetField("m_Tuning",Hidden).SetValue(Owner,tuning);
            Colony.Resume(DateTime.UtcNow);Call(Owner,"RecoverInfestation");
            foreach(Mobile mobile in (List<Mobile>)Get(Owner,"m_Creatures"))if(mobile is ArachnidOutbreakChampion)Queen=(ArachnidOutbreakChampion)mobile;
            if(Queen==null || !Queen.Alive)throw new Exception("Saved queen missing");
            SpreadingEncounterScheduler.Register(Owner);SeedDone=true;Registered=true;
            File.WriteAllText(Path.Combine(Evidence,"resume-bridge.txt"),"Copied saved attempt resumed through real recovery and scheduler; disabled-production Restore skipped only in this exact private harness. Initial comparison settings applied before clients connect. Ordinary clock/AI; no grant/heal/damage/patch construction.");
        }

        [CallPriority(1000000)]
        public static void Initialize()
        {
            if (!String.Equals(Core.BaseDirectory.TrimEnd('\\'),ExpectedRoot,StringComparison.OrdinalIgnoreCase)
                || !File.Exists(Path.Combine(Evidence,"native-private-root.txt")) || ArachnidOutbreakController.DeliveryReady
                || typeof(Core).Assembly.GetType("Server.PreviewClock") != null) throw new Exception("Exact unaccelerated disabled-production native copy required");
            AutoSave.SavesEnabled = false;
            if(File.Exists(Path.Combine(Evidence,"resume-copy.txt")))ResumeCopiedAttempt();
            EventSink.HealthChanged += Health; EventSink.MobileDeath += Death;
            Timer.DelayCall(TimeSpan.FromSeconds(1),Prepare);
            Timer.DelayCall(TimeSpan.FromSeconds(2),TimeSpan.FromMilliseconds(250),Tick);
        }
        private static void Prepare()
        {
            try
            {
                XmlDocument roles = new XmlDocument(); roles.XmlResolver = null; roles.Load(Path.Combine(Evidence,"client-roles.private.xml"));
                foreach(XmlElement role in roles.SelectNodes("roles/role"))
                {
                    PlayerMobile actor = World.FindMobile(Int32.Parse(role.GetAttribute("serial"))) as PlayerMobile;
                    if(actor == null || !actor.Alive || actor.Blessed || actor.AccessLevel != AccessLevel.Player) throw new Exception("Saved vulnerable Player missing for role " + role.GetAttribute("role"));
                    Session session = new Session(); session.Role = role.GetAttribute("role"); session.Actor = actor;
                    session.SafePoint=actor.Map==Map.Internal ? actor.LogoutLocation : actor.Location;
                    session.SafeMap=actor.Map==Map.Internal ? actor.LogoutMap : actor.Map;
                    Manifest(session,"saved");
                    IAccount previous = actor.Account;
                    if(previous != null) for(int slot=0;slot<previous.Length;++slot)if(previous[slot] == actor){previous[slot] = null;break;}
                    Account account = new Account(role.GetAttribute("username"),role.GetAttribute("password")); account[0] = actor;
                    actor.Combatant = null; actor.Warmode = false;
                    string safeIntro=Path.Combine(Evidence,"declared-safe-intro.xml");
                    if(File.Exists(safeIntro))
                    {
                        XmlDocument safe=new XmlDocument();safe.XmlResolver=null;safe.Load(safeIntro);
                        XmlElement point=(XmlElement)safe.SelectSingleNode("points/point[@serial='"+actor.Serial.Value+"']");
                        if(point!=null)
                        {
                            Map map=actor.Map==Map.Internal ? actor.LogoutMap : actor.Map;
                            if(map==null || map==Map.Internal)throw new Exception("Safe intro requires the saved actor's real map");
                            Point3D location=new Point3D(Int32.Parse(point.GetAttribute("x")),Int32.Parse(point.GetAttribute("y")),Int32.Parse(point.GetAttribute("z")));
                            session.SafeMap=map;session.SafePoint=location;
                            if(actor.Map==Map.Internal)actor.LogoutLocation=location;else actor.MoveToWorld(location,map);
                            File.AppendAllText(Path.Combine(Evidence,"safe-intro-interventions.tsv"),DateTime.UtcNow.ToString("o")+"\t"+session.Role+"\t"+location+"\tdeclared original precombat location; no health/resource grant\n");
                        }
                    }
                    Sessions.Add(session.Role,session); Manifest(session,"intro");
                }
                State = "intro-ready";
                File.WriteAllText(Path.Combine(Evidence,"intro-ready.txt"),"Actual saved builds retain resources; optional original precombat safe-location relocation is logged separately. No case or macro started; ordinary clock.");
            }
            catch(Exception error){Failure=error.ToString();State="failed";}
            Status();
        }
        private static void BeginNatural()
        {
            if(NaturalOwner!=null)throw new Exception("Natural colony already retained");
            NaturalOwner=new ArachnidOutbreakController();NaturalStarted=DateTime.UtcNow;
            XmlDocument tuning=new XmlDocument();tuning.XmlResolver=null;tuning.Load(Path.Combine(Evidence,"cycle-settings.xml"));
            NaturalOwner.InnerPopulationPercent=Int32.Parse(tuning.DocumentElement.GetAttribute("population"));
            NaturalOwner.ReplacementSeconds=Int32.Parse(tuning.DocumentElement.GetAttribute("replacement"));
            ((ArachnidOutbreakState)Get(NaturalOwner,"m_State")).SetEnabled(true);
            SpreadingEncounterScheduler.Register(NaturalOwner);
            File.AppendAllText(Path.Combine(Evidence,"case-events.tsv"),DateTime.UtcNow.ToString("o")+"\tnatural-origin\tordinary search, founder walking and timers; no specified origin or constructed patches\n");
        }
        private static Point3D Ground(int x,int y)
        {
            for(int radius=0;radius<24;++radius)for(int dx=-radius;dx<=radius;++dx)for(int dy=-radius;dy<=radius;++dy)
            {
                Point3D point;
                if(ArachnidInfestationTerrain.TryPoint(x+dx,y+dy,out point) && point.Z>-100 && Map.Sosaria.CanSpawnMobile(point))return point;
            }
            throw new Exception("No legal staged ground near " + x + "," + y);
        }
        private static void BeginSeed()
        {
            if(Owner != null)throw new Exception("Existing test colony retained; no implicit cleanup");
            Owner = new ArachnidOutbreakController(); ((ArachnidOutbreakState)Get(Owner,"m_State")).SetEnabled(true);
            if(!(bool)Call(Owner,"BeginInfestation",Ground(2514,2088),DateTime.UtcNow))throw new Exception("Natural fixture origin refused");
            Colony = (ArachnidInfestationState)Get(Owner,"m_Infestation");
            foreach(Mobile mobile in (List<Mobile>)Get(Owner,"m_Creatures"))if(mobile is ArachnidOutbreakChampion)Queen=(ArachnidOutbreakChampion)mobile;
            SeedQueue.Enqueue(Colony.Origin);SeedSeen.Add(new Point2D(Colony.Origin.X,Colony.Origin.Y),true);
            SeedStarted=DateTime.UtcNow;State="staging-territory";
        }
        private static void Seed()
        {
            int[] dx={0,12,0,-12},dy={-12,0,12,0};
            for(int work=0;work<4 && SeedQueue.Count>0;++work)
            {
                Point3D from=SeedQueue.Dequeue();ArachnidInfestationPatch parent=Colony.Cell(from.X,from.Y);
                for(int i=0;i<4;++i)
                {
                    int x=from.X+dx[i],y=from.Y+dy[i];Point2D key=new Point2D(x,y);Point3D point;
                    if(x<Colony.Origin.X-216 || x>Colony.Origin.X+420 || y<Colony.Origin.Y-72 || y>Colony.Origin.Y+72 || SeedSeen.ContainsKey(key))continue;
                    SeedSeen.Add(key,true);
                    if(!ArachnidInfestationTerrain.TryPoint(x,y,out point) || !SpreadingEncounterScheduler.CanReserve(Owner,Map.Sosaria,point)
                        || !ArachnidInfestationTerrain.Connected(Queen,from,point))continue;
                    ArachnidInfestationPatch patch=new ArachnidInfestationPatch();patch.Id=Colony.Patches.Count;patch.Parent=parent.Id;patch.Center=point;
                    patch.Established=true;patch.NextRefill=DateTime.UtcNow;patch.LastInterest=DateTime.UtcNow;
                    Colony.Add(patch);SpreadingEncounterScheduler.Reserve(Owner,Map.Sosaria,patch.Id,point);
                    Call(Owner,"EstablishPopulation",patch,DateTime.UtcNow);SeedQueue.Enqueue(point);
                }
            }
            if(SeedQueue.Count==0)
            {
                SeedDone=true;State="territory-ready";
                File.WriteAllText(Path.Combine(Evidence,"staged-territory.txt"),"Declared connected geometry fixture, not naturally aged growth. Cells="+Colony.Patches.Count+"; stage wall seconds="+(DateTime.UtcNow-SeedStarted).TotalSeconds+". All created actors retain ordinary AI; director starts only on an explicit test command.");
            }
        }
        private static void BeginCase(string role,string mode,bool preview=false)
        {
            bool naturalCase=mode.StartsWith("natural-");
            if((!naturalCase && !SeedDone) || !Sessions.ContainsKey(role))throw new Exception("Case requires completed private staging and saved role");
            Session session=Sessions[role];if((!preview && session.Actor.NetState==null) || !session.Actor.Alive)throw new Exception("Case requires connected living role");
            if(!preview)
            {
                string ready=Path.Combine(Evidence,"driver-ready-"+role+".txt");
                string heartbeat=Path.Combine(Evidence,"driver-heartbeat-"+role+".txt");
                if(File.Exists(Path.Combine(Evidence,"client-"+role+"-error.txt")) || !File.Exists(ready) || File.ReadAllText(ready)!=session.Actor.Serial.Value.ToString()
                    || !File.Exists(heartbeat) || DateTime.UtcNow-File.GetLastWriteTimeUtc(heartbeat)>TimeSpan.FromSeconds(5))throw new Exception("Fresh prepared client driver required; errors prohibit staging");
            }
            if(session.State=="running")throw new Exception("Existing case must reach its boundary before another starts");
            session.Goals.Clear();session.TargetPatch=-1;session.TargetSerial=0;
            int x=Colony==null?0:Colony.Origin.X,y=Colony==null?0:Colony.Origin.Y;
            if(mode=="queen-migration")
            { session.Opening=Ground(Queen.X-22,Queen.Y-12);session.Goals.Add(Queen.Location); }
            else if(mode=="passage")
            { session.Opening=Ground(x-240,y-36);session.Goals.Add(Ground(x-204,y-36));session.Goals.Add(Ground(x-180,y-36));session.Goals.Add(Ground(x-240,y-36)); }
            else if(mode=="sac")
            {
                ArachnidInfestationPatch egg=FindEgg(x-204,y);
                if(egg==null)throw new Exception("No viable staged outer sac");
                session.TargetSerial=egg.Guardian.Serial.Value;session.TargetPatch=egg.Id;
                session.Opening=Ground(egg.Center.X-12,egg.Center.Y);session.Goals.Add(egg.Center);session.Goals.Add(Ground(x-240,y-36));
            }
            else if(mode=="resistance" || mode=="ranger-hunt")
            {
                ArachnidInfestationPatch entry=null;int nearest=Int32.MaxValue;
                foreach(ArachnidInfestationPatch patch in Colony.Patches)
                {
                    if(!patch.Established || patch.Retired || Colony.Tier(patch)!=0)continue;
                    int distance=Math.Max(Math.Abs(patch.Center.X-(x-180)),Math.Abs(patch.Center.Y-(y-36)));
                    if(distance<nearest){entry=patch;nearest=distance;}
                }
                if(entry==null)throw new Exception("No established outskirts entry");
                session.Opening=Ground(entry.Center.X,entry.Center.Y);
                foreach(ArachnidInfestationPatch patch in Colony.Patches)
                    if(patch.Established && !patch.Retired && Colony.Tier(patch)==0 && ArachnidTerritoryPolicy.HasFrontierSpider(patch.Center)
                        && Math.Max(Math.Abs(patch.Center.X-entry.Center.X),Math.Abs(patch.Center.Y-entry.Center.Y))<=60)session.Goals.Add(patch.Center);
                session.Goals.Add(Ground(x-240,y-36));
            }
            else if(mode=="bystander")
            {
                System.Collections.IDictionary responses=(System.Collections.IDictionary)Get(Owner,"m_NestResistance");
                ArachnidInfestationPatch anchor=null;
                foreach(object key in responses.Keys)
                {
                    ArachnidInfestationPatch candidate=Colony.Find((int)key);
                    if(candidate!=null && !candidate.Retired && candidate.ResponseBirths<9
                        && (anchor==null || candidate.ResponseBirths>anchor.ResponseBirths))anchor=candidate;
                }
                if(anchor==null)throw new Exception("Bystander case requires an actual local disturbance");
                bool found=false;
                for(int radius=20;radius<=36 && !found;radius+=4)for(int dx=-radius;dx<=radius && !found;dx+=4)for(int dy=-radius;dy<=radius && !found;dy+=4)
                {
                    Point3D point;if(!ArachnidInfestationTerrain.TryPoint(anchor.Center.X+dx,anchor.Center.Y+dy,out point) || !Map.Sosaria.CanSpawnMobile(point))continue;
                    bool occupied=false;IPooledEnumerable nearby=Map.Sosaria.GetMobilesInRange(point,10);
                    try{foreach(Mobile mobile in nearby)if(mobile.Alive && !mobile.Deleted){occupied=true;break;}}finally{nearby.Free();}
                    if(!occupied){session.Opening=point;found=true;}
                }
                if(!found)throw new Exception("No quiet legal bystander opening near the existing disturbance");
                session.Goals.Add(Ground(x-240,y-36));
            }
            else if(naturalCase)
            {
                ArachnidInfestationState natural=NaturalOwner==null?null:(ArachnidInfestationState)Get(NaturalOwner,"m_Infestation");
                if(natural==null)throw new Exception("Natural origin not yet established");
                session.Opening=Ground(natural.Origin.X-24,natural.Origin.Y-12);session.Goals.Add(natural.Origin);
            }
            else if(mode=="frontier-combat")
            {
                ArachnidInfestationPatch entry=null;int nearest=Int32.MaxValue;
                foreach(ArachnidInfestationPatch patch in Colony.Patches)
                {
                    if(!patch.Established || patch.Retired || Colony.Tier(patch)!=0 || !ArachnidTerritoryPolicy.HasFrontierSpider(patch.Center) || ArachnidTerritoryPolicy.HasEggSac(patch.Center))continue;
                    if(Math.Max(Math.Abs(patch.Center.X-Queen.X),Math.Abs(patch.Center.Y-Queen.Y))<320)continue;
                    bool quiet=true;IPooledEnumerable wildlife=Map.Sosaria.GetMobilesInRange(patch.Center,32);
                    try
                    {
                        foreach(Mobile mobile in wildlife)
                        {
                            IArachnidOutbreakOwned owned=mobile as IArachnidOutbreakOwned;
                            BaseCreature creature=mobile as BaseCreature;
                            if(mobile.Deleted || !mobile.Alive || mobile.Player || owned!=null && owned.Controller==Owner || creature!=null && (creature.Controlled || creature.Summoned))continue;
                            if(creature!=null){quiet=false;break;}
                        }
                    }
                    finally{wildlife.Free();}
                    if(!quiet)continue;
                    int distance=Math.Max(Math.Abs(patch.Center.X-(x-190)),Math.Abs(patch.Center.Y-(y-36)));
                    if(distance<nearest){entry=patch;nearest=distance;}
                }
                if(entry==null)throw new Exception("No ordinary non-sac weak frontier cell available");
                int awayX=entry.Center.X-Queen.X,awayY=entry.Center.Y-Queen.Y;
                if(Math.Abs(awayX)>=Math.Abs(awayY)){awayX=Math.Sign(awayX);awayY=0;}else{awayY=Math.Sign(awayY);awayX=0;}
                session.TargetPatch=entry.Id;session.Opening=Ground(entry.Center.X+awayX*7,entry.Center.Y+awayY*7);
                Point3D exit=Ground(entry.Center.X+awayX*36,entry.Center.Y+awayY*36);
                if(Math.Max(Math.Abs(exit.X-Queen.X),Math.Abs(exit.Y-Queen.Y))<=Math.Max(Math.Abs(entry.Center.X-Queen.X),Math.Abs(entry.Center.Y-Queen.Y)))throw new Exception("Outward escape ground unavailable");
                session.Goals.Add(entry.Center);session.Goals.Add(exit);
            }
            else if(mode=="veteran-combat" || mode=="build-combat" || mode=="bard-support")
            {
                int edge=Queen.X;
                foreach(ArachnidInfestationPatch patch in Colony.Patches)
                    if(patch.Established && !patch.Retired)edge=Math.Min(edge,patch.Center.X);
                session.Opening=Ground(edge-24,Queen.Y-12);session.Goals.Add(Queen.Location);session.Goals.Add(session.Opening);
            }
            else throw new Exception("Unknown native case");
            if(preview)
            {
                File.WriteAllText(Path.Combine(Evidence,"preflight-"+role+"-"+mode+".txt"),"Legal opening="+session.Opening+"; goalCount="+session.Goals.Count+"; target="+session.TargetSerial+". No actor moved, no case started, no resources changed.");
                return;
            }
            session.Mode=mode;session.State="running";session.Started=DateTime.UtcNow;session.StartWarnings=Owner==null?0:Owner.BroodWarnings;
            session.Incoming=session.Direct=session.PetDamage=session.QueenDamage=session.ResponseDamage=0;
            Manifest(session,"before-"+mode);session.Actor.Combatant=null;session.Actor.Warmode=false;
            BaseCreature.TeleportPets(session.Actor,session.Opening,Map.Sosaria);
            foreach(Mobile mobile in session.Actor.AllFollowers)
            {
                BaseCreature summon=mobile as BaseCreature;
                if(summon!=null && summon.Summoned && summon.SummonMaster==session.Actor && summon.Map==session.Actor.Map && summon.InRange(session.Actor,20))summon.MoveToWorld(session.Opening,Map.Sosaria);
            }
            session.Actor.MoveToWorld(session.Opening,Map.Sosaria);
            Manifest(session,"staged-"+mode);
            if(Owner!=null && !Registered){SpreadingEncounterScheduler.Register(Owner);Registered=true;}
            File.AppendAllText(Path.Combine(Evidence,"case-events.tsv"),DateTime.UtcNow.ToString("o")+"\tstart\t"+role+"\t"+mode+"\tinitial legal staging only; no resources/skills/gear granted\n");
        }
        private static ArachnidInfestationPatch FindEgg(int x,int y)
        {
            ArachnidInfestationPatch best=null;int nearest=Int32.MaxValue;
            foreach(ArachnidInfestationPatch patch in Colony.Patches)
            {
                ArachnidInfestationCreature actor=patch.Guardian as ArachnidInfestationCreature;
                if(patch.Retired || actor==null || actor.Deleted || !actor.Alive || !actor.EggSac)continue;
                int distance=Math.Max(Math.Abs(patch.Center.X-x),Math.Abs(patch.Center.Y-y));if(distance<nearest){nearest=distance;best=patch;}
            }
            return best;
        }
        private static void Tick()
        {
            if(State=="preparing" || State=="failed")return;
            try
            {
                string command=Path.Combine(Evidence,"command.txt");
                if(File.Exists(command))
                {
                    string text=File.ReadAllText(command).Trim();
                    string archive=Path.Combine(Evidence,"command-"+DateTime.UtcNow.Ticks+".txt");File.Move(command,archive);
                    try
                    {
                    if(text=="seed")BeginSeed();
                    else if(text=="natural-origin")BeginNatural();
                    else if(text=="use-natural")
                    {
                        if(Owner!=null || NaturalOwner==null)throw new Exception("Exactly one existing natural colony required");
                        Owner=NaturalOwner;Colony=(ArachnidInfestationState)Get(Owner,"m_Infestation");
                        if(Colony==null)throw new Exception("Natural origin not ready");
                        foreach(Mobile mobile in (List<Mobile>)Get(Owner,"m_Creatures"))if(mobile is ArachnidOutbreakChampion)Queen=(ArachnidOutbreakChampion)mobile;
                        NaturalOwner=null;SeedDone=true;Registered=true;State="territory-ready";
                        File.WriteAllText(Path.Combine(Evidence,"natural-adopted.txt"),"Existing ordinary-clock random-origin colony adopted for client cases; no patches constructed, no second controller created.");
                    }
                    else if(text.StartsWith("case ")){string[] parts=text.Split(' ');if(parts.Length!=3)throw new Exception("Bad case command");BeginCase(parts[1],parts[2]);}
                    else if(text.StartsWith("preflight ")){string[] parts=text.Split(' ');if(parts.Length!=3)throw new Exception("Bad preflight command");BeginCase(parts[1],parts[2],true);}
                    else if(text.StartsWith("finish ")){EndCase(Sessions[text.Substring(7)],"bounded-window-ended");}
                    else if(text.StartsWith("snapshot ")){Session session=Sessions[text.Substring(9)];Manifest(session,"ready");}
                    else if(text=="checkpoint"){World.Save(false,false);File.WriteAllText(Path.Combine(Evidence,"checkpoint-written.txt"),DateTime.UtcNow.ToString("o"));}
                    else if(text=="hold-intro"){foreach(Session session in Sessions.Values)session.State="intro";State="demo-intro-hold";}
                    else throw new Exception("Unknown private command");
                    }
                    catch(Exception error)
                    {
                        File.AppendAllText(Path.Combine(Evidence,"command-errors.txt"),DateTime.UtcNow.ToString("o")+"\t"+text+"\t"+error+"\n");
                    }
                }
                if(State=="staging-territory")Seed();
                foreach(Session session in Sessions.Values)
                {
                    string driverError=Path.Combine(Evidence,"client-"+session.Role+"-error.txt");
                    if(File.Exists(driverError))
                    {
                        if(session.State=="running")EndCase(session,"invalid-driver-error");
                        else if(session.Started==DateTime.MinValue)File.WriteAllText(Path.Combine(Evidence,"driver-failure-held-"+session.Role+".txt"),"Safe intro failure observed; caseId remains zero and staging is prohibited. No rescue or combat trial.");
                        continue;
                    }
                    if(session.State!="running")continue;
                    string heartbeat=Path.Combine(Evidence,"driver-heartbeat-"+session.Role+".txt");
                    if(!session.Actor.Alive)EndCase(session,"dead");
                    else if(!File.Exists(heartbeat) || DateTime.UtcNow-File.GetLastWriteTimeUtc(heartbeat)>TimeSpan.FromSeconds(30))EndCase(session,"invalid-driver-heartbeat");
                    else if(File.Exists(Path.Combine(Evidence,"done-"+session.Role+"-"+session.Started.Ticks+".txt")))EndCase(session,"completed-driver");
                    else if(DateTime.UtcNow-session.Started>TimeSpan.FromMinutes(10))EndCase(session,"bounded-window-ended");
                }
                if(DateTime.UtcNow>=NextStatus){NextStatus=DateTime.UtcNow.AddSeconds(1);Status();}
            }
            catch(Exception error){Failure=error.ToString();State="failed";Status();}
        }
        private static void EndCase(Session session,string outcome)
        {
            session.State=outcome;Manifest(session,"after-"+session.Mode);
            File.AppendAllText(Path.Combine(Evidence,"case-events.tsv"),DateTime.UtcNow.ToString("o")+"\tend\t"+session.Role+"\t"+outcome+"\tmeasured endpoint before test-only safe parking; not ordinary escape evidence\n");
            if(session.Actor.Alive && session.SafeMap!=null && session.SafeMap!=Map.Internal)
            {
                session.Actor.Combatant=null;session.Actor.Warmode=false;
                BaseCreature.TeleportPets(session.Actor,session.SafePoint,session.SafeMap);
                session.Actor.MoveToWorld(session.SafePoint,session.SafeMap);
                Manifest(session,"parked-after-"+session.Mode);
            }
        }
        private static void Health(HealthChangedEventArgs e)
        {
            foreach(Session session in Sessions.Values)
            {
                bool running=session.State=="running";
                int lost=Math.Max(0,e.OldHits-e.NewHits);
                if(running && e.Mobile==session.Actor){session.Incoming+=lost;if(e.Source is ArachnidBroodReinforcement)session.ResponseDamage+=lost;}
                Mobile principal=e.Source==null?null:e.Source.Player?e.Source:e.Source.GetDamageMaster(e.Mobile);
                IArachnidOutbreakOwned victim=e.Mobile as IArachnidOutbreakOwned;
                bool outgoing=running && principal==session.Actor && e.Kind==HealthChangeKind.Damage && victim!=null && (victim.Controller==Owner || victim.Controller==NaturalOwner);
                if(outgoing){session.Direct+=lost;if(e.Source!=null && !e.Source.Player)session.PetDamage+=lost;if(e.Mobile==Queen)session.QueenDamage+=lost;}
                if(e.Mobile==session.Actor || outgoing || (running && (e.Mobile==Queen || e.Mobile.Serial.Value==session.TargetSerial)))File.AppendAllText(Path.Combine(Evidence,"health.tsv"),DateTime.UtcNow.ToString("o")+"\t"+session.Role+"\t"+e.Mobile.Serial.Value+"\t"+e.OldHits+"\t"+e.NewHits+"\t"+e.Kind+"\t"+(e.Source==null?"none":e.Source.GetType().Name+":"+e.Source.Serial.Value)+"\t"+session.State+"\n");
            }
        }
        private static void Death(Mobile mobile)
        {
            foreach(Session session in Sessions.Values)if(session.Actor==mobile)++session.Deaths;
            ArachnidInfestationCreature brood=mobile as ArachnidInfestationCreature;
            if(brood!=null && (brood.Controller==Owner || brood.Controller==NaturalOwner))
                File.AppendAllText(Path.Combine(Evidence,"brood-deaths.tsv"),DateTime.UtcNow.ToString("o")+"\t"+mobile.Serial.Value+"\t"+mobile.GetType().Name+"\t"+brood.Tier+"\t"+brood.PatchId+"\t"+(mobile.LastKiller==null?"none":mobile.LastKiller.GetType().Name+":"+mobile.LastKiller.Serial.Value)+"\n");
            IArachnidOutbreakOwned killer=mobile.LastKiller as IArachnidOutbreakOwned;
            if(killer!=null && killer.Controller==Owner){if(mobile is Citizens)++CitizensDied;if(mobile is TownGuards)++GuardsDied;}
        }
        private static void Manifest(Session session,string suffix)
        {
            PlayerMobile actor=session.Actor;
            using(XmlWriter writer=XmlWriter.Create(Path.Combine(Evidence,session.Role+"-"+suffix+".private.xml"),new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("actor");Attr(writer,"serial",actor.Serial.Value);Attr(writer,"access",actor.AccessLevel);Attr(writer,"blessed",actor.Blessed);
                Attr(writer,"hits",actor.Hits);Attr(writer,"mana",actor.Mana);Attr(writer,"stam",actor.Stam);Attr(writer,"tithing",actor.TithingPoints);Attr(writer,"followers",actor.Followers);Attr(writer,"x",actor.X);Attr(writer,"y",actor.Y);Attr(writer,"z",actor.Z);
                for(int i=0;i<actor.Skills.Length;++i){Skill skill=actor.Skills[i];writer.WriteStartElement("skill");Attr(writer,"name",skill.SkillName);Attr(writer,"base",skill.Base);Attr(writer,"effective",skill.Value);writer.WriteEndElement();}
                foreach(Item item in actor.Items){writer.WriteStartElement("equipped");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);Attr(writer,"container",item is Container);writer.WriteEndElement();}
                if(actor.Backpack!=null)foreach(Item item in actor.Backpack.FindItemsByType(typeof(Item),true))
                {
                    writer.WriteStartElement("carried");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);Attr(writer,"container",item is Container);
                    Item parent=item.Parent as Item;Attr(writer,"parent",parent==null?0:parent.Serial.Value);writer.WriteEndElement();
                }
                foreach(Item equipped in actor.Items)
                {
                    Container cargo=equipped as Container;
                    if(cargo==null || cargo==actor.Backpack || equipped.Layer==Layer.Bank)continue;
                    foreach(Item item in cargo.FindItemsByType(typeof(Item),true))
                    {
                        writer.WriteStartElement("carried");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);Attr(writer,"container",item is Container);
                        Item parent=item.Parent as Item;Attr(writer,"parent",parent==null?0:parent.Serial.Value);writer.WriteEndElement();
                    }
                }
                foreach(Mobile companion in actor.AllFollowers){writer.WriteStartElement("companion");Attr(writer,"serial",companion.Serial.Value);Attr(writer,"type",companion.GetType().FullName);Attr(writer,"alive",companion.Alive);Attr(writer,"x",companion.X);Attr(writer,"y",companion.Y);Attr(writer,"z",companion.Z);writer.WriteEndElement();}
                writer.WriteEndElement();
            }
        }
        private static void Status()
        {
            string temporary=Path.Combine(Evidence,"native-state.tmp"),destination=Path.Combine(Evidence,"native-state.xml");
            using(XmlWriter writer=XmlWriter.Create(temporary,new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("polish");Attr(writer,"state",State);Attr(writer,"failure",Failure);Attr(writer,"utc",DateTime.UtcNow.ToString("o"));
                Attr(writer,"citizenDeaths",CitizensDied);Attr(writer,"guardDeaths",GuardsDied);Attr(writer,"actorCount",SpreadingEncounterScheduler.ActorCount);
                if(Owner!=null){Attr(writer,"warnings",Owner.BroodWarnings);Attr(writer,"responses",Owner.BroodResponseBirths);Attr(writer,"phase",Owner.Phase);Attr(writer,"status",Owner.Status);Attr(writer,"populationPercent",Owner.InnerPopulationPercent);Attr(writer,"replacementSeconds",Owner.ReplacementSeconds);
                    Attr(writer,"queenMode",Owner.QueenMode);Attr(writer,"queenBackupSpent",Owner.QueenBackupSpent);Attr(writer,"queenBackupPending",Owner.QueenBackupPending);Attr(writer,"queenRallyPatch",Owner.QueenRallyPatch);Attr(writer,"frontierRecipients",Owner.FrontierRewardRecipients);}
                if(Colony!=null){Attr(writer,"patches",Colony.Patches.Count);Attr(writer,"epoch",Colony.Epoch);}
                if(Queen!=null){Attr(writer,"queenX",Queen.X);Attr(writer,"queenY",Queen.Y);Attr(writer,"queenHits",Queen.Hits);Attr(writer,"queenGoal",Owner.QueenShelterPatch);Attr(writer,"queenMoves",Owner.QueenShelterMoves);Attr(writer,"queenArrivals",Owner.QueenShelterArrivals);Attr(writer,"queenBlocked",Owner.QueenShelterBlocked);}
                if(Colony!=null)foreach(ArachnidInfestationPatch patch in Colony.Patches)
                {
                    writer.WriteStartElement("patch");Attr(writer,"id",patch.Id);Attr(writer,"x",patch.Center.X);Attr(writer,"y",patch.Center.Y);Attr(writer,"tier",Colony.Tier(patch));Attr(writer,"established",patch.Established);Attr(writer,"retired",patch.Retired);Attr(writer,"residents",patch.Residents.Count);writer.WriteEndElement();
                }
                if(Colony!=null)foreach(ArachnidInfestationPatch patch in Colony.Patches)
                    if(patch.ResponseBirths>0){writer.WriteStartElement("responseNest");Attr(writer,"id",patch.Id);Attr(writer,"births",patch.ResponseBirths);Attr(writer,"x",patch.Center.X);Attr(writer,"y",patch.Center.Y);Attr(writer,"retired",patch.Retired);writer.WriteEndElement();}
                if(NaturalOwner!=null)
                {
                    writer.WriteStartElement("natural");Attr(writer,"serial",NaturalOwner.Serial.Value);Attr(writer,"phase",NaturalOwner.Phase);Attr(writer,"started",NaturalStarted.ToString("o"));
                    ArachnidInfestationState natural=(ArachnidInfestationState)Get(NaturalOwner,"m_Infestation");
                    if(natural!=null)
                    {
                        Attr(writer,"originX",natural.Origin.X);Attr(writer,"originY",natural.Origin.Y);Attr(writer,"patches",natural.Patches.Count);Attr(writer,"epoch",natural.Epoch);
                        foreach(Mobile mobile in (List<Mobile>)Get(NaturalOwner,"m_Creatures"))
                            if(mobile is ArachnidOutbreakChampion && mobile.Alive){Attr(writer,"queenX",mobile.X);Attr(writer,"queenY",mobile.Y);Attr(writer,"queenZ",mobile.Z);Attr(writer,"queenHits",mobile.Hits);break;}
                        foreach(ArachnidInfestationPatch patch in natural.Patches)
                        {writer.WriteStartElement("patch");Attr(writer,"id",patch.Id);Attr(writer,"x",patch.Center.X);Attr(writer,"y",patch.Center.Y);Attr(writer,"established",patch.Established);Attr(writer,"retired",patch.Retired);Attr(writer,"establishAfter",patch.EstablishAfter.ToString("o"));if(patch.Founder!=null){Attr(writer,"founder",patch.Founder.Serial.Value);Attr(writer,"founderX",patch.Founder.X);Attr(writer,"founderY",patch.Founder.Y);Attr(writer,"founderZ",patch.Founder.Z);}writer.WriteEndElement();}
                    }
                    writer.WriteEndElement();
                }
                foreach(Session session in Sessions.Values)
                {
                    PlayerMobile actor=session.Actor;writer.WriteStartElement("actor");Attr(writer,"role",session.Role);Attr(writer,"serial",actor.Serial.Value);Attr(writer,"state",session.State);Attr(writer,"mode",session.Mode);
                    Attr(writer,"caseId",session.Started.Ticks);Attr(writer,"connected",actor.NetState!=null);Attr(writer,"alive",actor.Alive);Attr(writer,"blessed",actor.Blessed);Attr(writer,"x",actor.X);Attr(writer,"y",actor.Y);Attr(writer,"z",actor.Z);
                    Attr(writer,"casting",actor.Spell!=null);Attr(writer,"frozen",actor.Frozen);Attr(writer,"paralyzed",actor.Paralyzed);
                    Attr(writer,"hits",actor.Hits);Attr(writer,"hitsMax",actor.HitsMax);Attr(writer,"mana",actor.Mana);Attr(writer,"manaMax",actor.ManaMax);Attr(writer,"stam",actor.Stam);Attr(writer,"tithing",actor.TithingPoints);Attr(writer,"poisoned",actor.Poisoned);Attr(writer,"incoming",session.Incoming);Attr(writer,"responseDamage",session.ResponseDamage);Attr(writer,"deaths",session.Deaths);Attr(writer,"queenDamage",session.QueenDamage);Attr(writer,"directDamage",session.Direct);Attr(writer,"petDamage",session.PetDamage);
                    Attr(writer,"startWarnings",session.StartWarnings);Attr(writer,"targetSerial",session.TargetSerial);Attr(writer,"targetRetired",Colony!=null && session.TargetPatch>=0 && Colony.Find(session.TargetPatch).Retired);
                    foreach(Point3D goal in session.Goals){writer.WriteStartElement("goal");Attr(writer,"x",goal.X);Attr(writer,"y",goal.Y);Attr(writer,"z",goal.Z);writer.WriteEndElement();}
                    foreach(Mobile companion in actor.AllFollowers){writer.WriteStartElement("companion");Attr(writer,"serial",companion.Serial.Value);Attr(writer,"hits",companion.Hits);Attr(writer,"hitsMax",companion.HitsMax);Attr(writer,"x",companion.X);Attr(writer,"y",companion.Y);Attr(writer,"z",companion.Z);writer.WriteEndElement();}
                    writer.WriteEndElement();
                }
                if(Owner!=null)foreach(Mobile mobile in ((List<Mobile>)Get(Owner,"m_Creatures")).ToArray())
                    if(mobile!=null && !mobile.Deleted && mobile.Alive){writer.WriteStartElement("enemy");Attr(writer,"serial",mobile.Serial.Value);Attr(writer,"type",mobile.GetType().Name);Attr(writer,"name",mobile.Name);Attr(writer,"x",mobile.X);Attr(writer,"y",mobile.Y);Attr(writer,"z",mobile.Z);Attr(writer,"hits",mobile.Hits);Mobile bard=Server.SkillHandlers.Discordance.GetBard(mobile);Attr(writer,"discordBard",bard==null?0:bard.Serial.Value);ArachnidInfestationCreature brood=mobile as ArachnidInfestationCreature;if(brood!=null){Attr(writer,"combatant",mobile.Combatant==null?0:mobile.Combatant.Serial.Value);Attr(writer,"tier",brood.Tier);Attr(writer,"egg",brood.EggSac);}writer.WriteEndElement();}
                writer.WriteEndElement();
            }
            if(File.Exists(destination))File.Replace(temporary,destination,null);else File.Move(temporary,destination);
        }
        private static void Attr(XmlWriter writer,string name,object value){writer.WriteAttributeString(name,Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture));}
    }
}
