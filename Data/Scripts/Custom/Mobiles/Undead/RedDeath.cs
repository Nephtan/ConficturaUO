using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the FrailSkeleton referenced as "Red Death" in the Dio XML spawn list.
    ///     Applies the XML-specified combat statistics and visual hue to match the original encounter.
    /// </summary>
    public class RedDeath : FrailSkeleton
    {
        [Constructable]
        public RedDeath()
            : base()
        {
            // Identity fields from the XML definition
            Name = "Red Death";
            Title = null; // No title specified in the spawn definition

            // Increased survivability values
            HitsMaxSeed = 150;
            SetHits(150, 150);

            // Custom melee damage range
            SetDamage(50, 100);

            // Apply the designated appearance hue
            Hue = 38;
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public RedDeath(Serial serial)
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
