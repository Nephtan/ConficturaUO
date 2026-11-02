using System;

namespace Server.Custom.Confictura.Champions
{
    public sealed class ArachnidOutbreakTuning
    {
        public int InnerPopulationPercent = 45;
        public int ReplacementSeconds = 30;
        public int FrontierPerception = 8;
        public int FrontierPursuit = 24;
        public int LocalWarningSeconds = 10;
        public int QueenRetreatPercent = 45;
        public int QueenReengagePercent = 70;
        public int QueenBackupCount = 6;
        public int QueenBackupDelaySeconds = 10;
        public bool FrontierSuppliesEnabled = true;

        public ArachnidOutbreakTuning Copy() { return (ArachnidOutbreakTuning)MemberwiseClone(); }
        private static void Range(int value, int minimum, int maximum, string name)
        { if (value < minimum || value > maximum) throw new ArgumentOutOfRangeException(name, "Allowed range: " + minimum + " to " + maximum + "."); }

        public void Validate()
        {
            Range(InnerPopulationPercent,20,100,"InnerPopulationPercent");
            Range(ReplacementSeconds,5,180,"ReplacementSeconds");
            Range(FrontierPerception,4,12,"FrontierPerception");
            Range(FrontierPursuit,12,40,"FrontierPursuit");
            Range(LocalWarningSeconds,5,30,"LocalWarningSeconds");
            Range(QueenRetreatPercent,20,70,"QueenRetreatPercent");
            Range(QueenReengagePercent,45,95,"QueenReengagePercent");
            Range(QueenBackupCount,0,12,"QueenBackupCount");
            Range(QueenBackupDelaySeconds,5,30,"QueenBackupDelaySeconds");
            if (FrontierPursuit < FrontierPerception+4) throw new ArgumentException("Pursuit must exceed perception by at least four tiles.");
            if (QueenReengagePercent < QueenRetreatPercent+15) throw new ArgumentException("Queen reengagement must exceed retreat by at least fifteen health percentage points.");
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write(InnerPopulationPercent); writer.Write(ReplacementSeconds);
            writer.Write(FrontierPerception); writer.Write(FrontierPursuit); writer.Write(LocalWarningSeconds);
            writer.Write(QueenRetreatPercent); writer.Write(QueenReengagePercent);
            writer.Write(QueenBackupCount); writer.Write(QueenBackupDelaySeconds); writer.Write(FrontierSuppliesEnabled);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version != 0) throw new InvalidOperationException("Unknown outbreak tuning version.");
            InnerPopulationPercent=reader.ReadInt(); ReplacementSeconds=reader.ReadInt();
            FrontierPerception=reader.ReadInt(); FrontierPursuit=reader.ReadInt(); LocalWarningSeconds=reader.ReadInt();
            QueenRetreatPercent=reader.ReadInt(); QueenReengagePercent=reader.ReadInt();
            QueenBackupCount=reader.ReadInt(); QueenBackupDelaySeconds=reader.ReadInt(); FrontierSuppliesEnabled=reader.ReadBool();
            Validate();
        }
    }

    public partial class ArachnidOutbreakController
    {
        private ArachnidOutbreakTuning m_Tuning = new ArachnidOutbreakTuning();
        public ArachnidOutbreakTuning Tuning { get { return m_Tuning.Copy(); } }
        private void SetTuning(Action<ArachnidOutbreakTuning> change)
        {
            if (m_State.Phase != ArachnidOutbreakPhase.Disabled) throw new InvalidOperationException("Stop this controller before changing tuning. Changes apply to its next attempt.");
            ArachnidOutbreakTuning candidate=m_Tuning.Copy(); change(candidate); candidate.Validate(); m_Tuning=candidate;
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int InnerPopulationPercent { get { return m_Tuning.InnerPopulationPercent; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.InnerPopulationPercent=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int ReplacementSeconds { get { return m_Tuning.ReplacementSeconds; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.ReplacementSeconds=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int FrontierPerception { get { return m_Tuning.FrontierPerception; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.FrontierPerception=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int FrontierPursuit { get { return m_Tuning.FrontierPursuit; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.FrontierPursuit=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int LocalWarningSeconds { get { return m_Tuning.LocalWarningSeconds; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.LocalWarningSeconds=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int QueenRetreatPercent { get { return m_Tuning.QueenRetreatPercent; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.QueenRetreatPercent=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int QueenReengagePercent { get { return m_Tuning.QueenReengagePercent; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.QueenReengagePercent=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int QueenBackupCount { get { return m_Tuning.QueenBackupCount; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.QueenBackupCount=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public int QueenBackupDelaySeconds { get { return m_Tuning.QueenBackupDelaySeconds; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.QueenBackupDelaySeconds=value;}); } }
        [CommandProperty(AccessLevel.Administrator)]
        public bool FrontierSuppliesEnabled { get { return m_Tuning.FrontierSuppliesEnabled; } set { SetTuning(delegate(ArachnidOutbreakTuning t){t.FrontierSuppliesEnabled=value;}); } }

        private void SendTuning(Mobile from)
        {
            from.SendMessage("Next-attempt tuning: population {0}% (20-100); replacement {1}s (5-180); frontier perception/pursuit {2}/{3} (4-12/12-40).",InnerPopulationPercent,ReplacementSeconds,FrontierPerception,FrontierPursuit);
            from.SendMessage("Local warning {0}s (5-30); queen retreat/reengage {1}/{2}% (20-70/45-95, gap>=15); backup {3} (0-12), delay {4}s (5-30); frontier supplies {5}.",LocalWarningSeconds,QueenRetreatPercent,QueenReengagePercent,QueenBackupCount,QueenBackupDelaySeconds,FrontierSuppliesEnabled);
            from.SendMessage("Current queen {0}; backup spent/pending {1}/{2}; rally patch {3}; interrupted calls {4}; frontier supply recipients {5}.",QueenMode,QueenBackupSpent,QueenBackupPending,QueenRallyPatch,QueenBackupInterrupted,FrontierRewardRecipients);
            from.SendMessage("Edit with [Props while disabled. No live retuning, actor target, global level or production delivery setting is exposed here.");
        }
    }
}
