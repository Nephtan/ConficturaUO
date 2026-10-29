using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;

// Separate processes exercise actual World.Save/Load. No existing save is copied
// or loaded, and no listener, script-wide Initialize or main timer loop starts.
internal static class ArachnidOutbreakWorldReload
{
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
    }
    private static ArachnidOutbreakState State(ArachnidOutbreakController controller)
    {
        return (ArachnidOutbreakState)typeof(ArachnidOutbreakController).GetField("m_State", Private).GetValue(controller);
    }
    private static List<Mobile> Creatures(ArachnidOutbreakController controller)
    {
        return (List<Mobile>)typeof(ArachnidOutbreakController).GetField("m_Creatures", Private).GetValue(controller);
    }
    private static List<Item> Signs(ArachnidOutbreakController controller)
    {
        return (List<Item>)typeof(ArachnidOutbreakController).GetField("m_Signs", Private).GetValue(controller);
    }
    private static void Bootstrap(string assets)
    {
        Core.Assembly = typeof(Core).Assembly;
        Core.Expansion = Expansion.SA;
        Core.DataDirectories.Add(assets);
        ScriptCompiler.Assemblies = new Assembly[] { typeof(ArachnidOutbreakController).Assembly };
        MapDefinitions.Configure();
        RaceDefinitions.Configure();
        PoisonImpl.Configure();
        Corpse.Initialize();
        World.Load();
    }
    private static void SaveCase(string assets, string name)
    {
        Check(!Directory.Exists("Saves"), "worker must start with a new empty world");
        Bootstrap(assets);
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        ArachnidOutbreakState state = State(controller);
        if (name != "disabled") state.SetEnabled(true);
        if (name.StartsWith("broods-") || name.StartsWith("boss") || name == "engaged" || name == "cooldown" || name == "webs")
        {
            Point3D bossPoint = new Point3D(1000, 1000, 0);
            Point3D[] nests = new Point3D[] { new Point3D(972,980,0), new Point3D(1028,980,0), new Point3D(1000,1030,0) };
            ArachnidOutbreakPlacement placement = new ArachnidOutbreakPlacement(bossPoint, nests, new Point3D[] { bossPoint });
            typeof(ArachnidOutbreakController).GetMethod("BeginAt", Private).Invoke(controller,
                new object[] { new ArachnidOutbreakSite("reload-fixture",1000,1000,false), placement });
            int mask = name.StartsWith("broods-") ? Int32.Parse(name.Substring(7)) : name == "engaged" ? 0 : 7;
            foreach (Mobile mobile in Creatures(controller).ToArray())
            {
                ArachnidOutbreakSpider spider = (ArachnidOutbreakSpider)mobile;
                typeof(ArachnidOutbreakSpider).GetField("m_Reinforcements", Private).SetValue(spider, 15);
                if ((mask & (1 << spider.Stage)) != 0) spider.Kill();
            }
            if (mask != 0 || name == "engaged") state.RecordActivity(DateTime.UtcNow);
            if (name == "boss" || name == "cooldown" || name == "webs")
            {
                ArachnidOutbreakChampion boss = new ArachnidOutbreakChampion(controller);
                typeof(ArachnidOutbreakChampion).GetField("m_Reinforcements", Private).SetValue(boss, 7);
                Creatures(controller).Add(boss);
                typeof(ArachnidOutbreakController).GetField("m_BossCreated", Private).SetValue(controller, true);
                boss.MoveToWorld(bossPoint, Map.Sosaria);
                if (name == "webs")
                {
                    Mobile warned = new Mobile(); warned.Player = true; warned.Body = 0x190; warned.RawStr = 100; warned.Hits = warned.HitsMax; warned.MoveToWorld(bossPoint,Map.Sosaria);
                    Mobile held = new Mobile(); held.Player = true; held.Body = 0x190; held.RawStr = 100; held.Hits = held.HitsMax; held.MoveToWorld(bossPoint,Map.Sosaria);
                    ArachnidWebWarning warning = (ArachnidWebWarning)Activator.CreateInstance(typeof(ArachnidWebWarning),Private,null,new object[]{boss,warned},null);
                    ArachnidWebRestraint restraint = (ArachnidWebRestraint)Activator.CreateInstance(typeof(ArachnidWebRestraint),Private,null,new object[]{boss,held},null);
                    Check(ArachnidWebRestraint.Find(held) == restraint, "producer binds a live transient restraint before saving");
                    File.WriteAllText("webs.tsv",String.Join("\t",new string[]{warning.Serial.Value.ToString(),restraint.Serial.Value.ToString(),warned.Serial.Value.ToString(),held.Serial.Value.ToString(),boss.Serial.Value.ToString()}));
                }
                if (name == "cooldown") boss.Kill();
            }
        }
        if (name == "antivenom")
        {
            BroodAntivenomEffect.Initialize();Mobile owner=new Mobile();owner.Player=true;owner.RawStr=100;owner.Body=0x190;
            owner.AddItem(new Backpack());owner.MoveToWorld(new Point3D(2514,2088,0),Map.Sosaria);
            BroodAntivenom treatment=new BroodAntivenom();treatment.Amount=2;owner.Backpack.AddItem(treatment);string reason;
            Check(treatment.TryApply(owner,owner,out reason),"actual treatment creates saved effect");
            DreadSpider wild=new DreadSpider();owner.ApplyPoison(wild,Poison.Deadly);
            int count;BroodAntivenomEffect effect=BroodAntivenomEffect.Find(owner,out count);
            Check(effect.Charges==2 && !owner.Poisoned,"one protection consumed before save");
            File.WriteAllText("antivenom.tsv",String.Join("\t",new string[]{owner.Serial.Value.ToString(),effect.Serial.Value.ToString(),
                effect.Expires.Ticks.ToString(),effect.NextUse.Ticks.ToString(),wild.Serial.Value.ToString()}));
        }
        Item sentinel = new Item(0x1);
        sentinel.Name = "outbreak persistence sentinel";
        Signs(controller).Add(sentinel); // Deliberately foreign saved reference.
        DreadSpider unrelated = new DreadSpider();
        unrelated.MoveToWorld(new Point3D(1200,1200,0), Map.Sosaria);
        if (name == "rewards" || name == "engaged")
        {
            Mobile participant = new Mobile();
            participant.Player = true;
            participant.RawStr = 100;
            participant.Body = 0x192;
            participant.AddItem(new Backpack());
            ArachnidOutbreakLedger ledger = name == "rewards" ? new ArachnidOutbreakLedger("saved-awards",4320,9792)
                : (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger",Private).GetValue(controller);
            ledger.CreditDamage(1,participant,200);
            ledger.CreditValidatedSupport(2,participant,4.25);
            if (name == "rewards")
            {
                ledger.CreditDamage(0,participant,432); ledger.Seal(0);
                typeof(ArachnidOutbreakController).GetField("m_Ledger",Private).SetValue(controller,ledger);
                ArachnidOutbreakClaim brood = ArachnidOutbreakClaim.QueueGlands(participant,"saved-awards",0);
                ArachnidOutbreakClaim boss = ArachnidOutbreakClaim.QueueGlands(participant,"saved-awards",3);
                MethodInfo queue=typeof(ArachnidOutbreakClaim).GetMethod("QueueReward",BindingFlags.Static|BindingFlags.NonPublic);
                ArachnidOutbreakClaim skull=(ArachnidOutbreakClaim)queue.Invoke(null,new object[]{participant,"saved-awards",3,ArachnidOutbreakClaimKind.Skull,
                    new Func<Item>(delegate{return new ChampionSkull(Server.Engines.CannedEvil.ChampionSkullType.Venom);})});
                ArachnidOutbreakClaim artifact=(ArachnidOutbreakClaim)queue.Invoke(null,new object[]{participant,"saved-awards",3,ArachnidOutbreakClaimKind.Artifact,
                    new Func<Item>(delegate{return new Calm();})});
                File.WriteAllText("claims.tsv",String.Join("\t",new string[] { participant.Serial.Value.ToString(),brood.Serial.Value.ToString(),
                    boss.Serial.Value.ToString(),brood.Items[0].Serial.Value.ToString(),boss.Items[0].Serial.Value.ToString(),
                    skull.Serial.Value.ToString(),artifact.Serial.Value.ToString(),skull.Items[0].Serial.Value.ToString(),artifact.Items[0].Serial.Value.ToString() }));
            }
            File.WriteAllText("participant.txt",participant.Serial.Value.ToString());
            if (name == "engaged")
            {
                participant.Body=0x190;participant.Hits=participant.HitsMax;
                HealthChangedEventHandler handler=(HealthChangedEventHandler)Delegate.CreateDelegate(typeof(HealthChangedEventHandler),
                    typeof(ArachnidOutbreakController).GetMethod("OnHealthChanged",BindingFlags.Static|BindingFlags.NonPublic));
                EventSink.HealthChanged+=handler;
                try { participant.Damage(30,Creatures(controller)[0]); }
                finally { EventSink.HealthChanged-=handler; }
            }
        }
        string expected = String.Join("\t", new string[] { controller.Serial.Value.ToString(),
            ((int)controller.Phase).ToString(), controller.ClearedBroods.ToString(), controller.AttemptId,
            controller.SiteId, sentinel.Serial.Value.ToString(), unrelated.Serial.Value.ToString(),
            state.LastActivity.Ticks.ToString(), state.NextStart.Ticks.ToString(),
            Creatures(controller).FindAll(delegate(Mobile m) { return !m.Deleted; }).Count.ToString(),
            Signs(controller).FindAll(delegate(Item i) { return !i.Deleted; }).Count.ToString() });
        File.WriteAllText("expected.tsv", expected);
        World.Save(false, false);
        World.WaitForWriteCompletion();
        Check(File.Exists(World.ItemDataPath) && File.Exists(World.MobileDataPath), "actual world data files written");
        Console.WriteLine("SAVED " + name);
    }
    private static void LoadCase(string assets, string name)
    {
        string[] expected = File.ReadAllText("expected.tsv").Split('\t');
        Bootstrap(assets);
        ArachnidOutbreakController controller = World.FindItem((Serial)Int32.Parse(expected[0])) as ArachnidOutbreakController;
        Check(controller != null, "typed controller reconstructed");
        Check((int)controller.Phase == Int32.Parse(expected[1]) && controller.ClearedBroods == Int32.Parse(expected[2]), "phase and cleared mask preserved");
        Check(controller.AttemptId == expected[3] && controller.SiteId == expected[4], "attempt identity and site preserved");
        Check(State(controller).LastActivity.Ticks == Int64.Parse(expected[7]) && State(controller).NextStart.Ticks == Int64.Parse(expected[8]), "activity and cooldown preserved");
        Check(Creatures(controller).Count == Int32.Parse(expected[9]) && Signs(controller).Count == Int32.Parse(expected[10]), "strong reference lists restored");
        foreach (Mobile mobile in Creatures(controller))
        {
            IArachnidOutbreakOwned owned = mobile as IArachnidOutbreakOwned;
            Check(owned != null && owned.Controller == controller && owned.AttemptId == controller.AttemptId, "mobile back-reference resolves to loaded controller");
            ArachnidOutbreakSpider spider = mobile as ArachnidOutbreakSpider;
            if (spider != null)
            {
                Check(spider.Guardian && spider.Stage >= 0 && spider.Stage < 3, "guardian role and stage preserved");
                Check((int)typeof(ArachnidOutbreakSpider).GetField("m_Reinforcements", Private).GetValue(spider) == 15, "brood budget mask preserved");
            }
            else
                Check((int)typeof(ArachnidOutbreakChampion).GetField("m_Reinforcements", Private).GetValue(mobile) == 7, "boss budget mask preserved");
        }
        foreach (Item item in Signs(controller))
        {
            ArachnidBroodSign sign = item as ArachnidBroodSign;
            if (sign != null) Check(sign.Controller == controller && sign.AttemptId == controller.AttemptId, "sign back-reference restored");
        }
        if (name == "boss") Check((bool)typeof(ArachnidOutbreakController).GetField("m_BossCreated", Private).GetValue(controller), "boss-created guard preserved");
        if (name == "boss-pending") Check(!(bool)typeof(ArachnidOutbreakController).GetField("m_BossCreated", Private).GetValue(controller), "boss not yet created guard preserved");
        if (name == "engaged" || name == "rewards")
        {
            Mobile participant = World.FindMobile((Serial)Int32.Parse(File.ReadAllText("participant.txt")));
            ArachnidOutbreakLedger ledger = (ArachnidOutbreakLedger)typeof(ArachnidOutbreakController).GetField("m_Ledger",Private).GetValue(controller);
            Check(ledger.Points(1,participant)>4.6 && ledger.Points(2,participant)==4.25, "partial damage and fractional support survive item-before-mobile deserialization");
            Check(participant.Alive==(name=="engaged") && participant.NetState==null, "saved contributor life and offline state preserved");
            if(name=="engaged")
            {
                Mobile healer=new Mobile();healer.Player=true;
                HealthChangedEventHandler handler=(HealthChangedEventHandler)Delegate.CreateDelegate(typeof(HealthChangedEventHandler),
                    typeof(ArachnidOutbreakController).GetMethod("OnHealthChanged",BindingFlags.Static|BindingFlags.NonPublic));
                EventSink.HealthChanged+=handler;
                try
                {
                    participant.Heal(30,healer,false);
                    Check(ledger.Points(0,healer)==3,"actual persisted encounter injury earns only restored health after fresh load");
                    participant.Heal(30,healer,false);
                    Check(ledger.Points(0,healer)==3,"loaded injury cannot pay twice");
                }
                finally {EventSink.HealthChanged-=handler;healer.Delete();}
            }
        }
        if (name == "webs")
        {
            string[] web = File.ReadAllText("webs.tsv").Split('\t');
            ArachnidWebWarning warning = World.FindItem((Serial)Int32.Parse(web[0])) as ArachnidWebWarning;
            ArachnidWebRestraint restraint = World.FindItem((Serial)Int32.Parse(web[1])) as ArachnidWebRestraint;
            Check(warning != null && restraint != null && warning.Controller == controller && restraint.Controller == controller, "new serialized web types and controller references load");
            Check(warning.Victim == World.FindMobile((Serial)Int32.Parse(web[2])) && restraint.Victim == World.FindMobile((Serial)Int32.Parse(web[3]))
                && warning.Champion == World.FindMobile((Serial)Int32.Parse(web[4])) && restraint.Champion == warning.Champion, "web victim/boss references survive item-before-mobile loading");
            Check(ArachnidWebRestraint.Find(restraint.Victim) == null && !restraint.Victim.Frozen && !restraint.Victim.Paralyzed, "fresh process does not rebind a saved restraint or status flags");
        }
        ArachnidOutbreakController.Initialize();
        Check(controller.Phase == ArachnidOutbreakPhase.Disabled, "delivery gate disarms loaded controller");
        Check(Creatures(controller).Count == 0 && Signs(controller).Count == 0, "startup removed owned encounter objects");
        Check(World.FindItem((Serial)Int32.Parse(expected[5])) != null, "foreign item survives startup cleanup");
        Check(World.FindMobile((Serial)Int32.Parse(expected[6])) != null, "unrelated creature survives startup cleanup");
        if (name == "webs")
        {
            string[] web = File.ReadAllText("webs.tsv").Split('\t');
            Check(World.FindItem((Serial)Int32.Parse(web[0])) == null && World.FindItem((Serial)Int32.Parse(web[1])) == null, "gated startup deletes owned web markers without resuming callbacks");
        }
        if(name=="antivenom")
        {
            string[] data=File.ReadAllText("antivenom.tsv").Split('\t');Mobile owner=World.FindMobile((Serial)Int32.Parse(data[0]));
            BroodAntivenomEffect effect=World.FindItem((Serial)Int32.Parse(data[1])) as BroodAntivenomEffect;
            Check(effect!=null && effect.Owner==owner && effect.AppliedBy==owner && effect.Charges==2,"effect owner, applicator and consumed charges restored");
            Check(effect.Expires.Ticks==Int64.Parse(data[2]) && effect.NextUse.Ticks==Int64.Parse(data[3]),"effect expiry and reuse dates do not refresh on reload");
            BroodAntivenomEffect.Initialize();owner.ApplyPoison(World.FindMobile((Serial)Int32.Parse(data[4])),Poison.Lethal);
            Check(effect.Charges==1 && !owner.Poisoned && owner.NetState==null,"loaded effect protects offline owner without encounter activation");
            World.Save(false,false);World.WaitForWriteCompletion();
        }
        if (name == "rewards")
        {
            string[] claims=File.ReadAllText("claims.tsv").Split('\t');
            Mobile participant=World.FindMobile((Serial)Int32.Parse(claims[0]));
            ArachnidOutbreakClaim brood=World.FindItem((Serial)Int32.Parse(claims[1])) as ArachnidOutbreakClaim;
            ArachnidOutbreakClaim boss=World.FindItem((Serial)Int32.Parse(claims[2])) as ArachnidOutbreakClaim;
            Check(brood!=null && boss!=null && brood.Owner==participant && boss.Owner==participant, "pending owners restored independently of controller disarm");
            Check(brood.Kind==ArachnidOutbreakClaimKind.Glands && boss.Kind==ArachnidOutbreakClaimKind.Glands,"gland claim kind preserved including version-zero default");
            Check(brood.Items[0].Serial.Value==Int32.Parse(claims[3]) && boss.Items[0].Serial.Value==Int32.Parse(claims[4]), "exact saved reward identities restored");
            Check(brood.Map==Map.Internal && !brood.Movable && !brood.Visible && !brood.TryClaim(participant), "internal dead/offline claim remains protected");
            participant.Body=0x190;
            participant.Backpack.MaxItems=1;
            Item filler=new Item(1);participant.Backpack.AddItem(filler);
            Check(!brood.TryClaim(participant), "loaded reward retained when backpack is full");filler.Delete();
            Check(brood.TryClaim(participant) && !brood.TryClaim(participant), "loaded reward transfers once");
            participant.Backpack.MaxItems=125;
            Check(boss.TryClaim(participant) && participant.Backpack.GetAmount(typeof(BroodVenomGland))==4, "loaded boss reward merges without duplication");
            if(claims.Length>5)
            {
                ArachnidOutbreakClaim skull=World.FindItem((Serial)Int32.Parse(claims[5])) as ArachnidOutbreakClaim;
                ArachnidOutbreakClaim artifact=World.FindItem((Serial)Int32.Parse(claims[6])) as ArachnidOutbreakClaim;
                Check(skull.Kind==ArachnidOutbreakClaimKind.Skull && artifact.Kind==ArachnidOutbreakClaimKind.Artifact,"collectible claim kinds restored");
                Check(skull.Items[0].Serial.Value==Int32.Parse(claims[7]) && artifact.Items[0].Serial.Value==Int32.Parse(claims[8]),"exact saved collectible identities restored");
                Check(skull.TryClaim(participant) && artifact.TryClaim(participant),"saved named collectibles delivered once");
            }
            World.Save(false,false);World.WaitForWriteCompletion();
        }
        Console.WriteLine("RELOADED " + name);
    }
    private static void LoadPaid(string assets)
    {
        Bootstrap(assets);
        Mobile participant=World.FindMobile((Serial)Int32.Parse(File.ReadAllText("participant.txt")));
        foreach(Item item in World.Items.Values)
            Check(!(item is ArachnidOutbreakClaim), "paid claims do not reappear after another fresh reload");
        Check(participant.Backpack.GetAmount(typeof(BroodVenomGland))==4, "paid gland quantity survives a second fresh reload");
        if(File.ReadAllText("claims.tsv").Split('\t').Length>5)
            Check(participant.Backpack.GetAmount(typeof(ChampionSkull))==1 && participant.Backpack.GetAmount(typeof(Calm))==1,"paid named collectibles survive a second fresh reload");
        Console.WriteLine("RELOADED rewards");
    }
    private static void LoadEffectAgain(string assets)
    {
        Bootstrap(assets);string[] data=File.ReadAllText("antivenom.tsv").Split('\t');
        Mobile owner=World.FindMobile((Serial)Int32.Parse(data[0]));BroodAntivenomEffect effect=World.FindItem((Serial)Int32.Parse(data[1])) as BroodAntivenomEffect;
        Check(effect!=null && effect.Owner==owner && effect.Charges==1 && effect.Expires.Ticks==Int64.Parse(data[2]) && effect.NextUse.Ticks==Int64.Parse(data[3]),"post-consumption charges and original clocks persist through another fresh reload");
        Console.WriteLine("RELOADED antivenom");
    }
    private static void Worker(string exe, string assets, string mode, string name, string directory)
    {
        ProcessStartInfo start = new ProcessStartInfo(exe, "\"" + assets + "\" " + mode + " " + name);
        start.WorkingDirectory = directory;
        start.UseShellExecute = false;
        start.CreateNoWindow = true;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        using (Process process = Process.Start(start))
        {
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> error = process.StandardError.ReadToEndAsync();
            bool completed = process.WaitForExit(30000);
            if (!completed)
            {
                process.Kill();
                process.WaitForExit();
            }
            File.WriteAllText(Path.Combine(directory, mode + ".log"), output.Result + error.Result);
            Check(completed, "owned reload worker timed out: " + mode + " " + name + "; output retained");
            Check(process.ExitCode == 0 && output.Result.Contains(mode == "save" ? "SAVED " + name : "RELOADED " + name), "worker succeeded: " + mode + " " + name + "; see retained worker log");
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            if (args.Length == 3)
            {
                Check(Path.GetFullPath(Environment.CurrentDirectory).StartsWith(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "save worker confined beneath fixture snapshot");
                if (args[1] == "save") SaveCase(args[0], args[2]);
                else if (args[1] == "load") LoadCase(args[0], args[2]);
                else if (args[1] == "paid") LoadPaid(args[0]);
                else if (args[1] == "effect") LoadEffectAgain(args[0]);
                else throw new Exception("Unknown worker mode");
                return 0;
            }
            string exe = Assembly.GetExecutingAssembly().Location;
            string root = Path.Combine(Path.GetDirectoryName(exe), "outbreak-world-reload-" + Guid.NewGuid().ToString("N"));
            string[] cases = new string[] { "disabled", "searching", "broods-0", "engaged", "broods-1", "broods-2", "broods-3", "broods-4", "broods-5", "broods-6", "boss-pending", "boss", "cooldown", "rewards", "antivenom", "webs" };
            foreach (string name in cases)
            {
                string directory = Path.Combine(root, name);
                Directory.CreateDirectory(directory);
                Worker(exe, args[0], "save", name, directory);
                Worker(exe, args[0], "load", name, directory);
                if (name == "rewards") Worker(exe,args[0],"paid",name,directory);
                if (name == "antivenom") Worker(exe,args[0],"effect",name,directory);
                Console.WriteLine("PASS fresh-process world reload: " + name);
            }
            Console.WriteLine("PASS outbreak world reload: 16 worlds, 34 bounded workers; no existing saves or listeners.");
            return 0;
        }
        catch (Exception exception) { Console.Error.WriteLine(exception); return 1; }
    }
}
