using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    [FlipableAttribute(0x18DD, 0x18DE)]
    public class MandrakeUprooted : Item, ICarvable
    {
        public void Carve(Mobile from, Item item)
        {
            int count = Utility.Random(3);
            if (count == 0)
            {
                from.SendMessage("You find no useable pieces of root.");
                this.Consume();
            }
            else
            {
                base.ScissorHelper(from, new MandrakeRoot(), count);
                from.SendMessage("You cut {0} root{1}.", count, (count == 1 ? "" : "s"));
            }
        }

        [Constructable]
        public MandrakeUprooted()
            : this(1) { }

        [Constructable]
        public MandrakeUprooted(int amount)
            : base(Utility.RandomList(0x18DD, 0x18DE))
        {
            Stackable = false;
            Weight = 1.0;

            Movable = true;
            Amount = amount;

            Name = "Uprooted Mandrake Plant";
        }

        public MandrakeUprooted(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
