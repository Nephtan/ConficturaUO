using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the Skeletal Samurai named "Honorguard" that guards Wolfgang's crypt.
    ///     Applies the XML-spawner configuration including health, damage, hue, facing, and custom helm loot.
    /// </summary>
    public class Honorguard : SkeletalSamurai
    {
        private const Direction DefaultDirection = Direction.South;

        private static readonly Dictionary<Point3D, Direction> DirectionOverrides = new Dictionary<Point3D, Direction>()
        {
            // The east-facing sentry positioned at (5963, 2578, 0)
            { new Point3D(5963, 2578, 0), Direction.East }
        };

        [Constructable]
        public Honorguard()
            : base()
        {
            // Identity and appearance
            Name = "Honorguard";
            Title = null; // No title specified in the XML definition
            Hue = 2091;
            Direction = DefaultDirection;

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

        public Honorguard(Serial serial)
            : base(serial)
        {
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            ApplyDirectionalOverride();
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

        /// <summary>
        ///     Applies any coordinate-specific facing overrides, defaulting to south when none are found.
        /// </summary>
        private void ApplyDirectionalOverride()
        {
            Direction facing;

            if (DirectionOverrides.TryGetValue(Location, out facing))
            {
                Direction = facing;
            }
            else
            {
                Direction = DefaultDirection;
            }
        }
    }
} 
