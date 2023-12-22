using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;
using Server.Regions;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    [FlipableAttribute(0x1868, 0x189D)]
    public class ObsidianGate : Item, ISecurable
    {
        public SecureLevel m_Level;

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        [Constructable]
        public ObsidianGate()
            : base(0x1868)
        {
            Name = "Blackrock Gate";
            Light = LightType.Circle300;
            Weight = 25.0;
            m_Level = SecureLevel.Anyone;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Use The Blackrock Gate For Traveling");
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Movable)
            {
                from.SendMessage("This must be secured down in a home to use.");
            }
            else if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendMessage("You will have to get closer to use that.");
            }
            else if (!CheckAccess(from))
            {
                from.SendMessage("You cannot seem to use this.");
            }
            else
            {
                from.PlaySound(0x20E);
                from.CloseGump(typeof(Server.Items.GateMoon.MoonGateGump));
                from.SendGump(new Server.Items.GateMoon.MoonGateGump(from, true));
                from.SendMessage("Choose a destination.");
            }

            return;
        }

        public bool CheckAccess(Mobile m)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && (house.Public ? house.IsBanned(m) : !house.HasAccess(m)))
                return false;

            return (house != null && house.HasSecureAccess(m, m_Level));
        }

        public ObsidianGate(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Level = (SecureLevel)reader.ReadInt();
        }
    }
}
