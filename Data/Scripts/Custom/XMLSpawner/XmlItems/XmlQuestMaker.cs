using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;
using Server;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

/*
** XmlQuestMaker
**
** Version 1.00
** updated 9/03/04
** ArteGordon
**
*/
namespace Server.Items
{
    public class XmlQuestMaker : Item
    {
        public XmlQuestMaker(Serial serial)
            : base(serial) { }

        [Constructable]
        public XmlQuestMaker()
            : base(0xED4)
        {
            Name = "XmlQuestMaker";
            Movable = false;
            Visible = true;
        }

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

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!(from is PlayerMobile))
                return;

            // make a quest note
            QuestHolder newquest = new QuestHolder();
            newquest.PlayerMade = true;
            newquest.Creator = from as PlayerMobile;
            newquest.Hue = 500;
            from.AddToBackpack(newquest);
            from.SendMessage("A blank quest has been added to your pack!");
        }
    }
}
