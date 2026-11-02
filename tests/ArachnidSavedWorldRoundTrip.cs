using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

// Read an ordinary private World.Save without invoking global script initialization or starting listeners.
public static class ArachnidSavedWorldRoundTrip
{
    private static readonly BindingFlags Hidden=BindingFlags.Instance|BindingFlags.NonPublic;
    private static int Checks;
    private sealed class Before
    {
        public ArachnidInfestationCreature Actor;
        public Guid Identity;
        public int Hits,Stam,Mana,Pack;
    }
    private static void Check(bool condition,string reason){if(!condition)throw new Exception(reason);++Checks;}
    private static object Get(object owner,string name){return owner.GetType().GetField(name,Hidden).GetValue(owner);}
    public static int Main(string[] args)
    {
        try
        {
            Check(Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase),"Private copied-save directory required");
            Check(File.Exists("Saves/Items/Items.bin"),"Actual saved world required");
            Check(typeof(Core).Assembly.GetType("Server.PreviewClock")==null,"Unmodified operational clock required");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            // Offline failures must throw with the original cause, never offer to delete save objects.
            typeof(Core).GetField("m_Service",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,true);
            ScriptCompiler.Assemblies=new Assembly[]{typeof(ArachnidOutbreakController).Assembly};
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();World.Load();
            ArachnidOutbreakController owner=null;ArachnidInfestationState colony=null;
            foreach(Item item in World.Items.Values)
            {
                ArachnidOutbreakController candidate=item as ArachnidOutbreakController;
                if(candidate==null)continue;ArachnidInfestationState state=(ArachnidInfestationState)Get(candidate,"m_Infestation");
                int originX=args.Length>6 ? Int32.Parse(args[6]) : 2514, originY=args.Length>7 ? Int32.Parse(args[7]) : 2088;
                if(state!=null && state.Origin.X==originX && state.Origin.Y==originY){Check(owner==null,"Requested checkpoint director must be unique");owner=candidate;colony=state;}
            }
            Check(owner!=null && colony!=null,"Saved active infestation found");
            int retiredId=Int32.Parse(args[1]),minimumResponses=Int32.Parse(args[2]),responses=0;
            Check(retiredId<0 || colony.Find(retiredId)!=null && colony.Find(retiredId).Retired,"Requested player-cleared egg patch remained suppressed after World.Load");
            foreach(ArachnidInfestationPatch patch in colony.Patches){Check(patch.ResponseBirths>=0 && patch.ResponseBirths<=9,"Persisted nest response allowance bounded");responses+=patch.ResponseBirths;}
            Check(responses>=minimumResponses,"Actual response births survived positional save/load");
            PlayerMobile novice=World.FindMobile(203578) as PlayerMobile;
            bool expectDead=args.Length>4 && args[4]=="dead";
            int expectedNoviceHits=args.Length>5 ? Int32.Parse(args[5]) : 90;
            Check(novice!=null && (expectDead ? !novice.Alive && novice.Hits==0 : novice.Alive && novice.Hits==expectedNoviceHits),"Saved novice health and life state retained");
            bool carriedDagger=args.Length>10 && args[10]=="carried";
            Item weapon=expectDead || carriedDagger ? World.FindItem(1075032540) : novice.FindItemOnLayer(Layer.OneHanded);
            Check(weapon is Dagger && weapon.Serial.Value==1075032540,"Actual starting dagger retained its identity");
            if(carriedDagger && !expectDead)Check(weapon.Parent==novice || novice.Backpack!=null && weapon.IsChildOf(novice.Backpack),"Dagger remains carried by the same actual novice without requiring prior client equipment");
            List<Before> actors=new List<Before>();
            foreach(Mobile mobile in (List<Mobile>)Get(owner,"m_Creatures"))
            {
                ArachnidInfestationCreature actor=mobile as ArachnidInfestationCreature;
                if(actor==null || actor.Deleted || !actor.Alive)continue;
                Before before=new Before();before.Actor=actor;before.Identity=actor.Identity;before.Hits=actor.Hits;before.Stam=actor.Stam;before.Mana=actor.Mana;before.Pack=actor.Backpack==null?0:actor.Backpack.Serial.Value;actors.Add(before);
            }
            int mobiles=World.Mobiles.Count,items=World.Items.Count;
            int queenSpent=owner.QueenBackupSpent,paidRecipients=owner.FrontierRewardRecipients;
            Check(queenSpent>=(args.Length>8 ? Int32.Parse(args[8]) : 0),"Requested actual queen backup expenditure survived load");
            Check(paidRecipients>=(args.Length>9 ? Int32.Parse(args[9]) : 0),"Requested actual frontier-paid receipts survived load");
            owner.GetType().GetMethod("RecoverInfestation",Hidden).Invoke(owner,null);
            Check(World.Mobiles.Count==mobiles && World.Items.Count==items,"Recovery creates no replacement actors or inventory");
            Check(owner.QueenBackupSpent==queenSpent && owner.FrontierRewardRecipients==paidRecipients,"Recovery refunds no queen births and resets no frontier-paid receipts");
            int frontier=0,founders=0,reinforcements=0;
            foreach(Before before in actors)
            {
                ArachnidInfestationCreature actor=before.Actor;
                Check(!actor.Deleted && actor.Identity==before.Identity,"Live actor identity survives policy recovery");
                Check(actor.Hits<=before.Hits && actor.Stam<=before.Stam && actor.Mana<=before.Mana,"Recovery does not replenish finite resources");
                Check((actor.Backpack==null?0:actor.Backpack.Serial.Value)==before.Pack,"Live inventory identity retained");
                ArachnidBroodReinforcement reinforcement=actor as ArachnidBroodReinforcement;
                if(reinforcement!=null)
                {
                    ++reinforcements;
                    List<Mobile> principals=(List<Mobile>)Get(reinforcement,"m_Principals");
                    Check(principals.Count>0,"Actual saved defender retains contributing principals");
                    foreach(Mobile principal in principals)
                        Check(principal!=null && principal.Player && Object.ReferenceEquals(principal,World.FindMobile(principal.Serial)),"Defender principal resolves to the actual loaded player identity");
                    Check(!principals.Contains(novice),"Non-contributing novice does not acquire a defender participation flag during reload");
                }
                if(actor.Tier==0)
                {
                    ++frontier;if(actor.Founder)++founders;
                    Check(actor.ActiveSpeed==0.3 && actor.RangePerception==owner.FrontierPerception,"Saved frontier/founder receives the saved controller travel policy");
                    Check(actor.EggSac || actor.Body.BodyID==28,"Frontier migration uses spider model");
                }
            }
            Check(minimumResponses==0 || reinforcements>0,"Spent response snapshot also contains real saved defenders to verify");
            Console.WriteLine("PASS copied-world load/recovery: checks="+Checks+" patches="+colony.Patches.Count+" actors="+actors.Count+" frontier="+frontier+" founders="+founders+" responseBirths="+responses+" savedDefenders="+reinforcements+" suppressionChecked="+(retiredId>=0)+". No global script initialization, listener, gameplay resume or World.Save.");
            if(args.Length>3 && args[3]!="-")NavigationGrid(args[3]);
            return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
    private static void NavigationGrid(string output)
    {
        string path=Path.GetFullPath(output);
        Check(path.StartsWith(Path.GetFullPath(Environment.CurrentDirectory)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Grid output must remain beside isolated test");
        const int left=2240,right=2860,top=1960,bottom=2220;
        Dictionary<Point2D,Point3D> points=new Dictionary<Point2D,Point3D>();
        for(int x=left;x<=right;++x)for(int y=top;y<=bottom;++y)
        {Point3D point;if(ArachnidInfestationTerrain.TryPoint(x,y,out point))points.Add(new Point2D(x,y),point);}
        Mobile walker=new Mobile();walker.Player=true;walker.Body=0x190;walker.Map=Map.Sosaria;
        int[] dx={0,1,1,1,0,-1,-1,-1},dy={-1,-1,0,1,1,1,0,-1};
        using(StreamWriter writer=new StreamWriter(path))
        {
            writer.Write("{\"map\":1,\"scope\":\"Supplied copied-map terrain route; actual movement remains client native\",\"nodes\":[");bool first=true;
            foreach(KeyValuePair<Point2D,Point3D> entry in points)
            {
                int mask=0;
                for(int i=0;i<8;++i)
                {
                    Point3D next;int z;
                    if(points.TryGetValue(new Point2D(entry.Key.X+dx[i],entry.Key.Y+dy[i]),out next)
                        && Server.Movement.Movement.CheckMovement(walker,Map.Sosaria,entry.Value,(Direction)i,out z) && Math.Abs(z-next.Z)<=2)mask|=1<<i;
                }
                if(!first)writer.Write(',');first=false;
                writer.Write("["+entry.Value.X+","+entry.Value.Y+","+entry.Value.Z+","+mask+"]");
            }
            writer.Write("]}");
        }
        walker.Delete();Console.WriteLine("Private navigation grid written: nodes="+points.Count+". Terrain only; dynamic NPC collision remains native.");
    }
}
