using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the Surtaz encounter configured as "Vampire Illusion" in the Dio XML spawner.
    ///     Recreates the XML configuration by applying the same statistics and appearance customizations.
    /// </summary>
    public class VampireIllusion : Surtaz
    {
        [Constructable]
        public VampireIllusion()
            : base()
        {
            // Identity pulled from the XML definition
            Name = "Vampire Illusion";
            Title = null; // No custom title specified

            // Visual customization
            Hue = 1462;
            Direction = Direction.North;

            // Health values
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Melee damage output
            SetDamage(100, 160);
        }

        public VampireIllusion(Serial serial)
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