//------------------------------------------------------------------------------
///  <summary>
///
///  </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Globalization;
//------------------------------------------------------------------------------
using Server;
using Server.Misc;
using Server.Items;
using Server.Regions;
using Server.Mobiles;
using Server.Targeting;
using Server.Accounting;
using Server.Network;
using Server.Engines.XmlSpawner2;

//------------------------------------------------------------------------------
namespace Server.Misc
{
    class Helpers
    {
        private static Hashtable m_RoadTileHash;

        public static void Initialize()
        {
            m_RoadTileHash = GenerateRoadTileHash();
        }

        //----------------------------------------------------------------------
        //  Determines basic time of day for a mobile in notional time;
        //----------------------------------------------------------------------
        public static EncounterTime DetermineTimeForMobile(Mobile m)
        {
            int hours,
                minutes;

            Server.Items.Clock.GetTime(m.Map, m.X, m.Y, out hours, out minutes);

            if (hours < 4)
                return EncounterTime.Night;
            if (hours < 6)
                return EncounterTime.Twilight;
            if (hours > 22)
                return EncounterTime.Twilight;

            return EncounterTime.Day;
        }

        //----------------------------------------------------------------------
        //
        //----------------------------------------------------------------------
        public static string RegionCategory(Region currentRegion)
        {
            string str;

            if (currentRegion is VillageRegion)
            {
                str = "VillageRegion";
            }
            else if (currentRegion is SafeRegion)
            {
                str = "SafeRegion";
            }
            else if (currentRegion is DungeonRegion)
            {
                str = "DungeonRegion";
            }
            else
            {
                str = "OutDoorRegion";
            }

            return str;
        }

        //----------------------------------------------------------------------
        //  DetermineLandTypeForMobile -- determines what time of day it is for
        //     the player
        //----------------------------------------------------------------------
        public static LandType DetermineLandTypeForMobile(Mobile m)
        {
            LandTile landTile = m.Map.Tiles.GetLandTile(m.X, m.Y);

            int landId = landTile.ID & 0x3FFF;

            if ((TileData.LandTable[landTile.ID & 0x3FFF].Flags & TileFlag.Wet) == TileFlag.Wet)
                return LandType.Water;

            if (m_RoadTileHash.ContainsKey(landId))
                return LandType.OnRoad;

            return LandType.OffRoad;
        }

        //----------------------------------------------------------------------
        //
        //----------------------------------------------------------------------

        // This method calculates the level of a mobile based on its attributes, skills, fame, and karma
        // It takes in two parameters: the mobile to calculate the level for, and the type of level to calculate
        public static double CalculateLevelForMobile(Mobile m, LevelType levelType)
        {
            int fame = m.Fame;

            if (fame > 15000)
            {
                fame = 15000;
            }

            int karma = m.Karma;

            if (karma < 0)
            {
                karma = m.Karma * -1;
            }

            if (karma > 15000)
            {
                karma = 15000;
            }

            int skills = m.Skills.Total;

            if (skills > 10000)
            {
                skills = 10000;
            }

            skills = (int)(1.5 * skills); // UP TO 15,000

            int stats = m.RawStr + m.RawDex + m.RawInt;

            if (stats > 250)
            {
                stats = 250;
            }

            stats = 60 * stats; // UP TO 15,000

            int calcLevel = (int)((fame + karma + skills + stats) / 600);

            calcLevel = (int)((calcLevel - 10) * 1.12);

            if (calcLevel < 1)
            {
                calcLevel = 1;
            }

            if (calcLevel > 100)
            {
                calcLevel = 100;
            }

            return calcLevel;
        }

        public static double CalculateWeightedSkillsForClass(Mobile m, LevelType levelType)
        {
            double retval = 0;

            switch (levelType)
            {
                case (LevelType.Fighter):

                    retval +=
                        MaxOf(
                            m.Skills.Marksmanship.Value,
                            m.Skills.Fencing.Value,
                            m.Skills.Swords.Value,
                            m.Skills.Bludgeoning.Value,
                            m.Skills.FistFighting.Value
                        ) * 4;

                    retval +=
                        AvgOf(
                            m.Skills.Focus.Value,
                            m.Skills.Tactics.Value,
                            m.Skills.Anatomy.Value,
                            m.Skills.Parry.Value
                        ) * 2;

                    return retval;

                case (LevelType.Mage):

                    retval += m.Skills.Magery.Value * 4;

                    retval +=
                        AvgOf(
                            m.Skills.Alchemy.Value,
                            m.Skills.Inscribe.Value,
                            m.Skills.Meditation.Value,
                            m.Skills.MagicResist.Value,
                            m.Skills.Psychology.Value
                        ) * 2;

                    return retval;

                case (LevelType.Necromancer):

                    retval += m.Skills.Necromancy.Value * 4;

                    retval += AvgOf(m.Skills.Poisoning.Value, m.Skills.Spiritualism.Value) * 2;

                    return retval;

                case (LevelType.Thief):

                    retval += MaxOf(m.Skills.Stealing.Value, m.Skills.Snooping.Value) * 4;

                    retval +=
                        AvgOf(
                            m.Skills.Lockpicking.Value,
                            m.Skills.Hiding.Value,
                            m.Skills.Stealth.Value,
                            m.Skills.Begging.Value,
                            m.Skills.Searching.Value,
                            m.Skills.RemoveTrap.Value
                        ) * 2;

                    return retval;

                case (LevelType.Ranger):

                    retval += m.Skills.Tracking.Value * 4;

                    retval +=
                        AvgOf(
                            m.Skills.Cartography.Value,
                            m.Skills.Camping.Value,
                            m.Skills.Taming.Value,
                            m.Skills.Druidism.Value,
                            m.Skills.Veterinary.Value,
                            m.Skills.Herding.Value
                        ) * 2;

                    return retval;

                default: // Basic overall level, mostly for creatures...

                    RedBlackTree sortedSkills = new RedBlackTree(new DescendingComparer());

                    for (int i = 0; i < m.Skills.Length; i++)
                        sortedSkills.Add(m.Skills[i].Value);

                    double maxOfFirstFour = 0;
                    double sumSecondFour = 0;

                    int count = 0;
                    foreach (double skill in sortedSkills)
                    {
                        if (count < 4)
                        {
                            if (skill > maxOfFirstFour)
                                maxOfFirstFour = skill;
                        }
                        else
                        {
                            sumSecondFour += skill;
                        }

                        count++;

                        if (count == 7)
                            break;
                    }
                    return maxOfFirstFour * 4 + sumSecondFour / 4 * 2;
            }
        }

        public class DescendingComparer : IComparer
        {
            public int Compare(object o1, object o2)
            {
                double d1 = (double)o1;
                double d2 = (double)o2;

                if (d1 > d2)
                    return -1;
                if (d1 < d2)
                    return 1;
                else
                    return 0;
            }
        }

        //----------------------------------------------------------------------
        // Place each value of the road tile range set into a master hash table
        //   for rapid lookup.
        //----------------------------------------------------------------------
        private static Hashtable GenerateRoadTileHash()
        {
            Hashtable roadTiles = new Hashtable();

            for (int i = 0; i < m_RoadTileIds.Length; i += 2)
            {
                for (int j = m_RoadTileIds[i]; j < m_RoadTileIds[i + 1]; j++)
                    roadTiles[j] = j;
            }

            return roadTiles;
        }

        //----------------------------------------------------------------------
        //
        //----------------------------------------------------------------------
        private static int[] m_RoadTileIds = new int[]
        {
            0x071,
            0x08C,
            0x0E8,
            0x0EB,
            0x14C,
            0x14F,
            0x161,
            0x174,
            0x1F0,
            0x1F3,
            0x26E,
            0x279,
            0x27E,
            0x281,
            0x324,
            0x3AC,
            0x547,
            0x556,
            0x597,
            0x5A6,
            0x637,
            0x63A,
            0x7AE,
            0x7B1,
            0x442,
            0x479, // Sand stones
            0x501,
            0x510, // Sand stones
            0x009,
            0x015, // Furrows
            0x150,
            0x15C // Furrows
        };

        public static double MaxOf(params object[] paramlist)
        {
            double max = 0;
            foreach (double val in paramlist)
            {
                if (val > max)
                    max = val;
            }
            return max;
        }

        public static double AvgOf(params object[] paramlist)
        {
            double sum = 0;
            int i;
            for (i = 0; i < paramlist.Length; i++)
            {
                sum += (double)paramlist[i];
            }
            return sum / i;
        }
    }
}

//                    m.Skills.Alchemy.Value
//                    m.Skills.Anatomy.Value
//                    m.Skills.AnimalLore.Value
//                    m.Skills.ItemID.Value
//                    m.Skills.ArmsLore.Value
//                    m.Skills.Parry.Value
//                    m.Skills.Begging.Value
//                    m.Skills.Blacksmith.Value
//                    m.Skills.Fletching.Value
//                    m.Skills.Peacemaking.Value
//                    m.Skills.Camping.Value
//                    m.Skills.Carpentry.Value
//                    m.Skills.Cartography.Value
//                    m.Skills.Cooking.Value
//                    m.Skills.DetectHidden.Value
//                    m.Skills.Discordance.Value
//                    m.Skills.EvalInt.Value
//                    m.Skills.Healing.Value
//                    m.Skills.Fishing.Value
//                    m.Skills.Forensics.Value
//                    m.Skills.Herding.Value
//                    m.Skills.Hiding.Value
//                    m.Skills.Provocation.Value
//                    m.Skills.Inscribe.Value
//                    m.Skills.Lockpicking.Value
//                    m.Skills.Magery.Value
//                    m.Skills.MagicResist.Value
//                    m.Skills.Tactics.Value
//                    m.Skills.Snooping.Value
//                    m.Skills.Musicianship.Value
//                    m.Skills.Poisoning.Value
//                    m.Skills.Archery.Value
//                    m.Skills.SpiritSpeak.Value
//                    m.Skills.Stealing.Value
//                    m.Skills.Tailoring.Value
//                    m.Skills.AnimalTaming.Value
//                    m.Skills.TasteID.Value
//                    m.Skills.Tinkering.Value
//                    m.Skills.Tracking.Value
//                    m.Skills.Veterinary.Value
//                    m.Skills.Swords.Value
//                    m.Skills.Macing.Value
//                    m.Skills.Fencing.Value
//                    m.Skills.Wrestling.Value
//                    m.Skills.Lumberjacking.Value
//                    m.Skills.Mining.Value
//                    m.Skills.Meditation.Value
//                    m.Skills.Stealth.Value
//                    m.Skills.RemoveTrap.Value
//                    m.Skills.Necromancy.Value
//                    m.Skills.Focus.Value
//                    m.Skills.Chivalry.Value
//                    m.Skills.Bushido.Value
//                    m.Skills.Ninjitsu.Value
