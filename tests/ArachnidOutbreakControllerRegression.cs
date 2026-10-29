using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Custom.Confictura.Champions;
using Server.Commands;

// Production controller/component tests; private fixture setup bypasses placement
// approval only inside this process. This is not native client acceptance.
internal static class ArachnidOutbreakControllerRegression
{
    private static int s_Checks;
    private static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        ++s_Checks;
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
    private static object Invoke(ArachnidOutbreakController controller, string method, params object[] args)
    {
        return typeof(ArachnidOutbreakController).GetMethod(method, Private).Invoke(controller, args);
    }
    private static ArachnidOutbreakController Begin()
    {
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        State(controller).SetEnabled(true);
        Point3D boss = new Point3D(1000, 1000, 0);
        Point3D[] nests = new Point3D[] { new Point3D(972, 980, 0), new Point3D(1028, 980, 0), new Point3D(1000, 1030, 0) };
        ArachnidOutbreakPlacement placement = new ArachnidOutbreakPlacement(boss, nests, new Point3D[] { boss });
        Invoke(controller, "BeginAt", new ArachnidOutbreakSite("fixture", 1000, 1000, false), placement);
        return controller;
    }
    private static void Disabled()
    {
        ArachnidOutbreakController first = new ArachnidOutbreakController();
        try
        {
            string reason;
            Check(first.Phase == ArachnidOutbreakPhase.Disabled, "new controller is disabled");
            Check(!first.TryEnable(out reason) && reason.Contains("acceptance"), "normal activation stays gated");
            Check(Creatures(first).Count == 0 && Signs(first).Count == 0, "construction spawns nothing");
            Check(typeof(ArachnidOutbreakController).GetField("m_Timer", Private).GetValue(first) == null, "construction starts no timer");
            ArachnidOutbreakController second = new ArachnidOutbreakController();
            try { Check(!first.TryEnable(out reason) && reason.Contains("ambiguous"), "duplicate controllers fail closed"); }
            finally { second.Delete(); }
        }
        finally { first.Delete(); }
    }
    private static void CommandAccess()
    {
        int before = World.Items.Count;
        ArachnidOutbreakController.Initialize();
        Check(World.Items.Count == before, "startup registers commands without creating a controller");
        CommandEntry entry = CommandSystem.Entries["ArachnidOutbreak"];
        Check(entry.AccessLevel == AccessLevel.Administrator, "command registration requires administrator");
        Mobile player = new Mobile();
        player.AccessLevel = AccessLevel.Player;
        try
        {
            entry.Handler(new CommandEventArgs(player, "ArachnidOutbreak", "create", new string[] { "create" }));
            Check(World.Items.Count == before, "handler independently refuses low-access creation");
            player.AccessLevel = AccessLevel.Administrator;
            entry.Handler(new CommandEventArgs(player, "ArachnidOutbreak", "create", new string[] { "create" }));
            ArachnidOutbreakController created = null;
            foreach (Item item in World.Items.Values)
                if (item is ArachnidOutbreakController) created = (ArachnidOutbreakController)item;
            Check(created != null && created.Phase == ArachnidOutbreakPhase.Disabled, "administrator creates disabled controller");
            try
            {
                entry.Handler(new CommandEventArgs(player, "ArachnidOutbreak", "start", new string[] { "start" }));
                Check(created.Phase == ArachnidOutbreakPhase.Disabled, "command cannot bypass delivery gate");
            }
            finally { if (created != null) created.Delete(); }
        }
        finally { player.Delete(); }
    }
    private static void PortalClearance()
    {
        Point3D point = new Point3D(1400, 1400, 0);
        Rectangle2D bounds = new Rectangle2D(1400, 1400, 1, 1);
        SosariaOutbreakTerrain terrain = new SosariaOutbreakTerrain();
        Moongate permanent = new Moongate();
        Item transient = null;
        try
        {
            permanent.MoveToWorld(point, Map.Sosaria);
            Check(SosariaOutbreakTerrain.IsPortal(permanent), "ordinary moongate recognized");
            Check(terrain.HasStructuralConflict(bounds), "permanent gate excludes active site");
            permanent.Delete();
            Type[] spells = new Type[] { typeof(Server.Spells.Seventh.GateTravelSpell),
                typeof(Server.Spells.Elementalism.Elemental_Gate_Spell),
                typeof(Server.Spells.Herbalist.MushroomGatewaySpell),
                typeof(Server.Spells.Undead.UndeadGraveyardGatewaySpell) };
            object[][] arguments = new object[][] { new object[] { point, Map.Sosaria },
                new object[] { point, Map.Sosaria, 0 }, new object[] { point, Map.Sosaria, 0.0 },
                new object[] { point, Map.Sosaria, 0 } };
            for (int i = 0; i < spells.Length; ++i)
            {
                Type gateType = spells[i].GetNestedType("InternalItem", BindingFlags.NonPublic);
                transient = (Item)Activator.CreateInstance(gateType, arguments[i]);
                transient.MoveToWorld(point, Map.Sosaria);
                Check(terrain.IsReserved(bounds), spells[i].Name + " excludes fresh placement");
                Check(!terrain.HasStructuralConflict(bounds), spells[i].Name + " does not cancel active attempt");
                transient.Delete();
            }
        }
        finally
        {
            permanent.Delete();
            if (transient != null) transient.Delete();
        }
    }
    private static void FailedReturnAborts()
    {
        ArachnidOutbreakController controller = Begin();
        try
        {
            ArachnidOutbreakSpider guardian = (ArachnidOutbreakSpider)Creatures(controller)[0];
            Point3D point = guardian.Location;
            typeof(ArachnidOutbreakController).GetField("m_Placement", Private).SetValue(controller,
                new ArachnidOutbreakPlacement(point, new Point3D[] { point, point, point }, new Point3D[0]));
            guardian.OnThink();
            Check(controller.Phase == ArachnidOutbreakPhase.Cooldown && guardian.Deleted,
                "failed return cancels rather than leaving an internalized guardian that never thinks again");
        }
        finally { controller.Delete(); }
    }
    private static void OwnershipAndPersistence()
    {
        ArachnidOutbreakController controller = Begin();
        Mobile unrelated = new Mobile();
        Bag unrelatedItem = new Bag();
        ArachnidOutbreakController restored = null;
        try
        {
            Check(controller.Phase == ArachnidOutbreakPhase.Broods, "world creation starts three broods");
            Check(Creatures(controller).Count == 3 && Signs(controller).Count == 3, "three owned guardians and signs");
            Mobile[] guardians = Creatures(controller).ToArray();
            foreach (Mobile mobile in guardians)
            {
                ArachnidOutbreakSpider spider = (ArachnidOutbreakSpider)mobile;
                Check(spider.Controller == controller && spider.AttemptId == controller.AttemptId && spider.Guardian, "ownership points to exact attempt");
                Check(spider.HitsMax == 3000 && !spider.Tamable, "initial guardian tuning and no taming");
            }
            Invoke(controller, "RecordCombat", guardians[0], unrelated, 20);
            Check(!State(controller).Engaged, "unattributed NPC fighting does not refresh recovery");
            unrelated.Player = true;
            Invoke(controller, "RecordCombat", guardians[0], unrelated, 20);
            Check(State(controller).Engaged, "attributed combat starts recovery");
            Check(!State(controller).CompleteBrood(0, DateTime.MinValue), "invalid timestamp cannot complete stage");
            Check(controller.ClearedBroods == 0, "failed completion leaves progress intact");
            guardians[0].Delete();
            Check(controller.ClearedBroods == 0, "administrative deletion grants no stage completion");

            // Deliberately contaminate the tracking lists: cleanup must still honor ownership.
            Creatures(controller).Add(unrelated);
            Signs(controller).Add(unrelatedItem);
            MemoryStream bytes = new MemoryStream();
            BinaryFileWriter writer = new BinaryFileWriter(bytes, true);
            controller.Serialize(writer);
            writer.Write(48161);
            writer.Close();
            restored = new ArachnidOutbreakController((Serial)0x400F0000);
            BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
            restored.Deserialize(reader);
            Check(reader.ReadInt() == 48161, "controller positional serializer ends at sentinel");
            reader.Close();
            Check(restored.Phase == controller.Phase && restored.AttemptId == controller.AttemptId
                && State(restored).LastActivity == State(controller).LastActivity, "controller preserves attempt and recovery clock");
            restored.Stop();
            Check(!guardians[1].Deleted && !unrelated.Deleted && !unrelatedItem.Deleted, "foreign references in restored controller are never deleted");
            controller.Stop();
            Check(guardians[1].Deleted && guardians[2].Deleted && Signs(controller).Count == 0, "stop cleans exact owned attempt");
            Check(!unrelated.Deleted && !unrelatedItem.Deleted, "stop preserves unrelated world entities");
            Check(controller.Phase == ArachnidOutbreakPhase.Disabled, "stop closes lifecycle before callbacks");
        }
        finally
        {
            if (restored != null) restored.Delete();
            controller.Delete();
            unrelated.Delete();
            unrelatedItem.Delete();
        }
    }
    private static void DeathAndMissingSite()
    {
        ArachnidOutbreakController controller = Begin();
        try
        {
            ArachnidOutbreakSpider guardian = (ArachnidOutbreakSpider)Creatures(controller)[0];
            guardian.Kill();
            Check(!guardian.Alive && (controller.ClearedBroods & 1) != 0, "real Kill/OnDeath advances the guardian stage");
            int cleared = controller.ClearedBroods;
            Invoke(controller, "CreatureDied", guardian, guardian.Corpse);
            Check(controller.ClearedBroods == cleared, "duplicate death callback cannot advance twice");
            Invoke(controller, "Reconcile", DateTime.UtcNow);
            Check(controller.Phase == ArachnidOutbreakPhase.Cooldown, "missing/unapproved site cancels into cooldown, not victory");
        }
        finally { controller.Delete(); }
    }
    private static void LegacyRewards()
    {
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        ArachnidOutbreakChampion boss = new ArachnidOutbreakChampion(controller);
        Mephitis legacy = new Mephitis();
        PlayerMobile player = new PlayerMobile();
        try
        {
            Check(legacy.AwardLegacyChampionRewards && !boss.AwardLegacyChampionRewards, "default champion policy unchanged; outbreak owns rewards");
            boss.Map = Map.Sosaria;
            player.Player = true;
            player.RawStr = 100;
            player.AddItem(new Backpack());
            player.Map = Map.Sosaria;
            boss.RegisterDamage(3000, player);
            boss.GivePowerScrolls();
            Check(player.Backpack.FindItemsByType(typeof(PowerScroll), true).Length == 0,
                "outbreak does not award power scrolls even with an eligible damage recipient");
            Check(boss.HitsMax == 9000, "initial boss tuning");
        }
        finally { boss.Delete(); legacy.Delete(); controller.Delete(); player.Delete(); }
    }

    private static void ReinforcementBudgets()
    {
        FieldInfo catalog = typeof(ArachnidOutbreakSites).GetField("m_Candidates", BindingFlags.Static | BindingFlags.NonPublic);
        object savedCatalog = catalog.GetValue(null);
        ArachnidOutbreakController controller = new ArachnidOutbreakController();
        try
        {
            ArachnidOutbreakSite site = new ArachnidOutbreakSite("component-budget-fixture", 2202, 532, false);
            catalog.SetValue(null, Array.AsReadOnly(new ArachnidOutbreakSite[] { site }));
            List<Point3D> tiles = new List<Point3D>();
            for (int x = 2150; x <= 2254; ++x)
                for (int y = 480; y <= 584; ++y)
                {
                    Point3D point = new Point3D(x, y, Map.Sosaria.GetAverageZ(x, y));
                    if (Map.Sosaria.CanSpawnMobile(point)) tiles.Add(point);
                }
            Check(tiles.Count > 100, "fixture has real map spawnable points");
            Point3D[] anchors = new Point3D[] { new Point3D(2180, 512, 0), new Point3D(2220, 512, 0), new Point3D(2202, 552, 0) };
            Point3D center = new Point3D(2202, 532, 0);
            State(controller).SetEnabled(true);
            Invoke(controller, "BeginAt", site, new ArachnidOutbreakPlacement(center, anchors, tiles.ToArray()));
            ArachnidOutbreakSpider guardian = (ArachnidOutbreakSpider)Creatures(controller)[0];
            guardian.OnThink();
            Check(Creatures(controller).Count == 7, "guardian spawns four initial defenders");
            guardian.OnThink();
            Check(Creatures(controller).Count == 7, "initial defender batch cannot replay");
            guardian.Hits = 2000;
            guardian.OnThink();
            Check(Creatures(controller).Count == 9, "first health threshold adds two");
            guardian.Hits = 500;
            guardian.OnThink();
            Check(Creatures(controller).Count == 13, "crossing two thresholds adds four, within total budget");
            guardian.Hits = guardian.HitsMax;
            guardian.OnThink();
            guardian.Hits = 100;
            guardian.OnThink();
            Check(Creatures(controller).Count == 13, "healing and repeated damage do not replenish defender budget");

            MemoryStream bytes = new MemoryStream();
            BinaryFileWriter writer = new BinaryFileWriter(bytes, true);
            guardian.Serialize(writer);
            writer.Write(71301);
            writer.Close();
            ArachnidOutbreakSpider restored = new ArachnidOutbreakSpider((Serial)0x000F0000);
            try
            {
                BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
                restored.Deserialize(reader);
                Check(reader.ReadInt() == 71301, "spider positional serializer ends at sentinel");
                reader.Close();
                Check((int)typeof(ArachnidOutbreakSpider).GetField("m_Reinforcements", Private).GetValue(restored) == 15,
                    "spent reinforcement budget survives serialization");
            }
            finally { restored.Delete(); }

            for (int stage = 0; stage < 3; ++stage) State(controller).CompleteBrood(stage, DateTime.UtcNow);
            ArachnidOutbreakChampion boss = new ArachnidOutbreakChampion(controller);
            Creatures(controller).Add(boss);
            boss.Home = center;
            boss.MoveToWorld(center, Map.Sosaria);
            int before = Creatures(controller).Count;
            boss.Hits = 100;
            boss.OnThink();
            Check(Creatures(controller).Count == before + 6, "boss permits six total threshold defenders");
            boss.Hits = boss.HitsMax;
            boss.OnThink();
            boss.Hits = 100;
            boss.OnThink();
            Check(Creatures(controller).Count == before + 6, "boss defender budget cannot replay");
        }
        finally
        {
            controller.Delete();
            catalog.SetValue(null, savedCatalog);
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(args[0]);
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            MapDefinitions.Configure();
            Corpse.Initialize();
            CommandAccess();
            PortalClearance();
            FailedReturnAborts();
            Disabled();
            OwnershipAndPersistence();
            DeathAndMissingSite();
            LegacyRewards();
            ReinforcementBudgets();
            Console.WriteLine("PASS outbreak controller: " + s_Checks + " component assertions; private setup; no saves or listeners.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
