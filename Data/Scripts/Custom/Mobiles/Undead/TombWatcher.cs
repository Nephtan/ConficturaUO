using System;
using Server;
using Server.Items;

using BaseBeholder = Server.Mobiles.Beholder;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded implementation of the Tomb Watcher encounter from the Dio XML spawn table.
    ///     The mobile inherits from the classic Beholder and reproduces the configured stats and loot.
    /// </summary>
    public class TombWatcher : BaseBeholder
    {
        [Constructable]
        public TombWatcher()
            : base()
        {
            // Identity configuration supplied by the XML data.
            Name = "Tomb Watcher";
            Title = null; // No title string in the XML definition.
            Direction = Direction.East;

            // Survivability configuration.
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Melee damage profile.
            SetDamage(10, 30);

            // Ensure the creature has a backpack so the configured loot can be granted reliably.
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // ADD/magictalisman - drop the requested talisman reward.
            pack.DropItem(new MagicTalisman());

            // ADD/<bonelegs/...> - customized bone legs with enhanced stats.
            BoneLegs boneLegs = new BoneLegs
            {
                Name = "Dead Legs",
                MaxHitPoints = 200,
                HitPoints = 200,
                PhysicalBonus = 67,
                StrRequirement = 0
            };

            pack.DropItem(boneLegs);
        }

        public TombWatcher(Serial serial)
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