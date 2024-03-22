using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.BulkOrders;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    public class TailorRewardVendor : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get { return m_SBInfos; }
        }

        public override NpcGuild NpcGuild
        {
            get { return NpcGuild.TailorsGuild; }
        }

        public override void InitSBInfo()
        {
        }

        [Constructable]
        public TailorRewardVendor()
            : base("Guild Reward Vendor")
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            list.Add(new RewardEntry(from, this));
        }

        public class RewardEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_RewardVendor;

            public RewardEntry(Mobile from, Mobile rewardVendor)
                : base(6103, 2) // Buy
            {
                m_Mobile = from;
                m_RewardVendor = rewardVendor;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                BuyRewards(m_Mobile, m_RewardVendor);
            }
        }

        public static void BuyRewards(Mobile player, Mobile rewardVendor)
        {
            player.CloseGump(typeof(RewardsGump));
            player.SendGump(new RewardsGump(player, 1));
        }

        public class ItemCheck
        {
            public int[] itemID { get; set; }
            public int[] itemHue { get; set; }
            public string[] itemString { get; set; }
            public string[] priceString { get; set; }
            public int[] priceStringOffset { get; set; }
        }

        public static ItemCheck GetValues(int page)
        {
            if (page == 1)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x14F0, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0x481, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Tailoring Power Scroll", "Tailoring Power Scroll" },
                    priceString = new string[] { "991", "91", "1", "91" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 } // 18 for 3 numbers, 10 for 2, 2 for 1
                };
            }
            else if (page == 2)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x14F0, 0x14F0, 0x14F0, 0x14F0 },
                    itemHue = new int[] { 0x481, 0x481, 0x481, 0x481 },
                    itemString = new string[] { "Tailoring Power Scroll", "Tailoring Power Scroll", "Tailoring Power Scroll", "Tailoring Power Scroll" },
                    priceString = new string[] { "919", "19", "9", "91" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 } // 18 for 3 numbers, 10 for 2, 2 for 1
                };
            }
            else if (page == 3)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "199", "99", "9", "99" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 } // 18 for 3 numbers, 10 for 2, 2 for 1
                };
            }
            else if (page == 4)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "589", "456", "732", "880" },
                    priceStringOffset = new int[] { 18, 18, 18, 18 }
                };
            }
            else if (page == 5)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "155", "515", "551", "55" },
                    priceStringOffset = new int[] { 18, 18, 18, 10 }
                };
            }
            else if (page == 6)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "444", "44", "4", "44" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 }
                };
            }
            else if (page == 7)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "333", "33", "3", "33" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 }
                };
            }
            else if (page == 8)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "222", "22", "2", "22" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 }
                };
            }
            else if (page == 9)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x4C81, 0x14F0, 0x4C81, 0x14F0 },
                    itemHue = new int[] { 0xB19, 0x481, 0xB19, 0x481 },
                    itemString = new string[] { "Serpent Leather Runic Kit", "Tailoring Power Scroll", "Serpent Leather Runic Kit", "Tailoring Power Scroll" },
                    priceString = new string[] { "111", "11", "1", "11" },
                    priceStringOffset = new int[] { 18, 10, 2, 10 }
                };
            }

            return new ItemCheck
            {
                itemID = new int[] { 0, 0, 0, 0 },
                itemHue = new int[] { 0, 0, 0, 0 },
                itemString = new string[] { "item error", "item error", "item error", "item error" },
                priceString = new string[] { "price error", "price error", "price error", "price error" },
                priceStringOffset = new int[] { 0, 0, 0, 0 }
            };

        }

        public class RewardsGump : Gump
        {
            public RewardsGump(Mobile from, int page)
                : base(0, 0)
            {
                int rule = 0;
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);
                this.AddBackground(0, 0, 389, 318, 9380);

                this.AddButton(278, 244, 247, 248, page + 100, GumpButtonType.Reply, 0); // okay button

                int prev = pageShow(from, page, false);
                int next = pageShow(from, page, true);

                if (prev != 30)
                    AddButton(278, 220, 4014, 4014, prev, GumpButtonType.Reply, 0); // prev is always 20 when it's the last card
                if (next != 30)
                    AddButton(311, 220, 4005, 4005, next, GumpButtonType.Reply, 0); // next is always 20 when it's the last card

                int count = from.Backpack.GetAmount(typeof(TailorGuildCoin));
                string guildCoin = "Tailor Guild Coins = " + count.ToString();
                this.AddItem(34, 230, 0xef2, 1155); // tailor guild coin
                this.AddLabel(77, 235, 2498, guildCoin); // tailor guild coin

                ItemCheck itemCheck = GetValues(page);
                this.AddItem(34, 50, itemCheck.itemID[0], itemCheck.itemHue[0]);
                this.AddLabel(78, 50, 2498, itemCheck.itemString[0]); // item name
                this.AddLabel(272, 50, 2498, itemCheck.priceString[0]); // item price
                this.AddRadio(320, 50, 210, 211, false, (int)Switches.radioButton1);

                this.AddItem(34, 90, itemCheck.itemID[1], itemCheck.itemHue[1]);
                this.AddLabel(78, 90, 2498, itemCheck.itemString[1]);
                this.AddLabel(272, 90, 2498, itemCheck.priceString[1]); // item price
                this.AddRadio(320, 90, 210, 211, false, (int)Switches.radioButton2);

                this.AddItem(34, 130, itemCheck.itemID[2], itemCheck.itemHue[2]);
                this.AddLabel(78, 130, 2498, itemCheck.itemString[2]);
                this.AddLabel(272, 130, 2498, itemCheck.priceString[2]); // item price
                this.AddRadio(320, 130, 210, 211, false, (int)Switches.radioButton3);

                this.AddItem(34, 170, itemCheck.itemID[3], itemCheck.itemHue[3]);
                this.AddLabel(78, 170, 2498, itemCheck.itemString[3]);
                this.AddLabel(272, 170, 2498, itemCheck.priceString[3]); // item price
                this.AddRadio(320, 170, 210, 211, false, (int)Switches.radioButton4);
            }

            public enum Switches
            {
                radioButton1,
                radioButton2,
                radioButton3,
                radioButton4,
            }

            public int pageShow(Mobile from, int page, bool forward)
            {
                if (forward)
                {
                    if (page >= 9)
                    {
                        page = 30;
                    }
                    else page++;
                }
                else
                {
                    if (page <= 1)
                    {
                        page = 30;
                    }
                    else page--;
                }
                return page;
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.CloseGump(typeof(RewardsGump));
                int count = from.Backpack.GetAmount(typeof(TailorGuildCoin));

                if (info.ButtonID >= 100)
                {
                    int cost = 0;
                    int itemSelected = 0;

                    ItemCheck itemCheck = GetValues(info.ButtonID - 100);

                    if (info.IsSwitched((int)Switches.radioButton1))
                    {
                        cost = int.Parse(itemCheck.priceString[0]);
                        itemSelected = 1;
                    }
                    if (info.IsSwitched((int)Switches.radioButton2))
                        from.SayTo(from, "I clicked on button 1 and switch 2");
                    if (info.IsSwitched((int)Switches.radioButton3))
                        from.SayTo(from, "I clicked on button 1 and switch 3");
                    if (info.IsSwitched((int)Switches.radioButton4))
                        from.SayTo(from, "I clicked on button 1 and switch 4");

                    if (count >= cost)
                    {
                        if (itemSelected == 1)
                        {
                            from.SayTo(from, "RewardItem");
                            from.Backpack.ConsumeTotal(typeof(TailorGuildCoin), cost);
                            from.AddToBackpack(new RunicSewingKit(CraftResource.RegularLeather + 3, 50));
                        }
                    }
                }
                else
                {
                    int page = info.ButtonID;
                    if (page != 0)
                    {
                        from.SendGump(new RewardsGump(from, page));
                    }
                }
            }
        }

        //public class RewardsGump : Gump
        //{
        //    private int m_Amount;

        //    //public void RenderGump(Mobile from)
        //    //{
        //    //    from.SendMessage("RenderGump");
        //    //    m_Amount = 1;
        //    //    RenderGump(100);
        //    //}

        //    public void RenderGump(Mobile from)
        //    {
        //        from.SendMessage("RenderGump22");
        //        int rule = 100;
        //        AddPage(0);
        //        AddBackground(0, 0, 400, 270, 9260);
        //        AddLabel(125, 20, 52, @"Distribute Items to Shard");
        //        AddLabel(25, 40, 52, @"Rules:");
        //        AddRadio(35, 60, 209, 208, rule == (int)Switches.GiveToAccount, (int)Switches.GiveToAccount);
        //        AddLabel(65, 60, 2100, @"Per Account");
        //        AddRadio(35, 80, 209, 208, rule == (int)Switches.GiveToCharacter, (int)Switches.GiveToCharacter);
        //        AddLabel(65, 80, 2100, @"Per Character (Mobile)");
        //        AddRadio(35, 100, 209, 208, rule == (int)Switches.GiveToAccessLevel, (int)Switches.GiveToAccessLevel);
        //        AddLabel(65, 100, 2100, @"Per AccessLevel");

        //    }

        //    public RewardsGump(Mobile from)
        //        : base(50, 50)
        //    {
        //        from.SendMessage("RewardsGump");
        //        RenderGump(from);
        //    }

        //    public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        //    {
        //        Mobile from = sender.Mobile;
        //        from.SendMessage("OnResponse");

        //        string TypeName = string.Empty;
        //        int GiveRule = 0;
        //        int Access = 0;

        //        foreach (int sw in info.Switches)
        //        {
        //            switch (sw)
        //            {
        //                case (int)Switches.GiveToCharacter:
        //                    {
        //                        GiveRule = (int)Switches.GiveToCharacter;
        //                        break;
        //                    }
        //                case (int)Switches.GiveToAccount:
        //                    {
        //                        GiveRule = (int)Switches.GiveToAccount;
        //                        break;
        //                    }
        //                case (int)Switches.GiveToAccessLevel:
        //                    {
        //                        GiveRule = (int)Switches.GiveToAccessLevel;
        //                        break;
        //                    }
        //                case (int)Switches.Administrator:
        //                case (int)Switches.GameMaster:
        //                case (int)Switches.Seer:
        //                case (int)Switches.Counselor:
        //                    {
        //                        Access += sw;
        //                        break;
        //                    }
        //            }
        //        }
        //        if (GiveRule == 0)
        //        {
        //            from.SendMessage("You must select the audience rule to receive the item.");
        //            from.SendGump(new RewardsGump(from));
        //            return;
        //        }
        //        else if (GiveRule == (int)Switches.GiveToAccessLevel && Access == 0)
        //        {
        //            from.SendMessage("You must select the AccessLevel to receive the item.");
        //            from.SendGump(new RewardsGump(from));
        //            return;
        //        }

        //        switch (info.ButtonID)
        //        {
        //            case (int)Buttons.GiveByTarget:
        //                {
        //                    from.SendMessage("What do you wish to give out?");
        //                    break;
        //                }
        //            case (int)Buttons.GiveByType:
        //                {
        //                    if (info.TextEntries.Length > 0)
        //                    {
        //                        TypeName = info.TextEntries[0].Text;
        //                    }

        //                    if (TypeName == string.Empty)
        //                    {
        //                        from.SendMessage("You must specify a type");
        //                        from.SendGump(new RewardsGump(from));
        //                    }
        //                    else
        //                    {
        //                    }
        //                    break;
        //                }
        //            case (int)Buttons.IncAmount:
        //                {
        //                    from.SendGump(new RewardsGump(from));
        //                    break;
        //                }
        //            case (int)Buttons.DecAmount:
        //                {
        //                    if (m_Amount > 1)
        //                        m_Amount -= 1;
        //                    else
        //                        from.SendMessage("You cannot give less than 1 item.");
        //                    from.SendGump(new RewardsGump(from));
        //                    break;
        //                }
        //        }
        //    }

        //    public enum Buttons
        //    {
        //        Cancel,
        //        GiveByTarget,
        //        GiveByType,
        //        IncAmount,
        //        DecAmount
        //    }

        //    public enum Switches
        //    {
        //        Administrator = 1,
        //        GameMaster = 2,
        //        Seer = 4,
        //        Counselor = 8,
        //        GiveToAccount = 100,
        //        GiveToCharacter = 200,
        //        GiveToAccessLevel = 300
        //    }
        //}

        public TailorRewardVendor(Serial serial)
            : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
