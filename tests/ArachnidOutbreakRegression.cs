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
