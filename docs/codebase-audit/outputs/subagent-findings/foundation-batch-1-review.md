# Foundation Batch 1 independent review

Reviewed 2026-10-25 against baseline `c747ecd8`. Reviewer scope is read-only production review; this report is the only reviewer-authored file. Root and audit `AGENTS.md` apply. The four pre-existing untracked paths were observed and left untouched. No prior controller, server, client, model, build, or fixture was launched by this reviewer.

**Current status: code/regression review complete; in-game acceptance pending.** The user's later October 25 requirement adds isolated in-game Qwen testing through the existing FIX-01–05 test session before Batch 1 may be called fully accepted or complete. The successful component review and local commit remain valid evidence; they do not fulfill this additional gate. Earlier review conclusions below are limited accordingly.

## Design review: acceptance pending

The bounded FIX-09/FIX-10 plan is appropriate. Preserve FIX-06 through FIX-08 as the distinct historical recommendations in `docs/skill-cap-investigation/REPAIR_RECOMMENDATIONS.md`; do not credit neighboring RB dispositions for these newly confirmed defects. Static design review does not accept implementation or runtime behavior.

### R1: include the direct `ConsumeUpTo` mutation

`Data/System/Source/Items/Container.cs:831-885` has a second shared negative-deduction defect. `ConsumeUpTo` forwards a negative request to `RecurseConsumeUpTo`, which computes negative `need`, increases `item.Amount` directly, and returns negative consumption. An `Item.Consume` guard cannot intercept this path. Add a negative-request return of zero before traversal and test that direct/nested stacks, totals, and return value are unchanged. This fits the approved shared deduction scope. Otherwise explicitly exclude this API from protection claims.

### R2: validate the complete request before any callback

The array overloads at `Container.cs:431`, `:538`, `:654`, and `:720` must scan all requested amounts before group discovery, callbacks, or deductions. A negative later entry must not follow consumption of an earlier valid entry. Preserve the existing failure-index contract (`-1` means success), existing argument-shape exceptions, and existing zero behavior. Group callbacks are user code too: negative requests should not invoke `CheckItemGroup` or `OnItemConsumed`.

`Item.Consume(0)` currently still reaches deletion for an already empty stack; `ConsumeTotal(type, 0)` returns false for an empty container but can invoke a zero-count callback with a matching item. Do not normalize these behaviors accidentally while adding a negative-only guard.

### R3: reject the whole invalid vendor quote before purchase

`BaseVendor.ProcessSinglePurchase` at `Data/Scripts/Mobiles/Base/BaseVendor.cs:2524-2555` and resale accumulation at `:2727` need wide arithmetic checked against the existing integer payment range. Invalid cost after an earlier valid request must invalidate the entire quote; simply omitting an overflowing entry would partially fulfill a malformed order. Reject duplicate serials before all debit/delivery paths, including staff's free-purchase branch. Pin tests to the actual `OnBuyItems` production method, not only an arithmetic helper.

The same method computes `bii.ControlSlots * amount` as an unchecked integer at `:2543`. Use wide arithmetic or otherwise reject invalid slot products while editing this quote boundary, preserving the existing insufficient-slot partial-purchase behavior. Ordinary zero-price policy should remain unchanged unless a separate approval changes it. `GenericBuy.Price` and generic sell-price calculation are upstream policies and should not be described as globally repaired by a final-cost guard.

### R4: freeze combined build and fixture inputs

`scripts/Test-ChampionRewards.ps1` copies scripts before building the core directly from the working tree. The reusable preflight should build the copied core and compile copied scripts/config/references, then compile each selected fixture against that exact core/script assembly pair. Record hashes and assert they still match before running fixtures and before packaging the core. Record expected marker, exit code, timeout/forced-termination state, and exact log path. A missing success marker must fail even when exit code is zero.

Classify historical defect-characterization separately from current regressions; do not rewrite `tests/SkillCapInvestigation.cs` historical expectations or execute it as a current acceptance gate. Invalid fixture API/reference and stale-snapshot tests must fail before any copied-world/client setup. A reused baseline build must be explicitly labeled historical, never current candidate evidence.

## Required acceptance evidence

- Negative direct item, all six `ConsumeTotal`/grouped implementations, both grouped array shapes, and `ConsumeUpTo` (direct/nested) leave quantities/totals and callbacks unchanged; mixed arrays fail at the invalid index before earlier deductions.
- Zero behavior, exact exhaustion, multi-stack deductions, positive mixed requests, grouped requests and a representative crafting call retain behavior.
- Negative bank withdrawal is false and leaves gold/checks untouched; zero and successful gold/check withdrawals remain compatible.
- Actual vendor requests cover negative price, per-entry overflow, aggregate overflow, repeated serials, duplicates after a valid entry, insufficient funds, ordinary success, stock clamping and control-slot limits. Rejected requests preserve stock, gold and delivered-object counts. Staff legitimate purchases still succeed; malformed staff quotes still fail.
- RuntimeScriptCompileTruth: forced isolated runtime compile succeeds. Operational core build is from the same frozen source snapshot; no serialized methods/layouts change. Matching production core hash is recorded and packaged with source only after no production process is found using the tracked executable.
- Tooling negative tests prove wrong API, wrong reference, stale inputs, absent marker and timeout fail with stage/reason. Current component regressions execute against exact assemblies; historical reproductions are reported separately.

## Candidate and evidence review: code/regression review complete

The final production diff implements R1-R3 without changing serializers, type identity, timer semantics, prices, or skill/origin policy. Every negative amount-array guard precedes discovery/callbacks/deductions; `ConsumeUpTo` is also guarded. Vendor duplicate/negative-request validation precedes stock lookup and staff/payment/delivery branches. Ordinary stock and resale costs use the same wide arithmetic boundary; an invalid quote rejects the whole request. Control-slot multiplication is wide while ordinary insufficient-slot handling remains local to that stock line.

R4 is satisfied by copied production source/config/reference inputs, isolated Release/x86 build and forced `-compileonly -nocache`, followed by fixture compilation against the exact generated assemblies. Fixture attempts now preserve unique copied sources/identity files and numbered logs, including failed attempts. Current and historical fixtures have distinct manifest classifications and markers. Existing external accepted Food/accounting fixtures are copied unchanged and hashed.

Independent inspection verified all **6,730** manifest input hashes against both candidate001 and the current production checkout, all three assembly hashes, and final payment/Food/accounting fixture copies against their named originals. The core SHA-256 is `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`; generated script assembly SHA-256 is `5782add05d0fbebe631a213ed4617e1c0f9b63aa99f7ca94cefabc8f5624e059`.

Evidence roots under `C:/Users/nepht/AppData/Local/Temp/`:

- `confictura-foundation-20261025-baseline`: isolated operational build, forced runtime script compile, and `payments-historical-run.log` with **21 historical assertions**. This is reproduction evidence, not candidate acceptance.
- `confictura-foundation-20261025-preflight`: `selftest.json` and separate result/log files demonstrate five expected failures: API mismatch, wrong reference, stale snapshot, absent marker and timeout. The timeout is explicitly forced termination.
- `confictura-foundation-20261025-batch1-001`: operational build and runtime compile passed. Final `payments-run-attempt-2.log` reports **74 assertions**, with fixture identity `payments-286ce731.identity.json`; fixture SHA-256 `52b45cf0e5c923486812acc144aac38bacd7315d72ec2d3586eb983ed15abc36`. It covers all six positive/negative total/grouped paths, nested up-to, zero/exact/multiple-stack behavior, gold/check bank cases, real ordinary/resale purchases, per-line and aggregate overflow, duplicate/negative requests, stock clamping, bank payment, staff free purchase and slot overflow. Expanded tests resolved the initial coverage gap.
- Same candidate: Food **1,853**, accounting **3,972**, champion **679**, Research **46** assertions passed against the candidate assemblies. Successful processes exited normally, with exit zero and required markers. Payment fixture compile attempt 2 failed on an invalid `Serial` constructor call; its log is retained. Attempt 3 corrected that test-only call to the existing cast API and passed. No production patch was required for this harness failure.

`git diff --check` passed. No unresolved material production or component-review finding remains. This approved the exact reviewed candidate for the Batch 1 source-plus-matching-executable local commit, subsequently delivered as `d9b4703f`. The later in-game Qwen acceptance requirement remains pending; the local commit does not mark the entire batch complete or authorize deployment.

### Limits

This is no-save/no-listener component and production-runtime-compiler evidence. No live shard, real client, copied full-world save/load, production balance reachability, upstream price-scalar policy, arbitrary callback rollback, or all serialized properties are certified. All source and tooling observations are independent review of recorded evidence; the reviewer did not launch tests or own runtime processes. Historical FIX-01 through FIX-08 and unrelated audit dispositions remain unchanged by this review.
