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
    public class TailorRewardVendor : BasePerson
    {
        public override bool IsInvulnerable
        {
            get { return true; }
        }

        [Constructable]
        public TailorRewardVendor()
            : base()
        {
            Name = NameList.RandomName("female");
            Title = "Guild Reward Vendor";

            FancyDress dress = new FancyDress(0xAFE);
            dress.ItemID = 0x1F00;
            AddItem(dress);
            AddItem(new Sandals());

            Utility.AssignRandomHair(this);
            HairHue = 0x92E;
            HairItemID = 8252;
            FacialHairItemID = 0;
            SetSkill(SkillName.Tailoring, 64.0, 100.0);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
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
            player.SendGump(new RewardsGump(player));
        }

        public class RewardsGump : Gump
        {
            public RewardsGump(Mobile from)
                : base(352, 303)
            {
                int rule = 0;
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);
                this.AddBackground(0, 0, 352, 303, 9200);
                this.AddButton(259, 269, 0xF7, 0xF8, 11, GumpButtonType.Reply, 0);


                int count = from.Backpack.GetAmount(typeof(TailorGuildCoin));

                string temp = "Tailor Guild Coins = " + count.ToString();

                this.AddItem(13, 251, 0xef0, 1155);
                this.AddLabel(73, 269, 2498, temp);

                this.AddItem(15, 20, 0x4C81, 2841);
                this.AddLabel(66, 31, 2498, @"Barbed Runic Sewing Kit - 750");
                this.AddRadio(145, 31, 209, 208, rule == (int)Switches.radioButton1, (int)Switches.radioButton1);

                this.AddImage(15, 70, 0x2);
                this.AddLabel(66, 81, 2498, @"placeholder");
                this.AddRadio(145, 81, 209, 208, rule == (int)Switches.radioButton2, (int)Switches.radioButton2);

                this.AddImage(15, 120, 3);
                this.AddLabel(66, 131, 2498, @"placeholder");
                this.AddRadio(145, 131, 209, 208, rule == (int)Switches.radioButton3, (int)Switches.radioButton3);

                this.AddImage(15, 170, 4);
                this.AddLabel(66, 181, 2498, @"placeholder");
                this.AddRadio(145, 181, 209, 208, rule == (int)Switches.radioButton4, (int)Switches.radioButton4);

                //this.AddImage(186, 20, 5);
                //this.AddLabel(237, 31, 1150, @"item name 5");
                //this.AddRadio(316, 31, 209, 208, rule == (int)Switches.radioButton5, (int)Switches.radioButton5);

                //this.AddImage(186, 70, 6);
                //this.AddLabel(237, 81, 1150, @"item name 6");
                //this.AddRadio(316, 81, 209, 208, rule == (int)Switches.radioButton6, (int)Switches.radioButton6);

                //this.AddImage(186, 120, 7);
                //this.AddLabel(237, 131, 1150, @"item name 7");
                //this.AddRadio(316, 131, 209, 208, rule == (int)Switches.radioButton7, (int)Switches.radioButton7);

                //this.AddImage(186, 170, 8);
                //this.AddLabel(237, 181, 1150, @"item name 8");
                //this.AddRadio(316, 181, 209, 208, rule == (int)Switches.radioButton8, (int)Switches.radioButton8);

            }

            public enum Switches
            {
                radioButton1,
                radioButton2,
                radioButton3,
                radioButton4,
                radioButton5,
                radioButton6,
                radioButton7,
                radioButton8,
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                int count = from.Backpack.GetAmount(typeof(TailorGuildCoin));

                if (info.ButtonID == 11)
                {
                    int cost = 0;
                    int itemSelected = 0;

                    if (info.IsSwitched((int)Switches.radioButton1))
                    {
                        cost = 750;
                        itemSelected = 1;
                        from.SayTo(from, "I clicked on button 1 and switch 1");
                    }                        
                    if (info.IsSwitched((int)Switches.radioButton2))
                        from.SayTo(from, "I clicked on button 1 and switch 2");
                    if (info.IsSwitched((int)Switches.radioButton3))
                        from.SayTo(from, "I clicked on button 1 and switch 3");
                    if (info.IsSwitched((int)Switches.radioButton4))
                        from.SayTo(from, "I clicked on button 1 and switch 4");
                    if (info.IsSwitched((int)Switches.radioButton5))
                        from.SayTo(from, "I clicked on button 1 and switch 5");
                    if (info.IsSwitched((int)Switches.radioButton6))
                        from.SayTo(from, "I clicked on button 1 and switch 6");
                    if (info.IsSwitched((int)Switches.radioButton7))
                        from.SayTo(from, "I clicked on button 1 and switch 7");
                    if (info.IsSwitched((int)Switches.radioButton8))
                        from.SayTo(from, "I clicked on button 1 and switch 8");

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
