using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Archangel champion stationed within the Underground Temple.
    ///     Hard codes the XMLSpawner definition for 'True Virtue'.
    /// </summary>
    public class TrueVirtue : Archangel
    {
        [Constructable]
        public TrueVirtue()
            : base()
        {
            // Identity
            Name = "True Virtue";
            Title = null; // No title specified in the XML definition
            Direction = Direction.South;

            // Health
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage
            SetDamage(240, 290);

            // Combat behaviour
            FightMode = FightMode.Closest;
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public TrueVirtue(Serial serial)
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
