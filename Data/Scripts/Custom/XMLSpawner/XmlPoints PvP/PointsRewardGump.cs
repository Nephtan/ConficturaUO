using System;
using System.Collections;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

/*
** PointsRewardGump
** ArteGordon
** updated 11/08/04
**
** Gives out rewards based on the XmlPointsReward reward list entries and the players Credits that are accumulated through pvp with the XmlPoints attachment.
** The Gump supports Item, Mobile, and Attachment type rewards.
*/

namespace Server.Gumps
{
    public class PointsRewardGump : Gump
    {
        private ArrayList Rewards;

        private int y_inc = 35;
        private int x_creditoffset = 350;
        private int x_pointsoffset = 480;
        private int maxItemsPerPage = 9;
        private int viewpage;

        public PointsRewardGump(Mobile from, int page)
            : base(20, 30)
        {
            from.CloseGump(typeof(PointsRewardGump));

            // determine the gump size based on the number of rewards
            Rewards = XmlPointsRewards.RewardsList;

            viewpage = page;

            int height = maxItemsPerPage * y_inc + 120;
            int width = x_pointsoffset + 110;

            /*
            if(Rewards != null && Rewards.Count > 0)
            {
                height = Rewards.Count*y_inc + 120;
            }
            */

            AddBackground(0, 0, width, height, 0xDAC);

            AddHtml(
                40,
                20,
                350,
                50,
                "Rewards Available for Purchase with Points Credits",
                false,
                false
            );

            AddLabel(
                400,
                20,
                0,
                String.Format("Available Credits: {0}", XmlPoints.GetCredits(from))
            );

            //AddButton( 30, height - 35, 0xFB7, 0xFB9, 0, GumpButtonType.Reply, 0 );
            //AddLabel( 70, height - 35, 0, "Close" );

            // put the page buttons in the lower right corner
            if (Rewards != null && Rewards.Count > 0)
            {
                AddLabel(
                    width - 165,
                    height - 35,
                    0,
                    String.Format(
                        "Page: {0}/{1}",
                        viewpage + 1,
                        (int)(Rewards.Count / maxItemsPerPage) + 1
                    )
                );

                // page up and down buttons
                AddButton(width - 55, height - 35, 0x15E0, 0x15E4, 13, GumpButtonType.Reply, 0);
                AddButton(width - 35, height - 35, 0x15E2, 0x15E6, 12, GumpButtonType.Reply, 0);
            }

            AddLabel(70, 50, 40, "Reward");
            AddLabel(x_creditoffset, 50, 40, "Credits");
            AddLabel(x_pointsoffset, 50, 40, "Min Points");

            // display the items with their selection buttons
            if (Rewards != null)
            {
                int y = 50;
                for (int i = 0; i < Rewards.Count; i++)
                {
                    if ((int)(i / maxItemsPerPage) != viewpage)
                        continue;

                    XmlPointsRewards r = Rewards[i] as XmlPointsRewards;
                    if (r == null)
                        continue;

                    y += y_inc;

                    int texthue = 0;

                    // display the item
                    if (r.MinPoints > XmlPoints.GetPoints(from))
                    {
                        texthue = 33;
                    }
                    else
                    {
                        // add the selection button
                        AddButton(30, y, 0xFA5, 0xFA7, 1000 + i, GumpButtonType.Reply, 0);
                    }

                    // display the name
                    AddLabel(70, y + 3, texthue, r.Name);

                    // display the cost
                    AddLabel(x_creditoffset, y + 3, texthue, r.Cost.ToString());

                    // display the item
                    if (r.ItemID > 0)
                        AddItem(x_creditoffset + 60, y, r.ItemID);

                    // display the min points requirement
                    AddLabel(x_pointsoffset, y + 3, texthue, r.MinPoints.ToString());
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info == null || state == null || state.Mobile == null || Rewards == null)
                return;

            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 12:
                    // page up
                    int nitems = 0;
                    if (Rewards != null)
                        nitems = Rewards.Count;

                    int page = viewpage + 1;
                    if (page > (int)(nitems / maxItemsPerPage))
                    {
                        page = (int)(nitems / maxItemsPerPage);
                    }
                    state.Mobile.SendGump(new PointsRewardGump(state.Mobile, page));
                    break;
                case 13:
                    // page down
                    page = viewpage - 1;
                    if (page < 0)
                    {
                        page = 0;
                    }
                    state.Mobile.SendGump(new PointsRewardGump(state.Mobile, page));
                    break;
                default:
                {
                    if (info.ButtonID >= 1000)
                    {
                        int selection = info.ButtonID - 1000;
                        if (selection < Rewards.Count)
                        {
                            XmlPointsRewards r = Rewards[selection] as XmlPointsRewards;

                            // check the price
                            if (XmlPoints.HasCredits(from, r.Cost))
                            {
                                // create an instance of the reward type
                                object o = null;

                                try
                                {
                                    o = Activator.CreateInstance(r.RewardType, r.RewardArgs);
                                }
                                catch { }

                                bool received = true;

                                if (o is Item)
                                {
                                    // and give them the item
                                    from.AddToBackpack((Item)o);
                                }
                                else if (o is Mobile)
                                {
                                    // if it is controllable then set the buyer as master.  Note this does not check for control slot limits.
                                    if (o is BaseCreature)
                                    {
                                        BaseCreature b = o as BaseCreature;
                                        b.Controlled = true;
                                        b.ControlMaster = from;
                                    }

                                    ((Mobile)o).MoveToWorld(from.Location, from.Map);
                                }
                                else if (o is XmlAttachment)
                                {
                                    XmlAttachment a = o as XmlAttachment;

                                    XmlAttach.AttachTo(from, a);
                                }
                                else
                                {
                                    from.SendMessage(
                                        33,
                                        "unable to create {0}.",
                                        r.RewardType.Name
                                    );
                                    received = false;
                                }

                                // complete the transaction
                                if (received)
                                {
                                    // charge them
                                    XmlPoints.TakeCredits(from, r.Cost);
                                    from.SendMessage(
                                        "You have purchased {0} for {1} credits.",
                                        r.Name,
                                        r.Cost
                                    );
                                }
                            }
                            else
                            {
                                from.SendMessage("Insufficient Credits for {0}.", r.Name);
                            }
                            from.SendGump(new PointsRewardGump(from, viewpage));
                        }
                    }
                    break;
                }
            }
        }
    }
}
