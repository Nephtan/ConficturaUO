using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;

namespace Server
{
    public class PlayerGovernmentSystem
    {
        //System Version
        public static readonly string SystemVersion = "2.3";
        public static string FileName = "Data/GovernmentVersion.xml";
        public static string FileVersion = "0.0";

        //Forensics Requirement
        public static bool NeedsForensics = false;
        public static double ForensicsRequirement = 35.0;

        // Global Start Timer
        public static readonly TimeSpan StartUpdate = TimeSpan.FromHours(24.0); // Default 1140 = 24 hours

        // City Update Time
        public static readonly TimeSpan CityUpdate = TimeSpan.FromDays(7.0); // Default 10080 = 7 days

        // Mayor Voting Time
        public static readonly TimeSpan VoteUpdate = TimeSpan.FromDays(14.0); // Default 20160 = 14 Days

        // Staring Treasury Amount RECOMMENDED: At least 20k so if player forgets to add, and dont lose on first update.
        public static int TreasuryAmount = 150000; // Default 20000

        // Level 1 City Limit Offset // See Docs On How To Set this.
        public static int L1CLOffset = 25; // 50x50 Area

        // Level 2 City Limit Offset // See Docs On How To Set this.
        public static int L2CLOffset = 38; // 75x75 Area (Is really 76x76)

        // Level 3 City Limit Offset // See Docs On How To Set this.
        public static int L3CLOffset = 50; // 100x100 Area

        // Level 4 City Limit Offset // See Docs On How To Set this.
        public static int L4CLOffset = 75; // 150x150 Area

        // Level 5 City Limit Offset // See Docs On How To Set this.
        public static int L5CLOffset = 88; // 175x175 Area (Is really 176x176)

        // Level 6 City Limit Offset // See Docs On How To Set this.
        public static int L6CLOffset = 100; // 200x200 Area

        // Set the amount of tiles the system checks for cities in range. // See Docs On How To Set This.
        public static int CityRangeOffset = 130; // 260x260 Area

        // Number of Max Lockdowns per City, Per Level
        public static int Level1LD = 100; // Default 10
        public static int Level2LD = 200; // Default 50
        public static int Level3LD = 300; // Default 100
        public static int Level4LD = 400; // Default 200
        public static int Level5LD = 500; // Default 300
        public static int Level6LD = 600; // Default 500

        // Number of citizens needed for each city level.
        public static int Level1 = 6; // Default 20 12
        public static int Level2 = 12; // Default 40 20
        public static int Level3 = 18; // Default 60 35
        public static int Level4 = 24; // Default 80 45
        public static int Level5 = 30; // Default 100 50
        public static int Level6 = 36; // Default 200 55

        /*
         * Member Rules:
         *
         * 1. If any character on an account owns a house within the city limits, all characters on that account are eligible for city membership.
         * 2. Characters cannot belong to multiple cities, even if they own multiple houses in different cities.
         * 3. Members can join multiple characters from their account to a single city.
         * 4. Each character from the member's account contributes to the city's total population count.
         * 5. For example, if 6 characters are enabled per account and all join the city, that account contributes 6 members to the city's population.
         */

        // Title for cities at each level.
        public static string Title1 = "outpost";
        public static string Title2 = "village";
        public static string Title3 = "township";
        public static string Title4 = "city";
        public static string Title5 = "metropolis";
        public static string Title6 = "empire";

        // Enable city placement for Underworld (Note: need to let players place houses as well in the Underworld.)
        public static bool EnableUnderworld = false;

        // Max amounts of cities per map.
        public static int MaxCitiesForLodor = 20;
        public static int MaxCitiesForSosaria = 20;
        public static int MaxCitiesForUnderworld = 10; //No Housing In Underworld.. However can be changed.
        public static int MaxCitiesForSerpentIsland = 10;
        public static int MaxCitiesForIslesDread = 5;

        //Max number of citizens per city
        public static int MaxCitizensPerCity = 100;

        //Max number of banned players NOTE: if 0 will disable city banning.
        public static int MaxBannedPerCity = 50;

        public static void Initialize()
        {
            CheckCitySystemVersion();
        }

        public static void PlaceCityHall(Mobile from, Point3D p, CityDeed deed)
        {
            if (CheckPlacement(from, p, deed.MultiID, deed.Offset))
            {
                if (!CheckCitiesInRange(from, p))
                {
                    deed.FinishPlacement(from, p);
                    from.SendGump(new CityWarningGump());
                }
                else
                {
                    int offset = CityRangeOffset * 2;

                    from.SendMessage("Another city is too close to place your city hall here.");
                    from.SendMessage(
                        38,
                        "You must be at least {0} yards (tiles) away from any other city in order to place your city hall.",
                        offset
                    );
                }
            }
        }

        public static void PlaceCivic(Mobile from, Point3D p, CityDeed deed)
        {
            if (CheckPlacement(from, p, deed.MultiID, deed.Offset))
            {
                deed.FinishPlacement(from, p);
            }
        }

        public static bool CheckPlacement(Mobile from, Point3D p, int multiId, Point3D offset)
        {
            ArrayList toMove;
            Point3D center = new Point3D(p.X - offset.X, p.Y - offset.Y, p.Z - offset.Z);
            HousePlacementResult res = HousePlacement.Check(from, multiId, center, out toMove);

            switch (res)
            {
                case HousePlacementResult.Valid:
                    return true;
                case HousePlacementResult.BadItem:
                case HousePlacementResult.BadLand:
                case HousePlacementResult.BadStatic:
                case HousePlacementResult.BadRegionHidden:
                    return HandleInvalidTerrain(from);
                case HousePlacementResult.NoSurface:
                    return HandleNoSurface(from);
                case HousePlacementResult.BadRegion:
                    return HandleInvalidRegion(from);
                default:
                    return false;
            }
        }

        private static bool HandleInvalidTerrain(Mobile from)
        {
            from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
            return false;
        }

        private static bool HandleNoSurface(Mobile from)
        {
            from.SendMessage(
                "The house could not be created here.  Part of the foundation would not be on any surface."
            );
            return false;
        }

        private static bool HandleInvalidRegion(Mobile from)
        {
            from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
            return false;
        }

        public static bool CheckIfInUnderworld(Mobile from)
        {
            return from.Map == Map.Underworld && !EnableUnderworld;
        }

        public static bool CheckIfCanBeMayor(Mobile from)
        {
            if (NeedsForensics)
                if (from.Skills[SkillName.Forensics].Base >= 35.0)
                    return true;
                else
                    return false;
            else
                return true;
        }

        public static bool CheckCitiesInRange(Mobile from, Point3D p)
        {
            Map map = from.Map;
            int offset = CityRangeOffset;
            int offset2 = offset * 3;
            int x1 = p.X - offset;
            int y1 = p.Y - offset;
            int width = offset2;
            int height = offset2;
            int depth = p.Z;

            for (int x = x1; x <= x1 + width; x += Map.SectorSize)
            {
                for (int y = y1; y <= y1 + height; y += Map.SectorSize)
                {
                    Sector s = map.GetSector(new Point3D(x, y, depth));
                    foreach (RegionRect rect in s.RegionRects)
                    {
                        Region r = rect.Region;
                        if (from.AccessLevel <= AccessLevel.GameMaster && (r is GuardedRegion || r is PlayerCityRegion))
                        {
                            return true; // Return as soon as a matching region is found
                        }
                    }
                }
            }
            return false;
        }

        public static List<CityManagementStone> AllCityStones = new List<CityManagementStone>();

        public static bool CheckIfMayor(Mobile from)
        {
            foreach (CityManagementStone stone in AllCityStones)
            {
                if (stone.Mayor == from)
                    return true;
            }
            return false;
        }

        public static bool CheckIfCitizen(Mobile from)
        {
            foreach (CityManagementStone stone in AllCityStones)
            {
                if (stone.Citizens.Contains(from))
                    return true;
            }
            return false;
        }

        public static List<BaseHouse> GetAllHouses(Mobile m) // thanks to bripbrip
        {
            List<BaseHouse> allHouses = new List<BaseHouse>();

            Account a = m.Account as Account;

            if (a == null)
                return allHouses;

            for (int i = 0; i < a.Length; ++i)
            {
                Mobile mob = a[i];

                if (mob != null)
                    allHouses.AddRange(BaseHouse.GetHouses(mob));
            }
            return allHouses;
        }

        public static bool CheckIfSpecificHouseInCity(BaseHouse h, Region region)
        {
            if (h != null)
            {
                int X = h.Sign.X;
                int Y = h.Sign.Y + 1;
                int Z = h.Sign.Z;

                Point3D hsp = new Point3D(X, Y, Z);
                Map hsm = h.Sign.Map;

                Region reg = Region.Find(hsp, hsm);

                if (reg == region)
                    return true;
            }
            return false;
        }

        public static bool CheckIfHouseInCity(Mobile from, Region cityRegion)
        {
            if (from == null || cityRegion == null)
                return false; // Did not find the house within the city

            List<BaseHouse> houses = GetAllHousesIncludingOwned(from);
            foreach (BaseHouse house in houses)
            {
                if (house != null)
                {
                    // Check the house's sign point
                    if (CheckIfSpecificHouseInCity(house, cityRegion))
                        return true; // Found the house within the city

                    // Check multiple points around the house
                    foreach (Point3D point in GetHouseCheckPoints(house))
                    {
                        Region reg = Region.Find(point, house.Map);
                        if (reg == cityRegion)
                            return true; // Found the house within the city
                    }
                }
            }

            return false; // Did not find the house within the city
        }

        private static List<BaseHouse> GetAllHousesIncludingOwned(Mobile m)
        {
            List<BaseHouse> allHouses = BaseHouse.GetHouses(m);
            allHouses.AddRange(GetAllHouses(m));
            return allHouses;
        }

        private static List<Point3D> GetHouseCheckPoints(BaseHouse house)
        {
            List<Point3D> points = new List<Point3D>();

            Rectangle3D rect = house.Region.Area[0];
            Point3D start = rect.Start;
            Point3D end = rect.End;

            // Check corners and center of the house's region
            points.Add(start); // top-left-front
            points.Add(new Point3D(end.X, start.Y, start.Z)); // top-right-front
            points.Add(new Point3D(start.X, end.Y, start.Z)); // bottom-left-front
            points.Add(new Point3D(end.X, end.Y, start.Z)); // bottom-right-front

            points.Add(new Point3D((start.X + end.X) / 2, (start.Y + end.Y) / 2, start.Z)); // center-front

            return points;
        }

        public static bool CheckMapCityLimit(Mobile from)
        {
            List<Item> count = new List<Item>();
            foreach (Item item in World.Items.Values)
            {
                if (item is CityManagementStone && item.Map == from.Map)
                {
                    count.Add(item);
                }
            }

            if (from.Map == Map.Lodor)
            {
                if (count.Count >= MaxCitiesForLodor)
                    return true;
            }
            else if (from.Map == Map.Sosaria)
            {
                if (count.Count >= MaxCitiesForSosaria)
                    return true;
            }
            else if (from.Map == Map.Underworld)
            {
                if (count.Count >= MaxCitiesForUnderworld)
                    return true;
            }
            else if (from.Map == Map.SerpentIsland)
            {
                if (count.Count >= MaxCitiesForSerpentIsland)
                    return true;
            }
            else if (from.Map == Map.IslesDread)
            {
                if (count.Count >= MaxCitiesForIslesDread)
                    return true;
            }
            else
            {
                return true;
            }

            return false;
        }

        public static bool CheckCityName(string text)
        {
            foreach (Item item in World.Items.Values)
            {
                if (item is CityManagementStone)
                {
                    CityManagementStone stone = (CityManagementStone)item;

                    if (stone.CityName == text)
                        return true;
                }
            }

            return false;
        }

        public static bool CheckIfBanned(Mobile from, Mobile target)
        {
            // Check basic conditions
            if (!(from is PlayerMobile) || !(target is PlayerMobile))
                return false;

            if (!(from.Region is PlayerCityRegion) || !(target.Region is PlayerCityRegion))
                return false;

            // Get the city stone of the 'from' player
            PlayerMobile pm = (PlayerMobile)from;
            CityManagementStone stone = pm.City;

            if (stone == null)
                return false;

            if (stone.PCRegion != target.Region)
                return false;

            // Check if 'from' is a member and 'target' is banned
            bool isMember = stone.Citizens.Contains(from);
            bool isBanned = stone.Banned.Contains(target);

            return isMember && isBanned;
        }

        public static bool CheckBanLootable(Mobile from, Mobile target)
        {
            if (from is PlayerMobile && target is PlayerMobile)
            {
                if (from.Region is PlayerCityRegion)
                {
                    PlayerMobile pm = (PlayerMobile)from;
                    CityManagementStone stone = pm.City;

                    if (stone != null)
                    {
                        bool isBanned = false;

                        foreach (Mobile checkBan in stone.Banned)
                        {
                            if (target == checkBan)
                                isBanned = true;
                        }

                        if (isBanned == true)
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool CheckAtWarWith(Mobile from, Mobile target)
        {
            if (from is PlayerMobile && target is PlayerMobile)
            {
                PlayerMobile pm1 = (PlayerMobile)from;
                PlayerMobile pm2 = (PlayerMobile)target;
                CityManagementStone fromCity = pm1.City;
                CityManagementStone targCity = pm2.City;

                if (fromCity != null && targCity != null)
                {
                    if (fromCity.Waring.Contains(targCity))
                        return true;
                }
            }

            return false;
        }

        public static bool CheckCityAlly(Mobile from, Mobile target)
        {
            if (from is PlayerMobile && target is PlayerMobile)
            {
                PlayerMobile pm1 = (PlayerMobile)from;
                PlayerMobile pm2 = (PlayerMobile)target;
                CityManagementStone fromCity = pm1.City;
                CityManagementStone targCity = pm2.City;

                if (fromCity != null)
                {
                    if (fromCity.Citizens.Contains(target))
                        return true;

                    if (fromCity.Allegiances.Contains(targCity))
                        return true;
                }
            }

            return false;
        }

        public static bool IsCityLevelReached(Mobile from, int level)
        {
            PlayerMobile pm = (PlayerMobile)from;

            if (pm.City != null && pm.City.Mayor == from)
            {
                if (pm.City.Level >= level)
                    return true;
            }

            return false;
        }

        public static bool IsAtCity(Mobile from, Mobile to)
        {
            PlayerMobile pm = (PlayerMobile)from;
            CityManagementStone city = pm.City;
            Region cityreg = Region.Find(from.Location, from.Map);
            Region targregion = Region.Find(to.Location, to.Map);

            if (city != null && cityreg != null && targregion != null)
            {
                if ((cityreg == city.PCRegion) && (cityreg == targregion))
                    return true;
            }

            return false;
        }

        public static bool IsAtCity(Mobile from)
        {
            PlayerMobile pm = (PlayerMobile)from;
            CityManagementStone city = pm.City;
            Region cityreg = Region.Find(from.Location, from.Map);

            if (cityreg != null && city != null)
            {
                if (cityreg == city.PCRegion)
                    return true;
            }

            return false;
        }

        public static bool IsAtCity(Item item)
        {
            Region reg = Region.Find(item.Location, item.Map);

            if (reg != null)
            {
                if (reg is PlayerCityRegion)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public static bool IsMemberOf(Mobile from, CityManagementStone stone)
        {
            PlayerMobile pm = (PlayerMobile)from;
            if (pm.City == null)
                return false;

            return stone.Citizens.Contains(from);
        }

        public static Rectangle3D[] FormatRegion(Rectangle2D box)
        {
            Rectangle3D area = Server.Region.ConvertTo3D(box);
            List<Rectangle3D> arealist = new List<Rectangle3D>();
            arealist.Add(area);
            Rectangle3D[] regarea = arealist.ToArray();
            return regarea;
        }

        public static bool IsIteminCity(Mobile from, Item item)
        {
            PlayerMobile pm = (PlayerMobile)from;
            if (pm.City == null)
                return false;

            CityManagementStone stone = pm.City;
            Point3D p = new Point3D(item.X, item.Y, item.Z);
            Region target = Region.Find(p, item.Map);

            return stone.PCRegion != null && stone.PCRegion == target;
        }

        public static bool AreThereVendors(Mobile from)
        {
            if (from == null)
                return false;

            foreach (BaseHouse h in BaseHouse.GetHouses(from))
            {
                if (h != null && (h.HasPersonalVendors || h.HasRentedVendors))
                {
                    return true;
                }
            }
            return false;
        }

        public static void CheckCitySystemVersion()
        {
            bool SystemUpgrade = false;

            if (!File.Exists(FileName))
                SystemUpgrade = true;
            else
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(FileName);

                    XmlElement system = xml["Version"];
                    FileVersion = system.InnerText;

                    if (FileVersion == "0.0")
                    {
                        Console.WriteLine("Government System: Error Loading Version Variable!");
                        return;
                    }
                    if (FileVersion != SystemVersion)
                        SystemUpgrade = true;
                }
                catch
                {
                    Console.WriteLine("Error Reading City Version File!");
                    return;
                }
            }
            if (SystemUpgrade)
                Console.WriteLine(
                    "The city system needs to be upgraded, please run the [upgradecitysystem command!"
                );
            else
                Console.WriteLine("City system current.  Version {0}", SystemVersion);
        }
    }
}
