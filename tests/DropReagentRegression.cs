using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

// Exercise the production loot helper without loading saves or starting listeners/timer threads.
internal static class DropReagentRegression
{
    private static int s_Checks;

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("FAILED: " + description);
        ++s_Checks;
    }

    private static void DropReagent(BaseCreature creature)
    {
        Type helper = typeof(BaseCreature).Assembly.GetType("Server.Misc.IntelligentAction", true);
        helper.GetMethod("DropReagent", BindingFlags.Public | BindingFlags.Static)
            .Invoke(null, new object[] { null, creature });
    }

    private static BaseCreature Creature(string name)
    {
        BaseCreature creature = new BaseCreature(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4);
        creature.Name = name;
        creature.Fame = 0;
        return creature;
    }

    private static int Count(BaseCreature creature, Type type)
    {
        int amount = 0;
        if (creature.Backpack != null)
        {
            foreach (Item item in creature.Backpack.Items)
            {
                if (item.GetType() == type)
                    amount += item.Amount;
            }
        }
        return amount;
    }

    private static int ItemCount(BaseCreature creature)
    {
        return creature.Backpack == null ? 0 : creature.Backpack.Items.Count;
    }

    private static void TestUnnamed()
    {
        BaseCreature creature = Creature(null);
        DropReagent(creature);
        Check(creature.Name == null, "unnamed creature remains unnamed");
        Check(ItemCount(creature) == 0, "null name adds no name-based loot");
        DropReagent(creature);
        Check(ItemCount(creature) == 0, "repeating the null-name call is safe");
        creature.Delete();

        creature = Creature(String.Empty);
        DropReagent(creature);
        Check(ItemCount(creature) == 0, "empty name adds no name-based loot");
        creature.Delete();
        Console.WriteLine("PASS: null and empty names, repeated call, no name mutation");
    }

    private static void TestNames()
    {
        string[] names = new string[] { "a beetle", "a werewolf", "a wolf man", "a frog", "a toad", "a spider" };
        foreach (string name in names)
        {
            BaseCreature creature = Creature(name);
            DropReagent(creature);
            Check(creature.Name == name, "name preserved: " + name);
            Check(ItemCount(creature) == 1, "one reagent kind: " + name);
            if (name == "a beetle")
                Check(Count(creature, typeof(BeetleShell)) == 1, "beetle shell quantity");
            else if (name == "a werewolf" || name == "a wolf man")
                Check(Count(creature, typeof(WerewolfClaw)) >= 1 && Count(creature, typeof(WerewolfClaw)) <= 2,
                    "werewolf claw quantity: " + name);
            else if (name == "a frog" || name == "a toad")
                Check((Count(creature, typeof(EyeOfToad)) >= 1 && Count(creature, typeof(EyeOfToad)) <= 2)
                    || Count(creature, typeof(DriedToad)) == 1, "frog/toad random alternatives: " + name);
            else
                Check(Count(creature, typeof(SilverWidow)) == 1, "spider reagent quantity");
            creature.Delete();
        }

        BaseCreature combined = Creature("beetle werewolf frog spider");
        DropReagent(combined);
        Check(ItemCount(combined) == 4, "matching name groups remain independent");
        combined.Delete();

        foreach (string name in new string[] { "a creature", "BEETLE WEREWOLF WOLF MAN FROG TOAD SPIDER" })
        {
            BaseCreature creature = Creature(name);
            DropReagent(creature);
            Check(ItemCount(creature) == 0, "existing case-sensitive matching preserved: " + name);
            creature.Delete();
        }
        Console.WriteLine("PASS: all name-based loot groups, quantities, combined matches and case sensitivity");
    }

    private static void TestTypeLoot()
    {
        // Skeleton loot runs before the old fault; blood and plant loot run after it.
        BaseCreature[] creatures = new BaseCreature[] { new Skeleton(), new BloodElemental(), new Corpser(), new Pixie() };
        foreach (BaseCreature creature in creatures)
        {
            if (creature.Backpack != null)
                creature.Backpack.Delete();
            creature.Name = null;
            creature.Fame = 21000;
            DropReagent(creature);
            Check(creature.Name == null, "type-based loot preserves null name: " + creature.GetType().Name);
            Check(ItemCount(creature) == 1, "type-based loot survives null name: " + creature.GetType().Name);
            Item reagent = creature.Backpack.Items[0];
            if (creature is Skeleton)
                Check(reagent is GraveDust && reagent.Amount >= 6 && reagent.Amount <= 18,
                    "undead slayer loot and fame quantity preserved");
            else if (creature is BloodElemental)
                Check(reagent is DaemonBlood && reagent.Amount >= 6 && reagent.Amount <= 18,
                    "blood elemental loot after name checks preserved");
            else if (creature is Corpser)
                Check((reagent is MandrakeRoot || reagent is Nightshade || reagent is SwampBerries
                    || reagent is RedLotus || reagent is Ginseng || reagent is Garlic || reagent is BitterRoot
                    || reagent is VioletFungus || reagent is BloodRose || reagent is Wolfsbane)
                    && reagent.Amount >= 6 && reagent.Amount <= 18, "plant slayer loot after name checks preserved");
            else
                Check((reagent is FairyEgg && reagent.Amount >= 1 && reagent.Amount <= 2)
                    || (reagent is PixieSkull && reagent.Amount == 1)
                    || (reagent is ButterflyWings && reagent.Amount >= 1 && reagent.Amount <= 2),
                    "pixie loot between name checks preserved");
            creature.Delete();
        }
        Console.WriteLine("PASS: unnamed undead, blood elemental, plant and pixie retain type-based loot");
    }

    private static void TestTriton()
    {
        BaseCreature king = new Server.Custom.Confictura.Mobiles.Triton();
        Check(king.Name == "Triton" && king.Title == "the Cursed King", "current Triton constructor assigns identity");
        if (king.Backpack != null)
            king.Backpack.Delete();
        DropReagent(king);
        Check(ItemCount(king) == 0, "ordinary Triton name has no matching reagent rules");
        king.Name = null;
        DropReagent(king);
        Check(king.Name == null && ItemCount(king) == 0, "Triton also tolerates a missing saved or edited name");
        king.Delete();
        Console.WriteLine("PASS: current Triton identity and reagent helper with normal and null names");
    }

    private static void TestBattleLogging()
    {
        Check(Path.GetFullPath(".").TrimEnd('\\', '/') == Core.BaseDirectory.TrimEnd('\\', '/'),
            "battle log writes stay beside the isolated test executable");
        string path = Path.Combine("Info", "battles.txt");
        if (File.Exists(path))
            throw new Exception("Use a fresh regression directory for battle logging.");
        Type logger = typeof(BaseCreature).Assembly.GetType("Server.Misc.LoggingFunctions", true);
        MethodInfo log = logger.GetMethod("LogBattles", BindingFlags.Public | BindingFlags.Static);
        PlayerMobile player = new PlayerMobile();
        player.Name = "regression mage";
        player.Title = "the test mage";
        player.PublicMyRunUO = true;
        BaseCreature creature = Creature(null);
        log.Invoke(null, new object[] { player, creature });
        Check(!File.Exists(path), "unnamed low-fame creature remains excluded from battle reports");
        creature.Fame = 3500;
        creature.Title = "the Cursed King";
        log.Invoke(null, new object[] { player, creature });
        Check(File.ReadAllLines(path)[0].Contains("has slain an unnamed creature the Cursed King#"),
            "public log uses an unnamed-creature fallback and retains the title");
        Check(creature.Name == null, "battle logging does not mutate the creature name");
        creature.Name = "a troll [fixture suffix]";
        creature.Title = null;
        log.Invoke(null, new object[] { player, creature });
        Check(File.ReadAllLines(path)[0].Contains("has slain a troll#"), "named log still strips bracket suffixes and trailing space");
        player.PublicMyRunUO = false;
        creature.Name = null;
        log.Invoke(null, new object[] { player, creature });
        string[] lines = File.ReadAllLines(path);
        Check(lines.Length == 3 && lines[0].Contains("has slain ") && !lines[0].Contains("unnamed creature")
            && !lines[0].Contains("Cursed King"), "private battle log retains anonymous opponent wording");
        log.Invoke(null, new object[] { null, creature });
        log.Invoke(null, new object[] { player, null });
        Check(File.ReadAllLines(path).Length == 3, "existing null-actor and null-opponent guards remain safe");
        creature.Delete();
        player.Delete();
        Console.WriteLine("PASS: follow-on battle logging with null names, fame filter, public/private text and named suffixes");
    }

    public static int Main(string[] args)
    {
        try
        {
            Core.Assembly = typeof(Core).Assembly;
            Core.Expansion = Expansion.SA;
            Core.DataDirectories.Add(Path.GetFullPath(args[0]));
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
            typeof(World).GetField("m_Mobiles", flags).SetValue(null, new Dictionary<Serial, Mobile>());
            typeof(World).GetField("m_Items", flags).SetValue(null, new Dictionary<Serial, Item>());
            MapDefinitions.Configure();
            TestUnnamed();
            TestNames();
            TestTypeLoot();
            TestTriton();
            TestBattleLogging();
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
