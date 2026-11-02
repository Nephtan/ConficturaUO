using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Confictura.Champions
{
    public enum ArachnidQueenMode { Regroup, Challenge, Engage, Withdraw }

    public static class ArachnidQueenAdvantage
    {
        public static bool Losing(int healthPercent, int lossPercent, int friends, int opponents, int retreatPercent)
        { return healthPercent <= retreatPercent || lossPercent >= 12 && healthPercent < 90 || opponents > friends+1 && healthPercent < 80; }
        public static bool Favored(int healthPercent, int opponentHealthPercent, int lossPercent, int friends, int opponents, int reengagePercent)
        { return lossPercent < 8 && (healthPercent >= reengagePercent && friends >= opponents || healthPercent >= 55 && opponentHealthPercent < 50); }
    }

    public partial class ArachnidOutbreakController
    {
        private sealed class QueenLoss { public DateTime At; public int Amount; }
        private readonly List<QueenLoss> m_QueenLosses = new List<QueenLoss>();
        private readonly Dictionary<Mobile,DateTime> m_QueenThreats = new Dictionary<Mobile,DateTime>();
        private ArachnidQueenMode m_QueenMode;
        private DateTime m_NextQueenDecision, m_QueenCommitUntil, m_QueenChallengeUntil, m_QueenBackupDue, m_QueenBackupExpires, m_NextQueenBackup;
        private int m_QueenBackupSpent, m_QueenBackupPending, m_QueenRallyPatch = -1;
        private readonly List<Mobile> m_QueenBackupPrincipals = new List<Mobile>();
        public ArachnidQueenMode QueenMode { get { return m_QueenMode; } }
        public int QueenBackupSpent { get { return m_QueenBackupSpent; } }
        public int QueenBackupPending { get { return m_QueenBackupPending; } }
        public int QueenRallyPatch { get { return m_QueenRallyPatch; } }
        public int QueenBackupInterrupted { get; private set; }
        public int QueenTacticalChanges { get; private set; }
        private bool QueenRetreating { get { return m_QueenMode == ArachnidQueenMode.Withdraw; } }

        private static bool QueenPlayerAlly(Mobile mobile)
        {
            BaseCreature creature=mobile as BaseCreature;
            return creature!=null && (creature.Controlled || creature.Summoned || creature.ControlMaster!=null || creature.SummonMaster!=null);
        }

        private void RecordQueenPressure(HealthChangedEventArgs e)
        {
            if (!(e.Mobile is ArachnidOutbreakChampion) || e.Kind != HealthChangeKind.Damage || e.NewHits >= e.OldHits) return;
            QueenLoss loss=new QueenLoss(); loss.At=DateTime.UtcNow; loss.Amount=e.OldHits-e.NewHits;
            m_QueenLosses.RemoveAll(delegate(QueenLoss old){return loss.At-old.At>TimeSpan.FromSeconds(10);}); m_QueenLosses.Add(loss);
            Mobile principal=e.Source==null ? null : e.Source.Player ? e.Source : e.Source.GetDamageMaster(e.Mobile);
            if (principal != null && principal.Player && !principal.Deleted && principal.AccessLevel == AccessLevel.Player) m_QueenThreats[principal]=loss.At;
        }

        private void ResetQueenTactics()
        {
            m_QueenMode=ArachnidQueenMode.Regroup; m_QueenLosses.Clear(); m_QueenThreats.Clear();
            m_QueenBackupPending=0; m_QueenRallyPatch=-1; m_QueenBackupPrincipals.Clear();
            m_NextQueenDecision=m_QueenCommitUntil=m_QueenChallengeUntil=m_NextQueenBackup=DateTime.MinValue;
        }

        private void QueenTell(ArachnidOutbreakChampion queen, string text)
        { queen.PublicOverheadMessage(MessageType.Regular,0x35,true,text); ++QueenTacticalChanges; }

        private void ChooseQueenRetreat(ArachnidOutbreakChampion queen, Mobile threat)
        {
            if (threat == null) return;
            ArachnidInfestationPatch best=null; int bestScore=Int32.MinValue;
            // A bounded local search supplies a real path goal, never a teleport or a free heal.
            for (int x=queen.X-36;x<=queen.X+36;x+=12) for (int y=queen.Y-36;y<=queen.Y+36;y+=12)
            {
                ArachnidInfestationPatch patch=m_Infestation.Cell(x,y);
                if (patch==null || !patch.Established || patch.Retired || !queen.InLOS(patch.Center)) continue;
                int away=Math.Max(Math.Abs(patch.Center.X-threat.X),Math.Abs(patch.Center.Y-threat.Y));
                int current=Math.Max(Math.Abs(queen.X-threat.X),Math.Abs(queen.Y-threat.Y));
                if (away<current+8) continue;
                int distance=Math.Max(Math.Abs(patch.Center.X-queen.X),Math.Abs(patch.Center.Y-queen.Y));
                int score=away*2-distance;
                if (score>bestScore) { best=patch; bestScore=score; }
            }
            if (best==null) return;
            m_QueenGoal=best.Id; m_QueenAtGoal=false; m_QueenPath=null; m_QueenRoute=null;
            m_QueenProgressPosition=queen.Location; m_QueenProgressTime=DateTime.UtcNow; ++QueenShelterSelections;
        }

        private void StartQueenBackup(ArachnidOutbreakChampion queen, DateTime now)
        {
            if (m_QueenBackupPending>0 || m_QueenBackupSpent>=QueenBackupCount || now<m_NextQueenBackup) return;
            ArachnidInfestationPatch source=null; int nearest=73;
            for (int x=queen.X-72;x<=queen.X+72;x+=12) for (int y=queen.Y-72;y<=queen.Y+72;y+=12)
            {
                ArachnidInfestationPatch patch=m_Infestation.Cell(x,y);
                if (patch==null || patch.Retired || !patch.Established || patch.Guardian==null || patch.Guardian.Deleted || !patch.Guardian.Alive || m_Infestation.Tier(patch)<2) continue;
                int distance=Math.Max(Math.Abs(patch.Center.X-queen.X),Math.Abs(patch.Center.Y-queen.Y));
                if (distance<nearest) { source=patch; nearest=distance; }
            }
            if (source==null) return;
            m_QueenBackupPrincipals.Clear();
            foreach (KeyValuePair<Mobile,DateTime> threat in m_QueenThreats)
                if (!threat.Key.Deleted && threat.Key.Alive && !threat.Key.IsDeadBondedPet && threat.Key.Map==queen.Map
                    && threat.Key.InRange(source.Center,48) && now-threat.Value<=TimeSpan.FromSeconds(60)) m_QueenBackupPrincipals.Add(threat.Key);
            if (m_QueenBackupPrincipals.Count==0) return;
            m_QueenRallyPatch=source.Id; m_QueenBackupPending=Math.Min(3,QueenBackupCount-m_QueenBackupSpent);
            m_QueenBackupDue=now.AddSeconds(QueenBackupDelaySeconds); m_QueenBackupExpires=m_QueenBackupDue.AddSeconds(20);
            m_NextQueenBackup=now.AddSeconds(60);
            QueenTell(queen,"The queen calls to a rally nest! Break its guardian to stop the approaching guard.");
            source.Guardian.PublicOverheadMessage(MessageType.Regular,0x35,true,"The queen's rally source pulses here!");
            Effects.SendLocationEffect(source.Guardian.Location,source.Guardian.Map,0x376A,30,10);
        }

        private void ServiceQueenBackup(ArachnidOutbreakChampion queen, DateTime now)
        {
            if (m_QueenBackupPending<=0) return;
            ArachnidInfestationPatch source=m_Infestation.Find(m_QueenRallyPatch);
            if (source==null || source.Retired || source.Guardian==null || source.Guardian.Deleted || !source.Guardian.Alive)
            {
                m_QueenBackupPending=0; m_QueenRallyPatch=-1; ++QueenBackupInterrupted;
                QueenTell(queen,"The rally source falls silent. The approaching guard is cut off."); return;
            }
            if (now>m_QueenBackupExpires) { m_QueenBackupPending=0; return; }
            if (now<m_QueenBackupDue) return;
            List<Mobile> principals=new List<Mobile>();
            foreach (Mobile principal in m_QueenBackupPrincipals)
                if (principal!=null && !principal.Deleted && principal.Alive && principal.Map==queen.Map && principal.InRange(source.Center,48)) principals.Add(principal);
            if (principals.Count==0) { m_QueenBackupPending=0; return; }
            Point3D point; if (!FindInfestationPoint(source.Center,18,out point)) return;
            bool clear=true; IPooledEnumerable nearby=queen.Map.GetMobilesInRange(point,10);
            try { foreach (Mobile mobile in nearby) if (mobile.Player || QueenPlayerAlly(mobile)) { clear=false; break; } }
            finally { nearby.Free(); }
            if (!clear || !SpreadingEncounterScheduler.AdmitStructuralActor()) return;
            ArachnidBroodReinforcement guard=new ArachnidBroodReinforcement(this,source.Id,3,principals);
            guard.Name="an arachnid queen's guard"; guard.Home=source.Center; guard.RangeHome=18; guard.LastRelevant=now;
            m_Creatures.Add(guard); guard.MoveToWorld(point,queen.Map); SpreadingEncounterScheduler.Track(guard);
            if (guard.AIObject!=null) guard.AIObject.Activate();
            ++m_QueenBackupSpent; --m_QueenBackupPending;
        }

        internal bool HandleQueenTactics(ArachnidOutbreakChampion queen)
        {
            if (m_Infestation==null || queen==null || queen.Deleted || !queen.Alive) return false;
            DateTime now=DateTime.UtcNow;
            Mobile foe=queen.Combatant;
            ArachnidInfestationPatch refuge=m_Infestation.Find(m_QueenGoal);
            Point3D pursuitHome=refuge==null ? queen.Home : refuge.Center;
            bool valid=foe!=null && !foe.Deleted && foe.Alive && !foe.IsDeadBondedPet && foe.Map==queen.Map
                && queen.InRange(foe,24) && (QueenRetreating || foe.InRange(pursuitHome,24))
                && InfestationContains(foe.Location) && queen.CanBeHarmful(foe,false);
            if (foe!=null && !valid) { queen.Combatant=null; queen.FocusMob=null; }
            if (now>=m_NextQueenDecision)
            {
                m_NextQueenDecision=now.AddSeconds(2);
                m_QueenLosses.RemoveAll(delegate(QueenLoss loss){return now-loss.At>TimeSpan.FromSeconds(10);});
                List<Mobile> stale=new List<Mobile>();
                foreach (KeyValuePair<Mobile,DateTime> threat in m_QueenThreats)
                    if (threat.Key.Deleted || now-threat.Value>TimeSpan.FromSeconds(60)) stale.Add(threat.Key);
                foreach (Mobile threat in stale) m_QueenThreats.Remove(threat);
                int lost=0; foreach (QueenLoss loss in m_QueenLosses) lost+=loss.Amount;
                int health=queen.Hits*100/Math.Max(1,queen.HitsMax), lossPercent=lost*100/Math.Max(1,queen.HitsMax);
                int friends=1, opponents=0;
                IPooledEnumerable nearby=queen.GetMobilesInRange(18);
                try { foreach (Mobile mobile in nearby)
                {
                    if (mobile.Deleted || !mobile.Alive || mobile.IsDeadBondedPet || mobile.Blessed || mobile.AccessLevel>AccessLevel.Player) continue;
                    IArachnidOutbreakOwned brood=mobile as IArachnidOutbreakOwned;
                    ArachnidInfestationCreature creature=mobile as ArachnidInfestationCreature;
                    if (brood!=null && brood.Controller==this && !QueenPlayerAlly(mobile)) { if (mobile!=queen && (creature==null || !creature.EggSac)) ++friends; }
                    else if ((mobile.Player || QueenPlayerAlly(mobile)) && queen.CanBeHarmful(mobile,false)) ++opponents;
                }} finally { nearby.Free(); }
                bool losing=valid && ArachnidQueenAdvantage.Losing(health,lossPercent,friends,opponents,QueenRetreatPercent);
                bool favored=valid && ArachnidQueenAdvantage.Favored(health,foe.Hits*100/Math.Max(1,foe.HitsMax),lossPercent,friends,opponents,QueenReengagePercent);
                if (!valid && m_QueenMode!=ArachnidQueenMode.Regroup && (m_QueenMode!=ArachnidQueenMode.Withdraw || now>=m_QueenCommitUntil))
                { m_QueenMode=ArachnidQueenMode.Regroup; m_NextQueenPlan=DateTime.MinValue; }
                else if (losing && m_QueenMode!=ArachnidQueenMode.Withdraw && (now>=m_QueenCommitUntil || health<QueenRetreatPercent/2))
                {
                    m_QueenMode=ArachnidQueenMode.Withdraw; m_QueenCommitUntil=now.AddSeconds(15);
                    QueenTell(queen,"The wounded queen recoils toward her brood!"); ChooseQueenRetreat(queen,foe); StartQueenBackup(queen,now);
                }
                else if (m_QueenMode==ArachnidQueenMode.Withdraw)
                {
                    if (favored && health>=QueenReengagePercent && now>=m_QueenCommitUntil)
                    { m_QueenMode=ArachnidQueenMode.Challenge; m_QueenChallengeUntil=now.AddSeconds(3); QueenTell(queen,"The queen turns, gathering herself to strike!"); }
                    else if (now>=m_QueenCommitUntil)
                    { ChooseQueenRetreat(queen,foe); m_QueenCommitUntil=now.AddSeconds(15); StartQueenBackup(queen,now); }
                }
                else if (m_QueenMode==ArachnidQueenMode.Challenge && now>=m_QueenChallengeUntil)
                { m_QueenMode=favored ? ArachnidQueenMode.Engage : ArachnidQueenMode.Regroup; m_QueenCommitUntil=now.AddSeconds(12); }
                else if (favored && m_QueenMode==ArachnidQueenMode.Regroup && now>=m_QueenCommitUntil)
                { m_QueenMode=ArachnidQueenMode.Challenge; m_QueenChallengeUntil=now.AddSeconds(3); QueenTell(queen,"The queen fixes on her prey and prepares to advance!"); }
            }
            if (m_QueenMode==ArachnidQueenMode.Engage && valid) return false;
            if (m_QueenMode==ArachnidQueenMode.Challenge) return true;
            if (MoveQueenToShelter(queen)) return true;
            return valid || m_QueenMode==ArachnidQueenMode.Withdraw;
        }
    }
}
