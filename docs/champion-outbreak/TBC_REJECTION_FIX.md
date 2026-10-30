# Rejected outsider intent: null-safe logging

The staged presentation001 copy reports a core bridge fault about 10 seconds after the northern pose: TurnBasedCombatManager.DecideCombatantChange throws NullReferenceException and the bridge disables turn-based combat. Its trace is retained at D:/ConficturaUOTestTools/outbreak-presentation-20261029-001/shard/Data/Logs/Console.log. The eastern pose then refuses the already-faulted bridge; no eastern rendering acceptance is claimed. Both observed frames and the failed copy remain retained with acknowledged no-save cleanup.

Source review identifies a real rejection branch: an unjoined actor targeting an existing participant can be rejected when it cannot join, yet the rejection log reads actorParticipant.Group. The actual-manager regression reproduces that null dereference on the accepted old assembly before the fix. The repair logs a null group when no actor participant exists. It changes no eligibility decision, enrollment, AP, movement, damage, NPC ability, reward or serializer. Native chained-scene confirmation is still required before attributing every observed native failure to this branch.

## Verification

- Old candidate reproduction: D:/ConficturaUOTestTools/outbreak-intent-repro-20261029-001, actual manager with a declared synthetic roster and an active outsider outside join range; expected null fault reproduced. Missing body.cfg warning is retained; no rendering or world loading is claimed by that reproduction.
- Corrected candidate: D:/ConficturaUOTestTools/outbreak-intent-fix-20261029-001. Forced production runtime script compilation passes. Actual manager/core bridge reject the outsider without fault or disable, retain the existing group/AP, refuse enrollment, retain missing-actor/clear-target guards and preserve the disabled real-time path. All 92 outbreak web production-method assertions also pass.
- Only production input changed is Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs. Core remains the accepted dbb7b2360c435834b32e0f1f4b0854a859cd9086971a4c270981b58d8f2f3ddb artifact. No core source/executable rebuild is needed for this script-only repair. No serialization or hook registration changes occur. Presentation rerun uses the new compiled candidate; production activation remains disabled.

## Retained evidence and carry-forward limits

The three-run mounted pacing series remains accepted at its frozen baseline. It is not relabeled as newly replayed source: those runs have TBC disabled, the benchmark never enables it, and DecideCombatantChange returns Native before reaching this logging branch when disabled. The corrected regression explicitly checks that guard. Thus no new pacing series is justified by this logging repair. Original real-time Zanthus stress has the same control-flow boundary.

The selected-snapshot offline reservation preflight invokes Configure/Region.Load/World.Load without Initialize or enabling the combat manager. Its terrain/housing/city/portal behavior and serializers are unaffected by this change; its original hashes/report remain intact. Older TBC native escape/dodge results retain their recorded source scope; the corrected manager has current component/bridge checks, with native presentation confirmation pending. Broader NPC AI remains read-only.

## Source traces

- [TurnBasedCombatManager.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs): ClassifyCombatantIntent, CanJoinInteraction, DecideCombatantChange Reject branch and Log. Trigger: native combatant intent from an eligible living actor/target; existing null/deleted/living/join guards remain.
- [TurnBasedCombat.cs](../../Data/System/Source/TurnBasedCombat.cs): DecideCombatantChange catches handler faults and Fail disables the bridge; unchanged core.
- [CombatantIntentRejectionRegression.cs](../../tests/CombatantIntentRejectionRegression.cs): actual manager/bridge, outsider rejection, preserved group/AP and real-time guard. Compiled/run against matching private core/script assemblies; --expect-fault is only the retained old-assembly reproduction.
