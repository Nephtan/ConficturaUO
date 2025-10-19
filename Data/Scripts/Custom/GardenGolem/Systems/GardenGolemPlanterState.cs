using System;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Custom.Confictura.GardenGolems.Systems
{
    /// <summary>
    ///     Lightweight container that mirrors the values tracked by <see cref="PlantSystem" />
    ///     for use inside the mobile planter implementation attached to <see cref="GardenGolems.Mobiles.GardenGolem" />.
    ///     The goal is to provide a persistable state bag that can be extended in future iterations without
    ///     pulling the entire gardening code into the creature.
    /// </summary>
    public class GardenGolemPlanterState
    {
        public const int MaxMoisture = 4;
        public const int MaxInfestation = 4;

        private string m_SeedTypeName;
        private PlantType m_SeedPlantType;
        private PlantHue m_SeedPlantHue;

        private TimeSpan m_GrowthInterval;
        private DateTime m_NextGrowth;

        private int m_StoredYields;
        private int m_Moisture;
        private int m_Infestation;
        private int m_FertilityBonus;

        /// <summary>
        ///     Initializes the planter with defaults that mirror a newly planted bowl of dirt: moist soil and no maladies.
        /// </summary>
        public GardenGolemPlanterState()
        {
            m_GrowthInterval = TimeSpan.FromHours(12.0);
            m_NextGrowth = DateTime.UtcNow + m_GrowthInterval;
            m_Moisture = MaxMoisture / 2;
            m_Infestation = 0;
            m_FertilityBonus = 0;
        }

        /// <summary>
        ///     Gets whether the planter currently has an assigned seed or produce specimen.
        /// </summary>
        public bool HasSeed
        {
            get { return !string.IsNullOrEmpty(m_SeedTypeName); }
        }

        /// <summary>
        ///     Gets the plant type currently cultivated by the golem.
        /// </summary>
        public PlantType SeedPlantType
        {
            get { return m_SeedPlantType; }
        }

        /// <summary>
        ///     Gets the plant hue associated with the cultivated crop.
        /// </summary>
        public PlantHue SeedPlantHue
        {
            get { return m_SeedPlantHue; }
        }

        /// <summary>
        ///     Gets a human readable summary for UI display.
        /// </summary>
        public string DisplaySeedLabel
        {
            get
            {
                if (!HasSeed)
                    return "None";

                PlantHueInfo hueInfo = PlantHueInfo.GetInfo(m_SeedPlantHue);
				
                if (hueInfo == null)
                    return m_SeedPlantType.ToString();

                return string.Format("{0} ({1})", m_SeedPlantType, hueInfo.PlantHue);
            }
        }

        /// <summary>
        ///     Gets whether the planter currently holds a yield that can be harvested by the caretaker.
        /// </summary>
        public bool HasReadyYield
        {
            get { return HasSeed && m_StoredYields > 0; }
        }

        /// <summary>
        ///     Gets the timestamp at which the next growth check will occur.
        /// </summary>
        public DateTime NextGrowth
        {
            get { return m_NextGrowth; }
        }

        /// <summary>
        ///     Current moisture level. Values above zero permit growth ticks while zero represents dry soil.
        /// </summary>
        public int Moisture
        {
            get { return m_Moisture; }
        }

        /// <summary>
        ///     Simple infestation counter used to gate harvests until caretakers perform treatments.
        /// </summary>
        public int Infestation
        {
            get { return m_Infestation; }
        }

        /// <summary>
        ///     Exposes the number of stored harvests waiting to be collected by the caretaker.
        /// </summary>
        public int StoredYields
        {
            get { return m_StoredYields; }
        }

        /// <summary>
        ///     Gets the currently applied fertility bonus multiplier from treatments.
        /// </summary>
        public int FertilityBonus
        {
            get { return m_FertilityBonus; }
        }

        /// <summary>
        ///     Adjusts the tracked moisture level, clamping the value within the accepted range.
        /// </summary>
        public void Water()
        {
            m_Moisture = MaxMoisture;
        }

        /// <summary>
        ///     Removes pests and fungus indicators. Future iterations can add granular counters if desired.
        /// </summary>
        public void TreatInfestation()
        {
            if (m_Infestation > 0)
                m_Infestation = 0;
        }

        /// <summary>
        ///     Configures the planter to track a seed or produce specimen supplied by a player.
        /// </summary>
        public void LoadSeed(Seed seed)
        {
            if (seed == null)
                throw new ArgumentNullException("seed");

            m_SeedTypeName = seed.GetType().AssemblyQualifiedName;
            m_SeedPlantType = seed.PlantType;
            m_SeedPlantHue = seed.PlantHue;

            ResetGrowthTimers();
            m_StoredYields = 0;
            m_Moisture = MaxMoisture;
            m_Infestation = 0;
        }

        /// <summary>
        ///     Clears the planter, typically when a caretaker ejects the seed or the golem dies.
        /// </summary>
        public void ClearSeed()
        {
            m_SeedTypeName = null;
            m_StoredYields = 0;
            m_FertilityBonus = 0;
            ResetGrowthTimers();
        }

        /// <summary>
        ///     Applies a small, persistent fertility bonus representing alchemical treatments.
        /// </summary>
        public void ApplyFertilityBoost(int amount)
        {
            m_FertilityBonus = Math.Max(0, Math.Min(3, m_FertilityBonus + amount));
        }

        /// <summary>
        ///     Performs the lightweight growth tick executed by the golem AI loop. Returns true when a new harvest becomes ready.
        /// </summary>
        public bool Tick(DateTime now)
        {
            if (!HasSeed)
                return false;

            if (m_Moisture <= 0)
                return false;

            if (m_NextGrowth > now)
                return false;

            m_NextGrowth = now + m_GrowthInterval;
            m_Moisture = Math.Max(0, m_Moisture - 1);

            // If the planter has been neglected we accumulate infestation as a warning mechanic.
            if (m_Moisture <= 1 && m_Infestation < MaxInfestation)
                m_Infestation++;

            // Cap the amount of stored yields to avoid runaway duplication.
            if (m_StoredYields < 3)
            {
                int increment = 1 + m_FertilityBonus;
                m_StoredYields = Math.Min(3, m_StoredYields + increment);
            }

            return m_StoredYields > 0;
        }

        /// <summary>
        ///     Attempts to create a harvestable item using the gardening tables. Returns null when the crop is not configured for resources.
        /// </summary>
        public Item Harvest()
        {
            if (!HasReadyYield)
                return null;

            if (m_Infestation >= MaxInfestation)
                return null;

            Item harvested = null;

            PlantResourceInfo resourceInfo = PlantResourceInfo.GetInfo(m_SeedPlantType, m_SeedPlantHue);

            if (resourceInfo != null)
            {
                harvested = resourceInfo.CreateResource();
            }
            else
            {
                // Fall back to handing the caretaker a fresh seed of the cultivated type.
                harvested = new Seed(m_SeedPlantType, m_SeedPlantHue, true);
            }

            m_StoredYields = 0;
            ResetGrowthTimers();

            return harvested;
        }

        /// <summary>
        ///     Serializes the planter state to the provided writer.
        /// </summary>
        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)1); // version

            writer.Write(m_SeedTypeName);
            writer.Write((int)m_SeedPlantType);
            writer.Write((int)m_SeedPlantHue);

            writer.Write(m_GrowthInterval);
            writer.Write(m_NextGrowth);

            writer.Write(m_StoredYields);
            writer.Write(m_Moisture);
            writer.Write(m_Infestation);
            writer.Write(m_FertilityBonus);
        }

        /// <summary>
        ///     Restores state from the provided reader.
        /// </summary>
        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            m_SeedTypeName = reader.ReadString();
            m_SeedPlantType = (PlantType)reader.ReadInt();
            m_SeedPlantHue = (PlantHue)reader.ReadInt();

            m_GrowthInterval = reader.ReadTimeSpan();
            m_NextGrowth = reader.ReadDateTime();

            m_StoredYields = reader.ReadInt();
            m_Moisture = reader.ReadInt();
            m_Infestation = reader.ReadInt();
            m_FertilityBonus = reader.ReadInt();

            if (version < 1)
                m_GrowthInterval = TimeSpan.FromHours(12.0);
        }

        private void ResetGrowthTimers()
        {
            m_NextGrowth = DateTime.UtcNow + m_GrowthInterval;
        }
    }
}