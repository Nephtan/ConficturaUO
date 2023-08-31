using System;
using Server.Network;

namespace Server.Items
{
    public class PumpkinLarge : Food
    {
        [Constructable]
        public PumpkinLarge()
            : this(1) { }

        [Constructable]
        public PumpkinLarge(int amount)
            : base(amount, 0x54DE)
        {
            Name = "large pumpkin";
            this.Weight = 10.0;
            this.FillFactor = 10;
        }

        public PumpkinLarge(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PumpkinTall : Food
    {
        [Constructable]
        public PumpkinTall()
            : this(1) { }

        [Constructable]
        public PumpkinTall(int amount)
            : base(amount, 0x5498)
        {
            Name = "tall pumpkin";
            this.Weight = 10.0;
            this.FillFactor = 10;
        }

        public PumpkinTall(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PumpkinGreen : Food
    {
        [Constructable]
        public PumpkinGreen()
            : this(1) { }

        [Constructable]
        public PumpkinGreen(int amount)
            : base(amount, 0x54E0)
        {
            Name = "green pumpkin";
            this.Weight = 10.0;
            this.FillFactor = 10;
        }

        public PumpkinGreen(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PumpkinGiant : Food
    {
        [Constructable]
        public PumpkinGiant()
            : this(1) { }

        [Constructable]
        public PumpkinGiant(int amount)
            : base(amount, 0x54DF)
        {
            Name = "giant pumpkin";
            this.Weight = 100.0;
            this.FillFactor = 20;
        }

        public PumpkinGiant(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallPumpkin : Food
    {
        [Constructable]
        public SmallPumpkin()
            : this(1) { }

        [Constructable]
        public SmallPumpkin(int amount)
            : base(amount, 0xC6C)
        {
            this.Weight = 1.0;
            this.FillFactor = 8;
        }

        public SmallPumpkin(Serial serial)
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
