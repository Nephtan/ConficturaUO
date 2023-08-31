using System;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefBaking : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Cooking; }
        }

        public override int GumpTitleNumber
        {
            get { return 0; } // Use String
        }

        public override string GumpTitleString
        {
            get { return "<basefont color=#FFFFFF><CENTER>BAKING MENU</CENTER></basefont>"; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefBaking();

                return m_CraftSystem;
            }
        }

        public override CraftECA ECA
        {
            get { return CraftECA.ChanceMinusSixtyToFourtyFive; }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefBaking()
            : base(1, 1, 1.25) // base( 1, 1, 1.5 )
        { }

        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckAccessible(tool, from))
                return 1044263; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from) { }

        public override int PlayEndingEffect(
            Mobile from,
            bool failed,
            bool lostMaterial,
            bool toolBroken,
            int quality,
            bool makersMark,
            CraftItem item
        )
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (lostMaterial)
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                else
                    return 1044157; // You failed to create the item, but no materials were lost.
            }
            else
            {
                if (quality == 0)
                    return 502785; // You were barely able to make this item.  It's quality is below average.
                else if (makersMark && quality == 2)
                    return 1044156; // You create an exceptional quality item and affix your maker's mark.
                else if (quality == 2)
                    return 1044155; // You create an exceptional quality item.
                else
                    return 1044154; // You create the item.
            }
        }

        // format of AddCraft: AddCraft( typeof( ThingToMake ), Category (text or ##),
        //			ThingToMake (text or ##), minskill, maxskill, typeof( FirstThingToConsume),
        //			FirstThingToConsume (text or ##), Qty,
        //			ErrorMessageForNotHavingFirstThingToConsume (text or ##) );
        // format of AddRes:   AddRes( index, typeof( SecondThingToConsume ),
        //			SecondThingToConsume (text or ##), Qty,
        //			ErrorMessageForNotHavingSecondThingToConsume (text or ##) );

        // index = AddCraft( typeof( Make ), Category, Make, minskill, maxskill, typeof( Consume1 ), Consume1, qty, Error );
        // AddRes( index, typeof( Consume2 ), Consume2, qty, Error );

        public override void InitCraftList()
        {
            int index = -1;

            /* Breads */

            index = AddCraft(
                typeof(BreadLoaf),
                "Breads",
                "Bread",
                0.0,
                100.0,
                typeof(Dough),
                "Dough",
                1,
                1044253
            );
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Muffins),
                "Breads",
                "Muffins",
                0.0,
                100.0,
                typeof(Batter),
                "Batter",
                1,
                1044253
            );
            AddRes(index, typeof(JarHoney), "Honey", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BananaBread),
                "Breads",
                "Banana Bread",
                0.0,
                100.0,
                typeof(SweetDough),
                "Sweet Dough",
                1,
                1044253
            );
            AddRes(index, typeof(Banana), "Banana", 6, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BlueberryMuffins),
                "Breads",
                "Blueberry Muffins",
                0.0,
                100.0,
                typeof(SweetDough),
                "Sweet Dough",
                1,
                1044253
            );
            AddRes(index, typeof(Blueberries), "Blueberries", 6, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CornBread),
                "Breads",
                "Corn Bread",
                0.0,
                100.0,
                typeof(BagOfCornmeal),
                "Bag of Cornmeal",
                1,
                1044253
            );
            AddRes(index, typeof(Batter), "Batter", 1, 1044253);
            AddRes(index, typeof(BagOfSugar), "Bag of Sugar", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Donuts),
                "Breads",
                "Donuts",
                0.0,
                100.0,
                typeof(SweetDough),
                "Sweet Dough",
                2,
                1044253
            );
            AddRes(index, typeof(JarHoney), "Honey", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PumpkinBread),
                "Breads",
                "Pumpkin Bread",
                0.0,
                100.0,
                typeof(SweetDough),
                "Sweet Dough",
                1,
                1044253
            );
            AddRes(index, typeof(Pumpkin), "Pumpkin", 3, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PumpkinMuffins),
                "Breads",
                "Pumpkin Muffins",
                0.0,
                100.0,
                typeof(SweetDough),
                "Sweet Dough",
                1,
                1044253
            );
            AddRes(index, typeof(Pumpkin), "Pumpkin", 2, 1044253);
            SetNeedOven(index, true);

            /* Cookies */

            index = AddCraft(
                typeof(AlmondCookies),
                "Cookies",
                "Almond Cookies",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Almonds), "Almond", 12, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChocChipCookies),
                "Cookies",
                "Chocolate Chip Cookies",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(ChocolateBar), "Chocolate Bar", 12, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(GingerSnaps),
                "Cookies",
                "Ginger Snaps",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(GingerRoot), "Ginger", 12, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(OatmealCookies),
                "Cookies",
                "Oatmeal Cookies",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(BagOfOats), "Bag of Oats", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PeanutButterCookies),
                "Cookies",
                "Peanut Butter Cookies",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(PeanutButter), "Peanut Butter", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PumpkinCookies),
                "Cookies",
                "Pumpkin Cookies",
                0.0,
                100.0,
                typeof(CookieMix),
                "Cookie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Pumpkin), "Pumpkin", 6, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            /* Desserts */

            index = AddCraft(
                typeof(ApplePie),
                "Desserts",
                "Apple Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Apple), "Apple", 8, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BlueberryPie),
                "Desserts",
                "Blueberry Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Blueberries), "Blueberries", 8, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CherryPie),
                "Desserts",
                "Cherry Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Cherries), "Cherries", 8, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(FruitPie),
                "Desserts",
                "Fruit Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(FruitBasket), "Fruit Basket", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(KeyLimePie),
                "Desserts",
                "Key Lime Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Lime), "Lime", 12, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(LemonMerenguePie),
                "Desserts",
                "Lemon Merengue Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Lemon), "Lemon", 12, 1044253);
            AddRes(index, typeof(Cream), "Cream", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PumpkinPie),
                "Desserts",
                "Pumpkin Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Pumpkin), "Pumpkin", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BlackberryCobbler),
                "Desserts",
                "Blackberry Cobbler",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Blackberries), "Blackberries", 10, 1044253);
            AddRes(index, typeof(JarHoney), "Honey", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PeachCobbler),
                "Desserts",
                "Peach Cobbler",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Peach), "Peach", 10, 1044253);
            AddRes(index, typeof(JarHoney), "Honey", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Brownies),
                "Desserts",
                "Brownies",
                0.0,
                100.0,
                typeof(ChocolateMix),
                "Chocolate Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Eggs), "Eggs", 2, 1044253);
            AddRes(index, typeof(CookingOil), "Cooking Oil", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChocolateBar),
                "Desserts",
                "Chocolate Bar",
                0.0,
                100.0,
                typeof(BagOfCocoa),
                "Bag of Cocoa",
                1,
                1044253
            );
            AddRes(index, typeof(Butter), "Butter", 1, 1044253);
            AddRes(index, typeof(BagOfSugar), "Bag of Sugar", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChocSunflowerSeeds),
                "Desserts",
                "Chocolate Sunflower Seeds",
                0.0,
                100.0,
                typeof(EdibleSun),
                "Sunflower Seeds",
                1,
                1044253
            );
            AddRes(index, typeof(ChocolateBar), "Chocolate Bar", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(RiceKrispTreat),
                "Desserts",
                "RiceKrispTreat",
                0.0,
                100.0,
                typeof(BowlRiceKrisps),
                "Bowl of Rice Krisps",
                1,
                1044253
            );
            AddRes(index, typeof(Butter), "Butter", 1, 1044253);
            AddRes(index, typeof(BagOfSugar), "Bag of Sugar", 1, 1044253);
            SetNeedOven(index, true);

            /* Cakes */

            index = AddCraft(
                typeof(BananaCake),
                "Cakes",
                "Banana Cake",
                0.0,
                100.0,
                typeof(CakeMix),
                "Cake Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Banana), "Banana", 4, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CarrotCake),
                "Cakes",
                "Carrot Cake",
                0.0,
                100.0,
                typeof(CakeMix),
                "Cake Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Carrot), "Carrot", 6, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChocolateCake),
                "Cakes",
                "Chocolate Cake",
                0.0,
                100.0,
                typeof(CakeMix),
                "Cake Mix",
                1,
                1044253
            );
            AddRes(index, typeof(BagOfCocoa), "Bag of Cocoa", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CoconutCake),
                "Cakes",
                "Coconut Cake",
                0.0,
                100.0,
                typeof(CakeMix),
                "Cake Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Coconut), "Coconut", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(LemonCake),
                "Cakes",
                "Lemon Cake",
                0.0,
                100.0,
                typeof(CakeMix),
                "Cake Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Lemon), "Lemon", 4, 1044253);
            SetNeedOven(index, true);

            /* Dinners */

            index = AddCraft(
                typeof(ChickenParmesian),
                "Dinners",
                "Chicken Parmesian",
                0.0,
                100.0,
                typeof(RawBird),
                "Raw Bird",
                1,
                1044253
            );
            AddRes(index, typeof(TomatoSauce), "Tomato Sauce", 1, 1044253);
            AddRes(index, typeof(CheeseWheel), "Cheese Wheel", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CheeseEnchilada),
                "Dinners",
                "Cheese Enchilada",
                0.0,
                100.0,
                typeof(CheeseWheel),
                "Cheese Wheel",
                1,
                1044253
            );
            AddRes(index, typeof(Tortilla), "Tortilla", 1, 1044253);
            AddRes(index, typeof(EnchiladaSauce), "Enchilada Sauce", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChickenEnchilada),
                "Dinners",
                "Chicken Enchilada",
                0.0,
                100.0,
                typeof(RawBird),
                "Raw Bird",
                1,
                1044253
            );
            AddRes(index, typeof(Tortilla), "Tortilla", 1, 1044253);
            AddRes(index, typeof(EnchiladaSauce), "Enchilada Sauce", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Lasagna),
                "Dinners",
                "Lasagna",
                0.0,
                100.0,
                typeof(PastaNoodles),
                "Pasta Noodles",
                3,
                1044253
            );
            AddRes(index, typeof(GroundBeef), "Ground Beef", 1, 1044253);
            AddRes(index, typeof(CheeseWheel), "Cheese Wheel", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(LemonChicken),
                "Dinners",
                "Lemon Chicken",
                0.0,
                100.0,
                typeof(RawBird),
                "Raw Bird",
                1,
                1044253
            );
            AddRes(index, typeof(Lemon), "Lemon", 1, 1044253);
            AddRes(index, typeof(BasketOfHerbs), "Herbs", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(OrangeChicken),
                "Dinners",
                "Orange Chicken",
                0.0,
                100.0,
                typeof(RawBird),
                "Raw Bird",
                1,
                1044253
            );
            AddRes(index, typeof(Orange), "Orange", 1, 1044253);
            AddRes(index, typeof(BasketOfHerbs), "Herbs", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(VealParmesian),
                "Dinners",
                "Veal Parmesian",
                0.0,
                100.0,
                typeof(RawLambLeg),
                1025641,
                2,
                1044253
            );
            AddRes(index, typeof(TomatoSauce), "Tomato Sauce", 1, 1044253);
            AddRes(index, typeof(CheeseWheel), "Cheese Wheel", 1, 1044253);
            AddRes(index, typeof(Plate), "Plate", 1, "You need a plate!");
            SetNeedOven(index, true);

            /* Food */

            index = AddCraft(
                typeof(BroccoliCheese),
                "Food",
                "Broccoli and Cheese",
                0.0,
                100.0,
                typeof(Broccoli),
                "Broccoli",
                5,
                1044253
            );
            AddRes(index, typeof(CheeseSauce), "Cheese Sauce", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BroccoliCaulCheese),
                "Food",
                "Broccoli, Cauliflower and Cheese",
                0.0,
                100.0,
                typeof(Broccoli),
                "Broccoli",
                5,
                1044253
            );
            AddRes(index, typeof(Cauliflower), "Cauliflower", 2, 1044253);
            AddRes(index, typeof(CheeseSauce), "Cheese Sauce", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(CauliflowerCheese),
                "Food",
                "Cauliflower and Cheese",
                0.0,
                100.0,
                typeof(Cauliflower),
                "Cauliflower",
                5,
                1044253
            );
            AddRes(index, typeof(CheeseSauce), "Cheese Sauce", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ChickenPie),
                "Food",
                "Chicken Pie",
                0.0,
                100.0,
                typeof(RawBird),
                "Raw Bird",
                1,
                1044253
            );
            AddRes(index, typeof(PieMix), "Pie Mix", 1, 1044253);
            AddRes(index, typeof(MixedVegetables), "Mixed Vegetables", 1, 1044253);
            AddRes(index, typeof(Gravy), "Gravy", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(BeefPie),
                "Food",
                "Beef Pie",
                0.0,
                100.0,
                typeof(GroundBeef),
                "Ground Beef",
                1,
                1044253
            );
            AddRes(index, typeof(PieMix), "Pie Mix", 1, 1044253);
            AddRes(index, typeof(MixedVegetables), "Mixed Vegetables", 1, 1044253);
            AddRes(index, typeof(Gravy), "Gravy", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Meatballs),
                "Food",
                "Meatballs",
                0.0,
                100.0,
                typeof(GroundBeef),
                "Ground Beef",
                1,
                1044253
            );
            AddRes(index, typeof(BreadLoaf), "Bread", 1, 1044253);
            AddRes(index, typeof(Eggs), "Eggs", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Meatloaf),
                "Food",
                "Meatloaf",
                0.0,
                100.0,
                typeof(GroundBeef),
                "Ground Beef",
                2,
                1044253
            );
            AddRes(index, typeof(Eggs), "Eggs", 2, 1044253);
            AddRes(index, typeof(Onion), "Onion", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(PotatoStrings),
                "Food",
                "Potato Strings",
                0.0,
                100.0,
                typeof(Potato),
                "Potato",
                1,
                1044253
            );
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(Quiche),
                "Food",
                "Quiche",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(Eggs), "Eggs", 1, 1044253);
            AddRes(index, typeof(RawHamSlices), "Raw Ham Slices", 3, 1044253);
            AddRes(index, typeof(Onion), "Onion", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(ShepherdsPie),
                "Food",
                "Shepherds Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(GroundBeef), "Ground Beef", 1, 1044253);
            AddRes(index, typeof(BowlMashedPotatos), "Bowl of Mashed Potatos", 1, 1044253);
            AddRes(index, typeof(Corn), "Corn", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(TurkeyPie),
                "Food",
                "Turkey Pie",
                0.0,
                100.0,
                typeof(PieMix),
                "Pie Mix",
                1,
                1044253
            );
            AddRes(index, typeof(SlicedTurkey), "Sliced Turkey", 2, 1044253);
            AddRes(index, typeof(MixedVegetables), "Mixed Vegetables", 1, 1044253);
            AddRes(index, typeof(Gravy), "Gravy", 1, 1044253);
            SetNeedOven(index, true);

            /* Pizzas */

            index = AddCraft(
                typeof(CheesePizza),
                "Pizzas",
                "Cheese Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(HamPineapplePizza),
                "Pizzas",
                "Ham and Pineapple Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            AddRes(index, typeof(RawHamSlices), "Raw Ham Slices", 1, 1044253);
            AddRes(index, typeof(Pineapple), "Pineapple", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(MushroomOnionPizza),
                "Pizzas",
                "Mushroom and Onion Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            AddRes(index, typeof(Mushrooms), "Mushrooms", 3, 1044253);
            AddRes(index, typeof(Onion), "Onion", 3, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(SausOnionMushPizza),
                "Pizzas",
                "Sausage Onion and Mushroom Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            AddRes(index, typeof(Sausage), "Sausage", 2, 1044253);
            AddRes(index, typeof(Onion), "Onion", 2, 1044253);
            AddRes(index, typeof(Mushrooms), "Mushrooms", 2, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(TacoPizza),
                "Pizzas",
                "Taco Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            AddRes(index, typeof(GroundBeef), "Ground Beef", 1, 1044253);
            AddRes(index, typeof(CheeseWheel), "Cheese Wheel", 1, 1044253);
            AddRes(index, typeof(EnchiladaSauce), "Enchilada Sauce", 1, 1044253);
            SetNeedOven(index, true);

            index = AddCraft(
                typeof(VeggiePizza),
                "Pizzas",
                "Vegetable Pizza",
                0.0,
                100.0,
                typeof(UncookedPizza),
                "Uncooked Pizza",
                1,
                1044253
            );
            AddRes(index, typeof(MixedVegetables), "Mixed Vegetables", 1, 1044523);
            SetNeedOven(index, true);
        }
    }
}
