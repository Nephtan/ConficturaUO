using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura
{
    // A lizardman-themed monster nest for low-to-mid world placement and AI testing fodder.
    public class LizardmanNest : MonsterNest
    {
        [Constructable]
        public LizardmanNest()
            : base()
        {
            Name = "Lizardman Nest";
            ItemID = 9956;
            Hue = 0;
            m_MaxCount = 30;
            m_RespawnTime = TimeSpan.FromSeconds(3.0);
            HitsMax = 5000;
            Hits = 5000;
            RangeHome = 20;
            m_LootLevel = 1;

            // Initialize the combat entity after the final health values are set.
            InitializeMonsterNestEntity();
        }

        // Weighted spawn pool: 50% Lizardman, 30% LizardmanArcher, 20% Reptalar.
        protected override BaseCreature CreateSpawnMobile()
        {
            switch (Utility.Random(10))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return new Lizardman();
                case 5:
                case 6:
                case 7:
                    return new LizardmanArcher();
                default:
                    return new Reptalar();
            }
        }

        public LizardmanNest(Serial serial)
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
