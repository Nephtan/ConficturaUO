using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Server;
using Server.Accounting;
using Server.ContextMenus;
using Server.Items;

namespace Server.Items
{
    public class RandomGiftBox : BaseContainer
    {
        [Constructable]
        public RandomGiftBox()
            : base(
                Utility.RandomDouble() > 0.75d
                    ? Utility.Random(0x232A, 2)
                    : Utility.Random(0x46A2, 6)
            )
        {
            SetHue();
        }

        public RandomGiftBox(Serial serial)
            : base(serial) { }

        private void SetHue()
        {
            if (ItemID <= 0x232B)
                Hue = GiftBoxHues.RandomNeonBoxHue;
            else
                Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public override int DefaultGumpID
        {
            get
            {
                switch (ItemID)
                {
                    case 0x232A:
                        return 0x11B;
                    case 0x232B:
                        return 0x11B;
                    case 0x46A2:
                        return 0x11B;
                    case 0x46A3:
                        return 0x11C;
                    case 0x46A4:
                        return 0x11D;
                    case 0x46A5:
                        return 0x11E;
                    case 0x46A6:
                        return 0x11E;
                    default:
                        return 0x11F;
                }
            }
        }

        public override void OnAfterDuped(Item newItem)
        {
            RandomGiftBox that = (RandomGiftBox)newItem;
            that.ItemID =
                Utility.RandomDouble() > 0.75d
                    ? Utility.Random(0x232A, 2)
                    : Utility.Random(0x46A2, 6);
            that.SetHue();
            that.InvalidateProperties();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt(); // Version
        }
    }
}

namespace Server.Customs
{
    public class PandorasGiftBox : RandomGiftBox
    {
        private bool m_OnePerAccount = true;
        private bool m_Active = false;
        private string m_Message = null;
        private List<Mobile> m_MobilesGivenTo = new List<Mobile>();
        private List<string> m_AccountsGivenTo = new List<string>();

        [CommandProperty(AccessLevel.GameMaster)]
        public bool OnePerAccount
        {
            get { return m_OnePerAccount; }
            set
            {
                m_OnePerAccount = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set
            {
                m_Active = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Message
        {
            get { return m_Message; }
            set
            {
                m_Message = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public PandorasGiftBox()
            : base()
        {
            Initialize();
        }

        public PandorasGiftBox(Serial serial)
            : base(serial)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Movable = false;
            this.Name = "Pandora's Gift Box";
            EventSink.CharacterCreated += EventSink_CharacterCreated;
            EventSink.Login += EventSink_Login;
        }

        public void EventSink_CharacterCreated(CharacterCreatedEventArgs e)
        {
            TryGift(e.Mobile);
        }

        public void EventSink_Login(LoginEventArgs e)
        {
            TryGift(e.Mobile);
        }

        public static void CopyProperties(Item dest, Item src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].CanRead && props[i].CanWrite)
                    {
                        props[i].SetValue(dest, props[i].GetValue(src, null), null);
                    }
                }
                catch { }
            }
        }

        private void Dupe(Server.Items.Container pack, Item copy)
        {
            bool done = false;
            Type t = copy.GetType();
            ConstructorInfo c = t.GetConstructor(Type.EmptyTypes);

            if (c != null)
            {
                try
                {
                    object o = c.Invoke(null);

                    if (o != null && o is Item)
                    {
                        Item newItem;

                        // Check if the item is a HalloweenBag
                        if (copy is HalloweenBag)
                        {
                            newItem = new HalloweenBag();
                        }
                        else
                        {
                            newItem = (Item)o;
                            CopyProperties(newItem, copy);
                            copy.OnAfterDuped(newItem);
                        }

                        newItem.Parent = null;
                        pack.DropItem(newItem);
                        newItem.InvalidateProperties();

                        if (copy is Server.Items.Container)
                        {
                            DupeContainer(
                                (Server.Items.Container)copy,
                                (Server.Items.Container)newItem
                            );
                        }
                    }

                    done = true;
                }
                catch { }
            }

            if (!done)
            {
                Console.WriteLine(
                    "Unable to copy an item in a Pandora's Gift Box. All items must have a 0 parameter constructor."
                );
            }
        }

        private void DupeContainer(Server.Items.Container from, Server.Items.Container to)
        {
            foreach (Item item in from.Items)
            {
                Dupe(to, item);
            }
        }

        public void TryGift(Mobile m)
        {
            if (
                m == null
                || m.Backpack == null
                || m.BankBox == null
                || !m_Active
                || Items.Count <= 0
            )
                return;
            if (m_OnePerAccount && m_AccountsGivenTo.Contains(m.Account.Username))
                return;
            if (!m_OnePerAccount && m_MobilesGivenTo.Contains(m))
                return;

            if (m.Backpack.CheckHold(m, this, false))
            {
                DupeContainer(this, m.Backpack);
                if (m_Message != null)
                    m.SendMessage(0x4, m_Message + " A gift has been placed in your backpack.");
                else
                    m.SendMessage(0x4, "A gift has been placed in your backpack.");
            }
            else
            {
                DupeContainer(this, m.BankBox);
                if (m_Message != null)
                    m.SendMessage(0x4, m_Message + " A gift has been placed in your bank box.");
                else
                    m.SendMessage(0x4, "A gift has been placed in your bank box.");
            }

            if (m_OnePerAccount)
            {
                if (!m_AccountsGivenTo.Contains(m.Account.Username))
                {
                    m_AccountsGivenTo.Add(m.Account.Username);
                }
            }
            else if (!m_MobilesGivenTo.Contains(m))
            {
                m_MobilesGivenTo.Add(m);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            // Only staff can open the box
            if (from.AccessLevel >= AccessLevel.GameMaster)
                base.OnDoubleClick(from);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(m_OnePerAccount ? "One per Account" : "One per Character");
            list.Add(m_Active ? "Active" : "Disabled");
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                list.Add(new EnableContextMenuEntry(!m_Active, this));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // Version

            writer.Write(m_OnePerAccount);
            writer.Write(m_Active);
            writer.Write(m_Message);
            writer.WriteMobileList(m_MobilesGivenTo);
            string[] accounts = m_AccountsGivenTo.ToArray();
            writer.Write(accounts.Length);
            foreach (string account in accounts)
            {
                writer.Write(account);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    m_OnePerAccount = reader.ReadBool();
                    m_Active = reader.ReadBool();
                    m_Message = reader.ReadString();
                    m_MobilesGivenTo.Clear();
                    ArrayList mobileList = reader.ReadMobileList();
                    m_MobilesGivenTo.Clear();
                    foreach (Mobile mob in mobileList)
                    {
                        m_MobilesGivenTo.Add(mob);
                    }
                    m_AccountsGivenTo.Clear();
                    int count = reader.ReadInt();
                    for (int i = 0; i < count; ++i)
                    {
                        m_AccountsGivenTo.Add(reader.ReadString());
                    }
                    break;
            }
        }

        private class EnableContextMenuEntry : ContextMenuEntry
        {
            private bool m_Activate;
            private PandorasGiftBox m_Parent;

            public EnableContextMenuEntry(bool activate, PandorasGiftBox parent)
                : base(activate ? 1011034 : 1011035)
            {
                m_Activate = activate;
                m_Parent = parent;
            }

            public override void OnClick()
            {
                m_Parent.Active = m_Activate;
            }
        }
    }
}
