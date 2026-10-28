using System;
using System.IO;
using Server;
using Server.Custom.Confictura.Champions;

// Exercises production lifecycle and positional serialization without a world or listeners.
internal static class ArachnidOutbreakRegression
{
    private static int s_Checks;
    private static readonly DateTime Epoch = new DateTime(2026, 10, 28, 12, 0, 0, DateTimeKind.Utc);

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception("FAILED: " + message);
        ++s_Checks;
    }

    private static ArachnidOutbreakState RoundTrip(ArachnidOutbreakState source)
    {
        MemoryStream bytes = new MemoryStream();
        BinaryFileWriter writer = new BinaryFileWriter(bytes, true);
        source.Serialize(writer);
        writer.Write(17843);
        writer.Close();
        BinaryFileReader reader = new BinaryFileReader(new BinaryReader(new MemoryStream(bytes.ToArray())));
        ArachnidOutbreakState restored = new ArachnidOutbreakState();
        restored.Deserialize(reader);
        Check(reader.ReadInt() == 17843, "positional reads stop at the sentinel");
        reader.Close();
        Check(restored.Phase == source.Phase && restored.AttemptId == source.AttemptId
            && restored.SiteId == source.SiteId && restored.ClearedBroods == source.ClearedBroods
            && restored.LastActivity == source.LastActivity && restored.NextStart == source.NextStart,
            "all persisted fields survive round trip");
        return restored;
    }

    private static void Permutation(int first, int second, int third)
    {
        ArachnidOutbreakState state = new ArachnidOutbreakState();
        Check(state.Phase == ArachnidOutbreakPhase.Disabled, "new state disabled");
        Check(!state.TryStart("site") && !state.RecordActivity(Epoch), "disabled refuses activity/start");
        state = RoundTrip(state);
        state.SetEnabled(true);
        Check(!state.TryStart(null) && !state.TryStart(String.Empty), "missing site refused");
        Check(state.TryStart("reviewed-site"), "start one outbreak");
        string attempt = state.AttemptId;
        Check(!state.TryStart("another"), "active attempt cannot be replaced");
        Check(!state.Advance(Epoch.AddDays(10), 30) && !state.Engaged, "undiscovered outbreak never expires");
        Check(!state.CompleteBoss(Epoch, 30), "boss cannot complete before broods");
        Check(!state.CompleteBrood(-1, Epoch) && !state.CompleteBrood(3, Epoch), "stage bounds");
        Check(state.CompleteBrood(first, Epoch), "first brood completion");
        Check(!state.CompleteBrood(first, Epoch.AddMinutes(20)), "duplicate brood cannot refresh activity or repay");
        Check(state.LastActivity == Epoch, "duplicate keeps original recovery deadline");
        state = RoundTrip(state);
        Check(!state.Advance(Epoch.AddMinutes(29), 60), "recovery before deadline");
        Check(state.RecordActivity(Epoch.AddMinutes(29)), "effective action refreshes recovery");
        state.RecordActivity(Epoch.AddMinutes(10));
        Check(state.LastActivity == Epoch.AddMinutes(29), "clock reversal cannot shorten recovery");
        Check(state.CompleteBrood(second, Epoch.AddMinutes(30)), "second brood");
        Check(state.CompleteBrood(third, Epoch.AddMinutes(31)), "third brood");
        Check(state.Phase == ArachnidOutbreakPhase.Boss && state.ClearedBroods == 7, "all orders unlock boss");
        state = RoundTrip(state);
        state.SetEnabled(true);
        Check(state.AttemptId == attempt, "repeated enable does not reset attempt");
        Check(state.CompleteBoss(Epoch.AddMinutes(35), 60), "real completion transitions once");
        Check(!state.CompleteBoss(Epoch.AddMinutes(35), 30), "duplicate completion cannot shorten cooldown");
        Check(!state.RecordActivity(Epoch.AddMinutes(36)), "cooldown cannot be extended by activity");
        state = RoundTrip(state);
        Check(!state.Advance(Epoch.AddMinutes(94), 30), "cooldown minimum honored");
        Check(state.Advance(Epoch.AddMinutes(95), 30), "cooldown ends at exact deadline");
        Check(state.TryStart("new-site") && state.AttemptId != attempt, "new identity after cooldown");
        state.SetEnabled(false);
        Check(!state.Active && !state.Engaged && state.ClearedBroods == 0, "disable discards active progress");
        Check(!state.Advance(Epoch.AddDays(20), 30), "disabled state never reactivates itself");
    }

    private static void Abandonment()
    {
        ArachnidOutbreakState state = new ArachnidOutbreakState();
        state.SetEnabled(true);
        state.TryStart("site");
        state.CompleteBrood(1, Epoch);
        state = RoundTrip(state);
        Check(state.Advance(Epoch.AddMinutes(30), 30), "abandonment at exact thirty minute boundary");
        Check(state.Phase == ArachnidOutbreakPhase.Cooldown, "abandonment cannot start another attempt immediately");
        Check(!state.TryStart("site"), "abandoned broods cannot immediately be farmed again");
        state = RoundTrip(state);
        Check(state.NextStart == Epoch.AddMinutes(60), "full cooldown follows abandonment");
        Check(state.Advance(Epoch.AddMinutes(60), 30) && state.ClearedBroods == 0, "next attempt starts clean");
        state.TryStart("site");
        state.RecordActivity(Epoch);
        bool threw = false;
        try { state.Advance(Epoch.AddMinutes(30), 29); }
        catch (ArgumentOutOfRangeException) { threw = true; }
        Check(threw && state.Phase == ArachnidOutbreakPhase.Broods, "invalid cooldown cannot partially mutate state");
    }

    private sealed class Terrain : IArachnidOutbreakTerrain
    {
        public int Mode;
        public int Calls;
        public bool HasPermanentHousingProhibition(Rectangle2D bounds) { return Mode != 7; }
        public bool IsReserved(Rectangle2D bounds) { return Mode == 1; }
        public bool TryGetPoint(int x, int y, out Point3D point)
        {
            ++Calls;
            point = new Point3D(x, y, Mode == 4 && x > 1000 ? 20 : 0);
            if (Mode == 2 && x == 1000 && y == 1000)
                return false;
            if (Mode == 3 && (Math.Abs(x - 1000) > 40 || Math.Abs(y - 1000) > 40))
                return false;
            if (Mode == 5 && x == 972 && y == 980)
                return false;
            if (Mode == 6 && x == 973 && y == 980)
                return false;
            return true;
        }
    }

    private static void Sites()
    {
        foreach (ArachnidOutbreakSite candidate in ArachnidOutbreakSites.Candidates)
        {
            Check(!candidate.Reviewed, "candidate has no implied client acceptance");
            Check(Object.ReferenceEquals(ArachnidOutbreakSites.Find(candidate.Id), candidate), "catalog lookup returns canonical site");
        }
        Check(ArachnidOutbreakSites.Find("unknown") == null, "unknown catalog site has no fallback");
        ArachnidOutbreakPlacement placement;
        string reason;
        Terrain terrain = new Terrain();
        ArachnidOutbreakSite site = new ArachnidOutbreakSite("test", 1000, 1000, false);
        Check(!site.Reviewed, "terrain validation alone does not grant client review");
        Check(site.Validate(terrain, out placement, out reason), "open, connected arena accepted");
        Check(placement.WalkableCount == 113 * 113, "bounded footprint includes all open tiles");
        Check(placement.Boss == new Point3D(1000, 1000, 0)
            && placement.Brood(0) == new Point3D(972, 980, 0)
            && placement.Brood(1) == new Point3D(1028, 980, 0)
            && placement.Brood(2) == new Point3D(1000, 1030, 0), "four connected objective anchors");
        for (int mode = 1; mode <= 7; ++mode)
        {
            terrain.Mode = mode;
            Check(!site.Validate(terrain, out placement, out reason) && placement == null && reason.Length > 0,
                "reject reserved, blocked center, sealed pocket, cliff, blocked brood, blocked staging tile or housing eligibility: " + mode);
        }
        int[] invalidX = new int[] { 0, 80, 5039, 6000 };
        foreach (int x in invalidX)
        {
            terrain.Calls = 0;
            Check(!new ArachnidOutbreakSite("invalid", x, 1000, false).Validate(terrain, out placement, out reason)
                && terrain.Calls == 0, "out of bounds fails before terrain access");
        }
        foreach (int y in new int[] { 0, 80, 3047, 4000 })
            Check(!new ArachnidOutbreakSite("invalid", 1000, y, false).Validate(terrain, out placement, out reason),
                "out of mainland vertical bounds");
    }

    public static int Main(string[] args)
    {
        try
        {
            for (int first = 0; first < 3; ++first)
                for (int second = 0; second < 3; ++second)
                    for (int third = 0; third < 3; ++third)
                        if (first != second && first != third && second != third)
                            Permutation(first, second, third);
            Abandonment();
            Sites();
            Console.WriteLine("PASS current outbreak: " + s_Checks + " lifecycle/serialization assertions; no world or listener.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
