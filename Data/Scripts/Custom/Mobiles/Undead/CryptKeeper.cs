using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Hard-coded variant of the ZombieMage that represents the "Crypt Keeper" encounter
    ///     defined within the Wolfgang's Crypt XML spawn configuration.
    /// </summary>
    public class CryptKeeper : ZombieMage
    {
        [Constructable]
        public CryptKeeper()
            : base()
        {
            // Identity copied from the XML definition
            Name = "Crypt Keeper";
            Title = null; // No custom title provided

            // Health configuration
            HitsMaxSeed = 200;
            SetHits(200, 200);

            // Melee damage output
            SetDamage(10, 50);

            // Guarantee a backpack so loot can be delivered reliably
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            // Unique bone helm granted by the ADD directive
            BoneHelm boneHelm = new BoneHelm
            {
                Name = "Dead Head",
                MaxHitPoints = 100,
                HitPoints = 100,
                StrRequirement = 0
            };

            pack.DropItem(boneHelm);
        }

        public CryptKeeper(Serial serial)
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