using Server.Items;

namespace Server.Custom.Confictura.Items
{
    /// <summary>
    ///     Bag of nutrient rich dirt dropped by garden golems. Acts as a crafting component for alchemical constructs.
    /// </summary>
    public class FreshGardenSoil : Item
    {
        public override string DefaultName
        {
            get { return "fresh garden soil"; }
        }

        [Constructable]
        public FreshGardenSoil()
            : this(1)
        {
        }

        [Constructable]
        public FreshGardenSoil(int amount)
            : base(0xF81)
        {
            Stackable = true;
            Weight = 1.0;
            Hue = 0x8A5;
            Amount = amount;
        }

        public FreshGardenSoil(Serial serial)
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