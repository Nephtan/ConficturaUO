using System;
using System.Collections;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

/*
** QuestRewardGump
** ArteGordon
** updated 9/18/05
**
** Gives out rewards based on the XmlQuestReward reward list entries and the players Credits that are accumulated through quests with the XmlQuestPoints attachment.
** The Gump supports Item, Mobile, and Attachment type rewards.
*/

namespace Server.Gumps
{
    public class QuestRewardGump : Gump
    {
        private ArrayList Rewards;
        private ArrayList m_Offers = new ArrayList();
        private Mobile m_From;
        private bool m_Responded;

        private int y_inc = 35;
        private int x_creditoffset = 350;
        private int x_pointsoffset = 480;
        private int maxItemsPerPage = 9;
        private int viewpage;

        public QuestRewardGump(Mobile from, int page)
            : base(20, 30)
        {
            from.CloseGump(typeof(QuestRewardGump));

            // determine the gump size based on the number of rewards
            Rewards = (ArrayList)XmlQuestPointsRewards.RewardsList.Clone();
            m_From = from;
            foreach (object reward in Rewards)
                m_Offers.Add(new XmlRewardOffer(reward));

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
                "Rewards Available for Purchase with QuestPoints Credits",
                false,
                false
            );

            AddLabel(
                400,
                20,
                0,
                String.Format("Available Credits: {0}", XmlQuestPoints.GetCredits(from))
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

                    XmlQuestPointsRewards r = Rewards[i] as XmlQuestPointsRewards;
                    if (r == null)
                        continue;

                    y += y_inc;

                    int texthue = 0;

                    // display the item
                    if (r.MinPoints > XmlQuestPoints.GetPoints(from))
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
            if (m_Responded || info == null || state == null || state.Mobile == null
                || state.Mobile != m_From || state.Mobile.Deleted || Rewards == null)
                return;

            m_Responded = true;
            Mobile from = state.Mobile;
            if (info.ButtonID <= 0)
                return;

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
                    state.Mobile.SendGump(new QuestRewardGump(state.Mobile, page));
                    break;
                case 13:
                    // page down
                    page = viewpage - 1;
                    if (page < 0)
                    {
                        page = 0;
                    }
                    state.Mobile.SendGump(new QuestRewardGump(state.Mobile, page));
                    break;
                default:
                {
                    int selection = info.ButtonID - 1000;
                    if (selection >= 0 && selection < m_Offers.Count
                        && selection / maxItemsPerPage == viewpage)
                    {
                        XmlRewardOffer offer = (XmlRewardOffer)m_Offers[selection];
                        offer.Redeem(from, XmlQuestPointsRewards.RewardsList, selection, true);
                        from.SendGump(new QuestRewardGump(from, viewpage));
                    }
                    break;
                }
            }
        }
    }
}

namespace Server.Gumps
{
    // A menu offer is a transient quote; mutable reward configuration must still match it at redemption.
    internal sealed class XmlRewardOffer
    {
        private object m_Entry;
        private Type m_Type;
        private object[] m_Args;
        private int m_Cost;
        private int m_MinPoints;
        private string m_Name;

        internal XmlRewardOffer(object entry)
        {
            m_Entry = entry;
            XmlQuestPointsRewards quest = entry as XmlQuestPointsRewards;
            XmlPointsRewards points = entry as XmlPointsRewards;
            if (quest != null)
            {
                m_Type = quest.RewardType; m_Cost = quest.Cost; m_MinPoints = quest.MinPoints;
                m_Name = quest.Name; m_Args = quest.RewardArgs;
            }
            else if (points != null)
            {
                m_Type = points.RewardType; m_Cost = points.Cost; m_MinPoints = points.MinPoints;
                m_Name = points.Name; m_Args = points.RewardArgs;
            }
            if (m_Args != null)
                m_Args = (object[])m_Args.Clone();
        }

        private bool Eligible(Mobile from, ArrayList current, int selection, bool quest)
        {
            if (from == null || from.Deleted || from.Map == null || from.Map == Map.Internal
                || current == null || selection < 0 || selection >= current.Count
                || current[selection] != m_Entry || m_Type == null || m_Cost < 0 || m_MinPoints < 0)
                return false;

            XmlRewardOffer now = new XmlRewardOffer(current[selection]);
            if (now.m_Type != m_Type || now.m_Cost != m_Cost || now.m_MinPoints != m_MinPoints || now.m_Name != m_Name)
                return false;
            if ((m_Args == null) != (now.m_Args == null))
                return false;
            if (m_Args != null)
            {
                if (m_Args.Length != now.m_Args.Length)
                    return false;
                for (int i = 0; i < m_Args.Length; ++i)
                    if (!Object.Equals(m_Args[i], now.m_Args[i]))
                        return false;
            }
            return quest
                ? XmlQuestPoints.GetPoints(from) >= m_MinPoints && XmlQuestPoints.HasCredits(from, m_Cost)
                : XmlPoints.GetPoints(from) >= m_MinPoints && XmlPoints.HasCredits(from, m_Cost);
        }

        private bool TakeCredits(Mobile from, int amount, bool quest)
        {
            return quest ? XmlQuestPoints.TakeCredits(from, amount) : XmlPoints.TakeCredits(from, amount);
        }

        internal void Redeem(Mobile from, ArrayList current, int selection, bool quest)
        {
            if (!Eligible(from, current, selection, quest))
            {
                from.SendMessage("You are not eligible for this reward offer, or its terms have changed.");
                return;
            }

            object reward = null;
            bool charged = false;
            bool received = false;
            try
            {
                reward = Activator.CreateInstance(m_Type, m_Args);
                if (!(reward is Item || reward is Mobile || reward is XmlAttachment)
                    || !Eligible(from, current, selection, quest))
                    return;

                charged = TakeCredits(from, m_Cost, quest);
                if (!charged)
                    return;

                Item item = reward as Item;
                Mobile mobile = reward as Mobile;
                XmlAttachment attachment = reward as XmlAttachment;
                if (item != null)
                {
                    // AddToBackpack intentionally falls back to the world; a successful merge deletes the new stack.
                    received = from.AddToBackpack(item);
                    if (!received)
                        received = !item.Deleted && item.Parent == null && item.Map == from.Map && item.Location == from.Location;
                }
                else if (mobile != null)
                {
                    BaseCreature creature = mobile as BaseCreature;
                    if (creature != null)
                    {
                        creature.Controlled = true;
                        creature.ControlMaster = from;
                    }
                    mobile.MoveToWorld(from.Location, from.Map);
                    received = !mobile.Deleted;
                }
                else
                    received = XmlAttach.AttachTo(from, attachment);
            }
            catch (Exception exception)
            {
                Console.WriteLine("XML reward {0} for {1} failed: {2}", m_Type, from.Serial, exception);
            }
            finally
            {
                if (!received)
                {
                    Item item = reward as Item;
                    Mobile mobile = reward as Mobile;
                    XmlAttachment attachment = reward as XmlAttachment;
                    if (item != null && !item.Deleted) item.Delete();
                    if (mobile != null && !mobile.Deleted) mobile.Delete();
                    if (attachment != null && !attachment.Deleted) attachment.Delete();
                    if (charged && !TakeCredits(from, -m_Cost, quest))
                        Console.WriteLine("XML reward credit restoration failed for {0}: {1}", from.Serial, m_Cost);
                    from.SendMessage(33, "Unable to deliver reward {0}.", m_Name);
                }
            }
            if (received)
                from.SendMessage("You have purchased {0} for {1} credits.", m_Name, m_Cost);
        }
    }
}
