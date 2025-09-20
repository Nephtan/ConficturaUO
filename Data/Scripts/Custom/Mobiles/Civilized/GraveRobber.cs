using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Grave robbing rogue that appears in Wolfgang's crypt.
    ///     Hard codes the XMLSpawner definition for 'rogue/name/Grave Robber'.
    /// </summary>
    public class GraveRobber : Rogue
    {
        [Constructable]
        public GraveRobber()
            : base()
        {
            // Identity
            Name = "Grave Robber";
            Title = string.Empty; // Title intentionally blank per spawn definition

            // Appearance
            Hue = 0;

            // Health
            HitsMaxSeed = 100;
            SetHits(100, 100);

            // Damage
            SetDamage(10, 50);
        }

        public GraveRobber(Serial serial)
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