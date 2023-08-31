using System;
using Server;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefBrewing : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Cooking; }
        }

        public override int GumpTitleNumber
        {
            get { return 0; } // <CENTER>Brewing MENU</CENTER>
        }

        public override string GumpTitleString
        {
            get { return "<basefont color=#FFFFFF><CENTER>Brewing Menu</CENTER></basefont>"; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefBrewing();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private DefBrewing()
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
            string skillNotice = "You have no idea how to brew with this type of hops.";

            /* Mead */

            index = AddCraft(
                typeof(MeadKeg),
                "Mead",
                "Keg of Mead",
                80.0,
                105.6,
                typeof(BitterHops),
                "Bitter Hops",
                50
            );
            AddRes(index, typeof(Keg), "Keg", 1);
            AddRes(index, typeof(BaseBeverage), "Water", 20);
            AddRes(index, typeof(Malt), "Malt", 3);
            SetNeedOven(index, true);

            /* Ale */

            index = AddCraft(
                typeof(AleKeg),
                "Ale",
                "Keg of Ale",
                80.0,
                105.6,
                typeof(BitterHops),
                "Bitter Hops",
                50
            );
            AddRes(index, typeof(Keg), "Keg", 1);
            AddRes(index, typeof(BaseBeverage), "Water", 20);
            AddRes(index, typeof(Barley), "Barley", 3);
            SetNeedOven(index, true);

            /* Cider */

            index = AddCraft(
                typeof(CiderKeg),
                "Cider",
                "Keg of Cider",
                80.0,
                105.6,
                typeof(Keg),
                "Keg",
                1
            );
            AddRes(index, typeof(Apple), "Apples", 100);
            AddRes(index, typeof(BaseBeverage), "Water", 20);
            AddRes(index, typeof(BagOfSugar), "Bag of Sugar", 1);

            // Set the overidable material
            SetSubRes(typeof(BitterHops), "Bitter Hops");

            // Add every material you want the player to be able to chose from
            // This will overide the overidable material

            AddSubRes(typeof(BitterHops), "Bitter Hops", 40.0, skillNotice);
            AddSubRes(typeof(SnowHops), "Snow Hops", 40.0, skillNotice);
            AddSubRes(typeof(ElvenHops), "Elven Hops", 80.0, skillNotice);
            AddSubRes(typeof(SweetHops), "Sweet Hops", 80.0, skillNotice);

            MarkOption = true;
            Repair = false;
        }
    }
}
