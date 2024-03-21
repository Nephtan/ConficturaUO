using System;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Spells.Third;
using Server.Targeting;

namespace Server.Items
{
    public class Artifact_RobeOfTeleportation : GiftRobe
    {
        [Constructable]
        public Artifact_RobeOfTeleportation()
        {
            Name = "Robe Of Teleportation";
            Hue = Utility.RandomColor(0);
            Server.Misc.Arty.ArtySetup(this, 5, "(Use to Teleport) ");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Parent != from)
            {
                from.SendMessage("You must be wearing the robe to teleport.");
            }
            else
            {
                new TeleportSpell(from, this).Cast();
            }
            return;
        }

        public Artifact_RobeOfTeleportation(Serial serial)
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
