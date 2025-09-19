using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Ranger loyalist stationed near Britain.
    ///     Hard codes the XMLSpawner definition for 'ranger/title/of the Britain Militia'.
    /// </summary>
    public class RangerOfTheBritainMilitia : Ranger
    {
        [Constructable]
        public RangerOfTheBritainMilitia()
            : base()
        {
            // Identity
            Title = "of the Britain Militia";

            // Health
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Damage
            SetDamage(50, 150);
        }

        public RangerOfTheBritainMilitia(Serial serial)
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