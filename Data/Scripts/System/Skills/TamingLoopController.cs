using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.SkillHandlers
{
    public static class TamingLoopController
    {
        private sealed class TamingLoopState
        {
            public Mobile From;
            public BaseCreature Creature;
            public Map StartMap;
            public int Generation;
            public bool ActiveAttempt;
        }

        private sealed class TamingLoopResumeState
        {
            public Mobile From;
            public int Generation;
        }

        private static readonly Dictionary<Mobile, TamingLoopState> m_States =
            new Dictionary<Mobile, TamingLoopState>();

        private static int m_NextGeneration;

        public static void Initialize()
        {
            EventSink.Logout += new LogoutEventHandler(OnLogout);
            EventSink.Disconnected += new DisconnectedEventHandler(OnDisconnected);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(OnPlayerDeath);
            EventSink.AggressiveAction += new AggressiveActionEventHandler(OnAggressiveAction);

            CommandSystem.Register(
                "StopTaming",
                AccessLevel.Player,
                new CommandEventHandler(StopTaming_OnCommand)
            );

            CommandSystem.Register(
                "StopTame",
                AccessLevel.Player,
                new CommandEventHandler(StopTaming_OnCommand)
            );
        }

        [Usage("StopTaming")]
        [Aliases("StopTame")]
        [Description("Stops the current auto-loop taming action.")]
        private static void StopTaming_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null)
                return;

            if (Cancel(from))
                from.SendMessage("You stop auto-taming.");
            else
                from.SendMessage("You are not currently auto-taming.");
        }

        public static void CancelForNewTarget(Mobile from)
        {
            Cancel(from);
        }

        public static void BeginLoop(Mobile from, BaseCreature creature)
        {
            if (from == null || creature == null)
                return;

            TamingLoopState state = new TamingLoopState();

            state.From = from;
            state.Creature = creature;
            state.StartMap = from.Map;
            state.Generation = ++m_NextGeneration;
            state.ActiveAttempt = true;

            m_States[from] = state;
        }

        public static void StopLoop(Mobile from)
        {
            Cancel(from);
        }

        public static void OnTamingFinished(
            Mobile from,
            BaseCreature creature,
            bool continueLoop
        )
        {
            TamingLoopState state;

            if (!TryGetMatchingState(from, creature, out state))
                return;

            if (!state.ActiveAttempt)
                return;

            state.ActiveAttempt = false;

            if (!continueLoop || !CanContinue(state))
            {
                Cancel(from);
                return;
            }

            TamingLoopResumeState resumeState = new TamingLoopResumeState();

            resumeState.From = from;
            resumeState.Generation = state.Generation;

            Timer.DelayCall(
                TimeSpan.Zero,
                new TimerStateCallback(ContinueTaming_Callback),
                resumeState
            );
        }

        private static void ContinueTaming_Callback(object state)
        {
            TamingLoopResumeState resumeState = state as TamingLoopResumeState;

            if (resumeState == null)
                return;

            ContinueTaming(resumeState);
        }

        private static void ContinueTaming(TamingLoopResumeState resumeState)
        {
            TamingLoopState state;

            if (!TryGetState(resumeState.From, resumeState.Generation, out state))
                return;

            if (!CanContinue(state))
            {
                Cancel(state.From);
                return;
            }

            if (state.ActiveAttempt)
                return;

            state.ActiveAttempt = true;

            if (!Taming.StartTamingFromLoop(state.From, state.Creature))
                Cancel(state.From);
        }

        private static bool TryGetState(
            Mobile from,
            int generation,
            out TamingLoopState state
        )
        {
            state = null;

            if (from == null)
                return false;

            if (!m_States.TryGetValue(from, out state))
                return false;

            if (state.Generation != generation)
            {
                state = null;
                return false;
            }

            return true;
        }

        private static bool TryGetMatchingState(
            Mobile from,
            BaseCreature creature,
            out TamingLoopState state
        )
        {
            state = null;

            if (from == null)
                return false;

            if (!m_States.TryGetValue(from, out state))
                return false;

            if (state.Creature != creature)
            {
                state = null;
                return false;
            }

            return true;
        }

        private static bool CanContinue(TamingLoopState state)
        {
            if (state == null)
                return false;

            Mobile from = state.From;
            BaseCreature creature = state.Creature;

            if (from == null || from.Deleted || !from.Alive)
                return false;

            if (creature == null || creature.Deleted)
                return false;

            if (from.Map == null || from.Map != state.StartMap)
                return false;

            if (creature.Map == null || creature.Map != state.StartMap)
                return false;

            if (!creature.Tamable || creature.Controlled)
                return false;

            if (from.Combatant != null || creature.Combatant == from)
                return false;

            return true;
        }

        private static bool Cancel(Mobile from)
        {
            if (from == null)
                return false;

            return m_States.Remove(from);
        }

        private static void CancelByCreature(Mobile mobile)
        {
            BaseCreature creature = mobile as BaseCreature;

            if (creature == null)
                return;

            List<Mobile> toCancel = new List<Mobile>();

            foreach (KeyValuePair<Mobile, TamingLoopState> kvp in m_States)
            {
                if (kvp.Value.Creature == creature)
                    toCancel.Add(kvp.Key);
            }

            for (int i = 0; i < toCancel.Count; ++i)
                Cancel(toCancel[i]);
        }

        private static void OnLogout(LogoutEventArgs e)
        {
            Cancel(e.Mobile);
        }

        private static void OnDisconnected(DisconnectedEventArgs e)
        {
            Cancel(e.Mobile);
        }

        private static void OnPlayerDeath(PlayerDeathEventArgs e)
        {
            Cancel(e.Mobile);
        }

        private static void OnAggressiveAction(AggressiveActionEventArgs e)
        {
            Cancel(e.Aggressor);
            Cancel(e.Aggressed);
            CancelByCreature(e.Aggressor);
            CancelByCreature(e.Aggressed);
        }
    }
}
