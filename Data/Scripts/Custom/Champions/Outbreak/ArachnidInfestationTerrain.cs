using System;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;
using Server.Regions;
using Server.Engines.CannedEvil;

namespace Server.Custom.Confictura.Champions
{
    public static class ArachnidInfestationTerrain
    {
        private static bool m_Budgeted;
        private static int m_Probes, m_Limit;
        internal static void BeginWork(int limit) { m_Budgeted = true; m_Probes = 0; m_Limit = limit; }
        internal static int EndWork() { m_Budgeted = false; return m_Probes; }
        public static bool TryPoint(int x, int y, out Point3D point)
        {
            return TryPoint(x,y,out point,true);
        }

        public static bool TryPoint(int x, int y, out Point3D point, bool placement)
        {
            point = Point3D.Zero;
            if (m_Budgeted && m_Probes++ >= m_Limit) return false;
            Map map = Map.Sosaria;
            if (x < 16 || x >= 5104 || y < 16 || y >= 3112) return false;
            point = new Point3D(x, y, map.GetAverageZ(x, y));
            Region region = Region.Find(point, map);
            if (region.IsPartOf(typeof(HouseRegion)) || region.IsPartOf(typeof(DungeonRegion))
                || region.IsPartOf(typeof(SafeRegion)) || region.IsPartOf(typeof(ProtectedRegion))
                || region.IsPartOf(typeof(PublicRegion)) || region.IsPartOf(typeof(StartRegion))
                || region.IsPartOf(typeof(ChampionSpawnRegion))
                || BaseHouse.FindHouseAt(point, map, 16) != null || BaseBoat.FindBoatAt(point, map) != null)
                return false;
            TileFlag flags = TileData.LandTable[map.Tiles.GetLandTile(x, y).ID & 0x3FFF].Flags;
            if ((flags & (TileFlag.Wet | TileFlag.Impassable)) != 0 || !map.CanFit(point, 16, false, false)) return false;
            foreach (StaticTile tile in map.Tiles.GetStaticTiles(x, y, true))
                if ((TileData.ItemTable[tile.ID & 0x3FFF].Flags & TileFlag.Roof) != 0) return false;
            IPooledEnumerable items = map.GetItemsInRange(point, 3);
            try
            {
                foreach (Item item in items)
                    if ((SosariaOutbreakTerrain.IsPortal(item) && (placement || !SosariaOutbreakTerrain.IsTravelSpellGate(item)))
                        || (item is BaseDoor && Math.Abs(item.X-x) <= 1 && Math.Abs(item.Y-y) <= 1))
                        return false;
            }
            finally { items.Free(); }
            return true;
        }

        public static bool Connected(Mobile mover, Point3D from, Point3D to)
        {
            if (mover == null || Math.Max(Math.Abs(from.X-to.X),Math.Abs(from.Y-to.Y)) > 12) return false;
            Dictionary<Point2D, Point3D> seen = new Dictionary<Point2D, Point3D>();
            Queue<Point3D> queue = new Queue<Point3D>(); queue.Enqueue(from); seen.Add(new Point2D(from.X,from.Y),from);
            Direction[] directions = new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };
            int[] dx = new int[] { 0,1,0,-1 }, dy = new int[] { -1,0,1,0 };
            while (queue.Count > 0 && seen.Count <= 256)
            {
                Point3D current = queue.Dequeue();
                if (current.X == to.X && current.Y == to.Y && Math.Abs(current.Z-to.Z) <= 2) return true;
                for (int i = 0; i < 4; ++i)
                {
                    int x = current.X+dx[i], y = current.Y+dy[i]; Point2D key = new Point2D(x,y); Point3D next; int z;
                    if (seen.ContainsKey(key) || x < Math.Min(from.X,to.X)-3 || x > Math.Max(from.X,to.X)+3
                        || y < Math.Min(from.Y,to.Y)-3 || y > Math.Max(from.Y,to.Y)+3
                        || !TryPoint(x,y,out next) || !Server.Movement.Movement.CheckMovement(mover,Map.Sosaria,current,directions[i],out z)
                        || Math.Abs(z-next.Z)>2) continue;
                    seen.Add(key,next); queue.Enqueue(next);
                }
            }
            return false;
        }

        public static ArachnidOutbreakPlacement LootPlacement(Point3D center)
        {
            List<Point3D> points = new List<Point3D>();
            for (int x = center.X-12; x <= center.X+12; ++x)
                for (int y = center.Y-12; y <= center.Y+12; ++y)
                {
                    Point3D point; if (TryPoint(x,y,out point)) points.Add(point);
                }
            return new ArachnidOutbreakPlacement(center,new Point3D[] { center,center,center },points.ToArray());
        }
    }
}
