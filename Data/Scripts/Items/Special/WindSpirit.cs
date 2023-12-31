using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class WindSpirit : Item
    {
        [Constructable]
        public WindSpirit()
            : base(0x1F1F)
        {
            Name = "wind spirit";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public WindSpirit(Serial serial)
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
