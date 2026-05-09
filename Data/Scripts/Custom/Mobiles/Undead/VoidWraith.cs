using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded Shroud variant "Void Wraith" generated from the Dio XML spawn definition.
    ///     Applies the specified statistics and attaches its mana/stamina drain auras.
    /// </summary>
    public class VoidWraith : Shroud
    {
        [Constructable]
        public VoidWraith()
            : base()
        {
            // Identity supplied by the XML configuration
            Name = "Void Wraith";
            Title = null; // No custom title defined

            // Durability adjustments
            HitsMaxSeed = 600;
            SetHits(600, 600);

            // Melee damage output (min/max)
            SetDamage(150, 180);

            // Resistances from the spawn data
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Fire, 90, 90);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 90, 90);

            // Attach XML-based mana and stamina drain effects
            XmlAttach.AttachTo(this, new XmlManaDrain(900));
            XmlAttach.AttachTo(this, new XmlStamDrain(90));
        }

        public VoidWraith(Serial serial)
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