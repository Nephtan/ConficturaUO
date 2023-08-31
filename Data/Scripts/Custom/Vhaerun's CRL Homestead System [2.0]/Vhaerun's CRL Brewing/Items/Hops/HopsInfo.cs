using System;
using System.Collections;

namespace Server.Items
{
    public enum HopsVariety
    {
        None = 0,
        BitterHops = 1,
        SnowHops,
        ElvenHops,
        SweetHops
    }

    public enum HopsVarietyType
    {
        None,
        Hops
    }

    public class HopsVarietyInfo
    {
        private int m_Hue;
        private int m_Number;
        private string m_Name;
        private HopsVariety m_Variety;
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
        public HopsVariety Resource
        {
            get { return m_Variety; }
        }
        public Type[] VarietyTypes
        {
            get { return m_VarietyTypes; }
        }

        public HopsVarietyInfo(
            int hue,
            int number,
            string name,
            HopsVariety variety,
            params Type[] varietyTypes
        )
        {
            m_Hue = hue;
            m_Number = number;
            m_Name = name;
            m_Variety = variety;
            m_VarietyTypes = varietyTypes;

            for (int i = 0; i < varietyTypes.Length; ++i)
                BrewingResources.RegisterType(varietyTypes[i], variety);
        }
    }

    public class BrewingResources
    {
        private static HopsVarietyInfo[] m_HopsInfo = new HopsVarietyInfo[]
        {
            new HopsVarietyInfo(
                0x000,
                0,
                "Bitter Hops",
                HopsVariety.BitterHops,
                typeof(BitterHops)
            ),
            new HopsVarietyInfo(0x481, 0, "Snow Hops", HopsVariety.SnowHops, typeof(SnowHops)),
            new HopsVarietyInfo(0x17, 0, "Elven Hops", HopsVariety.ElvenHops, typeof(ElvenHops)),
            new HopsVarietyInfo(0x30, 0, "Sweet Hops", HopsVariety.SweetHops, typeof(SweetHops))
        };

        /// <summary>
        /// Returns true if '<paramref name="variety"/>' is None or BitterHops, False if otherwise.
        /// </summary>
        public static bool IsStandard(HopsVariety variety)
        {
            return (variety == HopsVariety.None || variety == HopsVariety.BitterHops);
        }

        private static Hashtable m_TypeTable;

        /// <summary>
        /// Registers that '<paramref name="resourceType"/>' uses '<paramref name="variety"/>' so that it can later be queried by <see cref="BrewingResources.GetFromType"/>
        /// </summary>
        public static void RegisterType(Type resourceType, HopsVariety variety)
        {
            if (m_TypeTable == null)
                m_TypeTable = new Hashtable();

            m_TypeTable[resourceType] = variety;
        }

        /// <summary>
        /// Returns the <see cref="HopsVariety"/> value for which '<paramref name="resourceType"/>' uses -or- HopsVariety.None if an unregistered type was specified.
        /// </summary>
        public static HopsVariety GetFromType(Type resourceType)
        {
            if (m_TypeTable == null)
                return HopsVariety.None;

            object obj = m_TypeTable[resourceType];

            if (!(obj is HopsVariety))
                return HopsVariety.None;

            return (HopsVariety)obj;
        }

        /// <summary>
        /// Returns a <see cref="HopsVarietyInfo"/> instance describing '<paramref name="variety"/>' -or- null if an invalid variety was specified.
        /// </summary>
        public static HopsVarietyInfo GetInfo(HopsVariety variety)
        {
            HopsVarietyInfo[] list = null;

            switch (GetType(variety))
            {
                case HopsVarietyType.Hops:
                    list = m_HopsInfo;
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
        /// Returns a <see cref="HopsVarietyType"/> value indiciating the type of '<paramref name="variety"/>'.
        /// </summary>
        public static HopsVarietyType GetType(HopsVariety variety)
        {
            if (variety >= HopsVariety.BitterHops && variety <= HopsVariety.SweetHops)
                return HopsVarietyType.Hops;

            return HopsVarietyType.None;
        }

        /// <summary>
        /// Returns the first <see cref="GrapeVariety"/> in the series of varietys for which '<paramref name="variety"/>' belongs.
        /// </summary>
        public static HopsVariety GetStart(HopsVariety variety)
        {
            switch (GetType(variety))
            {
                case HopsVarietyType.Hops:
                    return HopsVariety.BitterHops;
            }

            return HopsVariety.None;
        }

        /// <summary>
        /// Returns the index of '<paramref name="variety"/>' in the seriest of varietys for which it belongs.
        /// </summary>
        public static int GetIndex(HopsVariety variety)
        {
            HopsVariety start = GetStart(variety);

            if (start == HopsVariety.None)
                return 0;

            return (int)(variety - start);
        }

        /// <summary>
        /// Returns the <see cref="HopsVarietyInfo.Number"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetLocalizationNumber(HopsVariety variety)
        {
            HopsVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Number);
        }

        /// <summary>
        /// Returns the <see cref="GrapeVarietyInfo.Hue"/> property of '<paramref name="variety"/>' -or- 0 if an invalid variety was specified.
        /// </summary>
        public static int GetHue(HopsVariety variety)
        {
            HopsVarietyInfo info = GetInfo(variety);

            return (info == null ? 0 : info.Hue);
        }

        /// <summary>
        /// Returns the <see cref="HopsVarietyInfo.Name"/> property of '<paramref name="variety"/>' -or- an empty string if the variety specified was invalid.
        /// </summary>
        public static string GetName(HopsVariety variety)
        {
            HopsVarietyInfo info = GetInfo(variety);

            return (info == null ? String.Empty : info.Name);
        }

        /// <summary>
        /// Returns the <see cref="HopsVariety"/> value which represents '<paramref name="info"/>' -or- HopsVariety.None if unable to convert.
        /// </summary>
        public static HopsVariety GetFromHopsInfo(HopsInfo info)
        {
            if (info.Level == 0)
                return HopsVariety.BitterHops;
            else if (info.Level == 1)
                return HopsVariety.SnowHops;
            else if (info.Level == 2)
                return HopsVariety.ElvenHops;
            else if (info.Level == 3)
                return HopsVariety.SweetHops;

            return HopsVariety.None;
        }
    }

    // NOTE: This class is only for compatability with very old RunUO versions.
    // No changes to it should be required for custom varietys.
    public class HopsInfo
    {
        //new HopsInfo (index, hue, name)
        public static readonly HopsInfo BitterHops = new HopsInfo(0, 0x000, "Bitter Hops");
        public static readonly HopsInfo SnowHops = new HopsInfo(1, 0x481, "Snow Hops");
        public static readonly HopsInfo ElvenHops = new HopsInfo(2, 0x17, "Elven Hops");
        public static readonly HopsInfo SweetHops = new HopsInfo(3, 0x30, "Sweet Hops");

        private int m_Level;
        private int m_Hue;
        private string m_Name;

        public HopsInfo(int level, int hue, string name)
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
