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
        }

        public static ItemCheck GetValues(int page)
        {
            if (page == 1)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x170D, 0x175D, 0x14F0, 0x14F0 },
                    itemHue = new int[] { 0, 0, 0, 0 },
                    itemString = new string[] {  "Sandals", "Special Colored Cloth", "Stretched Hide", "Tapestry" },
                    priceString = new string[] { "5", "10", "10", "10" },
                };
            }
            else if (page == 2)
            {
                return new ItemCheck
                {
                    itemID = new int[] { 0x14F0, 0x4C81, 0x4C81, 0x4C81 },
                    itemHue = new int[] { 0x000, 0x0ABB, 0x0AB0, 0x0B19 },
                    itemString = new string[] { "Clothing Blees Deed", "Deep Sea Serpent Runic Kit", "Lizard Runic Kit", "Serpent Runic Kit" },
                    priceString = new string[] { "300", "400", "600", "900" },
                };
            }

            return new ItemCheck
            {
                itemID = new int[] { 0, 0, 0, 0 },
                itemHue = new int[] { 0, 0, 0, 0 },
                itemString = new string[] { "item error", "item error", "item error", "item error" },
                priceString = new string[] { "price error", "price error", "price error", "price error" },
            };
        }

        private static Item ChooseAndCreateReward(int page, int RadioButtonSeleced)
        {
            if (page == 1)
            {
                if (RadioButtonSeleced == 0)
                {
                    return CreateSandals();
                }
                else if (RadioButtonSeleced == 1)
                {
                    return CreateCloth();
                }
                else if (RadioButtonSeleced == 2)
                {
                    return CreateStretchedHide();
                }
                else if (RadioButtonSeleced == 3)
                {
                    return CreateTapestry();
                }
            }
            if (page == 2)
            {
                if (RadioButtonSeleced == 0)
                {
                    return CreateCBD();
                }
                else if (RadioButtonSeleced == 1)
                {
                    return CreateRunicKit(1);
                }
                else if (RadioButtonSeleced == 2)
                {
                    return CreateRunicKit(2);
                }
                else if (RadioButtonSeleced == 3)
                {
                    return CreateRunicKit(3);
                }
            }

            return CreateCloth();
        }

        private static int[][] m_ClothHues = new int[][]
        {
            new int[] { 0x483, 0x48C, 0x488, 0x48A },
            new int[] { 0x495, 0x48B, 0x486, 0x485 },
            new int[] { 0x48D, 0x490, 0x48E, 0x491 },
            new int[] { 0x48F, 0x494, 0x484, 0x497 },
            new int[] { 0x489, 0x47F, 0x482, 0x47E }
        };

        private static Item CreateCloth()
        {
            int type = Utility.RandomMinMax(0, 4);
            if (type >= 0 && type < m_ClothHues.Length)
            {
                UncutCloth cloth = new UncutCloth(100);
                cloth.Hue = m_ClothHues[type][Utility.Random(m_ClothHues[type].Length)];
                return cloth;
            }

            throw new InvalidOperationException();
        }

        private static int[] m_SandalHues = new int[]
        {
            0x489,
            0x47F,
            0x482,
            0x47E,
            0x48F,
            0x494,
            0x484,
            0x497
        };

        private static Item CreateSandals()
        {
            return new Sandals(m_SandalHues[Utility.Random(m_SandalHues.Length)]);
        }

        private static Item CreateStretchedHide()
        {
            switch (Utility.Random(4))
            {
                default:
                case 0:
                    return new SmallStretchedHideEastDeed();
                case 1:
                    return new SmallStretchedHideSouthDeed();
                case 2:
                    return new MediumStretchedHideEastDeed();
                case 3:
                    return new MediumStretchedHideSouthDeed();
            }
        }

        private static Item CreateTapestry()
        {
            switch (Utility.Random(4))
            {
                default:
                case 0:
                    return new LightFlowerTapestryEastDeed();
                case 1:
                    return new LightFlowerTapestrySouthDeed();
                case 2:
                    return new DarkFlowerTapestryEastDeed();
                case 3:
                    return new DarkFlowerTapestrySouthDeed();
            }
        }

        private static Item CreateBearRug()
        {
            switch (Utility.Random(4))
            {
                default:
                case 0:
                    return new BrownBearRugEastDeed();
                case 1:
                    return new BrownBearRugSouthDeed();
                case 2:
                    return new PolarBearRugEastDeed();
                case 3:
                    return new PolarBearRugSouthDeed();
            }
        }

        private static Item CreateRunicKit(int type)
        {
            if (type >= 1 && type <= 3)
                return new RunicSewingKit(CraftResource.RegularLeather + type, 60 - (type * 10));

            throw new InvalidOperationException();
        }

        private static Item CreatePowerScroll(int type)
        {
            if (type == 5 || type == 10 || type == 15 || type == 20)
                return new PowerScroll(SkillName.Tailoring, 100 + type);

            throw new InvalidOperationException();
        }

        private static Item CreateCBD()
        {
            return new ClothingBlessDeed();
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
                    AddButton(278, 220, 4014, 4014, prev, GumpButtonType.Reply, 0); // prev is always 30 when it's the last card
                if (next != 30)
                    AddButton(311, 220, 4005, 4005, next, GumpButtonType.Reply, 0); // next is always 30 when it's the last card

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

                if (itemCheck.itemID[3] > 0)
                {
                    this.AddItem(34, 170, itemCheck.itemID[3], itemCheck.itemHue[3]);
                    this.AddLabel(78, 170, 2498, itemCheck.itemString[3]);
                    this.AddLabel(272, 170, 2498, itemCheck.priceString[3]); // item price
                    this.AddRadio(320, 170, 210, 211, false, (int)Switches.radioButton4);
                }
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
                    if (page >= 2)
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
                    int RadioButtonSeleced = 0;
                    int page = info.ButtonID - 100;

                    ItemCheck itemCheck = GetValues(info.ButtonID - 100);

                    if (info.IsSwitched((int)Switches.radioButton1))
                    {
                        RadioButtonSeleced = 0;
                        cost = int.Parse(itemCheck.priceString[RadioButtonSeleced]);
                    }
                    if (info.IsSwitched((int)Switches.radioButton2))
                    {
                        RadioButtonSeleced = 1;
                        cost = int.Parse(itemCheck.priceString[RadioButtonSeleced]);
                    }
                    if (info.IsSwitched((int)Switches.radioButton3))
                    {
                        RadioButtonSeleced = 2;
                        cost = int.Parse(itemCheck.priceString[RadioButtonSeleced]);
                    }
                    if (info.IsSwitched((int)Switches.radioButton4))
                    {
                        RadioButtonSeleced = 3;
                        cost = int.Parse(itemCheck.priceString[RadioButtonSeleced]);
                    }

                    if ((count >= cost) && ((info.IsSwitched((int)Switches.radioButton1)) || (info.IsSwitched((int)Switches.radioButton2)) || (info.IsSwitched((int)Switches.radioButton3)) || (info.IsSwitched((int)Switches.radioButton4))))
                    {
                        from.Backpack.ConsumeTotal(typeof(TailorGuildCoin), cost);
                        from.AddToBackpack(ChooseAndCreateReward(page, RadioButtonSeleced));
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
