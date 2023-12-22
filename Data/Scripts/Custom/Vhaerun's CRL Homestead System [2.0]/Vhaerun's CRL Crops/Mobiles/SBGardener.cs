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
    public class SBGardener : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBGardener() { }

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
                    Add(
                        new GenericBuyInfo(
                            "1060834",
                            typeof(Engines.Plants.PlantBowl),
                            2,
                            Utility.Random(1, 15),
                            0x15FD,
                            0
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cotton Seed",
                            typeof(CottonSeed),
                            250,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Flax Seed",
                            typeof(FlaxSeed),
                            250,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Wheat Seed",
                            typeof(WheatSeed),
                            150,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Planting Corn",
                            typeof(CornSeed),
                            150,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Carrot Seed",
                            typeof(CarrotSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Onion Seed",
                            typeof(OnionSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Garlic Seed",
                            typeof(GarlicSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Lettuce Seed",
                            typeof(LettuceSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cabbage Seed",
                            typeof(CabbageSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Asparagus Seed",
                            typeof(AsparagusSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Beet Seed",
                            typeof(BeetSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Bitter Hops Seed",
                            typeof(BitterHopsSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Broccoli Seed",
                            typeof(BroccoliSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cantaloupe Seed",
                            typeof(CantaloupeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cauliflower Seed",
                            typeof(CauliflowerSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Celery Seed",
                            typeof(CelerySeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "ChiliPepper Seed",
                            typeof(ChiliPepperSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Coffee Seed",
                            typeof(CoffeeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cucumber Seed",
                            typeof(CucumberSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Eggplant Seed",
                            typeof(EggplantSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Elven Hops Seed",
                            typeof(ElvenHopsSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Field Corn Seed",
                            typeof(FieldCornSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Ginger Seed",
                            typeof(GingerSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "ginseng Seed",
                            typeof(GinsengSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "GreenBean Seed",
                            typeof(GreenBeanSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Green Pepper Seed",
                            typeof(GreenPepperSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Green Squash Seed",
                            typeof(GreenSquashSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Hay Seed",
                            typeof(HaySeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Honeydew melon Seed",
                            typeof(HoneydewMelonSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Kiwi Seed",
                            typeof(KiwiSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Mandrake Seed",
                            typeof(MandrakeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Nightshade Seed",
                            typeof(NightshadeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Oats Seed",
                            typeof(OatsSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Orange Pepper Seed",
                            typeof(OrangePepperSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Peanut Seed",
                            typeof(PeanutSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Peas Seed",
                            typeof(PeasSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Potato Seed",
                            typeof(PotatoSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Pumpkin Seed",
                            typeof(PumpkinSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Radish Seed",
                            typeof(RadishSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Red Pepper Seed",
                            typeof(RedPepperSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Rice Seed",
                            typeof(RiceSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Snow Hops Seed",
                            typeof(SnowHopsSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Snow Peas Seed",
                            typeof(SnowPeasSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Soy Seed",
                            typeof(SoySeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Spinach Seed",
                            typeof(SpinachSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Squash Seed",
                            typeof(SquashSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Strawberry Seed",
                            typeof(StrawberrySeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Sugar Seed",
                            typeof(SugarSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Sun Flower Seed",
                            typeof(SunFlowerSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Sweet Hops Seed",
                            typeof(SweetHopsSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Sweet Potato Seed",
                            typeof(SweetPotatoSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Tea Seed",
                            typeof(TeaSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Tomato Seed",
                            typeof(TomatoSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Turnip Seed",
                            typeof(TurnipSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Watermelon Seed",
                            typeof(WatermelonSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Almond Tree Seed",
                            typeof(AlmondTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Apple Tree Seed",
                            typeof(AppleTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Apricot Tree Seed",
                            typeof(ApricotTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Avocado Tree Seed",
                            typeof(AvocadoTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Banana Palm Seed",
                            typeof(BananaTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Blackberry Bush Seed",
                            typeof(BlackberryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "BlackRaspberry Bush Seed",
                            typeof(BlackRaspberryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Blueberry Bush Seed",
                            typeof(BlueberryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cherry Tree Seed",
                            typeof(CherryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cocoa Tree Seed",
                            typeof(CocoaTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Coconut Palm Seed",
                            typeof(CoconutTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Cranberry Bush Seed",
                            typeof(CranberryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Date Palm Seed",
                            typeof(DateTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Grapefruit Tree Seed",
                            typeof(GrapefruitTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Lemon Tree Seed",
                            typeof(LemonTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Lime Tree Seed",
                            typeof(LimeTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Mango Tree Seed",
                            typeof(MangoTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Orange Tree Seed",
                            typeof(OrangeTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Peach Tree Seed",
                            typeof(PeachTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Pear Tree Seed",
                            typeof(PearTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Pineapple Tree Seed",
                            typeof(PineappleTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Pistacio Tree Seed",
                            typeof(PistacioTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Plum Tree Seed",
                            typeof(PlumTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Pomegranate Tree Seed",
                            typeof(PomegranateTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
                        )
                    );
                }
                if (MyServerSettings.SellChance())
                {
                    Add(
                        new GenericBuyInfo(
                            "Red Raspberry Bush Seed",
                            typeof(RedRaspberryTreeSeed),
                            50,
                            Utility.Random(1, 15),
                            0xF27,
                            0x5E2
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
