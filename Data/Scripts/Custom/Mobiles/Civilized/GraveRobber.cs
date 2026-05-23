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
            ApplyIdentity();

            // Appearance
            Hue = 0;

            // Health
            HitsMaxSeed = 100;
            SetHits(100, 100);

            // Damage
            SetDamage(10, 50);
        }

        private void ApplyIdentity()
        {
            Name = "Grave Robber";
            Title = null; // Title intentionally blank per spawn definition
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            ApplyIdentity();
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
            ApplyIdentity();
        }
    }
}
