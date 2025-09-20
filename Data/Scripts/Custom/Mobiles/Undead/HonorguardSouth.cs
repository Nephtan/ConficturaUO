using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the Skeletal Samurai named "Honorguard" that guards Wolfgang's crypt.
    ///     Applies the XML-spawner configuration including health, damage, hue, facing, and custom helm loot.
    /// </summary>
    public class HonorguardSouth : SkeletalSamurai
    {
        [Constructable]
        public HonorguardSouth()
            : base()
        {
            // Identity and appearance
            Name = "Honorguard";
            Title = null; // No title specified in the XML definition
            Hue = 2091;
            Direction = Direction.South;

            // Health and damage values from the XML entry
            HitsMaxSeed = 900;
            SetHits(900, 900);
            SetDamage(100, 200);

            // Ensure the mobile has a backpack for guaranteed loot delivery
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Signature helm defined in the ADD clause
            DragonHelm crown = new DragonHelm
            {
                Name = "Honorguard Crown",
                Hue = 2091,
                StrRequirement = 0,
                MaxHitPoints = 900,
                HitPoints = 900,
                PhysicalBonus = 66,
                ColdBonus = 65,
                FireBonus = 66,
                PoisonBonus = 67,
                EnergyBonus = 65,
                Resource = CraftResource.Gold
            };

            pack.DropItem(crown);
        }

        public HonorguardSouth(Serial serial)
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