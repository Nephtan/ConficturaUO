using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded mobile for the Castle Dio spawn representing a watcher variant named "Dio's Monster".
    ///     Generated from the XML spawner definition in DioXMLSpawns.md.
    /// </summary>
    [CorpseName("a watcher corpse")]
    public class DiosMonster : Watcher
    {
        [Constructable]
        public DiosMonster()
            : base()
        {
            // Identity
            Name = "Dio's Monster";
            Title = null; // No title specified

            // Health
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage
            SetDamage(200, 280);

            // Resistances
            SetResistance(ResistanceType.Energy, 100, 100);

            // Not tamable
            Tamable = false;
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public DiosMonster(Serial serial)
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
