/***************************************************************************/
/*			ResourceBox.cs | ResourceBoxGump.cs | StorageTypes.cs					*/
/*							Created by A_Li_N													*/
/*				Credits :																		*/
/*						Original Gump Layout - Lysdexic									*/
/*						Hashtable help - UOT and daat99									*/
/***************************************************************************/
/*	Addition of different Resources :													*/
/*		To add/remove resource types from the box, simply put the Type of		*/
/*		the resource in the catagory you wish it to be in.  Each catagory		*/
/*		can hold up to 32 entries without messing a LOT with the gump.			*/
/*	Removing of Resources :																	*/
/*		Commenting out or deleting the type you wish to remove will remove	*/
/*		the type AND the amount each Resource Box contains.						*/
/***************************************************************************/

using System;

namespace Server.Items
{
    public class StorageTypes
    {
        private static Type[] m_Logs = new Type[]
        {
            typeof(Log),
            typeof(AshLog),
            typeof(CherryLog),
            typeof(EbonyLog),
            typeof(GoldenOakLog),
            typeof(HickoryLog),
            typeof(MahoganyLog),
            typeof(OakLog),
            typeof(PineLog),
            typeof(RosewoodLog),
            typeof(WalnutLog),
            typeof(DriftwoodLog),
            typeof(GhostLog),
            typeof(PetrifiedLog),
            typeof(ElvenLog),
        };
        public static Type[] Logs
        {
            get { return m_Logs; }
        }

        private static Type[] m_Boards = new Type[]
        {
            typeof(Board),
            typeof(AshBoard),
            typeof(CherryBoard),
            typeof(EbonyBoard),
            typeof(GoldenOakBoard),
            typeof(HickoryBoard),
            typeof(MahoganyBoard),
            typeof(OakBoard),
            typeof(PineBoard),
            typeof(RosewoodBoard),
            typeof(WalnutBoard),
            typeof(DriftwoodBoard),
            typeof(GhostBoard),
            typeof(PetrifiedBoard),
            typeof(ElvenBoard),
        };
        public static Type[] Boards
        {
            get { return m_Boards; }
        }

        private static Type[] m_Ingots = new Type[]
        {
            typeof(IronIngot),
            typeof(DullCopperIngot),
            typeof(ShadowIronIngot),
            typeof(CopperIngot),
            typeof(BronzeIngot),
            typeof(GoldIngot),
            typeof(AgapiteIngot),
            typeof(VeriteIngot),
            typeof(ValoriteIngot),
            typeof(SteelIngot),
            typeof(BrassIngot),
            typeof(MithrilIngot),
            typeof(DwarvenIngot),
            typeof(XormiteIngot),
            typeof(ObsidianIngot),
            typeof(NepturiteIngot),
        };
        public static Type[] Ingots
        {
            get { return m_Ingots; }
        }

        private static Type[] m_Granites = new Type[]
        {
            typeof(Granite),
            typeof(DullCopperGranite),
            typeof(ShadowIronGranite),
            typeof(CopperGranite),
            typeof(BronzeGranite),
            typeof(GoldGranite),
            typeof(AgapiteGranite),
            typeof(VeriteGranite),
            typeof(ValoriteGranite),
            typeof(ObsidianGranite),
            typeof(MithrilGranite),
            typeof(DwarvenGranite),
            typeof(XormiteGranite),
            typeof(NepturiteGranite),
        };
        public static Type[] Granites
        {
            get { return m_Granites; }
        }

        private static Type[] m_Scales = new Type[]
        {
            typeof(RedScales),
            typeof(YellowScales),
            typeof(BlackScales),
            typeof(GreenScales),
            typeof(WhiteScales),
            typeof(BlueScales),
            typeof(DinosaurScales),
        };
        public static Type[] Scales
        {
            get { return m_Scales; }
        }

        private static Type[] m_Leathers = new Type[]
        {
            typeof(Leather),
            typeof(SpinedLeather),
            typeof(HornedLeather),
            typeof(BarbedLeather),
            typeof(NecroticLeather),
            typeof(VolcanicLeather),
            typeof(FrozenLeather),
            typeof(GoliathLeather),
            typeof(DraconicLeather),
            typeof(HellishLeather),
            typeof(DinosaurLeather),
            typeof(AlienLeather),
        };
        public static Type[] Leathers
        {
            get { return m_Leathers; }
        }

        private static Type[] m_Misc = new Type[]
        {
            typeof(Sand),
            typeof(Cloth),
            typeof(Cotton),
            typeof(Flax),
            typeof(Wool),
            typeof(Bandage),
            typeof(Arrow),
            typeof(Bolt),
            typeof(Feather),
            typeof(Shaft),

            /*	typeof( FletcherTools ),
                typeof( TinkerTools ),
                typeof( Saw ),
                typeof( Tongs ),
                typeof( MortarPestle ),
                typeof( SewingKit ),
                typeof( Pickaxe ),
                typeof( ScribesPen ), */
        };
        public static Type[] Misc
        {
            get { return m_Misc; }
        }

        private static Type[] m_Reagents = new Type[]
        {
            typeof(BlackPearl),
            typeof(Bloodmoss),
            typeof(Garlic),
            typeof(Ginseng),
            typeof(MandrakeRoot),
            typeof(Nightshade),
            typeof(SulfurousAsh),
            typeof(SpidersSilk),
            typeof(Brimstone),
            typeof(ButterflyWings),
            typeof(EyeOfToad),
            typeof(FairyEgg),
            typeof(GargoyleEar),
            typeof(BeetleShell),
            typeof(MoonCrystal),
            typeof(PixieSkull),
            typeof(RedLotus),
            typeof(SeaSalt),
            typeof(SilverWidow),
            typeof(SwampBerries),
            typeof(BatWing),
            typeof(GraveDust),
            typeof(DaemonBlood),
            typeof(NoxCrystal),
            typeof(PigIron),
            typeof(BitterRoot),
            typeof(BlackSand),
            typeof(BloodRose),
            typeof(DriedToad),
            typeof(Maggot),
            typeof(MummyWrap),
            typeof(VioletFungus),
            typeof(WerewolfClaw),
            typeof(Wolfsbane),
        };
        public static Type[] Reagents
        {
            get { return m_Reagents; }
        }
    }
}
