using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class DoomFlayerNote : Item
    {
        [Constructable]
        public DoomFlayerNote()
            : base(0xE34)
        {
            Weight = 1.0;
            Hue = 0xB98;
            Name = "a dusty scroll";
            ItemID = 0x14EE;
        }

        public class ClueGump : Gump
        {
            public ClueGump(Mobile from)
                : base(100, 100)
            {
                from.PlaySound(0x249);
                string sText =
                    "The demon opened the black gate and unleashed chaos across Lodoria, where the dwarven armies had been defeated. The elven forces gathered their magic and banished the beast back to the void. Although the world had provided the natural forces to summon the demon, its power had now diminished to the core of the world. For centuries, we had been searching for this place deep beneath the city of Lodoria. The drow joined our cause and assisted in the construction of our hidden city below. Here, we could continue our secret search. With my research in Doom completed, I will now return to the cemetery at night to avoid being seen.";

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);

                AddImage(0, 0, 10901, 2786);
                AddImage(0, 0, 10899, 2117);
                AddHtml(
                    45,
                    78,
                    386,
                    218,
                    @"<BODY><BASEFONT Color=#d9c781>" + sText + "</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.PlaySound(0x249);
            }
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m.InRange(this.GetWorldLocation(), 2))
            {
                m.SendGump(new ClueGump(m));
                m.PlaySound(0x249);
            }
            else
            {
                m.SendLocalizedMessage(502138); // That is too far away for you to use
            }
        }

        public DoomFlayerNote(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
