using System;
using Server.Targeting;

namespace Server
{
    public enum TurnActionKind
    {
        None,
        Attack,
        Movement,
        Spell,
        Skill,
        ItemUse,
        MobileUse,
        Lift,
        Drop,
        Equip,
        Travel,
        EndTurn,
        Escape
    }

    public enum TurnActionPhase
    {
        Reserve,
        PendingTarget,
        Commit,
        Refund,
        Cancel
    }

    public enum TurnMutationKind
    {
        Unknown,
        Action,
        Damage,
        Healing,
        Hits,
        Stamina,
        Mana,
        Poison,
        Paralysis,
        Freeze,
        Regeneration,
        Effect,
        Administrative
    }

    public sealed class TurnActionLease
    {
        private readonly long m_ID;

        public long ID
        {
            get { return m_ID; }
        }

        public TurnActionLease(long id)
        {
            m_ID = id;
        }
    }

    public sealed class TurnActionRequest
    {
        private readonly Mobile m_Actor;
        private readonly object m_Target;
        private readonly object m_Context;
        private readonly TurnActionKind m_Kind;
        private readonly int m_RequestedAP;
        private readonly bool m_Harmful;
        private readonly bool m_Beneficial;

        public Mobile Actor { get { return m_Actor; } }
        public object Target { get { return m_Target; } }
        public object Context { get { return m_Context; } }
        public TurnActionKind Kind { get { return m_Kind; } }
        public int RequestedAP { get { return m_RequestedAP; } }
        public bool Harmful { get { return m_Harmful; } }
        public bool Beneficial { get { return m_Beneficial; } }

        public TurnActionRequest(
            Mobile actor,
            object target,
            object context,
            TurnActionKind kind,
            int requestedAP,
            bool harmful,
            bool beneficial
        )
        {
            m_Actor = actor;
            m_Target = target;
            m_Context = context;
            m_Kind = kind;
            m_RequestedAP = requestedAP;
            m_Harmful = harmful;
            m_Beneficial = beneficial;
        }
    }

    public sealed class TurnActionDecision
    {
        private readonly bool m_Allowed;
        private readonly bool m_Handled;
        private readonly TurnActionLease m_Lease;
        private readonly string m_Message;

        public bool Allowed { get { return m_Allowed; } }
        public bool Handled { get { return m_Handled; } }
        public TurnActionLease Lease { get { return m_Lease; } }
        public string Message { get { return m_Message; } }

        public TurnActionDecision(bool allowed, bool handled, TurnActionLease lease, string message)
        {
            m_Allowed = allowed;
            m_Handled = handled;
            m_Lease = lease;
            m_Message = message;
        }

        public static TurnActionDecision Allow()
        {
            return new TurnActionDecision(true, false, null, null);
        }

        public static TurnActionDecision Block(string message)
        {
            return new TurnActionDecision(false, false, null, message);
        }
    }

    public sealed class TurnActionResult
    {
        private readonly Mobile m_Actor;
        private readonly object m_Target;
        private readonly TurnActionPhase m_Phase;
        private readonly bool m_Succeeded;

        public Mobile Actor { get { return m_Actor; } }
        public object Target { get { return m_Target; } }
        public TurnActionPhase Phase { get { return m_Phase; } }
        public bool Succeeded { get { return m_Succeeded; } }

        public TurnActionResult(Mobile actor, object target, TurnActionPhase phase, bool succeeded)
        {
            m_Actor = actor;
            m_Target = target;
            m_Phase = phase;
            m_Succeeded = succeeded;
        }
    }

    public sealed class TurnMutationRequest
    {
        private readonly Mobile m_Actor;
        private readonly Mobile m_Target;
        private readonly TurnMutationKind m_Kind;
        private readonly int m_OldValue;
        private readonly int m_NewValue;

        public Mobile Actor { get { return m_Actor; } }
        public Mobile Target { get { return m_Target; } }
        public TurnMutationKind Kind { get { return m_Kind; } }
        public int OldValue { get { return m_OldValue; } }
        public int NewValue { get { return m_NewValue; } }

        public TurnMutationRequest(
            Mobile actor,
            Mobile target,
            TurnMutationKind kind,
            int oldValue,
            int newValue
        )
        {
            m_Actor = actor;
            m_Target = target;
            m_Kind = kind;
            m_OldValue = oldValue;
            m_NewValue = newValue;
        }
    }

    public interface ITurnBasedCombatHandler
    {
        bool Enabled { get; }
        bool IsParticipant(Mobile mobile);
        TurnActionDecision BeginAction(TurnActionRequest request);
        TurnActionLease GetPendingActionLease(Mobile mobile);
        void CompleteAction(TurnActionLease lease, TurnActionResult result);
        bool AuthorizeMutation(TurnMutationRequest request);
        DateTime GetActorTime(Mobile mobile, DateTime wallTime);
        TimeSpan GetActorInterval(Mobile mobile);
        void TargetFinished(Mobile mobile, Target target, bool invoked);
        void EffectScheduled(Mobile mobile, TurnMutationKind kind, TimeSpan duration);
        void EffectTimerChanged(Mobile mobile, TurnMutationKind kind, Timer timer);
        void MobileRelocated(Mobile mobile, Point3D oldLocation, Map oldMap, bool forced);
        void MobileDeleted(Mobile mobile);
        void EmergencyDisable(string reason, Exception exception);
    }

    public static class TurnBasedCombatBridge
    {
        private sealed class ActionScope : IDisposable
        {
            private readonly ActionScope m_Previous;
            private readonly TurnActionLease m_Lease;
            private bool m_Disposed;

            public ActionScope Previous { get { return m_Previous; } }
            public TurnActionLease Lease { get { return m_Lease; } }

            public ActionScope(ActionScope previous, TurnActionLease lease)
            {
                m_Previous = previous;
                m_Lease = lease;
            }

            public void Dispose()
            {
                if (!m_Disposed)
                {
                    m_Disposed = true;
                    m_ActionScope = m_Previous;
                }
            }
        }

        private sealed class MutationScope : IDisposable
        {
            private readonly MutationScope m_Previous;
            private readonly Mobile m_Actor;
            private readonly Mobile m_Target;
            private bool m_Disposed;

            public MutationScope Previous { get { return m_Previous; } }
            public Mobile Actor { get { return m_Actor; } }
            public Mobile Target { get { return m_Target; } }

            public MutationScope(MutationScope previous, Mobile actor, Mobile target)
            {
                m_Previous = previous;
                m_Actor = actor;
                m_Target = target;
            }

            public void Dispose()
            {
                if (!m_Disposed)
                {
                    m_Disposed = true;
                    m_MutationScope = m_Previous;
                }
            }
        }

        private sealed class TargetInvocationScope : IDisposable
        {
            private readonly TargetInvocationScope m_Previous;
            private readonly Mobile m_Actor;
            private readonly object m_Target;
            private bool m_Invalid;
            private bool m_Disposed;

            public TargetInvocationScope Previous { get { return m_Previous; } }
            public Mobile Actor { get { return m_Actor; } }
            public object Target { get { return m_Target; } }
            public bool Invalid { get { return m_Invalid; } }

            public TargetInvocationScope(TargetInvocationScope previous, Mobile actor, object target)
            {
                m_Previous = previous;
                m_Actor = actor;
                m_Target = target;
            }

            public void Invalidate()
            {
                m_Invalid = true;
            }

            public void Dispose()
            {
                if (!m_Disposed)
                {
                    m_Disposed = true;
                    m_TargetInvocationScope = m_Previous;
                }
            }
        }

        private sealed class EmptyScope : IDisposable
        {
            public void Dispose() { }
        }

        private static readonly EmptyScope m_EmptyScope = new EmptyScope();
        private static ITurnBasedCombatHandler m_Handler;

        [ThreadStatic]
        private static ActionScope m_ActionScope;

        [ThreadStatic]
        private static MutationScope m_MutationScope;

        [ThreadStatic]
        private static TargetInvocationScope m_TargetInvocationScope;

        public static ITurnBasedCombatHandler Handler
        {
            get { return m_Handler; }
        }

        public static bool Register(ITurnBasedCombatHandler handler)
        {
            if (handler == null || m_Handler != null)
                return false;

            m_Handler = handler;
            return true;
        }

        public static void Unregister(ITurnBasedCombatHandler handler)
        {
            if (m_Handler == handler)
                m_Handler = null;
        }

        public static bool IsEnabled
        {
            get
            {
                ITurnBasedCombatHandler handler = m_Handler;

                if (handler == null)
                    return false;

                try
                {
                    return handler.Enabled;
                }
                catch (Exception ex)
                {
                    Fail(handler, "Enabled check", ex);
                    return false;
                }
            }
        }

        public static bool IsParticipant(Mobile mobile)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return false;

            try
            {
                return handler.Enabled && handler.IsParticipant(mobile);
            }
            catch (Exception ex)
            {
                Fail(handler, "Participant check", ex);
                return false;
            }
        }

        public static TurnActionDecision BeginAction(TurnActionRequest request)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || request == null)
                return TurnActionDecision.Allow();

            try
            {
                if (!handler.Enabled)
                    return TurnActionDecision.Allow();

                TurnActionDecision decision = handler.BeginAction(request);
                return decision == null ? TurnActionDecision.Block("The turn-based combat handler rejected that action.") : decision;
            }
            catch (Exception ex)
            {
                Fail(handler, "Action reservation", ex);
                return TurnActionDecision.Block("Turn-based combat was disabled after an internal error.");
            }
        }

        public static IDisposable BeginActionScope(TurnActionLease lease)
        {
            if (lease == null)
                return m_EmptyScope;

            ActionScope scope = new ActionScope(m_ActionScope, lease);
            m_ActionScope = scope;
            return scope;
        }

        public static IDisposable BeginPendingActionScope(Mobile mobile)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return m_EmptyScope;

            try
            {
                if (!handler.Enabled)
                    return m_EmptyScope;

                return BeginActionScope(handler.GetPendingActionLease(mobile));
            }
            catch (Exception ex)
            {
                Fail(handler, "Pending action scope", ex);
                return m_EmptyScope;
            }
        }

        public static bool IsActionLeaseActive(TurnActionLease lease)
        {
            if (lease == null)
                return false;

            ActionScope scope = m_ActionScope;

            while (scope != null)
            {
                if (scope.Lease != null && scope.Lease.ID == lease.ID)
                    return true;

                scope = scope.Previous;
            }

            return false;
        }

        public static IDisposable BeginTargetInvocation(Mobile actor, object target)
        {
            TargetInvocationScope scope = new TargetInvocationScope(
                m_TargetInvocationScope,
                actor,
                target
            );
            m_TargetInvocationScope = scope;
            return scope;
        }

        public static void InvalidateTargetIntent(Mobile actor, object target)
        {
            TargetInvocationScope scope = m_TargetInvocationScope;

            if (scope == null || scope.Actor != actor)
                return;

            if (Object.ReferenceEquals(scope.Target, target))
                scope.Invalidate();
        }

        public static bool IsTargetIntentValid(Mobile actor)
        {
            TargetInvocationScope scope = m_TargetInvocationScope;
            return scope == null || scope.Actor != actor || !scope.Invalid;
        }

        public static void CompleteAction(TurnActionLease lease, TurnActionResult result)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || lease == null || result == null)
                return;

            try
            {
                handler.CompleteAction(lease, result);
            }
            catch (Exception ex)
            {
                Fail(handler, "Action completion", ex);
            }
        }

        public static IDisposable BeginMutation(TurnMutationRequest request)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || request == null)
                return m_EmptyScope;

            if (m_ActionScope != null)
                return PushScope(request.Actor, request.Target);

            if (m_MutationScope != null && ScopeMatches(request.Target))
                return PushScope(request.Actor, request.Target);

            try
            {
                if (!handler.Enabled || handler.AuthorizeMutation(request))
                    return PushScope(request.Actor, request.Target);

                return null;
            }
            catch (Exception ex)
            {
                Fail(handler, "Mutation authorization", ex);
                return null;
            }
        }

        public static bool AllowStateMutation(
            Mobile target,
            TurnMutationKind kind,
            int oldValue,
            int newValue
        )
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || target == null)
                return true;

            if (m_ActionScope != null)
                return true;

            if (m_MutationScope != null && ScopeMatches(target))
                return true;

            try
            {
                if (!handler.Enabled)
                    return true;

                return handler.AuthorizeMutation(
                    new TurnMutationRequest(null, target, kind, oldValue, newValue)
                );
            }
            catch (Exception ex)
            {
                Fail(handler, "State mutation authorization", ex);
                return false;
            }
        }

        public static DateTime GetTime(Mobile mobile)
        {
            DateTime wallTime = DateTime.Now;
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return wallTime;

            try
            {
                return handler.Enabled ? handler.GetActorTime(mobile, wallTime) : wallTime;
            }
            catch (Exception ex)
            {
                Fail(handler, "Actor time lookup", ex);
                return wallTime;
            }
        }

        public static TimeSpan GetActorInterval(Mobile mobile)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return TimeSpan.FromSeconds(5.0);

            try
            {
                return handler.Enabled
                    ? handler.GetActorInterval(mobile)
                    : TimeSpan.FromSeconds(5.0);
            }
            catch (Exception ex)
            {
                Fail(handler, "Actor interval lookup", ex);
                return TimeSpan.FromSeconds(5.0);
            }
        }

        public static void TargetFinished(Mobile mobile, Target target, bool invoked)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return;

            try
            {
                if (handler.Enabled)
                    handler.TargetFinished(mobile, target, invoked);
            }
            catch (Exception ex)
            {
                Fail(handler, "Target completion", ex);
            }
        }

        public static void EffectScheduled(Mobile mobile, TurnMutationKind kind, TimeSpan duration)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return;

            try
            {
                if (handler.Enabled)
                    handler.EffectScheduled(mobile, kind, duration);
            }
            catch (Exception ex)
            {
                Fail(handler, "Effect scheduling", ex);
            }
        }

        public static void EffectTimerChanged(Mobile mobile, TurnMutationKind kind, Timer timer)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return;

            try
            {
                if (handler.Enabled)
                    handler.EffectTimerChanged(mobile, kind, timer);
            }
            catch (Exception ex)
            {
                Fail(handler, "Effect timer change", ex);
            }
        }

        public static void MobileRelocated(
            Mobile mobile,
            Point3D oldLocation,
            Map oldMap,
            bool forced
        )
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return;

            try
            {
                if (handler.Enabled)
                    handler.MobileRelocated(mobile, oldLocation, oldMap, forced);
            }
            catch (Exception ex)
            {
                Fail(handler, "Relocation notification", ex);
            }
        }

        public static void MobileDeleted(Mobile mobile)
        {
            ITurnBasedCombatHandler handler = m_Handler;

            if (handler == null || mobile == null)
                return;

            try
            {
                if (handler.Enabled)
                    handler.MobileDeleted(mobile);
            }
            catch (Exception ex)
            {
                Fail(handler, "Mobile deletion", ex);
            }
        }

        private static IDisposable PushScope(Mobile actor, Mobile target)
        {
            MutationScope scope = new MutationScope(m_MutationScope, actor, target);
            m_MutationScope = scope;
            return scope;
        }

        private static bool ScopeMatches(Mobile target)
        {
            for (MutationScope scope = m_MutationScope; scope != null; scope = scope.Previous)
            {
                if (scope.Target == null || scope.Target == target)
                    return true;
            }

            return false;
        }

        private static void Fail(ITurnBasedCombatHandler handler, string operation, Exception exception)
        {
            Console.WriteLine("Turn-based combat bridge failure during {0}: {1}", operation, exception);

            Timer.DelayCall(
                TimeSpan.Zero,
                delegate()
                {
                    try
                    {
                        handler.EmergencyDisable(operation, exception);
                    }
                    catch (Exception nested)
                    {
                        Console.WriteLine("Turn-based combat emergency disable failed: {0}", nested);
                    }
                }
            );

            if (m_Handler == handler)
                m_Handler = null;
        }
    }
}
