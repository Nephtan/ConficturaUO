using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom elementalist from the Elemental Chaos spawn.
    ///     Hard codes the XMLSpawner definition for 'Geode the Earth Spirit'.
    /// </summary>
    public class Geode : Elementalist
    {
        [Constructable]
        public Geode()
            : base()
        {
            // Identity
            Name = "Geode";
            Title = "the Earth Spirit";
            Hue = 0;
            BodyValue = 753;
            BaseSoundID = 268;
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 9000;
            SetHits(9000, 9000);

            // Damage
            SetDamage(100, 200);
        }

        public Geode(Serial serial)
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
