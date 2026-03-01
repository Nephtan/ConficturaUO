using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Imp variant from Wolfgang's Crypt known as the "Tomb Pest".
    ///     Hard codes the XML spawner definition, ensuring its stats and loot.
    /// </summary>
    public class TombPest : Imp
    {
        [Constructable]
        public TombPest()
            : base()
        {
            // Identity
            Name = "Tomb Pest";
            Title = null; // No title specified in the definition

            // Health
            HitsMaxSeed = 10;
            SetHits(10, 10);

            // Damage
            SetDamage(10, 50);

            // Ensure the arcane gem loot is always present
            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();
                AddItem(pack);
            }

            ArcaneGem arcaneGem = new ArcaneGem();
            pack.DropItem(arcaneGem);
        }

        public TombPest(Serial serial)
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