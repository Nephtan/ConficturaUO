using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the Surtaz encounter configured as "Vampire Illusion" in the Dio XML spawner.
    ///     Recreates the XML configuration by applying the same statistics and appearance customizations.
    /// </summary>
    public class VampireIllusion : Surtaz
    {
        [Constructable]
        public VampireIllusion()
            : base()
        {
            // Identity pulled from the XML definition
            Name = "Vampire Illusion";
            Title = null; // No custom title specified

            // Visual customization
            Hue = 1462;
            Direction = Direction.North;

            // Health values
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Melee damage output
            SetDamage(100, 160);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public VampireIllusion(Serial serial)
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
