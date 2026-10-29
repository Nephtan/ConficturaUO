using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.Confictura.Champions
{
    public abstract class ArachnidOutbreakWebItem : Item, IArachnidOutbreakOwned
    {
        private ArachnidOutbreakController m_Controller;
        private ArachnidOutbreakChampion m_Champion;
        private string m_AttemptId;
        private Mobile m_Victim;
        public ArachnidOutbreakController Controller { get { return m_Controller; } }
        public ArachnidOutbreakChampion Champion { get { return m_Champion; } }
        public string AttemptId { get { return m_AttemptId; } }
        public Mobile Victim { get { return m_Victim; } }

        protected ArachnidOutbreakWebItem(ArachnidOutbreakChampion champion, Mobile victim) : base(0x10D5)
        {
            m_Champion = champion;
            m_Controller = champion.Controller;
            m_AttemptId = champion.AttemptId;
            m_Victim = victim;
            Movable = false;
            MoveToWorld(victim.Location, victim.Map);
            m_Controller.RegisterWeb(this);
        }

        protected bool Valid()
        {
            return !Deleted && m_Controller != null && m_Controller.IsWebContext(m_Champion, m_Victim)
                && m_AttemptId == m_Controller.AttemptId && Map == m_Victim.Map && Location == m_Victim.Location;
        }

        internal static TurnParticipant Actor(Mobile mobile)
        {
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;
            return manager != null && manager.Enabled ? manager.GetParticipant(mobile) : null;
        }

        internal static bool SameMode(ArachnidOutbreakChampion champion, Mobile victim, out TurnParticipant actor)
        {
            actor = Actor(victim);
            TurnParticipant boss = Actor(champion);
            if (TurnBasedCombatBridge.IsParticipant(champion) || TurnBasedCombatBridge.IsParticipant(victim))
                return actor != null && boss != null && actor.Group != null && actor.Group == boss.Group;
            return actor == null && boss == null;
        }

        protected ArachnidOutbreakWebItem(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Controller);
            writer.Write(m_Champion);
            writer.Write(m_AttemptId);
            writer.Write(m_Victim);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Controller = reader.ReadItem() as ArachnidOutbreakController;
            m_Champion = reader.ReadMobile() as ArachnidOutbreakChampion;
            m_AttemptId = reader.ReadString();
            m_Victim = reader.ReadMobile();
        }
    }

    public sealed class ArachnidWebWarning : ArachnidOutbreakWebItem
    {
        private DateTime m_Due;
        private TurnParticipant m_Actor;
        private TurnCombatGroup m_Group;
        private long m_TurnsStarted;
        private Timer m_Timer;

        internal ArachnidWebWarning(ArachnidOutbreakChampion champion, Mobile victim) : base(champion, victim)
        {
            Name = "quivering web strands: move away before they land";
            Hue = 0x21;
            m_Due = DateTime.UtcNow.AddSeconds(3);
            m_Actor = Actor(victim);
            if (m_Actor != null)
            {
                m_Group = m_Actor.Group;
                m_TurnsStarted = m_Actor.TurnsStarted;
            }
            victim.SendMessage(0x21, "Web strands gather at your feet! Move away from this spot.");
            m_Timer = Timer.DelayCall(TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250), OnTimer);
        }

        private void OnTimer()
        {
            TryResolve(false);
        }

        internal bool TryResolve(bool bossPulse)
        {
            TurnParticipant actor;
            if (!Valid() || !SameMode(Champion, Victim, out actor)
                || actor != m_Actor || (actor != null && actor.Group != m_Group))
            {
                Delete();
                return false;
            }
            if (DateTime.UtcNow < m_Due)
                return false;
            if (actor != null && (!bossPulse || actor.TurnsStarted <= m_TurnsStarted || actor.Group.Current == actor))
                return false;
            bool applied = Champion.PerformWebAction(Victim, delegate
            {
                if (!Valid() || ArachnidWebRestraint.Find(Victim) != null)
                    return false;
                new ArachnidWebRestraint(Champion, Victim);
                return true;
            });
            if (applied || ArachnidWebRestraint.Find(Victim) != null)
                Delete();
            return applied;
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null) m_Timer.Stop();
            base.OnAfterDelete();
        }
        public ArachnidWebWarning(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Ground warnings never resume a delayed hostile callback after reload.
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Delete));
        }
    }

    public sealed class ArachnidWebRestraint : ArachnidOutbreakWebItem
    {
        private static readonly Dictionary<Mobile, ArachnidWebRestraint> m_Held = new Dictionary<Mobile, ArachnidWebRestraint>();
        private DateTime m_Until;
        private TurnParticipant m_Actor;
        private TurnCombatGroup m_Group;
        private Timer m_Timer;
        private bool m_Released;

        public static void Initialize()
        {
            EventSink.Movement -= OnMovement;
            EventSink.Movement += OnMovement;
            EventSink.MobileDeath -= OnDeath;
            EventSink.MobileDeath += OnDeath;
        }

        internal ArachnidWebRestraint(ArachnidOutbreakChampion champion, Mobile victim) : base(champion, victim)
        {
            Name = "binding web: double-click to free the trapped adventurer";
            m_Actor = Actor(victim);
            m_Group = m_Actor == null ? null : m_Actor.Group;
            m_Until = Now().AddSeconds(4);
            m_Held.Add(victim, this);
            victim.SendMessage("The web holds your feet. You can still attack, heal and cast. Double-click the web to break free.");
            m_Timer = Timer.DelayCall(TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250), OnTimer);
        }

        private DateTime Now()
        {
            return m_Actor == null ? DateTime.UtcNow : TurnBasedCombatManager.Instance.GetActionTime(Victim, DateTime.Now);
        }

        private bool Active()
        {
            TurnParticipant actor;
            return !m_Released && Valid() && SameMode(Champion, Victim, out actor) && actor == m_Actor
                && (actor == null || actor.Group == m_Group) && Now() < m_Until;
        }

        public static ArachnidWebRestraint Find(Mobile victim)
        {
            ArachnidWebRestraint restraint;
            if (victim == null || !m_Held.TryGetValue(victim, out restraint))
                return null;
            if (restraint.Active()) return restraint;
            restraint.Delete();
            return null;
        }

        private static void OnMovement(MovementEventArgs e)
        {
            if (e.Mobile != null && Find(e.Mobile) != null)
            {
                e.Blocked = true;
                e.Mobile.SendMessage("The web holds your feet. Double-click it to break free.");
            }
        }
        private static void OnDeath(Mobile mobile)
        {
            ArachnidWebRestraint restraint;
            if (mobile != null && m_Held.TryGetValue(mobile, out restraint)) restraint.Delete();
        }
        private void OnTimer()
        {
            if (!Active()) Delete();
        }

        public bool TryFree(Mobile helper, out string reason)
        {
            reason = "You cannot free that adventurer from here.";
            if (!Active() || helper == null || helper.Deleted || !helper.Player || !helper.Alive || helper.AccessLevel != AccessLevel.Player
                || helper.Map != Map || !helper.InRange(Location, 2) || !helper.InLOS(Victim) || !helper.CanBeBeneficial(Victim, false))
                return false;
            TurnActionDecision decision = TurnBasedCombatBridge.BeginAction(new TurnActionRequest(helper, Victim, this, TurnActionKind.ItemUse, 4, false, true));
            if (!decision.Allowed) { reason = decision.Message; return false; }
            bool freed = false;
            try
            {
                using (IDisposable action = TurnBasedCombatBridge.BeginActionScope(decision.Lease))
                using (IDisposable scope = TurnBasedCombatBridge.BeginMutation(new TurnMutationRequest(helper, Victim, TurnMutationKind.Effect, 0, 0)))
                {
                    if (scope == null || !Active()) { reason = "You must free them through a legal action on your turn."; return false; }
                    m_Released = true;
                    helper.DoBeneficial(Victim);
                    Controller.RecordWebRescue(Champion, helper, Victim);
                    Victim.SendMessage("You are free of the web.");
                    Delete();
                    reason = helper == Victim ? "You tear yourself free of the web." : "You tear the web away.";
                    freed = true;
                    return true;
                }
            }
            finally
            {
                TurnBasedCombatBridge.CompleteAction(decision.Lease,
                    new TurnActionResult(helper, Victim, freed ? TurnActionPhase.Commit : TurnActionPhase.Refund, freed));
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            string reason; TryFree(from, out reason);
            if (from != null && !from.Deleted) from.SendMessage(reason);
        }
        public override void OnAfterDelete()
        {
            if (m_Timer != null) m_Timer.Stop();
            ArachnidWebRestraint current;
            if (Victim != null && m_Held.TryGetValue(Victim, out current) && current == this) m_Held.Remove(Victim);
            base.OnAfterDelete();
        }
        public ArachnidWebRestraint(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Restraints are transient; never bind a loading mobile or restore AP clocks.
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Delete));
        }
    }
}
