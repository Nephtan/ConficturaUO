using System; // Line 1
using System.Collections;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

// Namespace declaration aligns with Server.Items to extend the game's item functionalities.
namespace Server.Items
{
    // Defines a MonsterNest item, which acts as a spawner for creatures within the game.
    public class MonsterNest : Item
    {
        // Fields to hold the nest's configuration.
        public string m_NestSpawnType; // Specifies the type of creature to spawn.
        public int m_MaxCount; // The maximum number of creatures the nest can spawn.
        public TimeSpan m_RespawnTime; // The interval between spawn attempts.
        public ArrayList m_Spawn; // Tracks the creatures spawned by this nest.
        public int m_HitsMax; // The maximum health of the nest.
        public int m_Hits; // The current health of the nest.
        public int m_RangeHome; // The maximum distance spawned creatures can wander from the nest.
        public DateTime m_Attackable; // Unused: potentially could track when the nest becomes attackable.
        public int m_LootLevel; // Influences the quality of loot dropped when the nest is destroyed.
        public Mobile m_Entity; // A mobile entity that represents the nest in combat scenarios.

        // Properties expose the nest's configurable attributes to the server's administration tools.
        // Allows game masters to adjust the nest's settings in-game.
        [CommandProperty(AccessLevel.GameMaster)]
        public string NestSpawnType
        {
            get { return m_NestSpawnType; }
            set { m_NestSpawnType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RespawnTime
        {
            get { return m_RespawnTime; }
            set { m_RespawnTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMax
        {
            get { return m_HitsMax; }
            set { m_HitsMax = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hits
        {
            get { return m_Hits; }
            set { m_Hits = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeHome
        {
            get { return m_RangeHome; }
            set { m_RangeHome = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LootLevel
        {
            get { return m_LootLevel; }
            set { m_LootLevel = value; }
        }

        // Constructor sets up the nest with default properties and initializes timers for spawning and regeneration.
        [Constructable]
        public MonsterNest()
            : base(4962) // Base constructor call sets the item ID to represent the nest visually.
        {
            // Initialization of fields with default values.
            m_NestSpawnType = null;
            Name = "Monster Nest"; // Default name for the nest.
            m_Spawn = new ArrayList(); // Initialize the list to track spawned creatures.
            Weight = 0.1; // Set a nominal weight for the item.
            Hue = 1818; // Default color for the nest.
            HitsMax = 300; // Set default maximum health.
            Hits = 300; // Initialize current health to max.
            RangeHome = 10; // Default wander range for spawned creatures.
            RespawnTime = TimeSpan.FromSeconds(15.0); // Set the default respawn time for creatures.

            // Start the timers for spawning creatures and regenerating health.
            new InternalTimer(this).Start();
            new RegenTimer(this).Start();

            // Spawn entity to represent the nest in combat.
            Movable = false; // Ensure the nest cannot be moved.
        }

        public void UpdateEntityHealth()
        {
            if (m_Entity != null)
            {
                m_Entity.Hits = this.Hits;
            }
        }

        protected void InitializeMonsterNestEntity()
        {
            if (m_Entity == null)
            {
                m_Entity = new MonsterNestEntity(this);
                m_Entity.MoveToWorld(this.Location, this.Map);
            }
            UpdateEntityHealth();
        }

        // Cleanup logic to ensure spawned creatures and the combat entity are properly removed when the nest is deleted.
        public override void OnDelete()
        {
            base.OnDelete(); // Call base deletion logic.

            // Delete all spawned creatures.
            if (m_Spawn != null && m_Spawn.Count > 0)
            {
                foreach (Mobile m in m_Spawn)
                {
                    m.Delete(); // Remove the creature from the world.
                }
            }

            // Delete the combat entity associated with the nest.
            if (m_Entity != null)
                m_Entity.Delete();
        }

        // Allows the nest to take damage and potentially be destroyed if health drops to zero.
        public void Damage(int damage)
        {
            this.Hits -= damage; // Subtract damage from the nest's health.
            this.PublicOverheadMessage(MessageType.Regular, 0x22, false, damage.ToString()); // Show damage feedback.

            if (this.Hits <= 0)
                this.Destroy(); // Destroy the nest if health is depleted.
        }

        // Initiates combat with the nest when it is double-clicked by a player.
        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage(0, "You begin to attack the object."); // Feedback message.

            if (m_Entity != null)
                from.Combatant = m_Entity; // Set the player's target to the nest's combat entity.
        }

        // Adds loot to the game world at the nest's location upon its destruction.
        public virtual void AddLoot()
        {
            MonsterNestLoot loot = new MonsterNestLoot(
                6585,
                this.Hue,
                this.m_LootLevel,
                "Monster Nest remains"
            );
            loot.MoveToWorld(this.Location, this.Map);
        }

        // Handles the destruction of the nest, killing all spawned monsters and dropping loot.
        public void Destroy()
        {
            AddLoot(); // Calls the AddLoot method to generate and place loot at the nest's location.

            // Iterate through the list of spawned creatures and kill each one.
            if (m_Spawn != null && m_Spawn.Count > 0)
            {
                for (int i = 0; i < this.m_Spawn.Count; i++)
                {
                    Mobile m = (Mobile)this.m_Spawn[i];
                    m.Kill(); // Kills the creature, triggering any death effects or loot drops.
                }
            }

            // Delete the combat entity associated with the nest, if it exists.
            if (this.m_Entity != null)
                this.m_Entity.Delete();

            this.Delete(); // Finally, delete the nest itself from the world.
        }

        // Counts the number of alive monsters spawned by the nest.
        public int Count()
        {
            int c = 0; // Initialize counter to zero.

            // Loop through each spawned creature to check if it is alive.
            if (this.m_Spawn != null && this.m_Spawn.Count > 0)
            {
                for (int i = 0; i < this.m_Spawn.Count; i++)
                {
                    Mobile m = (Mobile)this.m_Spawn[i];

                    if (m.Alive)
                        c += 1; // Increment counter for each alive creature.
                }
            }

            return c; // Return the total count of alive creatures.
        }

        // Attempts to spawn a new creature based on the nest's configuration.
        public void DoSpawn()
        {
            // Ensure the combat entity is positioned at the nest's location, if it exists.
            if (this.m_Entity != null)
                this.m_Entity.MoveToWorld(this.Location, this.Map);

            // Check conditions: spawn type is set, spawn list exists, and current count is below max allowed.
            if (
                this.NestSpawnType != null
                && this.m_Spawn != null
                && this.Count() < this.m_MaxCount
            )
            {
                try
                {
                    Type type = SpawnerType.GetType(this.NestSpawnType); // Get the Type object for the specified spawn.
                    object o = Activator.CreateInstance(type); // Attempt to create an instance of the specified type.

                    if (o != null && o is Mobile)
                    {
                        Mobile c = o as Mobile;
                        if (c is BaseCreature)
                        {
                            BaseCreature m = o as BaseCreature; // Cast the object to BaseCreature if applicable.
                            this.m_Spawn.Add(m); // Add the creature to the spawn list.
                            m.OnBeforeSpawn(this.Location, this.Map); // Invoke any pre-spawn actions.
                            m.MoveToWorld(this.Location, this.Map); // Place the creature in the world at the nest's location.
                            m.Home = this.Location; // Set the creature's home location to the nest's location.
                            m.RangeHome = this.m_RangeHome; // Set the creature's wander range.
                        }
                    }
                }
                catch
                {
                    this.NestSpawnType = null; // Reset spawn type if there was an error during spawning.
                }
            }
        }

        // Constructor for deserialization. Required for saving and loading nest objects from data files.
        public MonsterNest(Serial serial)
            : base(serial) { }

        // Serializes the MonsterNest object's state, allowing it to be saved to a data file.
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class Serialize method.

            writer.Write((int)0); // Version number for future compatibility.
            writer.Write((string)m_NestSpawnType);
            writer.WriteMobileList(m_Spawn); // Serialize the list of spawned creatures.
            writer.Write((int)m_MaxCount);
            writer.Write((TimeSpan)m_RespawnTime);
            writer.Write((int)m_HitsMax);
            writer.Write((int)m_Hits);
            writer.Write((int)m_RangeHome);
            writer.Write((int)m_LootLevel);
            writer.Write((Mobile)m_Entity); // Serialize the combat entity.
        }

        // Deserializes the MonsterNest object's state, allowing it to be loaded from a data file.
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class Deserialize method.

            int version = reader.ReadInt(); // Version number for future compatibility.
            m_NestSpawnType = reader.ReadString();
            m_Spawn = reader.ReadMobileList();
            m_MaxCount = reader.ReadInt();
            m_RespawnTime = reader.ReadTimeSpan();
            m_HitsMax = reader.ReadInt();
            m_Hits = reader.ReadInt();
            m_RangeHome = reader.ReadInt();
            m_LootLevel = reader.ReadInt();
            m_Entity = reader.ReadMobile();
        }

        // RegenTimer handles periodic regeneration of the nest's hit points.
        private class RegenTimer : Timer
        {
            private MonsterNest nest; // Reference to the nest object.

            public RegenTimer(MonsterNest n)
                : base(TimeSpan.FromMinutes(1.0)) // Set the timer to tick every minute.
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    nest.Hits += nest.HitsMax / 10; // Regenerate 10% of the nest's max hit points.

                    // Ensure hit points do not exceed maximum.
                    if (nest.Hits > nest.HitsMax)
                        nest.Hits = nest.HitsMax;

                    new RegenTimer(nest).Start(); // Restart the timer for continuous regeneration.
                }
            }
        }

        // InternalTimer handles the periodic spawning of creatures from the nest.
        private class InternalTimer : Timer
        {
            private MonsterNest nest; // Reference to the nest object.

            public InternalTimer(MonsterNest n)
                : base(n.RespawnTime) // Timer interval is based on the nest's respawn time.
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    nest.DoSpawn(); // Attempt to spawn a creature.
                    new InternalTimer(nest).Start(); // Restart the timer to continue spawning.
                }
            }
        }
    }
}
