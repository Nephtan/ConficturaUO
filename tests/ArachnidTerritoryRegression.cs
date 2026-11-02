using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura.Champions;

internal static class ArachnidTerritoryRegression
{
    private static int Checks;
    private static readonly BindingFlags Hidden = BindingFlags.Instance|BindingFlags.NonPublic;
    private static void Check(bool value,string message) { ++Checks; if (!value) throw new Exception(message); }
    private static object Get(object value,string field) { return value.GetType().GetField(field,Hidden).GetValue(value); }
    private static object Call(object owner,string method,params object[] args)
    { return owner.GetType().InvokeMember(method,Hidden|BindingFlags.InvokeMethod,null,owner,args); }
    private static void Geometry()
    {
        Point3D voidPoint;
        Check(!ArachnidInfestationTerrain.TryPoint(4500,2600,out voidPoint),"custom-map void floor is not a random origin or an expansion route despite unflagged ground");
        Point3D queen = new Point3D(2000,1500,0);
        for (int x=-151;x<=150;++x) for (int y=-151;y<=150;++y)
        {
            int tier = ArachnidTerritoryPolicy.TierAt(new Point3D(queen.X+x,queen.Y+y,0),queen);
            bool outside = x < -150 || x >= 150 || y < -150 || y >= 150;
            Check(outside ? tier==0 : tier>=2 && tier<=4,"exact queen-centered square and outer low tier");
        }
        Check(ArachnidTerritoryPolicy.TierAt(new Point3D(2049,1549,0),queen)==4,"strongest central layer");
        Check(ArachnidTerritoryPolicy.TierAt(new Point3D(2050,1500,0),queen)==3,"central boundary enters middle layer");
        Check(ArachnidTerritoryPolicy.TierAt(new Point3D(2100,1500,0),queen)==2,"middle boundary enters third-highest layer");
        int eggs=0,spiders=0;
        for (int x=1800;x<1944;x+=12) for(int y=1800;y<1944;y+=12)
        { Point3D p=new Point3D(x,y,0);if(ArachnidTerritoryPolicy.HasEggSac(p))++eggs;if(ArachnidTerritoryPolicy.HasFrontierSpider(p))++spiders; }
        Check(eggs==16 && spiders==36,"outskirts density independently spaces one sac per nine cells and one spider per four");
    }
    private static void Serialization()
    {
        for(int version=0;version<=2;++version)
        {
            MemoryStream stream=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(stream,true);
            writer.Write(version);writer.Write(0);writer.Write(-1);writer.Write(0);writer.Write(0);writer.Write(1);
            writer.Write(new Point3D(2400,2100,0));writer.Write((Mobile)null);writer.Write((Mobile)null);
            writer.Write(true);writer.Write(true);writer.Write(DateTime.MinValue);writer.Write(DateTime.MinValue);
            if(version>=1){writer.Write((byte)0);writer.Write(true);writer.Write(0);}
            if(version>=2)writer.Write(9);
            writer.Write(719421);writer.Close();
            BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(stream.ToArray())));
            ArachnidInfestationPatch patch=new ArachnidInfestationPatch();patch.Deserialize(reader);
            Check(patch.Retired && patch.ResponseBirths==(version>=2?9:0) && reader.ReadInt()==719421,"old and current patch layouts preserve suppression, budget and positional boundary");reader.Close();
        }
    }
    private static void Planner()
    {
        QueenBroodMassPlanner planner=new QueenBroodMassPlanner(new Point3D(2400,2100,0),0);
        for(int i=0;i<50;++i)planner.Add(i,new Point3D(2400+i*12,2100,0),1);
        while(!planner.Complete)planner.Advance(16);
        Check(planner.Goal.X>=2650 && planner.Goal.X<=2730,"asymmetric occupied territory moves goal far from original nest");
        Check(planner.Route.Count>15,"planner supplies a real occupied route rather than an isolated teleport goal");
        planner=new QueenBroodMassPlanner(new Point3D(2400,2100,0),0);planner.Add(0,new Point3D(2400,2100,0),1);
        for(int i=0;i<20;++i)planner.Add(i+1,new Point3D(3000+i*12,2100,0),1);
        while(!planner.Complete)planner.Advance(16);
        Check(planner.Goal.X>=3000,"larger disconnected territory outranks current queen location");
    }
    private static void TuningAndTactics()
    {
        ArachnidInfestationPatch patch=new ArachnidInfestationPatch();patch.Center=new Point3D(2400,2100,0);
        Check(ArachnidTerritoryPolicy.PopulationTarget(patch,2,45)==1 && ArachnidTerritoryPolicy.PopulationTarget(patch,3,45)==2 && ArachnidTerritoryPolicy.PopulationTarget(patch,4,45)==3,"candidate inner populations are1/2/3 rather than3/5/7");
        Check(ArachnidTerritoryPolicy.PopulationTarget(patch,2,100)==3 && ArachnidTerritoryPolicy.PopulationTarget(patch,4,100)==7,"full-density control retains previous target counts for paired comparisons");
        int nests=0;for(int x=0;x<100;++x)if(ArachnidTerritoryPolicy.HasInnerNest(new Point3D(x*12,0,0),45))++nests;
        Check(nests==45,"density also spaces guardian nests instead of making every low-population cell a guardian");
        ArachnidOutbreakTuning tuning=new ArachnidOutbreakTuning();tuning.Validate();
        MemoryStream stream=new MemoryStream();BinaryFileWriter writer=new BinaryFileWriter(stream,true);tuning.Serialize(writer);writer.Write(717091);writer.Close();
        BinaryFileReader reader=new BinaryFileReader(new BinaryReader(new MemoryStream(stream.ToArray())));ArachnidOutbreakTuning restored=new ArachnidOutbreakTuning();restored.Deserialize(reader);
        Check(restored.InnerPopulationPercent==45 && restored.ReplacementSeconds==30 && restored.FrontierPerception==8 && restored.QueenBackupCount==6 && reader.ReadInt()==717091,"tuning value snapshot preserves its positional boundary");reader.Close();
        tuning.FrontierPursuit=12;tuning.FrontierPerception=12;bool invalid=false;try{tuning.Validate();}catch(ArgumentException){invalid=true;}
        Check(invalid,"invalid perception/pursuit combination cannot be installed");
        tuning=new ArachnidOutbreakTuning();tuning.QueenReengagePercent=50;invalid=false;try{tuning.Validate();}catch(ArgumentException){invalid=true;}
        Check(invalid,"queen thresholds enforce a meaningful hysteresis gap");
        ArachnidOutbreakController controller=new ArachnidOutbreakController();
        try
        {
            controller.InnerPopulationPercent=100;controller.ReplacementSeconds=5;
            Check(controller.InnerPopulationPercent==100 && controller.ReplacementSeconds==5,"disabled controller supports a density/replacement control configuration");
            ArachnidOutbreakTuning copy=controller.Tuning;copy.InnerPopulationPercent=20;
            Check(controller.InnerPopulationPercent==100,"editing returned snapshot does not live-retune controller");
            ((ArachnidOutbreakState)Get(controller,"m_State")).SetEnabled(true);invalid=false;
            try{controller.InnerPopulationPercent=45;}catch(InvalidOperationException){invalid=true;}
            Check(invalid && controller.InnerPopulationPercent==100,"active settings edits fail without partial mutation");
        }
        finally{controller.Delete();}
        Check(ArachnidQueenAdvantage.Favored(95,80,0,4,1,70),"healthy queen with local advantage can engage");
        Check(ArachnidQueenAdvantage.Losing(40,0,8,1,45),"low health requires withdrawal despite numerical advantage");
        Check(ArachnidQueenAdvantage.Losing(80,15,4,1,45),"recent substantial actual damage can trigger retreat before critical health");
        Check(!ArachnidQueenAdvantage.Favored(60,80,0,4,1,70),"partially healed queen does not immediately reengage across the retreat threshold");
        Check(!ArachnidQueenAdvantage.Favored(90,80,10,4,1,70),"ongoing pressure prevents an apparently healthy queen treating the fight as won");
    }
    private static void ProductionInteractions()
    {
        ArachnidOutbreakController owner=new ArachnidOutbreakController();Mobile player=new Mobile();player.Player=true;player.Body=0x190;player.RawStr=100;player.Hits=100;player.AddItem(new Backpack());
        Mobile bystander=new Mobile();bystander.Player=true;bystander.Body=0x190;
        Hind pet=new Hind();LargeSpider npc=new LargeSpider();
        try
        {
            ((ArachnidOutbreakState)Get(owner,"m_State")).SetEnabled(true);
            Check((bool)Call(owner,"BeginInfestation",new Point3D(2514,2088,0),DateTime.UtcNow),"production controller starts on legal terrain");
            ArachnidInfestationState state=(ArachnidInfestationState)Get(owner,"m_Infestation");
            state.QueenCenter=new Point3D(3000,2500,0);state.QueenCenterKnown=true;
            ArachnidInfestationPatch patch=state.Patches[0];
            ArachnidInfestationCreature guardian=(ArachnidInfestationCreature)patch.Guardian;
            guardian.Hits=17;guardian.Mana=3;guardian.Stam=5;guardian.ApplyTier(0);
            Check(guardian.Tier==0 && guardian.Hits==17 && guardian.Mana<=3 && guardian.Stam<=5,"reclassification cannot heal or refill finite resources");
            ArachnidInfestationPatch dormant=new ArachnidInfestationPatch();dormant.Id=state.Patches.Count;dormant.Parent=0;
            dormant.Center=new Point3D(2502,2076,0);dormant.Established=true;dormant.Initialized=true;state.Add(dormant);
            Check(ArachnidTerritoryPolicy.PopulationTarget(dormant,0)==0,"density regression uses an empty frontier cluster");
            for(byte slot=1;slot<=5;++slot)
            {
                ArachnidInfestationResident resident=new ArachnidInfestationResident();resident.Slot=slot;resident.Tier=4;
                resident.Identity=Guid.NewGuid();resident.Hits=30;resident.MaximumHits=350;resident.Position=dormant.Center;
                dormant.Residents.Add(resident);
            }
            int beforeHydration=World.Mobiles.Count;
            Call(owner,"HydrateInfestationPatch",dormant,DateTime.UtcNow);
            Check(dormant.Residents.Count==0 && World.Mobiles.Count==beforeHydration,"moved core cannot rehydrate obsolete dense dormant population into visible outskirts");
            ArachnidInfestationPatch egg=null;
            for(int x=2502;x<2550 && egg==null;x+=12)for(int y=2076;y<2124 && egg==null;y+=12)
            {
                Point3D point;
                if(!ArachnidTerritoryPolicy.HasEggSac(new Point3D(x,y,0)) || !ArachnidInfestationTerrain.TryPoint(x,y,out point))continue;
                egg=new ArachnidInfestationPatch();egg.Id=state.Patches.Count;egg.Parent=0;egg.Center=point;egg.Established=true;state.Add(egg);Call(owner,"EstablishPopulation",egg,DateTime.UtcNow);
            }
            Check(egg!=null && egg.Guardian!=null,"production outskirts creates its sparse attackable sac");
            ArachnidInfestationCreature sac=(ArachnidInfestationCreature)egg.Guardian;
            Check(sac.EggSac && sac.HitsMax==60 && sac.CantWalk && !sac.Frozen && sac.AIObject.m_Timer.Running,"sac is weak stationary structure with ordinary AI timer, not frozen budget actor");
            player.MoveToWorld(egg.Center,Map.Sosaria);bystander.MoveToWorld(egg.Center,Map.Sosaria);pet.SetControlMaster(player);pet.MoveToWorld(egg.Center,Map.Sosaria);npc.MoveToWorld(egg.Center,Map.Sosaria);
            Check(!sac.CanBeHarmful(player,false),"egg sac cannot damage its attacker even if normal reactive AI acquires a combatant");
            ArachnidInfestationCreature frontier=new ArachnidInfestationCreature(owner,egg.Id,0,false,false);
            frontier.Home=egg.Center;frontier.MoveToWorld(egg.Center,Map.Sosaria);
            ((List<Mobile>)Get(owner,"m_Creatures")).Add(frontier);
            Check(frontier.ActiveSpeed==0.3 && frontier.RangePerception==8 && frontier.AIObject.m_Timer.Running,"weak frontier uses ordinary slower local patrol AI with eight-tile perception, not scheduler freezing");
            frontier.NextReacquireTime=DateTime.MinValue;
            Check(frontier.AIObject.AcquireFocusMob(8,FightMode.Closest,true,false,true),"ordinary unprovoked acquisition finds a visible legal nearby player without waiting to be attacked");
            Check(frontier.CanBeHarmful(player,false),"local frontier can fight a nearby player normally");
            player.MoveToWorld(new Point3D(egg.Center.X+25,egg.Center.Y,egg.Center.Z),Map.Sosaria);
            Check(!frontier.CanBeHarmful(player,false) && !frontier.IsEnemy(player),"frontier does not sustain harmful pursuit beyond its local patch");
            player.MoveToWorld(egg.Center,Map.Sosaria);
            ArachnidBroodReinforcement defender=new ArachnidBroodReinforcement(owner,egg.Id,2,new List<Mobile>{player});defender.Home=egg.Center;defender.MoveToWorld(egg.Center,Map.Sosaria);
            ((List<Mobile>)Get(owner,"m_Creatures")).Add(defender);
            Check(defender.CanBeHarmful(player,false) && defender.CanBeHarmful(pet,false),"response recognizes principal and controlled pet");
            Check(!defender.CanBeHarmful(bystander,false),"response excludes uninvolved player");
            Check(defender.Backpack==null || defender.Backpack.Items.Count==0,"reinforcement does not mint loot");
            for(int kill=0;kill<3;++kill)
            {
                ArachnidInfestationCreature victim=new ArachnidInfestationCreature(owner,egg.Id,0,false,false);
                ((List<Mobile>)Get(owner,"m_Creatures")).Add(victim);victim.MoveToWorld(egg.Center,Map.Sosaria);
                victim.Damage(1000,kill==0?(Mobile)pet:player);if(victim.Alive)victim.Kill();
            }
            Check(owner.BroodWarnings==1 && owner.BroodResponseBirths==0,"three attributable kills including a pet warn before any defenders arrive");
            Call(owner,"ServiceNestResponses",DateTime.UtcNow);
            Check(owner.BroodResponseBirths==0,"ordinary current time cannot bypass the ten-second warning");
            System.Collections.IDictionary responses=(System.Collections.IDictionary)Get(owner,"m_NestResistance");
            object response=responses[egg.Id];response.GetType().GetField("Due").SetValue(response,DateTime.UtcNow.AddSeconds(-1));
            for(int work=0;work<64 && owner.BroodResponseBirths<2;++work)Call(owner,"ServiceNestResponses",DateTime.UtcNow);
            Check(owner.BroodResponseBirths==2 && egg.ResponseBirths==2,"controlled due-time fixture produces exactly the finite first wave");
            player.MoveToWorld(new Point3D(egg.Center.X+49,egg.Center.Y,egg.Center.Z),Map.Sosaria);
            Check(!defender.CanBeHarmful(player,false),"local response has a practical pursuit boundary");
            player.MoveToWorld(egg.Center,Map.Sosaria);sac.Damage(1000,player);sac.Kill();
            Check(egg.Retired && !patch.Retired,"ordinary damage to sac suppresses its own patch, not independent parent territory");
            Check(!responses.Contains(egg.Id) && !defender.Deleted,"clearing a sac cancels its response without deleting existing combatants");
            Call(owner,"EstablishPopulation",egg,DateTime.UtcNow.AddMinutes(20));Check(egg.Retired,"suppressed nest cannot immediately recreate itself");
            Check(owner.FrontierRewardRecipients==1,"actual frontier objective damage earns one bounded supply entitlement");
            ArachnidOutbreakClaim supplies=null;
            foreach(Item item in World.Items.Values){ArachnidOutbreakClaim claim=item as ArachnidOutbreakClaim;if(claim!=null && claim.Owner==player && claim.Kind==ArachnidOutbreakClaimKind.FrontierSupplies)supplies=claim;}
            Check(supplies!=null && supplies.TryClaim(player),"frontier supplies use ordinary earned-claim delivery");
            Call(owner,"AwardFrontierReclamation",sac);
            int pendingSupplies=0;foreach(Item item in World.Items.Values){ArachnidOutbreakClaim claim=item as ArachnidOutbreakClaim;if(claim!=null && claim.Owner==player && claim.Kind==ArachnidOutbreakClaimKind.FrontierSupplies)++pendingSupplies;}
            Check(pendingSupplies==0 && owner.FrontierRewardRecipients==1,"collecting/deleting a claim does not reset the separate paid receipt");
            state.QueenCenter=patch.Center;state.QueenCenterKnown=true;player.MoveToWorld(new Point3D(patch.Center.X+12,patch.Center.Y,patch.Center.Z),Map.Sosaria);
            ArachnidOutbreakChampion queen=null;foreach(Mobile mobile in (List<Mobile>)Get(owner,"m_Creatures"))if(mobile is ArachnidOutbreakChampion)queen=(ArachnidOutbreakChampion)mobile;
            owner.GetType().GetField("m_QueenGoal",Hidden).SetValue(owner,patch.Id);
            owner.GetType().GetField("m_QueenMode",Hidden).SetValue(owner,ArachnidQueenMode.Engage);
            queen.MoveToWorld(new Point3D(patch.Center.X+20,patch.Center.Y,patch.Center.Z),Map.Sosaria);
            player.MoveToWorld(new Point3D(patch.Center.X+30,patch.Center.Y,patch.Center.Z),Map.Sosaria);queen.Combatant=player;
            Call(owner,"HandleQueenTactics",queen);
            Check(queen.Combatant==null && owner.QueenMode==ArachnidQueenMode.Regroup,"engagement pursuit is anchored to the refuge, not a moving leash that can kite across the landmass");
            owner.GetType().GetField("m_QueenMode",Hidden).SetValue(owner,ArachnidQueenMode.Withdraw);
            owner.GetType().GetField("m_NextQueenDecision",Hidden).SetValue(owner,DateTime.MinValue);
            owner.GetType().GetField("m_QueenCommitUntil",Hidden).SetValue(owner,DateTime.UtcNow.AddSeconds(15));
            Call(owner,"HandleQueenTactics",queen);
            Check(owner.QueenMode==ArachnidQueenMode.Withdraw,"losing sight of a target cannot cancel a committed retreat immediately");
            owner.GetType().GetField("m_QueenMode",Hidden).SetValue(owner,ArachnidQueenMode.Regroup);
            DateTime now=DateTime.UtcNow;
            ((Dictionary<Mobile,DateTime>)Get(owner,"m_QueenThreats"))[player]=now;
            Call(owner,"StartQueenBackup",queen,now);
            Check(owner.QueenBackupPending==3 && owner.QueenBackupSpent==0 && owner.QueenRallyPatch==patch.Id,"queen backup is finite, warned and tied to a real living source");
            owner.GetType().GetField("m_QueenBackupDue",Hidden).SetValue(owner,now.AddSeconds(-1));
            for(int work=0;work<256 && owner.QueenBackupPending>0;++work)Call(owner,"ServiceQueenBackup",queen,now);
            Check(owner.QueenBackupSpent==3 && owner.QueenBackupPending==0,"first queen call creates exactly three real defenders");
            now=now.AddSeconds(61);((Dictionary<Mobile,DateTime>)Get(owner,"m_QueenThreats"))[player]=now;Call(owner,"StartQueenBackup",queen,now);
            Check(owner.QueenBackupPending==3,"second call can use only the remaining finite allowance");
            Call(owner,"RetireInfestationBranch",patch.Id);Call(owner,"ServiceQueenBackup",queen,now.AddSeconds(20));
            Check(owner.QueenBackupSpent==3 && owner.QueenBackupPending==0 && owner.QueenBackupInterrupted==1,"destroyed rally source cancels pending backup without refunding spent births");
        }
        finally {owner.Delete();player.Delete();bystander.Delete();pet.Delete();npc.Delete();}
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();ArachnidOutbreakController.Initialize();
            Geometry();Serialization();Planner();TuningAndTactics();ProductionInteractions();
            Console.WriteLine("PASS territory: "+Checks+" component/source-method checks. Native movement, timing and player acceptance remain separate.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
