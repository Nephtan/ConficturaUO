# Investigation and verification record

Date: 2026-10-05. Working directory: `D:\ConficturaUO-main`. Branch: `main`. Baseline: `75bc9dfb`. Initial worktree was clean; no unrelated pre-existing edits were present. Production source, project files, configuration and saves remain unchanged. New work is confined to this documentation directory, two investigation scripts and two fixture source files under `tests`.

## Scope and completion

Required inputs were the confirmed deployed source baseline and the approved investigation plan. Latest saves/logs were explicitly unavailable. Required outputs were a source-backed report, classified mutation/cap-change register, reproducible isolated cases and prioritized recommendations. These are present in this directory. The complete source inventory is reconciled to project truth and compiled mutation/reflection calls; every identified source path has a classification. Saved dynamic actions and incident attribution are recorded as unresolved rather than inferred from old local saves.

Root `AGENTS.md` and instruction scopes were checked with `rg --files -g AGENTS.md`. No nested instruction file applies to the new scripts, tests or this dedicated documentation directory. The separate `docs/codebase-audit` phase controller/status files were not advanced because this work implements the approved focused investigation, not the entire reorganization program. No source edits, live actions or subagent changes occurred.

## Commands and evidence

| Work | Command / method | Result and output |
| --- | --- | --- |
| Baseline | `git status --short`; `git branch --show-current`; `git rev-parse --short HEAD` | Clean `main`, `75bc9dfb` before investigation. |
| Runtime/core coverage | `rg --files Data/Scripts Data/System/Source -g '*.cs' -g '!**/bin/**' -g '!**/obj/**'`; exporter SHA-256 inventory | 6,598 runtime scripts, 120 core files. `source-coverage.csv`. No runtime VB files. |
| Project truth | Exporter XML parses both project files, URI-decodes Include paths and checks literal resolved paths | Zero omitted runtime scripts; zero missing project includes. `runtime-scripts-omitted-from-project.csv`, `coverage-summary.json`. |
| Skill source review | Broad `rg` searches for base/fixed-point/cap/total/Player/component writes, aliased `Skill` variables, constructors/readers, callbacks, XML actions, reflection and origin/progression callers | Classified routes in README, route register and reviewed families. Compiled sink discovery avoids reliance on textual syntax. |
| Reflection closure | Searches for `SetValue`, `SetValueDirect`, `GetSetMethod`, `InvokeMember`, `GetField(s)`, `CreateDelegate`, `DynamicMethod`, raw backing fields and `ref/out` use | All live skill-relevant writes route through registered direct or reflection families. Documentation-only/commented uses distinguished. No separate by-reference base/cache/cap mutation found. |
| Region closure | Compiled Region types + declared OnEnter call chains; source review of Region.OnRegionChange, movement/map/load and each exception | 47 region types; 40 handlers reach verifier, 7 do not. Newly entered parent may independently verify. `region-entry-register.csv`. |
| Text config/data | `python scripts/Export-SkillCapInvestigation.py <run-directory>` with the expression saved in coverage-summary | 240 candidate runtime-oriented files hashed/scanned; one harmless settings comment match, zero unresolved matches or identified configured mutation actions. Not a current saved-world scan. |
| Operational build | `scripts/Test-SkillCapInvestigation.ps1` invokes VS 2022 MSBuild on `Data/System/Source/Server.csproj /p:Configuration=Release /p:Platform=x86` with isolated OutputPath and IntermediateOutputPath | Passed, exit 0. Final server-build log timestamp: 2026-10-05 16:43:54 UTC. |
| Runtime script compile | Isolated `ConficturaServer.exe -compileonly -nocache` | Passed, exit 0 and explicit compile-only success milestone. Log timestamp: 2026-10-05 16:44:44 UTC. No script initialization/world load/listeners. |
| Fixture build | Framework v4 csc, `/target:exe /platform:x86`, referencing that server and runtime script assembly; `tests/SkillCapInvestigation.cs` + `tests/SkillCapMutationInventory.cs` | Passed, exit 0; fixture remains outside runtime scripts. |
| Production-method fixtures and compiled inventory | Isolated `SkillCapInvestigation.exe D:\ConficturaUO-main\Data\Files` | Passed, exit 0: **335 assertions**; 187 mutation refs, 40 reflection refs, 5,297 NPC setter calls, zero unresolved member operands. Log timestamp: 2026-10-05 16:44:47 UTC. |
| Register export | `python scripts/Export-SkillCapInvestigation.py output/skill-cap-investigation/823aed05104b40aaabb6db831ff903c9` | All references classified by exactly one of 35 reviewed families; source declarations resolve; no unmapped calls. |
| Artifact verification | JSON/CSV parse, referenced source existence/line checks, document link checks, source SHA reconciliation, repeat-export hash comparison, `git diff --check` | Final results recorded in the delivery commit; no production files staged. |

Final successful isolated run:

`output/skill-cap-investigation/823aed05104b40aaabb6db831ff903c9`

That ignored directory retains `server-build.log`, `runtime-compile.log`, `fixture-build.log`, `regression.log`, stderr logs and the raw compiled inventories. `verification-results.txt` preserves the small successful build/compile/fixture outputs for review without committing executable/cache output. The full manifests/registers are committed documents, not runtime inputs.

## Reproduction boundaries and earlier harness corrections

The first sandboxed MSBuild attempt could not read the installed SDK registry/metadata location (`MSB4184`, Microsoft SDKs access denied). Running the isolated build with narrowly approved access resolved it. This was an environment restriction, not a shard-source compile failure. No automatic approval rejection remains unresolved.

During harness development, an invalid test override of nonvirtual Backpack.MaxItems was replaced with constructor configuration, and internal MyServerSettings was accessed through reflection instead of a direct reference. The inventory was corrected to exclude its CSV header from counts and normalize inherited MethodInfo objects before comparing region handlers. A register export attempted before its producing run completed was retried after completion. Final logs and artifacts use the corrected harness, with no unresolved failures.

Fixture scope is intentionally bounded: no current world loaded/saved, no socket/listener, no timer thread, no full script Initialize sweep, no connected-client test and no live pile. Gump responses use an inert NetState. Soulstone CheckUse eligibility and food output CheckSequence are the only gameplay-method overrides used for those component fixtures; the grant/transfer/output implementations are production. Spell skill checks and mana regeneration are separately exercised through real methods. Full PlayerMobile/world restart, actual connected-client behavior and specific historical incident attribution remain unverified for these stated reasons.

Completed test families:

- Normal/accelerated gains with totals 999.0, 999.9, 1000.0 and 1000.1; Up/Down/Locked; donor/no donor; independent base sum versus cache.
- Individual cap overshoot; false Player flag; insufficient accelerated donor loss without grant.
- Every whitelisted cap at its total; unsupported cap below total limit; every permitted default-origin boost setting; Counselor exemption; BaseRegion and GuardedRegion entry; skill/profession/attribute collateral wipe.
- XML real-point bypass; XML cached-total bypass and save-time cache repair; disabled/enabled election reward with locked candidate and repeated voter response.
- Scroll, skill stone, soulstone and teaching boundaries; nonzero insufficient donor points; partial teaching; target locks; repeated and changed-state responses.
- Actual Skills save/load preservation of excess; cap reduction and origin recomputation.
- Offline study; 200 real Create Food casting-skill and mana-regeneration callback iterations at total cap.
- Full-pack food delivery: 25 outputs, 50 ground entities, one tile; 96 KiB undrained SendQueue threshold. No player or live-world objects used.

The commit associated with these artifacts is the investigation delivery commit on `main`; its hash is reported in the final user response. Generated verification output remains ignored. Public APIs, save formats and gameplay behavior are unchanged.

## FIX-01 protection repair — 2026-10-18

Status: verified local candidate, uncommitted by explicit instruction. Baseline `17c7217b99a3dc7a44d3a7567fcabfd87ddea520`; branch `main`. Required inputs were current PlayerMobile/BaseRegion/SkillCheck/Skills/PlayerSettings source, the historical investigation/fixtures, root/audit instructions and verified isolated multiplayer-ready saves. Outputs are the narrow PlayerMobile repair, dedicated regression runner/fixture, report/goal, active CSV/workbook, independent review and external rollback/evidence/Resident handoff. No serializer/project/gain-policy changes. Initial unrelated HTML and Excel lock were preserved; lock subsequently disappeared outside this task, and workbook replacement was guarded by original hash equality.

Commands from `D:\ConficturaUO-main`:

- `git status --short`, `git log -5 --oneline`; instruction discovery and literal source reads. `rg` was unavailable, so PowerShell/Python searches were used.
- `scripts/Test-SkillVerificationProtection.ps1 -CharacterizeBaseline -OutputDirectory D:\ConficturaUOTestTools\fix01-20261018\baseline` (before repair): Release/x86 source build, forced runtime script compile, and historical destructive fixtures passed. The finalized runner now requires `-BaselinePlayerMobilePath` for safe reproducibility from the repaired checkout.
- `scripts/Test-SkillVerificationProtection.ps1 -OutputDirectory D:\ConficturaUOTestTools\fix01-20261018\repaired-async`: Release/x86 build, forced `-compileonly -nocache`, fixture compile and 817 protection assertions plus unchanged historical gain matrix passed. Final run `97564e9b974f450a9c0148b701e731a3`.
- `python output/fix01-tools/world_acceptance.py`: complete-data disposable full startup/save/restart, 205+207 assertions and 200 transitions passed. Both protected 20-file save sets unchanged; input saves restored; task-owned runtimes stopped; loopback 24506 free. Initial source-only run omitted two XML assets; it remains recorded separately and was superseded. Logs use service Console.log, copied separately per phase in the corrected runner.
- Native capability recheck: `cua.getState()` returned no apps and only IAB/MCP-App browsers; `cua.listApps()` returned `cua.listApps is not a function`; current tool definition disables native computer APIs and callable tool catalog has no `node_repl`/Sky entry point. No real-client/Qwen execution or unsupported input attempted.
- `python output/fix01-tools/update_workbook.py`: 6,823 source rows, all prior 6,819 preserved; 953 Fixed, 3 Deferred; status/category/system/date reconciliation passed; native charts/styles/layout retained. Original on-disk workbook hash matched rollback copy before candidate replacement. Visual Excel render/recalculation unavailable.
- Independent read-only review resolved the synchronous-logging stall finding using a single bounded background file writer. No production state was changed by review.

Evidence: `D:\ConficturaUOTestTools\fix01-20261018`, especially `world-complete-data-validation.json`, `workbook-validation.json`, `delivery-manifest.json` and `resident-feedback/register.json`. Canonical report: `docs/skill-cap-investigation/FIX-01-REPORT.md`. RuntimeScriptCompileTruth is separate from IDEProjectHygiene (not needed; no project changes). Existing cap-load/grant/accounting limitations remain FIX-02/03/04; no broader invalid-character policy was invented. Real-client/Qwen acceptance is an explicit remaining coverage limit. No commit, push or live restart occurred.

Recorded UTC: 2026-10-18T01:39:50+00:00.

## FIX-01 actual-client continuation — 2026-10-18 13:48 UTC

Cwd `D:\ConficturaUO-main`; external run `D:\ConficturaUOTestTools\fix01-20261018\client-acceptance-20261018`. Direct human approval resolved the earlier delegated-only native authorization rejection. Root/audit instructions and existing candidate worktree were rechecked. No source edits, staging, commits, pushes or production restart.

Required inputs: unchanged repaired PlayerMobile/build hashes, complete checked-in script data, protected multiplayer-ready save manifest, ordinary Player saved login and supported native controls. Outputs: actual-client JSONL, immutable evaluator XML, region/diagnostic logs, three Qwen traces, save/reconnect/gate snapshots, restoration/process/port proofs and updated report/backlog/feedback. Exit scope: preserve all progress through actual client actions, retain failures honestly, restore complete fixtures and stop owned runtimes.

Commands/tools and results:

- Supported `node_repl` + `@oai/sky`: harmless native F5 check, Player login, guarded macro start, normal logout, reconnect and final logout passed. Fresh server identity proved Nephtest 0x209 with Player access.
- `tools/session.py server third` and `server fourth`: full startup with `-service -nocache`, complete data and loopback 24506. Unchanged previously verified Release/x86 core binary; no IDE/project build required. Compile/launcher/network/bootstrap setup failures were retained and safely corrected in the isolated tools only.
- `tools/client_ops.py scenario ...`: seven cases, 30 actual crossings, all full progress tuples equal and one diagnostic per controlled scenario. `train under`: 0.1 real gain on one normal Arms Lore attempt. `train at`: eight attempts, zero gain/excess.
- `tools/client_ops.py qwen`: three actual local model choices and successful ordinary bridge effects; supervised, autonomy=false. No generated speech sent or model training.
- `tools/acceptance_steps.py persistence-before/offline/reloaded/reconnected`: actual logout/save/restart/reconnect progress equality passed. `recall` and three `gate-verify` results passed with exact server destination/map/progress checks. First gate range refusal remains a failed attempt despite Resident's success label.
- `tools/acceptance_steps.py restore`: progress restored; exact inventory assertion failed (BankBox identity after reload and Candle parent during play). `tools/restore_files.py`: authoritative complete save rollback passed for all three 20-file save sets, and full client profile/settings hashes matched.
- Controlled STOP files closed owned runtimes. Exact task-path process enumeration found no remaining processes; exclusive bind checks proved 24506/25091/25092/25093/25094/21437/4713 free. `tools/verify_acceptance.py` passed the bounded progress/rollback checks while explicitly retaining the inventory failure.

Canonical evidence/limits: `docs/skill-cap-investigation/FIX-01-REPORT.md` and external `REPORT.md`, `evidence/acceptance-validation.json`, `file-restoration.json`, `processes-stopped.json`, `ports-stopped.json`. This updates the earlier unavailable client/Qwen status without deleting that historical evidence. Original phase statuses and unrelated changes are preserved.

Workbook continuation: only FIX-01 Notes cell P6821 and the matching CSV field changed. All 6,823 records (953 Fixed, 3 Deferred), other cell values/formulas, styles, charts, filters and all unrelated package parts were verified unchanged. Artifact Tool authored/recalculated a candidate and rendered the existing filtered view. Its full export changed unrelated native parts/cached values, so only the authored note was retained in the original package. Native Excel recalculation was not performed; no formulas or their inputs changed. Evidence: client-acceptance-20261018/evidence/workbook-validation.json and artifact-roundtrip-differences.json.

## Local commit authority and evidence closeout — 2026-10-18

After testing, the user instructed: "Don't forget the commits when everything is finished. Make them clean entries with descriptions." This supersedes the initial no-commit instruction. The coordinating chat created implementation commit `1b49df74e8b972eb53805a790797f1f4872ab001` (`fix: preserve character progress during region validation`). This was verified locally using `git log` and the unchanged tested PlayerMobile SHA-256 `5cea5070f6a36c3d3d5404c235465bea029a52de9351834f2ad5a8450da2c357`. It was not pushed or deployed. Earlier run/review no-commit statements are historical.

This documentation/backlog evidence batch accompanies the implementation. The coordinating chat owns its final review, staging and commit; the documentation commit hash will be recorded externally after that commit, avoiding a self-referential follow-up commit. The current work updates FIX-01's CSV Commit/Notes fields and workbook N6821/P6821 only, preserving all 6,823 records and unrelated workbook content. The sanitized Resident feedback page distinguishes reliable mechanics, bounded positive cases, negative gate-result labeling, unresolved inventory lifecycle observations and complete verified rollback.

The user excludes new test fixtures from Git. `scripts/Test-SkillVerificationProtection.ps1` and `tests/SkillVerificationProtection.cs` remain untracked and untouched; exact external copies and hashes are in `D:\ConficturaUOTestTools\fix01-20261018\regression-harness`. A clean checkout lacks these two files. The report and external harness README explain using them with an explicitly selected fresh disposable checkout. Existing tracked historical tests, unrelated HTML and any lock file are preserved.

Closeout verification is limited to source/commit identity, harness copy hashes and untracked status, Markdown source/link checks, CSV/workbook structural equality and `git diff --check`. No tests, server/client/model runtimes, source edits, push or live activation were performed in this closeout. Prior source build and forced runtime script compile evidence still correspond to the exact implementation hash; IDE/project builds were unnecessary because this batch changes documentation only. Detailed current verification and pending documentation-commit metadata are external under `D:\ConficturaUOTestTools\fix01-20261018\commit-closeout-20261018`.
