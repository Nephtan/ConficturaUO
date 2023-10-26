using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
    public class LearnMetalBook : Item
    {
        [Constructable]
        public LearnMetalBook()
            : base(0x02DD)
        {
            Weight = 1.0;
            Name = "Scroll of Various Metals";
            ItemID = Utility.RandomList(0x02DD, 0x201A);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("A Listing Of Metals");
        }

        public class LearnMetalGump : Gump
        {
            public LearnMetalGump(Mobile from)
                : base(50, 50)
            {
                string color = "#ddbc4b";

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);

                AddImage(0, 0, 9546, Server.Misc.PlayerSettings.GetGumpHue(from));

                AddHtml(
                    15,
                    15,
                    600,
                    20,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">INFORMATION ON VARIOUS TYPES OF METAL</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(867, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);

                AddItem(7, 72, 7153);
                AddHtml(
                    50,
                    76,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Iron</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 114, 7153, MaterialInfo.GetMaterialColor("dull copper", "", 0));
                AddHtml(
                    50,
                    118,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Dull Copper</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 156, 7153, MaterialInfo.GetMaterialColor("shadow iron", "", 0));
                AddHtml(
                    50,
                    160,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Shadow Iron</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 198, 7153, MaterialInfo.GetMaterialColor("copper", "", 0));
                AddHtml(
                    50,
                    202,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Copper</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 240, 7153, MaterialInfo.GetMaterialColor("bronze", "", 0));
                AddHtml(
                    50,
                    244,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Bronze</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 282, 7153, MaterialInfo.GetMaterialColor("gold", "", 0));
                AddHtml(
                    50,
                    286,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Gold</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 324, 7153, MaterialInfo.GetMaterialColor("agapite", "", 0));
                AddHtml(
                    50,
                    328,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Agapite</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 366, 7153, MaterialInfo.GetMaterialColor("verite", "", 0));
                AddHtml(
                    50,
                    370,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Verite</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 408, 7153, MaterialInfo.GetMaterialColor("valorite", "", 0));
                AddHtml(
                    50,
                    412,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Valorite</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 450, 7153, MaterialInfo.GetMaterialColor("nepturite", "", 0));
                AddHtml(
                    50,
                    454,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Nepturite</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 492, 7153, MaterialInfo.GetMaterialColor("obsidian", "", 0));
                AddHtml(
                    50,
                    496,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Obsidian</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 534, 7153, MaterialInfo.GetMaterialColor("steel", "", 0));
                AddHtml(
                    50,
                    538,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Steel</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 576, 7153, MaterialInfo.GetMaterialColor("brass", "", 0));
                AddHtml(
                    50,
                    580,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Brass</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 618, 7153, MaterialInfo.GetMaterialColor("mithril", "", 0));
                AddHtml(
                    50,
                    622,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Mithril</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 660, 7153, MaterialInfo.GetMaterialColor("xormite", "", 0));
                AddHtml(
                    50,
                    664,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Xormite</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(7, 702, 7153, MaterialInfo.GetMaterialColor("dwarven", "", 0));
                AddHtml(
                    50,
                    706,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Dwarven</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddHtml(
                    209,
                    67,
                    679,
                    702,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">Mining is the skill needed to find ore in caves and mountains. This ore can be crafted into ingots, which blacksmiths and tinkers can use to create weapons, armor, and tools. To start mining, simply equip a pickaxe or shovel, double-click it, and target a mountain or cave floor. Initially, you will find regular ore, but as you gain more experience, you will be able to discover different types of ore.<br><br>The various types of metal are listed below, starting from lower quality and progressing to higher quality. For example, a shield made from valorite will be superior to one made from copper. The same applies to weapons forged from different metals. The color of the metal used will determine the color of the weapon or armor. This also applies to many metal containers. A metal chest made from shadow iron, for instance, will be black in color.<br><br>To create items from ore, you must first turn the ore into ingots. Double-click the ore and target a forge. Forges are commonly found in blacksmith shops. Once you have the ingots, you can begin crafting using a blacksmith hammer or tinkering tools. Remember that you must be near a forge and anvil to smith items.</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x249);
            }
        }

        public override void OnDoubleClick(Mobile e)
        {
            if (!IsChildOf(e.Backpack) && this.Weight != -50.0)
            {
                e.SendMessage("This must be in your backpack to read.");
            }
            else
            {
                e.CloseGump(typeof(LearnMetalGump));
                e.SendGump(new LearnMetalGump(e));
                e.PlaySound(0x249);
                Server.Gumps.MyLibrary.readBook(this, e);
            }
        }

        public LearnMetalBook(Serial serial)
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
