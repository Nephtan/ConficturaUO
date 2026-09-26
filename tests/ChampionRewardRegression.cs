using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

// Runs against freshly compiled scripts via scripts/Test-ChampionRewards.ps1.
// No saves, script initialization, timer thread, listeners, or clients are started.
internal static class ChampionRewardRegression
{
    private static int s_Checks;
    private static readonly FieldInfo RandomField = typeof(Utility).GetField("m_Random", BindingFlags.Static | BindingFlags.NonPublic);

    private sealed class RewardRandom : Random
    {
        private readonly double m_Roll;
        private readonly int m_Choice;

        public RewardRandom(double roll, int choice)
        {
            m_Roll = roll;
            m_Choice = choice;
        }

        public override double NextDouble() { return m_Roll; }
        public override int Next(int maxValue) { return maxValue == 0 ? 0 : Math.Min(m_Choice, maxValue - 1); }
    }

    private sealed class Champion : BaseChampion
    {
        public Champion() : base(AIType.AI_Melee)
        {
            Name = "reward fixture";
            SetStr(100);
            SetHits(3000);
        }

        public Champion(Serial serial) : base(serial) { }
        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }
        public override Type[] UniqueList { get { return new Type[0]; } }
        public override Type[] SharedList { get { return new Type[0]; } }
        public override Type[] DecorativeList { get { return new Type[0]; } }
        public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[0]; } }
        public override bool NoGoodies { get { return true; } }
        public override void GenerateLoot() { }
    }

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("FAILED: " + description);
        ++s_Checks;
    }

    private static PlayerMobile Player(Map map, bool alien, int equipmentLuck)
    {
        PlayerMobile player = new PlayerMobile();
        player.Name = "champion reward tester";
        player.Player = true;
        player.Body = 400;
        player.RawStr = 100;
        player.SkillStart = alien ? 40000 : 10000;
        player.AddItem(new Backpack());
        if (equipmentLuck > 0)
        {
            GoldRing ring = new GoldRing();
            ring.Attributes.Luck = equipmentLuck;
            player.AddItem(ring);
        }
        player.MoveToWorld(new Point3D(80, 80, 0), map);
        Check(player.Luck == (alien ? 0 : equipmentLuck), "actual PlayerMobile Luck and alien rule");
        return player;
    }

    private static Champion Boss(Map map)
    {
        Champion boss = new Champion();
        boss.Map = map;
        return boss;
    }

    private static Item[] Items(Container container, Type type)
    {
        return container.FindItemsByType(type, true);
    }

    private static void TestBossMaps(Map[] maps)
    {
        foreach (Map map in maps)
        {
            for (int profile = 0; profile < 4; ++profile)
            {
                PlayerMobile player = Player(map, profile >= 2, profile % 2 == 0 ? 0 : 2000);
                Champion boss = Boss(map);
                boss.RegisterDamage(3000, player);
                Check(boss.OnBeforeDeath(), "boss death continues");
                Item[] scrolls = Items(player.Backpack, typeof(PowerScroll));
                Check(scrolls.Length == 6, map.Name + " gives six scrolls, profile " + profile);
                foreach (PowerScroll scroll in scrolls)
                {
                    Check(scroll.Value == 110 || scroll.Value == 115 || scroll.Value == 120, "unchanged boss cap tiers");
                    Check(scroll.Skill != SkillName.Blacksmith && scroll.Skill != SkillName.Tailoring, "non-craft scroll pool");
                }
                Backpack corpse = new Backpack();
                boss.OnDeath(corpse);
                Item[] skulls = Items(player.Backpack, typeof(ChampionSkull));
                Check(skulls.Length == 1 && ((ChampionSkull)skulls[0]).Type == ChampionSkullType.Power,
                    map.Name + " gives the boss skull, profile " + profile);
                Check(Items(corpse, typeof(ChampionSkull)).Length == 0, "credited skull is not duplicated on corpse");
            }
            Console.WriteLine("PASS: boss scrolls/skull on " + map.Name + ", normal/alien with 0/2000 equipment Luck");
        }
    }

    private static void TestRightsAndDelivery()
    {
        Map map = Map.Sosaria;
        PlayerMobile strong = Player(map, false, 0);
        PlayerMobile weak = Player(map, false, 0);
        Champion boss = Boss(map);
        boss.RegisterDamage(1600, strong);
        boss.RegisterDamage(1, weak);
        boss.GivePowerScrolls();
        boss.OnDeath(new Backpack());
        Check(Items(strong.Backpack, typeof(PowerScroll)).Length == 6, "qualified damage earns scrolls");
        Check(Items(weak.Backpack, typeof(PowerScroll)).Length == 0 && Items(weak.Backpack, typeof(ChampionSkull)).Length == 0,
            "one hit below the rights threshold earns neither scrolls nor skull");

        PlayerMobile first = Player(map, false, 0);
        PlayerMobile second = Player(map, false, 0);
        boss = Boss(map);
        boss.RegisterDamage(1500, first);
        boss.RegisterDamage(1500, second);
        boss.GivePowerScrolls();
        boss.OnDeath(new Backpack());
        Check(Items(first.Backpack, typeof(PowerScroll)).Length == 3 && Items(second.Backpack, typeof(PowerScroll)).Length == 3,
            "six scrolls total, shared between two eligible players");
        Check(Items(first.Backpack, typeof(ChampionSkull)).Length + Items(second.Backpack, typeof(ChampionSkull)).Length == 1,
            "one skull total for a group");

        PlayerMobile owner = Player(map, false, 0);
        Champion pet = Boss(map);
        pet.Controlled = true;
        pet.ControlMaster = owner;
        boss = Boss(map);
        boss.RegisterDamage(3000, pet);
        boss.GivePowerScrolls();
        Check(Items(owner.Backpack, typeof(PowerScroll)).Length == 6, "pet damage credits the player master");

        PlayerMobile expired = Player(map, false, 0);
        boss = Boss(map);
        boss.RegisterDamage(3000, expired).LastDamage = DateTime.Now - TimeSpan.FromMinutes(3);
        boss.GivePowerScrolls();
        Backpack unclaimed = new Backpack();
        boss.OnDeath(unclaimed);
        Check(Items(expired.Backpack, typeof(PowerScroll)).Length == 0, "expired damage does not qualify");
        Check(Items(unclaimed, typeof(ChampionSkull)).Length == 1, "no eligible player puts skull on boss corpse");

        PlayerMobile ghost = Player(map, false, 0);
        ghost.Body = 402;
        ghost.Corpse = new Backpack();
        Check(!ghost.Alive, "fixture uses a dead player");
        boss = Boss(map);
        boss.RegisterDamage(3000, ghost);
        boss.GivePowerScrolls();
        Check(Items(ghost.Corpse, typeof(PowerScroll)).Length == 6 && Items(ghost.Backpack, typeof(PowerScroll)).Length == 0,
            "dead player's scrolls go to the existing corpse");
        ghost.Corpse.Delete();
        boss.GivePowerScrolls();
        Check(Items(ghost.Backpack, typeof(PowerScroll)).Length == 6, "missing corpse falls back to backpack");

        PlayerMobile full = Player(map, false, 0);
        for (int i = 0; i < full.Backpack.MaxItems; ++i)
            full.Backpack.DropItem(new Item(1));
        PowerScroll overflow = new PowerScroll(SkillName.Magery, 120);
        BaseChampion.GivePowerScrollTo(full, overflow);
        Check(overflow.Parent == null && overflow.Map == full.Map && overflow.Location == full.Location,
            "full backpack preserves the existing ground fallback");

        PlayerMobile excluded = Player(map, false, 0);
        foreach (Map invalid in new Map[] { Map.Internal, null })
        {
            boss = Boss(invalid);
            boss.RegisterDamage(3000, excluded);
            boss.GivePowerScrolls();
            Check(Items(excluded.Backpack, typeof(PowerScroll)).Length == 0, "internal/null maps do not award scrolls");
        }
        boss = Boss(Map.Internal);
        Backpack internalCorpse = new Backpack();
        boss.OnDeath(internalCorpse);
        Check(Items(internalCorpse, typeof(ChampionSkull)).Length == 0, "internal map does not create a skull");

        boss = Boss(map);
        boss.NoKillAwards = true;
        boss.RegisterDamage(3000, excluded);
        boss.OnBeforeDeath();
        Check(Items(excluded.Backpack, typeof(PowerScroll)).Length == 0, "NoKillAwards still suppresses boss scrolls");
        Console.WriteLine("PASS: rights, shared quantity, pet credit, expiry, ghost/full-pack delivery, and excluded maps");
    }

    private static void TestWave(Map map, double roll, int choice, bool petKill, Type expectedType)
    {
        PlayerMobile player = Player(map, false, 0);
        ChampionSpawn spawn = new ChampionSpawn();
        spawn.Map = map;
        spawn.ExpireTime = DateTime.Now + TimeSpan.FromHours(1);
        typeof(ChampionSpawn).GetField("m_Active", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(spawn, true);
        List<Mobile> creatures = (List<Mobile>)typeof(ChampionSpawn)
            .GetField("m_Creatures", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spawn);
        // Keep the real OnSlice path from spawning a new wave during this one-kill check.
        Mobile placeholder = new Mobile();
        for (int i = 0; i < 200; ++i)
            creatures.Add(placeholder);
        Champion killed = Boss(map);
        Champion pet = null;
        if (petKill)
        {
            pet = Boss(map);
            pet.Controlled = true;
            pet.ControlMaster = player;
        }
        killed.RegisterDamage(100, petKill ? (Mobile)pet : player);
        killed.Delete();
        creatures.Add(killed);
        Random saved = (Random)RandomField.GetValue(null);
        try
        {
            RandomField.SetValue(null, new RewardRandom(roll, choice));
            spawn.OnSlice();
        }
        finally
        {
            RandomField.SetValue(null, saved);
        }
        Item[] scrolls = Items(player.Backpack, typeof(SpecialScroll));
        if (expectedType == null)
            Check(scrolls.Length == 0, map.Name + " does not award outside the wave chance/expansion/map gate");
        else
        {
            Check(scrolls.Length == 1 && scrolls[0].GetType() == expectedType, map.Name + " awards exactly one " + expectedType.Name);
            SpecialScroll scroll = (SpecialScroll)scrolls[0];
            Check(expectedType == typeof(PowerScroll) ? scroll.Value == 105 : scroll.Value >= 0.6 && scroll.Value <= 1.0,
                "Lodor wave magnitude is preserved");
        }
        spawn.Delete();
        placeholder.Delete();
        if (pet != null)
            pet.Delete();
    }

    private static void TestWaveMaps(Map[] maps)
    {
        foreach (Map map in maps)
        {
            TestWave(map, 0.0005, 0, false, typeof(ScrollofTranscendence));
            TestWave(map, 0.0005, 24, false, typeof(ScrollofTranscendence));
            TestWave(map, 0.0005, 25, false, typeof(PowerScroll));
            TestWave(map, 0.0005, 25, true, typeof(PowerScroll));
            TestWave(map, 0.001, 0, false, null);
            TestWave(map, 0.00125, 0, false, null);
            Console.WriteLine("PASS: " + map.Name + " wave branches, pet master, 0.1% boundary, and no legacy extra roll");
        }
        TestWave(Map.Internal, 0.0005, 0, false, null);
        Core.Expansion = Expansion.SE;
        TestWave(Map.Sosaria, 0.0005, 0, false, null);
        Core.Expansion = Expansion.SA;
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            typeof(World).GetField("m_Mobiles", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            Map[] maps = new Map[] { Map.Lodor, Map.Sosaria, Map.Underworld, Map.SerpentIsland, Map.IslesDread, Map.SavagedEmpire, Map.Atlantis };
            TestBossMaps(maps);
            TestRightsAndDelivery();
            TestWaveMaps(maps);
            Console.WriteLine("PASS: " + s_Checks + " assertions; no world saves loaded or listeners started.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
