using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded implementation of the "Dark Dragon Avatar" primeval black dragon from Dio's The Black Temple spawn.
    ///     Applies the stat, resistance, attachment, and loot overrides specified in the DioXMLSpawns.md configuration.
    /// </summary>
    public class DarkDragonAvatar : PrimevalBlackDragon
    {
        [Constructable]
        public DarkDragonAvatar()
            : base()
        {
            // Identity configuration from the XML definition
            Name = "Dark Dragon Avatar";
            Title = null; // No title value provided
            Hue = 1590;

            // Survivability overrides
            HitsMaxSeed = 9000;
            SetHits(9000, 9000);

            // Damage output overrides
            SetDamage(260, 290);

            // Resistance seeds supplied by the XML entry
            PhysicalResistanceSeed = 100;
            ColdResistSeed = 100;
            PoisonResistSeed = 100;
            FireResistSeed = 90;
            EnergyResistSeed = 90;

            // Ensure attachments that drain vital resources when the dragon strikes
            XmlAttach.AttachTo(this, new XmlManaDrain(900));
            XmlAttach.AttachTo(this, new XmlStamDrain(900));
            XmlAttach.AttachTo(this, new XmlLifeDrain(70));

            // Ensure the creature carries its unique reward trophy
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            StatueJormungandr trophy = new StatueJormungandr
            {
                Name = "Dark Dragon Trophy",
                Weight = 0
            };

            pack.DropItem(trophy);
        }

        public DarkDragonAvatar(Serial serial)
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