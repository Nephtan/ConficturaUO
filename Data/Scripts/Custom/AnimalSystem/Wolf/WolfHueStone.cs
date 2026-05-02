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
    public class WolfHueStone : Item
    {
        [Constructable]
        public WolfHueStone()
            : base(0x1870)
        {
            Name = "A Wolf Hue Stone";
            Movable = false;
            LootType = LootType.Blessed;
            Hue = 36;
        }

        public WolfHueStone(Serial serial)
            : base(serial)
        {
        }

        public int m_Hued;
        public int m_SkinHue;
        public int s_Hue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SHue
        {
            get { return s_Hue; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hued
        {
            get { return m_Hued; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SkinHue
        {
            get { return m_SkinHue; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            //if (m_Hued == 1)
            //{
            //    UnColor(from);
            //}
            //else
            //{
                Color(from);
            //}
        }

        public override bool HandlesOnSpeech { get { return true; } }
/*
        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && this.IsChildOf(e.Mobile.Backpack))
            {
                string keyword = e.Speech;
                switch (keyword)
                {
                    case "color":
                        {
                            if (m_Hued == 0)
                            {
                                Color(e.Mobile);
                                e.Handled = true;
                            }
                            break;
                        }
                    case "uncolor":
                        {
                            if (m_Hued == 1)
                            {
                                UnColor(e.Mobile);
                                e.Handled = true;
                            }
                            break;
                        }
                }
            }
            base.OnSpeech(e);
        }
*/
        public void Color(Mobile from)
        {
            m_Hued = 1;
            m_SkinHue = SHue;
            from.Hue = SHue;
        }

        public void UnColor(Mobile from)
        {
            m_Hued = 0;
            from.Hue = m_SkinHue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version 
            writer.Write((int)s_Hue);
            writer.Write((int)m_Hued);
            writer.Write((int)m_SkinHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        s_Hue = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Hued = reader.ReadInt();
                        m_SkinHue = reader.ReadInt();
                        break;
                    }
            }
        }

        private class HueGump : Gump
        {
            private WolfHueStone wss;
            private Mobile user;

            public HueGump(WolfHueStone huestone, Mobile from)
                : base(150, 150)
            {
                wss = huestone;
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
                this.AddLabel(71, 18, 0, @"@@@@@");
                this.AddLabel(71, 44, 646, @"default");
                this.AddLabel(71, 72, 642, @"@@@@@");
                this.AddLabel(71, 99, 644, @"@@@@@");
                this.AddLabel(71, 128, 1153, @"@@@@@");

            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 1: wss.s_Hue = 1175; break;
                    case 2: wss.s_Hue = 33805; break;
                    case 3: wss.s_Hue = 644; break;
                    case 4: wss.s_Hue = 646; break;
                    case 5: wss.s_Hue = 1153; break;
                }
                from.CloseGump(typeof(HueGump));

            }
        }

        //public override void GetContextMenuEntries(Mobile from, ArrayList list)
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
                list.Add(new ChangeHue(this, from));

            base.GetContextMenuEntries(from, list);
        }
        private class ChangeHue : ContextMenuEntry
        {
            private WolfHueStone WSS;
            private Mobile m_From;

            public ChangeHue(WolfHueStone wsss, Mobile from)
                : base(0313, 1)
            {
                WSS = wsss;
                m_From = from;
            }

            public override void OnClick()
            {
                m_From.SendGump(new HueGump(WSS, m_From));
            }
        }

    }
}
