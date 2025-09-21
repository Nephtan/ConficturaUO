using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded FrailSkeleton variant corresponding to the "Servant" entry from the Dio XML spawner.
    ///     Applies the specified combat stats and equips the skeleton with its unique ribcage armor loot.
    /// </summary>
    public class Servant : FrailSkeleton
    {
        [Constructable]
        public Servant()
            : base()
        {
            // Identity defined in the XML configuration
            Name = "Servant";
            Title = null; // No custom title supplied

            // Health adjustments
            HitsMaxSeed = 100;
            SetHits(100, 100);

            // Damage profile (min/max)
            SetDamage(5, 20);

            // Unique loot: enchanted bone chest piece carried by the servant
            BoneChest ribcage = new BoneChest
            {
                Name = "Undead Ribcage",
                StrRequirement = 0,
                ColdBonus = 66,
                EnergyBonus = 66,
                PoisonBonus = 68,
                MaxHitPoints = 100,
                HitPoints = 100
            };

            PackItem(ribcage);
        }

        public Servant(Serial serial)
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