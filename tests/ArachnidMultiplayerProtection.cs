using System;
using System.Collections.Generic;
using System.Globalization;
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
    // Unsaved actual historical players and ordinary native clients. Relocation is declared fixture input, never a learned player spell.
    public static class ArachnidMultiplayerProtection
    {
        private static readonly string Evidence=Path.GetFullPath(Path.Combine(Core.BaseDirectory,"../evidence"));
        private static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        private static readonly BindingFlags Static=BindingFlags.Static|BindingFlags.NonPublic;
        private static readonly Type Native=typeof(ArachnidArchmagePreparation);
        private static readonly List<ArachnidOutbreakController> Additional=new List<ArachnidOutbreakController>();
        private static PlayerMobile Partner;
        private static ArachnidOutbreakController Secondary;
        private static Point3D SecondaryOrigin,Observation;
        private static string Phase="preparing",Failure=String.Empty;
        private static DateTime Began,PhaseBegan,Deadline=DateTime.UtcNow.AddMinutes(20);
        private static Timer Poll;
        private static int Moves,PartnerMoves,DamageEvents,PetDamageEvents;
        private static PlayerMobile Actor {get{return (PlayerMobile)Native.GetField("Actor",Static).GetValue(null);}}
        private static ArachnidOutbreakController Primary {get{return (ArachnidOutbreakController)Native.GetField("Controller",Static).GetValue(null);}}
        private static void Call(ArachnidOutbreakController owner,string method,params object[] args){typeof(ArachnidOutbreakController).GetMethod(method,Private).Invoke(owner,args);}
        private static ArachnidInfestationState Colony(ArachnidOutbreakController owner){return (ArachnidInfestationState)typeof(ArachnidOutbreakController).GetField("m_Infestation",Private).GetValue(owner);}
        private static void Attr(XmlWriter w,string name,object value){w.WriteAttributeString(name,Convert.ToString(value,CultureInfo.InvariantCulture));}

        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            if(!File.Exists(Path.Combine(Evidence,"protection-account.private")))return;
            if(ArachnidOutbreakController.DeliveryReady || File.ReadAllText(Path.Combine(Evidence,"native-private-root.txt")).Trim()!=Core.BaseDirectory.TrimEnd('\\'))throw new Exception("Guarded private multiplayer copy required.");
            AutoSave.SavesEnabled=false;
            EventSink.Movement+=Moved;EventSink.HealthChanged+=Health;
            Poll=Timer.DelayCall(TimeSpan.FromSeconds(3),TimeSpan.FromMilliseconds(250),Tick);
        }
        private static void Moved(MovementEventArgs e){if(e.Mobile==Actor)++Moves;else if(e.Mobile==Partner)++PartnerMoves;}
        private static void Health(HealthChangedEventArgs e)
        {
            if(e.NewHits>=e.OldHits || e.Kind!=HealthChangeKind.Damage || !(e.Mobile is IArachnidOutbreakOwned))return;
            ++DamageEvents;
            BaseCreature source=e.Source as BaseCreature;
            if(source!=null && (source.ControlMaster==Partner || source.SummonMaster==Actor || source.ControlMaster==Actor))++PetDamageEvents;
            File.AppendAllText(Path.Combine(Evidence,"protection-damage.private.tsv"),DateTime.UtcNow.ToString("o")+"\t"+Phase+"\t"+e.Mobile.Serial.Value+"\t"+(e.Source==null?0:e.Source.Serial.Value)+"\t"+e.OldHits+"\t"+e.NewHits+"\n");
        }
        private static void Setup()
        {
            if(Actor==null)return;
            Partner=World.FindMobile((Serial)Int32.Parse(File.ReadAllText(Path.Combine(Evidence,"partner-serial.private")).Trim(),CultureInfo.InvariantCulture)) as PlayerMobile;
            if(Partner==null || Partner==Actor || !Partner.Alive || Partner.Blessed || Partner.AccessLevel!=AccessLevel.Player)throw new Exception("Actual eligible saved partner required.");
            ArachnidArchmagePreparation.WriteCharacterManifest("partner-original.private.xml",Partner);
            string[] login=File.ReadAllLines(Path.Combine(Evidence,"protection-account.private"));
            if(login.Length!=2 || Accounts.GetAccount(login[0])!=null)throw new Exception("Fresh private generated login required.");
            IAccount original=Partner.Account;
            if(original!=null)for(int i=0;i<original.Length;++i)if(original[i]==Partner){original[i]=null;break;}
            Account account=new Account(login[0],login[1]);account[0]=Partner;
            Partner.MoveToWorld(Safe(2490,2010),Map.Sosaria);Phase="ready";PhaseBegan=DateTime.UtcNow;
        }
        private static Point3D Safe(int x,int y)
        {
            for(int r=0;r<=12;++r)for(int dx=-r;dx<=r;++dx)for(int dy=-r;dy<=r;++dy)
            {Point3D p;if(ArachnidInfestationTerrain.TryPoint(x+dx,y+dy,out p) && Map.Sosaria.CanSpawnMobile(p))return p;}
            throw new Exception("No legal finite fixture relocation point.");
        }
        private static ArachnidOutbreakController NewController()
        {
            ArachnidOutbreakController owner=new ArachnidOutbreakController();
            ((ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State",Private).GetValue(owner)).SetEnabled(true);
            Additional.Add(owner);return owner;
        }
        private static void Begin()
        {
            if(Actor.NetState==null || Partner.NetState==null || !Actor.Alive || !Partner.Alive || Actor.Blessed || Partner.Blessed)throw new Exception("Both actual vulnerable clients must be connected.");
            Native.GetMethod("Begin",Static).Invoke(null,null);
            Actor.MoveToWorld(Safe(2490,2010),Map.Sosaria);
            SecondaryOrigin=Safe(2754,2088);Secondary=NewController();
            if(!(bool)typeof(ArachnidOutbreakController).GetMethod("BeginInfestation",Private).Invoke(Secondary,new object[]{SecondaryOrigin,DateTime.UtcNow}))throw new Exception("Secondary native origin refused.");
            Call(Secondary,"StartTimer");
            for(int i=0;i<14;++i)Call(NewController(),"StartTimer");
            Began=DateTime.UtcNow;Change("quiet-growth");
        }
        private static void Change(string phase){Phase=phase;PhaseBegan=DateTime.UtcNow;}
        private static Point3D ObservationPoint(ArachnidOutbreakController owner)
        {
            ArachnidInfestationState colony=Colony(owner);
            for(int radius=32;radius<=56;radius+=4)for(int direction=0;direction<8;++direction)
            {
                int[] dx=new int[]{-1,-1,0,1,1,1,0,-1},dy=new int[]{0,-1,-1,-1,0,1,1,1};
                Point3D p;
                if(!ArachnidInfestationTerrain.TryPoint(colony.Origin.X+radius*dx[direction],colony.Origin.Y+radius*dy[direction],out p) || !Map.Sosaria.CanSpawnMobile(p))continue;
                int nearest=Int32.MaxValue;
                foreach(Mobile enemy in (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(owner))
                    if(enemy!=null && !enemy.Deleted && enemy.Alive)nearest=Math.Min(nearest,Distance(p,enemy.Location));
                foreach(ArachnidInfestationPatch patch in colony.Patches)foreach(ArachnidInfestationResident resident in patch.Residents)
                    if(resident.Hits>0)nearest=Math.Min(nearest,Distance(p,resident.Actor!=null && !resident.Actor.Deleted?resident.Actor.Location:resident.Position));
                if(nearest>=24 && nearest<=40)return p;
            }
            throw new Exception("No surveyed outer observation point; do not substitute root tanking.");
        }
        private static int Distance(Point3D a,Point3D b){return Math.Max(Math.Abs(a.X-b.X),Math.Abs(a.Y-b.Y));}
        private static void Request(string command)
        {
            if(command=="start" && Phase=="ready"){Begin();return;}
            if(Primary==null || Secondary==null)throw new Exception("No native multiplayer colonies.");
            if(command=="separated")
            {
                Observation=ObservationPoint(Primary);Actor.MoveToWorld(Observation,Map.Sosaria);Partner.MoveToWorld(Safe(SecondaryOrigin.X-18,SecondaryOrigin.Y),Map.Sosaria);
                int petIndex=0;
                foreach(Mobile mobile in new List<Mobile>(World.Mobiles.Values))
                {
                    BaseCreature pet=mobile as BaseCreature;
                    if(pet!=null && pet.Alive && !pet.Deleted && pet.ControlMaster==Partner && pet.Map!=Map.Internal)
                        pet.MoveToWorld(Safe(SecondaryOrigin.X-8+(petIndex++%3),SecondaryOrigin.Y-2),Map.Sosaria);
                }
            }
            else if(command=="approach")Actor.MoveToWorld(Safe(Observation.X+Math.Sign(Observation.X-2514)*40,Observation.Y+Math.Sign(Observation.Y-2088)*40),Map.Sosaria);
            else if(command=="overlap")Partner.MoveToWorld(Safe(Observation.X+1,Observation.Y),Map.Sosaria);
            else if(command=="first-leaves")Actor.MoveToWorld(Safe(Observation.X+Math.Sign(Observation.X-2514)*90,Observation.Y+Math.Sign(Observation.Y-2088)*90),Map.Sosaria);
            else if(command=="chase")Actor.MoveToWorld(Safe(2514-18,2088),Map.Sosaria);
            else if(command=="teleport-combat")Actor.MoveToWorld(Safe(Observation.X+Math.Sign(Observation.X-2514)*90,Observation.Y+Math.Sign(Observation.Y-2088)*90),Map.Sosaria);
            else if(command=="complete"){Change(command);return;}
            else throw new Exception("Unsupported finite protection phase.");
            Change(command);
        }
        private static void Cleanup()
        {
            foreach(ArachnidOutbreakController owner in Additional)if(!owner.Deleted)owner.Delete();Additional.Clear();
            Poll.Stop();AutoSave.SavesEnabled=false;
        }
        private static void Tick()
        {
            try
            {
                if(Phase=="preparing")Setup();
                if(Phase=="ready" && File.Exists(Path.Combine(Evidence,"protection-partner-prepared.txt")) && !File.Exists(Path.Combine(Evidence,"partner-prepared.private.xml")))
                    ArachnidArchmagePreparation.WriteCharacterManifest("partner-prepared.private.xml",Partner);
                string command=Path.Combine(Evidence,"protection-command.txt");
                if(File.Exists(command)){string text=File.ReadAllText(command).Trim();File.Delete(command);Request(text);}
                Status();
                if(File.Exists(Path.Combine(Evidence,"native-stop.txt")) || DateTime.UtcNow>Deadline)Cleanup();
            }
            catch(Exception e){Failure=e.ToString();Phase="failed";Status();Cleanup();File.WriteAllText(Path.Combine(Evidence,"protection-failure.txt"),Failure);}
        }
        private static void Describe(XmlWriter w,Mobile actor,string role)
        {
            w.WriteStartElement(role);Attr(w,"type",actor.GetType().Name);Attr(w,"serial",actor.Serial.Value);Attr(w,"alive",actor.Alive && !actor.Deleted);Attr(w,"hits",actor.Hits);Attr(w,"hitsMax",actor.HitsMax);Attr(w,"mana",actor.Mana);Attr(w,"stam",actor.Stam);Attr(w,"x",actor.X);Attr(w,"y",actor.Y);Attr(w,"z",actor.Z);
            Attr(w,"backpack",actor.Backpack==null?0:actor.Backpack.Serial.Value);Attr(w,"combatant",actor.Combatant==null?0:actor.Combatant.Serial.Value);
            BaseCreature creature=actor as BaseCreature;
            if(creature!=null){Attr(w,"ai",creature.AIObject==null?"none":creature.AIObject.GetType().Name);Attr(w,"aiRunning",creature.AIObject!=null && creature.AIObject.m_Timer.Running);Attr(w,"currentSpeed",creature.CurrentSpeed);Attr(w,"activeSpeed",creature.ActiveSpeed);Attr(w,"interested",SpreadingEncounterScheduler.Interested(creature));Attr(w,"busy",SpreadingEncounterScheduler.Busy(creature));Attr(w,"order",creature.ControlOrder);}
            ArachnidInfestationCreature resident=actor as ArachnidInfestationCreature;if(resident!=null){Attr(w,"identity",resident.Identity);Attr(w,"tier",resident.Tier);}
            w.WriteEndElement();
        }
        private static void Status()
        {
            string path=Path.Combine(Evidence,"protection-state.xml");
            using(XmlWriter w=XmlWriter.Create(path+".tmp",new XmlWriterSettings{Indent=true}))
            {
                w.WriteStartElement("protection");Attr(w,"phase",Phase);Attr(w,"failure",Failure);Attr(w,"utc",DateTime.UtcNow.ToString("o"));Attr(w,"seconds",Began==DateTime.MinValue?0:(DateTime.UtcNow-Began).TotalSeconds);Attr(w,"phaseSeconds",(DateTime.UtcNow-PhaseBegan).TotalSeconds);
                Attr(w,"mainOnline",Actor!=null && Actor.NetState!=null);Attr(w,"partnerOnline",Partner!=null && Partner.NetState!=null);Attr(w,"moves",Moves);Attr(w,"partnerMoves",PartnerMoves);Attr(w,"damageEvents",DamageEvents);Attr(w,"petDamageEvents",PetDamageEvents);Attr(w,"aggregateActors",SpreadingEncounterScheduler.ActorCount);
                w.WriteStartElement("waypoint");Attr(w,"x",Observation.X);Attr(w,"y",Observation.Y);Attr(w,"z",Observation.Z);w.WriteEndElement();
                if(Actor!=null)Describe(w,Actor,"player");if(Partner!=null)Describe(w,Partner,"partner");
                foreach(Mobile mobile in World.Mobiles.Values)
                {
                    BaseCreature pet=mobile as BaseCreature;
                    if(pet!=null && !pet.Deleted && (pet.ControlMaster==Partner || pet.ControlMaster==Actor || pet.SummonMaster==Actor))Describe(w,pet,"pet");
                }
                foreach(ArachnidOutbreakController owner in new ArachnidOutbreakController[]{Primary,Secondary})
                {
                    if(owner==null || owner.Deleted)continue;
                    w.WriteStartElement("colony");Attr(w,"serial",owner.Serial.Value);Attr(w,"primary",owner==Primary);Attr(w,"phase",owner.Phase);Attr(w,"status",owner.Status);
                    ArachnidInfestationState colony=Colony(owner);Attr(w,"births",colony==null?0:colony.Births);
                    foreach(Mobile mobile in (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures",Private).GetValue(owner))if(mobile!=null && !mobile.Deleted)Describe(w,mobile,"enemy");
                    if(colony!=null)foreach(ArachnidInfestationPatch patch in colony.Patches)foreach(ArachnidInfestationResident resident in patch.Residents)
                    {w.WriteStartElement("resident");Attr(w,"identity",resident.Identity);Attr(w,"storedHits",resident.Hits);Attr(w,"live",resident.Actor!=null && !resident.Actor.Deleted && resident.Actor.Alive);Attr(w,"serial",resident.Actor==null?0:resident.Actor.Serial.Value);w.WriteEndElement();}
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
            if(File.Exists(path))File.Replace(path+".tmp",path,null);else File.Move(path+".tmp",path);
        }
    }
}
