using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Mobiles
{
    /// <summary>
    ///     Creepy witch vendor haunting Wolfgang's Crypt.
    ///     Hard codes the XMLSpawner definition for 'Siddo'.
    /// </summary>
    public class Siddo : Witches
    {
        [Constructable]
        public Siddo()
            : base()
        {
            // Identity and appearance
            Name = "Siddo";
            Title = "the Creepy Merchant";
            BodyValue = 253;
            Hue = 0;
            Direction = Direction.South;

            // Durability profile
            HitsMaxSeed = 1000;
            SetHits(1000, 1000);

            // Combat output
            SetDamage(110, 150);

            // Resistances provided in the XML definition
            FireResistSeed = 100;
            ColdResistSeed = 100;
            PoisonResistSeed = 100;

            // Signature treat carried by the vendor
            ChocolateMonster chocolate = new ChocolateMonster();
            PackItem(chocolate);
        }

        public Siddo(Serial serial)
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