using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom lava giant from the Elemental Chaos spawn.
    ///     Hard codes the XMLSpawner definition for 'Fire King'.
    /// </summary>
    [CorpseName("a giant corpse")]
    public class FireKing : LavaGiant
    {
        [Constructable]
        public FireKing()
            : base()
        {
            // Identity
            Name = "Fire King";
            Title = null; // No title specified
            Hue = 1161;
            Direction = Direction.East;

            // Health
            HitsMaxSeed = 3000;
            SetHits(3000, 3000);

            // Damage
            SetDamage(200, 250);

            // Unique loot
            PackItem(new GodSmithing());
        }

        public FireKing(Serial serial)
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
