using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.Confictura.Invasions
{
    public enum InvasionCityId
    {
        Britain,
        Montor,
        DevilGuard
    }

    public enum InvasionFaction
    {
        None,
        ClockworkDominion,
        BloodCourt,
        AbyssalLegion
    }

    public enum InvasionState
    {
        Dormant,
        RitualReady,
        Ritual,
        Encirclement,
        Breach,
        FinalBattle,
        Occupied,
        Liberation
    }

    public enum InvasionSide
    {
        None,
        Invader,
        Defender
    }

    public enum InvasionRole
    {
        None,
        RitualFocus,
        CampStandard,
        CivicWard,
        SupplyAnchor,
        RankMelee,
        RankRanged,
        RankCaster,
        Lieutenant,
        Commander,
        CivicWarden,
        OccupationPatrol,
        AnchorCaptain,
        Banker,
        Healer,
        Provisioner,
        Fence,
        Quartermaster
    }

    public enum InvasionContributionType
    {
        Murder,
        Theft,
        CriminalAggression,
        NpcCrime
    }

    public interface IInvasionOwned
    {
        int InvasionSession { get; }
        InvasionCityId InvasionCity { get; }
        InvasionFaction InvasionFaction { get; }
        InvasionSide InvasionSide { get; }
        InvasionRole InvasionRole { get; }
        int InvasionObjectiveIndex { get; }
    }

    public sealed class InvasionCityDefinition
    {
        private readonly InvasionCityId m_City;
        private readonly string m_Name;
        private readonly string m_RegionName;
        private readonly Rectangle2D[] m_RegionBounds;
        private Point3D m_Ritual;
        private Point3D[] m_Camps;
        private Point3D[] m_Wards;
        private Point3D m_FinalBattle;

        public InvasionCityId City { get { return m_City; } }
        public string Name { get { return m_Name; } }
        public string RegionName { get { return m_RegionName; } }
        public Rectangle2D[] RegionBounds { get { return m_RegionBounds; } }
        public Point3D Ritual { get { return m_Ritual; } set { m_Ritual = value; } }
        public Point3D[] Camps { get { return m_Camps; } }
        public Point3D[] Wards { get { return m_Wards; } }
        public Point3D FinalBattle { get { return m_FinalBattle; } set { m_FinalBattle = value; } }

        public InvasionCityDefinition(
            InvasionCityId city,
            string name,
            string regionName,
            Rectangle2D[] regionBounds,
            Point3D ritual,
            Point3D[] camps,
            Point3D[] wards,
            Point3D finalBattle
        )
        {
            m_City = city;
            m_Name = name;
            m_RegionName = regionName;
            m_RegionBounds = regionBounds;
            m_Ritual = ritual;
            m_Camps = camps;
            m_Wards = wards;
            m_FinalBattle = finalBattle;
        }

        public bool ContainsTown(Point3D location)
        {
            for (int i = 0; i < m_RegionBounds.Length; ++i)
            {
                if (m_RegionBounds[i].Contains(location))
                    return true;
            }

            return false;
        }

        public bool ContainsConflict(Point3D location)
        {
            if (ContainsTown(location) || InRange2D(location, m_Ritual, 24) || InRange2D(location, m_FinalBattle, 24))
                return true;

            for (int i = 0; i < m_Camps.Length; ++i)
            {
                if (InRange2D(location, m_Camps[i], 24))
                    return true;
            }

            for (int i = 0; i < m_Wards.Length; ++i)
            {
                if (InRange2D(location, m_Wards[i], 24))
                    return true;
            }

            return false;
        }

        private static bool InRange2D(Point3D first, Point3D second, int range)
        {
            return Math.Abs(first.X - second.X) <= range && Math.Abs(first.Y - second.Y) <= range;
        }
    }

    public static class InvasionCities
    {
        private static readonly InvasionCityDefinition[] m_Definitions = new InvasionCityDefinition[]
        {
            new InvasionCityDefinition(
                InvasionCityId.Britain,
                "Britain",
                "the City of Britain",
                new Rectangle2D[]
                {
                    new Rectangle2D(2929, 985, 127, 143),
                    new Rectangle2D(2940, 886, 156, 105)
                },
                new Point3D(3125, 1035, 0),
                new Point3D[]
                {
                    new Point3D(2914, 1068, 0),
                    new Point3D(2999, 870, 0),
                    new Point3D(3115, 1059, 0)
                },
                new Point3D[]
                {
                    new Point3D(2961, 1073, 0),
                    new Point3D(2995, 936, 0),
                    new Point3D(3048, 1057, 0)
                },
                new Point3D(2999, 1053, 0)
            ),
            new InvasionCityDefinition(
                InvasionCityId.Montor,
                "Montor",
                "the City of Montor",
                new Rectangle2D[] { new Rectangle2D(3057, 2561, 326, 102) },
                new Point3D(3243, 2785, 0),
                new Point3D[]
                {
                    new Point3D(3035, 2607, 0),
                    new Point3D(3212, 2545, 0),
                    new Point3D(3400, 2607, 0)
                },
                new Point3D[]
                {
                    new Point3D(3078, 2610, 0),
                    new Point3D(3209, 2624, 0),
                    new Point3D(3356, 2611, 0)
                },
                new Point3D(3212, 2610, 0)
            ),
            new InvasionCityDefinition(
                InvasionCityId.DevilGuard,
                "Devil Guard",
                "the Town of Devil Guard",
                new Rectangle2D[]
                {
                    new Rectangle2D(1581, 1439, 129, 169),
                    new Rectangle2D(1687, 1494, 58, 141),
                    new Rectangle2D(1731, 1575, 48, 68),
                    new Rectangle2D(1674, 1584, 19, 21)
                },
                new Point3D(1800, 1624, 2),
                new Point3D[]
                {
                    new Point3D(1565, 1500, 2),
                    new Point3D(1660, 1425, 2),
                    new Point3D(1800, 1624, 2)
                },
                new Point3D[]
                {
                    new Point3D(1605, 1542, 7),
                    new Point3D(1671, 1463, 2),
                    new Point3D(1725, 1538, 2)
                },
                new Point3D(1691, 1464, 15)
            )
        };

        public static InvasionCityDefinition[] Definitions { get { return m_Definitions; } }

        public static InvasionCityDefinition Get(InvasionCityId city)
        {
            return m_Definitions[(int)city];
        }
    }

    public sealed class InvasionSpawnerRecord
    {
        public int Serial;
        public bool WasRunning;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(Serial);
            writer.Write(WasRunning);
        }

        public static InvasionSpawnerRecord Deserialize(GenericReader reader)
        {
            InvasionSpawnerRecord record = new InvasionSpawnerRecord();
            record.Serial = reader.ReadInt();
            record.WasRunning = reader.ReadBool();
            return record;
        }
    }

    public sealed class InvasionConsentRecord
    {
        public int PlayerSerial;
        public InvasionCityId City;
        public InvasionSide AssignedSide;
        public NONPK PreviousConsent;
        public bool ConsentChanged;
        public Point3D EntryLocation;
        public DateTime GraceUntilUtc;
        public DateTime LastCombatUtc;
        public DateTime TraitorUntilUtc;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(PlayerSerial);
            writer.Write((int)City);
            writer.Write((int)AssignedSide);
            writer.Write((int)PreviousConsent);
            writer.Write(ConsentChanged);
            writer.Write(EntryLocation);
            writer.Write(GraceUntilUtc);
            writer.Write(LastCombatUtc);
            writer.Write(TraitorUntilUtc);
        }

        public static InvasionConsentRecord Deserialize(GenericReader reader)
        {
            InvasionConsentRecord record = new InvasionConsentRecord();
            record.PlayerSerial = reader.ReadInt();
            record.City = (InvasionCityId)reader.ReadInt();
            record.AssignedSide = (InvasionSide)reader.ReadInt();
            record.PreviousConsent = (NONPK)reader.ReadInt();
            record.ConsentChanged = reader.ReadBool();
            record.EntryLocation = reader.ReadPoint3D();
            record.GraceUntilUtc = reader.ReadDateTime();
            record.LastCombatUtc = reader.ReadDateTime();
            record.TraitorUntilUtc = reader.ReadDateTime();
            return record;
        }
    }

    public sealed class InvasionContributionEvent
    {
        public string AccountName;
        public InvasionCityId City;
        public InvasionContributionType Type;
        public int Points;
        public DateTime CreatedUtc;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(AccountName);
            writer.Write((int)City);
            writer.Write((int)Type);
            writer.Write(Points);
            writer.Write(CreatedUtc);
        }

        public static InvasionContributionEvent Deserialize(GenericReader reader)
        {
            InvasionContributionEvent entry = new InvasionContributionEvent();
            entry.AccountName = reader.ReadString();
            entry.City = (InvasionCityId)reader.ReadInt();
            entry.Type = (InvasionContributionType)reader.ReadInt();
            entry.Points = reader.ReadInt();
            entry.CreatedUtc = reader.ReadDateTime();
            return entry;
        }
    }

    public sealed class InvasionCycleCredit
    {
        public string AccountName;
        public InvasionCityId City;
        public int Points;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(AccountName);
            writer.Write((int)City);
            writer.Write(Points);
        }

        public static InvasionCycleCredit Deserialize(GenericReader reader)
        {
            InvasionCycleCredit credit = new InvasionCycleCredit();
            credit.AccountName = reader.ReadString();
            credit.City = (InvasionCityId)reader.ReadInt();
            credit.Points = reader.ReadInt();
            return credit;
        }
    }

    public sealed class InvasionCooldownRecord
    {
        public string Key;
        public DateTime ExpiresUtc;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(Key);
            writer.Write(ExpiresUtc);
        }

        public static InvasionCooldownRecord Deserialize(GenericReader reader)
        {
            InvasionCooldownRecord record = new InvasionCooldownRecord();
            record.Key = reader.ReadString();
            record.ExpiresUtc = reader.ReadDateTime();
            return record;
        }
    }

    public sealed class InvasionCityState
    {
        public InvasionCityId City;
        public bool Enabled;
        public bool ValidationPassed;
        public bool StaffPaused;
        public bool IdlePaused;
        public InvasionState State;
        public InvasionFaction Faction;
        public int Session;
        public int Corruption;
        public int RitualLeaderSerial;
        public DateTime CycleStartedUtc;
        public DateTime StateStartedUtc;
        public DateTime PhaseEndsUtc;
        public DateTime LastPlayerSeenUtc;
        public DateTime LastDecayUtc;
        public DateTime ProtectionUntilUtc;
        public DateTime RitualLockUntilUtc;
        public int CampHoldSeconds;
        public bool[] CampsCleansed;
        public int[] WardProgressSeconds;
        public int[] AnchorIntegrity;
        public int[] AnchorDailyRepair;
        public DateTime[] AnchorRepairDayUtc;
        public int HighestParticipants;
        public bool CommanderDead;
        public bool WardenDead;
        public List<int> OwnedSerials;
        public List<InvasionSpawnerRecord> SuppressedSpawners;

        public InvasionCityState(InvasionCityId city)
        {
            City = city;
            State = InvasionState.Dormant;
            Faction = InvasionFaction.None;
            CycleStartedUtc = DateTime.UtcNow;
            StateStartedUtc = DateTime.UtcNow;
            LastPlayerSeenUtc = DateTime.UtcNow;
            LastDecayUtc = DateTime.UtcNow;
            CampsCleansed = new bool[3];
            WardProgressSeconds = new int[3];
            AnchorIntegrity = new int[] { 100, 100, 100 };
            AnchorDailyRepair = new int[3];
            AnchorRepairDayUtc = new DateTime[] { DateTime.UtcNow.Date, DateTime.UtcNow.Date, DateTime.UtcNow.Date };
            OwnedSerials = new List<int>();
            SuppressedSpawners = new List<InvasionSpawnerRecord>();
        }

        public bool IsConflictActive
        {
            get
            {
                return State == InvasionState.Ritual
                    || State == InvasionState.Encirclement
                    || State == InvasionState.Breach
                    || State == InvasionState.FinalBattle
                    || State == InvasionState.Occupied
                    || State == InvasionState.Liberation;
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0); // version
            writer.Write((int)City);
            writer.Write(Enabled);
            writer.Write(ValidationPassed);
            writer.Write(StaffPaused);
            writer.Write(IdlePaused);
            writer.Write((int)State);
            writer.Write((int)Faction);
            writer.Write(Session);
            writer.Write(Corruption);
            writer.Write(RitualLeaderSerial);
            writer.Write(CycleStartedUtc);
            writer.Write(StateStartedUtc);
            writer.Write(PhaseEndsUtc);
            writer.Write(LastPlayerSeenUtc);
            writer.Write(LastDecayUtc);
            writer.Write(ProtectionUntilUtc);
            writer.Write(RitualLockUntilUtc);
            writer.Write(CampHoldSeconds);
            WriteBoolArray(writer, CampsCleansed);
            WriteIntArray(writer, WardProgressSeconds);
            WriteIntArray(writer, AnchorIntegrity);
            WriteIntArray(writer, AnchorDailyRepair);
            WriteDateArray(writer, AnchorRepairDayUtc);
            writer.Write(HighestParticipants);
            writer.Write(CommanderDead);
            writer.Write(WardenDead);
            WriteIntList(writer, OwnedSerials);
            writer.Write(SuppressedSpawners.Count);

            for (int i = 0; i < SuppressedSpawners.Count; ++i)
                SuppressedSpawners[i].Serialize(writer);
        }

        public static InvasionCityState Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            InvasionCityState state = new InvasionCityState((InvasionCityId)reader.ReadInt());
            state.Enabled = reader.ReadBool();
            state.ValidationPassed = reader.ReadBool();
            state.StaffPaused = reader.ReadBool();
            state.IdlePaused = reader.ReadBool();
            state.State = (InvasionState)reader.ReadInt();
            state.Faction = (InvasionFaction)reader.ReadInt();
            state.Session = reader.ReadInt();
            state.Corruption = reader.ReadInt();
            state.RitualLeaderSerial = reader.ReadInt();
            state.CycleStartedUtc = reader.ReadDateTime();
            state.StateStartedUtc = reader.ReadDateTime();
            state.PhaseEndsUtc = reader.ReadDateTime();
            state.LastPlayerSeenUtc = reader.ReadDateTime();
            state.LastDecayUtc = reader.ReadDateTime();
            state.ProtectionUntilUtc = reader.ReadDateTime();
            state.RitualLockUntilUtc = reader.ReadDateTime();
            state.CampHoldSeconds = reader.ReadInt();
            state.CampsCleansed = ReadBoolArray(reader, 3);
            state.WardProgressSeconds = ReadIntArray(reader, 3, 0);
            state.AnchorIntegrity = ReadIntArray(reader, 3, 100);
            state.AnchorDailyRepair = ReadIntArray(reader, 3, 0);
            state.AnchorRepairDayUtc = ReadDateArray(reader, 3);
            state.HighestParticipants = reader.ReadInt();
            state.CommanderDead = reader.ReadBool();
            state.WardenDead = reader.ReadBool();
            state.OwnedSerials = ReadIntList(reader);
            state.SuppressedSpawners.Clear();
            int spawnerCount = reader.ReadInt();

            for (int i = 0; i < spawnerCount; ++i)
                state.SuppressedSpawners.Add(InvasionSpawnerRecord.Deserialize(reader));

            return state;
        }

        private static void WriteBoolArray(GenericWriter writer, bool[] values)
        {
            writer.Write(values.Length);

            for (int i = 0; i < values.Length; ++i)
                writer.Write(values[i]);
        }

        private static bool[] ReadBoolArray(GenericReader reader, int requiredLength)
        {
            bool[] values = new bool[requiredLength];
            int count = reader.ReadInt();

            for (int i = 0; i < count; ++i)
            {
                bool value = reader.ReadBool();

                if (i < values.Length)
                    values[i] = value;
            }

            return values;
        }

        private static void WriteIntArray(GenericWriter writer, int[] values)
        {
            writer.Write(values.Length);

            for (int i = 0; i < values.Length; ++i)
                writer.Write(values[i]);
        }

        private static int[] ReadIntArray(GenericReader reader, int requiredLength, int defaultValue)
        {
            int[] values = new int[requiredLength];

            for (int i = 0; i < values.Length; ++i)
                values[i] = defaultValue;

            int count = reader.ReadInt();

            for (int i = 0; i < count; ++i)
            {
                int value = reader.ReadInt();

                if (i < values.Length)
                    values[i] = value;
            }

            return values;
        }

        private static void WriteDateArray(GenericWriter writer, DateTime[] values)
        {
            writer.Write(values.Length);

            for (int i = 0; i < values.Length; ++i)
                writer.Write(values[i]);
        }

        private static DateTime[] ReadDateArray(GenericReader reader, int requiredLength)
        {
            DateTime[] values = new DateTime[requiredLength];
            int count = reader.ReadInt();

            for (int i = 0; i < count; ++i)
            {
                DateTime value = reader.ReadDateTime();

                if (i < values.Length)
                    values[i] = value;
            }

            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i] == DateTime.MinValue)
                    values[i] = DateTime.UtcNow.Date;
            }

            return values;
        }

        private static void WriteIntList(GenericWriter writer, List<int> values)
        {
            writer.Write(values.Count);

            for (int i = 0; i < values.Count; ++i)
                writer.Write(values[i]);
        }

        private static List<int> ReadIntList(GenericReader reader)
        {
            int count = reader.ReadInt();
            List<int> values = new List<int>(count);

            for (int i = 0; i < count; ++i)
                values.Add(reader.ReadInt());

            return values;
        }
    }
}
