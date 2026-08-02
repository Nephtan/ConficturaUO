using System;
using System.Collections.Generic;
using System.IO;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.Confictura
{
    public sealed class TurnPendingAction
    {
        public TurnActionLease Lease;
        public TurnActionRequest Request;
        public int Cost;
    }

    public sealed class TurnHostilityEdge
    {
        public Mobile Source;
        public Mobile Target;
        public bool Support;
    }

    public sealed class TurnEffectState
    {
        public TimeSpan HitsRegenRemaining;
        public TimeSpan StamRegenRemaining;
        public TimeSpan ManaRegenRemaining;
        public TimeSpan ParalyzeRemaining;
        public TimeSpan FreezeRemaining;
        public PoisonImpl.PoisonTimer PoisonTimer;
        public TimeSpan PoisonRemaining;
        public TimeSpan PoisonInterval;
        public TimeSpan SummonRemaining;
    }

    public sealed class TurnParticipant
    {
        public Mobile Mobile;
        public TurnCombatGroup Group;
        public int InitiativeRoll;
        public int InitiativeTotal;
        public int Dexterity;
        public int EligibleRound;
        public bool Acted;
        public int ActionPoints;
        public DateTime LogicalTime;
        public bool HasTakenFirstTurn;
        public bool Disconnected;
        public DateTime DisconnectedSince;
        public TurnPendingAction Pending;
        public TurnEffectState Effects;
        public int AIDecisionsThisTurn;
        public int AINoProgressCount;
        public double AISimulatedSecondsThisTurn;
        public TimeSpan PendingEndEffectElapsed;
    }

    public sealed class TurnCombatGroup
    {
        public long ID;
        public int Round;
        public int CatalogVersion;
        public List<TurnParticipant> Participants;
        public List<TurnHostilityEdge> Edges;
        public TurnParticipant Current;
        public DateTime TurnDeadline;

        public TurnCombatGroup()
        {
            Participants = new List<TurnParticipant>();
            Edges = new List<TurnHostilityEdge>();
        }
    }

    internal enum TurnCombatantIntentDisposition
    {
        Native,
        OpenGroup,
        JoinActor,
        JoinTarget,
        MergeGroups,
        Select,
        Reject
    }

    internal enum TurnEffectTimerChangeDisposition
    {
        Removed,
        ActorClock,
        WallClock,
        Unknown
    }

    public sealed class TurnBasedCombatManager : ITurnBasedCombatHandler
    {
        private sealed class InitiativeComparer : IComparer<TurnParticipant>
        {
            public int Compare(TurnParticipant x, TurnParticipant y)
            {
                int result = y.InitiativeTotal.CompareTo(x.InitiativeTotal);

                if (result == 0)
                    result = y.Dexterity.CompareTo(x.Dexterity);

                if (result == 0)
                    result = x.Mobile.Serial.CompareTo(y.Mobile.Serial);

                return result;
            }
        }

        private sealed class SchedulerTimer : Timer
        {
            private readonly TurnBasedCombatManager m_Manager;

            public SchedulerTimer(TurnBasedCombatManager manager)
                : base(TimeSpan.FromMilliseconds(250.0), TimeSpan.FromMilliseconds(250.0))
            {
                m_Manager = manager;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                m_Manager.OnSchedulerTick();
            }
        }

        private static TurnBasedCombatManager m_Instance;
        private readonly Dictionary<Mobile, TurnParticipant> m_Participants;
        private readonly Dictionary<long, TurnPendingAction> m_PendingActions;
        private readonly List<TurnCombatGroup> m_Groups;
        private TurnBasedCombatConfiguration m_Configuration;
        private SchedulerTimer m_Scheduler;
        private bool m_RuntimeEnabled;
        private bool m_ProcessingAI;
        private long m_NextGroupID;
        private long m_NextLeaseID;
        private DateTime m_NextHudRefresh;

        public static TurnBasedCombatManager Instance
        {
            get { return m_Instance; }
        }

        public bool Enabled
        {
            get { return m_RuntimeEnabled; }
        }

        public int GroupCount
        {
            get { return m_Groups.Count; }
        }

        public TurnBasedCombatConfiguration Configuration
        {
            get { return m_Configuration; }
        }

        public static int ComputeInitiativeTotal(int roll, int dexterity)
        {
            return roll + (dexterity / 10);
        }

        public static int ComputeDurationAP(double seconds, int maximum)
        {
            int cost = (int)Math.Ceiling(seconds / 0.25);

            if (cost < 1)
                cost = 1;
            else if (cost > maximum)
                cost = maximum;

            return cost;
        }

        internal static TurnEffectTimerChangeDisposition ClassifyPoisonTimerChange(
            Type timerType,
            TurnEffectRule rule
        )
        {
            if (timerType == null)
                return TurnEffectTimerChangeDisposition.Removed;

            if (
                rule == null
                || rule.RuntimeType != timerType.FullName
                || !typeof(PoisonImpl.PoisonTimer).IsAssignableFrom(timerType)
            )
                return TurnEffectTimerChangeDisposition.Unknown;

            return rule.ClockPolicy == "ActorClock"
                ? TurnEffectTimerChangeDisposition.ActorClock
                : TurnEffectTimerChangeDisposition.WallClock;
        }

        public static void Initialize()
        {
            if (m_Instance != null)
                return;

            TurnBasedCombatManager manager = new TurnBasedCombatManager();
            m_Instance = manager;

            if (!TurnBasedCombatBridge.Register(manager))
            {
                Console.WriteLine("Turn-based combat could not register its core bridge.");
                m_Instance = null;
                return;
            }

            EventSink.Disconnected += new DisconnectedEventHandler(manager.OnDisconnected);
            EventSink.Connected += new ConnectedEventHandler(manager.OnConnected);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(manager.OnPlayerDeath);
            EventSink.WorldSave += new WorldSaveEventHandler(manager.OnWorldSave);
            EventSink.Shutdown += new ShutdownEventHandler(manager.OnShutdown);

            manager.m_Scheduler = new SchedulerTimer(manager);
            manager.m_Scheduler.Start();

            if (manager.m_Configuration.Enabled)
            {
                string reason;

                if (!manager.TryEnable(out reason))
                    manager.Log("startup_enable_blocked", null, null, reason);
            }

            Console.WriteLine(
                "Turn-based combat initialized ({0}).",
                manager.m_RuntimeEnabled ? "enabled" : "disabled"
            );
        }

        private TurnBasedCombatManager()
        {
            m_Participants = new Dictionary<Mobile, TurnParticipant>();
            m_PendingActions = new Dictionary<long, TurnPendingAction>();
            m_Groups = new List<TurnCombatGroup>();
            m_Configuration = TurnBasedCombatConfiguration.Load();
            m_RuntimeEnabled = false;
            m_NextGroupID = 1;
            m_NextLeaseID = 1;
            m_NextHudRefresh = DateTime.Now;
        }

        public bool IsParticipant(Mobile mobile)
        {
            return mobile != null && m_Participants.ContainsKey(mobile);
        }

        public TurnParticipant GetParticipant(Mobile mobile)
        {
            if (mobile == null)
                return null;

            TurnParticipant participant;
            m_Participants.TryGetValue(mobile, out participant);
            return participant;
        }

        internal static TurnCombatantIntentDisposition ClassifyCombatantIntent(
            bool enabled,
            bool actorParticipant,
            bool targetParticipant,
            bool sameGroup,
            bool actorCurrent
        )
        {
            if (!enabled)
                return TurnCombatantIntentDisposition.Native;

            if (!actorParticipant && !targetParticipant)
                return TurnCombatantIntentDisposition.OpenGroup;

            if (!actorParticipant)
                return TurnCombatantIntentDisposition.JoinActor;

            if (!actorCurrent)
                return TurnCombatantIntentDisposition.Reject;

            if (!targetParticipant)
                return TurnCombatantIntentDisposition.JoinTarget;

            if (!sameGroup)
                return TurnCombatantIntentDisposition.MergeGroups;

            return TurnCombatantIntentDisposition.Select;
        }

        internal static TurnCombatantChangeMode GetCombatantChangeMode(
            TurnCombatantIntentDisposition disposition
        )
        {
            switch (disposition)
            {
                case TurnCombatantIntentDisposition.OpenGroup:
                case TurnCombatantIntentDisposition.JoinActor:
                case TurnCombatantIntentDisposition.Reject:
                    return TurnCombatantChangeMode.Reject;
                case TurnCombatantIntentDisposition.JoinTarget:
                case TurnCombatantIntentDisposition.MergeGroups:
                case TurnCombatantIntentDisposition.Select:
                    return TurnCombatantChangeMode.SelectionOnly;
                default:
                    return TurnCombatantChangeMode.Native;
            }
        }

        public TurnCombatGroup GetGroup(Mobile mobile)
        {
            TurnParticipant participant = GetParticipant(mobile);
            return participant == null ? null : participant.Group;
        }

        public TurnCombatantChangeMode DecideCombatantChange(TurnCombatantChangeRequest request)
        {
            if (!m_RuntimeEnabled || request == null || request.NewCombatant == null)
                return TurnCombatantChangeMode.Native;

            Mobile actor = request.Actor;
            Mobile target = request.NewCombatant;

            if (
                actor == null
                || actor.Deleted
                || !actor.Alive
                || target.Deleted
                || !target.Alive
            )
                return TurnCombatantChangeMode.Reject;

            TurnParticipant actorParticipant = GetParticipant(actor);
            TurnParticipant targetParticipant = GetParticipant(target);
            TurnCombatantIntentDisposition disposition = ClassifyCombatantIntent(
                true,
                actorParticipant != null,
                targetParticipant != null,
                actorParticipant != null
                    && targetParticipant != null
                    && actorParticipant.Group == targetParticipant.Group,
                actorParticipant != null && actorParticipant.Group.Current == actorParticipant
            );

            switch (disposition)
            {
                case TurnCombatantIntentDisposition.OpenGroup:
                {
                    TurnCombatGroup group = CreateGroup(actor, target);
                    Log("combatant_intent_opened", group, actor, "Opening selection rejected until initiative.");
                    return GetCombatantChangeMode(disposition);
                }
                case TurnCombatantIntentDisposition.JoinActor:
                {
                    TurnCombatGroup group = targetParticipant.Group;
                    AddParticipant(group, actor, group.Round + 1);
                    AddFollowers(group, actor, group.Round + 1);
                    AddEdge(group, actor, target, false);
                    RefreshGroup(group);
                    Log("combatant_actor_joined", group, actor, "EligibleRound=" + (group.Round + 1));
                    return GetCombatantChangeMode(disposition);
                }
                case TurnCombatantIntentDisposition.JoinTarget:
                {
                    TurnCombatGroup group = actorParticipant.Group;
                    AddParticipant(group, target, group.Round + 1);
                    AddFollowers(group, target, group.Round + 1);
                    AddEdge(group, actor, target, false);
                    RefreshGroup(group);
                    Log("combatant_target_joined", group, target, "EligibleRound=" + (group.Round + 1));
                    return GetCombatantChangeMode(disposition);
                }
                case TurnCombatantIntentDisposition.MergeGroups:
                {
                    TurnCombatGroup group = actorParticipant.Group;
                    MergeGroups(group, targetParticipant.Group);
                    AddEdge(group, actor, target, false);
                    RefreshGroup(group);
                    return GetCombatantChangeMode(disposition);
                }
                case TurnCombatantIntentDisposition.Select:
                {
                    AddEdge(actorParticipant.Group, actor, target, false);
                    RefreshGroup(actorParticipant.Group);
                    return GetCombatantChangeMode(disposition);
                }
                case TurnCombatantIntentDisposition.Reject:
                {
                    Log("combatant_selection_blocked", actorParticipant.Group, actor, "Not current actor.");
                    return GetCombatantChangeMode(disposition);
                }
                default:
                    return GetCombatantChangeMode(disposition);
            }
        }

        public bool IsCurrentActor(Mobile mobile)
        {
            TurnParticipant participant = GetParticipant(mobile);
            return participant != null && participant.Group.Current == participant;
        }

        public TurnActionDecision BeginAction(TurnActionRequest request)
        {
            if (!m_RuntimeEnabled)
                return TurnActionDecision.Allow();

            if (request.Actor == null || request.Actor.Deleted)
                return TurnActionDecision.Block("That actor is no longer available.");

            if (request.Kind == TurnActionKind.Attack)
                return BeginAttack(request);

            Mobile target = request.Target as Mobile;
            TurnParticipant actorParticipant = GetParticipant(request.Actor);
            TurnParticipant targetParticipant = GetParticipant(target);

            if ((request.Harmful || request.Beneficial) && target != null)
            {
                TurnActionDecision interaction = PrepareInteraction(
                    request,
                    ref actorParticipant,
                    ref targetParticipant
                );

                if (interaction != null)
                    return interaction;
            }

            if (actorParticipant == null)
                return TurnActionDecision.Allow();

            if (actorParticipant.Group.Current != actorParticipant)
                return TurnActionDecision.Block("It is not your turn.");

            if (
                request.Kind == TurnActionKind.Spell
                && request.RequestedAP == 0
                && actorParticipant.Pending != null
            )
                return TurnActionDecision.Allow();

            if (
                actorParticipant.Pending != null
                && TurnBasedCombatBridge.IsActionLeaseActive(actorParticipant.Pending.Lease)
            )
            {
                TurnActionRule nestedRule = m_Configuration.ResolveAction(request);

                if (nestedRule == null)
                    return TurnActionDecision.Block("That nested action is not classified for turn-based combat.");

                if (!nestedRule.Allowed)
                    return TurnActionDecision.Block(nestedRule.FailureMessage);

                return TurnActionDecision.Allow();
            }

            return ReserveAction(actorParticipant, request);
        }

        private TurnActionDecision PrepareInteraction(
            TurnActionRequest request,
            ref TurnParticipant actorParticipant,
            ref TurnParticipant targetParticipant
        )
        {
            Mobile actor = request.Actor;
            Mobile target = (Mobile)request.Target;

            if (target == null || target.Deleted || !target.Alive)
                return TurnActionDecision.Block("That target is no longer available.");

            if (request.Harmful && !actor.CanBeHarmful(target, false))
                return TurnActionDecision.Block("You may not harm that target.");

            if (actorParticipant == null && targetParticipant == null)
            {
                if (!request.Harmful)
                    return null;

                TurnCombatGroup group = CreateGroup(actor, target);
                Log("group_opened", group, actor, "Opening action deferred until initiative.");
                return new TurnActionDecision(
                    false,
                    true,
                    null,
                    "Combat begins. The opening action is deferred until initiative."
                );
            }

            if (actorParticipant == null && targetParticipant != null)
            {
                actorParticipant = AddParticipant(
                    targetParticipant.Group,
                    actor,
                    targetParticipant.Group.Round + 1
                );
                AddFollowers(targetParticipant.Group, actor, targetParticipant.Group.Round + 1);
                AddEdge(targetParticipant.Group, actor, target, request.Beneficial);
                Log("outsider_joined", targetParticipant.Group, actor, "Action deferred to next round.");
                RefreshGroup(targetParticipant.Group);
                return new TurnActionDecision(
                    false,
                    true,
                    null,
                    "You join the combat and will act next round."
                );
            }

            if (actorParticipant != null && targetParticipant == null)
            {
                if (actorParticipant.Group.Current != actorParticipant)
                    return TurnActionDecision.Block("It is not your turn.");

                targetParticipant = AddParticipant(
                    actorParticipant.Group,
                    target,
                    actorParticipant.Group.Round + 1
                );
                AddFollowers(actorParticipant.Group, target, actorParticipant.Group.Round + 1);
                Log("target_joined", actorParticipant.Group, target, "Joined from current actor action.");
            }
            else if (
                actorParticipant != null
                && targetParticipant != null
                && actorParticipant.Group != targetParticipant.Group
            )
            {
                if (actorParticipant.Group.Current != actorParticipant)
                    return TurnActionDecision.Block("It is not your turn.");

                MergeGroups(actorParticipant.Group, targetParticipant.Group);
                targetParticipant = GetParticipant(target);
            }

            AddEdge(actorParticipant.Group, actor, target, request.Beneficial);
            RefreshGroup(actorParticipant.Group);
            return null;
        }

        private TurnActionDecision BeginAttack(TurnActionRequest request)
        {
            Mobile actor = request.Actor;
            Mobile target = request.Target as Mobile;

            if (target == null || target.Deleted || !target.Alive)
                return TurnActionDecision.Block("That target is no longer available.");

            if (!actor.CanBeHarmful(target, false))
                return TurnActionDecision.Block("You may not harm that target.");

            TurnParticipant actorParticipant = GetParticipant(actor);
            TurnParticipant targetParticipant = GetParticipant(target);

            if (actorParticipant == null && targetParticipant == null)
            {
                TurnCombatGroup group = CreateGroup(actor, target);
                Log("group_opened", group, actor, "Attack deferred until initiative.");
                return new TurnActionDecision(
                    false,
                    true,
                    null,
                    "Combat begins. Attack again on your turn."
                );
            }

            if (actorParticipant == null)
            {
                actorParticipant = AddParticipant(
                    targetParticipant.Group,
                    actor,
                    targetParticipant.Group.Round + 1
                );
                AddFollowers(targetParticipant.Group, actor, targetParticipant.Group.Round + 1);
                AddEdge(targetParticipant.Group, actor, target, false);
                RefreshGroup(targetParticipant.Group);
                return new TurnActionDecision(
                    false,
                    true,
                    null,
                    "You join the combat and will act next round."
                );
            }

            if (actorParticipant.Group.Current != actorParticipant)
                return TurnActionDecision.Block("It is not your turn.");

            if (targetParticipant == null)
            {
                targetParticipant = AddParticipant(
                    actorParticipant.Group,
                    target,
                    actorParticipant.Group.Round + 1
                );
                AddFollowers(actorParticipant.Group, target, actorParticipant.Group.Round + 1);
            }
            else if (targetParticipant.Group != actorParticipant.Group)
            {
                MergeGroups(actorParticipant.Group, targetParticipant.Group);
            }

            AddEdge(actorParticipant.Group, actor, target, false);

            string failure;
            ExecuteWeaponAttack(actorParticipant, target, out failure);

            return new TurnActionDecision(false, true, null, failure);
        }

        private TurnActionDecision ReserveAction(TurnParticipant participant, TurnActionRequest request)
        {
            if (participant.Pending != null)
                return TurnActionDecision.Block("Finish or cancel your current target first.");

            TurnActionRule rule = m_Configuration.ResolveAction(request);

            if (rule == null)
            {
                Log("unknown_action_blocked", participant.Group, participant.Mobile, request.Kind.ToString());
                return TurnActionDecision.Block("That action is not classified for turn-based combat.");
            }

            if (!rule.Allowed)
                return TurnActionDecision.Block(rule.FailureMessage);

            int cost = request.RequestedAP >= 0 ? request.RequestedAP : rule.APValue;

            if (cost < 0)
                cost = 0;
            else if (cost > m_Configuration.ActionPoints)
                cost = m_Configuration.ActionPoints;

            if (participant.ActionPoints < cost)
                return TurnActionDecision.Block("You do not have enough action points.");

            TurnActionLease lease = new TurnActionLease(m_NextLeaseID++);
            TurnPendingAction pending = new TurnPendingAction();
            pending.Lease = lease;
            pending.Request = request;
            pending.Cost = cost;

            participant.ActionPoints -= cost;
            participant.Pending = pending;
            m_PendingActions[lease.ID] = pending;

            Log(
                "action_reserved",
                participant.Group,
                participant.Mobile,
                request.Kind + " AP=" + cost
            );
            RefreshGroup(participant.Group);
            return new TurnActionDecision(true, false, lease, null);
        }

        public void CompleteAction(TurnActionLease lease, TurnActionResult result)
        {
            TurnPendingAction pending;

            if (!m_PendingActions.TryGetValue(lease.ID, out pending))
                return;

            TurnParticipant participant = GetParticipant(result.Actor);
            m_PendingActions.Remove(lease.ID);

            if (participant == null || participant.Pending != pending)
                return;

            bool refund = result.Phase == TurnActionPhase.Refund || result.Phase == TurnActionPhase.Cancel;

            if (refund)
                participant.ActionPoints += pending.Cost;

            participant.Pending = null;

            Log(
                refund ? "action_refunded" : "action_committed",
                participant.Group,
                participant.Mobile,
                pending.Request.Kind + " AP=" + pending.Cost
            );

            if (participant.ActionPoints <= 0 && participant.Group.Current == participant)
                EndTurn(participant.Mobile, false);
            else
                RefreshGroup(participant.Group);
        }

        public TurnActionLease GetPendingActionLease(Mobile mobile)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (
                participant == null
                || participant.Group.Current != participant
                || participant.Pending == null
            )
                return null;

            return participant.Pending.Lease;
        }

        public bool AuthorizeMutation(TurnMutationRequest request)
        {
            if (!m_RuntimeEnabled || request.Target == null)
                return true;

            TurnParticipant targetParticipant = GetParticipant(request.Target);
            TurnParticipant actorParticipant = GetParticipant(request.Actor);

            if (targetParticipant == null && actorParticipant == null)
            {
                if (
                    request.Actor != null
                    && request.Actor != request.Target
                    && request.Kind == TurnMutationKind.Damage
                    && request.Actor.Alive
                    && request.Target.Alive
                    && request.Actor.CanBeHarmful(request.Target, false)
                )
                {
                    TurnCombatGroup group = CreateGroup(request.Actor, request.Target);
                    Log("mutation_opened_group", group, request.Actor, request.Kind.ToString());
                    return false;
                }

                return true;
            }

            if (targetParticipant != null && actorParticipant == null)
            {
                if (request.Actor != null && request.Actor != request.Target)
                {
                    AddParticipant(
                        targetParticipant.Group,
                        request.Actor,
                        targetParticipant.Group.Round + 1
                    );
                    AddEdge(
                        targetParticipant.Group,
                        request.Actor,
                        request.Target,
                        request.Kind == TurnMutationKind.Healing
                    );
                    Log("mutation_outsider_joined", targetParticipant.Group, request.Actor, request.Kind.ToString());
                }

                Log("mutation_blocked", targetParticipant.Group, request.Target, request.Kind.ToString());
                return false;
            }

            if (actorParticipant != null && targetParticipant == null)
            {
                if (actorParticipant.Group.Current != actorParticipant || actorParticipant.Pending == null)
                    return false;

                targetParticipant = AddParticipant(
                    actorParticipant.Group,
                    request.Target,
                    actorParticipant.Group.Round + 1
                );
                AddEdge(
                    actorParticipant.Group,
                    request.Actor,
                    request.Target,
                    request.Kind == TurnMutationKind.Healing
                );
                return true;
            }

            if (actorParticipant != null && targetParticipant != null)
            {
                if (actorParticipant.Group != targetParticipant.Group)
                {
                    if (actorParticipant.Group.Current != actorParticipant)
                        return false;

                    MergeGroups(actorParticipant.Group, targetParticipant.Group);
                }

                return actorParticipant.Group.Current == actorParticipant && actorParticipant.Pending != null;
            }

            return false;
        }

        public DateTime GetActorTime(Mobile mobile, DateTime wallTime)
        {
            TurnParticipant participant = GetParticipant(mobile);
            return participant == null ? wallTime : participant.LogicalTime;
        }

        public TimeSpan GetActorInterval(Mobile mobile)
        {
            return TimeSpan.FromSeconds(m_Configuration.ActorSeconds);
        }

        public void TargetFinished(Mobile mobile, Target target, bool invoked)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null || participant.Pending == null)
                return;

            CompleteAction(
                participant.Pending.Lease,
                new TurnActionResult(
                    mobile,
                    target,
                    invoked ? TurnActionPhase.Commit : TurnActionPhase.Refund,
                    invoked
                )
            );
        }

        public void EffectScheduled(Mobile mobile, TurnMutationKind kind, TimeSpan duration)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null)
                return;

            TurnEffectRule rule = m_Configuration.ResolveEffect(kind, mobile);

            if (rule == null)
            {
                EmergencyDisable("Unclassified participant effect: " + kind, null);
                return;
            }

            if (rule.ClockPolicy != "ActorClock")
                return;

            if (kind == TurnMutationKind.Paralysis)
                participant.Effects.ParalyzeRemaining = duration;
            else if (kind == TurnMutationKind.Freeze)
                participant.Effects.FreezeRemaining = duration;

            mobile.SuspendTurnBasedStatusTimers();
            Log("effect_scheduled", participant.Group, mobile, kind + " duration=" + duration);
        }

        public void EffectTimerChanged(Mobile mobile, TurnMutationKind kind, Timer timer)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null || kind != TurnMutationKind.Poison)
                return;

            TurnEffectRule rule = timer == null
                ? null
                : m_Configuration.ResolveEffect(kind, timer);
            TurnEffectTimerChangeDisposition disposition = ClassifyPoisonTimerChange(
                timer == null ? null : timer.GetType(),
                rule
            );

            if (disposition == TurnEffectTimerChangeDisposition.Removed)
            {
                CapturePoison(participant, null);
                Log("poison_cleared", participant.Group, mobile, null);
                return;
            }

            if (disposition == TurnEffectTimerChangeDisposition.Unknown)
            {
                EmergencyDisable(
                    "Unclassified participant poison timer: " + timer.GetType().FullName,
                    null
                );
                return;
            }

            if (disposition == TurnEffectTimerChangeDisposition.WallClock)
                return;

            CapturePoison(participant, timer as PoisonImpl.PoisonTimer);
        }

        public void MobileRelocated(Mobile mobile, Point3D oldLocation, Map oldMap, bool forced)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null)
                return;

            bool tactical = participant.Group.Current == participant
                && participant.Pending != null
                && TurnBasedCombatBridge.IsActionLeaseActive(participant.Pending.Lease)
                && mobile.Map == oldMap
                && mobile.InRange(oldLocation, m_Configuration.EscapeRange);

            if (tactical)
            {
                Log("participant_tactical_relocation", participant.Group, mobile, oldMap + " " + oldLocation);
                return;
            }

            Log("participant_relocated", participant.Group, mobile, oldMap + " " + oldLocation);
            RemoveParticipant(participant, "relocated", true);
        }

        public void MobileDeleted(Mobile mobile)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant != null)
                RemoveParticipant(participant, "deleted", true);
        }

        public void EmergencyDisable(string reason, Exception exception)
        {
            string detail = reason;

            if (exception != null)
                detail += ": " + exception.GetType().FullName + ": " + exception.Message;

            Disable(detail);
        }

        public bool TryEnable(out string reason)
        {
            reason = null;

            if (!TurnBasedCombatBridge.IsHandlerReady(this))
            {
                reason = TurnBasedCombatBridge.IsFaulted
                    ? "Core bridge faulted and requires a server restart: " + TurnBasedCombatBridge.FaultReason
                    : "Core bridge handler is not registered.";
                return false;
            }

            if (m_RuntimeEnabled)
                return true;

            if (!m_Configuration.CheckCompatibilityGate(out reason))
                return false;

            m_RuntimeEnabled = true;
            Log("enabled", null, null, "Global runtime enable.");
            return true;
        }

        public void Disable(string reason)
        {
            m_RuntimeEnabled = false;
            DissolveAll(reason == null ? "disabled" : reason, true);
            Log("disabled", null, null, reason);
        }

        public bool Reload(out string reason)
        {
            reason = null;

            if (m_Groups.Count != 0)
            {
                reason = "Configuration reload is refused while combat groups exist.";
                return false;
            }

            try
            {
                TurnBasedCombatConfiguration configuration = TurnBasedCombatConfiguration.Load();
                configuration.CatalogVersion = m_Configuration.CatalogVersion + 1;
                m_Configuration = configuration;
                Log("configuration_reloaded", null, null, "Version=" + configuration.CatalogVersion);
                return true;
            }
            catch (Exception ex)
            {
                reason = ex.Message;
                return false;
            }
        }

        public bool EndTurn(Mobile mobile, bool voluntary)
        {
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null || participant.Group.Current != participant)
                return false;

            if (participant.Pending != null)
                RefundPending(participant, TurnActionPhase.Cancel);

            if (participant.PendingEndEffectElapsed > TimeSpan.Zero)
            {
                ProcessActorEffects(
                    participant,
                    participant.PendingEndEffectElapsed,
                    "EndOfTurn"
                );
                participant.PendingEndEffectElapsed = TimeSpan.Zero;
            }

            participant.ActionPoints = 0;
            participant.Acted = true;
            Log(voluntary ? "turn_ended" : "turn_exhausted", participant.Group, mobile, null);
            AdvanceTurn(participant.Group);
            return true;
        }

        public bool TryEscape(Mobile mobile, out string reason)
        {
            reason = null;
            TurnParticipant participant = GetParticipant(mobile);

            if (participant == null || participant.Group.Current != participant)
            {
                reason = "You may only escape during your turn.";
                return false;
            }

            if (participant.ActionPoints <= 0)
            {
                reason = "You have no action points remaining.";
                return false;
            }

            for (int i = 0; i < participant.Group.Edges.Count; ++i)
            {
                TurnHostilityEdge edge = participant.Group.Edges[i];

                if (edge.Support)
                    continue;

                Mobile hostile = null;

                if (edge.Source == mobile)
                    hostile = edge.Target;
                else if (edge.Target == mobile)
                    hostile = edge.Source;

                if (hostile == null || hostile.Deleted || !hostile.Alive)
                    continue;

                if (
                    hostile.Map == mobile.Map
                    && (
                        mobile.InRange(hostile, m_Configuration.EscapeRange)
                        || mobile.InLOS(hostile)
                    )
                )
                {
                    reason = "A living hostile is still within range or line of sight.";
                    return false;
                }
            }

            participant.ActionPoints = 0;
            RemoveParticipant(participant, "escaped", true);
            return true;
        }

        public bool ForceDissolve(Mobile mobile, string reason)
        {
            TurnCombatGroup group = GetGroup(mobile);

            if (group == null)
                return false;

            DissolveGroup(group, reason == null ? "staff dissolve" : reason, true);
            return true;
        }

        private TurnCombatGroup CreateGroup(Mobile actor, Mobile target)
        {
            TurnCombatGroup group = new TurnCombatGroup();
            group.ID = m_NextGroupID++;
            group.Round = 1;
            group.CatalogVersion = m_Configuration.CatalogVersion;
            m_Groups.Add(group);

            AddParticipant(group, actor, 1);
            AddParticipant(group, target, 1);
            AddFollowers(group, actor, 1);
            AddFollowers(group, target, 1);
            AddEdge(group, actor, target, false);
            group.Participants.Sort(new InitiativeComparer());
            StartNextActor(group);
            return group;
        }

        private TurnParticipant AddParticipant(TurnCombatGroup group, Mobile mobile, int eligibleRound)
        {
            TurnParticipant existing = GetParticipant(mobile);

            if (existing != null)
                return existing;

            TurnParticipant participant = new TurnParticipant();
            participant.Mobile = mobile;
            participant.Group = group;
            participant.InitiativeRoll = Utility.RandomMinMax(1, 20);
            participant.Dexterity = mobile.Dex;
            participant.InitiativeTotal = ComputeInitiativeTotal(
                participant.InitiativeRoll,
                mobile.Dex
            );
            participant.EligibleRound = eligibleRound;
            participant.LogicalTime = DateTime.Now;
            participant.Effects = CaptureEffects(mobile);
            participant.Disconnected = mobile.Player && mobile.NetState == null;
            participant.DisconnectedSince = participant.Disconnected ? DateTime.Now : DateTime.MinValue;

            group.Participants.Add(participant);
            group.Participants.Sort(new InitiativeComparer());
            m_Participants[mobile] = participant;
            mobile.SuspendTurnBasedStatusTimers();

            Log(
                "initiative",
                group,
                mobile,
                "d20=" + participant.InitiativeRoll + " total=" + participant.InitiativeTotal
            );
            return participant;
        }

        private void AddFollowers(TurnCombatGroup group, Mobile seed, int eligibleRound)
        {
            if (seed == null || seed.Map == null || seed.Map == Map.Internal)
                return;

            List<Mobile> masters = new List<Mobile>();
            masters.Add(seed);

            BaseCreature seedCreature = seed as BaseCreature;

            if (seedCreature != null)
            {
                Mobile master = seedCreature.GetMaster();

                if (master != null && master.Map == seed.Map && master.InRange(seed, m_Configuration.EscapeRange))
                {
                    AddParticipant(group, master, eligibleRound);
                    masters.Add(master);
                }
            }

            for (int masterIndex = 0; masterIndex < masters.Count; ++masterIndex)
            {
                Mobile master = masters[masterIndex];
                IPooledEnumerable mobiles = master.Map.GetMobilesInRange(
                    master.Location,
                    m_Configuration.EscapeRange
                );

                foreach (Mobile candidate in mobiles)
                {
                    BaseCreature creature = candidate as BaseCreature;

                    if (creature == null || creature.Deleted || !creature.Alive)
                        continue;

                    Mobile owner = creature.GetMaster();

                    if (owner == master || (owner != null && GetParticipant(owner) != null))
                    {
                        if (GetParticipant(creature) == null)
                        {
                            AddParticipant(group, creature, eligibleRound);
                            masters.Add(creature);
                        }
                    }
                }

                mobiles.Free();

                PlayerMobile playerMaster = master as PlayerMobile;

                if (playerMaster != null)
                {
                    List<Mobile> followers = playerMaster.AllFollowers;

                    for (int followerIndex = 0; followerIndex < followers.Count; ++followerIndex)
                    {
                        BaseCreature follower = followers[followerIndex] as BaseCreature;

                        if (
                            follower == null
                            || follower.Deleted
                            || !follower.Alive
                            || follower.Map != master.Map
                            || GetParticipant(follower) != null
                        )
                            continue;

                        bool linked = GetParticipant(follower.Combatant) != null
                            || GetParticipant(follower.ControlTarget) != null;

                        if (linked)
                        {
                            AddParticipant(group, follower, eligibleRound);
                            masters.Add(follower);
                        }
                    }
                }
            }
        }

        private void AddEdge(TurnCombatGroup group, Mobile source, Mobile target, bool support)
        {
            for (int i = 0; i < group.Edges.Count; ++i)
            {
                TurnHostilityEdge edge = group.Edges[i];

                if (edge.Source == source && edge.Target == target && edge.Support == support)
                    return;
            }

            TurnHostilityEdge newEdge = new TurnHostilityEdge();
            newEdge.Source = source;
            newEdge.Target = target;
            newEdge.Support = support;
            group.Edges.Add(newEdge);
        }

        private void MergeGroups(TurnCombatGroup primary, TurnCombatGroup absorbed)
        {
            if (primary == null || absorbed == null || primary == absorbed)
                return;

            for (int i = 0; i < absorbed.Participants.Count; ++i)
            {
                TurnParticipant participant = absorbed.Participants[i];
                bool waitingForNextRound = participant.EligibleRound > absorbed.Round;
                participant.Group = primary;
                participant.EligibleRound = waitingForNextRound
                    ? primary.Round + 1
                    : primary.Round;
                primary.Participants.Add(participant);
            }

            for (int i = 0; i < absorbed.Edges.Count; ++i)
            {
                TurnHostilityEdge edge = absorbed.Edges[i];
                AddEdge(primary, edge.Source, edge.Target, edge.Support);
            }

            primary.Participants.Sort(new InitiativeComparer());
            m_Groups.Remove(absorbed);
            Log("groups_merged", primary, primary.Current == null ? null : primary.Current.Mobile, "Absorbed=" + absorbed.ID);
            RefreshGroup(primary);
        }

        private void StartNextActor(TurnCombatGroup group)
        {
            if (group == null || !m_Groups.Contains(group))
                return;

            RemoveInvalidParticipants(group);

            if (group.Participants.Count == 0 || !HasHostility(group))
            {
                DissolveGroup(group, "no living hostility edges", false);
                return;
            }

            TurnParticipant next = null;

            for (int i = 0; i < group.Participants.Count; ++i)
            {
                TurnParticipant candidate = group.Participants[i];

                if (!candidate.Acted && candidate.EligibleRound <= group.Round && IsActive(candidate.Mobile))
                {
                    next = candidate;
                    break;
                }
            }

            if (next == null)
            {
                ++group.Round;

                for (int i = 0; i < group.Participants.Count; ++i)
                    group.Participants[i].Acted = false;

                for (int i = 0; i < group.Participants.Count; ++i)
                {
                    TurnParticipant candidate = group.Participants[i];

                    if (candidate.EligibleRound <= group.Round && IsActive(candidate.Mobile))
                    {
                        next = candidate;
                        break;
                    }
                }
            }

            if (next == null)
            {
                DissolveGroup(group, "no eligible living actors", false);
                return;
            }

            group.Current = next;
            next.ActionPoints = m_Configuration.ActionPoints;
            next.AIDecisionsThisTurn = 0;
            next.AINoProgressCount = 0;
            next.AISimulatedSecondsThisTurn = 0.0;

            if (next.HasTakenFirstTurn)
            {
                TimeSpan elapsed = TimeSpan.FromSeconds(m_Configuration.ActorSeconds);
                next.LogicalTime += elapsed;
                next.PendingEndEffectElapsed = elapsed;
                ProcessActorEffects(next, elapsed, "StartOfTurn");

                if (!IsActive(next.Mobile))
                {
                    if (GetParticipant(next.Mobile) != null)
                        RemoveParticipant(next, "actor-time effect removal", false);

                    if (m_Groups.Contains(group) && group.Current == null)
                        StartNextActor(group);

                    return;
                }
            }
            else
            {
                next.HasTakenFirstTurn = true;
            }

            group.TurnDeadline = DateTime.Now + TimeSpan.FromSeconds(m_Configuration.PlayerTimeoutSeconds);
            next.Mobile.SendMessage(
                0x59,
                "Your turn: {0} AP. Round {1}.",
                next.ActionPoints,
                group.Round
            );
            Log("turn_started", group, next.Mobile, "AP=" + next.ActionPoints);
            RefreshGroup(group);
        }

        private void AdvanceTurn(TurnCombatGroup group)
        {
            if (group == null || !m_Groups.Contains(group))
                return;

            if (group.Current != null)
                group.Current.Acted = true;

            group.Current = null;
            StartNextActor(group);
        }

        private void OnSchedulerTick()
        {
            if (!m_RuntimeEnabled)
                return;

            List<TurnCombatGroup> groups = new List<TurnCombatGroup>(m_Groups);

            for (int i = 0; i < groups.Count; ++i)
            {
                TurnCombatGroup group = groups[i];

                if (!m_Groups.Contains(group))
                    continue;

                RemoveInvalidParticipants(group);

                TurnParticipant current = group.Current;

                if (current == null)
                {
                    StartNextActor(group);
                    continue;
                }

                if (current.Disconnected)
                {
                    EndTurn(current.Mobile, false);
                    continue;
                }

                if (current.Mobile.Player)
                {
                    if (DateTime.Now >= group.TurnDeadline)
                    {
                        if (current.Mobile.Target != null)
                            current.Mobile.Target.Cancel(current.Mobile, TargetCancelType.Timeout);

                        EndTurn(current.Mobile, false);
                        current.Mobile.SendMessage("Your combat turn timed out.");
                    }
                }
                else
                {
                    ProcessAITurn(current);
                }
            }

            if (DateTime.Now >= m_NextHudRefresh)
            {
                m_NextHudRefresh = DateTime.Now + TimeSpan.FromMilliseconds(
                    m_Configuration.HudRefreshMilliseconds
                );

                for (int i = 0; i < m_Groups.Count; ++i)
                    RefreshGroup(m_Groups[i]);
            }
        }

        private void ProcessAITurn(TurnParticipant participant)
        {
            if (m_ProcessingAI || participant.Group.Current != participant)
                return;

            BaseCreature creature = participant.Mobile as BaseCreature;

            if (creature == null || creature.AIObject == null)
            {
                EndTurn(participant.Mobile, false);
                return;
            }

            TurnAIRule aiRule = m_Configuration.ResolveAI(creature);

            if (aiRule == null || aiRule.Strategy == "Blocked")
            {
                Log(
                    "unknown_ai_blocked",
                    participant.Group,
                    participant.Mobile,
                    creature.GetType().FullName + "/" + creature.AIObject.GetType().FullName
                );
                EndTurn(participant.Mobile, false);
                return;
            }

            m_ProcessingAI = true;
            DateTime sliceEnd = DateTime.Now + TimeSpan.FromMilliseconds(
                m_Configuration.SchedulerSliceMilliseconds
            );

            try
            {
                while (
                    participant.Group.Current == participant
                    && participant.ActionPoints > 0
                    && participant.AISimulatedSecondsThisTurn < m_Configuration.ActorSeconds
                    && participant.AIDecisionsThisTurn < m_Configuration.MaxAIDecisionsPerTurn
                    && DateTime.Now <= sliceEnd
                )
                {
                    ++participant.AIDecisionsThisTurn;
                    double pulseSeconds = Math.Max(0.05, creature.CurrentSpeed);
                    participant.AISimulatedSecondsThisTurn += pulseSeconds;
                    int oldAP = participant.ActionPoints;
                    Point3D oldLocation = participant.Mobile.Location;
                    Mobile oldCombatant = participant.Mobile.Combatant;

                    if (!creature.AIObject.ProcessTurnBasedPulse())
                    {
                        EndTurn(participant.Mobile, false);
                        return;
                    }

                    if (participant.Group.Current != participant)
                        return;

                    Mobile combatant = participant.Mobile.Combatant;

                    if (combatant != null && IsParticipant(combatant))
                    {
                        string failure;
                        ExecuteWeaponAttack(participant, combatant, out failure);
                    }

                    if (
                        oldAP == participant.ActionPoints
                        && oldLocation == participant.Mobile.Location
                        && oldCombatant == participant.Mobile.Combatant
                    )
                        ++participant.AINoProgressCount;
                    else
                        participant.AINoProgressCount = 0;

                    if (participant.AINoProgressCount >= 3)
                    {
                        EndTurn(participant.Mobile, false);
                        return;
                    }
                }

                if (
                    participant.Group.Current == participant
                    && (
                        participant.ActionPoints <= 0
                        || participant.AISimulatedSecondsThisTurn >= m_Configuration.ActorSeconds
                        || participant.AIDecisionsThisTurn >= m_Configuration.MaxAIDecisionsPerTurn
                    )
                )
                    EndTurn(participant.Mobile, false);
            }
            finally
            {
                m_ProcessingAI = false;
            }
        }

        private bool ExecuteWeaponAttack(
            TurnParticipant participant,
            Mobile target,
            out string failure
        )
        {
            failure = null;
            Mobile actor = participant.Mobile;
            IWeapon weapon = actor.Weapon;

            if (weapon == null || target == null || target.Deleted || !target.Alive)
            {
                failure = "There is no valid target to attack.";
                return false;
            }

            if (!actor.InRange(target, weapon.MaxRange) || !actor.InLOS(target))
            {
                failure = "That target is out of weapon range or line of sight.";
                return false;
            }

            if (!actor.CanBeHarmful(target, false))
            {
                failure = "You may not harm that target.";
                return false;
            }

            int cost = m_Configuration.ActionPoints;
            BaseWeapon baseWeapon = weapon as BaseWeapon;

            if (baseWeapon != null)
            {
                cost = ComputeDurationAP(
                    baseWeapon.GetDelay(actor).TotalSeconds,
                    m_Configuration.ActionPoints
                );
            }

            TurnActionRequest request = new TurnActionRequest(
                actor,
                target,
                weapon,
                TurnActionKind.Attack,
                cost,
                true,
                false
            );
            TurnActionDecision decision = ReserveAction(participant, request);

            if (!decision.Allowed)
            {
                failure = decision.Message;
                return false;
            }

            bool succeeded = false;
            IDisposable scope = TurnBasedCombatBridge.BeginMutation(
                new TurnMutationRequest(actor, null, TurnMutationKind.Action, 0, 0)
            );

            try
            {
                if (scope == null)
                    return false;

                actor.Combatant = target;
                weapon.OnBeforeSwing(actor, target);
                actor.RevealingAction();
                weapon.OnSwing(actor, target);
                succeeded = true;
                return true;
            }
            finally
            {
                if (scope != null)
                    scope.Dispose();

                CompleteAction(
                    decision.Lease,
                    new TurnActionResult(
                        actor,
                        target,
                        succeeded ? TurnActionPhase.Commit : TurnActionPhase.Refund,
                        succeeded
                    )
                );
            }
        }

        private TurnEffectState CaptureEffects(Mobile mobile)
        {
            TurnEffectState effects = new TurnEffectState();
            effects.HitsRegenRemaining = NormalizeRate(Mobile.GetHitsRegenRate(mobile));
            effects.StamRegenRemaining = NormalizeRate(Mobile.GetStamRegenRate(mobile));
            effects.ManaRegenRemaining = NormalizeRate(Mobile.GetManaRegenRate(mobile));
            effects.ParalyzeRemaining = mobile.GetTurnBasedParalyzeRemaining();
            effects.FreezeRemaining = mobile.GetTurnBasedFreezeRemaining();

            BaseCreature creature = mobile as BaseCreature;

            if (creature != null && creature.Summoned)
                effects.SummonRemaining = creature.SuspendTurnBasedUnsummonTimer();

            Timer poisonTimer = mobile.PoisonTimer;
            PoisonImpl.PoisonTimer poison = poisonTimer as PoisonImpl.PoisonTimer;

            if (poison != null)
            {
                effects.PoisonTimer = poison;
                effects.PoisonRemaining = Remaining(poison);
                effects.PoisonInterval = NormalizeRate(poison.Interval);
                poison.Stop();
            }

            return effects;
        }

        private void CapturePoison(TurnParticipant participant, PoisonImpl.PoisonTimer poison)
        {
            participant.Effects.PoisonTimer = poison;

            if (poison == null)
            {
                participant.Effects.PoisonRemaining = TimeSpan.Zero;
                participant.Effects.PoisonInterval = TimeSpan.Zero;
                return;
            }

            participant.Effects.PoisonRemaining = Remaining(poison);
            participant.Effects.PoisonInterval = NormalizeRate(poison.Interval);
            poison.Stop();
            Log("poison_captured", participant.Group, participant.Mobile, null);
        }

        private void ProcessActorEffects(
            TurnParticipant participant,
            TimeSpan elapsed,
            string tickPhase
        )
        {
            TurnEffectState effects = participant.Effects;
            Mobile mobile = participant.Mobile;
            IDisposable scope = TurnBasedCombatBridge.BeginMutation(
                new TurnMutationRequest(mobile, mobile, TurnMutationKind.Effect, 0, 0)
            );

            if (scope == null)
                return;

            try
            {
                bool regenerationEnabled = IsEffectEnabled(
                    participant,
                    TurnMutationKind.Regeneration,
                    mobile,
                    tickPhase
                );

                if (regenerationEnabled)
                {
                ProcessRegeneration(
                    mobile,
                    elapsed,
                    ref effects.HitsRegenRemaining,
                    Mobile.GetHitsRegenRate(mobile),
                    TurnMutationKind.Hits
                );
                ProcessRegeneration(
                    mobile,
                    elapsed,
                    ref effects.StamRegenRemaining,
                    Mobile.GetStamRegenRate(mobile),
                    TurnMutationKind.Stamina
                );
                ProcessRegeneration(
                    mobile,
                    elapsed,
                    ref effects.ManaRegenRemaining,
                    Mobile.GetManaRegenRate(mobile),
                    TurnMutationKind.Mana
                );
                }

                if (
                    IsEffectEnabled(participant, TurnMutationKind.Paralysis, mobile, tickPhase)
                    && mobile.Paralyzed
                    && effects.ParalyzeRemaining > TimeSpan.Zero
                )
                {
                    effects.ParalyzeRemaining -= elapsed;

                    if (effects.ParalyzeRemaining <= TimeSpan.Zero)
                        mobile.Paralyzed = false;
                }

                if (
                    IsEffectEnabled(participant, TurnMutationKind.Freeze, mobile, tickPhase)
                    && mobile.Frozen
                    && effects.FreezeRemaining > TimeSpan.Zero
                )
                {
                    effects.FreezeRemaining -= elapsed;

                    if (effects.FreezeRemaining <= TimeSpan.Zero)
                        mobile.Frozen = false;
                }

                if (
                    IsEffectEnabled(
                        participant,
                        TurnMutationKind.Poison,
                        effects.PoisonTimer,
                        tickPhase
                    )
                    && effects.PoisonTimer != null
                    && mobile.Poisoned
                )
                {
                    effects.PoisonRemaining -= elapsed;

                    while (
                        effects.PoisonTimer != null
                        && mobile.Poisoned
                        && effects.PoisonRemaining <= TimeSpan.Zero
                    )
                    {
                        PoisonImpl.PoisonTimer timer = effects.PoisonTimer;
                        timer.ProcessTurnBasedTick();

                        if (!mobile.Poisoned || mobile.PoisonTimer != timer)
                        {
                            CapturePoison(participant, mobile.PoisonTimer as PoisonImpl.PoisonTimer);
                            break;
                        }

                        effects.PoisonRemaining += effects.PoisonInterval;
                    }
                }

                BaseCreature creature = mobile as BaseCreature;

                if (
                    creature != null
                    && creature.Summoned
                    && IsEffectEnabled(
                        participant,
                        TurnMutationKind.Effect,
                        creature,
                        tickPhase
                    )
                )
                {
                    effects.SummonRemaining -= elapsed;

                    if (effects.SummonRemaining <= TimeSpan.Zero && !creature.Deleted)
                        creature.Delete();
                }

                Log("effect_clock_advanced", participant.Group, mobile, "elapsed=" + elapsed);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private bool IsEffectEnabled(
            TurnParticipant participant,
            TurnMutationKind kind,
            object context,
            string tickPhase
        )
        {
            TurnEffectRule rule = m_Configuration.ResolveEffect(kind, context);

            if (rule == null)
            {
                Log("unknown_effect_blocked", participant.Group, participant.Mobile, kind.ToString());
                return false;
            }

            return rule.ClockPolicy == "ActorClock" && rule.TickPhase == tickPhase;
        }

        private void ProcessRegeneration(
            Mobile mobile,
            TimeSpan elapsed,
            ref TimeSpan remaining,
            TimeSpan rate,
            TurnMutationKind kind
        )
        {
            remaining -= elapsed;
            rate = NormalizeRate(rate);

            while (remaining <= TimeSpan.Zero)
            {
                if (kind == TurnMutationKind.Hits && mobile.CanRegenHits && mobile.Hits < mobile.HitsMax)
                    ++mobile.Hits;
                else if (kind == TurnMutationKind.Stamina && mobile.CanRegenStam && mobile.Stam < mobile.StamMax)
                    ++mobile.Stam;
                else if (kind == TurnMutationKind.Mana && mobile.CanRegenMana && mobile.Mana < mobile.ManaMax)
                    ++mobile.Mana;

                remaining += rate;
            }
        }

        private void RemoveInvalidParticipants(TurnCombatGroup group)
        {
            List<TurnParticipant> remove = new List<TurnParticipant>();

            for (int i = 0; i < group.Participants.Count; ++i)
            {
                TurnParticipant participant = group.Participants[i];
                Mobile mobile = participant.Mobile;

                if (
                    !IsActive(mobile)
                    || (
                        participant.Disconnected
                        && participant.DisconnectedSince != DateTime.MinValue
                        && DateTime.Now
                            >= participant.DisconnectedSince
                                + TimeSpan.FromSeconds(m_Configuration.DisconnectGraceSeconds)
                    )
                )
                    remove.Add(participant);
            }

            for (int i = 0; i < remove.Count; ++i)
                RemoveParticipant(remove[i], "invalid", false);
        }

        private void RemoveParticipant(TurnParticipant participant, string reason, bool advance)
        {
            if (participant == null || !m_Participants.ContainsKey(participant.Mobile))
                return;

            TurnCombatGroup group = participant.Group;
            bool wasCurrent = group.Current == participant;

            if (participant.Pending != null)
                RefundPending(participant, TurnActionPhase.Cancel);

            m_Participants.Remove(participant.Mobile);
            group.Participants.Remove(participant);

            for (int i = group.Edges.Count - 1; i >= 0; --i)
            {
                if (
                    group.Edges[i].Source == participant.Mobile
                    || group.Edges[i].Target == participant.Mobile
                )
                    group.Edges.RemoveAt(i);
            }

            ResumeEffects(participant);
            RebaseCooldowns(participant);
            participant.Mobile.Combatant = null;
            participant.Mobile.CloseGump(typeof(TurnBasedCombatGump));
            Log("participant_removed", group, participant.Mobile, reason);

            if (wasCurrent)
                group.Current = null;

            if (group.Participants.Count == 0 || !HasHostility(group))
            {
                DissolveGroup(group, reason, false);
            }
            else if (wasCurrent && advance)
            {
                StartNextActor(group);
            }
            else
            {
                RefreshGroup(group);
            }
        }

        private void ResumeEffects(TurnParticipant participant)
        {
            Mobile mobile = participant.Mobile;
            TurnEffectState effects = participant.Effects;
            IDisposable scope = TurnBasedCombatBridge.BeginMutation(
                new TurnMutationRequest(mobile, mobile, TurnMutationKind.Administrative, 0, 0)
            );

            if (scope == null)
                return;

            try
            {
                if (mobile.Paralyzed)
                {
                    TimeSpan remaining = effects.ParalyzeRemaining;
                    mobile.Paralyzed = false;

                    if (remaining > TimeSpan.Zero)
                        mobile.Paralyze(remaining);
                }

                if (mobile.Frozen)
                {
                    TimeSpan remaining = effects.FreezeRemaining;
                    mobile.Frozen = false;

                    if (remaining > TimeSpan.Zero)
                        mobile.Freeze(remaining);
                }

                PoisonImpl.PoisonTimer poison = mobile.PoisonTimer as PoisonImpl.PoisonTimer;

                if (poison != null && mobile.Poisoned)
                {
                    poison.Delay = effects.PoisonRemaining > TimeSpan.Zero
                        ? effects.PoisonRemaining
                        : NormalizeRate(poison.Interval);
                    poison.Start();
                }

                BaseCreature creature = mobile as BaseCreature;

                if (creature != null && creature.Summoned && !creature.Deleted)
                    creature.ResumeTurnBasedUnsummonTimer(effects.SummonRemaining);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private void RebaseCooldowns(TurnParticipant participant)
        {
            DateTime wallTime = DateTime.Now;
            TimeSpan skillRemaining = participant.Mobile.NextSkillTime - participant.LogicalTime;
            TimeSpan spellRemaining = participant.Mobile.NextSpellTime - participant.LogicalTime;
            TimeSpan actionRemaining = participant.Mobile.NextActionTime - participant.LogicalTime;
            TimeSpan combatRemaining = participant.Mobile.NextCombatTime - participant.LogicalTime;

            participant.Mobile.NextSkillTime = skillRemaining > TimeSpan.Zero
                ? wallTime + skillRemaining
                : wallTime;
            participant.Mobile.NextSpellTime = spellRemaining > TimeSpan.Zero
                ? wallTime + spellRemaining
                : wallTime;
            participant.Mobile.NextActionTime = actionRemaining > TimeSpan.Zero
                ? wallTime + actionRemaining
                : wallTime;
            participant.Mobile.NextCombatTime = combatRemaining > TimeSpan.Zero
                ? wallTime + combatRemaining
                : wallTime;
        }

        private void RefundPending(TurnParticipant participant, TurnActionPhase phase)
        {
            if (participant.Pending == null)
                return;

            CompleteAction(
                participant.Pending.Lease,
                new TurnActionResult(participant.Mobile, null, phase, false)
            );
        }

        private void DissolveGroup(
            TurnCombatGroup group,
            string reason,
            bool resumeNativeCombat
        )
        {
            if (group == null || !m_Groups.Contains(group))
                return;

            List<TurnParticipant> participants = new List<TurnParticipant>(group.Participants);
            m_Groups.Remove(group);

            for (int i = 0; i < participants.Count; ++i)
            {
                TurnParticipant participant = participants[i];

                if (participant.Pending != null)
                    RefundPending(participant, TurnActionPhase.Cancel);

                m_Participants.Remove(participant.Mobile);
                ResumeEffects(participant);
                RebaseCooldowns(participant);

                if (resumeNativeCombat)
                    participant.Mobile.ResumeTurnBasedCombatScheduling();
                else
                    participant.Mobile.Combatant = null;

                participant.Mobile.CloseGump(typeof(TurnBasedCombatGump));
                participant.Mobile.SendMessage("Turn-based combat ended: {0}.", reason);
            }

            group.Participants.Clear();
            group.Edges.Clear();
            group.Current = null;
            Log("group_dissolved", group, null, reason);
        }

        private void DissolveAll(string reason, bool resumeNativeCombat)
        {
            List<TurnCombatGroup> groups = new List<TurnCombatGroup>(m_Groups);

            for (int i = 0; i < groups.Count; ++i)
                DissolveGroup(groups[i], reason, resumeNativeCombat);
        }

        private bool HasHostility(TurnCombatGroup group)
        {
            for (int i = 0; i < group.Edges.Count; ++i)
            {
                TurnHostilityEdge edge = group.Edges[i];

                if (
                    !edge.Support
                    && IsActive(edge.Source)
                    && IsActive(edge.Target)
                    && GetParticipant(edge.Source) != null
                    && GetParticipant(edge.Target) != null
                )
                    return true;
            }

            return false;
        }

        private static bool IsActive(Mobile mobile)
        {
            return mobile != null
                && !mobile.Deleted
                && mobile.Alive
                && mobile.Map != null
                && mobile.Map != Map.Internal;
        }

        private static TimeSpan NormalizeRate(TimeSpan rate)
        {
            return rate > TimeSpan.Zero ? rate : TimeSpan.FromMilliseconds(1.0);
        }

        private static TimeSpan Remaining(Timer timer)
        {
            TimeSpan remaining = timer.Next - DateTime.Now;
            return remaining > TimeSpan.Zero ? remaining : NormalizeRate(timer.Delay);
        }

        private void RefreshGroup(TurnCombatGroup group)
        {
            if (group == null || !m_Groups.Contains(group))
                return;

            for (int i = 0; i < group.Participants.Count; ++i)
            {
                Mobile mobile = group.Participants[i].Mobile;

                if (mobile.Player && mobile.NetState != null)
                {
                    mobile.CloseGump(typeof(TurnBasedCombatGump));
                    mobile.SendGump(new TurnBasedCombatGump(mobile, group));
                }
            }
        }

        private void OnDisconnected(DisconnectedEventArgs e)
        {
            TurnParticipant participant = GetParticipant(e.Mobile);

            if (participant != null)
            {
                participant.Disconnected = true;
                participant.DisconnectedSince = DateTime.Now;

                if (participant.Group.Current == participant)
                    EndTurn(e.Mobile, false);
            }
        }

        private void OnConnected(ConnectedEventArgs e)
        {
            TurnParticipant participant = GetParticipant(e.Mobile);

            if (participant != null)
            {
                participant.Disconnected = false;
                participant.DisconnectedSince = DateTime.MinValue;
                RefreshGroup(participant.Group);
            }
        }

        private void OnPlayerDeath(PlayerDeathEventArgs e)
        {
            TurnParticipant participant = GetParticipant(e.Mobile);

            if (participant != null)
                RemoveParticipant(participant, "death", true);
        }

        private void OnWorldSave(WorldSaveEventArgs e)
        {
            if (m_Groups.Count > 0)
                Log("world_save", null, null, "ActiveGroups=" + m_Groups.Count + " retained in memory.");
        }

        private void OnShutdown(ShutdownEventArgs e)
        {
            Disable("server shutdown");
        }

        public void Log(string eventName, TurnCombatGroup group, Mobile mobile, string detail)
        {
            string line = String.Format(
                "{0:o}\tevent={1}\tgroup={2}\tmobile={3}\tdetail={4}",
                DateTime.Now,
                eventName,
                group == null ? "-" : group.ID.ToString(),
                mobile == null ? "-" : mobile.Serial + ":" + mobile.Name,
                detail == null ? "-" : detail.Replace('\t', ' ')
            );

            Console.WriteLine(line);

            if (!m_Configuration.LogEnabled)
                return;

            try
            {
                string directory = Path.Combine(Core.BaseDirectory, "Logs");
                Directory.CreateDirectory(directory);
                File.AppendAllText(
                    Path.Combine(directory, "TurnBasedCombat.log"),
                    line + Environment.NewLine
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Turn-based combat log write failed: {0}", ex.Message);
            }
        }
    }
}
