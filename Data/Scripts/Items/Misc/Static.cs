using System;
using Server;

namespace Server.Items
{
    public class Static : Item
    {
        public Static()
            : base(0x80)
        {
            Movable = false;
        }

        [Constructable]
        public Static(int itemID)
            : base(itemID)
        {
            Movable = false;
        }

        [Constructable]
        public Static(int itemID, int count)
            : this(Utility.Random(itemID, count)) { }

        public Static(Serial serial)
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

            if (version == 0 && Weight == 0)
                Weight = -1;
        }
    }

    public class LocalizedStatic : Static
    {
        private int m_LabelNumber;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Number
        {
            get { return m_LabelNumber; }
            set
            {
                m_LabelNumber = value;
                InvalidateProperties();
            }
        }

        public override int LabelNumber
        {
            get { return m_LabelNumber; }
        }

        [Constructable]
        public LocalizedStatic(int itemID)
            : this(itemID, itemID < 0x4000 ? 1020000 + itemID : 1078872 + itemID) { }

        [Constructable]
        public LocalizedStatic(int itemID, int labelNumber)
            : base(itemID)
        {
            m_LabelNumber = labelNumber;
        }

        public LocalizedStatic(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((byte)0); // version
            writer.WriteEncodedInt((int)m_LabelNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadByte();

            switch (version)
            {
                case 0:
                {
                    m_LabelNumber = reader.ReadEncodedInt();
                    break;
                }
            }
        }
    }

    public sealed class WeightedDecorationStatic : Static
    {
        public WeightedDecorationStatic() { }

        public WeightedDecorationStatic(Serial serial)
            : base(serial) { }

        public override bool DisplayWeight
        {
            get
            {
                if (Weight != -2)
                    return base.DisplayWeight;

                return DefaultWeight > 0 && (Movable || IsLockedDown || IsSecure || ItemData.Weight != 255);
            }
        }

        public override void AddWeightProperty(ObjectPropertyList list)
        {
            if (Weight != -2)
            {
                base.AddWeightProperty(list);
                return;
            }

            // Preserve the default art weight in the tooltip while decoration owns the item.
            int weight = (int)Math.Ceiling(DefaultWeight * Amount) + TotalWeight;
            list.Add(weight == 1 ? 1072788 : 1072789, weight.ToString());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
