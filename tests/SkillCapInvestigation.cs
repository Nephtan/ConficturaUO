using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Spells.First;

// Characterization, not repaired-behavior tests. No live data, script Initialize
// sweep, timer thread, sockets, World.Load or World.Save is used.
internal static class SkillCapInvestigation
{
    private static int s_Checks;
    private static readonly FieldInfo RandomField = typeof(Utility).GetField("m_Random", BindingFlags.NonPublic | BindingFlags.Static);
    private static readonly Type SettingsType = typeof(PlayerMobile).Assembly.GetType("Server.Misc.MyServerSettings", true);

    private sealed class FixedRandom : Random
    {
        public override double NextDouble() { return 0.5; }
        public override int Next(int maxValue) { return Math.Max(0, maxValue - 1); }
    }

    private sealed class Teacher : BaseCreature
    {
        public Teacher() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4) { }
        public override bool CanTeach { get { return true; } }
    }

    private sealed class FoodCast : CreateFoodSpell
    {
        public FoodCast(Mobile caster) : base(caster, null) { }
        // Exercise the real OnCast/output code without needing a live spell timer.
        public override bool CheckSequence() { return true; }
    }

    private sealed class TinyBackpack : Backpack
    {
        public TinyBackpack() { MaxItems = 1; }
    }

    private sealed class TransferStone : SoulStone
    {
        // The transfer handler is real; bypass only location/account/combat prerequisites.
        protected override bool CheckUse(Mobile from) { return !Deleted; }
    }

    private static void Check(bool condition, string text)
    {
        if (!condition)
            throw new Exception("FAILED: " + text);
        ++s_Checks;
    }

    private static PlayerMobile Player(int total, int target, SkillName name)
    {
        PlayerMobile player = new PlayerMobile();
        player.Name = "isolated skill investigation";
        player.Player = true;
        player.Body = 400;
        player.RawStr = player.RawDex = player.RawInt = 50;
        player.StrLock = player.DexLock = player.IntLock = StatLockType.Locked;
        player.SkillStart = 10000;
        player.Skills.Cap = 10000;
        player.StatCap = 250;
        player.AddItem(new Backpack());
        player.Skills[name].BaseFixedPoint = target;
        int remaining = total - target;
        for (int i = 0; i < player.Skills.Length; ++i)
        {
            Skill skill = player.Skills[i];
            skill.SetLockNoRelay(SkillLock.Locked);
            if (skill.SkillName == name)
                continue;
            int points = Math.Min(1000, Math.Max(0, remaining));
            skill.BaseFixedPoint = points;
            remaining -= points;
        }
        Check(remaining == 0 && player.Skills.Total == total, "fixture real total");
        player.Skills[name].SetLockNoRelay(SkillLock.Up);
        return player;
    }

    private static NetState State(Mobile player)
    {
        // Only OnResponse reads Mobile. Avoid constructing NetState/socket machinery.
        NetState state = (NetState)FormatterServices.GetUninitializedObject(typeof(NetState));
        state.Mobile = player;
        return state;
    }

    private static RelayInfo Reply(int button, params TextRelay[] entries)
    {
        return new RelayInfo(button, new int[0], entries);
    }

    private static Skill Donor(PlayerMobile player, SkillName except)
    {
        for (int i = 0; i < player.Skills.Length; ++i)
            if (player.Skills[i].SkillName != except && player.Skills[i].BaseFixedPoint >= 20)
                return player.Skills[i];
        throw new Exception("No fixture donor");
    }

    private static int RealTotal(Mobile player)
    {
        int total = 0;
        for (int i = 0; i < player.Skills.Length; ++i)
            total += player.Skills[i].BaseFixedPoint;
        return total;
    }

    private static void OneTenthDonor(PlayerMobile player)
    {
        Skill donor = Donor(player, SkillName.Magery);
        int displaced = donor.BaseFixedPoint - 1;
        donor.BaseFixedPoint = 1;
        donor.SetLockNoRelay(SkillLock.Down);
        player.Skills.Throwing.BaseFixedPoint += displaced;
        Check(RealTotal(player) == player.Skills.Total, "tiny donor preserves independent total");
    }

    private static void TestGains()
    {
        foreach (int total in new int[] { 9990, 9999, 10000, 10001 })
        foreach (SkillLock skillLock in new SkillLock[] { SkillLock.Up, SkillLock.Down, SkillLock.Locked })
        foreach (bool accelerated in new bool[] { false, true })
        foreach (bool down in new bool[] { false, true })
        {
            PlayerMobile player = Player(total, 500, SkillName.Magery);
            player.Skills.Magery.SetLockNoRelay(skillLock);
            if (down)
                Donor(player, SkillName.Magery).SetLockNoRelay(SkillLock.Down);
            if (accelerated)
            {
                player.AcceleratedSkill = SkillName.Magery;
                player.AcceleratedStart = DateTime.Now.AddMinutes(5);
            }
            for (int i = 0; i < 20; ++i)
                SkillCheck.Gain(player, player.Skills.Magery);
            Check(player.Skills.Total <= Math.Max(total, player.Skills.Cap), "gain cannot create or worsen total excess");
            Check(RealTotal(player) == player.Skills.Total, "cached total equals independent base-point sum");
            if (skillLock != SkillLock.Up)
                Check(player.Skills.Magery.BaseFixedPoint == 500, "gain respects target lock");
        }
        PlayerMobile low = Player(9900, 99, SkillName.Magery);
        low.Skills.Magery.CapFixedPoint = 100;
        SkillCheck.Gain(low, low.Skills.Magery);
        Check(low.Skills.Magery.BaseFixedPoint == 103 && low.Skills.Total <= low.Skills.Cap,
            "ordinary low-skill gain overshoots individual cap by 0.3 but respects total");
        PlayerMobile acceleratedPlayer = Player(9900, 999, SkillName.Magery);
        acceleratedPlayer.AcceleratedSkill = SkillName.Magery;
        acceleratedPlayer.AcceleratedStart = DateTime.Now.AddMinutes(5);
        SkillCheck.Gain(acceleratedPlayer, acceleratedPlayer.Skills.Magery);
        Check(acceleratedPlayer.Skills.Magery.BaseFixedPoint == 1004 && acceleratedPlayer.Skills.Total <= 10000,
            "accelerated gain overshoots individual cap by 0.4 but respects total");
        PlayerMobile falseFlag = Player(10000, 500, SkillName.Magery);
        falseFlag.Player = false;
        SkillCheck.Gain(falseFlag, falseFlag.Skills.Magery);
        Check(falseFlag.Skills.Total == 10001, "incorrect Player flag bypasses total guard");
        PlayerMobile insufficient = Player(10000, 500, SkillName.Magery);
        OneTenthDonor(insufficient);
        insufficient.AcceleratedSkill = SkillName.Magery;
        insufficient.AcceleratedStart = DateTime.Now.AddMinutes(5);
        SkillCheck.Gain(insufficient, insufficient.Skills.Magery);
        Check(insufficient.Skills.Total == 9999 && insufficient.Skills.Magery.BaseFixedPoint == 500,
            "acceleration can consume small Down loss without enough headroom to grant");
        Console.WriteLine("PASS: gain boundary/lock/acceleration matrix; individual-cap overshoots; conditional Player=false bypass");
    }

    private static void TestReset()
    {
        foreach (int cap in new int[] { 10000, 11000, 13000, 15000, 16000, 18000, 40000, 45000 })
        {
            PlayerMobile player = Player(cap, 500, SkillName.Magery);
            player.Skills.Cap = cap;
            player.Profession = (cap == 13000 || cap == 18000) ? 1 : 0;
            PlayerMobile.SkillVerification(player);
            Check(player.Skills.Total == cap, "supported cap preserves at-cap skills: " + cap);
        }
        PlayerMobile excess = Player(10001, 500, SkillName.Magery);
        excess.StatCap = 260;
        PlayerMobile.SkillVerification(excess);
        Check(excess.Skills.Total == 0 && excess.Profession == 0 && excess.StatCap == 250
            && excess.RawStr == 20 && excess.RawDex == 20 && excess.RawInt == 20, "excess wipes skills and inconsistent stats");
        PlayerMobile unsupported = Player(5000, 500, SkillName.Magery);
        unsupported.Skills.Cap = 12000;
        unsupported.SkillBoost = 2000;
        PlayerMobile.SkillVerification(unsupported);
        Check(unsupported.Skills.Total == 0, "unrecognized cap wipes under-cap skills");
        PlayerMobile staff = Player(10001, 500, SkillName.Magery);
        staff.AccessLevel = AccessLevel.Counselor;
        PlayerMobile.SkillVerification(staff);
        Check(staff.Skills.Total == 10001, "counselor exemption");
        PlayerMobile entry = Player(10001, 500, SkillName.Magery);
        BaseRegion region = new BaseRegion("skill fixture", Map.Sosaria, 10, new Rectangle2D(80, 80, 2, 2));
        region.OnEnter(entry);
        Check(entry.Skills.Total == 0, "actual BaseRegion.OnEnter triggers wipe");
        PlayerMobile guardedEntry = Player(10001, 500, SkillName.Magery);
        GuardedRegion guarded = new GuardedRegion("guarded fixture", Map.Sosaria, 10, new Rectangle2D(80, 80, 2, 2));
        guarded.OnEnter(guardedEntry);
        Check(guardedEntry.Skills.Total == 10001, "GuardedRegion.OnEnter skips base verification; parent entry is separate");
        FieldInfo boostField = SettingsType.GetField("S_SkillBoost", BindingFlags.Static | BindingFlags.NonPublic);
        int originalBoost = (int)boostField.GetValue(null);
        try
        {
            for (int boost = 0; boost <= 10; ++boost)
            {
                boostField.SetValue(null, boost);
                PlayerMobile configured = Player(5000, 500, SkillName.Magery);
                SettingsType.GetMethod("SkillBegin").Invoke(null, new object[] { "default", configured });
                Check(configured.Skills.Cap == 10000 + boost * 1000, "actual configured cap calculation");
                PlayerMobile.SkillVerification(configured);
                bool survives = boost == 0 || boost == 1 || boost == 5 || boost == 6;
                Check(configured.Skills.Total == (survives ? 5000 : 0), "configured default-origin boost " + boost);
            }
        }
        finally { boostField.SetValue(null, originalBoost); }
        Console.WriteLine("PASS: all supported cap tiers, unsupported cap, over-cap region entry, staff exemption, collateral stats");
    }

    private static void TestDirectRewards()
    {
        PlayerMobile target = Player(10000, 500, SkillName.Magery);
        string result = BaseXmlSpawner.SetPropertyValue(null, target, "SKILL,Magery", "50.1");
        Check(target.Skills.Total == 10001, "XML SKILL setter exceeds real total: " + result);
        PlayerMobile cached = Player(10000, 500, SkillName.Magery);
        result = BaseXmlSpawner.SetPropertyValue(null, cached, "Skills.Total", "9999");
        Check(cached.Skills.Total == 9999 && RealTotal(cached) == 10000, "XML reflection can understate cached total: " + result);
        SkillCheck.Gain(cached, cached.Skills.Magery);
        Check(cached.Skills.Total == 10000 && RealTotal(cached) == 10001, "understated total permits real excess through normal gain");
        MemoryStream cacheStream = new MemoryStream();
        cached.Skills.Serialize(new BinaryFileWriter(cacheStream, true));
        Check(cached.Skills.Total == 10001, "serialization exposes previously understated real total");
        PlayerMobile candidate = Player(10000, 500, SkillName.Forensics);
        candidate.Skills.Forensics.SetLockNoRelay(SkillLock.Locked);
        PlayerMobile voter = Player(0, 0, SkillName.Magery);
        CityVotingStone stone = new CityVotingStone();
        stone.Voters = new ArrayList();
        stone.RunnerOne = candidate;
        bool previous = PlayerGovernmentSystem.NeedsForensics;
        try
        {
            PlayerGovernmentSystem.NeedsForensics = false;
            new VotingStoneGump(stone, voter).OnResponse(State(voter), Reply(1));
            Check(candidate.Skills.Total == 10000, "disabled reward cannot raise total");
            stone.Voters.Clear();
            PlayerGovernmentSystem.NeedsForensics = true;
            new VotingStoneGump(stone, voter).OnResponse(State(voter), Reply(1));
            Check(candidate.Skills.Total == 10001, "enabled election reward ignores total cap and Locked skill");
            new VotingStoneGump(stone, voter).OnResponse(State(voter), Reply(1));
            Check(candidate.Skills.Total == 10001, "same voter cannot award a second gain");
        }
        finally { PlayerGovernmentSystem.NeedsForensics = previous; }
        Console.WriteLine("PASS: conditional XML and election total-cap bypasses; disabled reward and repeated voter controls");
    }

    private static void TestTransfers()
    {
        foreach (int total in new int[] { 9990, 9999, 10000 })
        foreach (bool down in new bool[] { false, true })
        {
            PlayerMobile player = Player(total, 500, SkillName.Magery);
            if (down)
                Donor(player, SkillName.Magery).SetLockNoRelay(SkillLock.Down);
            ScrollofTranscendence scroll = new ScrollofTranscendence(SkillName.Magery, 1.0);
            player.Backpack.DropItem(scroll);
            scroll.Use(player);
            Check(player.Skills.Total <= 10000, "transcendence boundary");
            int after = player.Skills.Total;
            scroll.Use(player);
            Check(player.Skills.Total == after, "consumed/rejected scroll cannot bypass total on replay");

            PlayerMobile transfer = Player(total, 500, SkillName.Magery);
            if (down)
                Donor(transfer, SkillName.Magery).SetLockNoRelay(SkillLock.Down);
            TransferStone soul = new TransferStone();
            soul.Skill = SkillName.Magery;
            soul.SkillValue = 51.0;
            Type gumpType = typeof(SoulStone).GetNestedType("ConfirmTransferGump", BindingFlags.NonPublic);
            Gump gump = (Gump)Activator.CreateInstance(gumpType, new object[] { soul, transfer });
            gump.OnResponse(State(transfer), Reply(2));
            Check(transfer.Skills.Total <= 10000, "soulstone boundary");
            after = transfer.Skills.Total;
            gump.OnResponse(State(transfer), Reply(2));
            Check(transfer.Skills.Total <= 10000 && transfer.Skills.Total == after, "soulstone replay");
        }
        PlayerMobile set = Player(9999, 499, SkillName.Magery);
        SkillStone skillStone = new SkillStone(10000, 100);
        set.Backpack.DropItem(skillStone);
        SetSkillsGump setter = new SetSkillsGump(set, skillStone, SkillInfo.Table, null);
        setter.OnResponse(State(set), Reply(1, new TextRelay((int)SkillName.Magery + 1, "50.1")));
        Check(set.Skills.Total == 9999, "skill stone rejects 0.1 excess");
        setter.OnResponse(State(set), Reply(1, new TextRelay((int)SkillName.Magery + 1, "50.0")));
        Check(set.Skills.Total <= 10000, "skill stone at-cap decimal boundary");
        setter.OnResponse(State(set), Reply(1, new TextRelay((int)SkillName.Magery + 1, "51.0")));
        Check(set.Skills.Total <= 10000, "skill stone stale response rechecks total");
        Teacher teacher = new Teacher();
        teacher.Skills.Magery.Base = 120;
        PlayerMobile student = Player(10000, 100, SkillName.Magery);
        int points = 0;
        teacher.CheckTeachSkills(SkillName.Magery, student, 0, ref points, true);
        Check(student.Skills.Total == 10000 && student.Skills.Magery.BaseFixedPoint == 100, "teaching rejects no free points");
        Donor(student, SkillName.Magery).SetLockNoRelay(SkillLock.Down);
        teacher.CheckTeachSkills(SkillName.Magery, student, 0, ref points, true);
        Check(student.Skills.Total <= 10000 && student.Skills.Magery.BaseFixedPoint == 400, "teaching frees enough points");
        PlayerMobile tiny = Player(10000, 500, SkillName.Magery);
        OneTenthDonor(tiny);
        ScrollofTranscendence tooLarge = new ScrollofTranscendence(SkillName.Magery, 1.0);
        tiny.Backpack.DropItem(tooLarge);
        tooLarge.Use(tiny);
        Check(tiny.Skills.Total == 10000 && tiny.Skills.Magery.BaseFixedPoint == 500,
            "scroll rejects nonzero but insufficient Down points");
        TransferStone tinyStone = new TransferStone();
        tinyStone.Skill = SkillName.Magery;
        tinyStone.SkillValue = 51;
        Type transferType = typeof(SoulStone).GetNestedType("ConfirmTransferGump", BindingFlags.NonPublic);
        Gump tinyGump = (Gump)Activator.CreateInstance(transferType, new object[] { tinyStone, tiny });
        tinyGump.OnResponse(State(tiny), Reply(2));
        Check(tiny.Skills.Total == 10000 && tiny.Skills.Magery.BaseFixedPoint == 500,
            "soulstone rejects nonzero but insufficient Down points");
        PlayerMobile partial = Player(10000, 100, SkillName.Magery);
        OneTenthDonor(partial);
        teacher.CheckTeachSkills(SkillName.Magery, partial, 0, ref points, true);
        Check(partial.Skills.Total == 10000 && partial.Skills.Magery.BaseFixedPoint == 101,
            "teacher limits grant to available nonzero Down points");
        PlayerMobile stale = Player(9990, 500, SkillName.Magery);
        TransferStone staleStone = new TransferStone();
        staleStone.Skill = SkillName.Magery;
        staleStone.SkillValue = 51;
        Gump staleGump = (Gump)Activator.CreateInstance(transferType, new object[] { staleStone, stale });
        stale.Skills.Throwing.BaseFixedPoint += 10;
        staleGump.OnResponse(State(stale), Reply(2));
        Check(stale.Skills.Total == 10000 && stale.Skills.Magery.BaseFixedPoint == 500,
            "soulstone rechecks total changed after confirmation opened");
        foreach (SkillLock locked in new SkillLock[] { SkillLock.Down, SkillLock.Locked })
        {
            PlayerMobile lockedPlayer = Player(9990, 500, SkillName.Magery);
            lockedPlayer.Skills.Magery.SetLockNoRelay(locked);
            ScrollofTranscendence lockedScroll = new ScrollofTranscendence(SkillName.Magery, 1.0);
            lockedPlayer.Backpack.DropItem(lockedScroll);
            lockedScroll.Use(lockedPlayer);
            TransferStone lockedStone = new TransferStone();
            lockedStone.Skill = SkillName.Magery;
            lockedStone.SkillValue = 51;
            Gump lockedGump = (Gump)Activator.CreateInstance(transferType, new object[] { lockedStone, lockedPlayer });
            lockedGump.OnResponse(State(lockedPlayer), Reply(2));
            Check(lockedPlayer.Skills.Total == 9990 && lockedPlayer.Skills.Magery.BaseFixedPoint == 500,
                "scroll and soulstone honor non-Up target " + locked);
        }
        Console.WriteLine("PASS: scroll, soulstone, skill stone, teaching boundaries and repeated responses");
    }

    private static void TestPersistenceAndCaps()
    {
        PlayerMobile player = Player(10001, 500, SkillName.Magery);
        MemoryStream stream = new MemoryStream();
        BinaryFileWriter writer = new BinaryFileWriter(stream, true);
        player.Skills.Serialize(writer);
        writer.Flush();
        Check(player.Skills.Total == 10001, "serialization recalculates actual over-cap total");
        stream.Position = 0;
        Skills restored = new Skills(player, new BinaryFileReader(new BinaryReader(stream)));
        Check(restored.Total == 10001 && restored.Cap == 10000, "skill data load preserves excess");
        PlayerMobile lowered = Player(10000, 500, SkillName.Magery);
        lowered.Skills.Cap = 9999;
        Check(lowered.Skills.Total == 10000, "cap setter does not reduce existing skills");
        PlayerMobile.SkillVerification(lowered);
        Check(lowered.Skills.Total == 0, "lowered cap leads to reset");
        PlayerMobile origin = Player(20000, 500, SkillName.Magery);
        origin.SkillStart = 40000;
        origin.Skills.Cap = 40000;
        SettingsType.GetMethod("SkillBegin").Invoke(null, new object[] { "savage", origin });
        Check(origin.Skills.Total == 20000 && origin.Skills.Cap == 11000, "origin reinitialization can lower cap beneath current total");
        Console.WriteLine("PASS: actual Skills serialization/deserialization preserves excess; cap reduction followed by reset");
    }

    private static void TestOfflineAndSpellChecks()
    {
        foreach (int total in new int[] { 9999, 10000 })
        foreach (SkillLock skillLock in new SkillLock[] { SkillLock.Up, SkillLock.Locked })
        {
            PlayerMobile player = Player(total, 100, SkillName.Magery);
            player.Skills.Magery.SetLockNoRelay(skillLock);
            StudyBook book = new StudyBook(SkillName.Magery, 1000);
            book.StartStudy(player);
            typeof(StudyBook).GetField("_StartStudy", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(book, DateTime.Now.AddHours(-2));
            book.EndStudy(player);
            Check(RealTotal(player) <= 10000, "offline training preserves total cap");
            if (skillLock == SkillLock.Locked)
                Check(player.Skills.Magery.BaseFixedPoint == 100, "offline training respects lock");
        }
        PlayerMobile caster = Player(10000, 100, SkillName.Magery);
        caster.Skills.Psychology.SetLockNoRelay(SkillLock.Up);
        caster.Skills.Meditation.SetLockNoRelay(SkillLock.Up);
        caster.Skills.Focus.SetLockNoRelay(SkillLock.Up);
        caster.Mana = 0;
        MethodInfo manaRegen = typeof(RegenRates).GetMethod("Mobile_ManaRegenRate", BindingFlags.Static | BindingFlags.NonPublic);
        for (int i = 0; i < 200; ++i)
        {
            new CreateFoodSpell(caster, null).CheckFizzle();
            manaRegen.Invoke(null, new object[] { caster });
        }
        Check(RealTotal(caster) == 10000, "real Create Food skill checks and mana regeneration cannot cross total cap");
        Console.WriteLine("PASS: actual offline training, Create Food CheckFizzle and mana-regeneration skill checks at cap");
    }

    private static void TestFoodAndQueue()
    {
        PlayerMobile player = Player(10000, 500, SkillName.Magery);
        player.Backpack.Delete();
        TinyBackpack backpack = new TinyBackpack();
        player.AddItem(backpack);
        backpack.DropItem(new WaterFlask());
        player.Map = Map.Sosaria;
        player.Location = new Point3D(80, 80, 0);
        int before = World.Items.Count;
        for (int i = 0; i < 25; ++i)
            new FoodCast(player).OnCast();
        int dropped = 0;
        int water = 0;
        foreach (Item item in World.Items.Values)
        {
            if (item.Parent == null && item.Map == player.Map && item.Location == player.Location)
            {
                ++dropped;
                if (item is WaterFlask)
                    ++water;
            }
        }
        Check(dropped == 50 && water == 25 && World.Items.Count >= before + 50,
            "25 output casts create 50 separate ground objects on one tile");
        Check(player.Skills.Total == 10000, "food output does not mutate real skills");
        SendQueue queue = new SendQueue();
        byte[] buffer = new byte[1024];
        bool exceeded = false;
        int accepted = 0;
        try
        {
            for (int i = 0; i < 200; ++i)
            {
                queue.Enqueue(buffer, buffer.Length);
                ++accepted;
            }
        }
        catch (CapacityExceededException) { exceeded = true; }
        Check(exceeded && accepted == 96, "undrained production SendQueue rejects above 96 KiB");
        Console.WriteLine("PASS: full-pack food ground output and 96 KiB queue guard (no connected client / no live pile)");
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
            SkillCheck.Initialize(); // Only registers the real skill-check delegates.
            BaseXmlSpawner.Initialize(); // Only installs its two protected-property entries.
            RandomField.SetValue(null, new FixedRandom());
            SkillCapMutationInventory.Export(Directory.GetCurrentDirectory());
            TestGains();
            TestReset();
            TestDirectRewards();
            TestTransfers();
            TestPersistenceAndCaps();
            TestOfflineAndSpellChecks();
            TestFoodAndQueue();
            Console.WriteLine("PASS: " + s_Checks + " assertions; isolated production methods; no saves loaded or listeners started.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
