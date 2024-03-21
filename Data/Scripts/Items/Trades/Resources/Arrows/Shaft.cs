using System;
using Server.Items;

namespace Server.Items
{
    public class Shaft : Item, ICommodity
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
        public Shaft()
            : this(1) { }

        [Constructable]
        public Shaft(int amount)
            : base(0x1BD4)
        {
            Stackable = true;
            Amount = amount;
            Name = "shaft";
        }

        public Shaft(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Update version to 1 for new serialization format
            if (Name != "shaft") // Only serialize Name if it's different from default
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
                    Name = "shaft"; // Set default name if not serialized
                }
            }
            else
            {
                // For items saved with the old version, set the default name here if desired
                Name = "shaft";
            }
        }
    }
}
