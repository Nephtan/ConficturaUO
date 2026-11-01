using System;
using System.Collections.Generic;
using System.Diagnostics;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public interface ISpreadingEncounter
    {
        bool Scheduled { get; }
        bool OriginPending { get; }
        bool RespondsTo(Mobile witness);
        void WorkDeferred(DateTime now);
        void Step(DateTime now);
        void Admit(int territory, DateTime now);
    }

    public interface ISpreadingEncounterActor { ISpreadingEncounter Encounter { get; } }

    // Only background work spends these tokens. Existing interactions always run ordinary AI.
    public static class SpreadingEncounterScheduler
    {
        public const int ActorAdmissionTarget = 160, SceneryAdmissionTarget = 240;
        public const int TerrainPerSecond = 4096, RoutesPerSecond = 2, ActorsPerSecond = 6, SceneryPerSecond = 24;
        public const int ProtectionRadius = 56, QuietSeconds = 90;
        private sealed class Territory
        {
            public ISpreadingEncounter Owner;
            public int Id;
            public Point3D Center;
        }
        private sealed class Activity
        {
            public Map Map;
            public int X = -1, Y = -1;
            public List<IDisposable> Leases = new List<IDisposable>();
            public void Release() { foreach (IDisposable lease in Leases) lease.Dispose(); Leases.Clear(); X = Y = -1; }
        }
        internal sealed class Deferred : Exception { }
        private static readonly List<ISpreadingEncounter> m_Directors = new List<ISpreadingEncounter>();
        private static readonly Dictionary<Map, Dictionary<Point2D, Territory>> m_Territories = new Dictionary<Map, Dictionary<Point2D, Territory>>();
        private static readonly Dictionary<Mobile, Activity> m_Actors = new Dictionary<Mobile, Activity>();
        private static readonly Dictionary<Item, bool> m_Scenery = new Dictionary<Item, bool>();
        private static readonly List<ISpreadingEncounter>[] m_Waiters = new List<ISpreadingEncounter>[]
        { new List<ISpreadingEncounter>(),new List<ISpreadingEncounter>(),new List<ISpreadingEncounter>(),new List<ISpreadingEncounter>() };
        private static readonly Dictionary<ISpreadingEncounter,long[]> m_Grants = new Dictionary<ISpreadingEncounter,long[]>();
        private static ISpreadingEncounter m_Current;
        private static int m_Requested, m_Granted;
        private static Timer m_Timer;
        private static int m_Cursor, m_Terrain, m_Routes, m_Births, m_Parts, m_AdmissionDepth;
        private static DateTime m_Window;
        private static bool m_Working, m_Initialized;
        private static bool m_OriginTurn;
        private static Stopwatch m_Watch = new Stopwatch();
        public static int ActorCount { get { return m_Actors.Count; } }
        public static int SceneryCount { get { return m_Scenery.Count; } }
        internal static bool PriorityAdmission { get { return m_AdmissionDepth > 0; } }
        public static long Turns, DeferredTurns;

        public static void Initialize()
        {
            if (m_Initialized) return;
            m_Initialized = true;
            Sector.MobileEntering += OnEntering;
            EventSink.AggressiveAction += OnAggressiveAction;
        }

        public static void Register(ISpreadingEncounter director)
        {
            Initialize();
            if (!m_Directors.Contains(director)) { m_Directors.Add(director); m_Grants.Add(director,new long[4]); }
            if (m_Timer == null) m_Timer = Timer.DelayCall(TimeSpan.FromMilliseconds(250),TimeSpan.FromMilliseconds(250),OnTick);
        }

        public static void Unregister(ISpreadingEncounter director)
        {
            m_Directors.Remove(director);
            foreach (List<ISpreadingEncounter> waiters in m_Waiters) waiters.Remove(director);
            m_Grants.Remove(director);
            ReleaseTerritory(director);
            if (m_Directors.Count == 0 && m_Timer != null) { m_Timer.Stop(); m_Timer = null; }
        }

        internal static void ReleaseTerritory(ISpreadingEncounter director)
        {
            foreach (Dictionary<Point2D, Territory> cells in m_Territories.Values)
            {
                List<Point2D> remove = new List<Point2D>();
                foreach (KeyValuePair<Point2D,Territory> cell in cells) if (cell.Value.Owner == director) remove.Add(cell.Key);
                foreach (Point2D key in remove) cells.Remove(key);
            }
        }

        public static bool CanReserve(ISpreadingEncounter owner, Map map, Point3D center)
        {
            Dictionary<Point2D,Territory> cells;
            if (!m_Territories.TryGetValue(map,out cells)) return true;
            int x = center.X/12, y = center.Y/12;
            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                {
                    Territory cell;
                    if (cells.TryGetValue(new Point2D(x+dx,y+dy),out cell)
                        && (dx == 0 && dy == 0 || Math.Abs(cell.Center.X-center.X)<10 && Math.Abs(cell.Center.Y-center.Y)<10)) return false;
                }
            return true;
        }

        public static bool Reserve(ISpreadingEncounter owner, Map map, int id, Point3D center)
        {
            if (!CanReserve(owner,map,center)) return false;
            Dictionary<Point2D,Territory> cells;
            if (!m_Territories.TryGetValue(map,out cells)) { cells = new Dictionary<Point2D,Territory>(); m_Territories.Add(map,cells); }
            Territory cell = new Territory(); cell.Owner = owner; cell.Id = id; cell.Center = center;
            cells.Add(new Point2D(center.X/12,center.Y/12),cell);
            return true;
        }

        private static void ResetWindow(DateTime now)
        {
            if (now < m_Window || now-m_Window >= TimeSpan.FromSeconds(1))
            { m_Window = now; m_Terrain = m_Routes = m_Births = m_Parts = 0; }
        }

        // Visits can alias the one-second quota window. Preserve denied demand independently
        // of visit position, and release unused demand after its director's next turn.
        private static bool Permit(int resource, bool available, bool bootstrap)
        {
            int bit = 1 << resource; m_Requested |= bit;
            List<ISpreadingEncounter> waiters = m_Waiters[resource];
            while (waiters.Count > 0 && (!m_Directors.Contains(waiters[0]) || !waiters[0].Scheduled)) waiters.RemoveAt(0);
            bool turn = bootstrap || (m_Granted & bit) != 0 || waiters.Count == 0 || waiters[0] == m_Current;
            if (!turn || !available)
            { if (!waiters.Contains(m_Current)) waiters.Add(m_Current); return false; }
            if ((m_Granted & bit) == 0) { waiters.Remove(m_Current); m_Granted |= bit; }
            long[] grants = m_Grants[m_Current]; if (grants[resource] < Int64.MaxValue) ++grants[resource];
            return true;
        }

        public static long WorkGrants(ISpreadingEncounter director, int resource)
        {
            long[] grants;
            return resource >= 0 && resource < 4 && m_Grants.TryGetValue(director,out grants) ? grants[resource] : 0;
        }

        internal static void Terrain()
        {
            if (!m_Working || m_AdmissionDepth > 0) return;
            if (!Permit(0,m_Terrain < TerrainPerSecond && m_Watch.Elapsed.TotalMilliseconds < 5,false)) throw new Deferred();
            ++m_Terrain;
        }

        internal static bool Route()
        {
            if (!m_Working || m_AdmissionDepth > 0) return true;
            if (!Permit(1,m_Routes < RoutesPerSecond,false)) return false;
            ++m_Routes; return true;
        }

        internal static bool AdmitActor(bool owed)
        {
            if (owed && m_AdmissionDepth > 0) return true;
            bool room = ActorCount < ActorAdmissionTarget;
            if (m_Working && !m_OriginTurn)
            {
                int origins = 0;
                foreach (ISpreadingEncounter director in m_Directors) if (director.Scheduled && director.OriginPending) ++origins;
                // Leave a queen and guardian's admission room for directors still seeking an origin.
                if (ActorCount >= ActorAdmissionTarget-origins*2) room = false;
            }
            if (!m_Working) return room;
            // Capacity-denied background demand must not hold the shared birth queue
            // ahead of structural growth, whose admission deliberately ignores that target.
            if (!room) return false;
            if (!Permit(2,room && m_Births < ActorsPerSecond,m_OriginTurn)) return false;
            ++m_Births;
            return true;
        }

        internal static bool AdmitStructuralActor()
        {
            if (!m_Working || m_AdmissionDepth > 0) return true;
            if (!Permit(2,m_Births < ActorsPerSecond,m_OriginTurn)) return false;
            ++m_Births;
            return true;
        }

        internal static bool AdmitScenery(int parts)
        {
            if (m_AdmissionDepth > 0) return true;
            bool room = SceneryCount+parts <= SceneryAdmissionTarget;
            if (!m_Working) return room;
            if (!Permit(3,room && m_Parts+parts <= SceneryPerSecond,false)) return false;
            if (m_Working) m_Parts += parts;
            return true;
        }

        private static void OnTick() { Service(DateTime.UtcNow); }
        internal static void Service(DateTime now)
        {
            ResetWindow(now);
            if (m_Directors.Count == 0) return;
            m_Cursor = Math.Max(0,m_Cursor) % m_Directors.Count;
            ISpreadingEncounter director = m_Directors[m_Cursor]; m_Cursor = (m_Cursor+1)%m_Directors.Count;
            if (!director.Scheduled) { foreach (List<ISpreadingEncounter> waiters in m_Waiters) waiters.Remove(director); return; }
            m_Current = director; m_Requested = m_Granted = 0;
            m_Working = true; m_OriginTurn = director.OriginPending; m_Watch.Restart();
            try { ++Turns; director.Step(now); }
            catch (Deferred) { ++DeferredTurns; director.WorkDeferred(now); }
            finally
            {
                for (int resource = 0; resource < 4; ++resource) if ((m_Requested & (1 << resource)) == 0) m_Waiters[resource].Remove(director);
                m_Current = null; m_Working = false; m_OriginTurn = false; m_Watch.Stop();
            }
        }

        private static void OnEntering(Sector sector, Mobile actor)
        {
            if (m_AdmissionDepth > 0 || actor == null || actor.Deleted || actor.Map == null || actor.Map == Map.Internal
                || actor is ISpreadingEncounterActor || !(actor.Player || actor.Alive && actor is BaseCreature)) return;
            AdmitArea(actor.Map,actor.Location,actor.Player || PlayerAlly(actor) ? ProtectionRadius : 32,actor);
        }

        private static void OnAggressiveAction(AggressiveActionEventArgs e)
        {
            ISpreadingEncounterActor owned = e.Aggressed as ISpreadingEncounterActor;
            if (owned == null) owned = e.Aggressor as ISpreadingEncounterActor;
            if (owned == null || owned.Encounter == null || !owned.Encounter.Scheduled) return;
            Mobile actor = e.Aggressed is ISpreadingEncounterActor ? e.Aggressed : e.Aggressor;
            AdmitArea(actor.Map,actor.Location,ProtectionRadius);
        }

        internal static void AdmitArea(Map map, Point3D center, int radius)
        { AdmitArea(map,center,radius,null); }

        private static void AdmitArea(Map map, Point3D center, int radius, Mobile witness)
        {
            Dictionary<Point2D,Territory> cells;
            if (map == null || !m_Territories.TryGetValue(map,out cells)) return;
            List<Territory> nearby = new List<Territory>();
            for (int x = Math.Max(0,(center.X-radius-12)/12); x <= (center.X+radius+12)/12; ++x)
                for (int y = Math.Max(0,(center.Y-radius-12)/12); y <= (center.Y+radius+12)/12; ++y)
                {
                    Territory cell;
                    if (cells.TryGetValue(new Point2D(x,y),out cell)) nearby.Add(cell);
                }
            ++m_AdmissionDepth;
            try { foreach (Territory cell in nearby) if (witness == null || cell.Owner.RespondsTo(witness)) cell.Owner.Admit(cell.Id,DateTime.UtcNow); }
            finally { --m_AdmissionDepth; }
        }

        private static bool PlayerAlly(Mobile actor)
        {
            BaseCreature creature = actor as BaseCreature;
            return creature != null && (creature.Controlled || creature.Summoned || creature.ControlMaster != null || creature.SummonMaster != null);
        }

        internal static bool Interested(Mobile actor)
        {
            if (actor == null || actor.Deleted || actor.Map == null || actor.Map == Map.Internal) return false;
            ISpreadingEncounterActor owned = actor as ISpreadingEncounterActor;
            IPooledEnumerable nearby = actor.GetMobilesInRange(ProtectionRadius);
            try
            {
                foreach (Mobile other in nearby)
                    if (other != actor && !other.Deleted && (other.Player || other.Alive && PlayerAlly(other)
                        || other.Alive && other is BaseCreature && !(other is ISpreadingEncounterActor)
                            && (owned == null || owned.Encounter == null || owned.Encounter.RespondsTo(other)))) return true;
            }
            finally { nearby.Free(); }
            return false;
        }

        internal static bool Busy(BaseCreature actor)
        {
            if (actor.Controlled || actor.Summoned || actor.Poisoned || actor.Paralyzed || actor.Frozen || actor.Spell != null
                || actor.BardPacified || actor.BardProvoked || actor.StatMods.Count > 0 || actor.SkillMods.Count > 0
                || actor.Combatant != null && !actor.Combatant.Deleted && actor.Combatant.Alive
                || actor.FocusMob != null && !actor.FocusMob.Deleted && actor.FocusMob.Alive) return true;
            foreach (AggressorInfo aggression in actor.Aggressors) if (!aggression.Expired) return true;
            foreach (AggressorInfo aggression in actor.Aggressed) if (!aggression.Expired) return true;
            foreach (DamageEntry damage in actor.DamageEntries) if (!damage.HasExpired) return true;
            return false;
        }

        internal static void Track(Mobile actor)
        {
            if (actor == null || actor.Deleted || actor.Map == null || actor.Map == Map.Internal) return;
            Activity activity;
            if (!m_Actors.TryGetValue(actor,out activity)) { activity = new Activity(); m_Actors.Add(actor,activity); }
            int x = actor.X >> Map.SectorShift, y = actor.Y >> Map.SectorShift;
            if (activity.Map == actor.Map && activity.X == x && activity.Y == y) return;
            List<IDisposable> next = new List<IDisposable>();
            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                    if ((x+dx)*16 >= 0 && (y+dy)*16 >= 0 && (x+dx)*16 < actor.Map.Width && (y+dy)*16 < actor.Map.Height)
                        next.Add(actor.Map.GetSector((x+dx)*16,(y+dy)*16).AcquireActivityLease());
            activity.Release(); activity.Map = actor.Map; activity.X = x; activity.Y = y; activity.Leases = next;
        }

        internal static void Untrack(Mobile actor)
        {
            Activity activity;
            if (actor != null && m_Actors.TryGetValue(actor,out activity)) { m_Actors.Remove(actor); activity.Release(); }
        }
        internal static void Track(Item scenery) { if (scenery != null && !m_Scenery.ContainsKey(scenery)) m_Scenery.Add(scenery,true); }
        internal static void Untrack(Item scenery) { if (scenery != null) m_Scenery.Remove(scenery); }
    }
}
