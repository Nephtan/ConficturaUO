using System;
using Server;

namespace Server.Items
{
    public class InquisitorsLeggings : PlateLegs
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
            get { return 1060206; }
        } // The Inquisitor's Leggings

        public override int BaseColdResistance
        {
            get { return 24; }
        }
        public override int BaseEnergyResistance
        {
            get { return 19; }
        }

        [Constructable]
        public InquisitorsLeggings()
        {
            Name = "Inquisitor's Leggings";
            Hue = 0x4F2;
            ItemID = 0x46AA;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 10;
            ArmorAttributes.MageArmor = 1;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public InquisitorsLeggings(Serial serial)
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
            {
                ColdBonus = 0;
                EnergyBonus = 0;
            }
        }
    }
}
