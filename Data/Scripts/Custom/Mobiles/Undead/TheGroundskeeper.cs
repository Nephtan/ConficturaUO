using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the frail skeleton that represents "The Groundskeeper" from Mad Duke's Manor.
    ///     Recreates the XMLSpawner configuration for the Groundskeeper encounter.
    /// </summary>
    [CorpseName("a skeletal corpse")]
    public class TheGroundskeeper : FrailSkeleton
    {
        [Constructable]
        public TheGroundskeeper()
            : base()
        {
            // Identity pulled from the XML definition
            Name = "The Groundskeeper";
            Title = null; // No custom title specified

            // Health values
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Melee damage output
            SetDamage(50, 100);
        }

        public TheGroundskeeper(Serial serial)
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