using System;
using Server.Network;

namespace Server.Items
{
    public class Grapes : Food
    {
        public string Desc;

        [Constructable]
        public Grapes()
            : this(1) { }

        [Constructable]
        public Grapes(int amount)
            : base(amount, 0x9D1)
        {
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Desc = "grape";
        }

        public Grapes(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((string)Desc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    Desc = reader.ReadString();
                    break;
                }
            }
        }
    }
}
