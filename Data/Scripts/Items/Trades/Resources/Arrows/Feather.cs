using System;
using Server.Items;

namespace Server.Items
{
    public class Feather : Item, ICommodity
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
        public Feather()
            : this(1) { }

        [Constructable]
        public Feather(int amount)
            : base(0x4CCD)
        {
            Name = "feather";
            Stackable = true;
            Amount = amount;
        }

        public Feather(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Update version to 1 for new serialization format
            if (Name != "feather") // Only serialize Name if it's different from default
            {
                writer.Write(true);
                writer.Write(Name);
            }
            else
            {
                writer.Write(false);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Light = LightType.Circle150;
            if (version >= 1)
            {
                bool hasCustomName = reader.ReadBool();
                if (hasCustomName)
                {
                    Name = reader.ReadString();
                }
                else
                {
                    Name = "feather"; // Set default name if not serialized
                }
            }
            else
            {
                // For items saved with the old version, set the default name here if desired
                Name = "feather";
            }
        }
    }
}
