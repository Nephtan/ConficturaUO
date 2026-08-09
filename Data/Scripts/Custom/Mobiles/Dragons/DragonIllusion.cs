using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Illusory dragon encountered within the Tower of Runes event.
    ///     Hard codes the XMLSpawner definition for 'Dragon Illusion'.
    /// </summary>
    public class DragonIllusion : CaddelliteDragon
    {
        [Constructable]
        public DragonIllusion()
            : base()
        {
            // Identity
            Name = "Dragon Illusion";
            Title = null; // No title specified
            Hue = 1462;

            // Health
            HitsMaxSeed = 5000;
            SetHits(5000, 5000);

            // Damage
            SetDamage(190, 200);

            // Orientation
            Direction = Direction.North;
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public DragonIllusion(Serial serial)
            : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            MobileBalanceCatalog.DropLoot(this, c);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 1)
            {
                MobileBalanceCatalog.ApplyProfile(this);
            }
        }
    }
}
