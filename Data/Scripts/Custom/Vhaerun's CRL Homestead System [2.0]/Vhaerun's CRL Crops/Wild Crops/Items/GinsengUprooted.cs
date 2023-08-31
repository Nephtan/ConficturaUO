using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    [FlipableAttribute(0x18E7, 0x18E8)]
    public class GinsengUprooted : Item, ICarvable
    {
        public void Carve(Mobile from, Item item)
        {
            int count = Utility.Random(3);
            if (count == 0)
            {
                from.SendMessage("You find no useable roots.");
                this.Consume();
            }
            else
            {
                base.ScissorHelper(from, new Ginseng(), count);
                from.SendMessage("You cut {0} root{1}.", count, (count == 1 ? "" : "s"));
            }
        }

        [Constructable]
        public GinsengUprooted()
            : this(1) { }

        [Constructable]
        public GinsengUprooted(int amount)
            : base(Utility.RandomList(0x18EB, 0x18EC))
        {
            Stackable = false;
            Weight = 1.0;

            Movable = true;
            Amount = amount;

            Name = "Uprooted Ginseng Plant";
        }

        public GinsengUprooted(Serial serial)
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
