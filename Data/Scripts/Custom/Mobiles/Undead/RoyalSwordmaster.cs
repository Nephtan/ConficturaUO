using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded Skeletal Samurai champion "Royal Swordmaster" from the Dio XML spawn definition.
    ///     Applies the configured combat stats and ensures the unique swordsmanship loot is packed.
    /// </summary>
    public class RoyalSwordmaster : SkeletalSamurai
    {
        [Constructable]
        public RoyalSwordmaster()
            : base()
        {
            // Identity values sourced from the XML spawner entry
            Name = "Royal Swordmaster";
            Title = null; // No title specified in the XML definition

            // Health and damage overrides
            HitsMaxSeed = 900;
            SetHits(900, 900);
            SetDamage(200, 270);

            // Guarantee the presence of a backpack for delivering custom loot
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Legendary blade reward (ADD/conanssword)
            ConansSword conansSword = new ConansSword();
            pack.DropItem(conansSword);

            // Ceremonial swords display (ADD/honorableswords)
            HonorableSwords honorableSwords = new HonorableSwords();
            pack.DropItem(honorableSwords);

            // Advanced swordsmanship study tome (ADD/advancedswordsstudybook)
            AdvancedSwordsStudyBook studyBook = new AdvancedSwordsStudyBook();
            pack.DropItem(studyBook);
        }

        public RoyalSwordmaster(Serial serial)
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