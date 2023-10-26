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
    public class LearnWoodBook : Item
    {
        [Constructable]
        public LearnWoodBook()
            : base(0x02DD)
        {
            Weight = 1.0;
            Name = "Scroll of Various Wood";
            ItemID = Utility.RandomList(0x02DD, 0x201A);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("A Listing of Wood");
        }

        public class LearnWoodBookGump : Gump
        {
            public LearnWoodBookGump(Mobile from)
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
                        + ">INFORMATION ON VARIOUS TYPES OF WOOD</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(867, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);

                AddItem(9, 72, 7137);
                AddHtml(
                    50,
                    76,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Regular</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 114, 7137, MaterialInfo.GetMaterialColor("ash", "", 0));
                AddHtml(
                    50,
                    118,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Ash</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 156, 7137, MaterialInfo.GetMaterialColor("cherry", "", 0));
                AddHtml(
                    50,
                    160,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Cherry</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 198, 7137, MaterialInfo.GetMaterialColor("ebony", "", 0));
                AddHtml(
                    50,
                    202,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Ebony</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 240, 7137, MaterialInfo.GetMaterialColor("golden oak", "", 0));
                AddHtml(
                    50,
                    244,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Golden Oak</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 282, 7137, MaterialInfo.GetMaterialColor("hickory", "", 0));
                AddHtml(
                    50,
                    286,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Hickory</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 324, 7137, MaterialInfo.GetMaterialColor("mahogany", "", 0));
                AddHtml(
                    50,
                    328,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Mahogany</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 366, 7137, MaterialInfo.GetMaterialColor("oak", "", 0));
                AddHtml(
                    50,
                    370,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Oak</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 408, 7137, MaterialInfo.GetMaterialColor("pine", "", 0));
                AddHtml(
                    50,
                    412,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Pine</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 450, 7137, MaterialInfo.GetMaterialColor("ghostwood", "", 0));
                AddHtml(
                    50,
                    454,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Ghostwood</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 492, 7137, MaterialInfo.GetMaterialColor("rosewood", "", 0));
                AddHtml(
                    50,
                    496,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Rosewood</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 534, 7137, MaterialInfo.GetMaterialColor("walnut", "", 0));
                AddHtml(
                    50,
                    538,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Walnut</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 576, 7137, MaterialInfo.GetMaterialColor("petrified", "", 0));
                AddHtml(
                    50,
                    580,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Petrified</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 618, 7137, MaterialInfo.GetMaterialColor("driftwood", "", 0));
                AddHtml(
                    50,
                    622,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Driftwood</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );
                AddItem(9, 660, 7137, MaterialInfo.GetMaterialColor("elven", "", 0));
                AddHtml(
                    50,
                    664,
                    122,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">Elven</BASEFONT></BODY>",
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
                        + ">Lumberjacking is a task that has been carried out for a long time, even before the mining of ore. To start chopping trees, all you need is an axe. Simply double-click the axe and then target a tree. Initially, you will obtain regular wood, but as you gain more skill, you will be able to chop different types of wood. With wood, you can craft arrows, bows, and crossbows using the bowcrafting skill. Additionally, you can create furniture, weapons, and armor using the carpentry skill.<br><br>Here is a list of the various types of wood, starting from lower quality and progressing to higher quality. For instance, a shield made of walnut will be superior to one made of ash. The same applies to bows, crossbows, and other wooden weapons. The color of the wood used will determine the color of the resulting weapon, armor, or instrument. This also applies to many wooden furniture and containers. For example, a wooden chest made from cherry wood will have a red color.<br><br>To create items from wood, you need to convert logs into boards. To do this, double-click on the logs and target a sawmill. These mills are commonly found in carpenter shops. Once you have the boards, you can then begin crafting using carpentry tools or bowcrafting tools.</BASEFONT></BODY>",
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
                e.CloseGump(typeof(LearnWoodBookGump));
                e.SendGump(new LearnWoodBookGump(e));
                e.PlaySound(0x249);
                Server.Gumps.MyLibrary.readBook(this, e);
            }
        }

        public LearnWoodBook(Serial serial)
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
