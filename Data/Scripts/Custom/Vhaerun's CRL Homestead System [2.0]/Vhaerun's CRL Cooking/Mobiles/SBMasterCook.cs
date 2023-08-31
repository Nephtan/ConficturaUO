using System;
using System.Collections.Generic;
using System.Collections;
using Server.Engines.Apiculture;
using Server.Items;
using Server.Items.Crops;
using Server.Misc;
using Server.Multis;
using Server.Guilds;
using Server.Engines.Mahjong;
using Server.Spells.Elementalism;

namespace Server.Mobiles
{
    public class SBMasterCook : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBMasterCook() { }

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
                    Add(new GenericBuyInfo(typeof(Plate), 5, Utility.Random(1, 15), 0x9D7, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BakersBoard),
                            10,
                            Utility.Random(1, 15),
                            0x14EA,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(CooksCauldron),
                            10,
                            Utility.Random(1, 15),
                            0x9ED,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(FryingPan), 10, Utility.Random(1, 15), 0x9E2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(FoodDyes), 500, Utility.Random(1, 15), 0xFA9, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(CheeseForm), 50, Utility.Random(1, 15), 0x0E78, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(MilkBucket),
                            500,
                            Utility.Random(1, 15),
                            0x0FFA,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BasketOfHerbs),
                            20,
                            Utility.Random(1, 15),
                            0x194F,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellRareChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(MushroomGardenEastDeed),
                            1000,
                            Utility.Random(1, 15),
                            0x14F0,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellRareChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(MushroomGardenSouthDeed),
                            1000,
                            Utility.Random(1, 15),
                            0x14F0,
                            0
                        )
                    );
                }
                //if (MyServerSettings.SellChance()) { Add(new GenericBuyInfo(typeof(GinsengUprooted), 5, Utility.Random(1, 15), 0x18E7, 0)); }
                //if (MyServerSettings.SellChance()) { Add(new GenericBuyInfo(typeof(GinsengUprooted), 5, Utility.Random(1, 15), 0x18E8, 0)); }
                //if (MyServerSettings.SellChance()) { Add(new GenericBuyInfo(typeof(MandrakeUprooted), 5, Utility.Random(1, 15), 0x18DD, 0)); }
                //if (MyServerSettings.SellChance()) { Add(new GenericBuyInfo(typeof(MandrakeUprooted), 5, Utility.Random(1, 15), 0x18DE, 0)); }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(BagOfCocoa), 10, Utility.Random(1, 15), 0x1039, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BagOfCoffee),
                            10,
                            Utility.Random(1, 15),
                            0x1039,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BagOfCornmeal),
                            10,
                            Utility.Random(1, 15),
                            0x1039,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(BagOfOats), 10, Utility.Random(1, 15), 0x1039, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(BagOfRicemeal),
                            10,
                            Utility.Random(1, 15),
                            0x1039,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(BagOfSoy), 10, Utility.Random(1, 15), 0x1039, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(BagOfSugar), 10, Utility.Random(1, 15), 0x1039, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(CocoaNut), 5, Utility.Random(1, 15), 0x1726, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(FieldCorn), 5, Utility.Random(1, 15), 0xC81, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Hay), 5, Utility.Random(1, 15), 0xF36, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(OatSheath), 5, Utility.Random(1, 15), 0x1EBD, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(RiceSheath), 5, Utility.Random(1, 15), 0x1A9D, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Sugarcane), 5, Utility.Random(1, 15), 0x1A9D, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(TeaLeaves), 5, Utility.Random(1, 15), 0x1AA2, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Wheat), 5, Utility.Random(1, 15), 0x1EBD, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Tortilla), 5, Utility.Random(1, 15), 0x973, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(Eggs), 5, Utility.Random(1, 15), 0x9B5, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RawBacon), 5, Utility.Random(1, 15), 0x979, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RawBaconSlab), 5, 20, 0x976, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RawBird), 5, Utility.Random(1, 15), 0x9B9, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(RawChickenLeg),
                            5,
                            Utility.Random(1, 15),
                            0x1607,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(RawFishSteak),
                            5,
                            Utility.Random(1, 15),
                            0x097A,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RawHam), 5, Utility.Random(1, 15), 0x9C9, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(RawHamSlices),
                            5,
                            Utility.Random(1, 15),
                            0x1E1F,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(RawHeadlessFish),
                            5,
                            Utility.Random(1, 15),
                            0x1E17,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(typeof(RawLambLeg), 5, Utility.Random(1, 15), 0x1609, 0)
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(new GenericBuyInfo(typeof(RawRibs), 5, Utility.Random(1, 15), 0x9F1, 0));
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            typeof(RawScaledFish),
                            5,
                            Utility.Random(1, 15),
                            0x1E16,
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
