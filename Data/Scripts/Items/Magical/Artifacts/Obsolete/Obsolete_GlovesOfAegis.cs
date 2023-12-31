using System;
using Server;

namespace Server.Items
{
    public class GlovesOfAegis : PlateGloves
    {
        public override int InitMinHits
        {
            get { return 80; }
        }
        public override int InitMaxHits
        {
            get { return 160; }
        }

        public override int LabelNumber
        {
            get { return 1061602; }
        } // Gloves of �gis

        public override int BasePhysicalResistance
        {
            get { return 8; }
        }

        [Constructable]
        public GlovesOfAegis()
        {
            Name = "Gloves of Aegis";
            Hue = 0x47E;
            ArmorAttributes.SelfRepair = 5;
            Attributes.ReflectPhysical = 10;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 4;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public GlovesOfAegis(Serial serial)
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
