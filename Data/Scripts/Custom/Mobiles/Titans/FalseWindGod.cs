using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     False Wind God variant of Titan Stratos from the Tower of Runes XML spawner.
    ///     Hard codes the XMLSpawner definition for "False Wind God".
    /// </summary>
    public class FalseWindGod : TitanStratos
    {
        [Constructable]
        public FalseWindGod()
            : base()
        {
            // Identity
            Name = "False Wind God";
            Title = null; // No title specified in the XML definition
            Hue = 1462;
            Direction = Direction.East;

            // Health pool
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Damage range
            SetDamage(100, 200);
        }

        public FalseWindGod(Serial serial)
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