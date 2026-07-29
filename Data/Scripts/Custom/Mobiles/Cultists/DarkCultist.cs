using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Mage cultist serving in the Dark Dragon Temple.
    ///     Hard codes the XMLSpawner definition for 'Dark Cultist'.
    /// </summary>
    [CorpseName("a dark cultist corpse")]
    public class DarkCultist : EvilMageLord
    {
        [Constructable]
        public DarkCultist()
            : base()
        {
            // Identity
            Name = "Dark Cultist";
            Title = null; // No title specified
            Direction = Direction.West;

            // Health
            HitsMaxSeed = 400;
            SetHits(400, 400);

            // Damage
            SetDamage(100, 200);
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public DarkCultist(Serial serial)
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
