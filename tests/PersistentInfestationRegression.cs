using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

public static class PersistentInfestationRegression
{
    private static int Checks;
    private static BindingFlags Hidden = BindingFlags.NonPublic|BindingFlags.Instance;
    private static void Check(bool value,string message) { ++Checks; if (!value) throw new Exception(message); }
    private static object Field(object owner,string name) { return owner.GetType().GetField(name,Hidden).GetValue(owner); }
    private static object Call(object owner,string name,params object[] args)
    { try { return owner.GetType().InvokeMember(name,Hidden|BindingFlags.InvokeMethod,null,owner,args); } catch(TargetInvocationException error) { throw error.InnerException; } }
    private static object Shared(string name,params object[] args)
    { try { return typeof(SpreadingEncounterScheduler).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,args); } catch(TargetInvocationException error) { throw error.InnerException; } }
    private static void Track(Mobile actor,bool add)
    { typeof(SpreadingEncounterScheduler).GetMethod(add?"Track":"Untrack",BindingFlags.Static|BindingFlags.NonPublic,null,new Type[]{typeof(Mobile)},null).Invoke(null,new object[]{actor}); }
    private static ArachnidOutbreakController Start()
    {
        ArachnidOutbreakController owner = new ArachnidOutbreakController();
        ((ArachnidOutbreakState)Field(owner,"m_State")).SetEnabled(true);
        Check((bool)Call(owner,"BeginInfestation",new Point3D(2514,2088,0),DateTime.UtcNow),"start a real controller on checked terrain");
        return owner;
    }
    private static ArachnidInfestationPatch Root(ArachnidOutbreakController owner)
    { return ((ArachnidInfestationState)Field(owner,"m_Infestation")).Patches[0]; }
    private static void Quiet(ArachnidInfestationPatch patch)
    {
        patch.LastInterest = DateTime.UtcNow.AddMinutes(-10);
        foreach(ArachnidInfestationResident resident in patch.Residents)
        { ArachnidInfestationCreature actor = resident.Actor as ArachnidInfestationCreature; if(actor!=null)actor.LastRelevant=patch.LastInterest; }
    }
    private static void ParkQueenForCompaction(ArachnidOutbreakController owner)
    {
        // Compaction fixtures deliberately isolate quiet survivors from queen protection.
        foreach(Mobile actor in (List<Mobile>)Field(owner,"m_Creatures"))
            if(actor is ArachnidOutbreakChampion)actor.MoveToWorld(new Point3D(4800,2600,0),Map.Sosaria);
    }
    private static void SurvivorAndAdmission()
    {
        ArachnidOutbreakController owner=Start(); Mobile observer=new Mobile();observer.Player=true;
        ParkQueenForCompaction(owner);
        List<Mobile> registryFixtures=new List<Mobile>();
        try
        {
            ArachnidInfestationPatch patch=Root(owner);ArachnidInfestationCreature original=(ArachnidInfestationCreature)patch.Guardian;
            Guid identity=original.Identity;Serial serial=original.Serial;original.Hits=original.HitsMax/3;original.Stam=11;original.Mana=7;
            if(original.Backpack==null)original.AddItem(new Backpack());Container inventory=original.Backpack;Bandage finite=new Bandage(7);inventory.DropItem(finite);
            int hits=original.Hits;Quiet(patch);
            original.Hidden=true;
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"hidden survivors retain their real effect state");original.Hidden=false;
            Point3D position=original.Location;original.MoveToWorld(new Point3D(position.X+100,position.Y,position.Z),Map.Sosaria);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"a roaming survivor outside the territory admission index remains real");
            original.MoveToWorld(position,Map.Internal);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"a survivor on another map cannot lose its facet in a compact record");original.MoveToWorld(position,Map.Sosaria);
            Check((bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"quiet uncontested survivors can compact");
            Check(original.Deleted && inventory.Parent==null && finite.Amount==7,"compaction retains the finite inventory outside the removed mobile");
            Check(patch.Residents[0].Identity==identity && patch.Residents[0].Actor==null && patch.Residents[0].Hits==hits,"compact logical identity and wounds survive");
            // Registry fixtures test admission arithmetic, not NPC throughput or active-client play.
            for(int i=0;i<160;++i){Mobile fixture=new Mobile();fixture.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria);Track(fixture,true);registryFixtures.Add(fixture);}
            Check(!(bool)Shared("AdmitActor",false),"new background admissions wait at the shared target");
            observer.MoveToWorld(patch.Center,Map.Sosaria);
            ArachnidInfestationCreature restored=(ArachnidInfestationCreature)patch.Guardian;
            Check(restored!=null && restored.Identity==identity && restored.Serial!=serial,"sector entry restores the owed survivor with its stable logical identity");
            Check(restored.Hits==hits && restored.Stam==11 && restored.Mana==7,"rehydration cannot heal or refill resources");
            Check(restored.Backpack==inventory && finite.RootParent==restored && finite.Amount==7,"rehydration transfers the same inventory exactly once");
            Check(SpreadingEncounterScheduler.ActorCount>SpreadingEncounterScheduler.ActorAdmissionTarget,"player admission crosses the soft target instead of dropping survivors");
            Check(restored.AIObject.m_Timer.Running,"restored survivor has its ordinary AI timer");
            Quiet(patch);Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow) && !restored.Deleted,"a nearby player prevents compaction");
            observer.MoveToWorld(new Point3D(4500,2800,0),Map.Sosaria);restored.Combatant=observer;Quiet(patch);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow) && restored.Serial==patch.Guardian.Serial,"ongoing pursuit retains the existing actor and identity");
        }
        finally{owner.Delete();observer.Delete();foreach(Mobile actor in registryFixtures){Track(actor,false);actor.Delete();}}
    }
    private static void MissingActorCannotReset()
    {
        ArachnidOutbreakController owner=Start();
        try
        {
            ArachnidInfestationPatch patch=Root(owner);ArachnidInfestationCreature victim=null;
            foreach(ArachnidInfestationResident resident in patch.Residents)if(resident.Slot!=0)victim=resident.Actor as ArachnidInfestationCreature;
            Check(victim!=null,"fixture has a live brood member");Guid identity=victim.Identity;victim.Delete();
            Call(owner,"HydrateInfestationPatch",patch,DateTime.UtcNow);
            bool resurrected=false;foreach(ArachnidInfestationResident resident in patch.Residents)
                if(resident.Identity==identity && resident.Actor!=null && !resident.Actor.Deleted)resurrected=true;
            Check(!resurrected,"a removed live reference cannot silently reset the same survivor");
        }
        finally{owner.Delete();}
    }
    private static void InteractionProtection()
    {
        ArachnidOutbreakController owner=Start();Mobile first=new Mobile();first.Player=true;Mobile second=new Mobile();second.Player=true;
        ParkQueenForCompaction(owner);
        Hind pet=new Hind(),summon=new Hind();IDisposable sectorLease=null;
        try
        {
            ArachnidInfestationPatch patch=Root(owner);ArachnidInfestationCreature survivor=(ArachnidInfestationCreature)patch.Guardian;
            Point3D far=new Point3D(4500,2800,0),east=new Point3D(patch.Center.X+50,patch.Center.Y,patch.Center.Z),west=new Point3D(patch.Center.X-50,patch.Center.Y,patch.Center.Z);
            first.MoveToWorld(east,Map.Sosaria);second.MoveToWorld(west,Map.Sosaria);
            Check((bool)Shared("Interested",survivor),"combined player envelopes protect the colony before ordinary visible range");
            first.MoveToWorld(far,Map.Sosaria);Quiet(patch);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"one departing player cannot remove another player's existing actors");
            second.MoveToWorld(far,Map.Sosaria);
            // These are classification fixtures, not claims that taming or summoning succeeded in a client.
            pet.SetControlMaster(first);pet.MoveToWorld(east,Map.Sosaria);Quiet(patch);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"a controlled pet protects actors with its owner distant");pet.MoveToWorld(far,Map.Internal);
            summon.Summoned=true;summon.SummonMaster=second;summon.MoveToWorld(west,Map.Sosaria);Quiet(patch);
            Check(!(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"a summon protects actors with its owner distant");summon.MoveToWorld(far,Map.Internal);
            Guid identity=survivor.Identity;int hits=survivor.Hits;Quiet(patch);
            Check((bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"uncontested patch compacts after all player and companion envelopes leave");
            first.MoveToWorld(east,Map.Sosaria);survivor=(ArachnidInfestationCreature)patch.Guardian;
            Check(survivor!=null && survivor.Identity==identity && survivor.Hits==hits && survivor.AIObject.m_Timer.Running,"approach admits the owed identity and ordinary AI before visibility");
            first.MoveToWorld(far,Map.Sosaria);Quiet(patch);Check((bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"approach departure permits quiet compaction again");
            sectorLease=Map.Sosaria.GetSector(patch.Center).AcquireActivityLease();
            second.MoveToWorld(patch.Center,Map.Sosaria);survivor=(ArachnidInfestationCreature)patch.Guardian;
            Check(survivor!=null && survivor.Identity==identity && survivor.AIObject.m_Timer.Running,"teleport into an already active sector still admits existing survivors");
            second.MoveToWorld(far,Map.Sosaria);first.DoHarmful(survivor);Quiet(patch);
            Check((bool)Shared("Busy",survivor) && !(bool)Call(owner,"CompactInfestationPatch",patch,DateTime.UtcNow),"registered harmful intent retains normal actors while delayed effects or pursuit may still resolve");
            Check(!survivor.Deleted && survivor.AIObject.m_Timer.Running,"interaction protection never substitutes a sleeping AI or deleted actor");
        }
        finally{owner.Delete();if(sectorLease!=null)sectorLease.Dispose();pet.Delete();summon.Delete();first.Delete();second.Delete();}
    }
    private sealed class WorkProbe : ISpreadingEncounter
    {
        public int Visits, TerrainGrants, RouteGrants, ActorGrants, SceneryGrants, WorkMode;
        public bool Pending, TestAdmission, LastAdmission;
        public bool Scheduled{get{return true;}}
        public bool OriginPending{get{return Pending;}}
        public bool RespondsTo(Mobile witness){return true;}
        public void WorkDeferred(DateTime now){}
        public void Step(DateTime now)
        {
            ++Visits;if(TestAdmission){LastAdmission=(bool)Shared("AdmitActor",false);return;}
            if(WorkMode==3)return;
            if(WorkMode==4){if((bool)Shared("AdmitStructuralActor"))++ActorGrants;return;}
            if(WorkMode==1){for(int i=0;i<10;++i)if((bool)Shared("AdmitActor",false))++ActorGrants;return;}
            if(WorkMode==2){for(int i=0;i<3;++i)if((bool)Shared("AdmitScenery",16))++SceneryGrants;return;}
            if((bool)Shared("Route"))++RouteGrants;
            for(int i=0;i<1500;++i){Shared("Terrain");++TerrainGrants;}
        }
        public void Admit(int territory,DateTime now){}
    }
    private static void BootstrapAndInterest()
    {
        ArachnidOutbreakController owner=new ArachnidOutbreakController();Hind wildlife=new Hind();Mobile viewer=new Mobile();viewer.Player=true;
        try
        {
            Check(!owner.RespondsTo(wildlife),"ordinary friendly wildlife is not treated as an ongoing contest");
            wildlife.Team=17;Check(owner.RespondsTo(wildlife),"a different NPC team retains real simulation");
            Check(owner.RespondsTo(viewer),"every nearby player retains admission regardless of privilege or combat");
            ((ArachnidOutbreakState)Field(owner,"m_State")).SetEnabled(true);DateTime now=DateTime.UtcNow;
            owner.WorkDeferred(now);Check((DateTime)Field(owner,"m_NextSearch")==now.AddSeconds(2),"quota deferral retries shortly instead of waiting the unsuccessful-origin interval");
        }
        finally{owner.Delete();wildlife.Delete();viewer.Delete();}
        List<Mobile> fixtures=new List<Mobile>();List<WorkProbe> probes=new List<WorkProbe>();
        try
        {
            WorkProbe established=new WorkProbe();established.TestAdmission=true;probes.Add(established);SpreadingEncounterScheduler.Register(established);
            for(int i=0;i<5;++i){WorkProbe waiting=new WorkProbe();waiting.Pending=true;waiting.TestAdmission=true;probes.Add(waiting);SpreadingEncounterScheduler.Register(waiting);}
            for(int i=0;i<150;++i){Mobile actor=new Mobile();actor.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria);Track(actor,true);fixtures.Add(actor);}
            Shared("Service",DateTime.UtcNow.AddHours(3));Check(!established.LastAdmission,"new refills leave bootstrap headroom for the five waiting directors");
            Shared("Service",DateTime.UtcNow.AddHours(3).AddMilliseconds(250));Check(probes[1].LastAdmission,"a waiting director may spend its bootstrap admission turn");
            Check(SpreadingEncounterScheduler.ActorCount==150,"headroom scheduling does not evict existing actors");
        }
        finally{foreach(WorkProbe probe in probes)SpreadingEncounterScheduler.Unregister(probe);foreach(Mobile actor in fixtures){Track(actor,false);actor.Delete();}}
    }
    private static void SharedWork()
    {
        List<WorkProbe> probes=new List<WorkProbe>();DateTime now=DateTime.UtcNow.AddHours(1);
        try
        {
            for(int i=0;i<16;++i){WorkProbe probe=new WorkProbe();probes.Add(probe);SpreadingEncounterScheduler.Register(probe);}
            for(int i=0;i<256;++i)
            {
                Shared("Service",now.AddMilliseconds(i*250));
                int spent=(int)typeof(SpreadingEncounterScheduler).GetField("m_Terrain",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
                Check(spent<=SpreadingEncounterScheduler.TerrainPerSecond,"sixteen directors share one terrain quota");
            }
            foreach(WorkProbe probe in probes)
            {
                Check(probe.Visits==16,"all directors retain round-robin visits");
                Check(probe.TerrainGrants>0 && probe.RouteGrants>0,"visits must include actual terrain and route grants despite fixed quota-window alignment");
            }
            Check(SpreadingEncounterScheduler.Reserve(probes[0],Map.Sosaria,0,new Point3D(2514,2088,0)),"first attempt claims a territory cell");
            Check(!SpreadingEncounterScheduler.Reserve(probes[1],Map.Sosaria,0,new Point3D(2514,2088,0)),"overlapping attempts cannot double-own territory");
        }
        finally{foreach(WorkProbe probe in probes)SpreadingEncounterScheduler.Unregister(probe);}
    }
    private static void FiniteGeographyAndVersions()
    {
        ArachnidInfestationState state=new ArachnidInfestationState();state.Origin=new Point3D(2514,2088,0);state.Epoch=24;state.Births=901;state.Started=state.LastTick=DateTime.UtcNow;
        for(int i=0;i<25;++i)
        {
            ArachnidInfestationPatch patch=new ArachnidInfestationPatch();patch.Id=i;patch.Parent=i-1;patch.Birth=i;
            int row=i/5,column=i%5;if(row%2==1)column=4-column;
            patch.Center=new Point3D(2514+column*48,2088+row*48,0);patch.Established=true;state.Add(patch);
        }
        int ranks=0;foreach(ArachnidInfestationPatch patch in state.Patches)ranks|=1<<state.Tier(patch);
        Check(ranks==29,"queen distance exposes the lowest outskirts and three highest inner ranks independently of age");
        MemoryStream stream=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(stream,true);state.Serialize(writer);writer.Write(887612);writer.Close();
        BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(stream.ToArray())));ArachnidInfestationState copy=new ArachnidInfestationState();copy.Deserialize(reader);
        Check(copy.Patches.Count==25 && copy.Births==901 && reader.ReadInt()==887612,"new value format accepts more than sixteen cells and four hundred births without losing its boundary");reader.Close();
        DateTime next=state.LastTick.AddDays(5);state.Resume(next);Check(state.Epoch==24 && state.Births==901 && state.NextGrowth>next,"reload does not simulate missed growth or replay births");
        stream=new MemoryStream();writer=new BinaryFileWriter(stream,true);writer.Write(0);writer.Write(state.Origin);writer.Write(0);writer.Write(1);writer.Write(0);
        writer.Write(state.Started);writer.Write(state.NextGrowth);writer.Write(state.LastTick);writer.Write(1);
        writer.Write(0);writer.Write(0);writer.Write(-1);writer.Write(0);writer.Write(0);writer.Write(1);writer.Write(state.Origin);writer.Write((Mobile)null);writer.Write((Mobile)null);
        writer.Write(true);writer.Write(false);writer.Write(DateTime.MinValue);writer.Write(DateTime.MinValue);writer.Write(998721);writer.Close();
        reader=new BinaryFileReader(new BinaryReader(new MemoryStream(stream.ToArray())));copy=new ArachnidInfestationState();copy.Deserialize(reader);
        Check(copy.Patches.Count==1 && copy.Patches[0].Residents.Count==0 && reader.ReadInt()==998721,"version zero is still consumed in its original positional order");reader.Close();
    }
    private static void AdmissionFairness()
    {
        for(int mode=1;mode<=2;++mode)
        {
            List<WorkProbe> probes=new List<WorkProbe>();DateTime now=DateTime.UtcNow.AddHours(4+mode);
            try
            {
                for(int i=0;i<16;++i){WorkProbe probe=new WorkProbe();probe.WorkMode=mode;probes.Add(probe);SpreadingEncounterScheduler.Register(probe);}
                for(int i=0;i<256;++i)
                {
                    Shared("Service",now.AddMilliseconds(i*250));
                    int births=(int)typeof(SpreadingEncounterScheduler).GetField("m_Births",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
                    int parts=(int)typeof(SpreadingEncounterScheduler).GetField("m_Parts",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
                    Check(births<=SpreadingEncounterScheduler.ActorsPerSecond && parts<=SpreadingEncounterScheduler.SceneryPerSecond,"fair admission retains aggregate actor/scenery quotas");
                }
                foreach(WorkProbe probe in probes)Check(mode==1?probe.ActorGrants>0:probe.SceneryGrants>0,"every aligned director receives real actor/scenery admission, not just visits");
                probes[0].WorkMode=3;Shared("Service",now.AddMinutes(2));
                foreach(WorkProbe probe in probes)Check(SpreadingEncounterScheduler.WorkGrants(probe,mode+1)>0,"bounded per-director telemetry records actual granted work");
            }
            finally{foreach(WorkProbe probe in probes)SpreadingEncounterScheduler.Unregister(probe);}
        }
        List<ISpreadingEncounter>[] waiters=(List<ISpreadingEncounter>[])typeof(SpreadingEncounterScheduler).GetField("m_Waiters",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
        foreach(List<ISpreadingEncounter> queue in waiters)Check(queue.Count==0,"unregister releases pending quota demand");
    }
    private static void StructuralFairnessAboveTarget()
    {
        List<Mobile> fixtures=new List<Mobile>();List<WorkProbe> probes=new List<WorkProbe>();
        try
        {
            for(int i=0;i<160;++i){Mobile actor=new Mobile();actor.MoveToWorld(new Point3D(5000,3000,0),Map.Sosaria);Track(actor,true);fixtures.Add(actor);}
            WorkProbe denied=new WorkProbe();denied.WorkMode=1;probes.Add(denied);SpreadingEncounterScheduler.Register(denied);
            for(int i=0;i<16;++i){WorkProbe structural=new WorkProbe();structural.WorkMode=4;probes.Add(structural);SpreadingEncounterScheduler.Register(structural);}
            DateTime now=DateTime.UtcNow.AddHours(8);
            for(int i=0;i<400;++i)Shared("Service",now.AddMilliseconds(i*250));
            Check(denied.ActorGrants==0,"background admission remains denied at the target");
            for(int i=1;i<probes.Count;++i)Check(probes[i].ActorGrants>0,"every structural director progresses despite capacity-denied competing demand");
        }
        finally{foreach(WorkProbe probe in probes)SpreadingEncounterScheduler.Unregister(probe);foreach(Mobile actor in fixtures){Track(actor,false);actor.Delete();}}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();ArachnidOutbreakController.Initialize();
            SurvivorAndAdmission();MissingActorCannotReset();InteractionProtection();SharedWork();BootstrapAndInterest();FiniteGeographyAndVersions();AdmissionFairness();StructuralFairnessAboveTarget();
            Check(SpreadingEncounterScheduler.ActorCount==0 && SpreadingEncounterScheduler.SceneryCount==0,"owned teardown releases actor leases and scenery accounting");
            Console.WriteLine("PASS persistent infestation: "+Checks+" source-method checks; no saves/listener; synthetic scheduler/registry fixtures are not throughput or native gameplay evidence.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
