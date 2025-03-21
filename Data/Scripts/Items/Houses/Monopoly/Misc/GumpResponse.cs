using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Knives.TownHouses
{
    public class GumpResponse
    {
        private static PacketHandler m_Successor;

        public static void Initialize()
        {
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(AfterInit));
        }

        private static void AfterInit()
        {
            m_Successor = PacketHandlers.GetHandler(0xB1);

            PacketHandlers.Register(0xB1, 0, true, new OnPacketReceive(DisplayGumpResponse));
        }

        public static void DisplayGumpResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int typeID = pvSrc.ReadInt32();
            int buttonID = pvSrc.ReadInt32();

            List<Gump> gumps = ((List<Gump>)state.Gumps);

            for (int i = 0; i < gumps.Count; ++i)
            {
                Gump gump = gumps[i];

                if (gump.Serial == serial && gump.TypeID == typeID)
                {
                    int switchCount = pvSrc.ReadInt32();

                    if (switchCount < 0)
                    {
                        Console.WriteLine(
                            "Client: {0}: Invalid gump response, disconnecting...",
                            state
                        );
                        state.Dispose();
                        return;
                    }

                    int[] switches = new int[switchCount];

                    for (int j = 0; j < switches.Length; ++j)
                        switches[j] = pvSrc.ReadInt32();

                    int textCount = pvSrc.ReadInt32();

                    if (textCount < 0)
                    {
                        Console.WriteLine(
                            "Client: {0}: Invalid gump response, disconnecting...",
                            state
                        );
                        state.Dispose();
                        return;
                    }

                    TextRelay[] textEntries = new TextRelay[textCount];

                    for (int j = 0; j < textEntries.Length; ++j)
                    {
                        int entryID = pvSrc.ReadUInt16();
                        int textLength = pvSrc.ReadUInt16();

                        if (textLength > 239)
                            return;

                        string text = pvSrc.ReadUnicodeStringSafe(textLength);
                        textEntries[j] = new TextRelay(entryID, text);
                    }

                    state.RemoveGump(i);

                    if (!CheckResponse(gump, state.Mobile, buttonID))
                        return;

                    gump.OnResponse(state, new RelayInfo(buttonID, switches, textEntries));

                    return;
                }
            }

            if (m_Successor != null)
            {
                m_Successor.OnReceive(state, pvSrc);
            }
            else
            {
                PacketHandlers.DisplayGumpResponse(state, pvSrc);
            }
        }

        private static bool CheckResponse(Gump gump, Mobile m, int id)
        {
            if (m == null || !m.Player)
                return true;

            TownHouse th = null;

            ArrayList list = new ArrayList();

            foreach (Item item in m.GetItemsInRange(20))
            {
                if (item is TownHouse)
                    list.Add(item);
            }

            foreach (TownHouse t in list)
            {
                if (t.Owner == m)
                {
                    th = t;
                    break;
                }
            }

            if (th == null || th.ForSaleSign == null)
                return true;

            if (gump is HouseGumpAOS)
            {
                int val = id - 1;

                if (val < 0)
                    return true;

                int type = val % 15;
                int index = val / 15;

                if (th.ForSaleSign.ForcePublic && type == 3 && index == 12 && th.Public)
                {
                    m.SendMessage("This house cannot be private.");
                    m.SendGump(gump);
                    return false;
                }

                if (th.ForSaleSign.ForcePrivate && type == 3 && index == 13 && !th.Public)
                {
                    m.SendMessage("This house cannot be public.");
                    m.SendGump(gump);
                    return false;
                }

                if (th.ForSaleSign.NoTrade && type == 6 && index == 1)
                {
                    m.SendMessage("This house cannot be traded.");
                    m.SendGump(gump);
                    return false;
                }
            }

            return true;
        }
    }
}
