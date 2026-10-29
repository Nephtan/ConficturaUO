using System;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;
using Server.Regions;
using Server.Engines.CannedEvil;

namespace Server.Custom.Confictura.Champions
{
    public interface IArachnidOutbreakTerrain
    {
        bool TryGetPoint(int x, int y, out Point3D point);
        bool HasPermanentHousingProhibition(Rectangle2D bounds);
        bool IsReserved(Rectangle2D bounds);
    }

    public sealed class ArachnidOutbreakSite
    {
        public const int Radius = 56;
        private readonly string m_Id;
        private readonly Point2D m_Center;
        private readonly bool m_Reviewed;

        public string Id { get { return m_Id; } }
        public Point2D Center { get { return m_Center; } }
        public bool Reviewed { get { return m_Reviewed; } }
        public Rectangle2D Bounds
        {
            get { return new Rectangle2D(m_Center.X - Radius, m_Center.Y - Radius, Radius * 2 + 1, Radius * 2 + 1); }
        }
        public Rectangle2D ClearanceBounds
        {
            get { return new Rectangle2D(m_Center.X - 80, m_Center.Y - 80, 161, 161); }
        }

        public ArachnidOutbreakSite(string id, int x, int y, bool reviewed)
        {
            m_Id = id;
            m_Center = new Point2D(x, y);
            m_Reviewed = reviewed;
        }

        public bool Validate(IArachnidOutbreakTerrain terrain, out ArachnidOutbreakPlacement placement, out string reason)
        {
            placement = null;
            reason = "Outside mainland Sosaria clearance bounds.";
            if (m_Center.X < 81 || m_Center.Y < 81 || m_Center.X > 5038 || m_Center.Y > 3046)
                return false;

            Rectangle2D clearance = ClearanceBounds;
            reason = "No permanent housing prohibition across the footprint and buffer.";
            if (!terrain.HasPermanentHousingProhibition(clearance))
                return false;
            reason = "Reserved region, housing, service, portal or active encounter.";
            if (terrain.IsReserved(clearance))
                return false;

            Point3D center;
            reason = "Boss anchor is not spawnable.";
            if (!terrain.TryGetPoint(m_Center.X, m_Center.Y, out center))
                return false;

            Dictionary<Point2D, Point3D> reached = new Dictionary<Point2D, Point3D>();
            Queue<Point3D> queue = new Queue<Point3D>();
            reached.Add(m_Center, center);
            queue.Enqueue(center);
            int[] dx = new int[] { -1, 1, 0, 0 };
            int[] dy = new int[] { 0, 0, -1, 1 };
            bool north = false, south = false, east = false, west = false;
            while (queue.Count != 0)
            {
                Point3D current = queue.Dequeue();
                north |= current.Y == m_Center.Y - Radius;
                south |= current.Y == m_Center.Y + Radius;
                west |= current.X == m_Center.X - Radius;
                east |= current.X == m_Center.X + Radius;
                for (int i = 0; i < 4; ++i)
                {
                    Point2D key = new Point2D(current.X + dx[i], current.Y + dy[i]);
                    Point3D next;
                    if (!Bounds.Contains(key) || reached.ContainsKey(key)
                        || !terrain.TryGetPoint(key.X, key.Y, out next)
                        || Math.Abs(next.Z - current.Z) > 2)
                        continue;
                    reached.Add(key, next);
                    queue.Enqueue(next);
                }
            }

            reason = "Insufficient connected combat space or opposite exits.";
            if (reached.Count < 4000 || !((north && south) || (east && west)))
                return false;

            Point2D[] anchors = new Point2D[]
            {
                new Point2D(m_Center.X - 28, m_Center.Y - 20),
                new Point2D(m_Center.X + 28, m_Center.Y - 20),
                new Point2D(m_Center.X, m_Center.Y + 30)
            };
            Point3D[] broods = new Point3D[3];
            for (int stage = 0; stage < anchors.Length; ++stage)
            {
                reason = "Brood anchor lacks connected staging space.";
                if (!reached.TryGetValue(anchors[stage], out broods[stage]))
                    return false;
                for (int x = -2; x <= 2; ++x)
                    for (int y = -2; y <= 2; ++y)
                        if (!reached.ContainsKey(new Point2D(anchors[stage].X + x, anchors[stage].Y + y)))
                            return false;
            }

            Point3D[] points = new Point3D[reached.Count];
            reached.Values.CopyTo(points, 0);
            placement = new ArachnidOutbreakPlacement(center, broods, points);
            reason = String.Empty;
            return true;
        }
    }

    public sealed class ArachnidOutbreakPlacement
    {
        private readonly Point3D m_Boss;
        private readonly Point3D[] m_Broods;
        private readonly Point3D[] m_Walkable;
        public Point3D Boss { get { return m_Boss; } }
        public Point3D Brood(int stage) { return m_Broods[stage]; }
        public int WalkableCount { get { return m_Walkable.Length; } }

        public ArachnidOutbreakPlacement(Point3D boss, Point3D[] broods, Point3D[] walkable)
        {
            m_Boss = boss;
            m_Broods = (Point3D[])broods.Clone();
            m_Walkable = (Point3D[])walkable.Clone();
        }

        public bool TryFindSpawn(Point3D anchor, int range, out Point3D point)
        {
            // Randomize among prevalidated tiles; there is no unsafe fallback location.
            int offset = Utility.Random(m_Walkable.Length);
            for (int i = 0; i < m_Walkable.Length; ++i)
            {
                Point3D candidate = m_Walkable[(i + offset) % m_Walkable.Length];
                if (Math.Abs(candidate.X - anchor.X) <= range && Math.Abs(candidate.Y - anchor.Y) <= range
                    && Map.Sosaria.CanSpawnMobile(candidate) && ClearForSpawn(candidate))
                {
                    point = candidate;
                    return true;
                }
            }
            point = Point3D.Zero;
            return false;
        }

        private static bool ClearForSpawn(Point3D point)
        {
            if (BaseHouse.FindHouseAt(point, Map.Sosaria, 16) != null)
                return false;
            IPooledEnumerable items = Map.Sosaria.GetItemsInRange(point, 2);
            try
            {
                foreach (Item item in items)
                    if (SosariaOutbreakTerrain.IsPortal(item))
                        return false;
            }
            finally { items.Free(); }
            return true;
        }
    }

    public sealed class SosariaOutbreakTerrain : IArachnidOutbreakTerrain
    {
        public static bool IsPortal(Item item)
        {
            return item is Teleporter || item is Moongate || item is PublicMoongate
                || item is DoorTeleporter || item is HouseTeleporter
                || item is PlayersHouseTeleporter || item is PlayersZTeleporter;
        }
        public bool HasPermanentHousingProhibition(Rectangle2D bounds)
        {
            for (int x = bounds.X; x < bounds.X + bounds.Width; ++x)
                for (int y = bounds.Y; y < bounds.Y + bounds.Height; ++y)
                {
                    Point3D point = new Point3D(x, y, Map.Sosaria.GetAverageZ(x, y));
                    if (WildernessEncounterReserves.ContainsHousingTile(Map.Sosaria, x, y))
                        continue;
                    Region region = Region.Find(point, Map.Sosaria);
                    Region noHousing = region.GetRegion(typeof(NoHousingRegion));
                    // HousePlacement rejects a center inside NoHousingRegion outright.
                    // OutDoorBadRegion unconditionally denies housing. Conditional,
                    // inherited, temporary and player-created rules are not sufficient.
                    if (noHousing != null && !noHousing.Dynamic)
                        continue;
                    if (region.GetType() == typeof(OutDoorBadRegion) && !region.Dynamic)
                        continue;
                    return false;
                }
            return true;
        }

        public bool TryGetPoint(int x, int y, out Point3D point)
        {
            Map map = Map.Sosaria;
            point = new Point3D(x, y, map.GetAverageZ(x, y));
            TileFlag flags = TileData.LandTable[map.Tiles.GetLandTile(x, y).ID & 0x3FFF].Flags;
            // Connectivity survives a transient mobile standing on an objective. Actual
            // placement still uses CanSpawnMobile, including its live occupancy check.
            return (flags & (TileFlag.Wet | TileFlag.Impassable)) == 0
                && Region.Find(point, map).AllowSpawn() && map.CanFit(point, 16, false, false);
        }

        public bool IsReserved(Rectangle2D bounds)
        {
            return IsReserved(bounds, true);
        }

        public bool HasStructuralConflict(Rectangle2D bounds)
        {
            return IsReserved(bounds, false);
        }

        private static bool IsTravelSpellGate(Item item)
        {
            Type spell = item.GetType().DeclaringType;
            return item is Moongate && (spell == typeof(Server.Spells.Seventh.GateTravelSpell)
                || spell == typeof(Server.Spells.Elementalism.Elemental_Gate_Spell)
                || spell == typeof(Server.Spells.Herbalist.MushroomGatewaySpell)
                || spell == typeof(Server.Spells.Undead.UndeadGraveyardGatewaySpell));
        }

        private bool IsReserved(Rectangle2D bounds, bool includeTravelSpellGates)
        {
            Map map = Map.Sosaria;
            for (int x = bounds.X; x < bounds.X + bounds.Width; ++x)
                for (int y = bounds.Y; y < bounds.Y + bounds.Height; ++y)
                {
                    Point3D point = new Point3D(x, y, map.GetAverageZ(x, y));
                    Region region = Region.Find(point, map);
                    if (!region.AllowSpawn() || region.IsPartOf(typeof(GuardedRegion))
                        || region.IsPartOf(typeof(DungeonRegion)) || region.IsPartOf(typeof(HouseRegion))
                        || region.IsPartOf(typeof(VillageRegion)) || region.IsPartOf(typeof(PublicRegion))
                        || region.IsPartOf(typeof(ProtectedRegion))
                        || region.IsPartOf(typeof(SafeRegion)) || region.IsPartOf(typeof(ChampionSpawnRegion))
                        || BaseHouse.FindHouseAt(point, map, 16) != null)
                        return true;
                }

            IPooledEnumerable items = map.GetItemsInBounds(bounds);
            try
            {
                foreach (Item item in items)
                    // Player travel must not cancel a running encounter. These
                    // spell-owned gates expire and delete themselves on reload.
                    if (IsPortal(item) && (includeTravelSpellGates || !IsTravelSpellGate(item)))
                        return true;
            }
            finally { items.Free(); }

            IPooledEnumerable mobiles = map.GetMobilesInBounds(bounds);
            try
            {
                foreach (Mobile mobile in mobiles)
                    if (mobile is Server.Mobiles.BaseVendor)
                        return true;
            }
            finally { mobiles.Free(); }
            return false;
        }
    }
}
