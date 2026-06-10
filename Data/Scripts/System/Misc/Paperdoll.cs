using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Misc
{
    public class Paperdoll
    {
        public static void Initialize()
        {
            EventSink.PaperdollRequest += new PaperdollRequestEventHandler(
                EventSink_PaperdollRequest
            );
        }

        public static void EventSink_PaperdollRequest(PaperdollRequestEventArgs e)
        {
            if (e == null || e.Beholder == null || e.Beholder.Deleted || e.Beheld == null || e.Beheld.Deleted)
                return;

            Mobile beholder = e.Beholder;
            Mobile beheld = e.Beheld;

            beholder.Send(
                new DisplayPaperdoll(
                    beheld,
                    Titles.ComputeTitle(beholder, beheld),
                    beheld.AllowEquipFrom(beholder)
                )
            );

            if (ObjectPropertyList.Enabled)
            {
                List<Item> items = beheld.Items;

                if (items == null)
                    return;

                for (int i = 0; i < items.Count; ++i)
                {
                    Item item = items[i];

                    if (item != null && !item.Deleted)
                        beholder.Send(item.OPLPacket);
                }
            }
        }
    }
}
