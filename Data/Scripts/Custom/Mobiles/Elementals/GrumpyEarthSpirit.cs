using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom earth elemental from the Elemental Chaos spawn.
    ///     Hard codes the XMLSpawner definition for 'Grumpy Earth Spirit'.
    /// </summary>
    [CorpseName("a goliath corpse")]
    public class GrumpyEarthSpirit : CrystalGoliath
    {
        [Constructable]
        public GrumpyEarthSpirit()
            : base()
        {
            // Identity
            Name = "Grumpy Earth Spirit";
            Title = null; // No title specified

            // Health
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage
            SetDamage(100, 200);
        }

        public GrumpyEarthSpirit(Serial serial)
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
