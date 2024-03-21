using System;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Apiculture;
using Server.Engines.Mahjong;
using Server.Guilds;
using Server.Items;
using Server.Items.Crops;
using Server.Misc;
using Server.Multis;
using Server.Spells.Elementalism;

namespace Server.Mobiles
{
    public class SBJuiceMaker : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBJuiceMaker() { }

        public override IShopSellInfo SellInfo
        {
            get { return m_SellInfo; }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get { return m_BuyInfo; }
        } // Change the return type to match the base class

        public class InternalBuyInfo : List<GenericBuyInfo> // Change ArrayList to List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(JuicersTools),
                            500,
                            Utility.Random(1, 15),
                            0xE4F,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Lemon), 5, Utility.Random(1, 15), 0x1728, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Lime), 5, Utility.Random(1, 15), 0x172A, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Apple), 5, Utility.Random(1, 15), 0x9D0, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Dates), 5, Utility.Random(1, 15), 0x1727, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Orange), 5, Utility.Random(1, 15), 0x9D0, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Peach), 5, Utility.Random(1, 15), 0x9D2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Pear), 5, Utility.Random(1, 15), 0x994, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Pumpkin), 5, Utility.Random(1, 15), 0xC6A, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Tomato), 5, Utility.Random(1, 15), 0x9D0, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Watermelon), 5, Utility.Random(1, 15), 0xC5C, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Banana), 5, Utility.Random(1, 15), 0x171F, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Apricot), 5, Utility.Random(1, 15), 0x9D2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(Blackberries), 5, Utility.Random(1, 15), 0x9D1, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(Blueberries), 5, Utility.Random(1, 15), 0x9D1, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Cherries), 5, Utility.Random(1, 15), 0x9D1, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Grapefruit), 5, Utility.Random(1, 15), 0x9D0, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Kiwi), 5, Utility.Random(1, 15), 0xF8B, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Mango), 5, Utility.Random(1, 15), 0x9D0, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Pineapple), 5, Utility.Random(1, 15), 0xFC4, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(Pomegranate), 5, Utility.Random(1, 15), 0x9D0, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Strawberry), 5, Utility.Random(1, 15), 0xF2A, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(EmptyJuiceBottle),
                            5,
                            Utility.Random(1, 15),
                            0x99B,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellRareChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(FarmLabelMaker),
                            500,
                            Utility.Random(1, 15),
                            0xFC0,
                            0
                        )
                    );
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
