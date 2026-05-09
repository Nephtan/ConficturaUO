using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Royal sphinx champion guarding Wolfgang's Crypt.
    ///     Hard codes the XMLSpawner definition for the "Eternal Guardian" entry.
    /// </summary>
    [CorpseName("a sphinx corpse")]
    public class EternalGuardian : RoyalSphinx
    {
        [Constructable]
        public EternalGuardian()
            : base()
        {
            // Identity configuration
            Name = "Eternal Guardian";
            Title = null; // No title provided in the XML entry

            // Survivability values from the spawn definition
            HitsMaxSeed = 5000;
            SetHits(5000, 5000);

            // Damage output configuration
            SetDamage(200, 270);

            // Resistance seeds supplied by the spawn definition
            FireResistSeed = 100;
            EnergyResistSeed = 100;

            // Ensure the creature has a backpack to contain its custom loot
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Signature wing cosmetic item
            DemonWings wings = new DemonWings
            {
                Name = "Wings of the Guardian",
                StrRequirement = 0,
                Hue = 0
            };

            pack.DropItem(wings);

            // Magical dyes carried by the guardian
            MagicalDyes dyes = new MagicalDyes();
            pack.DropItem(dyes);

            // Pandora's Box reward item
            PandorasBox pandorasBox = new PandorasBox();
            pack.DropItem(pandorasBox);
        }

        public EternalGuardian(Serial serial)
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