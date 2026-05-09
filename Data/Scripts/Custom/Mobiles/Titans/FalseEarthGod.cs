using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom titan from the Tower of Runes spawn.
    ///     Hard codes the XMLSpawner definition for 'False Earth God'.
    /// </summary>
    [CorpseName("a titan corpse")]
    public class FalseEarthGod : TitanLithos
    {
        [Constructable]
        public FalseEarthGod()
            : base()
        {
            // Identity
            Name = "False Earth God";
            Title = null; // No title specified
            Hue = 1462;
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage
            SetDamage(100, 200);
        }

        public FalseEarthGod(Serial serial)
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