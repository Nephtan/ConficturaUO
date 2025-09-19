using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Ordain Ruins spawn representing the kobold shaman leader.
    ///     Generated from the XML spawner definition for 'KoboldMage' named 'Kobold Chief'.
    /// </summary>
    [CorpseName("a kobold corpse")]
    public class KoboldChief : Server.Mobiles.KoboldMage
    {
        [Constructable]
        public KoboldChief()
            : base()
        {
            // Identity
            Name = "Kobold Chief";
            Title = null; // No title specified

            // Health
            HitsMaxSeed = 400;
            SetHits(400, 400);
        }

        public KoboldChief(Serial serial)
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