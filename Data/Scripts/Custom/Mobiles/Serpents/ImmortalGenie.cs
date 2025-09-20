using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Custom serpyn sorceress variant representing the Immortal Genie spawn.
    ///     Hard codes the XMLSpawner definition for 'Immortal Genie'.
    /// </summary>
    [CorpseName("a serpyn corpse")]
    public class ImmortalGenie : SerpynSorceress
    {
        [Constructable]
        public ImmortalGenie()
            : base()
        {
            // Identity
            Name = "Immortal Genie";
            Title = "of the vase";

            // Health
            HitsMaxSeed = 900;
            SetHits(900, 900);

            // Damage output
            SetDamage(150, 200);

            // Resistances
            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Fire, 100, 100);
            SetResistance(ResistanceType.Cold, 100, 100);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 100, 100);

            // Magical protection
            MagicDamageAbsorb = 100;

            // Ensure the creature has a backpack to receive its custom loot.
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Pandora's Box reward item
            PandorasBox pandorasBox = new PandorasBox();
            pack.DropItem(pandorasBox);

            // Ornate stone vase tied to the genie
            StoneOrnateVase stoneVase = new StoneOrnateVase();
            pack.DropItem(stoneVase);
        }

        public ImmortalGenie(Serial serial)
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