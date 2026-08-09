using System;
using System.Collections.Generic;
using Server.Items;

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
                    manager.Configuration.ActivationMode == TurnBasedCombatActivationMode.PvPOnly,
                    "Activation mode is PvP-only.",
                    failures
                );
                Assert(
                    Math.Abs(manager.Configuration.ActorSeconds - 5.0) < 0.001,
                    "Actor interval is five seconds.",
                    failures
                );
                Assert(manager.Configuration.PlayerTimeoutSeconds == 30, "Player timeout is 30 seconds.", failures);
                Assert(manager.Configuration.DisconnectGraceSeconds == 300, "Disconnect grace is five minutes.", failures);
                Assert(manager.Configuration.EscapeRange == 18, "Escape range is 18 tiles.", failures);
                Assert(manager.Configuration.NpcJoinRange == 12, "NPC join range is 12 tiles.", failures);
                Assert(manager.Configuration.DisengageRange == 18, "Disengage range is 18 tiles.", failures);
                Assert(manager.Configuration.RequireJoinLineOfSight, "NPC joining requires line of sight.", failures);
                Assert(manager.Configuration.EffectRuleCount >= 8, "Effect catalog is loaded.", failures);
                Assert(manager.Configuration.AIRuleCount >= 7, "AI catalog is loaded.", failures);

                TurnEffectRule actorClockPoisonRule = new TurnEffectRule();
                actorClockPoisonRule.RuntimeType = typeof(PoisonImpl.PoisonTimer).FullName;
                actorClockPoisonRule.ClockPolicy = "ActorClock";
                TurnEffectRule wallClockPoisonRule = new TurnEffectRule();
                wallClockPoisonRule.RuntimeType = typeof(PoisonImpl.PoisonTimer).FullName;
                wallClockPoisonRule.ClockPolicy = "WallClockUnaffected";

                Assert(
                    TurnBasedCombatManager.ClassifyPoisonTimerChange(null, null)
                        == TurnEffectTimerChangeDisposition.Removed,
                    "A null poison timer is classified as effect removal.",
                    failures
                );
                Assert(
                    TurnBasedCombatManager.ClassifyPoisonTimerChange(
                        typeof(PoisonImpl.PoisonTimer),
                        actorClockPoisonRule
                    ) == TurnEffectTimerChangeDisposition.ActorClock,
                    "A cataloged poison timer uses the actor clock.",
                    failures
                );
                Assert(
                    TurnBasedCombatManager.ClassifyPoisonTimerChange(
                        typeof(PoisonImpl.PoisonTimer),
                        wallClockPoisonRule
                    ) == TurnEffectTimerChangeDisposition.WallClock,
                    "A cataloged wall-clock poison timer remains native.",
                    failures
                );
                Assert(
                    TurnBasedCombatManager.ClassifyPoisonTimerChange(
                        typeof(Timer),
                        actorClockPoisonRule
                    ) == TurnEffectTimerChangeDisposition.Unknown,
                    "An unsupported non-null poison timer remains fail-closed.",
                    failures
                );
                Assert(
                    TurnBasedCombatManager.ClassifyPoisonTimerChange(
                        typeof(PoisonImpl.PoisonTimer),
                        null
                    ) == TurnEffectTimerChangeDisposition.Unknown,
                    "An uncataloged non-null poison timer remains fail-closed.",
                    failures
                );

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
                false,
                true,
                true
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
                false,
                true,
                true
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
                true,
                true,
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
                true,
                true,
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
                false,
                true,
                true
            );
            Assert(
                outOfTurnIntent == TurnCombatantIntentDisposition.Reject
                    && TurnBasedCombatManager.GetCombatantChangeMode(outOfTurnIntent)
                        == TurnCombatantChangeMode.Reject,
                "Out-of-turn combatant selection is rejected.",
                failures
            );

            TurnCombatantIntentDisposition ordinaryPvE = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                false,
                false,
                false,
                false,
                false,
                true
            );
            Assert(
                ordinaryPvE == TurnCombatantIntentDisposition.Native,
                "Ordinary PvE remains native when no two player-controlled sides are present.",
                failures
            );

            TurnCombatantIntentDisposition distantNpc = TurnBasedCombatManager.ClassifyCombatantIntent(
                true,
                false,
                true,
                false,
                false,
                false,
                false
            );
            Assert(
                distantNpc == TurnCombatantIntentDisposition.Reject,
                "An out-of-bound outsider cannot join an active PvP group.",
                failures
            );

            Assert(
                TurnBasedCombatManager.IsJoinWithinBoundary(true, true, true, true),
                "An in-range visible NPC may join through hostile intent.",
                failures
            );
            Assert(
                !TurnBasedCombatManager.IsJoinWithinBoundary(true, false, true, true)
                    && !TurnBasedCombatManager.IsJoinWithinBoundary(true, true, false, true),
                "NPC join range and line of sight are both enforced.",
                failures
            );
            Assert(
                TurnBasedCombatManager.ShouldPruneEdge(false, true, true)
                    && TurnBasedCombatManager.ShouldPruneEdge(true, false, false)
                    && !TurnBasedCombatManager.ShouldPruneEdge(true, false, true),
                "Edges prune across maps or only when both distant and unseen.",
                failures
            );

            bool[,] splitGraph = new bool[4, 4];
            splitGraph[0, 1] = splitGraph[1, 0] = true;
            splitGraph[2, 3] = splitGraph[3, 2] = true;
            Assert(
                TurnBasedCombatManager.CountConnectedComponents(splitGraph) == 2,
                "Disconnected hostility graphs are classified into separate components.",
                failures
            );

            TimeSpan nextTick;
            int dueTicks = TurnBasedCombatManager.ComputeDueTickCount(
                TimeSpan.FromSeconds(2.0),
                TimeSpan.FromSeconds(5.0),
                TimeSpan.FromSeconds(2.0),
                out nextTick
            );
            Assert(
                dueTicks == 2 && nextTick == TimeSpan.FromSeconds(1.0),
                "Actor-clock regeneration processes every exactly due tick.",
                failures
            );
            Assert(
                BandageContext.AdvanceTurnBasedRemaining(
                    TimeSpan.FromSeconds(7.0),
                    TimeSpan.FromSeconds(5.0)
                ) == TimeSpan.FromSeconds(2.0)
                    && BandageContext.AdvanceTurnBasedRemaining(
                        TimeSpan.FromSeconds(2.0),
                        TimeSpan.FromSeconds(5.0)
                    ) == TimeSpan.Zero,
                "Bandage application advances and completes on actor time.",
                failures
            );
            Assert(
                PoisonImpl.PoisonTimer.ComputeRemainingTicks(10, 0) == 11
                    && PoisonImpl.PoisonTimer.ComputeRemainingTicks(10, 11) == 0,
                "Poison exposes every remaining damage or natural-expiry tick.",
                failures
            );

            DateTime actorTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime wallTime = actorTime.AddHours(8.0);
            Assert(
                TurnBasedCombatManager.ComputeRebasedCooldown(
                    actorTime.AddSeconds(3.0),
                    actorTime,
                    wallTime
                ) == wallTime.AddSeconds(3.0),
                "Cooldown rebasing preserves only actor-relative remaining time.",
                failures
            );
            Assert(
                TurnBasedCombatManager.ClearRunningDirection(Direction.North | Direction.Running)
                    == Direction.North,
                "Turn completion strips the running direction flag.",
                failures
            );

            if (testMobile != null && testMobile.Player)
            {
                Assert(
                    TurnBasedCombatManager.ResolveEffectivePlayerPrincipal(testMobile) == testMobile,
                    "A player resolves to their own effective principal.",
                    failures
                );
                Assert(
                    !TurnBasedCombatManager.IsDifferentPlayerSide(testMobile, testMobile),
                    "The same player principal cannot seed PvP against itself.",
                    failures
                );
            }

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
