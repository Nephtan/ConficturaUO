using System;

namespace Server.Items
{
    public class TailorGuildCoin : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        [Constructable]
        public TailorGuildCoin()
            : this(1) { }

        [Constructable]
        public TailorGuildCoin(int amountFrom, int amountTo)
            : this(Utility.RandomMinMax(amountFrom, amountTo)) { }

        [Constructable]
        public TailorGuildCoin(int amount)
            : base(0xEF0)
        {
            Name = "Tailor Guild Coin";
            Hue = 0x483;
            Stackable = true;
            Amount = amount;
            Light = LightType.Circle150;
        }

        public TailorGuildCoin(Serial serial)
            : base(serial) { }

        public override int GetDropSound()
        {
            if (Amount <= 1)
                return 0x2E4;
            else if (Amount <= 5)
                return 0x2E5;
            else
                return 0x2E6;
        }

        protected override void OnAmountChange(int oldValue)
        {
            int newValue = this.Amount;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Light = LightType.Circle150;
            Name = "Tailor Guild Coin";
            Hue = 0x483;
        }
    }    
}
