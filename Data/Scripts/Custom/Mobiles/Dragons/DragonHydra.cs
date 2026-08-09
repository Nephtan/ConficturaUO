using System;
using Server;
using Server.Custom.Confictura.PvE.MobileBalance;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded DragonTurtle variant generated from the Dio XML spawn entry for "Dragon Hydra".
    ///     Applies the stat, resistance, facing, and attachment overrides defined in DioXMLSpawns.md.
    /// </summary>
    public class DragonHydra : DragonTurtle
    {
        [Constructable]
        public DragonHydra()
            : base()
        {
            // Identity configuration from the XML definition
            Name = "Dragon Hydra";
            Title = null; // No title specified in the XML entry
            Direction = Direction.South;
            Tamable = false;

            // Health pool overrides
            HitsMaxSeed = 7000;
            SetHits(7000, 7000);

            // Damage profile overrides
            SetDamage(200, 220);

            // Resistance tuning
            SetResistance(ResistanceType.Physical, 90, 90);
            SetResistance(ResistanceType.Fire, 100, 100);

            // Attachments defined via ATTACH directives in the XML entry
            XmlAttach.AttachTo(this, new XmlStamDrain(900));
            XmlAttach.AttachTo(this, new XmlManaDrain(90));
            MobileBalanceCatalog.ApplyProfile(this);
        }

        public DragonHydra(Serial serial)
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

            Tamable = false;
        }
    }
}
