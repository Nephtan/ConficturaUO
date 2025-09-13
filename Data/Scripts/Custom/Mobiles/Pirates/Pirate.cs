using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Booty Bay spawn representing a generic pirate.
    ///     Generated from the XML spawner definition for 'Evilpirate'.
    /// </summary>
    [CorpseName("a pirate corpse")]
    public class Pirate : EvilPirate
    {
        [Constructable]
        public Pirate()
            : base()
        {
            // Identity
            Name = "Pirate";
            Title = null; // No title in definition
            Hue = 0;

            // Stats from XML definition
            SetHits(300, 300);
            SetDamage(50, 100);
        }

        public Pirate(Serial serial)
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
