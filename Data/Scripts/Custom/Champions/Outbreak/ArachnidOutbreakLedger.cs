using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Custom.Confictura.Champions
{
    // Contained controller state. Callers must validate encounter provenance and
    // resolved effects before crediting support; this class does not observe spells.
    public sealed class ArachnidOutbreakLedger
    {
        public const double MinimumMeaningfulPoints = 2.0;
        public const double SmallGroupQualification = 10.0;
        private string m_AttemptId;
        private readonly int[] m_Budgets = new int[4];
        private readonly bool[] m_Sealed = new bool[4];
        private readonly Dictionary<Mobile, double>[] m_Points = new Dictionary<Mobile, double>[4];
        public string AttemptId { get { return m_AttemptId; } }

        public ArachnidOutbreakLedger()
        {
            for (int i = 0; i < 4; ++i)
                m_Points[i] = new Dictionary<Mobile, double>();
        }

        public ArachnidOutbreakLedger(string attemptId, int broodBudget, int bossBudget) : this()
        {
            if (String.IsNullOrEmpty(attemptId) || broodBudget <= 0 || bossBudget <= 0)
                throw new ArgumentException("An attempt and positive stage health budgets are required.");
            m_AttemptId = attemptId;
            for (int i = 0; i < 4; ++i)
                m_Budgets[i] = i == 3 ? bossBudget : broodBudget;
        }

        public bool CreditDamage(int stage, Mobile participant, int resolvedDamage)
        {
            if (stage < 0 || stage > 3 || m_Budgets[stage] <= 0 || resolvedDamage <= 0)
                return false;
            return Credit(stage, participant, (double)resolvedDamage * 100.0 / m_Budgets[stage]);
        }

        public bool CreditValidatedHealing(int stage, Mobile healer, Mobile patient, int encounterHealthRestored)
        {
            if (patient == null || patient.Deleted || patient == healer || patient.HitsMax <= 0 || encounterHealthRestored <= 0)
                return false;
            return Credit(stage, healer, (double)encounterHealthRestored * 10.0 / patient.HitsMax);
        }

        public bool CreditValidatedSupport(int stage, Mobile participant, double points)
        {
            return Credit(stage, participant, points);
        }

        private bool Credit(int stage, Mobile participant, double points)
        {
            if (stage < 0 || stage > 3 || m_Budgets[stage] <= 0 || m_Sealed[stage]
                || participant == null || participant.Deleted || !participant.Player
                || Double.IsNaN(points) || Double.IsInfinity(points) || points <= 0.0)
                return false;
            double previous;
            if (!m_Points[stage].TryGetValue(participant, out previous) && m_Points[stage].Count >= 100000)
                return false;
            double next = Math.Min(10.0, previous + points);
            m_Points[stage][participant] = next;
            return next > previous;
        }

        public double Points(int stage, Mobile participant)
        {
            double points;
            return stage >= 0 && stage < 4 && participant != null && m_Points[stage].TryGetValue(participant, out points) ? points : 0.0;
        }

        public List<Mobile> Qualified(int stage)
        {
            List<Mobile> result = new List<Mobile>();
            double required = QualificationThreshold(stage);
            if (stage >= 0 && stage < 4)
                foreach (KeyValuePair<Mobile, double> entry in m_Points[stage])
                    if (!entry.Key.Deleted && entry.Key.Player && entry.Value >= required - 1e-9)
                        result.Add(entry.Key);
            result.Sort(delegate(Mobile a, Mobile b) { return a.Serial.CompareTo(b.Serial); });
            return result;
        }

        public int ActiveContributors(int stage)
        {
            int count = 0;
            if (stage >= 0 && stage < 4)
                foreach (KeyValuePair<Mobile, double> entry in m_Points[stage])
                    if (!entry.Key.Deleted && entry.Key.Player && entry.Value >= MinimumMeaningfulPoints - 1e-9)
                        ++count;
            return count;
        }

        public double QualificationThreshold(int stage)
        {
            int count = ActiveContributors(stage);
            if (count == 0) return SmallGroupQualification;
            // A stage's damage budget represents 100 points. Require half an
            // equal share, capped at the small-group target and an absolute floor.
            // Only meaningful stage effort counts: party/proximity and tiny tags
            // cannot lower the target. Earned dead/offline contribution remains.
            return Math.Max(MinimumMeaningfulPoints, Math.Min(SmallGroupQualification, 50.0 / count));
        }

        // Queue all qualified entitlements before sealing and advancing lifecycle.
        // No further contribution can be added to the completed stage.
        public bool Seal(int stage)
        {
            if (stage < 0 || stage > 3 || m_Sealed[stage] || m_Budgets[stage] <= 0)
                return false;
            m_Sealed[stage] = true;
            return true;
        }

        public bool IsSealed(int stage)
        {
            return stage < 0 || stage > 3 || m_Sealed[stage];
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write(m_AttemptId);
            for (int stage = 0; stage < 4; ++stage)
            {
                writer.Write(m_Budgets[stage]);
                writer.Write(m_Sealed[stage]);
                writer.Write(m_Points[stage].Count);
                foreach (KeyValuePair<Mobile, double> entry in m_Points[stage])
                {
                    writer.Write(entry.Key);
                    writer.Write(entry.Value);
                }
            }
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            if (version != 0)
                throw new InvalidDataException("Unknown outbreak ledger version.");
            m_AttemptId = reader.ReadString();
            for (int stage = 0; stage < 4; ++stage)
            {
                m_Budgets[stage] = reader.ReadInt();
                m_Sealed[stage] = reader.ReadBool();
                int count = reader.ReadInt();
                if (count < 0 || count > 100000)
                    throw new InvalidDataException("Invalid outbreak participant count.");
                m_Points[stage].Clear();
                for (int i = 0; i < count; ++i)
                {
                    Mobile participant = reader.ReadMobile();
                    double points = reader.ReadDouble();
                    // Item deserialization can precede mobile field loading;
                    // Player/Deleted eligibility is checked when using the entry.
                    if (participant != null
                        && !Double.IsNaN(points) && !Double.IsInfinity(points) && points > 0.0)
                        m_Points[stage][participant] = Math.Min(10.0, points);
                }
            }
        }
    }
}
