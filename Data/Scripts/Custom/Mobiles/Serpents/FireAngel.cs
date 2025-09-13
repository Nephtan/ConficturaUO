using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Serpyn sorceress infused with fire magic.
    ///     Hard codes the XMLSpawner definition for 'Fire Angel'.
    /// </summary>
    [CorpseName("a serpyn corpse")]
    public class FireAngel : SerpynSorceress
    {
        [Constructable]
        public FireAngel()
            : base()
        {
            // Identity
            Name = "Fire Angel";
            Title = null; // No title specified
            Hue = 1161;

            // Health
            HitsMaxSeed = 900;
            SetHits(900, 900);

            // Damage
            SetDamage(250, 290);
        }

        public FireAngel(Serial serial)
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
