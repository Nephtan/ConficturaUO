using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom Titan Hydros variant from the Tower of Runes spawn.
    ///     Hard codes the XMLSpawner definition for 'False Sea God'.
    /// </summary>
    public class FalseSeaGod : TitanHydros
    {
        [Constructable]
        public FalseSeaGod()
            : base()
        {
            // Identity
            Name = "False Sea God";
            Title = null; // No title specified in XML definition
            Direction = Direction.North;

            // Health
            HitsMaxSeed = 3000;
            SetHits(3000, 3000);

            // Damage
            SetDamage(90, 150);
        }

        public FalseSeaGod(Serial serial)
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