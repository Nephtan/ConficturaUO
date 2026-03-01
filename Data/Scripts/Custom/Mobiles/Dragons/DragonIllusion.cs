using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Illusory dragon encountered within the Tower of Runes event.
    ///     Hard codes the XMLSpawner definition for 'Dragon Illusion'.
    /// </summary>
    public class DragonIllusion : CaddelliteDragon
    {
        [Constructable]
        public DragonIllusion()
            : base()
        {
            // Identity
            Name = "Dragon Illusion";
            Title = null; // No title specified
            Hue = 1462;

            // Health
            HitsMaxSeed = 5000;
            SetHits(5000, 5000);

            // Damage
            SetDamage(190, 200);

            // Orientation
            Direction = Direction.North;
        }

        public DragonIllusion(Serial serial)
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