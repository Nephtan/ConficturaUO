using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Lighthouse keeper who provides provisions to travelers in Ordain Ruins.
    ///     Hard codes the XMLSpawner definition for 'Gort' the provisioner.
    /// </summary>
    public class Gort : Provisioner
    {
        [Constructable]
        public Gort()
            : base()
        {
            // Identity
            Name = "Gort";
            Title = "the Lighthouse Keeper";
            Female = false; // Explicitly male per XML definition

            // Orientation
            Direction = Direction.East;

            // Health
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);
        }

        public Gort(Serial serial)
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