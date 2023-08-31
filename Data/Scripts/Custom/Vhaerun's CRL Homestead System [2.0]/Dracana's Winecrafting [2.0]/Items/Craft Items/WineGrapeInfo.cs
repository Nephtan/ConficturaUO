//================================================//
// Created by dracana				  //
// Desc: Base Winegrape variety information       //
//================================================//
using System;
using System.Collections;

namespace Server.Items
{
    public enum GrapeVariety
    {
        None = 0,
        CabernetSauvignon = 1,
        Chardonnay,
        CheninBlanc,
        Merlot,
        PinotNoir,
        Riesling,
        Sangiovese,
        SauvignonBlanc,
        Shiraz,
        Viognier,
        Zinfandel
    }

    public enum GrapeVarietyType
    {
        None,
        Grapes
    }

    public class GrapeVarietyInfo
    {
        private int m_Hue;
        private int m_Number;
        private string m_Name;
        private GrapeVariety m_Variety;
        private Type[] m_VarietyTypes;

        public int Hue
        {
            get { return m_Hue; }
        }
        public int Number
        {
            get { return m_Number; }
        }
        public string Name
        {
            get { return m_Name; }
        }
        public GrapeVariety Resource
        {
            get { return m_Variety; }
        }
        public Type[] VarietyTypes
        {
            get { return m_VarietyTypes; }
        }

        public GrapeVarietyInfo(
            int hue,
            int number,
            string name,
            GrapeVariety variety,
            params Type[] varietyTypes
        )
        {
            m_Hue = hue;
            m_Number = number;
            m_Name = name;
            m_Variety = variety;
            m_VarietyTypes = varietyTypes;

            for (int i = 0; i < varietyTypes.Length; ++i)
                WinemakingResources.RegisterType(varietyTypes[i], variety);
        }
    }

    public class WinemakingResources
    {
        private static GrapeVarietyInfo[] m_GrapeInfo = new GrapeVarietyInfo[]
        {
            new GrapeVarietyInfo(
                0x000,
                0,
                "Cabernet Sauvignon",
                GrapeVariety.CabernetSauvignon,
                typeof(CabernetSauvignonGrapes)
            ),
            new GrapeVarietyInfo(
                0x1CC,
                0,
                "Chardonnay",
                GrapeVariety.Chardonnay,
                typeof(ChardonnayGrapes)
            ),
            new GrapeVarietyInfo(
                0x16B,
                0,
                "Chenin Blanc",
                GrapeVariety.CheninBlanc,
                typeof(CheninBlancGrapes)
            ),
            new GrapeVarietyInfo(0x2CE, 0, "Merlot", GrapeVariety.Merlot, typeof(MerlotGrapes)),
            new GrapeVarietyInfo(
                0x2CE,
                0,
                "Pinot Noir",
                GrapeVariety.PinotNoir,
                typeof(PinotNoirGrapes)
            ),
            new GrapeVarietyInfo(
                0x1CC,
                0,
                "Riesling",
                GrapeVariety.Riesling,
                typeof(RieslingGrapes)
            ),
            new GrapeVarietyInfo(
                0x000,
                0,
                "Sangiovese",
                GrapeVariety.Sangiovese,
                typeof(SangioveseGrapes)
            ),
            new GrapeVarietyInfo(
                0x16B,
                0,
                "Sauvignon Blanc",
                GrapeVariety.SauvignonBlanc,
                typeof(SauvignonBlancGrapes)
            ),
            new GrapeVarietyInfo(0x2CE, 0, "Shiraz", GrapeVariety.Shiraz, typeof(ShirazGrapes)),
            new GrapeVarietyInfo(
                0x16B,
                0,
                "Viognier",
                GrapeVariety.Viognier,
                typeof(ViognierGrapes)
            ),
            new GrapeVarietyInfo(
                0x000,
                0,
                "Zinfandel",
                GrapeVariety.Zinfandel,
                typeof(ZinfandelGrapes)
            )
        };

        /// <summary>
        /// Returns true if '<paramref name="variety"/>' is None or CabernetSauvignon, False if otherwise.
        /// </summary>
        public static bool IsStandard(GrapeVariety variety)
        {
            return (variety == GrapeVariety.None || variety == GrapeVariety.CabernetSauvignon);
        }

        private static Hashtable m_TypeTable;

        /// <summary>
        /// Registers that '<paramref name="resourceType"/>' uses '<paramref name="variety"/>' so that it can later be queried by <see cref="WinemakingResources.GetFromType"/>
        /// </summary>
        public static void RegisterType(Type resourceType, GrapeVariety variety)
        {
            if (m_TypeTable == null)
                m_TypeTable = new Hashtable();

            m_TypeTable[resourceType] = variety;
        }

        /// <summary>
        /// Returns the <see cref="GrapeVariety"/> value for which '<paramref name="resourceType"/>' uses -or- GrapeVariety.None if an unregistered type was specified.
        /// </summary>
        public static GrapeVariety GetFromType(Type resourceType)
        {
            if (m_TypeTable == null)
                return GrapeVariety.None;

            object obj = m_TypeTable[resourceType];

            if (!(obj is GrapeVariety))
                return GrapeVariety.None;

            return (GrapeVariety)obj;
        }

        /// <summary>
        /// Returns a <see cref="GrapeVarietyInfo"/> instance describing '<paramref name="variety"/>' -or- null if an invalid variety was specified.
        /// </summary>
        public static GrapeVarietyInfo GetInfo(GrapeVariety variety)
        {
            GrapeVarietyInfo[] list = null;

            switch (GetType(variety))
            {
                case GrapeVarietyType.Grapes:
                    list = m_GrapeInfo;
                    break;
            }

            if (list != null)
            {
                int index = GetIndex(variety);

                if (index >= 0 && index < list.Length)
                    return list[index];
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="GrapeVarietyType"/> value indiciating the type of '<paramref name="variety"/>'.
        /// </summary>
        public static GrapeVarietyType GetType(GrapeVariety variety)
        {
            if (variety >= GrapeVariety.CabernetSauvignon && variety <= GrapeVariety.Zinfandel)
                return GrapeVarietyType.Grapes;

            return GrapeVarietyType.None;
        }

        /// <summary>
        /// Returns the first <see cref="GrapeVariety"/> in the series of varietys for which '<paramref name="variety"/>' belongs.
        /// </summary>
        public static GrapeVariety GetStart(GrapeVariety variety)
        {
            switch (GetType(variety))
            {
                case GrapeVarietyType.Grapes:
                    return GrapeVariety.CabernetSauvignon;
            }

            return GrapeVariety.None;
        }

        /// <summary>
        /// Returns the index of '<paramref name="variety"/>' in the seriest of varietys for which it belongs.
        /// </summary>
        public static int GetIndex(GrapeVariety variety)
        {
            GrapeVariety start = GetStart(variety);

            if (start == GrapeVariety.None)
                return 0;

            return (int)(variety - start);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVarietyInfo.Number"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetLocalizationNumber(GrapeVariety variety)
        {
            GrapeVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Number);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVarietyInfo.Hue"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetHue(GrapeVariety variety)
        {
            GrapeVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Hue);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVarietyInfo.Name"/> property of '<paramref name="variety"/>' -or- an empty string if the variety specified was invalid.
        /// </summary>
        public static string GetName(GrapeVariety variety)
        {
            GrapeVarietyInfo info = GetInfo(variety);

            return (info == null ? String.Empty : info.Name);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVariety"/> value which represents '<paramref name="info"/>' -or- GrapeVariety.None if unable to convert.
        /// </summary>
        public static GrapeVariety GetFromWineGrapeInfo(WineGrapeInfo info)
        {
            if (info.Level == 0)
                return GrapeVariety.CabernetSauvignon;
            else if (info.Level == 1)
                return GrapeVariety.Chardonnay;
            else if (info.Level == 2)
                return GrapeVariety.CheninBlanc;
            else if (info.Level == 3)
                return GrapeVariety.Merlot;
            else if (info.Level == 4)
                return GrapeVariety.PinotNoir;
            else if (info.Level == 5)
                return GrapeVariety.Riesling;
            else if (info.Level == 6)
                return GrapeVariety.Sangiovese;
            else if (info.Level == 7)
                return GrapeVariety.SauvignonBlanc;
            else if (info.Level == 8)
                return GrapeVariety.Shiraz;
            else if (info.Level == 9)
                return GrapeVariety.Viognier;
            else if (info.Level == 10)
                return GrapeVariety.Zinfandel;

            return GrapeVariety.None;
        }
    }

    // NOTE: This class is only for compatability with very old RunUO versions.
    // No changes to it should be required for custom varietys.
    public class WineGrapeInfo
    {
        //new WineGrapeInfo (index, hue, name)
        public static readonly WineGrapeInfo CabernetSauvignon = new WineGrapeInfo(
            0,
            0x000,
            "Cabernet Sauvignon"
        );
        public static readonly WineGrapeInfo Chardonnay = new WineGrapeInfo(1, 0x1CC, "Chardonnay");
        public static readonly WineGrapeInfo CheninBlanc = new WineGrapeInfo(
            2,
            0x16B,
            "Chenin Blanc"
        );
        public static readonly WineGrapeInfo Merlot = new WineGrapeInfo(3, 0x2CE, "Merlot");
        public static readonly WineGrapeInfo PinotNoir = new WineGrapeInfo(4, 0x2CE, "Pinot Noir");
        public static readonly WineGrapeInfo Riesling = new WineGrapeInfo(5, 0x1CC, "Riesling");
        public static readonly WineGrapeInfo Sangiovese = new WineGrapeInfo(6, 0x000, "Sangiovese");
        public static readonly WineGrapeInfo SauvignonBlanc = new WineGrapeInfo(
            7,
            0x16B,
            "Sauvignon Blanc"
        );
        public static readonly WineGrapeInfo Shiraz = new WineGrapeInfo(8, 0x2CE, "Shiraz");
        public static readonly WineGrapeInfo Viognier = new WineGrapeInfo(9, 0x16B, "Viognier");
        public static readonly WineGrapeInfo Zinfandel = new WineGrapeInfo(10, 0x000, "Zinfandel");

        private int m_Level;
        private int m_Hue;
        private string m_Name;

        public WineGrapeInfo(int level, int hue, string name)
        {
            m_Level = level;
            m_Hue = hue;
            m_Name = name;
        }

        public int Level
        {
            get { return m_Level; }
        }

        public int Hue
        {
            get { return m_Hue; }
        }

        public string Name
        {
            get { return m_Name; }
        }
    }
}
