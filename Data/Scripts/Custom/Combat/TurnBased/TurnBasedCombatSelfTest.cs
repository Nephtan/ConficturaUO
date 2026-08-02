using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura
{
    public static class TurnBasedCombatSelfTest
    {
        public static bool Run(Mobile testMobile, out string report)
        {
            List<string> failures = new List<string>();
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;

            Assert(manager != null, "Runtime manager is registered.", failures);
            Assert(
                manager != null && TurnBasedCombatBridge.IsHandlerReady(manager),
                TurnBasedCombatBridge.IsFaulted
                    ? "Core bridge is faulted: " + TurnBasedCombatBridge.FaultReason
                    : "Core bridge is not registered to the runtime manager.",
                failures
            );

            if (manager != null)
            {
                Assert(manager.GetParticipant(null) == null, "Null participant lookup is safe.", failures);

                bool actorlessMutationCompleted = true;

                if (testMobile != null)
                {
                    try
                    {
                        manager.AuthorizeMutation(
                            new TurnMutationRequest(
                                null,
                                testMobile,
                                TurnMutationKind.Hits,
                                testMobile.Hits,
                                testMobile.Hits
                            )
                        );
                    }
                    catch
                    {
                        actorlessMutationCompleted = false;
                    }
                }

                Assert(
                    actorlessMutationCompleted,
                    "Actorless state-mutation authorization is null-safe.",
                    failures
                );

                Assert(manager.Configuration.ActionPoints == 20, "Default AP is 20.", failures);
                Assert(
                    Math.Abs(manager.Configuration.ActorSeconds - 5.0) < 0.001,
                    "Actor interval is five seconds.",
                    failures
                );
                Assert(manager.Configuration.PlayerTimeoutSeconds == 30, "Player timeout is 30 seconds.", failures);
                Assert(manager.Configuration.DisconnectGraceSeconds == 300, "Disconnect grace is five minutes.", failures);
                Assert(manager.Configuration.EscapeRange == 18, "Escape range is 18 tiles.", failures);
                Assert(manager.Configuration.EffectRuleCount >= 7, "Effect catalog is loaded.", failures);
                Assert(manager.Configuration.AIRuleCount >= 7, "AI catalog is loaded.", failures);

                TurnActionRule recallRule = manager.Configuration.FindActionRule(
                    TurnActionKind.Spell,
                    "Server.Spells.Fourth.RecallSpell"
                );
                Assert(
                    recallRule != null && !recallRule.Allowed,
                    "Recall is explicitly blocked while participating.",
                    failures
                );

                string gateReason;
                Assert(
                    manager.Configuration.CheckCompatibilityGate(out gateReason),
                    gateReason == null ? "Compatibility gate passes." : gateReason,
                    failures
                );
            }

            TurnCombatantIntentDisposition disabledIntent = TurnBasedCombatManager.ClassifyCombatantIntent(
                false,
                false,
                false,
                false,
                false
            );
            Assert(
                disabledIntent == TurnCombatantIntentDisposition.Native
                    && TurnBasedCombatManager.GetCombatantChangeMode(disabledIntent)
                        == TurnCombatantChangeMode.Native,
                "Disabled combatant changes remain native.",
                failures
            );

            TurnCombatantIntentDisposition openingIntent = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                false,
                false,
                false,
                false
            );
            Assert(
                openingIntent == TurnCombatantIntentDisposition.OpenGroup
                    && TurnBasedCombatManager.GetCombatantChangeMode(openingIntent)
                        == TurnCombatantChangeMode.Reject,
                "Two outsiders open initiative without native aggression.",
                failures
            );

            TurnCombatantIntentDisposition joiningActor = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                false,
                true,
                false,
                false
            );
            Assert(
                joiningActor == TurnCombatantIntentDisposition.JoinActor
                    && TurnBasedCombatManager.GetCombatantChangeMode(joiningActor)
                        == TurnCombatantChangeMode.Reject,
                "An outsider joins for next round without completing the opening selection.",
                failures
            );

            TurnCombatantIntentDisposition joiningTarget = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                true,
                false,
                false,
                true
            );
            Assert(
                joiningTarget == TurnCombatantIntentDisposition.JoinTarget
                    && TurnBasedCombatManager.GetCombatantChangeMode(joiningTarget)
                        == TurnCombatantChangeMode.SelectionOnly,
                "The current actor may select and join an outsider.",
                failures
            );

            TurnCombatantIntentDisposition sameGroup = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                true,
                true,
                true,
                true
            );
            Assert(
                sameGroup == TurnCombatantIntentDisposition.Select
                    && TurnBasedCombatManager.GetCombatantChangeMode(sameGroup)
                        == TurnCombatantChangeMode.SelectionOnly,
                "The current actor receives selection-only combat targeting.",
                failures
            );

            TurnCombatantIntentDisposition mergeIntent = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                true,
                true,
                false,
                true
            );
            Assert(
                mergeIntent == TurnCombatantIntentDisposition.MergeGroups
                    && TurnBasedCombatManager.GetCombatantChangeMode(mergeIntent)
                        == TurnCombatantChangeMode.SelectionOnly,
                "The current actor may merge groups through combatant intent.",
                failures
            );

            TurnCombatantIntentDisposition outOfTurnIntent = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                true,
                true,
                true,
                false
            );
            Assert(
                outOfTurnIntent == TurnCombatantIntentDisposition.Reject
                    && TurnBasedCombatManager.GetCombatantChangeMode(outOfTurnIntent)
                        == TurnCombatantChangeMode.Reject,
                "Out-of-turn combatant selection is rejected.",
                failures
            );

            Assert(
                TurnBasedCombatManager.ComputeInitiativeTotal(20, 100) == 30,
                "Initiative uses d20 plus Dexterity divided by ten.",
                failures
            );
            Assert(
                TurnBasedCombatManager.ComputeDurationAP(0.4, 20) == 2,
                "Walking rounds up to two AP.",
                failures
            );
            Assert(
                TurnBasedCombatManager.ComputeDurationAP(0.2, 20) == 1,
                "Fast movement has a one AP minimum.",
                failures
            );
            Assert(
                TurnBasedCombatManager.ComputeDurationAP(3600.0, 20) == 20,
                "Long actions clamp to a full turn.",
                failures
            );

            List<string> parsed = TurnBasedCombatConfiguration.ParseCsvLine(
                "one,\"two,with,commas\",\"three \"\"quoted\"\"\""
            );
            Assert(parsed.Count == 3, "CSV parser preserves field count.", failures);
            Assert(parsed[1] == "two,with,commas", "CSV parser preserves embedded commas.", failures);
            Assert(parsed[2] == "three \"quoted\"", "CSV parser preserves escaped quotes.", failures);

            if (failures.Count == 0)
            {
                report = "PASS: turn-based combat deterministic self-tests completed.";
                return true;
            }

            report = "FAIL: " + String.Join(" | ", failures.ToArray());
            return false;
        }

        private static void Assert(bool condition, string message, List<string> failures)
        {
            if (!condition)
                failures.Add(message);
        }
    }
}
