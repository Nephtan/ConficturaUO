using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Arachnar apparition stationed within the Tower of Runes.
    ///     Hard codes the XMLSpawner definition for 'Spider Illusion'.
    /// </summary>
    [CorpseName("a spider illusion corpse")]
    public class SpiderIllusion : Arachnar
    {
        [Constructable]
        public SpiderIllusion()
            : base()
        {
            // Identity
            Name = "Spider Illusion";
            Title = null; // No title specified
            Hue = 1462;
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 2000;
            SetHits(2000, 2000);

            // Damage
            SetDamage(140, 200);
        }

        public SpiderIllusion(Serial serial)
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