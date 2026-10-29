using System;

namespace Server.Custom.Confictura.Champions
{
    public enum ArachnidOutbreakPhase
    {
        Disabled,
        Searching,
        Broods,
        Boss,
        Cooldown
    }

    // All dates are UTC wall-clock dates. Combat effect clocks are separate.
    public sealed class ArachnidOutbreakState
    {
        private ArachnidOutbreakPhase m_Phase;
        private string m_AttemptId = String.Empty;
        private string m_SiteId = String.Empty;
        private int m_ClearedBroods;
        private DateTime m_LastActivity;
        private DateTime m_NextStart;

        public ArachnidOutbreakPhase Phase { get { return m_Phase; } }
        public string AttemptId { get { return m_AttemptId; } }
        public string SiteId { get { return m_SiteId; } }
        public int ClearedBroods { get { return m_ClearedBroods; } }
        public DateTime LastActivity { get { return m_LastActivity; } }
        public DateTime NextStart { get { return m_NextStart; } }
        public bool Engaged { get { return m_LastActivity != DateTime.MinValue; } }
        public bool Active
        {
            get { return m_Phase == ArachnidOutbreakPhase.Broods || m_Phase == ArachnidOutbreakPhase.Boss; }
        }

        public void SetEnabled(bool enabled)
        {
            if (!enabled)
            {
                m_Phase = ArachnidOutbreakPhase.Disabled;
                ClearAttempt();
                m_NextStart = DateTime.MinValue;
            }
            else if (m_Phase == ArachnidOutbreakPhase.Disabled)
                m_Phase = ArachnidOutbreakPhase.Searching;
        }

        public bool TryStart(string siteId)
        {
            if (m_Phase != ArachnidOutbreakPhase.Searching || String.IsNullOrEmpty(siteId))
                return false;

            ClearAttempt();
            m_AttemptId = Guid.NewGuid().ToString("N");
            m_SiteId = siteId;
            m_Phase = ArachnidOutbreakPhase.Broods;
            return true;
        }

        // Call only after an actual, attributed encounter outcome, never proximity.
        public bool RecordActivity(DateTime utcNow)
        {
            if (!Active || utcNow == DateTime.MinValue)
                return false;

            if (utcNow > m_LastActivity)
                m_LastActivity = utcNow;
            return true;
        }

        public bool CompleteBrood(int stage, DateTime utcNow)
        {
            if (m_Phase != ArachnidOutbreakPhase.Broods || stage < 0 || stage > 2 || utcNow == DateTime.MinValue)
                return false;

            int bit = 1 << stage;
            if ((m_ClearedBroods & bit) != 0)
                return false;

            m_ClearedBroods |= bit;
            RecordActivity(utcNow);
            if (m_ClearedBroods == 7)
                m_Phase = ArachnidOutbreakPhase.Boss;
            return true;
        }

        public bool CompleteBoss(DateTime utcNow, int cooldownMinutes)
        {
            if (m_Phase != ArachnidOutbreakPhase.Boss || utcNow == DateTime.MinValue)
                return false;

            EnterCooldown(utcNow, cooldownMinutes);
            return true;
        }

        public bool AbortAttempt(DateTime utcNow, int cooldownMinutes)
        {
            if (!Active || utcNow == DateTime.MinValue)
                return false;
            EnterCooldown(utcNow, cooldownMinutes);
            return true;
        }

        // Returns true only when owned-world reconciliation/cleanup is necessary.
        public bool Advance(DateTime utcNow, int cooldownMinutes)
        {
            if (Active && Engaged && utcNow - m_LastActivity >= TimeSpan.FromMinutes(30))
            {
                EnterCooldown(utcNow, cooldownMinutes);
                return true;
            }

            if (m_Phase == ArachnidOutbreakPhase.Cooldown && utcNow >= m_NextStart)
            {
                ClearAttempt();
                m_NextStart = DateTime.MinValue;
                m_Phase = ArachnidOutbreakPhase.Searching;
                return true;
            }

            return false;
        }

        private void EnterCooldown(DateTime utcNow, int minutes)
        {
            if (minutes < 30 || minutes > 60)
                throw new ArgumentOutOfRangeException("minutes");

            m_NextStart = utcNow.AddMinutes(minutes);
            m_Phase = ArachnidOutbreakPhase.Cooldown;
        }

        private void ClearAttempt()
        {
            m_AttemptId = String.Empty;
            m_SiteId = String.Empty;
            m_ClearedBroods = 0;
            m_LastActivity = DateTime.MinValue;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write((int)m_Phase);
            writer.Write(m_AttemptId);
            writer.Write(m_SiteId);
            writer.Write(m_ClearedBroods);
            writer.Write(m_LastActivity);
            writer.Write(m_NextStart);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version != 0)
                throw new InvalidOperationException("Unsupported arachnid outbreak state version.");

            m_Phase = (ArachnidOutbreakPhase)reader.ReadInt();
            m_AttemptId = reader.ReadString();
            m_SiteId = reader.ReadString();
            m_ClearedBroods = reader.ReadInt();
            m_LastActivity = reader.ReadDateTime();
            m_NextStart = reader.ReadDateTime();

            if (!Enum.IsDefined(typeof(ArachnidOutbreakPhase), m_Phase)
                || m_ClearedBroods < 0 || m_ClearedBroods > 7
                || (Active && (String.IsNullOrEmpty(m_AttemptId) || String.IsNullOrEmpty(m_SiteId)))
                || (m_Phase == ArachnidOutbreakPhase.Boss && m_ClearedBroods != 7)
                || (m_Phase == ArachnidOutbreakPhase.Broods && m_ClearedBroods == 7)
                || (m_Phase == ArachnidOutbreakPhase.Cooldown && m_NextStart == DateTime.MinValue))
                throw new InvalidOperationException("Invalid persisted arachnid outbreak state.");
        }
    }
}
