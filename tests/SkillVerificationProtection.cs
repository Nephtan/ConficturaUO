using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Regions;

// Compiled outside Data/Scripts; never install this fixture on a shard.
internal static class SkillVerificationProtection
{
    private static int checks;
    private static readonly FieldInfo Throttle = typeof(PlayerMobile).GetField("m_NextSkillVerificationDiagnostic", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly FieldInfo Sink = typeof(PlayerMobile).GetField("m_SkillVerificationDiagnosticWriter", BindingFlags.Static | BindingFlags.NonPublic);
    private static readonly FieldInfo Pending = typeof(PlayerMobile).GetField("m_SkillVerificationDiagnosticPending", BindingFlags.Static | BindingFlags.NonPublic);

    private static void WaitForReport()
    {
        Stopwatch watch = Stopwatch.StartNew();
        while ((int)Pending.GetValue(null) != 0)
        {
            if (watch.ElapsedMilliseconds > 5000) throw new Exception("Diagnostic worker did not complete");
            Thread.Sleep(1);
        }
    }

    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception("FAIL: " + message);
        ++checks;
    }

    private static PlayerMobile Player(int total, int cap)
    {
        PlayerMobile p = (PlayerMobile)typeof(SkillCapInvestigation).GetMethod("Player", BindingFlags.Static | BindingFlags.NonPublic)
            .Invoke(null, new object[] { total, Math.Min(total, 500), SkillName.Magery });
        p.Skills.Cap = cap;
        p.StatCap = 260;
        p.RawStr = 81; p.RawDex = 72; p.RawInt = 63;
        p.Profession = 7;
        p.CharacterKeys = "0#0#0#0#0#0#0#0#0#0#0#0#0#";
        return p;
    }

    private static string Snapshot(PlayerMobile p)
    {
        StringBuilder s = new StringBuilder();
        s.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|", p.Skills.Total, p.Skills.Cap,
            p.SkillStart, p.SkillBoost, p.SkillEther, p.Profession, p.StatCap, p.RawStr, p.RawDex, p.RawInt, p.Player, p.CharacterKeys);
        for (int i = 0; i < p.Skills.Length; ++i)
            s.AppendFormat("{0}:{1}:{2};", p.Skills[i].BaseFixedPoint, p.Skills[i].CapFixedPoint, p.Skills[i].Lock);
        return s.ToString();
    }

    private static string Observe(PlayerMobile p)
    {
        WaitForReport();
        object old = Sink.GetValue(null);
        StringWriter output = new StringWriter();
        try { Sink.SetValue(null, new Action<string>(output.WriteLine)); PlayerMobile.SkillVerification(p); WaitForReport(); }
        finally { Sink.SetValue(null, old); }
        return output.ToString();
    }

    private static void Preserve(PlayerMobile p, string label)
    {
        string before = Snapshot(p);
        Observe(p);
        Check(before == Snapshot(p), label);
    }

    private static void TestMatrix()
    {
        foreach (int start in new int[] { 10000, 11000, 13000, 40000 })
        foreach (int ether in new int[] { 0, 5000 })
        for (int boost = 0; boost <= 10000; boost += 1000)
        foreach (int offset in new int[] { -1, 0, 1 })
        {
            int cap = start + boost + ether;
            PlayerMobile p = Player(cap + offset, cap);
            p.SkillStart = start; p.SkillBoost = boost; p.SkillEther = ether;
            p.Profession = start == 13000 ? 1 : 7;
            Preserve(p, "origin/boost/ether cap matrix " + cap + "/" + offset);
        }
        PlayerMobile original = Player(5000, 12000);
        original.SkillBoost = 2000;
        string before = Snapshot(original);
        string diagnostic = Observe(original);
        Check(before == Snapshot(original), "original 500/1200 retains every base, cap, lock, stat, component and profession");
        Check(diagnostic.Contains("legacy_cap_unrecognized") && diagnostic.Contains("real=5000 cached=5000 cap=12000")
            && !diagnostic.Contains("real_total_exceeds_cap"), "unsupported legacy cap is distinct from real excess");
        File.WriteAllText("original-500-1200.txt", "Before=" + before + "\nAfter=" + Snapshot(original) + "\n" + diagnostic);
        foreach (int cap in new int[] { 13000, 18000 })
        foreach (int profession in new int[] { 0, 1, 7 })
        foreach (bool virtue in new bool[] { false, true })
        {
            PlayerMobile p = Player(5000, cap); p.Profession = profession;
            if (virtue) p.CharacterKeys = "0#0#0#0#0#0#0#0#0#1#0#0#0#";
            before = Snapshot(p); diagnostic = Observe(p);
            Check(before == Snapshot(p), "profession/virtue retained");
            Check(diagnostic.Contains("legacy_profession_mismatch") == (profession != 1 && !virtue), "profession reason matches legacy predicate");
        }
        foreach (string keys in new string[] { null, "", "bad#record", "0#0#0#0#0#0#0#0#0#1" })
        {
            PlayerMobile p = Player(5000, 13000); p.CharacterKeys = keys;
            Preserve(p, "keys are never normalized by diagnostic");
        }
        foreach (int cached in new int[] { -1, 4999, 12001, Int32.MaxValue })
        {
            PlayerMobile p = Player(5000, 12000); p.Skills.Total = cached;
            before = Snapshot(p); diagnostic = Observe(p);
            Check(before == Snapshot(p) && diagnostic.Contains("cached_total_mismatch"), "cache mismatch retained and diagnosed");
            Check(!diagnostic.Contains("real_total_exceeds_cap"), "high cache is not labeled real excess");
        }
        PlayerMobile excess = Player(10001, 10000); excess.Skills.Total = 9999; excess.Player = false;
        before = Snapshot(excess); diagnostic = Observe(excess);
        Check(before == Snapshot(excess) && diagnostic.Contains("real_total_exceeds_cap") && diagnostic.Contains("cached_total_mismatch")
            && diagnostic.Contains("player_flag_false"), "low cache/false flag cannot conceal real excess diagnostic");
        PlayerMobile overflow = Player(5000, 10000); overflow.SkillStart = overflow.SkillBoost = Int32.MaxValue;
        Check(Observe(overflow).Contains("cap_components_mismatch"), "component sum uses wide arithmetic");
        foreach (int cap in new int[] { 0, -1, 12345 }) Preserve(Player(5000, cap), "invalid caps never correct progress");
        Console.WriteLine("PASS: configured origin/boost/Ether boundaries, original 500/1200, profession/virtue, cache and flag diagnostics.");
    }

    private sealed class FailingWriter : TextWriter
    {
        public int Attempts;
        public override Encoding Encoding { get { return Encoding.UTF8; } }
        public override void WriteLine(string value) { ++Attempts; throw new IOException("Injected diagnostic failure"); }
    }

    private static void TestFailureAndThrottle()
    {
        PlayerMobile p = Player(5000, 12000);
        string before = Snapshot(p);
        Check(Observe(p).Contains("[SkillVerification]"), "initial diagnostic");
        for (int i = 0; i < 100; ++i) Check(Observe(p).Length == 0, "repeat diagnostic suppressed");
        Throttle.SetValue(p, DateTime.UtcNow.AddSeconds(-1));
        Check(Observe(p).Contains("[SkillVerification]"), "diagnostic resumes after throttle");
        Throttle.SetValue(p, DateTime.MinValue);
        FailingWriter writer = new FailingWriter(); object old = Sink.GetValue(null);
        try { Sink.SetValue(null, new Action<string>(writer.WriteLine)); for (int i = 0; i < 100; ++i) PlayerMobile.SkillVerification(p); WaitForReport(); }
        finally { Sink.SetValue(null, old); }
        Check(writer.Attempts == 1 && before == Snapshot(p), "failed logger is throttled, no exception escapes or progress changes");
        FieldInfo array = typeof(Skills).GetField("m_Skills", BindingFlags.Instance | BindingFlags.NonPublic);
        object saved = array.GetValue(p.Skills);
        Throttle.SetValue(p, DateTime.MinValue);
        string error;
        try { array.SetValue(p.Skills, null); error = Observe(p); }
        finally { array.SetValue(p.Skills, saved); }
        Check(error.Contains("validation_exception") && before == Snapshot(p), "validation exception is contained and reported");
        Throttle.SetValue(p, DateTime.MinValue); writer = new FailingWriter();
        try { array.SetValue(p.Skills, null); Sink.SetValue(null, new Action<string>(writer.WriteLine)); PlayerMobile.SkillVerification(p); PlayerMobile.SkillVerification(p); WaitForReport(); }
        finally { array.SetValue(p.Skills, saved); Sink.SetValue(null, old); }
        Check(writer.Attempts == 1 && before == Snapshot(p), "combined validation and output failures preserve state");
        ManualResetEvent entered = new ManualResetEvent(false); ManualResetEvent release = new ManualResetEvent(false);
        int writes = 0;
        try
        {
            Sink.SetValue(null, new Action<string>(delegate(string text) { Interlocked.Increment(ref writes); entered.Set(); release.WaitOne(); }));
            Throttle.SetValue(p, DateTime.MinValue);
            Stopwatch watch = Stopwatch.StartNew(); PlayerMobile.SkillVerification(p);
            Check(watch.ElapsedMilliseconds < 1000 && entered.WaitOne(5000), "slow writer runs outside region-entry thread");
            for (int i = 0; i < 100; ++i)
            {
                PlayerMobile other = Player(5000, 12000); string otherBefore = Snapshot(other);
                PlayerMobile.SkillVerification(other);
                Check(otherBefore == Snapshot(other), "busy report slot does not affect another character");
            }
            Check(writes == 1 && (int)Pending.GetValue(null) == 1, "one global outstanding report despite distinct characters");
            Check(before == Snapshot(p), "blocked output preserves original character");
        }
        finally { release.Set(); WaitForReport(); Sink.SetValue(null, old); entered.Close(); release.Close(); }
        string log = Path.Combine(Core.BaseDirectory, "Logs/SkillVerification.log");
        PlayerMobile disk = Player(5000, 12000); PlayerMobile.SkillVerification(disk); WaitForReport();
        Check(File.ReadAllText(log).Contains("serial=" + disk.Serial), "real dedicated file sink records snapshot");
        PlayerMobile locked = Player(5000, 12000); string lockedBefore = Snapshot(locked);
        using (FileStream file = File.Open(log, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            PlayerMobile.SkillVerification(locked); WaitForReport();
            Check(lockedBefore == Snapshot(locked), "exclusive log lock cannot affect progress or escape worker");
        }
        Throttle.SetValue(locked, DateTime.MinValue); PlayerMobile.SkillVerification(locked); WaitForReport();
        Check(File.ReadAllText(log).Contains("serial=" + locked.Serial), "file output recovers after lock released and throttle expires");
        PlayerMobile staff = Player(10001, 10000); staff.AccessLevel = AccessLevel.Counselor;
        Check(Observe(staff).Length == 0, "staff exemption retained");
        PlayerMobile deleted = Player(5000, 12000); deleted.Delete();
        Check(Observe(deleted).Length == 0 && Observe(null).Length == 0, "deleted/null guards");
        Console.WriteLine("PASS: validation exceptions, failed output, throttled retry, staff/null/deleted guards.");
    }

    private static void TestTravelAndPersistence()
    {
        BaseRegion a = new BaseRegion("FIX01-A", Map.Sosaria, 250, new Rectangle2D(100, 100, 4, 4));
        BaseRegion b = new BaseRegion("FIX01-B", Map.Sosaria, 250, new Rectangle2D(110, 100, 4, 4));
        a.Register(); b.Register();
        try
        {
            foreach (int total in new int[] { 5000, 12000, 12001 })
            {
                PlayerMobile p = Player(total, 12000); p.SkillBoost = 2000;
                string before = Snapshot(p);
                for (int i = 0; i < 20; ++i)
                {
                    p.MoveToWorld(new Point3D(i % 2 == 0 ? 101 : 111, 101, 0), Map.Sosaria);
                    Check(p.Region == (i % 2 == 0 ? a : b), "real region transition dispatch");
                    Check(before == Snapshot(p), "travel preserves skills/stats/profession");
                }
                MemoryStream stream = new MemoryStream(); BinaryFileWriter writer = new BinaryFileWriter(stream, true);
                p.Skills.Serialize(writer); writer.Flush(); stream.Position = 0;
                Skills loaded = new Skills(p, new BinaryFileReader(new BinaryReader(stream)));
                Check(loaded.Total == total && loaded.Cap == 12000, "existing Skills format retains total and cap");
                for (int i = 0; i < loaded.Length; ++i)
                    Check(loaded[i].BaseFixedPoint == p.Skills[i].BaseFixedPoint && loaded[i].CapFixedPoint == p.Skills[i].CapFixedPoint
                        && loaded[i].Lock == p.Skills[i].Lock, "saved skill state retained");
                p.Delete();
            }
        }
        finally { a.Unregister(); b.Unregister(); }
        Console.WriteLine("PASS: 60 actual registered-region transitions and under/exact/over-cap Skills serialization roundtrips.");
    }
    private static void Historical(string method)
    {
        typeof(SkillCapInvestigation).GetMethod(method, BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
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
            SkillCheck.Initialize();
            Type random = typeof(SkillCapInvestigation).GetNestedType("FixedRandom", BindingFlags.NonPublic);
            typeof(Utility).GetField("m_Random", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, Activator.CreateInstance(random));
            if (args.Length > 1 && args[1] == "baseline")
            {
                Historical("TestReset");
                Console.WriteLine("BASELINE: reproduced original 500/1200 wipe and destructive region/stat correction on disposable in-memory characters.");
                return 0;
            }
            TestMatrix();
            TestFailureAndThrottle();
            TestTravelAndPersistence();
            Historical("TestGains");
            Console.WriteLine("PASS: " + checks + " protection assertions plus unchanged historical normal/accelerated gain matrix.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
