using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded version of the "Evil Druid" undead caster from the Wolfgang's Crypt spawn.
    ///     Converts the XML spawner configuration into a concrete <see cref="UndeadDruid"/> subclass.
    /// </summary>
    public class EvilDruid : UndeadDruid
    {
        [Constructable]
        public EvilDruid()
            : base()
        {
            // Identity
            Name = "Evil Druid";
            Title = null; // No title specified in the XML definition

            // Health
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Damage
            SetDamage(10, 50);
        }

        public EvilDruid(Serial serial)
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