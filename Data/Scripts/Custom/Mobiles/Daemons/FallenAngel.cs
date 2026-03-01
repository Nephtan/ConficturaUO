using System;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Shadow demon champion from Dio's Black Temple spawn named "Fallen Angel".
    ///     Hard codes the XMLSpawner configuration so the encounter can be maintained via C#.
    /// </summary>
    public class FallenAngel : ShadowDemon
    {
        [Constructable]
        public FallenAngel()
            : base()
        {
            // Identity configuration from the XML spawn definition
            Name = "Fallen Angel";
            Title = null; // No title supplied in the spawn entry

            // Survivability profile
            HitsMaxSeed = 5000;
            SetHits(5000, 5000);

            // Damage output values
            SetDamage(190, 210);

            // Resistance seeds provided by the XML spawner
            PhysicalResistanceSeed = 100;
            ColdResistSeed = 100;
            PoisonResistSeed = 100;
            FireResistSeed = 90;
            EnergyResistSeed = 90;

            // Attach the mana, stamina, and life drain effects defined in the spawner
            XmlAttach.AttachTo(this, new XmlManaDrain(900));
            XmlAttach.AttachTo(this, new XmlStamDrain(90));
            XmlAttach.AttachTo(this, new XmlLifeDrain(100));
        }

        public FallenAngel(Serial serial)
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