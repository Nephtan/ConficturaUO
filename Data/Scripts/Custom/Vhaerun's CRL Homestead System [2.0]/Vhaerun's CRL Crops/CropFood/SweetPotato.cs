using System;
using Server.Network;

namespace Server.Items
{
    public class SweetPotato : Food
    {
        [Constructable]
        public SweetPotato()
            : this(1) { }

        [Constructable]
        public SweetPotato(int amount)
            : base(amount, 0xC64)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Name = "Sweet Potato";
            this.Hue = 0x45E;
        }

        public SweetPotato(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
