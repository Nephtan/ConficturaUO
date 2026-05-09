using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Ordain Ruins spawn representing a sturdier kobold foot soldier.
    ///     Generated from the XML spawner definition for 'Kobold'.
    /// </summary>
    [CorpseName("a kobold corpse")]
    public class KoboldFootSoldier : Server.Mobiles.Kobold
    {
        [Constructable]
        public KoboldFootSoldier()
            : base()
        {
            // Identity
            Name = "Kobold";
            Title = null; // No title specified

            // Health
            HitsMaxSeed = 100;
            SetHits(100, 100);

            // Damage
            SetDamage(10, 30);
        }

        public KoboldFootSoldier(Serial serial)
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