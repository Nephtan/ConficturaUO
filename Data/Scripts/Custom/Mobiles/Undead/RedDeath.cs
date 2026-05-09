using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the FrailSkeleton referenced as "Red Death" in the Dio XML spawn list.
    ///     Applies the XML-specified combat statistics and visual hue to match the original encounter.
    /// </summary>
    public class RedDeath : FrailSkeleton
    {
        [Constructable]
        public RedDeath()
            : base()
        {
            // Identity fields from the XML definition
            Name = "Red Death";
            Title = null; // No title specified in the spawn definition

            // Increased survivability values
            HitsMaxSeed = 150;
            SetHits(150, 150);

            // Custom melee damage range
            SetDamage(50, 100);

            // Apply the designated appearance hue
            Hue = 38;
        }

        public RedDeath(Serial serial)
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