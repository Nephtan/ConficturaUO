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
