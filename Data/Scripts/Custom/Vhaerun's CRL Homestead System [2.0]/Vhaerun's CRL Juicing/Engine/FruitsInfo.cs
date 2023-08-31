using System;
using System.Collections;

namespace Server.Items
{
    public enum FruitsVariety
    {
        None = 0,
        Apple = 1,
        Banana,
        Dates,
        Grapes,
        Lemon,
        Lime,
        Orange,
        Peach,
        Pear,
        Pumpkin,
        Tomato,
        Watermelon,
        Apricot,
        Blackberries,
        Blueberries,
        Cherries,
        Cranberries,
        Grapefruit,
        Kiwi,
        Mango,
        Pineapple,
        Pomegranate,
        Strawberry,
    }

    public enum FruitsVarietyType
    {
        None,
        Fruits
    }

    public class FruitsVarietyInfo
    {
        private int m_Hue;
        private int m_Number;
        private string m_Name;
        private FruitsVariety m_Variety;
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
        public FruitsVariety Resource
        {
            get { return m_Variety; }
        }
        public Type[] VarietyTypes
        {
            get { return m_VarietyTypes; }
        }

        public FruitsVarietyInfo(
            int hue,
            int number,
            string name,
            FruitsVariety variety,
            params Type[] varietyTypes
        )
        {
            m_Hue = hue;
            m_Number = number;
            m_Name = name;
            m_Variety = variety;
            m_VarietyTypes = varietyTypes;

            for (int i = 0; i < varietyTypes.Length; ++i)
                JuicingResources.RegisterType(varietyTypes[i], variety);
        }
    }

    public class JuicingResources
    {
        private static FruitsVarietyInfo[] m_FruitsInfo = new FruitsVarietyInfo[]
        {
            new FruitsVarietyInfo(0x0, 0, "Apple", FruitsVariety.Apple, typeof(Apple)),
            new FruitsVarietyInfo(0x0, 0, "Banana", FruitsVariety.Banana, typeof(Banana)),
            new FruitsVarietyInfo(0x0, 0, "Date", FruitsVariety.Dates, typeof(Dates)),
            new FruitsVarietyInfo(0x0, 0, "Grape", FruitsVariety.Grapes, typeof(Grapes)),
            new FruitsVarietyInfo(0x0, 0, "Lemon", FruitsVariety.Lemon, typeof(Lemon)),
            new FruitsVarietyInfo(0x0, 0, "Lime", FruitsVariety.Lime, typeof(Lime)),
            new FruitsVarietyInfo(0x0, 0, "Orange", FruitsVariety.Orange, typeof(Orange)),
            new FruitsVarietyInfo(0x0, 0, "Peach", FruitsVariety.Peach, typeof(Peach)),
            new FruitsVarietyInfo(0x0, 0, "Pear", FruitsVariety.Pear, typeof(Pear)),
            new FruitsVarietyInfo(0x0, 0, "Pumpkin", FruitsVariety.Pumpkin, typeof(Pumpkin)),
            new FruitsVarietyInfo(0x0, 0, "Tomato", FruitsVariety.Tomato, typeof(Tomato)),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Watermelon",
                FruitsVariety.Watermelon,
                typeof(Watermelon)
            ),
            new FruitsVarietyInfo(0x0, 0, "Apricot", FruitsVariety.Apricot, typeof(Apricot)),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Blackberry",
                FruitsVariety.Blackberries,
                typeof(Blackberries)
            ),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Blueberry",
                FruitsVariety.Blueberries,
                typeof(Blueberries)
            ),
            new FruitsVarietyInfo(0x0, 0, "Cherry", FruitsVariety.Cherries, typeof(Cherries)),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Cranberry",
                FruitsVariety.Cranberries,
                typeof(Cranberries)
            ),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Grapefruit",
                FruitsVariety.Grapefruit,
                typeof(Grapefruit)
            ),
            new FruitsVarietyInfo(0x0, 0, "Kiwi", FruitsVariety.Kiwi, typeof(Kiwi)),
            new FruitsVarietyInfo(0x0, 0, "Mango", FruitsVariety.Mango, typeof(Mango)),
            new FruitsVarietyInfo(0x0, 0, "Pineapple", FruitsVariety.Pineapple, typeof(Pineapple)),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Pomegranate",
                FruitsVariety.Pomegranate,
                typeof(Pomegranate)
            ),
            new FruitsVarietyInfo(
                0x0,
                0,
                "Strawberry",
                FruitsVariety.Strawberry,
                typeof(Strawberry)
            )
        };

        /// <summary>
        /// Returns true if '<paramref name="variety"/>' is None or Apple, False if otherwise.
        /// </summary>
        public static bool IsStandard(FruitsVariety variety)
        {
            return (variety == FruitsVariety.None || variety == FruitsVariety.Apple);
        }

        private static Hashtable m_TypeTable;

        /// <summary>
        /// Registers that '<paramref name="resourceType"/>' uses '<paramref name="variety"/>' so that it can later be queried by <see cref="JuicingResources.GetFromType"/>
        /// </summary>
        public static void RegisterType(Type resourceType, FruitsVariety variety)
        {
            if (m_TypeTable == null)
                m_TypeTable = new Hashtable();

            m_TypeTable[resourceType] = variety;
        }

        /// <summary>
        /// Returns the <see cref="FruitsVariety"/> value for which '<paramref name="resourceType"/>' uses -or- FruitsVariety.None if an unregistered type was specified.
        /// </summary>
        public static FruitsVariety GetFromType(Type resourceType)
        {
            if (m_TypeTable == null)
                return FruitsVariety.None;

            object obj = m_TypeTable[resourceType];

            if (!(obj is FruitsVariety))
                return FruitsVariety.None;

            return (FruitsVariety)obj;
        }

        /// <summary>
        /// Returns a <see cref="FruitsVarietyInfo"/> instance describing '<paramref name="variety"/>' -or- null if an invalid variety was specified.
        /// </summary>
        public static FruitsVarietyInfo GetInfo(FruitsVariety variety)
        {
            FruitsVarietyInfo[] list = null;

            switch (GetType(variety))
            {
                case FruitsVarietyType.Fruits:
                    list = m_FruitsInfo;
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
        /// Returns a <see cref="FruitsVarietyType"/> value indiciating the type of '<paramref name="variety"/>'.
        /// </summary>
        public static FruitsVarietyType GetType(FruitsVariety variety)
        {
            if (variety >= FruitsVariety.Apple && variety <= FruitsVariety.Strawberry)
                return FruitsVarietyType.Fruits;

            return FruitsVarietyType.None;
        }

        /// <summary>
        /// Returns the first <see cref="GrapeVariety"/> in the series of varietys for which '<paramref name="variety"/>' belongs.
        /// </summary>
        public static FruitsVariety GetStart(FruitsVariety variety)
        {
            switch (GetType(variety))
            {
                case FruitsVarietyType.Fruits:
                    return FruitsVariety.Apple;
            }

            return FruitsVariety.None;
        }

        /// <summary>
        /// Returns the index of '<paramref name="variety"/>' in the seriest of varietys for which it belongs.
        /// </summary>
        public static int GetIndex(FruitsVariety variety)
        {
            FruitsVariety start = GetStart(variety);

            if (start == FruitsVariety.None)
                return 0;

            return (int)(variety - start);
        }

        /// <summary>
        /// Returns the <see cref="FruitsVarietyInfo.Number"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetLocalizationNumber(FruitsVariety variety)
        {
            FruitsVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Number);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVarietyInfo.Hue"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetHue(FruitsVariety variety)
        {
            FruitsVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Hue);
        }

        /// <summary>
        /// Returns the <see cref="FruitsVarietyInfo.Name"/> property of '<paramref name="variety"/>' -or- an empty string if the variety specified was invalid.
        /// </summary>
        public static string GetName(FruitsVariety variety)
        {
            FruitsVarietyInfo info = GetInfo(variety);

            return (info == null ? String.Empty : info.Name);
        }

        /// <summary>
        /// Returns the <see cref="FruitsVariety"/> value which represents '<paramref name="info"/>' -or- FruitsVariety.None if unable to convert.
        /// </summary>
        public static FruitsVariety GetFromFruitsInfo(FruitsInfo info)
        {
            if (info.Level == 0)
                return FruitsVariety.Apple;
            else if (info.Level == 1)
                return FruitsVariety.Banana;
            else if (info.Level == 2)
                return FruitsVariety.Dates;
            else if (info.Level == 3)
                return FruitsVariety.Grapes;
            else if (info.Level == 4)
                return FruitsVariety.Lemon;
            else if (info.Level == 5)
                return FruitsVariety.Lime;
            else if (info.Level == 6)
                return FruitsVariety.Orange;
            else if (info.Level == 7)
                return FruitsVariety.Peach;
            else if (info.Level == 8)
                return FruitsVariety.Pear;
            else if (info.Level == 9)
                return FruitsVariety.Pumpkin;
            else if (info.Level == 10)
                return FruitsVariety.Tomato;
            else if (info.Level == 11)
                return FruitsVariety.Watermelon;
            else if (info.Level == 12)
                return FruitsVariety.Apricot;
            else if (info.Level == 13)
                return FruitsVariety.Blackberries;
            else if (info.Level == 14)
                return FruitsVariety.Blueberries;
            else if (info.Level == 15)
                return FruitsVariety.Cherries;
            else if (info.Level == 16)
                return FruitsVariety.Cranberries;
            else if (info.Level == 17)
                return FruitsVariety.Grapefruit;
            else if (info.Level == 18)
                return FruitsVariety.Kiwi;
            else if (info.Level == 19)
                return FruitsVariety.Mango;
            else if (info.Level == 20)
                return FruitsVariety.Pineapple;
            else if (info.Level == 21)
                return FruitsVariety.Pomegranate;
            else if (info.Level == 22)
                return FruitsVariety.Strawberry;

            return FruitsVariety.None;
        }
    }

    // NOTE: This class is only for compatability with very old RunUO versions.
    // No changes to it should be required for custom varietys.
    public class FruitsInfo
    {
        //new FruitsInfo (index, hue, name)
        public static readonly FruitsInfo BitterFruits = new FruitsInfo(0, 0x000, "Bitter Fruits");
        public static readonly FruitsInfo SnowFruits = new FruitsInfo(1, 0x481, "Snow Fruits");
        public static readonly FruitsInfo ElvenFruits = new FruitsInfo(2, 0x17, "Elven Fruits");
        public static readonly FruitsInfo SweetFruits = new FruitsInfo(3, 0x30, "Sweet Fruits");

        private int m_Level;
        private int m_Hue;
        private string m_Name;

        public FruitsInfo(int level, int hue, string name)
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
