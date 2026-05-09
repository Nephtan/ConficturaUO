using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Mighty titan guardian stationed in the Underground Temple.
    ///     Hard codes the XMLSpawner definition for 'Herculian'.
    /// </summary>
    public class Herculian : Titan
    {
        [Constructable]
        public Herculian()
            : base()
        {
            // Identity
            Name = "Herculian";
            Title = "the Titan";
            Direction = Direction.West;

            // Health
            HitsMaxSeed = 4000;
            SetHits(4000, 4000);

            // Damage
            SetDamage(250, 270);
        }

        public Herculian(Serial serial)
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