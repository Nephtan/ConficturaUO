# FIX-15: real manager actor-effect completion repair

2026-10-26 local / 2026-10-27 UTC. Batch 3 was explicitly resumed. This is an additive repair discovered by real-client acceptance, after the earlier BandageContext ownership commit `143e9fb33c4c56540ea9261f70b267441ab44284`.

## Failure and change

In isolated attempt `batch-3/client-003`, ordinary authenticated Nephtest used and targeted one bandage. The real manager suspended it with 1461ms remaining. One native End Turn button-1 response advanced round 1 to 2, but healing stayed suspended through subsequent rounds. The finite bridge action failed and the strict server check rejected the remaining active context. Cases 0-4 had passed; case 5 failed; cases 6-14 were unrun in that attempt. Full progression equality remained intact. No failed assertion was weakened.

`TurnBasedCombatManager.StartNextActor` calls `ProcessActorEffects` after the previous pending action has ended. Its self `Effect` request was rejected by `AuthorizeMutation`, which requires a pending action for ordinary participant mutations. The core bridge also matches mutation scopes by target, so a self scope alone cannot authorize healing a different captured patient.

`BeginActorEffectMutation` now opens a scope for the exact internal request only, after checking the current participant identity and attached group. The temporary request token is restored in `finally`; arbitrary `Effect` requests gain no permission. Bandage advancement opens a second scope for the captured patient before advancing either remaining clock. Both scopes close on exceptions. Completion clears the captured context only if it is still the same context, preserving a replacement installed by an `OnHeal` callback.

Production source: `Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs`, specifically `AuthorizeMutation`, `BeginActorEffectMutation`, `ProcessActorEffects`, `StartNextActor` and `EndTurn`. Cross-boundary evidence: `Data/System/Source/TurnBasedCombat.cs` (`BeginMutation`, `ScopeMatches`), `Data/System/Source/Mobile.cs` (`Heal`), and `Data/Scripts/Items/Trades/Misc/Bandage.cs` (`AdvanceTurnBased`, `EndHeal`, `StopHeal`). No core source/executable, serialization, project list, global timer, healing formula or production configuration changed.

## Verification at this source commit

External evidence root: `D:/ConficturaUOTestTools/foundation-ingame-20261025/batch-3/`.

- `candidate-748e36ca-001/bandages-run-attempt-4.log`: the new real-manager regression reproduced the exact StartOfTurn health/completion failure before the repair. Attempts 2 and 3 retained incomplete test-group setup failures; they are not defect reproductions.
- `candidate-actor-effects-001/runtime-compile.result.json` and `compile-service-marker.json`: forced runtime script compile with `-service -compileonly -nocache`, normal exit 0 and the service-log completion marker. This compiles production scripts; it is not a world-start or client acceptance claim.
- `candidate-actor-effects-001/repair-verification.json`: all seven component groups passed, 6,995 assertions (bandages 110, actions 261, payments 74, Food 1,853, accounting 3,972, Research 46, champion 679). Every process exited normally. The bandage fixture retains known diagnostics from deliberately unconnected capture-socket teardown; sockets never listen or connect.
- `tests/FoundationBandageRegression.cs` now registers the real enabled manager, uses the production StartOfTurn rule and real EndTurn transitions with no pending action, and verifies actual self/other health changes, exactly one completion, stale queued callback inertness, external self/other/outsider refusals, stale/out-of-turn participant refusal, callback replacement preservation, and exception-safe scope disposal. Earlier always-authorizing bridge tests remain separate coverage.

Candidate manager canonical SHA-256 `0463fd3d4fadf430195d8efbb61f694c46730ae89da9e711c518302196c648da`; regression `2e1f7e508495b780518984777d22d6685665dd1a8cbcf4c5f8e3f9c206e56dc6`; production scripts `f8ccb45a485db7c8787b0be6945e05217cbdc8aae874dabe9dc473d2298fd174`. The unchanged tracked core is `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`; all 124 core inputs retain the accepted build correspondence, so a new core build is not required. Optional IDE/project hygiene builds were not run because no project inclusion changed.

Failed attempt 003 was restored online, then its owned server/client were forcibly stopped at the milestone (not graceful shutdown). Resident/model closed normally. Full copied Saves/profiles and all 14 protected inputs verified restored/unchanged; process identities were absent and ports free. Executable/cache/evaluator were disarmed with exact hashes retained. See `client-003/RETAINED-FAILURE.md`, `evidence/cleanup-result.json`, and its disarm record. Earlier attempts 001/002 and their stat-cap fixture diagnosis remain preserved separately.

The repaired candidate still requires a fresh full native/guarded-bridge/Qwen run, final independent review, tracker/workbook reconciliation and acceptance commit. This source commit does not close FIX-15 or claim deployment. The sole controller remains active.
