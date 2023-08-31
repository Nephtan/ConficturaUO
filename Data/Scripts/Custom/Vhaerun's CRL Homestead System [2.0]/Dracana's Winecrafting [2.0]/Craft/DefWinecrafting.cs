//================================================//
// Created by dracana				  //
// Desc: Winecrafting system.                     //
//================================================//
using System;
using Server;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefWinecrafting : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Alchemy; }
        }

        public override int GumpTitleNumber
        {
            get { return 0; } // <CENTER>WINECRAFTING MENU</CENTER>
        }

        public override string GumpTitleString
        {
            get { return "<basefont color=#FFFFFF><CENTER>Winecrafting Menu</CENTER></basefont>"; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefWinecrafting();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private DefWinecrafting()
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
            string skillNotice = "You have no idea how to craft wine with this type of grape.";

            //Wines
            //following method will make just bottles that are ready to drink...
            //index = AddCraft( typeof( BottleOfWine ), "Wines", "Bottle of Wine", 80.0, 105.6, typeof( CabernetSauvignonGrapes ), "Cabernet Sauvignon Grapes", 3 );
            //this one will create kegs that need to sit for seven days before bottling (yields 100 bottles)
            index = AddCraft(
                typeof(WineKeg),
                "Wines",
                "Keg of Wine",
                80.0,
                105.6,
                typeof(CabernetSauvignonGrapes),
                "Cabernet Sauvignon Grapes",
                50
            );
            AddRes(index, typeof(Keg), "Keg", 1);
            AddRes(index, typeof(WinecrafterSugar), "Sugar", 1);
            AddRes(index, typeof(WinecrafterYeast), "Yeast", 1);

            // Set the overidable material
            SetSubRes(typeof(CabernetSauvignonGrapes), "Cabernet Sauvignon Grapes");

            // Add every material you want the player to be able to chose from
            // This will overide the overidable material

            AddSubRes(
                typeof(CabernetSauvignonGrapes),
                "Cabernet Sauvignon Grapes",
                80.0,
                skillNotice
            );
            AddSubRes(typeof(ChardonnayGrapes), "Chardonnay Grapes", 80.0, skillNotice);
            AddSubRes(typeof(CheninBlancGrapes), "Chenin Blanc Grapes", 85.0, skillNotice);
            AddSubRes(typeof(MerlotGrapes), "Merlot Grapes", 80.0, skillNotice);
            AddSubRes(typeof(PinotNoirGrapes), "Pinot Noir Grapes", 80.0, skillNotice);
            AddSubRes(typeof(RieslingGrapes), "Riesling Grapes", 85.0, skillNotice);
            AddSubRes(typeof(SangioveseGrapes), "Sangiovese Grapes", 90.0, skillNotice);
            AddSubRes(typeof(SauvignonBlancGrapes), "Sauvignon Blanc Grapes", 90.0, skillNotice);
            AddSubRes(typeof(ShirazGrapes), "Shiraz Grapes", 90.0, skillNotice);
            AddSubRes(typeof(ViognierGrapes), "Viognier Grapes", 99.0, skillNotice);
            AddSubRes(typeof(ZinfandelGrapes), "Zinfandel Grapes", 80.0, skillNotice);

            MarkOption = true;
            Repair = false;
        }
    }
}
