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
    public class SBBrewer : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBrewer() { }

        public override IShopSellInfo SellInfo
        {
            get { return m_SellInfo; }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get { return m_BuyInfo; }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Keg), 50, Utility.Random(1, 15), 0xE7F, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BrewersTools),
                            498,
                            Utility.Random(1, 15),
                            0x1EBC,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellRareChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BreweryLabelMaker),
                            487,
                            Utility.Random(1, 15),
                            0xFBF,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Malt), 10, Utility.Random(1, 15), 0x103D, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Barley), 20, Utility.Random(1, 15), 0x103F, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(EmptyAleBottle),
                            10,
                            Utility.Random(1, 15),
                            0x99B,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(EmptyMeadBottle),
                            10,
                            Utility.Random(1, 15),
                            0x99B,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(EmptyJug), 10, Utility.Random(1, 15), 0x9C8, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(BitterHops), 5, Utility.Random(1, 15), 0x1AA2, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(SnowHops), 5, Utility.Random(1, 15), 0x1AA2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(ElvenHops), 5, Utility.Random(1, 15), 0x1AA2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(SweetHops), 5, Utility.Random(1, 15), 0x1AA2, 0));
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
