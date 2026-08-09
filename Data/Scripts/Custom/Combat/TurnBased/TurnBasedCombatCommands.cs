using System;
using Server.Commands;
using Server.Targeting;

namespace Server.Custom.Confictura
{
    public static class TurnBasedCombatCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("TurnCombat", AccessLevel.Administrator, TurnCombat_OnCommand);
            CommandSystem.Register("EndTurn", AccessLevel.Player, EndTurn_OnCommand);
            CommandSystem.Register("EscapeCombat", AccessLevel.Player, EscapeCombat_OnCommand);
        }

        [Usage("TurnCombat <Status|Compatibility|Inspect|Enable|Disable|Reload|SelfTest|ArmTest|DisarmTest|ForceEnd|ForceDissolve|EndTurn|Escape>")]
        [Description("Administers the dynamic turn-based combat system.")]
        private static void TurnCombat_OnCommand(CommandEventArgs e)
        {
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;

            if (manager == null)
            {
                e.Mobile.SendMessage("The turn-based combat manager is unavailable.");
                return;
            }

            string action = e.Length == 0 ? "Status" : e.GetString(0);

            switch (action.ToLowerInvariant())
            {
                case "status":
                {
                    string gateReason;
                    bool gate = manager.Configuration.CheckCompatibilityGate(out gateReason);
                    bool bridgeReady = TurnBasedCombatBridge.IsHandlerReady(manager);
                    string bridgeStatus;

                    if (bridgeReady)
                        bridgeStatus = "ready";
                    else if (TurnBasedCombatBridge.IsFaulted)
                        bridgeStatus = "faulted - " + TurnBasedCombatBridge.FaultReason;
                    else
                        bridgeStatus = "unregistered";

                    e.Mobile.SendMessage(
                        "Turn combat: {0}; mode: {1}; groups: {2}; test armed: {3}; catalog: {4}; compatibility: {5}; bridge: {6}.",
                        manager.Enabled && bridgeReady ? "enabled" : "disabled",
                        manager.Configuration.ActivationMode,
                        manager.GroupCount,
                        manager.TestArmCount,
                        manager.Configuration.CatalogVersion,
                        gate ? "pass" : "blocked - " + gateReason,
                        bridgeStatus
                    );
                    break;
                }
                case "compatibility":
                {
                    string gateReason;
                    bool gate = manager.Configuration.CheckCompatibilityGate(out gateReason);
                    e.Mobile.SendMessage(
                        "Compatibility {0}; effects {1}; AI rules {2}; {3}",
                        gate ? "passes" : "is blocked",
                        manager.Configuration.EffectRuleCount,
                        manager.Configuration.AIRuleCount,
                        gate ? "runtime hashes match the generated register" : gateReason
                    );
                    break;
                }
                case "inspect":
                {
                    e.Mobile.SendMessage("Target a combat participant to inspect.");
                    e.Mobile.Target = new StaffTarget(StaffTargetAction.Inspect);
                    break;
                }
                case "enable":
                {
                    string reason;

                    if (manager.TryEnable(out reason))
                        e.Mobile.SendMessage("Turn-based combat is globally enabled.");
                    else
                        e.Mobile.SendMessage("Enable refused: {0}", reason);

                    break;
                }
                case "disable":
                {
                    manager.Disable("staff emergency disable by " + e.Mobile.Name);
                    e.Mobile.SendMessage("Turn-based combat is disabled and all groups were dissolved.");
                    break;
                }
                case "reload":
                {
                    string reason;

                    if (manager.Reload(out reason))
                        e.Mobile.SendMessage("Turn-based combat configuration reloaded.");
                    else
                        e.Mobile.SendMessage("Reload refused: {0}", reason);

                    break;
                }
                case "selftest":
                {
                    string report;
                    bool passed = TurnBasedCombatSelfTest.Run(e.Mobile, out report);
                    e.Mobile.SendMessage(passed ? 0x59 : 0x22, report);
                    manager.Log("self_test", null, e.Mobile, report);
                    break;
                }
                case "armtest":
                {
                    e.Mobile.SendMessage("Target one non-player mobile to arm for process-local PvE regression testing.");
                    e.Mobile.Target = new StaffTarget(StaffTargetAction.ArmTest);
                    break;
                }
                case "disarmtest":
                {
                    e.Mobile.SendMessage("Target the process-local PvE regression mobile to disarm.");
                    e.Mobile.Target = new StaffTarget(StaffTargetAction.DisarmTest);
                    break;
                }
                case "forceend":
                {
                    e.Mobile.SendMessage("Target the current actor whose turn should end.");
                    e.Mobile.Target = new StaffTarget(StaffTargetAction.ForceEnd);
                    break;
                }
                case "forcedissolve":
                {
                    e.Mobile.SendMessage("Target any participant in the group to dissolve.");
                    e.Mobile.Target = new StaffTarget(StaffTargetAction.ForceDissolve);
                    break;
                }
                case "endturn":
                {
                    EndTurn(e.Mobile, manager);
                    break;
                }
                case "escape":
                {
                    Escape(e.Mobile, manager);
                    break;
                }
                default:
                {
                    e.Mobile.SendMessage("Unknown TurnCombat action: {0}", action);
                    break;
                }
            }
        }

        [Usage("EndTurn")]
        [Description("Ends your current turn-based combat turn.")]
        private static void EndTurn_OnCommand(CommandEventArgs e)
        {
            EndTurn(e.Mobile, TurnBasedCombatManager.Instance);
        }

        [Usage("EscapeCombat")]
        [Description("Attempts to leave turn-based combat when all hostiles are distant and unseen.")]
        private static void EscapeCombat_OnCommand(CommandEventArgs e)
        {
            Escape(e.Mobile, TurnBasedCombatManager.Instance);
        }

        private static void EndTurn(Mobile mobile, TurnBasedCombatManager manager)
        {
            if (manager == null || !manager.EndTurn(mobile, true))
                mobile.SendMessage("It is not your turn.");
        }

        private static void Escape(Mobile mobile, TurnBasedCombatManager manager)
        {
            string reason = null;

            if (manager == null || !manager.TryEscape(mobile, out reason))
                mobile.SendMessage(reason == null ? "You are not in turn-based combat." : reason);
        }

        private enum StaffTargetAction
        {
            Inspect,
            ArmTest,
            DisarmTest,
            ForceEnd,
            ForceDissolve
        }

        private sealed class StaffTarget : Target
        {
            private readonly StaffTargetAction m_Action;

            public StaffTarget(StaffTargetAction action)
                : base(-1, false, TargetFlags.None)
            {
                m_Action = action;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Mobile mobile = targeted as Mobile;
                TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;

                if (mobile == null || manager == null)
                {
                    from.SendMessage("That is not a mobile combat participant.");
                    return;
                }

                if (m_Action == StaffTargetAction.ArmTest)
                {
                    if (manager.ArmTestMobile(mobile))
                        from.SendMessage("That non-player mobile is armed until disable or restart.");
                    else
                        from.SendMessage("That mobile cannot be armed (it must be a living non-player outside combat).");

                    return;
                }

                if (m_Action == StaffTargetAction.DisarmTest)
                {
                    if (manager.DisarmTestMobile(mobile))
                        from.SendMessage("That mobile is no longer armed for PvE regression testing.");
                    else
                        from.SendMessage("That mobile was not armed.");

                    return;
                }

                TurnParticipant participant = manager.GetParticipant(mobile);

                if (participant == null)
                {
                    from.SendMessage("That mobile is not in turn-based combat.");
                    return;
                }

                if (m_Action == StaffTargetAction.Inspect)
                {
                    from.SendMessage(
                        "Group {0}, round {1}, initiative {2}, eligible round {3}, AP {4}, current {5}, pending {6}.",
                        participant.Group.ID,
                        participant.Group.Round,
                        participant.InitiativeTotal,
                        participant.EligibleRound,
                        participant.ActionPoints,
                        participant.Group.Current == participant,
                        participant.Pending == null ? "none" : participant.Pending.Request.Kind.ToString()
                    );
                }
                else if (m_Action == StaffTargetAction.ForceEnd)
                {
                    if (manager.EndTurn(mobile, false))
                        from.SendMessage("The targeted turn was ended.");
                    else
                        from.SendMessage("That mobile is not the current actor.");
                }
                else if (manager.ForceDissolve(mobile, "staff forced dissolve by " + from.Name))
                {
                    from.SendMessage("The targeted combat group was dissolved.");
                }
            }
        }
    }
}
