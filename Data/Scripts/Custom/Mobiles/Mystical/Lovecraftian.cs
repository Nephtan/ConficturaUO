using Server;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hardcoded Kuthulu variant for the Dio Black Temple spawn.
    ///     Generated from the XML spawner definition for "Lovecraftian" in DioXMLSpawns.md.
    /// </summary>
    [CorpseName("a kuthulu corpse")]
    public class Lovecraftian : Kuthulu
    {
        [Constructable]
        public Lovecraftian()
            : base()
        {
            // Identity
            Name = "Lovecraftian";
            Title = null; // No title specified in the XML definition

            // Health and damage profile
            HitsMaxSeed = 6000;
            SetHits(6000, 6000);
            SetDamage(160, 200);

            // Resistances
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Fire, 90, 90);
            SetResistance(ResistanceType.Energy, 90, 90);

            // XML attachments that enhance the creature's melee pressure
            XmlAttach.AttachTo(this, new XmlManaDrain(900));
            XmlAttach.AttachTo(this, new XmlStamDrain(900));
        }

        public Lovecraftian(Serial serial)
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