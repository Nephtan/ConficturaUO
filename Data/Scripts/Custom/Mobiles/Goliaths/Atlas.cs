using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the AncientCyclops configured as "Atlas" in the Dio XML spawn data.
    ///     Applies the XML-defined statistics and loot so the encounter no longer depends on XMLSpawner.
    /// </summary>
    public class Atlas : AncientCyclops
    {
        [Constructable]
        public Atlas()
            : base()
        {
            // Identity and presentation defined by the XML entry
            Name = "Atlas";
            Title = null; // Empty title field in the XML definition

            // Survivability adjustments
            HitsMaxSeed = 3000;
            SetHits(3000, 3000);

            // Melee damage configuration
            SetDamage(100, 270);

            // Currency rewards mapped from cointype/coins
            CoinType = "jewels";
            Coins = 9000;
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public Atlas(Serial serial)
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
