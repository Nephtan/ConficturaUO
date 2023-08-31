using System;
using Server;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefJuicing : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Cooking; }
        }

        public override int GumpTitleNumber
        {
            get { return 0; } // <CENTER>Juicing MENU</CENTER>
        }

        public override string GumpTitleString
        {
            get { return "<basefont color=#FFFFFF><CENTER>JUICING MENU</CENTER></basefont>"; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefJuicing();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private DefJuicing()
            : base(1, 1, 1.25) // base( 1, 1, 3.0 )
        { }

        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            //else if ( !BaseTool.CheckAccessible( tool, from ) )
            //return 1044263; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x241);
            //from.PlaySound( 0x242 );
        }

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

        public override void InitCraftList()
        {
            int index = -1;
            string skillNotice = "You have no idea how to work with this type of fruit.";

            //Juices
            index = AddCraft(
                typeof(JuiceKeg),
                "Juices",
                "Keg of Juice",
                20.0,
                120.0,
                typeof(Apple),
                "Apples",
                25
            );
            AddRes(index, typeof(BaseBeverage), "Water", 5);
            AddRes(index, typeof(Keg), "Keg", 1);
            AddRes(index, typeof(BagOfSugar), "Bag of Sugar", 1);

            // Set the overidable material
            SetSubRes(typeof(Apple), "Apple");

            // Add every material you want the player to be able to chose from
            // This will overide the overidable material

            AddSubRes(typeof(Apple), "Apples", 20.0, skillNotice);
            AddSubRes(typeof(Banana), "Bananas", 20.0, skillNotice);
            AddSubRes(typeof(Dates), "Dates", 20.0, skillNotice);
            AddSubRes(typeof(Grapes), "Grapes", 20.0, skillNotice);
            AddSubRes(typeof(Lemon), "Lemons", 20.0, skillNotice);
            AddSubRes(typeof(Lime), "Limes", 20.0, skillNotice);
            AddSubRes(typeof(Orange), "Oranges", 20.0, skillNotice);
            AddSubRes(typeof(Peach), "Peaches", 20.0, skillNotice);
            AddSubRes(typeof(Pear), "Pears", 20.0, skillNotice);
            AddSubRes(typeof(Pumpkin), "Pumpkins", 20.0, skillNotice);
            AddSubRes(typeof(Tomato), "Tomatos", 20.0, skillNotice);
            AddSubRes(typeof(Watermelon), "Watermelons", 20.0, skillNotice);
            AddSubRes(typeof(Apricot), "Apricots", 20.0, skillNotice);
            AddSubRes(typeof(Blackberries), "Blackberries", 20.0, skillNotice);
            AddSubRes(typeof(Blueberries), "Blueberries", 20.0, skillNotice);
            AddSubRes(typeof(Cherries), "Cherries", 20.0, skillNotice);
            AddSubRes(typeof(Cranberries), "Cranberries", 20.0, skillNotice);
            AddSubRes(typeof(Grapefruit), "Grapefruit", 20.0, skillNotice);
            AddSubRes(typeof(Kiwi), "Kiwis", 20.0, skillNotice);
            AddSubRes(typeof(Mango), "Mangos", 20.0, skillNotice);
            AddSubRes(typeof(Pineapple), "Pineapples", 20.0, skillNotice);
            AddSubRes(typeof(Pomegranate), "Pomegranates", 20.0, skillNotice);
            AddSubRes(typeof(Strawberry), "Strawberries", 20.0, skillNotice);

            MarkOption = true;
            Repair = false;
        }
    }
}
