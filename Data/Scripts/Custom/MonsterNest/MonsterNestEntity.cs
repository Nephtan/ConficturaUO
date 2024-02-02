using System; // Line 1
using System.Collections;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

// Define the namespace as part of the Server.Items to align with the RunUO server's architecture.
namespace Server.Mobiles
{
	// MonsterNestEntity inherits from BaseCreature, making it an interactable entity in the game world.
	public class MonsterNestEntity : BaseCreature
	{
		// Holds a reference to the MonsterNest object that this entity represents.
		private Item m_MonsterNest;

		// Constructor for creating a new MonsterNestEntity instance.
		// It is marked as constructable to enable instantiation within the game world.
		[Constructable]
		public MonsterNestEntity(MonsterNest nest)
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4) // Initializes AI and combat parameters.
		{
			m_MonsterNest = nest; // Associate this entity with its corresponding MonsterNest.
			Name = nest.Name; // Inherits the name from the MonsterNest.
			Title = ""; // No title is assigned to keep the entity's display name clean.
			Body = 399; // Sets the entity's visual appearance using a predefined model.
			BaseSoundID = 0; // No sound is associated, making this entity silent.
			this.Hue = 0; // Default coloration.

			// Stats are intentionally set to 0 as this entity should not move or act on its own.
			SetStr(0);
			SetDex(0);
			SetInt(0);

			SetHits(nest.HitsMax); // Set the entity's health with the nest's maximum health.

			SetDamage(0, 0); // No damage is dealt by the entity, as it's not intended to fight.

			// Resistance settings are all set to zero, indicating no special resistances.
			SetResistance(ResistanceType.Physical, 0);
			SetResistance(ResistanceType.Fire, 0);
			SetResistance(ResistanceType.Cold, 0);
			SetResistance(ResistanceType.Poison, 0);
			SetResistance(ResistanceType.Energy, 0);

			Fame = 5000; // Fame and Karma are set for potential use in player interactions, though typically unused.
			Karma = -5000;

			VirtualArmor = 0; // No armor since it doesn't engage in combat.
			CantWalk = true; // Prevents the entity from moving.
		}

		// Overrides the base class's OnThink method to customize behavior.
		// This ensures the entity is always located at the MonsterNest's position and inherits its health state.
		public override void OnThink()
		{
			this.Frozen = true; // Ensures the entity does not move or act.
			this.Location = this.m_MonsterNest.Location; // Sync location with the MonsterNest.

			if (this.m_MonsterNest != null && this.m_MonsterNest is MonsterNest)
			{
				MonsterNest nest = this.m_MonsterNest as MonsterNest;
				this.Hits = nest.Hits; // Sync health with the MonsterNest.
			}
		}

		// Custom handler for when the entity takes damage.
		// Redirects the damage to the associated MonsterNest, allowing for integrated health management.
		public override void OnDamage(int amount, Mobile from, bool willkill)
		{
			if (this.m_MonsterNest != null && this.m_MonsterNest is MonsterNest)
			{
				MonsterNest nest = this.m_MonsterNest as MonsterNest;
				nest.Damage(amount); // Direct damage to the MonsterNest instead of this entity.
			}
			base.OnDamage(amount, from, willkill);
		}

		// Sound-related methods return a placeholder value as this entity does not produce sounds.
		public override int GetAngerSound()
		{
			return 9999;
		}

		public override int GetIdleSound()
		{
			return 9999;
		}

		public override int GetAttackSound()
		{
			return 9999;
		}

		public override int GetHurtSound()
		{
			return 9999;
		}

		public override int GetDeathSound()
		{
			return 9999;
		}

		// Prevents the entity from dying through normal means, as its life cycle is directly tied to the MonsterNest's state.
		public override bool OnBeforeDeath()
		{
			return false;
		}

		// Serialization constructor and methods for saving and loading the entity's state.
		// These are crucial for persistent world states across server restarts.
		public MonsterNestEntity(Serial serial)
			: base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer); // Call the base method to handle common serialization.
			writer.Write((int)0); // Versioning for future updates.
			writer.Write((Item)m_MonsterNest); // Serialize the reference to the MonsterNest.
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader); // Call the base method to handle common deserialization.
			int version = reader.ReadInt(); // Versioning for future updates.
			m_MonsterNest = reader.ReadItem(); // Deserialize the reference to the MonsterNest.
		}
	}
}