using System; // Line 1
using System.Collections;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class UndeadNest : MonsterNest
	{
		[Constructable]
		public UndeadNest() : base()
		{
			this.Name = "Undead Nest";
			this.ItemID = 13335;
			this.Hue = 1573;
			this.m_NestSpawnType = "Undead";
			this.m_MaxCount = 30;
			this.m_RespawnTime = TimeSpan.FromSeconds(3.0);
			this.HitsMax = 1000;
			this.Hits = 1000;
			this.RangeHome = 20;
			this.m_LootLevel = 1;

			// Ensure the MonsterNestEntity is initialized with the updated health values.
			InitializeMonsterNestEntity();
		}

		public UndeadNest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // Versioning in case we need to change the data structure later.
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
