using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class RamusNecromanticScalpel : ButcherKnife, IIslesDreadDyable
    {
        public override int InitMinHits
        {
            get { return 80; }
        }
        public override int InitMaxHits
        {
            get { return 160; }
        }

        [Constructable]
        public RamusNecromanticScalpel()
        {
            Name = "Ramus' Necromantic Scalpel";
            Hue = 1372;
            WeaponAttributes.HitLeechHits = 60;
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 20;
            Slayer = SlayerName.Repond;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Artefact");
        }

        public RamusNecromanticScalpel(Serial serial)
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
