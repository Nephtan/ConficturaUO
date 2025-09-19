using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Agile spiderling from the Time Awaits encounter.
    ///     Hard codes the XMLSpawner definition for 'Spiderling'.
    /// </summary>
    public class Spiderling : GiantSpider
    {
        [Constructable]
        public Spiderling()
            : base()
        {
            // Identity
            Name = "Spiderling";
            Title = null; // No title provided in the XML definition

            // Health
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Damage
            SetDamage(100, 150);
        }

        public Spiderling(Serial serial)
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