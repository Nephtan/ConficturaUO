using System;
using Server.Mobiles;

namespace Server.Items
{
    public class Arrow : Item, ICommodity
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
        public Arrow()
            : this(1) { }

        [Constructable]
        public Arrow(int amount)
            : base(0xF3F)
        {
            Stackable = true;
            Amount = amount;
            Name = "arrow";
        }

        public Arrow(Serial serial)
            : base(serial) { }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile && m.Alive && Movable)
            {
                m.PlaceInBackpack(this);
            }
            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Update version to 1 for new serialization format
            if (Name != "arrow") // Only serialize Name if it's different from default
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
                    Name = "arrow"; // Set default name if not serialized
                }
            }
            else
            {
                // For items saved with the old version, set the default name here if desired
                Name = "arrow";
            }
        }
    }
}
