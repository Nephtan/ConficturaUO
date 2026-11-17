using System;
using System.Collections;
using Server;

namespace Server.Items
{
    public class Artifact_BootsofHermes : GiftBoots
    {
        [Constructable]
        public Artifact_BootsofHermes()
        {
            Hue = 0xBAD;
            ItemID = 0x2FC4;
            Name = "Boots of Hermes";
            Attributes.BonusDex = 10;
            Server.Misc.Arty.ArtySetup(this, 15, "Sprinting ");
        }

        public override bool OnEquip(Mobile from)
        {
            if (
                Server.Misc.MyServerSettings.NoMountsInCertainRegions()
                && Server.Mobiles.AnimalTrainer.IsNoMountRegion(
                    from,
                    Region.Find(from.Location, from.Map)
                )
            )
            {
                Server.Mobiles.AnimalTrainer.RefreshMovementSpeed(from, false);
                Weight = 5.0;
                from.SendMessage("These shoes seem to have their magic diminished here.");
            }
            else
            {
                Weight = 3.0;
                Server.Mobiles.AnimalTrainer.RefreshMovementSpeed(from, true);
            }

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;
                Server.Mobiles.AnimalTrainer.RefreshMovementSpeed(from);
            }
            base.OnRemoved(parent);
        }

        public Artifact_BootsofHermes(Serial serial)
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
