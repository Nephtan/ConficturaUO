// Raspberries originally by Kajuk
// http://www.runuo.com/forum/viewtopic.php?t=27660

using System;
using Server.Network;

namespace Server.Items
{
    public class RedRaspberry : Food
    {
        [Constructable]
        public RedRaspberry()
            : this(1)
        {
            Weight = 0.5;
            Hue = 0x26;
            Name = "red raspberry";
        }

        [Constructable]
        public RedRaspberry(int amount)
            : base(amount, 0x9D1)
        {
            this.Weight = 0.5;
            this.FillFactor = 2;
            this.Hue = 0x26;
            this.Name = "Red Raspberry";
        }

        public RedRaspberry(Serial serial)
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

    public class BlackRaspberry : Food
    {
        [Constructable]
        public BlackRaspberry()
            : this(1)
        {
            Weight = 0.5;
            Hue = 1175;
            Name = "black raspberry";
        }

        [Constructable]
        public BlackRaspberry(int amount)
            : base(amount, 0x9D1)
        {
            this.Weight = 0.5;
            this.FillFactor = 2;
            this.Hue = 1175;
            this.Name = "Black Raspberry";
        }

        public BlackRaspberry(Serial serial)
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
