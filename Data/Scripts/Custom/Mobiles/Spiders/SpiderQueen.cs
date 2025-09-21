using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Matriarch of the drider brood that haunts Wolfgang's crypt.
    ///     Hard codes the XMLSpawner definition for 'driderwizard/name/Spider Queen'.
    /// </summary>
    public class SpiderQueen : DriderWizard
    {
        [Constructable]
        public SpiderQueen()
            : base()
        {
            // Identity
            Name = "Spider Queen";
            Title = null; // No title specified in the spawn definition

            // Health
            HitsMaxSeed = 600;
            SetHits(600, 600);

            // Damage
            SetDamage(10, 50);
        }

        public SpiderQueen(Serial serial)
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