using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom fire elemental variant representing the Fire Servant encountered in the Time Awaits spawn.
    ///     Hard codes the XMLSpawner definition for 'Fire Servant'.
    /// </summary>
    [CorpseName("an elemental corpse")]
    public class FireServant : FireElemental
    {
        [Constructable]
        public FireServant()
            : base()
        {
            // Identity
            Name = "Fire Servant";
            Title = null; // No title specified in the XML definition

            // Health
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Damage
            SetDamage(90, 100);
        }

        public FireServant(Serial serial)
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