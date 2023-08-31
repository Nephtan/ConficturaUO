using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    [FlipableAttribute(0x9BB, 0x9BC)]
    public class RoastPig : Food
    {
        [Constructable]
        public RoastPig()
            : this(1) { }

        [Constructable]
        public RoastPig(int amount)
            : base(amount, 0x9BB)
        {
            this.Weight = 45.0;
            this.FillFactor = 15;
        }

        public RoastPig(Serial serial)
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

    public class RoastPigNS : Food
    {
        [Constructable]
        public RoastPigNS()
            : this(1) { }

        [Constructable]
        public RoastPigNS(int amount)
            : base(amount)
        {
            this.ItemID = 0x9BC;
        }

        public RoastPigNS(Serial serial)
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

    public class RoastPigEW : RoastPig
    {
        [Constructable]
        public RoastPigEW()
            : this(1) { }

        [Constructable]
        public RoastPigEW(int amount)
            : base(amount)
        {
            this.ItemID = 0x9BB;
        }

        public RoastPigEW(Serial serial)
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
