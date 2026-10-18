# FIX-01: preserve progress during region validation

2026-10-18. Baseline `17c7217b99a3dc7a44d3a7567fcabfd87ddea520`, branch `main`. **Implementation committed locally as `1b49df74e8b972eb53805a790797f1f4872ab001`; not pushed or deployed.** The user authorized clean local commits after testing. This evidence/documentation batch accompanies the implementation; commit identities are recorded in the external handoff. Push and activation remain user-owned. This is a targeted repair, not a skill-system redesign or a claim about the live incident's cause.

## Result and cause

`PlayerMobile.SkillVerification` previously required a hard-coded numeric cap tier as well as cached total/conditional profession checks. A character with **500.0 real points and cap 1200.0** failed even though under cap. Failure called `SkillBegin("default", pm)`, cleared profession, set every skill to zero, and conditionally reset stat cap and all raw stats. Accepted branches also silently changed profession and cap components. The fresh pre-repair fixture reproduced the destructive behavior.

The replacement observes the independently summed `BaseFixedPoint` values and existing state. It never writes bases, individual caps, locks, total cache, total cap, components, raw stats, stat cap, profession, Player flag or CharacterKeys. The only new per-character write is a transient diagnostic deadline. It preserves the existing Counselor exemption and rejects null/deleted/non-player-mobile inputs. The pre-existing `SkillCheck.Gain` implementation is unchanged.

The original case now retains all 58 skill bases/caps/locks, 81/72/63 raw stats, stat cap 260, profession 7, cap 12000 and boost 2000. Evidence records exact before/after state equality. The profession value is a disposable test fixture, not a new eligibility policy.

## Diagnostics and runtime boundary

Entry point: `Data/Scripts/System/Regions/BaseRegion.cs`, `BaseRegion.OnEnter` calls the unchanged public `PlayerMobile.SkillVerification(Mobile)` API. Region movement, teleports, map changes and login placement may reach it through `Region.OnRegionChange`; not every region override calls BaseRegion. `GuardedRegion.OnEnter` still skips the base handler. No new command, packet handler, event subscription or timer is registered.

Reports go to `Logs/SkillVerification.log` beneath the executable's base directory. Records contain UTC time, character serial, independent real total, cached total, cap and start/boost/Ether components, profession, read-only tenth-slot Virtue flag, Player flag, raw stats/stat cap, map ID, location and the region type observed at callback time. Skill quantities are integer tenths. `regionAtCallback` may be the previous region during a transition; map/location identify where the observation occurred. No account name, credentials, character name or raw key string is logged.

Distinct reason codes:

| Code | Meaning |
| --- | --- |
| `real_total_exceeds_cap` | Actual sum of bases exceeds the current cap. |
| `cached_total_mismatch` | Cache disagrees with that independent sum, either high or low. |
| `legacy_cap_unrecognized` | Old numeric whitelist would reject this cap; **does not establish an unsupported configuration or cheating**. |
| `legacy_profession_mismatch` | Old 1300/1800 predicate would reject profession/Virtue state. |
| `cap_components_mismatch` | Wide-integer component sum differs from the current cap. |
| `player_flag_false` | A PlayerMobile has the false Player flag implicated in the gain bypass. |
| `validation_exception` | Observation failed; exception type and serial are reported, with progress preserved. |

At most one report attempt per character per five minutes, and one globally outstanding background write. If the worker is busy, later reports are dropped and the per-character cooldown still applies. A stalled filesystem cannot accumulate workers/queued messages or make this entry path wait on output. The worker receives immutable text, never a Mobile. Output exceptions are swallowed without a fallback to the shared console. Queue failures release the work slot. Reporting is deliberately best effort: a failed/stalled output, saturation, process exit or cooldown can lose a diagnostic. The throttle resets after process restart; no serialization change is needed. No bounded-size log rotation is added.

## Verification

Evidence root: `D:\ConficturaUOTestTools\fix01-20261018`.

| Check | Result and evidence |
| --- | --- |
| Old behavior on baseline | Release/x86 server build, forced script compile and historical reset fixtures passed; `baseline/bb218b5000ed4942804dc504dcccc9b0/regression.log`. Disposable in-memory characters only. |
| Final operational source build | `Server.csproj /p:Configuration=Release /p:Platform=x86` passed, outputs isolated; `repaired-async/97564e9b974f450a9c0148b701e731a3/server-build.log`. |
| RuntimeScriptCompileTruth | Built executable `-compileonly -nocache` passed; `runtime-compile.log` in that directory. This exits before world load/listeners. |
| Deterministic protection checks | **817 assertions** plus unchanged historical normal/accelerated gain matrix; `regression.log`. Includes four origins × eleven boosts × Ether off/on × below/exact/above cap, 500/1200, profession/Virtue combinations, malformed/null keys, high/low cache, false Player flag, invalid caps, wide component sums, null/deleted/staff guards. |
| Diagnostic failure containment | Throwing writer, validation exception, combined failure, stalled writer, 100 other characters during saturation, actual file output, exclusive file lock and recovery passed. Reflection injection exists only in the external fixture executable. |
| Travel/component persistence | 60 actual registered BaseRegion transitions plus under/exact/over-cap Skills serialization roundtrips, included in the 817 assertions. |
| Full-world persistence | Five disposable standard PlayerMobiles, actual `World.Save(false,false)`, process stop/reload and 200 additional transitions. 205 prepare + 207 reload assertions. Original player inventory owner/serial/type/amount equality checked. Complete-data rerun evidence is `world-complete-data-validation.json` and `world-complete-data-evidence/`. |
| Historical limited world run | `world-validation.json` passed focused assertions but lacked checked-in LoreBooks/RandomEncounters XML. Its prepare console was overwritten by service logging. Retained as a harness limitation, superseded by the complete-data rerun with separate console logs. |
| Independent review | [Review](../codebase-audit/outputs/subagent-findings/fix01-independent-review.md). The initial synchronous-console design was rejected and replaced with the tested bounded dedicated writer. |
| IDEProjectHygiene | Not run: no project includes, paths or names changed. This is not inferred from runtime compilation. |
| Real client / Qwen | **FIX-01 progress scope passed, with documented limits.** Direct local approval enabled supported native controls. Thirty Player crossings, below/at-cap gains, normal Recall/gates, logout/save/reload/reconnect and three supervised Qwen choices passed. Exact in-memory inventory comparison failed separately; complete input saves/profiles were restored and hash-verified. See the continuation below. |

Both protected 20-file multiplayer-ready and existing isolated shard save sets remained byte-identical. The disposable runtime's input saves were restored after preserving exercised saves separately. Task-owned processes were stopped and port 24506 was free. Production saves, live server and original Resident/Shardmind were not touched. Test isolation overlays and the administrative world fixture exist only beneath the external test root; none are in production `Data/Scripts` changes.

Earlier controller-session native capability recheck (historical, superseded by the directly authorized local continuation below): `cua.getState()` returned `apps: []` and only the ChatGPT in-app browser and Codex MCP Apps surfaces. The documented read-only `cua.listApps()` call returned `cua.listApps is not a function`. The current callable catalog has no `node_repl`/Sky tool and the CUA tool definition states native APIs are disabled. This is verified capability unavailability, not a denied gameplay input. Exact evidence and concise manual cases are in `native-tool-availability.json` and `MANUAL-CLIENT-ACCEPTANCE.md` under the evidence root.

## Save compatibility and policy limits

The entire file suffix beginning at `CheckMovement`, including Serialize/Deserialize, is unchanged after newline normalization. No serialized class, namespace, version, field order or public signature changed. Existing saved worlds load; newly saved consistent-component fixtures retain all tested progress through reload. Inventory checking covers owner/serial/type/amount, not every item property.

Preservation has intentional consequences. Profession 1 can persist on a non-fugitive numeric tier instead of being silently cleared; existing profession behavior (including BaseRegion's minimum kill count) still applies. A cap/component mismatch is preserved during travel, but **the existing Deserialize still recomputes the cap**. This repair does not promise arbitrary inconsistent caps survive load unchanged. Neither case justifies deleting skills.

With truthful cache/Player state, ordinary gain's existing final total guard still applies. False cache/Player flags, direct XML/reward grants, acceleration's rejected-loss behavior and individual-cap overshoot remain possible in the unchanged paths. A report is not a sanction, grant authorization or automatic restoration. No new invalid-character/grant policy was needed to remove the destructive travel correction; decisions below remain separate.

| Follow-up | Decision and acceptance boundary |
| --- | --- |
| `POST-SKILL-FIX-02` | Choose transactional grant/transfer semantics, reject/clamp/partial behavior and an audited staff-restoration exception. Exercise voting/XML/reward/transfer paths with locks and both caps. |
| `POST-SKILL-FIX-03` | Choose authoritative origin/cap policy and reject/defer/grandfather/migrate handling for cap reductions; make Ether completion idempotent. Review legacy/inconsistent save components and profession eligibility. |
| `POST-SKILL-FIX-04` | Protect real-total accounting and reliable player identity at mutation boundaries, including XML. Share policy with FIX-02 without changing saved layout accidentally. |

These three records are Deferred in the active backlog. FIX-06 remains the separate accelerated-loss/individual-cap recommendation. FIX-01 is Fixed with its implementation committed locally as `1b49df74e8b972eb53805a790797f1f4872ab001`; this does not mean deployed or broadly gameplay accepted. The bounded client progress scope is recorded below.

## Reproduce and rollback

The new regression harness is intentionally excluded from Git by the user's commit scope. `scripts/Test-SkillVerificationProtection.ps1` and `tests/SkillVerificationProtection.cs` remain untracked and untouched in the working checkout; exact copies, relative paths and SHA-256 values are preserved under `D:\ConficturaUOTestTools\fix01-20261018\regression-harness`. A clean checkout does not contain those two files. Existing tracked historical fixtures remain unchanged.

For a future rerun, prepare a fresh disposable checkout at implementation commit `1b49df74e8b972eb53805a790797f1f4872ab001` or a reviewed descendant, with VS 2022/.NET 4.8 build tools, repository dependencies and permitted client data at `Data/Files`. Verify the preserved harness against `regression-harness/manifest.json` and confirm its new test path is absent before copying. The example `$reproRoot` below is a future checkout, not one created during closeout:

```powershell
$fixtureRoot = 'D:\ConficturaUOTestTools\fix01-20261018\regression-harness'
$reproRoot = 'D:\ConficturaUOTestTools\repro\fix01-fresh'
if (Test-Path -LiteralPath (Join-Path $reproRoot 'tests/SkillVerificationProtection.cs')) {
    throw 'Use a fresh disposable checkout; do not overwrite an existing fixture.'
}
Copy-Item -LiteralPath (Join-Path $fixtureRoot 'tests/SkillVerificationProtection.cs') -Destination (Join-Path $reproRoot 'tests/SkillVerificationProtection.cs')
& (Join-Path $fixtureRoot 'scripts/Test-SkillVerificationProtection.ps1') -RepositoryRoot $reproRoot -OutputDirectory 'D:\ConficturaUOTestTools\repro\fix01-results'
```

The explicit repository root is required when invoking the external runner. It creates a GUID compilation snapshot with no saves, builds core, forces `-compileonly -nocache`, and compiles the new fixture together with tracked `tests/SkillCapInvestigation.cs` and `tests/SkillCapMutationInventory.cs`. This compile-only snapshot is not a complete-data world/client startup. To characterize old behavior, add `-CharacterizeBaseline -BaselinePlayerMobilePath 'D:\ConficturaUOTestTools\fix01-20261018\rollback\Data\Scripts\Mobiles\Base\PlayerMobile.cs'`; substitution occurs only inside the disposable snapshot. Full prerequisites and safeguards are in the external harness README. Historical `Test-SkillCapInvestigation.ps1` remains a characterization of older behavior, not the current repair acceptance command.

Full-world tools, source fixtures, inputs, hashes, patch and Resident feedback are retained under the evidence root's `tools/`, `world-complete-data-inputs.json`, `rollback-manifest.json`, `delivery-manifest.json`, `patches/`, and `resident-feedback/`. The world runner deliberately refuses existing output directories. For another run, use a fresh suffix, verify `fixtures/multiplayer-ready/manifest.json`, preserve original saves, retain loopback 24506/external integration isolation, copy required checked-in script XML, and keep the privileged fixture out of production.

Rollback originals for every modified pre-existing file are under `rollback/`, with hashes in the manifest. Before restoration, compare current candidate hashes to `delivery-manifest.json` and preserve later user edits. Restore only selected intended files from those originals. Remove newly added test/report files only if no subsequent work depends on them. Reverting PlayerMobile restores the known wipe, so an operational rollback requires that risk to be considered; no live rollback is performed here. Restoring a source file never requires copying test saves.

The finalized baseline command was exercised again successfully in `baseline-reproducibility/a70c8697b5eb40679e5aecc5d2cabd20`; rollback source was substituted only inside that new disposable compilation directory. `rollback-hashes.json` records all recovered originals, including the workbook whose initial exclusive hash read was unavailable while Excel held it open.

The active CSV/workbook contain 6,823 rows (953 Fixed, 3 Deferred); all original 6,819 rows are preserved. Native styles/charts/layout are retained, ranges and caches reconciled to CSV, and workbook bytes match the verified review copy. Structural validation passed; no Excel visual/render recalculation was performed. The initially open workbook became unlocked before replacement; its on-disk hash still matched the backup. The pre-existing HTML is untouched. The pre-existing Excel lock disappeared outside this task and was neither removed nor recreated by the agent.

This evidence/documentation batch accompanies the implementation; commit identities are recorded in the external handoff. Push and activation remain user-owned and have not been performed. Bounded Player/Qwen progress acceptance is complete; the inventory lifecycle observation and broader autonomous/different-client-facet coverage remain outside that result. No production-safety guarantee is inferred from these isolated results.

## Directly authorized Player/client/Qwen continuation — 2026-10-18

This is the historical testing-stage record, completed before local commit authorization. Its no-commit statements describe that stage; current implementation status is at the top and in the closeout below.

The human directly authorized this local chat to run the isolated native/client/Qwen tests. An earlier native enumeration rejection based on delegated-only permission was resolved by that direct approval. Supported `node_repl` + `@oai/sky` then passed enumeration, capture and harmless input, followed by actual client login/logout/reconnect and bridge controls. No credentials were extracted or printed. The earlier capability limitation above is historical.

Evidence root: `D:\ConficturaUOTestTools\fix01-20261018\client-acceptance-20261018`; detailed report `REPORT.md`, replayable offline verification `tools/verify_acceptance.py`, and result `evidence/acceptance-validation.json`. The exact repaired PlayerMobile hash `5cea5070f6a36c3d3d5404c235465bea029a52de9351834f2ad5a8450da2c357` and previously built core hash `db6702646c6fab0c6c2818c279e92fabb36cf55abd9a383b2e9beff8c63bc6d4` match the accepted build. Two complete-data startups forced runtime script compilation. Only the disposable snapshot had eight isolation overlays and the finite privileged evaluator; production source was unchanged by this continuation.

Actual connected actor was Nephtest (0x209), AccessLevel.Player, living with Player=true. Seven cases produced **30 ordinary client-driven BaseRegion crossings**: original 500/1200, exact, over, two profession mismatches and both cache discrepancies. All 58 skill bases/caps/locks, real/cache totals, cap/components, profession, CharacterKeys, raw stats/stat cap/stat locks and Player flag matched. Each controlled case emitted one diagnostic despite repeated crossings. The test fixture reset the transient diagnostic deadline only between cases.

Ordinary Arms Lore gained 0.1 below cap; eight attempts at cap with no Down donor gained zero and produced no excess. Normal runebook Recall, same-map moongate travel, and Sosaria to Atlantis to Sosaria gates preserved progress and reached verified destinations. MapIndex was 1 to 6 to 1; both maps use client MapID 1. Normal logout, a full disposable save/reload and actual reconnect retained all tested progress with consistent cap components.

Three recorded local Qwen3.5:9b choices selected crossing, return and inspect and completed through the Player bridge. These were bounded supervised choices with autonomy=false, not a sustained autonomous demonstration. Raw prompts/responses and model runtime identity are preserved in the fresh Resident copy; only client observations were provided to the model. Its generated `say` text was not transmitted. Actual outcomes were independently verified.

Two limits are explicit. First, Resident called the initial out-of-range gate request `succeeded` despite the server response "That is too far away." The closer retry reached the destination but was labeled `dispatched`; acceptance uses server position/progress, not that label. Second, exact in-memory inventory comparison failed: BankBox serial 1073753152 was absent after reload, and Candle serial 1073760310 later moved from Mobile 521 to backpack 0x40004816. No cause or expected-behavior explanation is established. This is retained in `restore-inventory-difference.json`, not reported as a pass or repaired under FIX-01.

Progress restoration passed. Authoritative cleanup restored the complete input Saves: both protected save sets and the disposable restored set each have exactly 20 files matching expected SHA-256 values. Client Data/Profiles (7 files), ClassicAssist/Profiles (1 file) and settings match backups. Exercised saves/profiles were retained separately. Owned server/client/Resident/model processes stopped; exact-path process enumeration returned no matches, and ports 24506, 25091/25092, 25093/25094, 21437 and 4713 were free. No production saves, live runtime, original Resident/Shardmind, model weights or unrelated HTML were changed. Evidence is in `file-restoration.json`, `processes-stopped.json` and `ports-stopped.json`.

The local progress scope is accepted with these limits. No commit, stage, push or deployment occurred. FIX-02/03/04 remain Deferred; no grant/accounting policy was changed.

Workbook continuation: only FIX-01 Notes cell P6821 and the matching CSV field changed. All 6,823 records (953 Fixed, 3 Deferred), other cell values/formulas, styles, charts, filters and all unrelated package parts were verified unchanged. Artifact Tool authored/recalculated a candidate and rendered the existing filtered view. Its full export changed unrelated native parts/cached values, so only the authored note was retained in the original package. Native Excel recalculation was not performed; no formulas or their inputs changed. Evidence: client-acceptance-20261018/evidence/workbook-validation.json and artifact-roundtrip-differences.json.

## Local commit authority and evidence closeout — 2026-10-18

After testing, the user instructed: "Don't forget the commits when everything is finished. Make them clean entries with descriptions." This supersedes the initial no-commit instruction. The coordinating chat created implementation commit `1b49df74e8b972eb53805a790797f1f4872ab001` (`fix: preserve character progress during region validation`). This was verified locally using `git log` and the unchanged tested PlayerMobile SHA-256 `5cea5070f6a36c3d3d5404c235465bea029a52de9351834f2ad5a8450da2c357`. It was not pushed or deployed. Earlier run/review no-commit statements are historical.

This documentation/backlog evidence batch accompanies the implementation. Commit identities are recorded in the external handoff, avoiding a self-referential documentation commit. Push and activation remain user-owned. The current work updates FIX-01's CSV Commit/Notes fields and workbook N6821/P6821 only, preserving all 6,823 records and unrelated workbook content. The sanitized Resident feedback page distinguishes reliable mechanics, bounded positive cases, negative gate-result labeling, unresolved inventory lifecycle observations and complete verified rollback.

The user excludes new test fixtures from Git. `scripts/Test-SkillVerificationProtection.ps1` and `tests/SkillVerificationProtection.cs` remain untracked and untouched; exact external copies and hashes are in `D:\ConficturaUOTestTools\fix01-20261018\regression-harness`. A clean checkout lacks these two files. The report and external harness README explain using them with an explicitly selected fresh disposable checkout. Existing tracked historical tests, unrelated HTML and any lock file are preserved.

Closeout verification is limited to source/commit identity, harness copy hashes and untracked status, Markdown source/link checks, CSV/workbook structural equality and `git diff --check`. No tests, server/client/model runtimes, source edits, push or live activation were performed in this closeout. Prior source build and forced runtime script compile evidence still correspond to the exact implementation hash; IDE/project builds were unnecessary because this batch changes documentation only. Detailed current verification and commit metadata are external under `D:\ConficturaUOTestTools\fix01-20261018\commit-closeout-20261018`.

See [sanitized Resident feedback](FIX-01-RESIDENT-FEEDBACK.md). Raw private prompts, configuration, saves and privileged fixtures are excluded from the documentation commit.
