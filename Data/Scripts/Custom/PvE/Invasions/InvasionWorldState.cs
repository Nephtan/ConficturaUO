using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Custom.Confictura.Invasions
{
    public sealed class InvasionWorldState : Item
    {
        private static InvasionWorldState m_Instance;

        private bool m_EngineEnabled;
        private bool m_LegacyScanReviewed;
        private int m_NextSession;
        private DateTime m_LastEngineTickUtc;
        private InvasionCityState[] m_Cities;
        private List<InvasionContributionEvent> m_Contributions;
        private List<InvasionCycleCredit> m_CycleCredits;
        private List<InvasionCooldownRecord> m_Cooldowns;
        private List<InvasionConsentRecord> m_ConsentRecords;

        public static InvasionWorldState Instance { get { return m_Instance; } }

        [CommandProperty(AccessLevel.Administrator)]
        public bool EngineEnabled
        {
            get { return m_EngineEnabled; }
            set { m_EngineEnabled = value; }
        }

        public List<InvasionContributionEvent> Contributions { get { return m_Contributions; } }
        public List<InvasionCycleCredit> CycleCredits { get { return m_CycleCredits; } }
        public List<InvasionCooldownRecord> Cooldowns { get { return m_Cooldowns; } }
        public List<InvasionConsentRecord> ConsentRecords { get { return m_ConsentRecords; } }
        public DateTime LastEngineTickUtc { get { return m_LastEngineTickUtc; } set { m_LastEngineTickUtc = value; } }

        [CommandProperty(AccessLevel.Administrator)]
        public bool LegacyScanReviewed
        {
            get { return m_LegacyScanReviewed; }
            set { m_LegacyScanReviewed = value; }
        }

        public override string DefaultName { get { return "organic invasion world state"; } }

        public InvasionWorldState()
            : base(0x1F14)
        {
            Visible = false;
            Movable = false;
            m_EngineEnabled = false;
            m_NextSession = 1;
            m_LastEngineTickUtc = DateTime.UtcNow;
            InitializeCollections();
            MoveToWorld(Point3D.Zero, Map.Internal);
        }

        public InvasionWorldState(Serial serial)
            : base(serial)
        {
        }

        public static void SetInstance(InvasionWorldState state)
        {
            m_Instance = state;
        }

        public InvasionCityState GetCity(InvasionCityId city)
        {
            return m_Cities[(int)city];
        }

        public InvasionCityState[] GetCities()
        {
            return m_Cities;
        }

        public int AcquireSession()
        {
            if (m_NextSession < 1)
                m_NextSession = 1;

            return m_NextSession++;
        }

        private void InitializeCollections()
        {
            m_Cities = new InvasionCityState[]
            {
                new InvasionCityState(InvasionCityId.Britain),
                new InvasionCityState(InvasionCityId.Montor),
                new InvasionCityState(InvasionCityId.DevilGuard)
            };
            m_Contributions = new List<InvasionContributionEvent>();
            m_CycleCredits = new List<InvasionCycleCredit>();
            m_Cooldowns = new List<InvasionCooldownRecord>();
            m_ConsentRecords = new List<InvasionConsentRecord>();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_EngineEnabled);
            writer.Write(m_LegacyScanReviewed);
            writer.Write(m_NextSession);
            writer.Write(m_LastEngineTickUtc);
            writer.Write(m_Cities.Length);

            for (int i = 0; i < m_Cities.Length; ++i)
                m_Cities[i].Serialize(writer);

            writer.Write(m_Contributions.Count);

            for (int i = 0; i < m_Contributions.Count; ++i)
                m_Contributions[i].Serialize(writer);

            writer.Write(m_CycleCredits.Count);

            for (int i = 0; i < m_CycleCredits.Count; ++i)
                m_CycleCredits[i].Serialize(writer);

            writer.Write(m_Cooldowns.Count);

            for (int i = 0; i < m_Cooldowns.Count; ++i)
                m_Cooldowns[i].Serialize(writer);

            writer.Write(m_ConsentRecords.Count);

            for (int i = 0; i < m_ConsentRecords.Count; ++i)
                m_ConsentRecords[i].Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_EngineEnabled = reader.ReadBool();
            m_LegacyScanReviewed = reader.ReadBool();
            m_NextSession = reader.ReadInt();
            m_LastEngineTickUtc = reader.ReadDateTime();
            InitializeCollections();

            int cityCount = reader.ReadInt();

            for (int i = 0; i < cityCount; ++i)
            {
                InvasionCityState city = InvasionCityState.Deserialize(reader);

                if ((int)city.City >= 0 && (int)city.City < m_Cities.Length)
                    m_Cities[(int)city.City] = city;
            }

            int contributionCount = reader.ReadInt();

            for (int i = 0; i < contributionCount; ++i)
                m_Contributions.Add(InvasionContributionEvent.Deserialize(reader));

            int creditCount = reader.ReadInt();

            for (int i = 0; i < creditCount; ++i)
                m_CycleCredits.Add(InvasionCycleCredit.Deserialize(reader));

            int cooldownCount = reader.ReadInt();

            for (int i = 0; i < cooldownCount; ++i)
                m_Cooldowns.Add(InvasionCooldownRecord.Deserialize(reader));

            int consentCount = reader.ReadInt();

            for (int i = 0; i < consentCount; ++i)
                m_ConsentRecords.Add(InvasionConsentRecord.Deserialize(reader));
        }
    }
}
