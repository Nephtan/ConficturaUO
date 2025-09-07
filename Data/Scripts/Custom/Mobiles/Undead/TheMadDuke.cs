using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded version of the "The Mad Duke" rotting corpse from XML spawner.
    /// </summary>
    [CorpseName("the mad duke's corpse")]
    public class TheMadDuke : RottingCorpse
    {
        [Constructable]
        public TheMadDuke()
            : base()
        {
            // Basic identity
            Name = "The Mad Duke";
            Title = null; // No title per XML definition

            // Health pool
            SetHits(1000, 1000);

            // Melee damage output
            SetDamage(150, 200);
        }

        public TheMadDuke(Serial serial)
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
