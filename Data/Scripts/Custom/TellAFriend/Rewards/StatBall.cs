//   RunUO script: StatBall
//   Copyright (c) 2003 by Philantrop (Wulf C. Krueger <wk@mailstation.de>)
//
//   This script is free software; you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation; version 2 of the License applies.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//   http://www.gnu.org/licenses/gpl.html
//
//   In short: Whatever you do to this script, you MUST publish it and
//   let it be used for free.
//
//   More of my scripts can be found on: http://www.mailstation.de
//
//   Please do NOT remove or change this header.

// v1.1 Ixtabay - If Stat + StatBonus > 125, then Stat = 125

using System;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class StatBall : Item
    {
        private int m_StatBonus = 10;
        private string m_BaseName = "a stat ball +";
        public bool GumpOpen = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StatBonus
        {
            get { return m_StatBonus; }
            set
            {
                m_StatBonus = value;
                this.Name = m_BaseName + Convert.ToString(m_StatBonus);
            }
        }

        [Constructable]
        public StatBall(int StatBonus)
            : base(6249)
        {
            m_StatBonus = StatBonus;
            Name = m_BaseName + Convert.ToString(StatBonus);
        }

        [Constructable]
        public StatBall()
            : base(6249)
        {
            Name = m_BaseName + Convert.ToString(StatBonus);
        }

        public StatBall(Serial serial)
            : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if ((this.StatBonus == 0) && (from.AccessLevel < AccessLevel.GameMaster))
            {
                from.SendMessage("This Stat Ball isn't charged. Please page for a GM.");
                return;
            }
            else if ((from.AccessLevel >= AccessLevel.GameMaster) && (this.StatBonus == 0))
            {
                from.SendGump(new PropertiesGump(from, this));
                return;
            }

            if (!IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else if (!GumpOpen)
            {
                GumpOpen = true;
                from.SendGump(new StatBallGump(from, this));
            }
            else if (GumpOpen)
                from.SendMessage("You're already using the ball.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_StatBonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                {
                    m_StatBonus = reader.ReadInt();
                    break;
                }
            }
        }
    }
}

namespace Server.Gumps
{
    public class StatBallGump : Gump
    {
        private StatBall m_skb;

        public StatBallGump(Mobile from, StatBall skb)
            : base(20, 30)
        {
            m_skb = skb;

            AddPage(0);
            AddBackground(0, 0, 260, 115, 5054);
            AddImageTiled(10, 10, 240, 23, 0x52);
            AddImageTiled(11, 11, 238, 21, 0xBBC);
            AddLabel(65, 11, 0, "Stats you can raise");

            int Strength = from.RawStr; // (sic!)
            int Dexterity = from.RawDex;
            int Intelligence = from.RawInt;

            if ((Strength) < 125)
            {
                AddImageTiled(10, 32 + (0 * 22), 240, 23, 0x52);
                AddImageTiled(11, 33 + (0 * 22), 238, 21, 0xBBC);
                AddLabelCropped(13, 33 + (0 * 22), 150, 21, 0, "Strength");
                AddImageTiled(180, 34 + (0 * 22), 50, 19, 0x52);
                AddImageTiled(181, 35 + (0 * 22), 48, 17, 0xBBC);
                AddLabelCropped(182, 35 + (0 * 22), 234, 21, 0, Strength.ToString("F1"));

                if (from.AccessLevel >= AccessLevel.Player)
                    AddButton(231, 35 + (0 * 22), 0x15E1, 0x15E5, 1, GumpButtonType.Reply, 0);
                else
                    AddImage(231, 35 + (0 * 22), 0x2622);
            }

            if ((Dexterity) < 125)
            {
                AddImageTiled(10, 32 + (1 * 22), 240, 23, 0x52);
                AddImageTiled(11, 33 + (1 * 22), 238, 21, 0xBBC);
                AddLabelCropped(13, 33 + (1 * 22), 150, 21, 0, "Dexterity");
                AddImageTiled(180, 34 + (1 * 22), 50, 19, 0x52);
                AddImageTiled(181, 35 + (1 * 22), 48, 17, 0xBBC);
                AddLabelCropped(182, 35 + (1 * 22), 234, 21, 0, Dexterity.ToString("F1"));

                if (from.AccessLevel >= AccessLevel.Player)
                    AddButton(231, 35 + (1 * 22), 0x15E1, 0x15E5, 2, GumpButtonType.Reply, 0);
                else
                    AddImage(231, 35 + (1 * 22), 0x2622);
            }

            if ((Intelligence) < 125)
            {
                AddImageTiled(10, 32 + (2 * 22), 240, 23, 0x52);
                AddImageTiled(11, 33 + (2 * 22), 238, 21, 0xBBC);
                AddLabelCropped(13, 33 + (2 * 22), 150, 21, 0, "Intelligence");
                AddImageTiled(180, 34 + (2 * 22), 50, 19, 0x52);
                AddImageTiled(181, 35 + (2 * 22), 48, 17, 0xBBC);
                AddLabelCropped(182, 35 + (2 * 22), 234, 21, 0, Intelligence.ToString("F1"));

                if (from.AccessLevel >= AccessLevel.Player)
                    AddButton(231, 35 + (2 * 22), 0x15E1, 0x15E5, 3, GumpButtonType.Reply, 0);
                else
                    AddImage(231, 35 + (2 * 22), 0x2622);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if ((from == null) || (m_skb.Deleted))
                return;

            m_skb.GumpOpen = false;

            if (info.ButtonID > 0)
            {
                int count = 0;
                count = from.RawStr + from.RawDex + from.RawInt;

                if ((count + m_skb.StatBonus) > (from.StatCap))
                {
                    from.SendMessage("You cannot exceed the " + from.StatCap + " Stat cap.");
                    return;
                }

                switch (info.ButtonID)
                {
                    case 1:
                    { // -------------------------------------------------------- Strength
                        if (from.RawStr + m_skb.StatBonus <= 125)
                        {
                            from.RawStr += m_skb.StatBonus;
                            m_skb.Delete();
                        }
                        else if (from.RawStr + m_skb.StatBonus > 125)
                        {
                            from.RawStr = 125;
                            m_skb.Delete();
                        }
                        else
                            from.SendMessage("Please choose another Stat.");
                        break;
                    }
                    case 2:
                    { // -------------------------------------------------------- Dexterity
                        if (from.RawDex + m_skb.StatBonus <= 125)
                        {
                            from.RawDex += m_skb.StatBonus;
                            m_skb.Delete();
                        }
                        else if (from.RawDex + m_skb.StatBonus > 125)
                        {
                            from.RawDex = 125;
                            m_skb.Delete();
                        }
                        else
                            from.SendMessage("Please choose another Stat.");
                        break;
                    }
                    case 3:
                    { // -------------------------------------------------------- Intelligence
                        if (from.RawInt + m_skb.StatBonus <= 125)
                        {
                            from.RawInt += m_skb.StatBonus;
                            m_skb.Delete();
                        }
                        else if (from.RawInt + m_skb.StatBonus > 125)
                        {
                            from.RawInt = 125;
                            m_skb.Delete();
                        }
                        else
                            from.SendMessage("Please choose another Stat.");
                        break;
                    }
                }
            }
        }
    }
}
