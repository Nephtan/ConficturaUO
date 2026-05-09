using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom baby dragon companion for the Time Awaits spawn.
    ///     Hard codes the XMLSpawner definition for 'Wyrmling'.
    /// </summary>
    [CorpseName("a baby dragon corpse")]
    public class Wyrmling : BabyDragon
    {
        [Constructable]
        public Wyrmling()
            : base()
        {
            // Identity
            Name = "Wyrmling";
            Title = null; // No title specified
            Hue = 1462;

            // Health
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Damage
            SetDamage(100, 150);
        }

        public Wyrmling(Serial serial)
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