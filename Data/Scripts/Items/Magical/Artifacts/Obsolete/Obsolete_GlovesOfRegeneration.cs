using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class GlovesOfRegeneration : LeatherGloves, IIslesDreadDyable
    {
        public override int InitMinHits
        {
            get { return 80; }
        }
        public override int InitMaxHits
        {
            get { return 160; }
        }

        public override int BaseColdResistance
        {
            get { return 8; }
        }
        public override int BaseEnergyResistance
        {
            get { return 10; }
        }
        public override int BasePhysicalResistance
        {
            get { return 5; }
        }
        public override int BasePoisonResistance
        {
            get { return 15; }
        }
        public override int BaseFireResistance
        {
            get { return 10; }
        }

        [Constructable]
        public GlovesOfRegeneration()
        {
            Name = "Gloves Of Regeneration";
            Hue = 1284;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 5;
            Attributes.RegenStam = 5;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public GlovesOfRegeneration(Serial serial)
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
