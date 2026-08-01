using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura
{
    public static class TurnBasedCombatSelfTest
    {
        public static bool Run(out string report)
        {
            List<string> failures = new List<string>();
            TurnBasedCombatManager manager = TurnBasedCombatManager.Instance;

            Assert(manager != null, "Runtime manager is registered.", failures);

            if (manager != null)
            {
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
