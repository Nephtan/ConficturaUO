using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Cerberus variant stationed as the Guardian of the Underworld.
    ///     Derived from the XML spawner definition to hardcode its custom stats and loot type.
    /// </summary>
    public class GuardianOfTheUnderworld : Cerberus
    {
        [Constructable]
        public GuardianOfTheUnderworld()
            : base()
        {
            // Identity
            Name = "Guardian of the Underworld";
            Title = null; // No title specified in the definition

            // Health
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Damage
            SetDamage(100, 180);

            // Loot & mechanics
            CoinType = "jewels";
            Tamable = false;
        }

        public GuardianOfTheUnderworld(Serial serial)
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