using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the AncientCyclops defined as "Cyclops Intruder" in the Dio XML spawn data.
    ///     Applies the XML-configured statistics so the spawn behaves consistently without XML dependencies.
    /// </summary>
    public class CyclopsIntruder : AncientCyclops
    {
        [Constructable]
        public CyclopsIntruder()
            : base()
        {
            // Identity pulled from the XML definition
            Name = "Cyclops Intruder";
            Title = null; // The XML entry specifies an empty title

            // Health adjustments supplied by hitsmaxseed/hits entries
            HitsMaxSeed = 700;
            SetHits(700, 700);

            // Melee damage range configured through damagemin/damagemax
            SetDamage(10, 90);
        }

        public CyclopsIntruder(Serial serial)
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