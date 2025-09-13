using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Apprentice mage cultist found within the Dark Dragon Temple.
    ///     Hard codes the XMLSpawner definition for 'Dark Cultist Apprentice'.
    /// </summary>
    [CorpseName("a dark cultist apprentice corpse")]
    public class DarkCultistApprentice : EvilMageLord
    {
        [Constructable]
        public DarkCultistApprentice()
            : base()
        {
            // Identity
            Name = "Dark Cultist Apprentice";
            Title = null; // No title specified
            Direction = Direction.West;

            // Health
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Damage
            SetDamage(100, 200);
        }

        public DarkCultistApprentice(Serial serial)
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
