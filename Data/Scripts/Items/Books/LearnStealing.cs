using System;
using System.Text;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class LearnStealingBook : Item
    {
        [Constructable]
        public LearnStealingBook()
            : base(0x02DD)
        {
            Weight = 1.0;
            Name = "The Art of Thievery";
            ItemID = Utility.RandomList(0x02DD, 0x201A);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("What To Steal For Better Profit");
        }

        public class LearnStealingGump : Gump
        {
            public LearnStealingGump(Mobile from)
                : base(50, 50)
            {
                string color = "#ddbc4b";

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);

                AddImage(0, 0, 9547, Server.Misc.PlayerSettings.GetGumpHue(from));

                AddHtml(
                    15,
                    15,
                    398,
                    20,
                    @"<BODY><BASEFONT Color=" + color + ">THE ART OF THIEVERY</BASEFONT></BODY>",
                    (bool)false,
                    (bool)false
                );

                AddButton(567, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);

                AddHtml(
                    14,
                    50,
                    579,
                    388,
                    @"<BODY><BASEFONT Color="
                        + color
                        + ">For those skilled in the art of snooping and stealing, the search for ancient artifacts can be a profitable venture. Searching crypts, tombs, and dungeons, you may come across pedestals with ornately crafted boxes and bags that could contain valuable items. These items could be rare treasures, fine art pieces, or ancient weapons. The finely crafted bags and boxes can be kept for oneself or sold to a thief in the guild, who will gladly pay 500 gold for each one. These items are highly collectible and the guild has contacts to resell them to royalty, art dealers, or collectors. When you encounter these pedestals with an item on top, double click it to attempt to steal it. If you lack proper training in snooping, you might trigger a deadly trap. Having good trap removal skills can help avoid the effects of such traps. Once the trap is avoided, your stealing skills will be put to the test. If you successfully obtain the item, open it up and claim your prize.<br><br>Many people in town are actively searching for rare artifacts and are willing to pay handsomely for them.<br><br>There are also footlockers, chests, bags, and boxes that contain treasure in these locations. You can attempt to steal from these containers. Remember to take what you want from them before stealing them, as you will empty the container during your escape. A thief in the guild may also pay money for these containers, as they are collectible items and can fetch a good price. To take one of these dungeon containers, use your stealing skill and target the container. Perhaps you will be quick enough.<br><br>Although you can also acquire gold by picking the pockets of merchants, you can also steal gold from their coffers. You can snoop the coffers to see how much gold they contain, and then use your stealing skill to try and take the gold. This can be a tricky maneuver if you are caught. You can steal coins and other items from creatures by standing next to them and attacking, where you may automatically steal such items during the attack.</BASEFONT></BODY>",
                    (bool)false,
                    (bool)true
                );

                AddItem(554, 449, 4643);
                AddItem(19, 457, 13042);
                AddItem(554, 447, 3702);
                AddItem(388, 484, 5373);
                AddItem(18, 459, 3712);
                AddItem(370, 461, 7183);
                AddItem(202, 458, 13111);
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
                e.CloseGump(typeof(LearnStealingGump));
                e.SendGump(new LearnStealingGump(e));
                e.PlaySound(0x249);
                Server.Gumps.MyLibrary.readBook(this, e);
            }
        }

        public LearnStealingBook(Serial serial)
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
