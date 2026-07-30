using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for Wolfgang's Crypt representing the skeletal dragon named "Wolfgang's Dragon".
    ///     Generated from the XML spawner definition for the skeletaldragon entry in DioXMLSpawns.md.
    /// </summary>
    public class WolfgangsDragon : SkeletalDragon
    {
        [Constructable]
        public WolfgangsDragon()
            : base()
        {
            // Identity
            Name = "Wolfgang's Dragon";
            Title = null; // No title specified
            Hue = 2091;
            Direction = Direction.East;

            // Health
            HitsMaxSeed = 9000;
            SetHits(9000, 9000);

            // Damage
            SetDamage(190, 200);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public WolfgangsDragon(Serial serial)
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
