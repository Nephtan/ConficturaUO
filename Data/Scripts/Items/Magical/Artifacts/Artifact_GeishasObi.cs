using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class Artifact_GeishasObi : GiftObi, IIslesDreadDyable
    {
        public override int InitMinHits
        {
            get { return 80; }
        }
        public override int InitMaxHits
        {
            get { return 160; }
        }

        public override int BasePhysicalResistance
        {
            get { return 5; }
        }

        [Constructable]
        public Artifact_GeishasObi()
        {
            Name = "Geishas Obi";
            Hue = 31;
            Attributes.BonusInt = 5;
            Attributes.DefendChance = 5;
            Attributes.ReflectPhysical = 10;
            Attributes.RegenHits = 3;
            Server.Misc.Arty.ArtySetup(this, 5, "");
        }

        public Artifact_GeishasObi(Serial serial)
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
