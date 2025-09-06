using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Booty Bay spawn representing a urc bowman.
    ///     Generated from the XML spawner definition for 'UrcBowman' named 'Scallywag'.
    /// </summary>
    [CorpseName("a urcish corpse")]
    public class Scallywag : UrcBowman
    {
        [Constructable]
        public Scallywag()
            : base()
        {
            // Identity
            Name = "Scallywag";
            Title = null; // No title in definition

            // Stats from XML definition
            SetDamage(100, 200);
        }

        public Scallywag(Serial serial)
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
