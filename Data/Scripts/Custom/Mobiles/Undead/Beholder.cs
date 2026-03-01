using System;
using Server;
using Server.Items;

using BaseBeholder = Server.Mobiles.Beholder;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the classic Beholder encounter that appears within Wolfgang's Crypt.
    ///     This version mirrors the XML configuration and provides deterministic loot and stats.
    /// </summary>
    public class Beholder : BaseBeholder
    {
        [Constructable]
        public Beholder()
            : base()
        {
            // Identity values supplied by the XML spawn definition
            Name = "Beholder";
            Title = null; // No title specified in the XML
            Direction = Direction.East;

            // Survivability configuration
            HitsMaxSeed = 500;
            SetHits(500, 500);

            // Melee damage profile
            SetDamage(10, 30);

            // Ensure a backpack exists so loot can be reliably added
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // ADD/magictalisman - grant the magic talisman reward
            pack.DropItem(new MagicTalisman());

            // ADD/<bonelegs/...> - customized bone legs with enhanced properties
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

        public Beholder(Serial serial)
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