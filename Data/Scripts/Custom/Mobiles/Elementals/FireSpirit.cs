using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom fire elemental from the Elemental Chaos spawn.
    ///     Hard codes the XMLSpawner definition for 'Fire Spirit'.
    /// </summary>
    [CorpseName("an elemental corpse")]
    public class FireSpirit : FireElemental
    {
        [Constructable]
        public FireSpirit()
            : base()
        {
            // Identity
            Name = "Fire Spirit";
            Title = null; // No title specified
            Hue = 1161;

            // Health
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Damage
            SetDamage(200, 250);
        }

        public FireSpirit(Serial serial)
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
