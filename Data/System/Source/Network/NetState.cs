/***************************************************************************
 *                                NetState.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server;
using Server.Accounting;
using Server.Diagnostics;
using Server.Gumps;
using Server.HuePickers;
using Server.Items;
using Server.Menus;
using Server.Network;

namespace Server.Network
{
    public interface IPacketEncoder
    {
        void EncodeOutgoingPacket(NetState to, ref byte[] buffer, ref int length);
        void DecodeIncomingPacket(NetState from, ref byte[] buffer, ref int length);
    }

    public delegate void NetStateCreatedCallback(NetState ns);

    public class NetState
    {
        private Socket m_Socket;
        private IPAddress m_Address;
        private ByteQueue m_Buffer;
        private byte[] m_RecvBuffer;
        private SendQueue m_SendQueue;
        private bool m_Seeded;
        private bool m_Running;

#if Framework_4_0
        private SocketAsyncEventArgs m_ReceiveEventArgs,
            m_SendEventArgs;
#else
        private AsyncCallback m_OnReceive,
            m_OnSend;
#endif

        private MessagePump m_MessagePump;
        private ServerInfo[] m_ServerInfo;
        private IAccount m_Account;
        private Mobile m_Mobile;
        private CityInfo[] m_CityInfo;
        private List<Gump> m_Gumps;
        private List<HuePicker> m_HuePickers;
        private List<IMenu> m_Menus;
        private List<SecureTrade> m_Trades;
        private int m_Sequence;
        private bool m_CompressionEnabled;
        private string m_ToString;
        private ClientVersion m_Version;
        private bool m_SentFirstPacket;
        private bool m_BlockAllPackets;

        private DateTime m_ConnectedOn;

        public DateTime ConnectedOn
        {
            get { return m_ConnectedOn; }
        }

        public TimeSpan ConnectedFor
        {
            get { return (DateTime.Now - m_ConnectedOn); }
        }

        internal int m_Seed;
        internal int m_AuthID;

        public IPAddress Address
        {
            get { return m_Address; }
        }

        private ClientFlags m_Flags;

        private static bool m_Paused;

        [Flags]
        private enum AsyncState
        {
            Pending = 0x01,
            Paused = 0x02
        }

        private AsyncState m_AsyncState;
        private object m_AsyncLock = new object();

        private IPacketEncoder m_Encoder = null;

        public IPacketEncoder PacketEncoder
        {
            get { return m_Encoder; }
            set { m_Encoder = value; }
        }

        private static NetStateCreatedCallback m_CreatedCallback;

        public static NetStateCreatedCallback CreatedCallback
        {
            get { return m_CreatedCallback; }
            set { m_CreatedCallback = value; }
        }

        public bool SentFirstPacket
        {
            get { return m_SentFirstPacket; }
            set { m_SentFirstPacket = value; }
        }

        public bool BlockAllPackets
        {
            get { return m_BlockAllPackets; }
            set { m_BlockAllPackets = value; }
        }

        public ClientFlags Flags
        {
            get { return m_Flags; }
            set { m_Flags = value; }
        }

        public ClientVersion Version
        {
            get { return m_Version; }
            set
            {
                m_Version = value;

                if (value >= m_Version70160)
                {
                    _ProtocolChanges = ProtocolChanges.Version70160;
                }
                else if (value >= m_Version70130)
                {
                    _ProtocolChanges = ProtocolChanges.Version70130;
                }
                else if (value >= m_Version7090)
                {
                    _ProtocolChanges = ProtocolChanges.Version7090;
                }
                else if (value >= m_Version7000)
                {
                    _ProtocolChanges = ProtocolChanges.Version7000;
                }
                else if (value >= m_Version60142)
                {
                    _ProtocolChanges = ProtocolChanges.Version60142;
                }
                else if (value >= m_Version6017)
                {
                    _ProtocolChanges = ProtocolChanges.Version6017;
                }
                else if (value >= m_Version6000)
                {
                    _ProtocolChanges = ProtocolChanges.Version6000;
                }
                else if (value >= m_Version502b)
                {
                    _ProtocolChanges = ProtocolChanges.Version502b;
                }
                else if (value >= m_Version500a)
                {
                    _ProtocolChanges = ProtocolChanges.Version500a;
                }
                else if (value >= m_Version407a)
                {
                    _ProtocolChanges = ProtocolChanges.Version407a;
                }
                else if (value >= m_Version400a)
                {
                    _ProtocolChanges = ProtocolChanges.Version400a;
                }
            }
        }

        private static ClientVersion m_Version400a = new ClientVersion("4.0.0a");
        private static ClientVersion m_Version407a = new ClientVersion("4.0.7a");
        private static ClientVersion m_Version500a = new ClientVersion("5.0.0a");
        private static ClientVersion m_Version502b = new ClientVersion("5.0.2b");
        private static ClientVersion m_Version6000 = new ClientVersion("6.0.0.0");
        private static ClientVersion m_Version6017 = new ClientVersion("6.0.1.7");
        private static ClientVersion m_Version60142 = new ClientVersion("6.0.14.2");
        private static ClientVersion m_Version7000 = new ClientVersion("7.0.0.0");
        private static ClientVersion m_Version7090 = new ClientVersion("7.0.9.0");
        private static ClientVersion m_Version70130 = new ClientVersion("7.0.13.0");
        private static ClientVersion m_Version70160 = new ClientVersion("7.0.16.0");

        private ProtocolChanges _ProtocolChanges;

        private enum ProtocolChanges
        {
            NewSpellbook = 0x00000001,
            DamagePacket = 0x00000002,
            Unpack = 0x00000004,
            BuffIcon = 0x00000008,
            NewHaven = 0x00000010,
            ContainerGridLines = 0x00000020,
            ExtendedSupportedFeatures = 0x00000040,
            StygianAbyss = 0x00000080,
            HighSeas = 0x00000100,
            NewCharacterList = 0x00000200,
            NewCharacterCreation = 0x00000400,

            Version400a = NewSpellbook,
            Version407a = Version400a | DamagePacket,
            Version500a = Version407a | Unpack,
            Version502b = Version500a | BuffIcon,
            Version6000 = Version502b | NewHaven,
            Version6017 = Version6000 | ContainerGridLines,
            Version60142 = Version6017 | ExtendedSupportedFeatures,
            Version7000 = Version60142 | StygianAbyss,
            Version7090 = Version7000 | HighSeas,
            Version70130 = Version7090 | NewCharacterList,
            Version70160 = Version70130 | NewCharacterCreation
        }

        public bool NewSpellbook
        {
            get { return ((_ProtocolChanges & ProtocolChanges.NewSpellbook) != 0); }
        }
        public bool DamagePacket
        {
            get { return ((_ProtocolChanges & ProtocolChanges.DamagePacket) != 0); }
        }
        public bool Unpack
        {
            get { return ((_ProtocolChanges & ProtocolChanges.Unpack) != 0); }
        }
        public bool BuffIcon
        {
            get { return ((_ProtocolChanges & ProtocolChanges.BuffIcon) != 0); }
        }
        public bool NewHaven
        {
            get { return ((_ProtocolChanges & ProtocolChanges.NewHaven) != 0); }
        }
        public bool ContainerGridLines
        {
            get { return ((_ProtocolChanges & ProtocolChanges.ContainerGridLines) != 0); }
        }
        public bool ExtendedSupportedFeatures
        {
            get { return ((_ProtocolChanges & ProtocolChanges.ExtendedSupportedFeatures) != 0); }
        }
        public bool StygianAbyss
        {
            get { return ((_ProtocolChanges & ProtocolChanges.StygianAbyss) != 0); }
        }
        public bool HighSeas
        {
            get { return ((_ProtocolChanges & ProtocolChanges.HighSeas) != 0); }
        }
        public bool NewCharacterList
        {
            get { return ((_ProtocolChanges & ProtocolChanges.NewCharacterList) != 0); }
        }
        public bool NewCharacterCreation
        {
            get { return ((_ProtocolChanges & ProtocolChanges.NewCharacterCreation) != 0); }
        }

        public bool IsUOTDClient
        {
            get
            {
                return (
                    (m_Flags & ClientFlags.UOTD) != 0
                    || (m_Version != null && m_Version.Type == ClientType.UOTD)
                );
            }
        }

        public bool IsSAClient
        {
            get { return (m_Version != null && m_Version.Type == ClientType.SA); }
        }

        public List<SecureTrade> Trades
        {
            get { return m_Trades; }
        }

        public void ValidateAllTrades()
        {
            for (int i = m_Trades.Count - 1; i >= 0; --i)
            {
                if (i >= m_Trades.Count)
                {
                    continue;
                }

                SecureTrade trade = m_Trades[i];

                if (
                    trade.From.Mobile.Deleted
                    || trade.To.Mobile.Deleted
                    || !trade.From.Mobile.Alive
                    || !trade.To.Mobile.Alive
                    || !trade.From.Mobile.InRange(trade.To.Mobile, 2)
                    || trade.From.Mobile.Map != trade.To.Mobile.Map
                )
                {
                    trade.Cancel();
                }
            }
        }

        public void CancelAllTrades()
        {
            for (int i = m_Trades.Count - 1; i >= 0; --i)
            {
                if (i < m_Trades.Count)
                {
                    m_Trades[i].Cancel();
                }
            }
        }

        public void RemoveTrade(SecureTrade trade)
        {
            m_Trades.Remove(trade);
        }

        public SecureTrade FindTrade(Mobile m)
        {
            for (int i = 0; i < m_Trades.Count; ++i)
            {
                SecureTrade trade = m_Trades[i];

                if (trade.From.Mobile == m || trade.To.Mobile == m)
                {
                    return trade;
                }
            }

            return null;
        }

        public SecureTradeContainer FindTradeContainer(Mobile m)
        {
            for (int i = 0; i < m_Trades.Count; ++i)
            {
                SecureTrade trade = m_Trades[i];

                SecureTradeInfo from = trade.From;
                SecureTradeInfo to = trade.To;

                if (from.Mobile == m_Mobile && to.Mobile == m)
                {
                    return from.Container;
                }
                else if (from.Mobile == m && to.Mobile == m_Mobile)
                {
                    return to.Container;
                }
            }

            return null;
        }

        public SecureTradeContainer AddTrade(NetState state)
        {
            SecureTrade newTrade = new SecureTrade(m_Mobile, state.m_Mobile);

            m_Trades.Add(newTrade);
            state.m_Trades.Add(newTrade);

            return newTrade.From.Container;
        }

        public bool CompressionEnabled
        {
            get { return m_CompressionEnabled; }
            set { m_CompressionEnabled = value; }
        }

        public int Sequence
        {
            get { return m_Sequence; }
            set { m_Sequence = value; }
        }

        public IEnumerable<Gump> Gumps
        {
            get { return m_Gumps; }
        }

        public IEnumerable<HuePicker> HuePickers
        {
            get { return m_HuePickers; }
        }

        public IEnumerable<IMenu> Menus
        {
            get { return m_Menus; }
        }

        private static int m_GumpCap = 512,
            m_HuePickerCap = 512,
            m_MenuCap = 512;

        public static int GumpCap
        {
            get { return m_GumpCap; }
            set { m_GumpCap = value; }
        }

        public static int HuePickerCap
        {
            get { return m_HuePickerCap; }
            set { m_HuePickerCap = value; }
        }

        public static int MenuCap
        {
            get { return m_MenuCap; }
            set { m_MenuCap = value; }
        }

        public void WriteConsole(string text)
        {
            Console.WriteLine("Client: {0}: {1}", this, text);
        }

        public void WriteConsole(string format, params object[] args)
        {
            WriteConsole(String.Format(format, args));
        }

        public void AddMenu(IMenu menu)
        {
            if (m_Menus == null)
            {
                m_Menus = new List<IMenu>();
            }

            if (m_Menus.Count < m_MenuCap)
            {
                m_Menus.Add(menu);
            }
            else
            {
                WriteConsole("Exceeded menu cap, disconnecting...");
                Dispose();
            }
        }

        public void RemoveMenu(IMenu menu)
        {
            if (m_Menus != null)
            {
                m_Menus.Remove(menu);
            }
        }

        public void RemoveMenu(int index)
        {
            if (m_Menus != null)
            {
                m_Menus.RemoveAt(index);
            }
        }

        public void ClearMenus()
        {
            if (m_Menus != null)
            {
                m_Menus.Clear();
            }
        }

        public void AddHuePicker(HuePicker huePicker)
        {
            if (m_HuePickers == null)
            {
                m_HuePickers = new List<HuePicker>();
            }

            if (m_HuePickers.Count < m_HuePickerCap)
            {
                m_HuePickers.Add(huePicker);
            }
            else
            {
                WriteConsole("Exceeded hue picker cap, disconnecting...");
                Dispose();
            }
        }

        public void RemoveHuePicker(HuePicker huePicker)
        {
            if (m_HuePickers != null)
            {
                m_HuePickers.Remove(huePicker);
            }
        }

        public void RemoveHuePicker(int index)
        {
            if (m_HuePickers != null)
            {
                m_HuePickers.RemoveAt(index);
            }
        }

        public void ClearHuePickers()
        {
            if (m_HuePickers != null)
            {
                m_HuePickers.Clear();
            }
        }

        public void AddGump(Gump gump)
        {
            if (m_Gumps == null)
            {
                m_Gumps = new List<Gump>();
            }

            if (m_Gumps.Count < m_GumpCap)
            {
                m_Gumps.Add(gump);
            }
            else
            {
                WriteConsole("Exceeded gump cap, disconnecting...");
                Dispose();
            }
        }

        public void RemoveGump(Gump gump)
        {
            if (m_Gumps != null)
            {
                m_Gumps.Remove(gump);
            }
        }

        public void RemoveGump(int index)
        {
            if (m_Gumps != null)
            {
                m_Gumps.RemoveAt(index);
            }
        }

        public void ClearGumps()
        {
            if (m_Gumps != null)
            {
                m_Gumps.Clear();
            }
        }

        public void LaunchBrowser(string url)
        {
            Send(
                new MessageLocalized(
                    Serial.MinusOne,
                    -1,
                    MessageType.Label,
                    0x35,
                    3,
                    501231,
                    "",
                    ""
                )
            );
            Send(new LaunchBrowser(url));
        }

        public CityInfo[] CityInfo
        {
            get { return m_CityInfo; }
            set { m_CityInfo = value; }
        }

        public Mobile Mobile
        {
            get { return m_Mobile; }
            set
            {
                lock (m_WorldSendLock)
                {
                    if (m_Mobile != value)
                        ClearWorldSendQueue();

                    m_Mobile = value;
                }
            }
        }

        public ServerInfo[] ServerInfo
        {
            get { return m_ServerInfo; }
            set { m_ServerInfo = value; }
        }

        public IAccount Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }

        public override string ToString()
        {
            return m_ToString;
        }

        private static List<NetState> m_Instances = new List<NetState>();

        public static List<NetState> Instances
        {
            get { return m_Instances; }
        }

        public static int AuthenticatedCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < m_Instances.Count; ++i)
                {
                    if (m_Instances[i].m_Account != null)
                        ++count;
                }

                return count;
            }
        }

        public static int InGameCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < m_Instances.Count; ++i)
                {
                    Mobile m = m_Instances[i].m_Mobile;

                    if (m != null && !m.Deleted)
                        ++count;
                }

                return count;
            }
        }

        public static string ConnectionSummary
        {
            get
            {
                return String.Format(
                    "sockets={0}, authenticated={1}, ingame={2}",
                    m_Instances.Count,
                    AuthenticatedCount,
                    InGameCount
                );
            }
        }

        private static BufferPool m_ReceiveBufferPool = new BufferPool("Receive", 2048, 2048);

        public NetState(Socket socket, MessagePump messagePump)
        {
            m_Socket = socket;
            m_Buffer = new ByteQueue();
            m_Seeded = false;
            m_Running = false;
            m_RecvBuffer = m_ReceiveBufferPool.AcquireBuffer();
            m_MessagePump = messagePump;
            m_Gumps = new List<Gump>();
            m_HuePickers = new List<HuePicker>();
            m_Menus = new List<IMenu>();
            m_Trades = new List<SecureTrade>();

            m_SendQueue = new SendQueue();

            m_NextCheckActivity = DateTime.Now + TimeSpan.FromMinutes(0.5);

            m_Instances.Add(this);

            try
            {
                m_Address = Utility.Intern(((IPEndPoint)m_Socket.RemoteEndPoint).Address);
                m_ToString = m_Address.ToString();
            }
            catch (Exception ex)
            {
                TraceException(ex);
                m_Address = IPAddress.None;
                m_ToString = "(error)";
            }

            m_ConnectedOn = DateTime.Now;

            if (m_CreatedCallback != null)
            {
                m_CreatedCallback(this);
            }
        }

        // World producers/draining are game-thread owned; console/IRC packet sends
        // can be background callers. Admission and encoder use share this lock,
        // but virtual item methods never run with it held.
        private readonly object m_WorldSendLock = new object();
        private const int WorldSendEntryLimit = 65536;
        private const int WorldSendPacketLimit = 96 * 1024;
        private const int WorldSendGroupLimit = 64 * 1024;
        private const int WorldSendPendingLimit = 16 * 1024;
        private Queue<WorldSendEntry> m_WorldSendQueue;
        private Dictionary<Item, WorldSendEntry> m_WorldSendItems;
        private int m_WorldSendPacketBytes;
        private int m_WorldViewGeneration;
        private int m_WorldSendCoalesceEpoch;
        private int m_WorldSendBudget;
        private int m_WorldSendExamineBudget;
        private DateTime m_NextWorldSendSlice;
        private WorldSendTimer m_WorldSendTimer;
        private bool m_DrainingWorldSend;
        private int m_WorldSendDrainThread;
        private List<byte[]> m_WorldSendCapture;
        private int m_WorldSendCaptureBytes;
        private bool m_WorldSendCaptureFailed;
        private DateTime m_NextWorldSendDiagnostic;
        private static int m_WorldSendDiagnosticPending;

        private sealed class WorldSendEntry
        {
            public Item Item;
            public bool Remove;
            public bool SendOpl;
            public bool PropertiesOnly;
            public bool PropertyListOnly;
            public int Generation;
            public int CoalesceEpoch;
            public byte[] Packet;
        }

        private sealed class WorldSendTimer : Timer
        {
            private NetState m_State;

            public WorldSendTimer(NetState state)
                : base(TimeSpan.FromMilliseconds(25.0), TimeSpan.FromMilliseconds(25.0))
            {
                m_State = state;
                Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                lock (m_State.m_WorldSendLock)
                {
                    if (m_State.m_WorldSendTimer != this)
                    {
                        Stop();
                        return;
                    }
                }

                m_State.DrainWorldSendQueue();
                m_State.Flush();
            }
        }

        public int PendingWorldSends
        {
            get
            {
                lock (m_WorldSendLock)
                    return m_WorldSendQueue == null ? 0 : m_WorldSendQueue.Count;
            }
        }

        public int PendingSendBytes
        {
            get
            {
                lock (m_SendQueue)
                    return m_SendQueue.PendingBytes;
            }
        }

        public void BeginWorldViewChange()
        {
            // Earlier item snapshots are stale; their ordered removes/control packets
            // still have to run before the next view. Do not clear that FIFO here.
            lock (m_WorldSendLock)
                ++m_WorldViewGeneration;
        }

        private bool CanSendWorldItem(Item item)
        {
            Mobile viewer = m_Mobile;
            return item != null && !item.Deleted && item.Parent == null
                && viewer != null && !viewer.Deleted && viewer.NetState == this
                && viewer.Map != null && viewer.Map != Map.Internal && item.Map == viewer.Map
                && viewer.CanSee(item) && viewer.InRange(item.Location, item.GetUpdateRange(viewer));
        }

        public void SendWorldItem(Item item)
        {
            SendWorldItem(item, ObjectPropertyList.Enabled);
        }

        public void SendWorldItem(Item item, bool sendOplPacket)
        {
            QueueWorldItem(item, sendOplPacket, false);
        }

        public void SendWorldItemProperties(Item item)
        {
            QueueWorldItem(item, true, true);
        }

        public void SendItemProperties(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item.Parent != null)
            {
                item.SendPropertiesTo(m_Mobile);
                return;
            }

            if (!CanSendWorldItem(item))
                return;

            lock (m_WorldSendLock)
            {
                if (m_Socket == null || m_Disposing || m_BlockAllPackets)
                    return;

                // Full query replies retain their position and virtual dispatch,
                // without copying a crowded view's property lists into control data.
                WorldSendEntry entry = new WorldSendEntry();
                entry.Item = item;
                entry.PropertyListOnly = true;
                entry.Generation = m_WorldViewGeneration;
                ++m_WorldSendCoalesceEpoch;
                if (!QueueWorldSend(entry))
                    return;
            }

            DrainWorldSendQueue();
        }

        private void QueueWorldItem(Item item, bool sendOplPacket, bool propertiesOnly)
        {
            if (!CanSendWorldItem(item))
                return;

            lock (m_WorldSendLock)
            {
                if (m_Socket == null || m_Disposing || m_BlockAllPackets)
                    return;

                WorldSendEntry pending;
                if (m_WorldSendItems != null && m_WorldSendItems.TryGetValue(item, out pending)
                    && pending.Generation == m_WorldViewGeneration
                    && pending.CoalesceEpoch == m_WorldSendCoalesceEpoch)
                {
                    pending.SendOpl |= sendOplPacket;
                    pending.PropertiesOnly &= propertiesOnly;
                    return;
                }

                WorldSendEntry entry = new WorldSendEntry();
                entry.Item = item;
                entry.SendOpl = sendOplPacket;
                entry.PropertiesOnly = propertiesOnly;
                entry.Generation = m_WorldViewGeneration;
                entry.CoalesceEpoch = m_WorldSendCoalesceEpoch;
                if (!QueueWorldSend(entry))
                    return;

                m_WorldSendItems[item] = entry;
            }

            DrainWorldSendQueue();
        }

        public void SendWorldItemRemove(Item item)
        {
            lock (m_WorldSendLock)
            {
                if (item == null || m_Socket == null || m_Disposing || m_BlockAllPackets)
                    return;

                WorldSendEntry entry = new WorldSendEntry();
                entry.Item = item;
                entry.Remove = true;
                ++m_WorldSendCoalesceEpoch;
                if (!QueueWorldSend(entry))
                    return;
            }

            DrainWorldSendQueue();
        }

        private bool QueueWorldSend(WorldSendEntry entry)
        {
            if (PendingWorldSends >= WorldSendEntryLimit)
            {
                ReportWorldSend("entry_limit", true);
                Dispose(false);
                return false;
            }

            if (m_WorldSendQueue == null)
            {
                m_WorldSendQueue = new Queue<WorldSendEntry>();
                m_WorldSendItems = new Dictionary<Item, WorldSendEntry>();
            }

            m_WorldSendQueue.Enqueue(entry);
            if (m_WorldSendQueue.Count >= 1024)
                ReportWorldSend("crowded_view", false);

            return true;
        }

        private void ClearWorldSendQueue()
        {
            lock (m_WorldSendLock)
            {
                if (m_WorldSendTimer != null)
                {
                    m_WorldSendTimer.Stop();
                    m_WorldSendTimer = null;
                }

                m_WorldSendQueue = null;
                m_WorldSendItems = null;
                m_WorldSendPacketBytes = 0;
                ++m_WorldViewGeneration;
            }
        }

        private void DrainWorldSendQueue()
        {
            lock (m_WorldSendLock)
            {
                if (m_DrainingWorldSend || m_WorldSendQueue == null)
                    return;

                if (m_Socket == null || m_Disposing || m_BlockAllPackets)
                {
                    ClearWorldSendQueue();
                    return;
                }

                DateTime now = DateTime.UtcNow;
                if (now >= m_NextWorldSendSlice)
                {
                    m_NextWorldSendSlice = now + TimeSpan.FromMilliseconds(25.0);
                    m_WorldSendBudget = 64;
                    m_WorldSendExamineBudget = 512;
                }

                m_DrainingWorldSend = true;
                m_WorldSendDrainThread = Thread.CurrentThread.ManagedThreadId;
            }

            try
            {
                while (true)
                {
                    WorldSendEntry entry;
                    lock (m_WorldSendLock)
                    {
                        if (m_WorldSendQueue == null || m_WorldSendQueue.Count == 0
                            || m_WorldSendBudget <= 0 || m_WorldSendExamineBudget <= 0
                            || m_Socket == null || m_Disposing || PendingSendBytes > WorldSendPendingLimit)
                            break;

                        --m_WorldSendExamineBudget;
                        entry = m_WorldSendQueue.Dequeue();
                        if (entry.Packet != null)
                            m_WorldSendPacketBytes -= entry.Packet.Length;
                        else if (!entry.Remove)
                        {
                            WorldSendEntry pending;
                            if (m_WorldSendItems.TryGetValue(entry.Item, out pending)
                                && Object.ReferenceEquals(entry, pending))
                                m_WorldSendItems.Remove(entry.Item);

                            if (entry.Generation != m_WorldViewGeneration)
                                continue;
                        }

                        --m_WorldSendBudget;
                    }

                    if (entry.Packet != null)
                    {
                        SendCompiledBuffer(entry.Packet, entry.Packet.Length);
                    }
                    else if (entry.Remove)
                    {
                        Send(entry.Item.RemovePacket);
                    }
                    else if (CanSendWorldItem(entry.Item))
                    {
                        SendWorldItemGroup(entry.Item, entry.SendOpl, entry.PropertiesOnly, entry.PropertyListOnly);
                    }
                }
            }
            catch (Exception exception)
            {
                lock (m_WorldSendLock)
                    ReportWorldSend("dispatch_exception_" + exception.GetType().Name, true);
                Dispose(false);
            }
            finally
            {
                lock (m_WorldSendLock)
                {
                    m_DrainingWorldSend = false;
                    m_WorldSendDrainThread = 0;
                    if (m_WorldSendQueue == null || m_WorldSendQueue.Count == 0 || m_Disposing || m_Socket == null)
                        ClearWorldSendQueue();
                    else if (m_WorldSendTimer == null)
                    {
                        m_WorldSendTimer = new WorldSendTimer(this);
                        m_WorldSendTimer.Start();
                    }
                }
            }
        }

        private void SendWorldItemGroup(Item item, bool sendOplPacket, bool propertiesOnly, bool propertyListOnly)
        {
            Mobile viewer;
            int generation;
            List<byte[]> packets = new List<byte[]>();
            lock (m_WorldSendLock)
            {
                viewer = m_Mobile;
                generation = m_WorldViewGeneration;
                m_WorldSendCapture = packets;
                m_WorldSendCaptureBytes = 0;
                m_WorldSendCaptureFailed = false;
            }
            try
            {
                // Include every packet from virtual corpse/house implementations. Encode
                // only after capture so stateful encoders retain actual wire order.
                if (propertyListOnly)
                    item.SendPropertiesTo(viewer);
                else if (propertiesOnly)
                    Send(item.OPLPacket);
                else
                    item.SendInfoTo(this, sendOplPacket);
            }
            finally
            {
                lock (m_WorldSendLock)
                    m_WorldSendCapture = null;
            }

            lock (m_WorldSendLock)
            {
                if (m_Socket == null || m_Disposing)
                    return;

                if (viewer != m_Mobile || generation != m_WorldViewGeneration || !CanSendWorldItem(item))
                {
                    ReportWorldSend("view_changed_during_dispatch", true);
                    Dispose(false);
                    return;
                }

                if (m_WorldSendCaptureFailed)
                {
                    ReportWorldSend("item_group_limit", true);
                    Dispose(false);
                    return;
                }

                using (MemoryStream group = new MemoryStream())
                {
                    foreach (byte[] packet in packets)
                    {
                        byte[] buffer = packet;
                        int length = buffer.Length;
                        if (m_Encoder != null)
                            m_Encoder.EncodeOutgoingPacket(this, ref buffer, ref length);

                        if (buffer == null || length < 0 || length > buffer.Length
                            || group.Length + length > WorldSendGroupLimit)
                        {
                            ReportWorldSend("encoded_item_group_limit", true);
                            Dispose(false);
                            return;
                        }

                        group.Write(buffer, 0, length);
                    }

                    if (group.Length > 0)
                        EnqueueSendBuffer(group.ToArray(), (int)group.Length);
                }
            }
        }

        private bool CaptureOrQueueWorldPacket(byte[] buffer, int length)
        {
            bool drainingThread = m_DrainingWorldSend
                && m_WorldSendDrainThread == Thread.CurrentThread.ManagedThreadId;
            if (m_WorldSendCapture != null && drainingThread)
            {
                if ((long)m_WorldSendCaptureBytes + length > WorldSendGroupLimit
                    || m_WorldSendCapture.Count >= 256)
                {
                    m_WorldSendCaptureFailed = true;
                    return true;
                }

                byte[] copy = new byte[length];
                System.Buffer.BlockCopy(buffer, 0, copy, 0, length);
                m_WorldSendCapture.Add(copy);
                m_WorldSendCaptureBytes += length;
                return true;
            }

            if (drainingThread || (!m_DrainingWorldSend && PendingWorldSends == 0))
                return false;

            if ((long)m_WorldSendPacketBytes + length > WorldSendPacketLimit)
            {
                ReportWorldSend("control_packet_limit", true);
                Dispose(false);
                return true;
            }

            WorldSendEntry entry = new WorldSendEntry();
            entry.Packet = new byte[length];
            System.Buffer.BlockCopy(buffer, 0, entry.Packet, 0, length);
            // A later incoming must not merge into an earlier one across a removal
            // or interaction response, even when the item has returned to view.
            ++m_WorldSendCoalesceEpoch;
            if (QueueWorldSend(entry))
                m_WorldSendPacketBytes += length;

            return true;
        }

        private void ReportWorldSend(string reason, bool terminal)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                if (!terminal && now < m_NextWorldSendDiagnostic)
                    return;

                m_NextWorldSendDiagnostic = now + TimeSpan.FromMinutes(5.0);
                if (Interlocked.CompareExchange(ref m_WorldSendDiagnosticPending, 1, 0) != 0)
                    return;

                bool queued = false;
                try
                {
                    Mobile viewer = m_Mobile;
                    string message = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "utc={0:o} reason={1} serial={2} map={3} location={4} work={5} controlBytes={6} pendingBytes={7} action={8}",
                        now, reason, viewer == null ? Serial.MinusOne : viewer.Serial,
                        viewer == null || viewer.Map == null ? -1 : viewer.Map.MapIndex,
                        viewer == null ? Point3D.Zero : viewer.Location, PendingWorldSends,
                        m_WorldSendPacketBytes, PendingSendBytes, terminal ? "disconnect" : "paced");
                    queued = ThreadPool.QueueUserWorkItem(WriteWorldSendDiagnostic, message);
                }
                finally
                {
                    if (!queued)
                        Interlocked.Exchange(ref m_WorldSendDiagnosticPending, 0);
                }
            }
            catch (Exception) { }
        }

        private static void WriteWorldSendDiagnostic(object state)
        {
            try
            {
                string directory = Path.Combine(Core.BaseDirectory, "Logs");
                Directory.CreateDirectory(directory);
                File.AppendAllText(Path.Combine(directory, "WorldItemStreaming.log"), (string)state + Environment.NewLine);
            }
            catch (Exception) { }
            finally
            {
                Interlocked.Exchange(ref m_WorldSendDiagnosticPending, 0);
            }
        }

        public virtual void Send(Packet p)
        {
            lock (m_WorldSendLock)
                SendPacket(p);
        }

        private void SendPacket(Packet p)
        {
            if (m_Socket == null || m_BlockAllPackets)
            {
                p.OnSend();
                return;
            }

            PacketSendProfile prof = PacketSendProfile.Acquire(p.GetType());

            int length;
            byte[] buffer = p.Compile(m_CompressionEnabled, out length);

            if (buffer != null)
            {
                if (buffer.Length <= 0 || length <= 0)
                {
                    p.OnSend();
                    return;
                }

                if (prof != null)
                {
                    prof.Start();
                }

                if (!CaptureOrQueueWorldPacket(buffer, length))
                    length = SendCompiledBuffer(buffer, length);

                p.OnSend();

                if (prof != null)
                {
                    prof.Finish(length);
                }
            }
            else
            {
                Console.WriteLine("Client: {0}: null buffer send, disconnecting...", this);
                using (StreamWriter op = new StreamWriter("null_send.log", true))
                {
                    op.WriteLine(
                        "{0} Client: {1}: null buffer send, disconnecting...",
                        DateTime.Now,
                        this
                    );
                    op.WriteLine(new System.Diagnostics.StackTrace());
                }
                Dispose();
            }
        }

        private int SendCompiledBuffer(byte[] buffer, int length)
        {
            lock (m_WorldSendLock)
            {
                if (m_Encoder != null)
                    m_Encoder.EncodeOutgoingPacket(this, ref buffer, ref length);

                EnqueueSendBuffer(buffer, length);
                return length;
            }
        }

        private void EnqueueSendBuffer(byte[] buffer, int length)
        {
            if (m_Socket == null || m_Disposing)
                return;

            try
            {
                SendQueue.Gram gram;

                lock (m_SendQueue)
                {
                    gram = m_SendQueue.Enqueue(buffer, length);
                }

                if (gram != null)
                {
#if Framework_4_0
                    m_SendEventArgs.SetBuffer(gram.Buffer, 0, gram.Length);
                    Send_Start();
#else
                    try
                    {
                        m_Socket.BeginSend(
                            gram.Buffer,
                            0,
                            gram.Length,
                            SocketFlags.None,
                            m_OnSend,
                            m_Socket
                        );
                    }
                    catch (Exception ex)
                    {
                        TraceException(ex);
                        Dispose(false);
                    }
#endif
                }
            }
            catch (CapacityExceededException)
            {
                Console.WriteLine("Client: {0}: Too much data pending, disconnecting...", this);
                Dispose(false);
            }

        }

#if Framework_4_0
        public void Start()
        {
            m_ReceiveEventArgs = new SocketAsyncEventArgs();
            m_ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
                Receive_Completion
            );
            m_ReceiveEventArgs.SetBuffer(m_RecvBuffer, 0, m_RecvBuffer.Length);

            m_SendEventArgs = new SocketAsyncEventArgs();
            m_SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Send_Completion);

            m_Running = true;

            if (m_Socket == null || m_Paused)
            {
                return;
            }

            Receive_Start();
        }

        private void Receive_Start()
        {
            try
            {
                bool result = false;

                do
                {
                    lock (m_AsyncLock)
                    {
                        if ((m_AsyncState & (AsyncState.Pending | AsyncState.Paused)) == 0)
                        {
                            m_AsyncState |= AsyncState.Pending;
                            result = !m_Socket.ReceiveAsync(m_ReceiveEventArgs);

                            if (result)
                                Receive_Process(m_ReceiveEventArgs);
                        }
                    }
                } while (result);
            }
            catch (Exception ex)
            {
                TraceException(ex);
                Dispose(false);
            }
        }

        private void Receive_Completion(object sender, SocketAsyncEventArgs e)
        {
            Receive_Process(e);

            if (!m_Disposing)
                Receive_Start();
        }

        private void Receive_Process(SocketAsyncEventArgs e)
        {
            int byteCount = e.BytesTransferred;

            if (e.SocketError != SocketError.Success || byteCount <= 0)
            {
                Dispose(false);
                return;
            }

            m_NextCheckActivity = DateTime.Now + TimeSpan.FromMinutes(1.2);

            byte[] buffer = m_RecvBuffer;

            if (m_Encoder != null)
                m_Encoder.DecodeIncomingPacket(this, ref buffer, ref byteCount);

            lock (m_Buffer)
                m_Buffer.Enqueue(buffer, 0, byteCount);

            m_MessagePump.OnReceive(this);

            lock (m_AsyncLock)
            {
                m_AsyncState &= ~AsyncState.Pending;
            }
        }

        private void Send_Start()
        {
            try
            {
                bool result = false;

                do
                {
                    result = !m_Socket.SendAsync(m_SendEventArgs);

                    if (result)
                        Send_Process(m_SendEventArgs);
                } while (result);
            }
            catch (Exception ex)
            {
                TraceException(ex);
                Dispose(false);
            }
        }

        private void Send_Completion(object sender, SocketAsyncEventArgs e)
        {
            Send_Process(e);

            if (m_Disposing)
                return;

            if (m_CoalesceSleep >= 0)
            {
                Thread.Sleep(m_CoalesceSleep);
            }

            SendQueue.Gram gram;

            lock (m_SendQueue)
            {
                gram = m_SendQueue.Dequeue();
            }

            if (gram != null)
            {
                m_SendEventArgs.SetBuffer(gram.Buffer, 0, gram.Length);
                Send_Start();
            }
        }

        private void Send_Process(SocketAsyncEventArgs e)
        {
            int bytes = e.BytesTransferred;

            if (e.SocketError != SocketError.Success || bytes <= 0)
            {
                Dispose(false);
                return;
            }

            m_NextCheckActivity = DateTime.Now + TimeSpan.FromMinutes(1.2);
        }

        public static void Pause()
        {
            m_Paused = true;

            for (int i = 0; i < m_Instances.Count; ++i)
            {
                NetState ns = m_Instances[i];

                lock (ns.m_AsyncLock)
                {
                    ns.m_AsyncState |= AsyncState.Paused;
                }
            }
        }

        public static void Resume()
        {
            m_Paused = false;

            for (int i = 0; i < m_Instances.Count; ++i)
            {
                NetState ns = m_Instances[i];

                if (ns.m_Socket == null)
                {
                    continue;
                }

                lock (ns.m_AsyncLock)
                {
                    ns.m_AsyncState &= ~AsyncState.Paused;

                    if ((ns.m_AsyncState & AsyncState.Pending) == 0)
                        ns.Receive_Start();
                }
            }
        }

        public bool Flush()
        {
            if (m_Socket == null || !m_SendQueue.IsFlushReady)
            {
                return false;
            }

            SendQueue.Gram gram;

            lock (m_SendQueue)
            {
                gram = m_SendQueue.CheckFlushReady();
            }

            if (gram != null)
            {
                m_SendEventArgs.SetBuffer(gram.Buffer, 0, gram.Length);
                Send_Start();
            }

            return false;
        }
#else

        public void Start()
        {
            m_OnReceive = new AsyncCallback(OnReceive);
            m_OnSend = new AsyncCallback(OnSend);

            m_Running = true;

            if (m_Socket == null || m_Paused)
            {
                return;
            }

            try
            {
                lock (m_AsyncLock)
                {
                    if ((m_AsyncState & (AsyncState.Pending | AsyncState.Paused)) == 0)
                    {
                        InternalBeginReceive();
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    TraceConnectionReset(ex);
                else
                    TraceException(ex);

                Dispose(false);
            }
            catch (Exception ex)
            {
                TraceException(ex);
                Dispose(false);
            }
        }

        private void InternalBeginReceive()
        {
            m_AsyncState |= AsyncState.Pending;

            m_Socket.BeginReceive(
                m_RecvBuffer,
                0,
                m_RecvBuffer.Length,
                SocketFlags.None,
                m_OnReceive,
                m_Socket
            );
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            Socket s = (Socket)asyncResult.AsyncState;

            try
            {
                int byteCount = s.EndReceive(asyncResult);

                if (byteCount > 0)
                {
                    m_NextCheckActivity = DateTime.Now + TimeSpan.FromMinutes(1.2);

                    byte[] buffer = m_RecvBuffer;

                    if (m_Encoder != null)
                        m_Encoder.DecodeIncomingPacket(this, ref buffer, ref byteCount);

                    lock (m_Buffer)
                        m_Buffer.Enqueue(buffer, 0, byteCount);

                    m_MessagePump.OnReceive(this);

                    lock (m_AsyncLock)
                    {
                        m_AsyncState &= ~AsyncState.Pending;

                        if ((m_AsyncState & AsyncState.Paused) == 0)
                        {
                            try
                            {
                                InternalBeginReceive();
                            }
                            catch (Exception ex)
                            {
                                TraceException(ex);
                                Dispose(false);
                            }
                        }
                    }
                }
                else
                {
                    Dispose(false);
                }
            }
            catch
            {
                Dispose(false);
            }
        }

        private void OnSend(IAsyncResult asyncResult)
        {
            Socket s = (Socket)asyncResult.AsyncState;

            try
            {
                int bytes = s.EndSend(asyncResult);

                if (bytes <= 0)
                {
                    Dispose(false);
                    return;
                }

                m_NextCheckActivity = DateTime.Now + TimeSpan.FromMinutes(1.2);

                if (m_CoalesceSleep >= 0)
                {
                    Thread.Sleep(m_CoalesceSleep);
                }

                SendQueue.Gram gram;

                lock (m_SendQueue)
                {
                    gram = m_SendQueue.Dequeue();
                }

                if (gram != null)
                {
                    try
                    {
                        s.BeginSend(gram.Buffer, 0, gram.Length, SocketFlags.None, m_OnSend, s);
                    }
                    catch (Exception ex)
                    {
                        TraceException(ex);
                        Dispose(false);
                    }
                }
            }
            catch (Exception)
            {
                Dispose(false);
            }
        }

        public static void Pause()
        {
            m_Paused = true;

            for (int i = 0; i < m_Instances.Count; ++i)
            {
                NetState ns = m_Instances[i];

                lock (ns.m_AsyncLock)
                {
                    ns.m_AsyncState |= AsyncState.Paused;
                }
            }
        }

        public static void Resume()
        {
            m_Paused = false;

            for (int i = 0; i < m_Instances.Count; ++i)
            {
                NetState ns = m_Instances[i];

                if (ns.m_Socket == null)
                {
                    continue;
                }

                lock (ns.m_AsyncLock)
                {
                    ns.m_AsyncState &= ~AsyncState.Paused;

                    try
                    {
                        if ((ns.m_AsyncState & AsyncState.Pending) == 0)
                            ns.InternalBeginReceive();
                    }
                    catch (Exception ex)
                    {
                        TraceException(ex);
                        ns.Dispose(false);
                    }
                }
            }
        }

        public bool Flush()
        {
            if (m_Socket == null || !m_SendQueue.IsFlushReady)
            {
                return false;
            }

            SendQueue.Gram gram;

            lock (m_SendQueue)
            {
                gram = m_SendQueue.CheckFlushReady();
            }

            if (gram != null)
            {
                try
                {
                    m_Socket.BeginSend(
                        gram.Buffer,
                        0,
                        gram.Length,
                        SocketFlags.None,
                        m_OnSend,
                        m_Socket
                    );
                    return true;
                }
                catch (Exception ex)
                {
                    TraceException(ex);
                    Dispose(false);
                }
            }

            return false;
        }
#endif

        public PacketHandler GetHandler(int packetID)
        {
            if (ContainerGridLines)
                return PacketHandlers.Get6017Handler(packetID);
            else
                return PacketHandlers.GetHandler(packetID);
        }

        public static void FlushAll()
        {
            for (int i = 0; i < m_Instances.Count; ++i)
            {
                NetState ns = m_Instances[i];

                ns.Flush();
            }
        }

        private static int m_CoalesceSleep = -1;

        public static int CoalesceSleep
        {
            get { return m_CoalesceSleep; }
            set { m_CoalesceSleep = value; }
        }

        private DateTime m_NextCheckActivity;

        public bool CheckAlive()
        {
            if (m_Socket == null)
                return false;

            if (DateTime.Now < m_NextCheckActivity)
            {
                return true;
            }

            Console.WriteLine("Client: {0}: Disconnecting due to inactivity...", this);

            Dispose();
            return false;
        }

        public static void TraceException(Exception ex)
        {
            try
            {
                using (StreamWriter op = new StreamWriter("network-errors.log", true))
                {
                    op.WriteLine("# {0}", DateTime.Now);

                    op.WriteLine(ex);

                    op.WriteLine();
                    op.WriteLine();
                }
            }
            catch { }

            try
            {
                Console.WriteLine(ex);
            }
            catch { }
        }

        private void TraceConnectionReset(SocketException ex)
        {
            Core.WriteDiagnostic(
                "Network: connection reset before receive from {0} ({1}; saving={2}, paused={3}, {4})",
                this,
                ex.SocketErrorCode,
                World.Saving,
                m_Paused,
                ConnectionSummary
            );
        }

        private bool m_Disposing;

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool flush)
        {
            if (m_Socket == null || m_Disposing)
            {
                return;
            }

            m_Disposing = true;

            if (flush)
                flush = Flush();

            try
            {
                m_Socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                TraceException(ex);
            }

            try
            {
                m_Socket.Close();
            }
            catch (SocketException ex)
            {
                TraceException(ex);
            }

            if (m_RecvBuffer != null)
                m_ReceiveBufferPool.ReleaseBuffer(m_RecvBuffer);

            m_Socket = null;

            m_Buffer = null;
            m_RecvBuffer = null;

#if Framework_4_0
            m_ReceiveEventArgs = null;
            m_SendEventArgs = null;
#else
            m_OnReceive = null;
            m_OnSend = null;
#endif

            m_Running = false;

            m_Disposed.Enqueue(this);

            if ( /*!flush &&*/
                !m_SendQueue.IsEmpty
            )
            {
                lock (m_SendQueue)
                    m_SendQueue.Clear();
            }
        }

        public static void Initialize()
        {
            Timer.DelayCall(
                TimeSpan.FromMinutes(1.0),
                TimeSpan.FromMinutes(1.5),
                new TimerCallback(CheckAllAlive)
            );
        }

        public static void CheckAllAlive()
        {
            try
            {
                for (int i = 0; i < m_Instances.Count; ++i)
                {
                    m_Instances[i].CheckAlive();
                }
            }
            catch (Exception ex)
            {
                TraceException(ex);
            }
        }

        private static Queue m_Disposed = Queue.Synchronized(new Queue());

        public static void ProcessDisposedQueue()
        {
            int breakout = 0;

            while (breakout < 200 && m_Disposed.Count > 0)
            {
                ++breakout;

                NetState ns = (NetState)m_Disposed.Dequeue();

                ns.ClearWorldSendQueue();

                Mobile m = ns.m_Mobile;
                IAccount a = ns.m_Account;

                if (m != null)
                {
                    m.NetState = null;
                    ns.m_Mobile = null;
                }

                ns.m_Gumps.Clear();
                ns.m_Menus.Clear();
                ns.m_HuePickers.Clear();
                ns.m_Account = null;
                ns.m_ServerInfo = null;
                ns.m_CityInfo = null;

                m_Instances.Remove(ns);

                if (a != null)
                {
                    ns.WriteConsole("Disconnected. [{0} Online] [{1}]", m_Instances.Count, a);
                }
                else
                {
                    ns.WriteConsole("Disconnected. [{0} Online]", m_Instances.Count);
                }
            }
        }

        public bool Running
        {
            get { return m_Running; }
        }

        public bool Seeded
        {
            get { return m_Seeded; }
            set { m_Seeded = value; }
        }

        public Socket Socket
        {
            get { return m_Socket; }
        }

        public ByteQueue Buffer
        {
            get { return m_Buffer; }
        }

        public ExpansionInfo ExpansionInfo
        {
            get
            {
                for (int i = ExpansionInfo.Table.Length - 1; i >= 0; i--)
                {
                    ExpansionInfo info = ExpansionInfo.Table[i];

                    if (
                        (info.RequiredClient != null && this.Version >= info.RequiredClient)
                        || ((this.Flags & info.ClientFlags) != 0)
                    )
                    {
                        return info;
                    }
                }

                return ExpansionInfo.GetInfo(Expansion.None);
            }
        }

        public Expansion Expansion
        {
            get { return (Expansion)this.ExpansionInfo.ID; }
        }

        public bool SupportsExpansion(ExpansionInfo info, bool checkCoreExpansion)
        {
            if (info == null || (checkCoreExpansion && (int)Core.Expansion < info.ID))
                return false;

            if (info.RequiredClient != null)
                return (this.Version >= info.RequiredClient);

            return ((this.Flags & info.ClientFlags) != 0);
        }

        public bool SupportsExpansion(Expansion ex, bool checkCoreExpansion)
        {
            return SupportsExpansion(ExpansionInfo.GetInfo(ex), checkCoreExpansion);
        }

        public bool SupportsExpansion(Expansion ex)
        {
            return SupportsExpansion(ex, true);
        }

        public bool SupportsExpansion(ExpansionInfo info)
        {
            return SupportsExpansion(info, true);
        }
    }
}
