# Foundation Batch 1: FIX-09 and FIX-10

Scope: shared negative deductions, vendor quote arithmetic/duplicates, and a thin component preflight. No serializer, price, global timer, skill/origin policy, namespace or project include changes.

## Repairs

- `Data/System/Source/Item.cs`, `Item.Consume(int)`: negative input is a no-op.
- `Data/System/Source/Items/Container.cs`: all six total/grouped implementations reject negative requests before callbacks; arrays return the invalid index before any deduction. `ConsumeUpTo` rejects negative input before its independent direct amount mutation. Existing zero behavior is preserved.
- `Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs`, `Withdraw`: negative input returns false before balance inspection/deduction.
- `Data/Scripts/Mobiles/Base/BaseVendor.cs`, `OnBuyItems`, `ProcessSinglePurchase`, `TryAddPurchaseCost`: reject duplicate serials and negative quantities before quoting; use wide cost/sum and control-slot arithmetic; reject invalid stock/resale quotes before payment, including staff orders. Existing successful price, stock clamp, delivery fallback, partial follower-slot policy and staff free purchase remain.
- `scripts/Test-Foundation.ps1` / `foundation_fixture_runner.py`, `tests/foundation-fixtures.json`: freeze combined source/config/reference snapshot; build operational core and force runtime compile; compile every selected fixture against those exact assemblies before execution. Source/assembly/fixture hashes and process results are recorded. Repeated attempts have distinct logs and copied fixtures. Historical cases require explicit selection and cannot serve as current regression gates. Existing wrappers remain unchanged.

## Measured evidence

All new evidence is under `C:/Users/nepht/AppData/Local/Temp/`, using the read-only client data and accepted fixtures in `D:/ConficturaUOTestTools`. No prior controller was restarted.

| Evidence root / case | Result |
| --- | --- |
| `confictura-foundation-20261025-baseline` | Operational Release/x86 build and forced `-compileonly -nocache`; 21 historical assertions reproduce negative direct/total/grouped/up-to/bank defects, duplicate overcharge and overflow-to-negative payment. Historical results are not acceptance. |
| `confictura-foundation-20261025-preflight` | All five deliberate failures detected: API mismatch, missing reference, stale snapshot, missing marker, timeout. Only the deliberate timeout child was forcibly terminated; process exit recorded. |
| `confictura-foundation-20261025-batch1-001` | Release/x86 core build and forced runtime script compilation passed. Final `payments-run-attempt-2.log`: 74 assertions, exact final fixture identity `payments-286ce731.identity.json`. |
| Same candidate, accepted regressions | Create Food 1,853; Research logout 46; champion rewards/display 679; FIX-04 accounting 3,972. Each compiled against the same core/scripts assemblies and exited normally with the expected marker. |

Payment coverage includes all six negative and positive total/grouped paths, whole-array no-mutation/callback rejection, nested up-to, zero/exact/multiple-stack deductions, grouped crafting-shaped consumption, gold/check bank withdrawals, normal/bank/staff purchases, zero cost, stock clamp, insufficient funds, negative cost/quantity, duplicate serials, distinct-entry aggregate overflow, resale branch and wide follower-slot refusal. It does not prove arbitrary callback rollback, all economic balance accumulation or every crafting recipe.

Retained fixture failure: `payments-compile-attempt-2.log` rejected a test's invalid `Serial(int)` constructor. Corrected to the supported cast; compile attempt 3 and run attempt 2 passed. Earlier 72-check evidence remains separate. No production change was needed for that harness error.

## Artifact correspondence and cleanup

Complete 125 core inputs match the frozen candidate and repository; full snapshot has 6,730 inputs. Manifest SHA-256 `24aeae30d6601051e3cef04f43780c8717371bd0ca1c0c1722e51c092205d95f`. Matching Release/x86 tracked `ConficturaServer.exe`: `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`. Runtime scripts: `5782add05d0fbebe631a213ed4617e1c0f9b63aa99f7ca94cefabc8f5624e059`. See `inputs.json`, `assemblies.json`, fixture identities and `matching-core-delivery.json` in candidate001.

Host CIM inspection at 2026-10-25T14:19:56Z found no Confictura/RunUO process or process using the root executable before replacement. Initial sandbox process visibility was restricted and was not used as host-wide proof. The packaged executable is byte-identical to the tested artifact and contains no fixture overlay. All test processes exited; no saved world, account, client, Resident or model was opened. There is no copied-world fixture to restore. Original saves/configuration were not written.

RuntimeScriptCompileTruth passed; this compile-only path returns before world load/listening. IDE/project builds were not run because no project entries changed. Full-world save/load, real Player/native UI, Qwen, deployment and live behavior were not tested in this bounded component batch. The shared core and affected callback behavior are proven only by the stated production-method fixtures and prior regressions.

Independent review: [foundation-batch-1-review.md](subagent-findings/foundation-batch-1-review.md). Final acceptance and local commit are recorded in the sequence register after review. `git diff --check` passed; only intended files and the matching executable are staged. Historical recommendations/dispositions and the four unrelated untracked paths remain preserved.
