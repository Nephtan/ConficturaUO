# Runtime performance repairs: committed source and verification

Date: 2026-10-17 (America/Chicago)
Batch: POST-PERF-20261017
Source commit: 0eb2a07a5799a4c1755588ecacb321623989827a
State: four reviewed repairs committed locally. The user will push. This record does not establish live deployment or a live restart.

## Scope and authorization

The user approved applying all four reviewed repairs after isolated testing, then explicitly requested committing them and updating the backlog and reports. Only five production C# files changed. No live process, production saves, client settings, original Resident/Shardmind files, or deployment configuration was changed. The pre-existing untracked Confictura-backlog-review-2026-10-17.html remains user-owned and excluded from both commits.

The initial branch was main at 6770279ce3096c5f7d968ccddc56261ddcf0ba25. Each reviewed patch passed git apply --check. The five applied files matched the reviewed candidates after line-ending normalization; no source changes were introduced during this application. Serializer method bodies remained identical. There were no overlapping pre-existing tracked edits.

## Repairs and backlog interpretation

| New active backlog ID | Repair | Source and boundary |
| --- | --- | --- |
| POST-PERF-20261017-01 | Create Food rejected-output containment | CreateFoodSpell.PlaceConjuredItem and OnCast in Data/Scripts/Magic/Magery/Magery 1st/CreateFood.cs. Each item uses normal backpack-only placement; rejected output is deleted; successful food and water remain independent; feedback reflects complete, partial or failed delivery. Normal costs, checks and completion are preserved. |
| POST-PERF-20261017-02 | StatusPage wanted-report batching | StatusPage.WriteWantedNotices and OnTick in Data/Scripts/System/Misc/Logs.cs. Build the report in memory and publish once on the normal path while retaining legacy order, physical-line window, and notifications. |
| POST-PERF-20261017-03 | Liars Dice state/event ownership | Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs and DiceCommand.cs. One shared item-game state and a distinct command-game state; idempotent initialization bounds duplicated event subscriptions and recurring timers. |
| POST-PERF-20261017-04 | Liars Dice turn lifetime and departure indices | Data/Scripts/Items/Misc/Games/LiarsDice/DiceChannel.cs. Retained timer handles and generation checks invalidate old turn callbacks; membership changes maintain first/current-player indices. |

These are four newly recorded post-audit findings, not four newly reclassified historical Phase 13 rows. Their initial HistoricalStatus is Open, meaning open when discovered in this post-audit work; they have no matching Phase 13 predecessor. ActiveStatus Fixed means repaired source with verification and commit evidence, not deployment confirmation.

The original 6,815 active records retain every field and status. The active CSV and workbook now contain 6,819 records and 952 Fixed records (previously 948). All other status counts are unchanged: ReviewedNoChange 2,650; FalsePositive 330; SafeNoChange 678; IntentionalLegacy 1,395; AcceptedFence 83; Documented 352; BlocksOnlyThisDomain 7; PolicyResolvedNoChange 372. The Phase 13 repair-backlog.csv and phase-13-repair-backlog.csv remain unchanged historical evidence.

RB-01742 and RB-01743 were already Fixed for earlier Dice guard repairs; this work does not count them twice or rewrite those repairs. RB-06509 remains Documented for the separate online.txt schema finding. Create Food containment does not close broader FIX-05 networking/queue recommendations. Existing world piles and deliberate dropping remain outside this repair.

Machine-readable review: [post-batch-runtime-performance-review.csv](post-batch-runtime-performance-review.csv). Current overlay: [post-audit-active-backlog-status.csv](post-audit-active-backlog-status.csv) and [workbook](post-audit-active-backlog-status.xlsx).

## Fresh verification of the applied main-source files

All generated build/test output is outside this repository under D:/ConficturaUOTestTools/application/20261017-2312.

| Check | Result and limits |
| --- | --- |
| Release/x86 source executable build | Passed using installed Visual Studio 2022 MSBuild on an exact copied Data/System/Source/Server.csproj and source tree. Output and intermediate directories were inside the test root. |
| RuntimeScriptCompileTruth | The fresh executable passed -service -compileonly -nocache against copied main Data/Scripts source, excluding isolated TestOnly scripts. The service console log confirms the forced compile success; a verifier initially looked at redirected stdout instead of that service log, then checked the actual log. This was a log-location check correction, not a compile failure. |
| Create Food production-method fixture | 1,853 assertions passed: count/weight limits, absent backpack, partial delivery, stacking, normal/race output, repeated rejection, inventory preservation and spell costs/checks/completion. |
| StatusPage compatibility fixture | 29,907 assertions passed, including retained report semantics. |
| Dice production-method and old-save fixture | 45 assertions passed, including loading the preserved version-0 item payload. Separate real timer-chain (3), stale callback (4), and first-player departure (1) assertions passed. The two timer workloads each ran about 35 seconds. |
| Research logout regression | Existing deployed repair remained present; all 46 regression assertions passed. No Research repair was reimplemented. |
| Full copied-world startup | A bounded -service -nocache -profile startup reached world load/Initialize/server-started with 737,263 items and 65,638 mobiles, listening only on 127.0.0.1:24506. Three Dice items resolved to the two expected shared game states and two handlers per relevant event. |
| Isolation and cleanup | The startup process was stopped; no clients or AI were started for this application verification. All 20 protected copied-save files remained byte-identical to the multiplayer-ready fixture. Main source and isolated repaired source matched; seven existing isolation overlays and five TestOnly scripts remained only in the test copy. |
| Source compatibility/static review | Reviewed patch equivalence, unchanged serializer bodies, intended five-file staged diff and git diff --check passed. No moves, new serialized types, project includes or save migration. |

IDEProjectHygiene: solution/Scripts.csproj builds were not repeated because no project/source inclusion changed. Runtime compilation was independently verified as described above.

Reproduction entry points in the application directory: tools/verify_main.py (fresh build, forced compile, harness runs) and tools/startup_check.py (protected full-world startup). Inspect their fixed paths and ports before rerunning; do not aim them at production. Evidence: applied-source-verification.json, build-inputs.json, validation-results.json, startup-validation.json, source-commit.json, evidence/*.log, and startup/. The compile success is also captured in evidence/runtime-compile-service-console.log.

## Workbook preservation and verification

The workbook update preserves its three sheets, eight native tables, four charts, existing source filter, styles, print settings, workbook views, defined print title, add-in metadata and shared-formula encoding. Only the new source rows, table/formula ending-row bounds, three scope labels and affected cached summary/chart values change. The current filter excludes Fixed; clear it to display the four appended Fixed records.

Artifact Tool authored and recalculated a candidate. Independent comparison found exporter metadata loss and calculation limitations for existing mixed-date summaries, some shared dependencies and HYPERLINK. The delivered workbook preserves the original native package and formulas, transfers the verified changes, and uses independently checked CSV-derived summary values where that engine was unsupported or incorrect. The original 6,815 source records and styles remain unchanged. Excel is instructed to perform a full refresh when opened. No native Excel application round-trip was performed; structural preservation and all nine reconciliation/completeness checks were independently verified. Evidence and the preservation helper are under the external application directory in workbook-preservation-validation.json and tools/preserve_workbook.py.

## Real-client and AI evidence carried forward

No new real-client acceptance was performed during main-source application. The applied production files are identical to the accepted candidates, so the prior evidence remains applicable with its original limits.

Create Food evidence includes 14 deterministic real Player-client casts and three bounded local-AI casts in the isolated environment. Ordinary Player access was checked server-side; an Owner character was only used for trusted setup.

The multiplayer supplement at D:/ConficturaUOTestTools/continuation/multiplayer-20261017 covers two real Player characters, Nephtest and Testneph, using independent client copies. It exercised joins, acceptance without unwanted deadline/stake reset, truthful and false bluff challenges with exact settlements, idle warning/kick, abrupt TCP disconnect, same-character reconnect and normal Exit/Yes. A 35.775-second post-reconnect observation made 29 invariant checks for duplicate awards, renewed membership and re-freezing. Six local-Qwen choices were bounded; inaccurate explanations and observation discrepancies were recorded as feedback, not training claims.

The supplement contains reports/confictura-review-with-multiplayer.html, reports/REPRODUCE-AND-ROLLBACK.txt, evidence/multiplayer-client.jsonl, evidence/package-validation.json and reviews/multiplayer-fixture-and-radio-review.md. Original inventories/banks and Frozen states were restored and temporary objects deleted. Use fixtures/multiplayer-ready for the preserved two-player world; the older player-ready fixture predates the second account.

Remaining acceptance limits: two loopback Players only; no production-scale load, graceful in-client logout, active-stake crash recovery, actual movement immobilization proof, or autonomous Dice strategy certification. Client-journal arrival times are not exact callback timing.

## What performance improvement is supported

Create Food provides a behavioral containment guarantee within the verified delivery paths: rejected items add no ground output and leave no surviving undelivered temporary items. The repair does not remove existing piles or establish a global performance gain.

StatusPage starts five seconds after initialization, then every 60 seconds (Logs.cs, StatusPage.Initialize/constructor). Timer.Slice invokes OnTick directly; Main.cs calls Timer.Slice before MessagePump.Slice on the main server loop. Repeated report filesystem work therefore creates a periodic pause on that loop. Account enumeration remains.

The matched timing study at D:/ConficturaUOTestTools/continuation/20261017-2036/reports/measurement-summary.json used 1,001 real copied accounts, 141 wanted entries, no connected clients/model, one warm-up and five measured runs per revision. Baseline median 218.3771 ms (197.121-262.8284) became 11.6473 ms (7.1035-13.0078). The output remained 7,474 identical bytes. This is approximately 207 ms less report work once per minute, about 0.35% of one thread's elapsed-minute budget in that fixture. It does not mean the whole shard runs 19 times faster. Save-time ranges overlapped; a save-speed gain and real-player latency gain were not established.

Dice state/subscriber/timer counts were structurally bounded: in the 1,000-item probe, shared item-game ownership replaced per-item state growth. Turn cancellation tests reject obsolete effects. Savings in the live world depend on item population and actual play. Other event/timer and world-search candidates remain investigations/proposals unless separately authorized.

## Rollout, rollback and next work

The source commit is 0eb2a07a5799a4c1755588ecacb321623989827a. The audit/documentation follow-up commit can be found with git log -- docs/codebase-audit/outputs/post-batch-runtime-performance-closeout.md. Neither commit was pushed by the agent. No live process was restarted; pushing through the user's deployment workflow is a separate step and running scripts must actually reload through that workflow before live behavior is confirmed.

Before live activation, preserve the normal production backup and verify the deployment/restart workflow. After activation, observe a normal and full-backpack Create Food cast, wanted report output/notifications and two-player Dice settlement/timeout/disconnect behavior. Compare real workload timings before claiming production speedups. These are rollout observations, not unfinished local verification gates.

Rollback preserves save compatibility: the five prior source files and all edited audit files were saved under D:/ConficturaUOTestTools/application/20261017-2312/rollback with path-relative layout and hashes in before.json and audit-before.json. If the commit has been deployed, review and revert the source commit through the normal Git/deployment workflow; do not reset unrelated work or replace live saves with test saves. Keep the audit history and record any rollback disposition separately. The preserved reviewed patches are under application/20261017-2312/patches.

No remaining blocker for this defined source application and local verification scope. The live push/activation belongs to the user, and production load/latency remains unmeasured.
