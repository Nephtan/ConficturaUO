using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0x175D, 0x1761)]
    public class Cloth : Item, IScissorable, IDyable, ICommodity
    {
        int ICommodity.DescriptionNumber
        {
            get { return LabelNumber; }
        }
        bool ICommodity.IsDeedable
        {
            get { return true; }
        }

        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        [Constructable]
        public Cloth()
            : this(1) { }

        [Constructable]
        public Cloth(int amount)
            : base(0x175D)
        {
            Stackable = true;
            Amount = amount;
            Name = "cut cloth";
        }

        public Cloth(Serial serial)
            : base(serial) { }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            ItemID = 0x175D;
            Name = "cut cloth";
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            Item cloth = new UncutCloth(Amount);
            cloth.Hue = Hue;
            from.AddToBackpack(cloth);
            from.SendMessage("You fold up the cloth.");
            Delete();
        }

        public override void OnSingleClick(Mobile from)
        {
            int number = (Amount == 1) ? 1049124 : 1049123;

            from.Send(
                new MessageLocalized(
                    Serial,
                    ItemID,
                    MessageType.Regular,
                    0x3B2,
                    3,
                    number,
                    "",
                    Amount.ToString()
                )
            );
        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (Deleted || !from.CanSee(this))
                return false;

            base.ScissorHelper(from, new Bandage(), 1);

            return true;
        }
    }
}
