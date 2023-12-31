using System;
using Server;

namespace Server.Items
{
    public class StaffOfPower : BlackStaff
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
            get { return 1070692; }
        }

        [Constructable]
        public StaffOfPower()
        {
            ItemID = 0x0DF1;
            WeaponAttributes.MageWeapon = 15;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 20;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 20;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public StaffOfPower(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
