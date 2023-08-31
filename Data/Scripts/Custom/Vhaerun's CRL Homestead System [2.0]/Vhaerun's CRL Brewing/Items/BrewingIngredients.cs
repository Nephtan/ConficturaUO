using System;

namespace Server.Items
{
    public class Malt : Item, ICommodity
    {
        int ICommodity.DescriptionNumber
        {
            get { return LabelNumber; }
        }

        // Add the missing IsDeedable property
        bool ICommodity.IsDeedable
        {
            get { return true; }
        }

        [Constructable]
        public Malt()
            : this(1) { }

        [Constructable]
        public Malt(int amount)
            : base(0x103D)
        {
            Name = "Malt";
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }

        public Malt(Serial serial)
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

    public class Yeast : Item
    {
        [Constructable]
        public Yeast()
            : this(1) { }

        [Constructable]
        public Yeast(int amount)
            : base(0x1039)
        {
            Name = "Yeast";
            Stackable = true;
            Weight = 6.0;
            Amount = amount;
        }

        public Yeast(Serial serial)
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

    public class Barley : Item, ICommodity
    {
        int ICommodity.DescriptionNumber
        {
            get { return LabelNumber; }
        }

        // Add the missing IsDeedable property
        bool ICommodity.IsDeedable
        {
            get { return true; }
        }

        [Constructable]
        public Barley()
            : this(1) { }

        [Constructable]
        public Barley(int amount)
            : base(0x103F)
        {
            Name = "Barley";
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }

        public Barley(Serial serial)
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
