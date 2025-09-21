using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded version of the "Tomb Warden" bone golem defined in the Wolfgang's Crypt XML spawn.
    ///     Applies the stat and behavior overrides listed in DioXMLSpawns.md for the BoneGolem entry.
    /// </summary>
    public class TombWarden : BoneGolem
    {
        [Constructable]
        public TombWarden()
            : base()
        {
            // Identity configuration
            Name = "Tomb Warden";
            Title = null; // No title specified in the source definition

            // Survivability configuration
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Damage profile taken from the XML definition
            SetDamage(10, 50);

            // Behavior overrides
            RangePerception = 3;
            Direction = Direction.South;
        }

        public TombWarden(Serial serial)
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