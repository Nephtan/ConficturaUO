using System;
using Server.Network;

namespace Server.Items
{
    public class Celery : Food
    {
        [Constructable]
        public Celery()
            : this(1) { }

        [Constructable]
        public Celery(int amount)
            : base(amount, 0xC77)
        {
            this.Weight = 0.5;
            this.FillFactor = 1;
            this.Name = "Celery";
            this.Hue = 0xAA;
        }

        public Celery(Serial serial)
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
