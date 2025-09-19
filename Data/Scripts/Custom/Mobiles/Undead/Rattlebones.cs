using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded champion form of the SkeletalKnight used for the "Rattlebones" XML spawn.
    ///     Applies the XML-defined statistics and guarantees the specified loot bundle.
    /// </summary>
    public class Rattlebones : SkeletalKnight
    {
        [Constructable]
        public Rattlebones()
            : base()
        {
            // Identity values from the XML definition
            Name = "Rattlebones";
            Title = null; // No title specified in the spawn definition

            // Increased health over the stock SkeletalKnight
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Configured melee damage range
            SetDamage(8, 30);

            // Ensure loot is packed even if the creature lacks a backpack by default
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Recall rune from the XML ADD entry
            RecallRune recallRune = new RecallRune();
            pack.DropItem(recallRune);

            // Bundle of blank scrolls defined by the amount parameter
            BlankScroll blankScrolls = new BlankScroll(20);
            pack.DropItem(blankScrolls);

            // Greater cure potion loot drop
            GreaterCurePotion curePotion = new GreaterCurePotion();
            pack.DropItem(curePotion);
        }

        public Rattlebones(Serial serial)
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