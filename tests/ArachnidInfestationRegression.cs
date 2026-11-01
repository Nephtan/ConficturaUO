using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

internal static class ArachnidInfestationRegression
{
    private static int Checks;
    private static BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool ok,string message) { if (!ok) throw new Exception("FAILED: "+message); ++Checks; }
    private static object Get(object value,string field) { return value.GetType().GetField(field,Private).GetValue(value); }
    private static object Call(object value,string method,params object[] args) { return value.GetType().InvokeMember(method,Private|BindingFlags.InvokeMethod,null,value,args); }
    private static ArachnidInfestationState Colony(ArachnidOutbreakController c) { return (ArachnidInfestationState)Get(c,"m_Infestation"); }
    private static List<Mobile> Mobs(ArachnidOutbreakController c) { return (List<Mobile>)Get(c,"m_Creatures"); }
    private static ArachnidOutbreakChampion Queen(ArachnidOutbreakController c) { return Mobs(c).Find(m=>m is ArachnidOutbreakChampion) as ArachnidOutbreakChampion; }
    private static ArachnidOutbreakController Start()
    {
        ArachnidOutbreakController c = new ArachnidOutbreakController();
        ((ArachnidOutbreakState)Get(c,"m_State")).SetEnabled(true);
        Check((bool)Call(c,"BeginInfestation",new Point3D(2514,2088,0),DateTime.UtcNow),"legal origin starts a boss-first infestation");
        return c;
    }
    private static Mobile Player()
    {
        Mobile m = new Mobile(); m.Player = true; m.Body = 0x190; m.RawStr = 100; m.Hits = m.HitsMax; m.AddItem(new Backpack()); return m;
    }
    private static int GroundGold()
    {
        int n = 0; foreach(Item i in World.Items.Values) if(i is Gold && i.Parent == null && i.Map == Map.Sosaria) n += i.Amount; return n;
    }
    private static int Glands(Mobile owner)
    {
        int n=0;foreach(Item i in World.Items.Values) { ArachnidOutbreakClaim claim=i as ArachnidOutbreakClaim; if(claim!=null && claim.Owner==owner && claim.Kind==ArachnidOutbreakClaimKind.Glands) foreach(Item reward in claim.Items)n+=reward.Amount; } return n;
    }
    private static void ClearRewards()
    {
        List<Item> list=new List<Item>();foreach(Item i in World.Items.Values)if(i is Gold || i is ArachnidOutbreakClaim || i is Corpse)list.Add(i);
        foreach(Item i in list)i.Delete();
    }
    private static void EarlyQueen(bool playerCredit)
    {
        ArachnidOutbreakController c=Start();Mobile player=Player();LargeSpider npc=new LargeSpider();
        try
        {
            Check(c.Phase==ArachnidOutbreakPhase.Boss && c.ClearedBroods==0 && Queen(c).Alive,"queen is vulnerable before any suppression milestone");
            Check(!Queen(c).HasBreath,"queen cannot inherit ordinary paralysis breath");
            Check(Queen(c).AIObject!=null && Queen(c).AIObject.m_Timer.Running,"event queen AI activates without relying on player sector entry");
            ArachnidOutbreakChampion queen=Queen(c);
            queen.Damage(9000,playerCredit?player:npc);queen.Kill();
            Check(c.Phase==ArachnidOutbreakPhase.Cooldown && c.InfestationPopulation==0,"early queen death settles and removes owned colony: "+c.Phase+"; "+c.Status+"; queen="+(Queen(c)==null?"none":Queen(c).Hits.ToString()));
            Check(GroundGold()==(playerCredit?15000:0),"NPC-only queen death cannot mint shared gold; earned early victory retains 15000");
            Check(Glands(player)==(playerCredit?3:0),"early queen reward does not require fixed brood stages");
            int gold=GroundGold();Call(c,"CreatureDied",Queen(c),null);
            Check(GroundGold()==gold,"repeated queen notification cannot mint another payout");
        }
        finally { c.Delete();player.Delete();npc.Delete();ClearRewards(); }
    }
    private static void FounderContainment()
    {
        ArachnidOutbreakController c=Start();LargeSpider guardProxy=new LargeSpider();
        try
        {
            ArachnidInfestationState s=Colony(c);DateTime now=s.Started;
            for(int i=0;i<50 && s.Patches.Count==1;++i) { now=now.AddSeconds(45);Call(c,"Tick",now); }
            Check(s.Patches.Count>1,"connected frontier founder is sent on real ground");
            ArachnidInfestationPatch child=s.Patches[s.Patches.Count-1];
            Check(!child.Established && child.Founder!=null && child.Founder.Alive && s.Epoch==0,"sending a founder alone does not mature the colony");
            Mobile founder=child.Founder;founder.Damage(1000,guardProxy);founder.Kill();
            Check(child.Retired && s.Epoch==0,"NPC frontier kill prevents establishment and maturation");
            Check(GroundGold()==0 && c.ClearedBroods==0,"NPC frontier containment earns no personal milestone or gold");
            Call(c,"Tick",now.AddSeconds(5));Check(s.Epoch==0,"elapsed ticks do not upgrade a defended frontier");
        }
        finally {c.Delete();guardProxy.Delete();ClearRewards();}
    }
    private static void GrowthAndBounds()
    {
        ArachnidOutbreakController c=Start();
        try
        {
            ArachnidInfestationState s=Colony(c);DateTime now=s.Started;int successful=0;
            for(int i=0;i<1500 && s.Patches.Count<ArachnidInfestationState.MaximumPatches;++i)
            {
                // AI timers are not drained here. Quiet actors use the real compaction path;
                // native cases remain the evidence for autonomous movement and resistance.
                now=now.AddSeconds(5);Call(c,"Tick",now);
                if(!c.Infestation)break;
                Check(s.Patches.Count<=ArachnidInfestationState.MaximumPatches,"finite map bound holds while structural growth is independent of the background actor target");
                Check(s.RouteRequests<=2,"director respects local path request cap");
                successful=s.Epoch;
                if(successful>=4)break;
            }
            if(successful==0)
            {
                foreach(ArachnidInfestationPatch patch in s.Patches)
                {
                    Console.WriteLine("PATCH "+patch.Id+" retired="+patch.Retired+" failures="+patch.Failures+" goal="+patch.Center+" founder="+(patch.Founder==null?"none":patch.Founder.Location+" hits="+patch.Founder.Hits));
                    if(patch.Founder!=null)
                    {
                        Direction direction=patch.Founder.GetDirectionTo(patch.Center);int z;
                        Console.WriteLine("MOVE geometry="+patch.Founder.CheckMovement(direction,out z)+" actual="+patch.Founder.Move(direction)+" frozen="+patch.Founder.Frozen+" stam="+patch.Founder.Stam+"/"+patch.Founder.StamMax);
                    }
                }
                foreach(Item item in (List<Item>)Get(c,"m_Signs"))Console.WriteLine("WEB "+item.ItemID+" flags="+item.ItemData.Flags+" loc="+item.Location);
            }
            Check(successful>0,"surviving founders establish connected broods without teleporting");
            int weakest=4,strongest=0;
            foreach(ArachnidInfestationPatch patch in s.Patches)if(patch.Established && !patch.Retired) { weakest=Math.Min(weakest,s.Tier(patch));strongest=Math.Max(strongest,s.Tier(patch)); }
            Check(strongest==4 && weakest>=0,"nearby early expansion retains queen-distance classification; outskirts boundaries are checked separately");
            int before=s.Epoch;DateTime started=s.Started;DateTime last=s.LastTick;
            s.Resume(last.AddHours(8));Check(s.Epoch==before && s.Started-started==TimeSpan.FromHours(8),"downtime freezes growth and lifetime instead of creating catch-up waves");
            MemoryStream bytes=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(bytes,true);s.Serialize(writer);writer.Write(613201);writer.Close();
            BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));ArachnidInfestationState restored=new ArachnidInfestationState();restored.Deserialize(reader);
            Check(reader.ReadInt()==613201 && restored.Patches.Count==s.Patches.Count && restored.Births==s.Births && restored.Epoch==s.Epoch,"value-state positional serialization preserves boundaries and finite budgets");reader.Close();
            for(int i=0;i<s.Patches.Count;++i)
                Check(restored.Patches[i].Guardian==(s.Patches[i].Guardian!=null && !s.Patches[i].Guardian.Deleted?s.Patches[i].Guardian:null)
                    && restored.Patches[i].Founder==(s.Patches[i].Founder!=null && !s.Patches[i].Founder.Deleted?s.Patches[i].Founder:null),
                    "patch reload retains live identity and consumes deleted references without respawning them");
            Mobile unrelated=new LargeSpider();unrelated.MoveToWorld(new Point3D(2514,2088,0),Map.Sosaria);c.Stop();
            Check(!unrelated.Deleted && c.InfestationPopulation==0,"cleanup is attempt-owned and preserves unrelated creatures");unrelated.Delete();
        }
        finally {c.Delete();ClearRewards();}
    }
    private static void Milestones()
    {
        ArachnidOutbreakController c=Start();Mobile player=Player();
        try
        {
            ArachnidInfestationPatch root=Colony(c).Patches[0];Mobile guardian=root.Guardian;
            guardian.Damage(1000,player);guardian.Kill();
            Check(Glands(player)==1 && c.ClearedBroods==1,"qualified suppression creates one capped milestone");
            Call(c,"CreatureDied",guardian,null);Call(c,"Tick",DateTime.UtcNow.AddMinutes(1));
            Check(Glands(player)==1 && root.Retired,"repeat or retired nests cannot repeat entitlements");
            ArachnidOutbreakClaim claim=null;foreach(Item i in World.Items.Values)if(i is ArachnidOutbreakClaim && ((ArachnidOutbreakClaim)i).Owner==player)claim=(ArachnidOutbreakClaim)i;
            c.Stop();Check(claim!=null && !claim.Deleted,"earned claim survives colony cleanup");
        }
        finally {c.Delete();player.Delete();ClearRewards();}
    }
    private static void DynamicConflict()
    {
        ArachnidOutbreakController c=Start();Moongate portal=new Moongate();
        try
        {
            portal.MoveToWorld(Colony(c).Origin,Map.Sosaria);Point3D point;
            Check(!ArachnidInfestationTerrain.TryPoint(portal.X,portal.Y,out point),"new portals reject local colony placement");
            Call(c,"Tick",DateTime.UtcNow.AddSeconds(5));
            Check(c.Phase==ArachnidOutbreakPhase.Boss && Queen(c).Alive && Colony(c).Patches[0].Retired,"new structural conflict retires its branch without deleting queen or cancelling the whole attempt");
            Check(!portal.Deleted,"event conflict handling never deletes the player's portal");
        }
        finally {c.Delete();portal.Delete();ClearRewards();}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();ArachnidOutbreakController.Initialize();
            EarlyQueen(true);EarlyQueen(false);FounderContainment();GrowthAndBounds();Milestones();DynamicConflict();
            Check(WildernessEncounterReserves.Proposals.Count==0 && !WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria,2514,2088),"permanent reservations are retired");
            Console.WriteLine("PASS infestation: "+Checks+" source-method assertions; no saves/listener; native town/appearance acceptance separate.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
