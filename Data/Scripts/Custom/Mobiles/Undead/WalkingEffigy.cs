using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded version of the Rotting Minotaur variant "Walking Effigy" from the Wolfgang's Crypt spawn.
    /// </summary>
    public class WalkingEffigy : RottingMinotaur
    {
        [Constructable]
        public WalkingEffigy()
            : base()
        {
            // Identity
            Name = "Walking Effigy";
            Title = null; // No title provided in the XML definition

            // Survivability
            HitsMaxSeed = 900;
            SetHits(900, 900);

            // Melee damage profile
            SetDamage(10, 90);
        }

        public WalkingEffigy(Serial serial)
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