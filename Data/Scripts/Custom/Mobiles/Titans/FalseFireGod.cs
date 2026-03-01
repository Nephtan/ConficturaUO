using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom titan illusion from the Tower of Runes spawn.
    ///     Hard codes the XMLSpawner definition for 'False Fire God'.
    /// </summary>
    public class FalseFireGod : TitanPyros
    {
        [Constructable]
        public FalseFireGod()
            : base()
        {
            // Identity
            Name = "False Fire God";
            Title = null; // No title specified in the XML definition
            Hue = 1462;
            Direction = Direction.West;

            // Health
            HitsMaxSeed = 2000;
            SetHits(2000, 2000);

            // Damage
            SetDamage(100, 300);
        }

        public FalseFireGod(Serial serial)
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