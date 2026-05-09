using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.ContextMenus;

namespace Server.Items
{
    public class DragonShapeShiftStone : Item
    {
        [Constructable]
        public DragonShapeShiftStone()
            : base(0x1870)
        {
            Name = "a Dragon ShapeShift Stone";
            Movable = false;
            LootType = LootType.Blessed;
            Hue = 37;
        }

        public DragonShapeShiftStone(Serial serial)
            : base(serial)
        {
        }

        public int m_Transformed;
        public int m_SkinBodyValue;
        public int m_BodyValue;
        public int s_BodyValue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SBodyValue
        {
            get { return s_BodyValue; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Transformed
        {
            get { return m_Transformed; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SkinBodyValue
        {
            get { return m_SkinBodyValue; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BodyValue
        {
            get { return m_BodyValue; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Transformed == 1)
            {
                UnTransform(from);
            }
            else
            {
                Transform(from);
            }
        }

        public override bool HandlesOnSpeech { get { return true; } }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && this.IsChildOf(e.Mobile.Backpack))
            {
                string keyword = e.Speech;
                switch (keyword)
                {
                    case "transform":
                        {
                            if (m_Transformed == 0)
                            {
                                Transform(e.Mobile);
                                e.Handled = true;
                            }
                            break;
                        }
                    case "untransform":
                        {
                            if (m_Transformed == 1)
                            {
                                UnTransform(e.Mobile);
                                e.Handled = true;
                            }
                            break;
                        }
                }
            }

            base.OnSpeech(e);
        }

        public void Transform(Mobile from)
        {
            m_Transformed = 1;
            if (from.Female)
                m_BodyValue = 401;
            else
                m_BodyValue = 400;
            from.BodyValue = SBodyValue;
        }

        public void UnTransform(Mobile from)
        {
            m_Transformed = 0;
            from.BodyValue = m_BodyValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version 
            writer.Write((int)s_BodyValue);
            writer.Write((int)m_Transformed);
            writer.Write((int)m_SkinBodyValue);
            writer.Write((int)m_BodyValue);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        s_BodyValue = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Transformed = reader.ReadInt();
                        m_SkinBodyValue = reader.ReadInt();
                        m_BodyValue = reader.ReadInt();
                        break;
                    }
            }
        }

        private class BodyValueGump : Gump
        {
            private DragonShapeShiftStone wss;
            private Mobile user;

            public BodyValueGump(DragonShapeShiftStone shiftstone, Mobile from)
                : base(150, 150)
            {
                wss = shiftstone;
                user = from;

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);
                this.AddBackground(2, -1, 172, 169, 9500);
                this.AddButton(16, 18, 209, 208, 1, GumpButtonType.Reply, 0);
                this.AddButton(16, 44, 209, 208, 2, GumpButtonType.Reply, 0);
                this.AddButton(16, 72, 209, 208, 3, GumpButtonType.Reply, 0);
                this.AddButton(16, 99, 209, 208, 4, GumpButtonType.Reply, 0);
                this.AddButton(16, 128, 209, 208, 5, GumpButtonType.Reply, 0);
                this.AddLabel(71, 18, 0, @"BigBrown");
                this.AddLabel(71, 44, 0, @"BigRed");
                this.AddLabel(71, 72, 0, @"SmlBrown");
                this.AddLabel(71, 99, 0, @"SmlRed");
                this.AddLabel(71, 128, 0, @"Wyvern");

            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 1: wss.s_BodyValue = 12; break;
                    case 2: wss.s_BodyValue = 59; break;
                    case 3: wss.s_BodyValue = 60; break;
                    case 4: wss.s_BodyValue = 61; break;
                    case 5: wss.s_BodyValue = 62; break;
                }
                from.CloseGump(typeof(BodyValueGump));

            }
        }

        //public override void GetContextMenuEntries(Mobile from, ArrayList list)
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
                list.Add(new ChangeBodyValue(this, from));

            base.GetContextMenuEntries(from, list);
        }

        private class ChangeBodyValue : ContextMenuEntry
        {
            private DragonShapeShiftStone WSS;
            private Mobile m_From;

            public ChangeBodyValue(DragonShapeShiftStone wsss, Mobile from)
                : base(0313, 3)
            {
                WSS = wsss;
                m_From = from;
            }

            public override void OnClick()
            {
                m_From.SendGump(new BodyValueGump(WSS, m_From));
            }
        }

    }
}
