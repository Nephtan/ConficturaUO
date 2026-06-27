using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Engines.Harvest
{
    public static class HarvestLoopController
    {
        private sealed class HarvestLoopState
        {
            public Mobile From;
            public Item Tool;
            public HarvestSystem System;
            public object ToHarvest;
            public Map StartMap;
            public Point3D StartLocation;
            public int Generation;
            public bool WaitingForCaptcha;
        }

        private sealed class HarvestLoopCaptchaState
        {
            public Mobile From;
            public int Generation;
        }

        private static readonly Dictionary<Mobile, HarvestLoopState> m_States =
            new Dictionary<Mobile, HarvestLoopState>();

        private static int m_NextGeneration;

        public static void Initialize()
        {
            EventSink.Movement += new MovementEventHandler(OnMovement);
            EventSink.Logout += new LogoutEventHandler(OnLogout);
            EventSink.Disconnected += new DisconnectedEventHandler(OnDisconnected);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(OnPlayerDeath);
            EventSink.AggressiveAction += new AggressiveActionEventHandler(OnAggressiveAction);

            CommandSystem.Register(
                "StopHarvest",
                AccessLevel.Player,
                new CommandEventHandler(StopHarvest_OnCommand)
            );
        }

        [Usage("StopHarvest")]
        [Description("Stops the current auto-loop harvesting action.")]
        private static void StopHarvest_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null)
                return;

            if (Cancel(from))
                from.SendMessage("You stop harvesting.");
            else
                from.SendMessage("You are not currently auto-harvesting.");
        }

        public static void CancelForNewTarget(Mobile from)
        {
            Cancel(from);
        }

        public static void BeginLoop(
            Mobile from,
            Item tool,
            HarvestSystem system,
            object toHarvest,
            Map startMap,
            Point3D startLocation
        )
        {
            if (from == null || tool == null || system == null)
                return;

            HarvestLoopState state = new HarvestLoopState();

            state.From = from;
            state.Tool = tool;
            state.System = system;
            state.ToHarvest = toHarvest;
            state.StartMap = startMap;
            state.StartLocation = startLocation;
            state.Generation = ++m_NextGeneration;

            m_States[from] = state;
        }

        public static void StopLoop(Mobile from)
        {
            Cancel(from);
        }

        public static void OnHarvestFinished(
            Mobile from,
            Item tool,
            HarvestSystem system,
            object toHarvest,
            bool continueLoop
        )
        {
            HarvestLoopState state;

            if (!TryGetMatchingState(from, tool, system, toHarvest, out state))
                return;

            if (!continueLoop || !CanContinue(state))
            {
                Cancel(from);
                return;
            }

            HarvestLoopCaptchaState resumeState = new HarvestLoopCaptchaState();

            resumeState.From = from;
            resumeState.Generation = state.Generation;

            Timer.DelayCall(
                TimeSpan.Zero,
                new TimerStateCallback(ContinueHarvesting_Callback),
                resumeState
            );
        }

        public static void OnCaptchaFailed(Mobile from, object actionObject)
        {
            HarvestLoopCaptchaState captchaState = actionObject as HarvestLoopCaptchaState;

            if (captchaState == null)
                return;

            Mobile mobile = from != null ? from : captchaState.From;
            HarvestLoopState state;

            if (TryGetState(mobile, captchaState.Generation, out state))
                Cancel(mobile);
        }

        private static void ContinueHarvesting_Callback(object state)
        {
            HarvestLoopCaptchaState resumeState = state as HarvestLoopCaptchaState;

            if (resumeState == null)
                return;

            ContinueHarvesting(resumeState);
        }

        private static void ContinueHarvesting(HarvestLoopCaptchaState resumeState)
        {
            HarvestLoopState state;

            if (!TryGetState(resumeState.From, resumeState.Generation, out state))
                return;

            if (!CanContinue(state))
            {
                Cancel(state.From);
                return;
            }

            if (!MyServerSettings.AllowMacroResources())
            {
                state.WaitingForCaptcha = true;
                CaptchaGump.sendCaptcha(
                    state.From,
                    ResumeAfterCaptcha,
                    new HarvestLoopCaptchaState
                    {
                        From = state.From,
                        Generation = state.Generation
                    }
                );
                return;
            }

            StartNextAttempt(state);
        }

        private static void ResumeAfterCaptcha(Mobile from, object actionObject)
        {
            HarvestLoopCaptchaState resumeState = actionObject as HarvestLoopCaptchaState;

            if (resumeState == null)
                return;

            HarvestLoopState state;

            if (!TryGetState(from, resumeState.Generation, out state))
                return;

            state.WaitingForCaptcha = false;

            if (!CanContinue(state))
            {
                Cancel(from);
                return;
            }

            StartNextAttempt(state);
        }

        private static void StartNextAttempt(HarvestLoopState state)
        {
            if (!state.System.StartHarvestingFromLoop(state.From, state.Tool, state.ToHarvest))
                Cancel(state.From);
        }

        private static bool TryGetState(Mobile from, int generation, out HarvestLoopState state)
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
            Item tool,
            HarvestSystem system,
            object toHarvest,
            out HarvestLoopState state
        )
        {
            state = null;

            if (from == null)
                return false;

            if (!m_States.TryGetValue(from, out state))
                return false;

            if (state.Tool != tool || state.System != system || state.ToHarvest != toHarvest)
            {
                state = null;
                return false;
            }

            return true;
        }

        private static bool CanContinue(HarvestLoopState state)
        {
            if (state == null)
                return false;

            Mobile from = state.From;
            Item tool = state.Tool;

            if (from == null || from.Deleted || !from.Alive)
                return false;

            if (tool == null || tool.Deleted)
                return false;

            if (state.System == null)
                return false;

            if (from.Map != state.StartMap || from.Location != state.StartLocation)
                return false;

            return true;
        }

        private static bool Cancel(Mobile from)
        {
            if (from == null)
                return false;

            return m_States.Remove(from);
        }

        private static void OnMovement(MovementEventArgs e)
        {
            Cancel(e.Mobile);
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
        }
    }
}
