using System;
using Server;

namespace Server.Items
{
    public class Aegis : HeaterShield
    {
        public override int LabelNumber
        {
            get { return 1061602; }
        } // �gis

        public override int BasePhysicalResistance
        {
            get { return 15; }
        }

        [Constructable]
        public Aegis()
        {
            Hue = 0x47E;
            ArmorAttributes.SelfRepair = 5;
            Attributes.ReflectPhysical = 15;
            Attributes.DefendChance = 15;
            Attributes.LowerManaCost = 8;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public Aegis(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
                PhysicalBonus = 0;
        }
    }
}
