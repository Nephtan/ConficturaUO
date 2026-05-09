using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     High priest stationed within the Underground Temple.
    ///     Hard codes the XMLSpawner definition for 'priest/name/Malachi/title/the Priest'.
    /// </summary>
    public class Malachi : Priest
    {
        [Constructable]
        public Malachi()
            : base()
        {
            // Identity
            Name = "Malachi";
            Title = "the Priest";
            Female = false;

            // Orientation
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 2000;
            SetHits(2000, 2000);

            // Damage
            SetDamage(200, 250);
        }

        public Malachi(Serial serial)
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