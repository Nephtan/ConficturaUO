using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public partial class ArachnidOutbreakController
    {
        // Transient policy/path state is rebuilt from saved brood identities after reload.
        private QueenBroodMassPlanner m_QueenPlan;
        private ArachnidInfestationState m_QueenColony;
        private int m_QueenScan, m_QueenScanLimit, m_QueenGoal = -1;
        private DateTime m_NextQueenPlan, m_NextQueenWake, m_QueenProgressTime;
        private Point3D m_QueenProgressPosition;
        private PathFollower m_QueenPath;
        private List<Point3D> m_QueenRoute; private int m_QueenRouteIndex;private Point3D m_QueenWaypoint;
        private ArachnidOutbreakChampion m_ShelteredQueen;
        private readonly Dictionary<int,DateTime> m_QueenAvoid = new Dictionary<int,DateTime>();
        public int QueenShelterPatch { get { return m_QueenGoal; } }
        public int QueenShelterMass { get; private set; }
        public int QueenShelterLocalMass { get; private set; }
        public int QueenShelterSelections { get; private set; }
        public int QueenShelterMoves { get; private set; }
        public int QueenShelterBlocked { get; private set; }
        public int QueenShelterArrivals { get; private set; }
        public int QueenBroodYieldMoves { get; private set; }
        public Point3D QueenShelterWaypoint { get { return m_QueenWaypoint; } }
        public int QueenShelterRouteIndex { get { return m_QueenRouteIndex; } }
        public int QueenShelterRouteLength { get { return m_QueenRoute==null?0:m_QueenRoute.Count; } }
        private bool m_QueenAtGoal;

        private static int LivingBroodWeight(ArachnidInfestationPatch patch)
        {
            // Territorial extent, not the queen's own escort density, determines her refuge.
            return patch.Established && !patch.Retired ? 1 : 0;
        }
        private bool AvoidQueenGoal(int id)
        { DateTime until; return m_QueenAvoid.TryGetValue(id,out until) && DateTime.UtcNow < until; }

        internal void ServiceQueenShelter(ArachnidOutbreakChampion queen, DateTime now)
        {
            if (queen == null || queen.Deleted || !queen.Alive || queen.Map != Map.Sosaria || m_Infestation == null) return;
            if(m_QueenColony!=m_Infestation)
            {
                m_QueenColony=m_Infestation;m_QueenPlan=null;m_QueenGoal=-1;m_QueenPath=null;m_QueenRoute=null;
                m_QueenAvoid.Clear();m_NextQueenPlan=DateTime.MinValue;m_NextQueenWake=DateTime.MinValue;m_QueenAtGoal=false;
            }
            m_ShelteredQueen = queen; SpreadingEncounterScheduler.Track(queen);
            if (now >= m_NextQueenWake)
            {
                m_NextQueenWake = now.AddSeconds(2);
                SpreadingEncounterScheduler.AdmitArea(queen.Map,queen.Location,18);
            }
            ArachnidInfestationPatch current = m_Infestation.Find(m_QueenGoal);
            if (current != null && (current.Retired || !current.Established || LivingBroodWeight(current) == 0))
            { m_QueenGoal = -1; m_QueenPath = null; m_NextQueenPlan = now; }
            if (m_QueenPlan == null)
            {
                if (now < m_NextQueenPlan) return;
                m_QueenPlan = new QueenBroodMassPlanner(queen.Location,m_QueenGoal); m_QueenPlan.Excluded = AvoidQueenGoal;
                m_QueenScan = 0; m_QueenScanLimit = m_Infestation.Patches.Count;
            }
            if (m_QueenScan < m_QueenScanLimit)
            {
                int end = Math.Min(m_QueenScanLimit,m_QueenScan+128);
                for (; m_QueenScan < end; ++m_QueenScan)
                {
                    ArachnidInfestationPatch patch = m_Infestation.Patches[m_QueenScan];
                    if (patch.Established && !patch.Retired) m_QueenPlan.Add(patch.Id,patch.Center,LivingBroodWeight(patch));
                }
                return;
            }
            m_QueenPlan.Advance(128);
            if (!m_QueenPlan.Complete) return;
            QueenBroodMassPlanner plan = m_QueenPlan; m_QueenPlan = null; m_NextQueenPlan = now.AddSeconds(30);
            if (plan.GoalId < 0) return;
            bool improve = m_QueenGoal < 0 || !plan.CurrentInLargest || plan.LocalMass > plan.CurrentLocalMass
                || plan.CurrentDistance-plan.GoalDistance >= 12;
            bool travelling = m_QueenGoal>=0 && current != null && !current.Retired && !queen.InRange(current.Center,5);
            if (travelling && plan.CurrentInLargest && plan.LocalMass <= plan.CurrentLocalMass*1.25) improve = false;
            if (improve && plan.GoalId != m_QueenGoal)
            {
                m_QueenGoal = plan.GoalId; m_QueenAtGoal=false; m_QueenPath = null; m_QueenRoute=plan.Route; m_QueenRouteIndex=0; m_QueenProgressPosition = queen.Location;
                m_QueenProgressTime = now; ++QueenShelterSelections;
            }
            QueenShelterMass = plan.Mass; QueenShelterLocalMass = plan.LocalMass;
        }

        internal bool QueenProtectsPatch(ArachnidInfestationPatch patch)
        { return m_ShelteredQueen != null && !m_ShelteredQueen.Deleted && m_ShelteredQueen.Alive && m_ShelteredQueen.Map == Map.Sosaria && m_ShelteredQueen.InRange(patch.Center,24); }

        private void MakeRoomForQueen(ArachnidOutbreakChampion queen)
        {
            List<ArachnidInfestationCreature> nearby=new List<ArachnidInfestationCreature>();
            IPooledEnumerable seen=queen.GetMobilesInRange(2);
            try { foreach(Mobile mobile in seen)
            {
                ArachnidInfestationCreature brood=mobile as ArachnidInfestationCreature;
                if(brood!=null && brood.Controller==this && brood.Alive && !brood.Deleted && !brood.Founder) nearby.Add(brood);
            }} finally { seen.Free(); }
            foreach(ArachnidInfestationCreature brood in nearby)
            {
                if(brood.Combatant!=null || brood.Paralyzed || brood.Frozen || brood.Spell!=null || brood.BardPacified || brood.BardProvoked || brood.AIObject==null)continue;
                TurnParticipant actor=ArachnidOutbreakWebItem.Actor(brood);
                if(actor!=null && (actor.Group.Current!=actor || actor.Pending!=null))continue;
                Point3D before=brood.Location;brood.AIObject.WalkMobileRange(queen,1,false,3,4);
                if(brood.Location!=before)++QueenBroodYieldMoves;
            }
        }

        internal bool MoveQueenToShelter(ArachnidOutbreakChampion queen)
        {
            if (m_Infestation == null || m_QueenGoal < 0 || queen.Deleted || !queen.Alive || queen.Map != Map.Sosaria) return false;
            ArachnidInfestationPatch goal = m_Infestation.Find(m_QueenGoal);
            if (goal == null || goal.Retired || !goal.Established) return false;
            queen.Home = goal.Center; queen.RangeHome = 2;
            if (queen.InRange(goal.Center,5)) { if(!m_QueenAtGoal){++QueenShelterArrivals;m_QueenAtGoal=true;} m_QueenProgressTime = DateTime.UtcNow; return false; }
            if (queen.Paralyzed || queen.Frozen || queen.BardPacified || queen.BardProvoked || queen.Spell != null)
            { m_QueenProgressTime = DateTime.UtcNow; return true; }
            TurnParticipant actor = ArachnidOutbreakWebItem.Actor(queen);
            if (actor != null && (actor.Group.Current != actor || actor.Pending != null)) return true;
            Point3D waypoint=goal.Center;
            if(m_QueenRoute!=null)
            {
                while(m_QueenRouteIndex<m_QueenRoute.Count && queen.InRange(m_QueenRoute[m_QueenRouteIndex],5))++m_QueenRouteIndex;
                if(m_QueenRouteIndex<m_QueenRoute.Count)waypoint=m_QueenRoute[m_QueenRouteIndex];
            }
            if(m_QueenPath==null || waypoint!=m_QueenWaypoint)
            {
                Point3D legal;
                if (!FindInfestationPoint(waypoint,3,out legal)) return true;
                m_QueenWaypoint=waypoint;m_QueenPath=new PathFollower(queen,legal);
                if(queen.AIObject!=null)m_QueenPath.Mover=queen.AIObject.DoMoveImpl;
            }
            MakeRoomForQueen(queen);
            Point3D before = queen.Location; m_QueenPath.Follow(false,1);
            if (queen.Location != before) ++QueenShelterMoves;
            if (queen.Location != m_QueenProgressPosition)
            { m_QueenProgressPosition = queen.Location; m_QueenProgressTime = DateTime.UtcNow; }
            else if (DateTime.UtcNow-m_QueenProgressTime >= TimeSpan.FromSeconds(90))
            {
                m_QueenAvoid[m_QueenGoal] = DateTime.UtcNow.AddSeconds(300); m_QueenGoal = -1; m_QueenPath = null;
                m_NextQueenPlan = DateTime.MinValue; ++QueenShelterBlocked;
            }
            return true;
        }
    }

    public sealed class ArachnidQueenShelterAI : MeleeAI
    {
        private readonly ArachnidOutbreakChampion m_Queen;
        public ArachnidQueenShelterAI(ArachnidOutbreakChampion queen) : base(queen) { m_Queen = queen; }
        private bool Shelter()
        { return m_Queen.Controller != null && m_Queen.Controller.Infestation && m_Queen.Controller.MoveQueenToShelter(m_Queen); }
        public override bool DoActionWander() { return Shelter() || base.DoActionWander(); }
        public override bool DoActionGuard() { return Shelter() || base.DoActionGuard(); }
        public override bool DoActionFlee() { return Shelter() || base.DoActionFlee(); }
        public override bool DoActionCombat()
        {
            if (Shelter()) return true;
            if (m_Queen.Controller != null && m_Queen.Controller.Infestation && m_Queen.Controller.QueenShelterPatch >= 0
                && m_Queen.Combatant != null && !m_Queen.InRange(m_Queen.Combatant,3)) return true;
            return base.DoActionCombat();
        }
    }
}
