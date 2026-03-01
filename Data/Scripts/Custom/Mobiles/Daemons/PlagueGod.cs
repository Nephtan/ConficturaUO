using System;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Elite plague beast lord variant named "Plague God" from the Black Temple spawn.
    ///     Applies the XMLSpawner configuration, including resistances and draining attachments.
    /// </summary>
    public class PlagueGod : PlagueBeastLord
    {
        [Constructable]
        public PlagueGod()
            : base()
        {
            // Identity values from the spawn definition
            Name = "Plague God";
            Title = null; // The original XML entry did not specify a title

            // Health pool adjustments
            HitsMaxSeed = 5000;
            SetHits(5000, 5000);

            // Damage output specified by the XML definition
            SetDamage(100, 120);

            // Resistance configuration
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Fire, 50, 50);
            SetResistance(ResistanceType.Energy, 90, 90);

            // Attachments to replicate mana and stamina drain effects from the spawn
            XmlAttach.AttachTo(this, new XmlManaDrain(90));
            XmlAttach.AttachTo(this, new XmlStamDrain(900));
        }

        public PlagueGod(Serial serial)
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
        }
    }
}