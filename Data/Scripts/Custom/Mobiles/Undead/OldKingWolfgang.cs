using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded form of the "Old King Wolfgang" skeletal knight defined in the Dio XML spawner.
    ///     Applies the stat overrides from the XML configuration for consistent in-game behaviour.
    /// </summary>
    public class OldKingWolfgang : SkeletalKnight
    {
        [Constructable]
        public OldKingWolfgang()
            : base()
        {
            // Identity settings
            Name = "Old King Wolfgang";
            Title = null; // No title specified in the XML definition
            Hue = 2091;
            BodyValue = 768;
            Direction = Direction.South;

            // Health pool configuration
            HitsMaxSeed = 4000;
            SetHits(4000, 4000);

            // High melee damage values from the XML definition
            SetDamage(210, 250);
        }

        public OldKingWolfgang(Serial serial)
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