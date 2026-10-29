using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Server.Custom.Confictura.Champions
{
    public sealed class WildernessEncounterReserve
    {
        private readonly string m_Id;
        private readonly Point2D m_Center;
        private readonly bool m_Reviewed;
        public string Id { get { return m_Id; } }
        public Point2D Center { get { return m_Center; } }
        public bool Reviewed { get { return m_Reviewed; } }
        public Rectangle2D Bounds
        {
            get { return new Rectangle2D(m_Center.X - 80, m_Center.Y - 80, 161, 161); }
        }
        public WildernessEncounterReserve(string id, int x, int y, bool reviewed)
        {
            m_Id = id;
            m_Center = new Point2D(x, y);
            m_Reviewed = reviewed;
        }
    }

    public static class WildernessEncounterReserves
    {
        // A reviewed reserve is permanent world geography. Encounter stop,
        // cooldown and delivery flags must never reopen it for player housing.
        // These proposals stay inactive until copied-world/native review.
        private static readonly ReadOnlyCollection<WildernessEncounterReserve> m_Proposals =
            new List<WildernessEncounterReserve>(new WildernessEncounterReserve[]
            {
                new WildernessEncounterReserve("central-woodland", 2514, 2088, false),
                new WildernessEncounterReserve("northern-woodland", 2242, 652, false),
                new WildernessEncounterReserve("eastern-woodland", 3746, 876, false),
                new WildernessEncounterReserve("northwestern-woodland", 1208, 928, false),
                new WildernessEncounterReserve("southeastern-woodland", 3762, 2744, false),
                new WildernessEncounterReserve("western-woodland", 1122, 1704, false)
            }).AsReadOnly();

        public static ReadOnlyCollection<WildernessEncounterReserve> Proposals { get { return m_Proposals; } }

        public static bool ContainsHousingTile(Map map, int x, int y)
        {
            if (map != Map.Sosaria)
                return false;
            Point2D point = new Point2D(x, y);
            foreach (WildernessEncounterReserve reserve in m_Proposals)
                if (reserve.Reviewed && reserve.Bounds.Contains(point))
                    return true;
            return false;
        }

        public static bool Intersects(Map map, Rectangle2D bounds)
        {
            if (map != Map.Sosaria || bounds.Width <= 0 || bounds.Height <= 0)
                return false;
            foreach (WildernessEncounterReserve reserve in m_Proposals)
            {
                Rectangle2D other = reserve.Bounds;
                if (reserve.Reviewed && bounds.X < other.X + other.Width && bounds.X + bounds.Width > other.X
                    && bounds.Y < other.Y + other.Height && bounds.Y + bounds.Height > other.Y)
                    return true;
            }
            return false;
        }
    }
}
