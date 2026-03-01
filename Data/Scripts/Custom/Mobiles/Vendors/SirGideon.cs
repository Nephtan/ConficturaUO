using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Elite chivalry vendor stationed within the Underground Temple.
    ///     Hard codes the XMLSpawner definition for 'Sir Gideon'.
    /// </summary>
    public class SirGideon : KeeperOfChivalry
    {
        [Constructable]
        public SirGideon()
            : base()
        {
            // Identity
            Name = "Sir Gideon";
            Title = null; // No title specified in the spawn definition
            Female = false; // Explicitly male per XML data

            // Orientation
            Direction = Direction.East;

            // Combat capability
            SetDamage(200, 250);

            // Health
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);
        }

        public SirGideon(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}