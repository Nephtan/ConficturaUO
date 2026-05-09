using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Cultist stationed within the Ordain Ruins lighthouse.
    ///     Hard codes the XMLSpawner definition for 'Dark Dragon Cultist'.
    /// </summary>
    public class DarkDragonCultist : EvilMageLord
    {
        [Constructable]
        public DarkDragonCultist()
            : base()
        {
            // Identity information
            Name = "Dark Dragon Cultist";
            Title = null; // No title specified in the XML definition

            // Health configuration
            HitsMaxSeed = 300;
            SetHits(300, 300);

            // Damage output
            SetDamage(40, 50);

            // Visual appearance
            Hue = 0;
        }

        public DarkDragonCultist(Serial serial)
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