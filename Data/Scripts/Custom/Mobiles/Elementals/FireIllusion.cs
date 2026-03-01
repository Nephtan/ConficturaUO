using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom Vulcrum variant representing the Fire Illusion encounter in the Tower of Runes.
    ///     Hard codes the XMLSpawner definition for 'Fire Illusion'.
    /// </summary>
    [CorpseName("Vulcrum's corpse")]
    public class FireIllusion : Vulcrum
    {
        [Constructable]
        public FireIllusion()
            : base()
        {
            // Identity
            Name = "Fire Illusion";
            Title = null; // No title specified
            Hue = 1462;
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Damage
            SetDamage(190, 260);
        }

        public FireIllusion(Serial serial)
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