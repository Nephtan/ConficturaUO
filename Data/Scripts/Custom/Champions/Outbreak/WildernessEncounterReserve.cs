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
        // Compatibility API only. The infestation has no permanent reservations.
        private static readonly ReadOnlyCollection<WildernessEncounterReserve> m_Proposals =
            new List<WildernessEncounterReserve>().AsReadOnly();

        public static ReadOnlyCollection<WildernessEncounterReserve> Proposals { get { return m_Proposals; } }

        public static bool ContainsHousingTile(Map map, int x, int y) { return false; }
        public static bool Intersects(Map map, Rectangle2D bounds) { return false; }
    }
}
