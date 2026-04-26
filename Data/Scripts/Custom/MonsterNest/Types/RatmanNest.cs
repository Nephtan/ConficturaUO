using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    // A ratman-themed monster nest for low-to-mid world placement and AI testing fodder.
    public class RatmanNest : MonsterNest
    {
        [Constructable]
        public RatmanNest()
            : base()
        {
            Name = "Ratman Nest";
            ItemID = 13335;
            Hue = 1573;
            m_MaxCount = 30;
            m_RespawnTime = TimeSpan.FromSeconds(3.0);
            HitsMax = 5000;
            Hits = 5000;
            RangeHome = 20;
            m_LootLevel = 1;

            // Initialize the combat entity after the final health values are set.
            InitializeMonsterNestEntity();
        }

        // Weighted spawn pool: 40% Ratman, 40% RatmanArcher, 20% RatmanMage.
        protected override BaseCreature CreateSpawnMobile()
        {
            switch (Utility.Random(5))
            {
                case 0:
                case 1:
                    return new Ratman();
                case 2:
                case 3:
                    return new RatmanArcher();
                default:
                    return new RatmanMage();
            }
        }

        public RatmanNest(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
