using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class BoxOfAtonement : Item
    {
        [Constructable]
        public BoxOfAtonement()
            : base(0x9A8)
        {
            Name = "Box of Atonement";
            Hue = 0x497;
            Movable = false;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold)
            {
                int nPenalty = AssassinFunctions.QuestFailure(from);

                if (dropped.Amount == nPenalty)
                {
                    PlayerSettings.ClearQuestInfo(from, "AssassinQuest");
                    from.PrivateOverheadMessage(
                        MessageType.Regular,
                        1153,
                        false,
                        "Your failure in this task has been forgiven.",
                        from.NetState
                    );
                    dropped.Delete();
                }
                else
                {
                    from.AddToBackpack(dropped);
                }
            }
            else
            {
                from.AddToBackpack(dropped);
            }
            return true;
        }

        public BoxOfAtonement(Serial serial)
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
