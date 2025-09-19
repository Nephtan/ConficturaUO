using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded elite variant of the DeadKnight defined in the Tower of Runes spawn.
    ///     Applies the XML-provided statistics and equips the configured Runeblade reward.
    /// </summary>
    public class RuneKnight : DeadKnight
    {
        [Constructable]
        public RuneKnight()
            : base()
        {
            // Identity derived from the XML entry
            Name = "Rune Knight";
            Title = null; // No title specified in the spawn definition

            // Survivability adjustments
            HitsMaxSeed = 2000;
            SetHits(2000, 2000);

            // Increased melee damage output
            SetDamage(240, 250);

            // Ensure the creature has a backpack before adding loot
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Custom Runeblade defined by the XML ADD entry
            RoyalSword runeblade = new RoyalSword
            {
                Name = "Runeblade",
                MinDamage = 20,
                MaxDamage = 50,
                Speed = 1.5,
                StrRequirement = 150,
                MaxHitPoints = 500,
                HitPoints = 500,
                Hue = 1150
            };

            pack.DropItem(runeblade);
        }

        public RuneKnight(Serial serial)
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