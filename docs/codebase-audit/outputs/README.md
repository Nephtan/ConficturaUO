# Codebase Audit Outputs

This directory stores durable outputs produced by the codebase systems audit and reorganization phase runner.

Raw command output may remain temporary when it is easy to regenerate. Curated tables, reviewed registers, blocker records, accepted risks, and verification notes belong here so future runs do not depend on chat history.

For the human start-here guide, read [../README.md](../README.md). This file is the artifact catalog for the `outputs/` directory.

## Output Policy

- Keep outputs source-verified and phase-labeled.
- Prefer repository-relative paths in tables.
- Exclude `bin` and `obj` unless a phase explicitly reviews generated output.
- Mark uncertain records as `Unknown`, `Partial`, `Blocked`, `Deferred`, or `Intentional` instead of guessing.
- Record generation commands and verification in `../RUN_LOG.md`.
- Do not approve reorganization moves from this directory alone; moves require Phase 12 design and Phase 6 save-compatibility review where serialized types are affected.

## How Outputs Are Organized

There are three kinds of files in this directory:

| Kind | Pattern | Meaning |
| --- | --- | --- |
| Canonical registers | `project-truth-register.csv`, `runtime-hook-map.csv`, `serialization-register.csv`, and similar unprefixed files. | Current canonical copy produced by a phase or post-audit baseline. Use these first for broad lookups. |
| Phase-scoped copies | `phase-NN-*`. | Evidence produced for a specific phase. These preserve what the phase generated and are useful for audits, reruns, and traceability. |
| Post-audit overlays and reviews | `post-audit-*`, `post-batch-*`, compile-only/source-build baseline files. | Current implementation and review state after the full phase audit. Use these to reconcile historical backlog rows. |

When a canonical register and a phase-scoped copy have the same purpose, the canonical file is the easier lookup target and the phase file is the traceable phase artifact. Example: use `serialization-register.csv` for normal serializer lookup, and `phase-06-serialization-register.csv` when proving exactly what Phase 6 generated.

## Start Here By Task

| Task | Open First | Then Open |
| --- | --- | --- |
| Understand current post-audit state | `../PHASE_STATUS.md` | `post-audit-next-steps.md`, `post-audit-active-backlog-status.csv` |
| Find live runtime compile context | `live-build-and-runtime-script-compile-model.md` | `runtime-script-compile-inventory.csv`, `compile-only-verification-baseline.md` |
| Find Visual Studio project drift | `project-truth-register.csv` | `missing-compile-targets.csv`, `unincluded-source-files.csv`, `project-cleanup-backlog.csv` |
| Find source ownership | `system-owner-map.csv` | `cross-tree-runtime-inventory.csv`, `system-cards/` |
| Review hooks, commands, packets, gumps, or timers | `runtime-hook-map.csv` | Phase 5 focused registers and relevant `post-batch-*` reviews |
| Review save compatibility | `serialization-register.csv` | `phase-06-high-risk-serializer-list.csv`, `phase-06-move-rename-risk-list.csv`, `post-batch-b-save-compatibility-triage.csv` |
| Review documentation truth | `documentation-truth-table.csv` | `phase-07-stale-claim-backlog.csv`, `phase-07-source-trace-coverage-report.csv` |
| Review dependencies | `dependency-graph.csv` | `phase-08-hard-dependency-list.csv`, `phase-08-soft-dependency-list.csv`, `phase-08-conflict-edge-list.csv` |
| Review gameplay synergy or conflicts | `synergy-conflict-matrix.csv` | `phase-09-balance-risk-list.csv`, `phase-09-preservation-notes.csv` |
| Find actionable repair state | `post-audit-active-backlog-status.csv` | `repair-backlog.csv`, `verification-matrix.csv`, relevant `post-batch-*` artifact |
| Review reorganization design | `reorganization-design.md` | `phase-12-move-proposal-table.csv`, `phase-12-keep-in-place-decisions.csv` |

## Historical Backlog vs Active Overlay

`repair-backlog.csv` is the Phase 13 generated backlog. It should remain stable as historical evidence unless Phase 13 is intentionally regenerated.

`post-audit-active-backlog-status.csv` is the active overlay for rows that have been reviewed, fixed, accepted, or otherwise dispositioned after Phase 13. It records the historical backlog state, active status, review artifact, source evidence, commit, and notes.

Use this reconciliation rule:

1. Search `post-audit-active-backlog-status.csv` by `BacklogId`.
2. If a row exists, trust its `ActiveStatus` and linked `ReviewArtifact` for current state.
3. If no overlay row exists, use `repair-backlog.csv` as the remaining historical backlog source.
4. Check `../PHASE_STATUS.md` for latest batch-level notes before starting work.

Do not mark a historical `Ready` row as still actionable until you have checked the active overlay.

## Common Status Terms

| Status | Meaning |
| --- | --- |
| `Ready` | Historical backlog item was generated as actionable. Check post-audit overlay before acting. |
| `Open` | Historical backlog item was still open at generation time. Check post-audit overlay before acting. |
| `Deferred` | Work was intentionally postponed, usually for design, migration, or reorganization gating. |
| `Fixed` | A later batch changed source/docs and recorded verification and commit evidence. |
| `ReviewedNoChange` | Source was reviewed and no edit was needed. |
| `SafeNoChange` | Source was reviewed and the generated risk was safe as-is. |
| `IntentionalLegacy` | Behavior is intentionally retained for compatibility. |
| `FalsePositive` | The generated risk row was not a real issue after source review. |

## CSV Reading Tips

Use PowerShell `Import-Csv` for large files:

```powershell
Import-Csv docs/codebase-audit/outputs/post-audit-active-backlog-status.csv |
    Where-Object { $_.BacklogId -eq 'RB-03235' } |
    Format-List
```

```powershell
Import-Csv docs/codebase-audit/outputs/system-owner-map.csv |
    Where-Object { $_.Path -like '*PvPConsent*' } |
    Select-Object Path,System,Evidence |
    Format-Table -AutoSize
```

Tips:

- Use `Select-Object` to narrow wide rows before reading.
- Avoid editing generated CSVs in spreadsheet tools unless you can preserve UTF-8, quoting, and line endings.
- Treat generated risk rows as leads. Source review and post-audit review artifacts decide final disposition.
- Keep repository-relative paths and exact IDs when copying evidence into new docs or backlog entries.

## Expected Outputs

| Output | Phase | Purpose | Initial Status |
| --- | --- | --- | --- |
| `phase-00-baseline.md` | Phase 0 | Record worktree baseline, instruction scopes, build surface, source roots, risk boundaries, and the no-reorganization decision. | Complete |
| `phase-01-summary.md` | Phase 1 | Summarize reproducible inventory generation, row counts, storage policy, and reproduction command. | Complete |
| `phase-01-source-files.csv` | Phase 1 | List audited `.cs` files excluding generated output folders. | Complete |
| `phase-01-project-files.csv` | Phase 1 | List solution and project files discovered by the inventory script. | Complete |
| `phase-01-config-files.csv` | Phase 1 | List XML/config/data files under `Data`, excluding generated output folders. | Complete |
| `phase-01-agents.csv` | Phase 1 | List instruction files and their directory scopes. | Complete |
| `phase-01-project-includes.csv` | Phase 1 | Decode and resolve every `Scripts.csproj` `Compile Include` with literal path checks. | Complete |
| `phase-01-missing-compile-targets.csv` | Phase 1 | List compile includes whose resolved file is missing. | Complete |
| `phase-01-unincluded-source-files.csv` | Phase 1 | List real script source files absent from `Scripts.csproj`. | Complete |
| `phase-01-namespace-type-inventory.csv` | Phase 1 | Record namespace and declared type markers for audited source files. | Complete |
| `phase-01-runtime-marker-inventory.csv` | Phase 1 | Record runtime entry marker hits for initialization, commands, hooks, timers, gumps, and regions. | Complete |
| `phase-01-command-registration-inventory.csv` | Phase 1 | Record command registration marker hits. | Complete |
| `phase-01-event-packet-hook-inventory.csv` | Phase 1 | Record event, packet, timer, and world hook marker hits. | Complete |
| `phase-01-serialization-marker-inventory.csv` | Phase 1 | Summarize serializer markers by source file. | Complete |
| `phase-01-gump-inventory.csv` | Phase 1 | Record gump open and response marker hits. | Complete |
| `phase-01-config-reference-inventory.csv` | Phase 1 | Record source string references to XML/config/text/json data files. | Complete |
| `phase-01-documentation-inventory.csv` | Phase 1 | Record markdown source-trace, code-verified, needs-rework, slug, and link markers. | Complete |
| `phase-01-duplicate-doc-slugs.csv` | Phase 1 | Record duplicate normalized documentation slugs. | Complete |
| `phase-02-summary.md` | Phase 2 | Summarize project truth counts, outputs, and build verification status. | Complete |
| `phase-02-project-truth-register.csv` | Phase 2 | Record one row per Scripts project include and one row per script source file. | Complete |
| `phase-02-missing-compile-targets-classified.csv` | Phase 2 | Classify missing `Scripts.csproj` compile targets. | Complete |
| `phase-02-unincluded-source-classified.csv` | Phase 2 | Classify real script source files absent from `Scripts.csproj`. | Complete |
| `phase-02-intentional-noncompiled-source.csv` | Phase 2 | Record any files classified as generated, backup, old, or intentional non-compiled source. | Complete |
| `phase-02-project-cleanup-backlog.csv` | Phase 2 | Group project truth discrepancies into repair backlog items. | Complete |
| `phase-02-solution-configurations.csv` | Phase 2 | Map solution configurations to `Server` and `Scripts` project configurations. | Complete |
| `phase-02-build-verification.md` | Phase 2 | Record direct project and maintained solution MSBuild verification results. | Complete |
| `project-truth-register.csv` | Phase 2 | Canonical project truth register copied from the Phase 2 register output. | Complete |
| `missing-compile-targets.csv` | Phase 2 | Canonical missing compile target list. | Complete |
| `unincluded-source-files.csv` | Phase 2 | Canonical unincluded script source list. | Complete |
| `intentional-noncompiled-source.csv` | Phase 2 | Canonical intentional/generated/backup non-compiled source list. | Complete |
| `project-cleanup-backlog.csv` | Phase 2 | Canonical project cleanup backlog. | Complete |
| `solution-configurations.csv` | Phase 2 | Canonical solution configuration map. | Complete |
| `cross-tree-runtime-inventory.csv` | Phase 3 | Canonical runtime role, owner, entry point, serialization, gump, and config inventory for audited source files. | Complete |
| `phase-03-cross-tree-runtime-inventory.csv` | Phase 3 | Phase-scoped copy of the runtime inventory. | Complete |
| `phase-03-root-role-summary.csv` | Phase 3 | Root-level counts for initialization, commands, events, packets, serialization, gumps, and unknowns. | Complete |
| `phase-03-unknown-owner-list.csv` | Phase 3 | Files with `Unknown` role or owner and a required follow-up. | Complete |
| `phase-03-high-risk-root-summary.csv` | Phase 3 | Machine-readable high-risk root summary. | Complete |
| `phase-03-high-risk-root-summary.md` | Phase 3 | Human-readable high-risk root summary. | Complete |
| `phase-03-summary.md` | Phase 3 | Summarize runtime inventory inputs, outputs, root counts, and exit criteria. | Complete |
| `cross-tree-runtime-inventory.*` | Phase 3 | Assign runtime role, owner, entry points, hooks, serialization, gumps, and config usage for source files. | Complete |
| `system-cards/` | Phase 4 | Store one canonical engineering card per seeded high-risk system. | Complete |
| `phase-04-summary.md` | Phase 4 | Summarize generated system cards, owner map, backlog, priority list, and verification states. | Complete |
| `phase-04-system-card-index.csv` | Phase 4 | Record one metadata row per generated system card. | Complete |
| `phase-04-system-owner-map.csv` | Phase 4 | Map matched files to generated system cards. | Complete |
| `system-owner-map.csv` | Phase 4 | Canonical system owner map generated from Phase 4 cards. | Complete |
| `phase-04-system-card-backlog.csv` | Phase 4 | Track follow-up work for partial or blocked system cards. | Complete |
| `phase-04-high-risk-system-priority-list.csv` | Phase 4 | Record high-risk system card review order and risk basis. | Complete |
| `runtime-hook-map.csv` | Phase 5 | Canonical hook map for commands, events, packets, timers, gumps, regions, speech, movement, login/logout, and world hooks. | Complete |
| `phase-05-runtime-hook-map.csv` | Phase 5 | Phase-scoped copy of the canonical runtime hook map. | Complete |
| `phase-05-global-hook-risk-list.csv` | Phase 5 | Focus global, high-risk, and critical hook rows for later review tracks. | Complete |
| `phase-05-command-surface-register.csv` | Phase 5 | Record command registration rows, access markers, duplicate counts, and guard-review state. | Complete |
| `phase-05-packet-handler-register.csv` | Phase 5 | Record packet handler rows as critical network entry points. | Complete |
| `post-batch-a-packet-handler-review.csv` | Post-audit | Source-review the 17 P0 packet handler rows and record fixed, reviewed, or deferred actions. | Complete |
| `phase-05-gump-response-risk-register.csv` | Phase 5 | Record gump send/response rows and conservative guard-review flags. | Complete |
| `phase-05-timer-world-hook-register.csv` | Phase 5 | Record timers, delayed calls, and world save/load hook rows. | Complete |
| `phase-05-summary.md` | Phase 5 | Summarize Phase 5 inputs, generated outputs, hook counts, risk counts, and exit criteria. | Complete |
| `runtime-hook-map.*` | Phase 5 | Map commands, events, packets, timers, gumps, regions, speech, movement, login/logout, and world hooks. | Complete |
| `serialization-register.csv` | Phase 6 | Canonical serializer map with class, save version, ordered writes/reads, version handling, field alignment, and move risk. | Complete |
| `phase-06-serialization-register.csv` | Phase 6 | Phase-scoped copy of the canonical serializer register. | Complete |
| `phase-06-high-risk-serializer-list.csv` | Phase 6 | Focus high-risk, ambiguous, asymmetric, unversioned, project-truth, and move-sensitive serializer rows. | Complete |
| `phase-06-move-rename-risk-list.csv` | Phase 6 | Classify serialized namespace/type rename and file-move risk. | Complete |
| `phase-06-serializer-comment-target-list.csv` | Phase 6 | Candidate Phase 11 source-comment targets for fragile save behavior. | Complete |
| `phase-06-save-compatibility-repair-backlog.csv` | Phase 6 | Concrete save-compatibility follow-up items generated from serializer risk signals. | Complete |
| `phase-06-summary.md` | Phase 6 | Summarize Phase 6 inputs, outputs, version handling, field alignment, move risk, and exit criteria. | Complete |
| `serialization-register.*` | Phase 6 | Record save format, versioning, write/read order, and move/rename risk for serialized types. | Complete |
| `documentation-truth-table.csv` | Phase 7 | Canonical documentation truth table with page classification, source trace, hook, serialization, and backlog status. | Complete |
| `phase-07-documentation-truth-table.csv` | Phase 7 | Phase-scoped copy of the documentation truth table. | Complete |
| `phase-07-canonical-page-map.csv` | Phase 7 | Map indexed canonical wiki pages to source-trace and coverage status. | Complete |
| `phase-07-alias-legacy-slug-map.csv` | Phase 7 | Classify legacy slug pages and independent-claim risks. | Complete |
| `phase-07-stale-claim-backlog.csv` | Phase 7 | Generated documentation verification backlog candidates. | Complete |
| `phase-07-source-trace-coverage-report.csv` | Phase 7 | Source-trace coverage grouped by wiki index category. | Complete |
| `phase-07-summary.md` | Phase 7 | Summarize Phase 7 inputs, generated outputs, coverage counts, and exit criteria. | Complete |
| `documentation-truth-table.*` | Phase 7 | Classify docs as canonical, alias, stale, partial, or verified with source traces. | Complete |
| `dependency-graph.csv` | Phase 8 | Canonical dependency graph across source references, runtime hooks, serialization, project includes, config, docs-only links, and conflicts. | Complete |
| `phase-08-dependency-graph.csv` | Phase 8 | Phase-scoped copy of the dependency graph. | Complete |
| `phase-08-hard-dependency-list.csv` | Phase 8 | Hard source, runtime, serialization, project, and config dependency edges. | Complete |
| `phase-08-soft-dependency-list.csv` | Phase 8 | Soft or speculative documentation/config relationships. | Complete |
| `phase-08-conflict-edge-list.csv` | Phase 8 | Conflict, mismatch, duplicate, or manual-review dependency edges. | Complete |
| `phase-08-standalone-proof-list.csv` | Phase 8 | Negative-evidence standalone proof table for system-card systems. | Complete |
| `phase-08-summary.md` | Phase 8 | Summarize Phase 8 inputs, outputs, edge counts, strength counts, and exit criteria. | Complete |
| `dependency-graph.*` | Phase 8 | Record source-verified hard, soft, speculative, docs-only, and conflict edges between systems. | Complete |
| `synergy-conflict-matrix.csv` | Phase 9 | Canonical pairwise matrix classifying gameplay, balance, maintenance, staff, and documentation relationships between system-card systems. | Complete |
| `phase-09-synergy-conflict-matrix.csv` | Phase 9 | Phase-scoped copy of the pairwise matrix. | Complete |
| `phase-09-domain-buckets.csv` | Phase 9 | Group system-card systems into Progression, Combat, PvP, PvE, AI, Magic, Economy, Crafting, Housing, Travel, Government, Staff events, Documentation, and related review domains. | Complete |
| `phase-09-balance-risk-list.csv` | Phase 9 | List gameplay, pacing, reward, policy, and economy balance risks separately from code correctness risks. | Complete |
| `phase-09-documentation-risk-list.csv` | Phase 9 | List source-trace, stale-claim, alias, and missing-path documentation risks surfaced during synergy review. | Complete |
| `phase-09-staff-dependency-list.csv` | Phase 9 | List relationships requiring staff tooling, staff event intervention, or explicit event override policy. | Complete |
| `phase-09-preservation-notes.csv` | Phase 9 | Preserve positive synergies during later cleanup and reorganization design. | Complete |
| `phase-09-player-objective-review.csv` | Phase 9 | Record immediate, medium-term, long-term, social/staff, exploration, crafting/economy, and progression goals by system. | Complete |
| `phase-09-summary.md` | Phase 9 | Summarize Phase 9 inputs, outputs, label counts, spot checks, and exit criteria. | Complete |
| `risk-track-findings.csv` | Phase 10 | Canonical risk-track findings across build drift, serializers, hooks, packets, gumps, commands, pooled enumerables, regions, PlayerMobile coupling, economy, staff tooling, legacy, XML/config, and docs contradictions. | Complete |
| `phase-10-risk-track-findings.csv` | Phase 10 | Phase-scoped copy of the risk-track findings. | Complete |
| `phase-10-non-issue-records.csv` | Phase 10 | Track-level non-issues and aggregate non-finding evidence. | Complete |
| `phase-10-repair-backlog-items.csv` | Phase 10 | One open follow-up item per Phase 10 finding. | Complete |
| `phase-10-accepted-risk-notes.csv` | Phase 10 | Risks accepted only for the audit stage, not for source implementation. | Complete |
| `phase-10-comment-target-additions.csv` | Phase 10 | Candidate Phase 11 source-comment targets generated from serializer, hook, packet, pooled enumerable, and PlayerMobile review. | Complete |
| `phase-10-pooled-enumerable-review.csv` | Phase 10 | Source-scan rows for range scans and pooled enumerable ownership. | Complete |
| `phase-10-track-coverage.csv` | Phase 10 | Per-track reviewed row, finding, backlog, non-issue, accepted-risk, and comment-target counts. | Complete |
| `phase-10-summary.md` | Phase 10 | Summarize Phase 10 inputs, outputs, severity counts, track counts, and exit criteria. | Complete |
| `comment-target-register.csv` | Phase 11 | Canonical reviewed comment target register with approval, rejection, and deferral decisions. | Complete |
| `phase-11-reviewed-comment-targets.csv` | Phase 11 | Phase-scoped reviewed target list. | Complete |
| `phase-11-approved-comment-targets.csv` | Phase 11 | Approved targets whose comments were applied in source. | Complete |
| `phase-11-rejected-comment-list.csv` | Phase 11 | Rejected or deferred comment targets with reasons. | Complete |
| `phase-11-source-comment-edits.csv` | Phase 11 | Source comment edits applied in this phase. | Complete |
| `phase-11-verification-notes.md` | Phase 11 | Verification notes, source comment evidence, rejection policy, and build caveat. | Complete |
| `phase-11-summary.md` | Phase 11 | Summarize Phase 11 inputs, outputs, decisions, and exit criteria. | Complete |
| `reorganization-design.csv` | Phase 12 | Canonical design principles and hard gates for reorganization. | Complete |
| `reorganization-design.md` | Phase 12 | Human-readable reorganization design narrative. | Complete |
| `phase-12-target-layout-proposal.csv` | Phase 12 | Proposed Custom target folders and ownership rules. | Complete |
| `phase-12-move-proposal-table.csv` | Phase 12 | Design-only move proposals with save risk, project updates, docs updates, verification, and rollback plans. | Complete |
| `phase-12-keep-in-place-decisions.csv` | Phase 12 | Existing roots and systems that should remain in place. | Complete |
| `phase-12-third-party-containment-plan.csv` | Phase 12 | Imported package containment rules. | Complete |
| `phase-12-save-compatibility-notes.csv` | Phase 12 | Save risk and migration gates for proposed moves. | Complete |
| `phase-12-project-update-plan.csv` | Phase 12 | `Scripts.csproj` update expectations for proposed moves. | Complete |
| `phase-12-namespace-plan.csv` | Phase 12 | Namespace and serialized type rename policy by move proposal. | Complete |
| `phase-12-documentation-move-plan.csv` | Phase 12 | Documentation update requirements for proposed moves. | Complete |
| `phase-12-summary.md` | Phase 12 | Summarize Phase 12 inputs, outputs, counts, and exit criteria. | Complete |
| `repair-backlog.csv` | Phase 13 | Canonical prioritized repair backlog. | Complete |
| `phase-13-repair-backlog.csv` | Phase 13 | Phase-scoped repair backlog copy. | Complete |
| `accepted-risk-register.csv` | Phase 13 | Canonical accepted-risk register with evidence and review triggers. | Complete |
| `phase-13-accepted-risk-register.csv` | Phase 13 | Phase-scoped accepted-risk register copy. | Complete |
| `deferred-work-register.csv` | Phase 13 | Canonical deferred work register for comments and organization moves. | Complete |
| `phase-13-deferred-work-register.csv` | Phase 13 | Phase-scoped deferred work register copy. | Complete |
| `phase-13-batch-plan.csv` | Phase 13 | Small-batch implementation plan. | Complete |
| `verification-matrix.csv` | Phase 13 | Canonical category verification matrix. | Complete |
| `phase-13-verification-matrix.csv` | Phase 13 | Phase-scoped verification matrix copy. | Complete |
| `phase-13-summary.md` | Phase 13 | Summarize Phase 13 inputs, outputs, priorities, statuses, categories, and exit criteria. | Complete |
| `phase-14-required-inputs.csv` | Phase 14 | Confirm required prior phase outputs and control files are present. | Complete |
| `phase-14-phase-status-snapshot.csv` | Phase 14 | Capture phase closure state before final commit. | Complete |
| `phase-14-change-classification.csv` | Phase 14 | Classify the final batch as documentation/generated audit data and map verification level. | Complete |
| `phase-14-verification-plan.csv` | Phase 14 | List final verification commands and expected results. | Complete |
| `phase-14-commit-history.csv` | Phase 14 | Capture recent focused audit commits. | Complete |
| `phase-14-worktree-status.md` | Phase 14 | Record generation-time git status and explain expected Phase 14 output changes. | Complete |
| `phase-14-verification-notes.md` | Phase 14 | Record inputs, outputs, build applicability, and prior phase closure notes. | Complete |
| `phase-14-final-status-report.md` | Phase 14 | Provide reviewer-facing final status, verification caveats, and changed-file summary. | Complete |
| `phase-14-summary.md` | Phase 14 | Summarize Phase 14 inputs, outputs, generated artifacts, and exit criteria. | Complete |
| `live-build-and-runtime-script-compile-model.md` | Post-audit | Record the source-build and runtime script compile truth model after live-operations context was supplied. | Complete |
| `runtime-script-compile-inventory.csv` | Post-audit | List runtime-visible `.cs` files gathered by the live server startup compile model, excluding generated output folders. | Complete |
| `source-build-and-runtime-compile-baseline.md` | Post-audit | Record source build result, runtime script inventory result, and startup smoke availability. | Complete |
| `post-audit-next-steps.md` | Post-audit | Record the current implementation state and explain why the Phase 13 batch plan is superseded for execution order. | Complete |
| `post-audit-batch-plan.csv` | Post-audit | Runtime-first repair batch order after live server compile context was supplied. | Complete |
| `compile-only-verification-baseline.md` | Post-audit | Record the safe runtime script compile verification flag, generated-folder runtime compiler fix, and successful compile-only run. | Complete |
| `post-audit-active-backlog-status.csv` | Post-audit | Overlay active post-audit dispositions for historical repair backlog rows without rewriting generated Phase 13 evidence. | Complete |
| `post-batch-b-save-compatibility-triage.csv` | Post-audit | Scope the 304 P0 critical save-compatibility rows and record source-reviewed decisions as `POST-BATCH-B` proceeds; all 304 rows are reviewed after `System:Regions` batch `POST-BATCH-B-33A`. | Complete |
| `post-batch-b-save-compatibility-closeout.md` | Post-audit | Close out `POST-BATCH-B`, summarize decision counts, record source fixes for remaining active save issues, and unblock `POST-BATCH-C`. | Complete |
| `post-batch-c-runtime-hooks-player-mobile-review.csv` | Post-audit | Review the 17 P0 runtime-hook rows and 8 P0 `PlayerMobile` coupling rows, reconciling prior packet-handler fixes and recording source-reviewed no-change coupling decisions. | Complete |
| `post-batch-d-pooled-enumerable-review.csv` | Post-audit | Review and repair P1 pooled enumerable ownership rows in focused source batches; all 408 rows are reviewed after `POST-BATCH-D-73A`: 406 fixed and 2 false positives. | Complete |
| `post-batch-e-hooks-gumps-commands-regions-review.csv` | Post-audit | Review and repair runtime hooks, gump guards, command access, and region/map assumptions by focused system groups; current coverage is 292 reviewed rows through `POST-BATCH-E-100A`; no unreviewed `Runtime hooks`, `Gump guards`, `Command access`, or `Regions` repair-backlog rows remain. | Complete |


| `post-batch-f-documentation-balance-review.csv` | Post-audit | Review all 540 POST-BATCH-F documentation contradiction, economy/reward, staff tooling, and XML/config schema rows with fixed/deferred/queued dispositions. | Complete |
| `post-batch-f-documentation-balance-closeout.md` | Post-audit | Close out POST-BATCH-F with decision counts, documentation fixes, balance/staff/config disposition policy, and verification notes. | Complete |

| `post-batch-g-project-include-drift-review.csv` | Post-audit | Review all 61 POST-BATCH-G historical project include drift rows and reconcile them to current zero-drift ScriptsProjectTruth evidence. | Complete |
| `post-batch-g-project-include-drift-closeout.md` | Post-audit | Close out POST-BATCH-G with project truth counts, overlay status, known direct Scripts.csproj limitation, and verification notes. | Complete |



| `post-batch-h-character-level-move-review.csv` | Post-audit | Review and disposition `RB-06802` for the POST-BATCH-H-01A Character Level reorganization pilot. | Complete |
| `post-batch-h-character-level-move-closeout.md` | Post-audit | Close out the Character Level move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |



| `post-batch-h-ai-overhaul-move-review.csv` | Post-audit | Review and disposition `RB-06809` for the POST-BATCH-H-02A AI Overhaul reorganization batch. | Complete |
| `post-batch-h-ai-overhaul-move-closeout.md` | Post-audit | Close out the AI Overhaul move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |

| `post-batch-h-static-gump-tool-move-review.csv` | Post-audit | Review and disposition `RB-06811` for the POST-BATCH-H-03A Static Gump Tool reorganization batch. | Complete |
| `post-batch-h-static-gump-tool-move-closeout.md` | Post-audit | Close out the Static Gump Tool move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |

| `post-batch-h-omniai-move-review.csv` | Post-audit | Review and disposition `RB-06810` for the POST-BATCH-H-04A OmniAI reorganization batch. | Complete |
| `post-batch-h-omniai-move-closeout.md` | Post-audit | Close out the OmniAI move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |


| `post-batch-h-staff-toolbar-move-review.csv` | Post-audit | Review and disposition `RB-06812` for the POST-BATCH-H-05A Staff Toolbar reorganization batch. | Complete |
| `post-batch-h-staff-toolbar-move-closeout.md` | Post-audit | Close out the Staff Toolbar move with project truth, runtime visibility, access/workflow, serialization, verification, and rollback evidence. | Complete |


| `post-batch-h-random-encounters-move-review.csv` | Post-audit | Review and disposition `RB-06804` for the POST-BATCH-H-06A Random Encounters reorganization batch. | Complete |
| `post-batch-h-random-encounters-move-closeout.md` | Post-audit | Close out the Random Encounters move with project truth, runtime visibility, XML path, serialization, verification, and rollback evidence. | Complete |


| `post-batch-h-clone-offline-move-review.csv` | Post-audit | Review and disposition `RB-06815` for the POST-BATCH-H-07A Clone Offline Player Characters reorganization batch. | Complete |
| `post-batch-h-clone-offline-move-closeout.md` | Post-audit | Close out the Clone Offline Player Characters move with project truth, runtime visibility, serialization, hook, verification, and rollback evidence. | Complete |


| `post-batch-h-pvp-consent-gate-review.csv` | Post-audit | Review and DeferredMoveGate disposition `RB-06806` for the POST-BATCH-H-08A PvP Consent reorganization gate. | Complete |
| `post-batch-h-pvp-consent-gate-closeout.md` | Post-audit | Close out the PvP Consent move gate with project truth, runtime hook, serialization, policy blocker, and next-action evidence. | Complete |


| `post-batch-h-monster-nests-move-review.csv` | Post-audit | Review and disposition `RB-06805` for the POST-BATCH-H-09A Monster Nests reorganization batch. | Complete |
| `post-batch-h-monster-nests-move-closeout.md` | Post-audit | Close out the Monster Nests move with project truth, runtime visibility, serialization, hook, verification, and rollback evidence. | Complete |


| `post-batch-h-invasion-gate-review.csv` | Post-audit | Review and DeferredMoveGate disposition `RB-06808` for the POST-BATCH-H-10A Invasion reorganization gate. | Complete |
| `post-batch-h-invasion-gate-closeout.md` | Post-audit | Close out the Invasion move gate with project truth, runtime hook, serialization, staff workflow blocker, and next-action evidence. | Complete |


| `post-batch-h-xmlspawner-gate-review.csv` | Post-audit | Review and NeedsHumanDecision disposition `RB-06813` for the POST-BATCH-H-11A XMLSpawner reorganization gate. | Complete |
| `post-batch-h-xmlspawner-gate-closeout.md` | Post-audit | Close out the XMLSpawner move gate with project truth, runtime hook, serialization, explicit approval blocker, and next-action evidence. | Complete |


| `post-batch-h-government-gate-review.csv` | Post-audit | Review and NeedsHumanDecision disposition `RB-06807` for the POST-BATCH-H-12A Government reorganization gate. | Complete |
| `post-batch-h-government-gate-closeout.md` | Post-audit | Close out the Government move gate with project truth, runtime hook, serialization, explicit approval blocker, and next-action evidence. | Complete |


| `post-batch-h-offline-skill-training-gate-review.csv` | Post-audit | Review and DeferredMoveGate disposition `RB-06803` for the POST-BATCH-H-13A Offline Skill Training serializer gate. | Complete |
| `post-batch-h-offline-skill-training-gate-closeout.md` | Post-audit | Close out the Offline Skill Training move gate with project truth, runtime hook, serialization, balance/doc blocker, and next-action evidence. | Complete |


| `post-batch-h-homestead-gate-review.csv` | Post-audit | Review and NeedsHumanDecision disposition `RB-06814` for the POST-BATCH-H-14A Homestead explicit approval gate. | Complete |
| `post-batch-h-homestead-gate-closeout.md` | Post-audit | Close out the Homestead move gate with project truth, runtime hook, serialization, nested AGENTS, explicit approval blocker, and next-action evidence. | Complete |


| `post-batch-h-folder-namespace-cleanup-closeout.md` | Post-audit | Final closeout for POST-BATCH-H, including all row dispositions, commit hash reconciliation, verification, and next-batch boundary. | Complete |

| `post-batch-i-servercore-save-compat-review.csv` | Post-audit | Review and reconcile the 19 residual P0 ServerCore save-compatibility rows against current source and POST-BATCH-B source-reviewed evidence. | Complete |
| `post-batch-i-servercore-save-compat-closeout.md` | Post-audit | Close out POST-BATCH-I with disposition counts, active-overlay reconciliation, P0 backlog verification, and source-regeneration boundary. | Complete |

| `post-batch-j-p1-save-compat-review.csv` | Post-audit | Review all 1,294 remaining P1 save-compatibility rows against current serializer-register source evidence and record triage dispositions. | Complete |
| `post-batch-j-p1-save-compat-closeout.md` | Post-audit | Close out POST-BATCH-J with disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |

| `post-batch-k-p1-runtime-surface-review.csv` | Post-audit | Review all 2,691 remaining P1 runtime-hook, gump guard, PlayerMobile coupling, region/map, and command-access rows against current source/runtime evidence. | Complete |
| `post-batch-k-p1-runtime-surface-closeout.md` | Post-audit | Close out POST-BATCH-K with category counts, disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |

| `post-batch-l-p2-residual-backlog-review.csv` | Post-audit | Review all 1,186 remaining P2 command-access, legacy-compatibility, save-compatibility, and region/map rows against current source/project/runtime evidence. | Complete |
| `post-batch-l-p2-residual-backlog-closeout.md` | Post-audit | Close out POST-BATCH-L with category counts, disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |

| `post-batch-m-command-access-source-review.csv` | Post-audit | Source review for the 174 POST-BATCH-L queued P2 command-access follow-ups, resolving helper access, parser offsets, legacy command surfaces, and policy-decision rows. | Complete |
| `post-batch-m-command-access-closeout.md` | Post-audit | Close out POST-BATCH-M with decision counts, review classes, human decision rows, active-overlay reconciliation, and verification notes. | Complete |
| `post-batch-n-source-readiness-queue.csv` | Post-audit | Classify all 899 formerly active `QueuedSourceFollowUp` rows into source-ready, docs-only, schema-docs-only, migration-plan, or policy-design lanes before source repair begins. | Complete |
| `post-batch-n-closeout.md` | Post-audit | Close out POST-BATCH-N with readiness counts, lane counts, gate policy, active-overlay reconciliation, and verification notes. | Complete |
| `post-batch-o-gump-guard-source-review.csv` | Post-audit | Source-review and repair dispositions for all 235 POST-BATCH-N gump guard source-batch rows. | Complete |
| `post-batch-o-gump-guard-closeout.md` | Post-audit | Close out POST-BATCH-O with source repair summary, decision counts, verification, and queue reconciliation. | Complete |
| `post-batch-p-runtime-hook-source-review.csv` | Post-audit | Source-review and repair dispositions for all 103 POST-BATCH-N runtime-hook source-batch rows. | Complete |
| `post-batch-p-runtime-hook-closeout.md` | Post-audit | Close out POST-BATCH-P with source repair summary, decision counts, verification, and queue reconciliation. | Complete |
| `post-batch-q-staff-command-metadata-source-review.csv` | Post-audit | Source-review and repair dispositions for all 92 POST-BATCH-N staff command metadata source-batch rows. | Complete |
| `post-batch-q-staff-command-metadata-closeout.md` | Post-audit | Close out POST-BATCH-Q with source repair summary, decision counts, verification, and queue reconciliation. | Complete |
| `post-batch-r-save-constructor-persistence-review.csv` | Post-audit | Source-review dispositions for all 23 POST-BATCH-N save constructor persistence rows. | Complete |
| `post-batch-r-save-constructor-persistence-closeout.md` | Post-audit | Close out POST-BATCH-R with constructor evidence, serializer alignment evidence, verification, and queue reconciliation. | Complete |
| `post-batch-s-schema-documentation-review.csv` | Post-audit | Schema/parser documentation dispositions for all 232 POST-BATCH-N schema documentation rows. | Complete |
| `post-batch-s-schema-documentation-closeout.md` | Post-audit | Close out POST-BATCH-S with schema evidence, diagnostic false positives, verification, and queue reconciliation. | Complete |


## Initial State

The Phase 0 baseline output, Phase 1 reproducible inventory outputs, Phase 2 project truth outputs, Phase 3 runtime inventory outputs, Phase 4 system cards, Phase 5 runtime hook map outputs, Phase 6 serialization/save-compatibility outputs, Phase 7 documentation truth outputs, Phase 8 dependency graph outputs, Phase 9 synergy/conflict outputs, Phase 10 risk-specific review outputs, Phase 11 inline code documentation outputs, Phase 12 reorganization design outputs, Phase 13 repair backlog outputs, and Phase 14 verification and commit workflow outputs have been generated. The audit runner records all phases as committed after the final status-record commit.

| post-batch-s-schema-documentation-closeout.md | Post-audit | Close out POST-BATCH-S with schema evidence, diagnostic false positives, verification, and queue reconciliation. | Complete |
| post-batch-t-docs-source-trace-review.csv | Post-audit | Documentation source-trace dispositions for all 133 POST-BATCH-N docs-only rows. | Complete |
| post-batch-t-docs-source-trace-closeout.md | Post-audit | Close out POST-BATCH-T with source-trace, alias, support-doc no-change, verification, and queue reconciliation. | Complete |
| post-batch-u-save-migration-plan-review.csv | Post-audit | Source-reviewed migration-plan dispositions for all 77 POST-BATCH-N save migration-plan rows. | Complete |
| post-batch-u-save-migration-plan-closeout.md | Post-audit | Close out POST-BATCH-U with migration-gate decisions, serializer evidence, verification, and queue reconciliation. | Complete |
| post-batch-v-region-policy-design-review.csv | Post-audit | Source-reviewed policy dispositions for all 4 POST-BATCH-N region policy-design rows. | Complete |
| post-batch-v-region-policy-design-closeout.md | Post-audit | Close out POST-BATCH-V with region policy decisions, false-positive evidence, verification, and queue reconciliation. | Complete |
| post-batch-w-historical-save-migration-plan-review.csv | Post-audit | Source-reviewed migration-plan dispositions for all 73 historical POST-BATCH-J save-compatibility NeedsMigrationPlan rows. | Complete |
| post-batch-w-historical-save-migration-plan-closeout.md | Post-audit | Close out POST-BATCH-W with helper serializer no-change decisions, scanner ambiguity false positives, the HouseFoundation legacy base-order migration plan, verification, and queue reconciliation. | Complete |
| post-batch-x-playermobile-coupling-policy-review.csv | Post-audit | Source-evidenced policy dispositions for all 383 historical POST-BATCH-K PlayerMobile coupling DeferredPolicyDecision rows. | Complete |
| post-batch-x-playermobile-coupling-policy-closeout.md | Post-audit | Close out POST-BATCH-X with PlayerMobile coupling policy decisions, false-positive evidence, verification, and overlay reconciliation. | Complete |
| post-batch-y-source-change-gate-register.csv | Post-audit | Residual source-change gate register for all 90 remaining human, balance, region, move, and migration gates before broad source work. | Complete |
| post-batch-y-source-change-readiness-closeout.md | Post-audit | Close out POST-BATCH-Y with accepted fences, domain-only blockers, first source-change boundary, verification, and overlay reconciliation. | Complete |
| post-batch-z-first-source-change-selection.csv | Post-audit | Select the first allowed source-change boundary after reconciling Phase 13 batch order, active overlay dispositions, and POST-BATCH-Y fences. | Complete |
| post-batch-z-first-source-change-selection-closeout.md | Post-audit | Close out POST-BATCH-Z with the selected non-gated source-change boundary, exclusions, verification requirements, and ready SOURCE-BATCH-001 goal command. | Complete |
| post-batch-aa-source-batch-roadmap.csv | Post-audit | Sequential roadmap for the remaining logical source-change batches using the active overlay and POST-BATCH-Y/Z fences. | Complete |
| post-batch-aa-source-batch-roadmap-closeout.md | Post-audit | Close out POST-BATCH-AA with roadmap order, gate evidence, and ready goal templates for non-gated and gated source batches. | Complete |
| source-batch-controller-roadmap-status.csv | Source batches | Controller status for POST-BATCH-AA roadmap rows plus executed non-gated source batches and the next pending repeatable non-gated target. | Complete |
| source-batch-controller-closeout.md | Source batches | Close out source-batch controller, intake, and source execution updates, including next non-gated input and gated approval blockers. | Complete |
| source-change-executive-decision-intake.csv | Source decisions | Excel-openable executive decision register for remaining source-change gates, including plain-English context, final executive decisions, risks, automation guidance, and implementation constraints. | Complete |
| source-batch-004-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch from EXEC-0001, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-004-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-004 candidate discovery with the recommended ArcaneGem target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-001-target.md | Source batches | Durable interview target for SOURCE-BATCH-001 OilCloth guard repair, including fence result, unchanged behavior, and ready goal command. | Complete |
| source-batch-intake-register.csv | Source batches | Structured intake register for executed non-gated source batches, the next pending non-gated target, and gated approval status. | Complete |
| source-batch-001-oilcloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-001 with OilCloth source guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-002-target.md | Source batches | Durable target for SOURCE-BATCH-002 OilCloth dye/scissor guard repair, including fence result, unchanged behavior, and ready goal command. | Complete |
| source-batch-002-oilcloth-dye-scissor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-002 with OilCloth dye/scissor guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-003-target.md | Source batches | Durable target for SOURCE-BATCH-003 Firebomb interaction guard repair, including fence result, active overlay result, unchanged behavior, and verification plan. | Complete |
| source-batch-003-firebomb-interaction-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-003 with Firebomb interaction guard changes, POST-BATCH-Y fence evidence, serializer/timer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-004-target.md | Source batches | Durable target for SOURCE-BATCH-004 ArcaneGem interaction guard repair, including fence result, active overlay result, unchanged behavior, and verification plan. | Complete |
| source-batch-004-arcanegem-interaction-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-004 with ArcaneGem interaction guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-005-target.md | Source batches | Durable target for SOURCE-BATCH-005 PowerCrystal target guard repair, including fence result, active overlay result, unchanged behavior, and verification plan. | Complete |
| source-batch-005-powercrystal-target-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-005 with PowerCrystal target guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-006-target.md | Source batches | Durable target for SOURCE-BATCH-006 ClockworkAssembly guard repair, including fence result, active overlay result, unchanged behavior, and verification plan. | Complete |
| source-batch-006-clockworkassembly-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-006 with ClockworkAssembly guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-007-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-006, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-007-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-007 candidate discovery with the recommended UnusualDyes target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-007-target.md | Source batches | Durable target for SOURCE-BATCH-007 UnusualDyes target guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-007-unusualdyes-target-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-007 with UnusualDyes target guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-008-target.md | Source batches | Durable target for SOURCE-BATCH-008 VelocityDeed guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-008-velocitydeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-008 with VelocityDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-009-target.md | Source batches | Durable target for SOURCE-BATCH-009 WeaponRenamingTool guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-009-weaponrenamingtool-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-009 with WeaponRenamingTool guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-010-target.md | Source batches | Durable target for SOURCE-BATCH-010 Scales guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-010-scales-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-010 with Scales guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-011-target.md | Source batches | Durable target for SOURCE-BATCH-011 MagicScissors guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-011-magicscissors-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-011 with MagicScissors guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-012-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-011, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-012-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-012 candidate discovery with the recommended BalancingDeed target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-012-target.md | Source batches | Durable target for SOURCE-BATCH-012 BalancingDeed guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-012-balancingdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-012 with BalancingDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-013-target.md | Source batches | Durable target for SOURCE-BATCH-013 HydraTooth guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-013-hydratooth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-013 with HydraTooth guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-014-target.md | Source batches | Durable target for SOURCE-BATCH-014 MagicHammer guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-014-magichammer-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-014 with MagicHammer guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-015-target.md | Source batches | Durable target for SOURCE-BATCH-015 BookofDead guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-015-bookofdead-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-015 with BookofDead guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-016-target.md | Source batches | Durable target for SOURCE-BATCH-016 MagicPigment guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-016-magicpigment-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-016 with MagicPigment guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-017-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-016, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-017-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-017 candidate discovery with the recommended PromotionalToken target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-017-target.md | Source batches | Durable target for SOURCE-BATCH-017 PromotionalToken guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-017-promotionaltoken-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-017 with PromotionalToken guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-018-target.md | Source batches | Durable target for SOURCE-BATCH-018 MagicalDyes guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-018-magicaldyes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-018 with MagicalDyes guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-019-target.md | Source batches | Durable target for SOURCE-BATCH-019 AllDyeTubsArmor guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-019-alldyetubsarmor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-019 with AllDyeTubsArmor guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-020-target.md | Source batches | Durable target for SOURCE-BATCH-020 AllDyeTubsWeapon guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-020-alldyetubsweapon-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-020 with AllDyeTubsWeapon guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-021-target.md | Source batches | Durable target for SOURCE-BATCH-021 AllDyeTubsFurniture guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-021-alldyetubsfurniture-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-021 with AllDyeTubsFurniture guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-022-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-021, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-022-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-022 candidate discovery with the recommended AllDyeTubsBookRune target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-022-target.md | Source batches | Durable target for SOURCE-BATCH-022 AllDyeTubsBookRune guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-022-alldyetubsbookrune-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-022 with AllDyeTubsBookRune guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-023-target.md | Source batches | Durable target for SOURCE-BATCH-023 AllDyeTubsBookSpell guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-023-alldyetubsbookspell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-023 with AllDyeTubsBookSpell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-024-target.md | Source batches | Durable target for SOURCE-BATCH-024 AllDyeTubsMountEthereal guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-024-alldyetubsmountethereal-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-024 with AllDyeTubsMountEthereal guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-025-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-024, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-025-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-025 candidate discovery with the recommended LuckyHorseShoes target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-025-target.md | Source batches | Durable target for SOURCE-BATCH-025 LuckyHorseShoes guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-025-luckyhorseshoes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-025 with LuckyHorseShoes guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-026-target.md | Source batches | Durable target for SOURCE-BATCH-026 SlayerDeed guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-026-slayerdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-026 with SlayerDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-027-target.md | Source batches | Durable target for SOURCE-BATCH-027 ArtifactManual guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-027-artifactmanual-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-027 with ArtifactManual guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-028-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-027, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-028-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-028 candidate discovery with the recommended DyeTub target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-028-target.md | Source batches | Durable target for SOURCE-BATCH-028 DyeTub guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-028-dyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-028 with DyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-029-target.md | Source batches | Durable target for SOURCE-BATCH-029 Key interaction guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-029-key-interaction-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-029 with Key interaction guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-030-target.md | Source batches | Durable target for SOURCE-BATCH-030 PuzzleCube guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-030-puzzlecube-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-030 with PuzzleCube guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-031-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-030, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-031-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-031 candidate discovery with the recommended Dice4 target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-031-target.md | Source batches | Durable target for SOURCE-BATCH-031 Dice4 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-031-dice4-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-031 with Dice4 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-032-target.md | Source batches | Durable target for SOURCE-BATCH-032 Dice6 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-032-dice6-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-032 with Dice6 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-033-target.md | Source batches | Durable target for SOURCE-BATCH-033 Dice8 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-033-dice8-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-033 with Dice8 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-034-target.md | Source batches | Durable target for SOURCE-BATCH-034 Dice10 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-034-dice10-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-034 with Dice10 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-035-target.md | Source batches | Durable target for SOURCE-BATCH-035 Dice12 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-035-dice12-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-035 with Dice12 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-036-target.md | Source batches | Durable target for SOURCE-BATCH-036 Dice20 guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-036-dice20-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-036 with Dice20 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-037-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-036, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-037-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-037 candidate discovery with the recommended EverlastingBottle target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-037-target.md | Source batches | Durable target for SOURCE-BATCH-037 EverlastingBottle guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-037-everlastingbottle-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-037 with EverlastingBottle guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-038-target.md | Source batches | Durable target for SOURCE-BATCH-038 EverlastingLoaf guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-038-everlastingloaf-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-038 with EverlastingLoaf guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-039-target.md | Source batches | Durable target for SOURCE-BATCH-039 MusicBox guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-039-musicbox-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-039 with MusicBox guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-040-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-039, including gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-040-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-040 candidate discovery with the recommended RewardBlackDyeTub target, candidate summary, exclusions, and verification notes. | Complete |
| source-batch-040-target.md | Source batches | Durable target for SOURCE-BATCH-040 RewardBlackDyeTub guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-040-rewardblackdyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-040 with RewardBlackDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-041-target.md | Source batches | Durable target for SOURCE-BATCH-041 SpecialDyeTub guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-041-specialdyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-041 with SpecialDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-042-target.md | Source batches | Durable target for SOURCE-BATCH-042 LeatherDyeTub guard repair, including fence result, active overlay result, unchanged behavior, and ready goal command. | Complete |
| source-batch-042-leatherdyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-042 with LeatherDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-043-target.md | Source batches | Durable target for SOURCE-BATCH-043 FurnitureDyeTub guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-043-furnituredyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-043 with FurnitureDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-044-target.md | Source batches | Durable target for SOURCE-BATCH-044 RunebookDyeTub guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-044-runebookdyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-044 with RunebookDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-045-target.md | Source batches | Durable target for SOURCE-BATCH-045 StatuetteDyeTub guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-045-statuettedyetub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-045 with StatuetteDyeTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-046-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-045, including OilMetal, OilLeather, and OilWood gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-046-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-046 candidate discovery with the recommended OilMetal target, oil-material candidate summary, exclusions, and verification notes. | Complete |
| source-batch-046-target.md | Source batches | Durable target for SOURCE-BATCH-046 OilMetal guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-046-oilmetal-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-046 with OilMetal guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-047-target.md | Source batches | Durable target for SOURCE-BATCH-047 OilLeather guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-047-oilleather-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-047 with OilLeather guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-048-target.md | Source batches | Durable target for SOURCE-BATCH-048 OilWood guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-048-oilwood-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-048 with OilWood guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-049-candidate-discovery.csv | Source batches | Candidate list for the next non-gated source batch after SOURCE-BATCH-048, including gem-specific oil gate evidence, active overlay evidence, risks, verification, and unchanged-behavior constraints. | Complete |
| source-batch-049-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-049 candidate discovery with the recommended OilAmethyst target, gem-specific oil candidate summary, exclusions, and verification notes. | Complete |
| source-batch-049-target.md | Source batches | Durable target for SOURCE-BATCH-049 OilAmethyst guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-049-oilamethyst-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-049 with OilAmethyst guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-050-target.md | Source batches | Durable target for SOURCE-BATCH-050 OilCaddellite guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-050-oilcaddellite-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-050 with OilCaddellite guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-051-target.md | Source batches | Durable target for SOURCE-BATCH-051 OilEmerald guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-051-oilemerald-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-051 with OilEmerald guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-052-target.md | Source batches | Durable target for SOURCE-BATCH-052 OilGarnet guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-052-oilgarnet-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-052 with OilGarnet guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-053-target.md | Source batches | Durable target for SOURCE-BATCH-053 OilIce guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-053-oilice-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-053 with OilIce guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-054-target.md | Source batches | Durable target for SOURCE-BATCH-054 OilJade guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-054-oiljade-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-054 with OilJade guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-055-target.md | Source batches | Durable target for SOURCE-BATCH-055 OilMarble guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-055-oilmarble-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-055 with OilMarble guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-056-target.md | Source batches | Durable target for SOURCE-BATCH-056 OilOnyx guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-056-oilonyx-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-056 with OilOnyx guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-057-target.md | Source batches | Durable target for SOURCE-BATCH-057 OilQuartz guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-057-oilquartz-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-057 with OilQuartz guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-058-target.md | Source batches | Durable target for SOURCE-BATCH-058 OilRuby guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-058-oilruby-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-058 with OilRuby guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-059-target.md | Source batches | Durable target for SOURCE-BATCH-059 OilSapphire guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-059-oilsapphire-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-059 with OilSapphire guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-060-target.md | Source batches | Durable target for SOURCE-BATCH-060 OilSilver guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-060-oilsilver-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-060 with OilSilver guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-061-target.md | Source batches | Durable target for SOURCE-BATCH-061 OilSpinel guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-061-oilspinel-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-061 with OilSpinel guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-062-target.md | Source batches | Durable target for SOURCE-BATCH-062 OilStarRuby guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-062-oilstarruby-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-062 with OilStarRuby guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-063-target.md | Source batches | Durable target for SOURCE-BATCH-063 OilTopaz guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-063-oiltopaz-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-063 with OilTopaz guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-064-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-064+ after the oil-material queue was exhausted; includes GlassblowingBook, SandMiningBook, SmokeBomb, and EggBomb candidates with gate/overlay evidence. | Complete |
| source-batch-064-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-064 candidate discovery with recommended target and exclusion notes. | Complete |
| source-batch-064-target.md | Source batches | Durable target for SOURCE-BATCH-064 GlassblowingBook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-064-glassblowingbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-064 with GlassblowingBook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-065-target.md | Source batches | Durable target for SOURCE-BATCH-065 SandMiningBook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-065-sandminingbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-065 with SandMiningBook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-066-target.md | Source batches | Durable target for SOURCE-BATCH-066 SmokeBomb guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-066-smokebomb-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-066 with SmokeBomb guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-067-target.md | Source batches | Durable target for SOURCE-BATCH-067 EggBomb guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-067-eggbomb-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-067 with EggBomb guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, and recorded build/runtime verification limitations. | Complete |
| source-batch-068-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-068+ after SOURCE-BATCH-067 restored verification availability; includes SkeletonsKey, MagicSkeltonsKey, and MasterSkeletonsKey candidates with gate/overlay evidence. | Complete |
| source-batch-068-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-068 candidate discovery with recommended target and verification recovery notes. | Complete |
| source-batch-068-target.md | Source batches | Durable target for SOURCE-BATCH-068 SkeletonsKey guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-068-skeletonskey-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-068 with SkeletonsKey guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-069-target.md | Source batches | Durable target for SOURCE-BATCH-069 MagicSkeltonsKey guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-069-magicskeltonskey-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-069 with MagicSkeltonsKey guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-070-target.md | Source batches | Durable target for SOURCE-BATCH-070 MasterSkeletonsKey guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-070-masterskeletonskey-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-070 with MasterSkeletonsKey guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-071-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-071+ after the skeleton-key queue was exhausted; includes DecoStatueDeed and MonsterStatueDeed candidates with gate/overlay evidence. | Complete |
| source-batch-071-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-071 candidate discovery with the recommended DecoStatueDeed target and exclusion notes. | Complete |
| source-batch-071-target.md | Source batches | Durable target for SOURCE-BATCH-071 DecoStatueDeed guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-071-decostatuedeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-071 with DecoStatueDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-072-target.md | Source batches | Durable target for SOURCE-BATCH-072 MonsterStatueDeed guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-072-monsterstatuedeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-072 with MonsterStatueDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-073-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-073+ after the decoration-deed queue was exhausted; includes MasonryBook and StoneMiningBook candidates with gate/overlay evidence. | Complete |
| source-batch-073-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-073 candidate discovery with the recommended MasonryBook target and exclusion notes. | Complete |
| source-batch-073-target.md | Source batches | Durable target for SOURCE-BATCH-073 MasonryBook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-073-masonrybook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-073 with MasonryBook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-074-target.md | Source batches | Durable target for SOURCE-BATCH-074 StoneMiningBook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-074-stoneminingbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-074 with StoneMiningBook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-075-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-075+ after the specialized learning-book queue was exhausted; includes DwarvenForge with gate/overlay evidence. | Complete |
| source-batch-075-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-075 candidate discovery with the recommended DwarvenForge target and exclusion notes. | Complete |
| source-batch-075-target.md | Source batches | Durable target for SOURCE-BATCH-075 DwarvenForge guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-075-dwarvenforge-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-075 with DwarvenForge guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-076-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-076+ after the DwarvenForge queue was exhausted; includes TaxidermyKit with gate/overlay evidence. | Complete |
| source-batch-076-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-076 candidate discovery with the recommended TaxidermyKit target and exclusion notes. | Complete |
| source-batch-076-target.md | Source batches | Durable target for SOURCE-BATCH-076 TaxidermyKit guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-076-taxidermykit-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-076 with TaxidermyKit guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-077-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-077+ after the TaxidermyKit queue was exhausted; includes MysticalPearl with gate/overlay evidence. | Complete |
| source-batch-077-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-077 candidate discovery with the recommended MysticalPearl target and exclusion notes. | Complete |
| source-batch-077-target.md | Source batches | Durable target for SOURCE-BATCH-077 MysticalPearl guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-077-mysticalpearl-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-077 with MysticalPearl guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-078-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-078+ after the MysticalPearl queue was exhausted; includes CrystallineJar, BottleOfAcid, and RepairDeed candidates with gate/overlay evidence. | Complete |
| source-batch-078-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-078 candidate discovery with the recommended CrystallineJar target and lower-priority candidate notes. | Complete |
| source-batch-078-target.md | Source batches | Durable target for SOURCE-BATCH-078 CrystallineJar guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-078-crystallinejar-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-078 with CrystallineJar guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-079-target.md | Source batches | Durable target for SOURCE-BATCH-079 BottleOfAcid guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-079-bottleofacid-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-079 with BottleOfAcid guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-080-target.md | Source batches | Durable target for SOURCE-BATCH-080 RepairDeed guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-080-repairdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-080 with RepairDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-081-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-081+ after the RepairDeed queue was exhausted; includes ArrowsAndBolts with gate/overlay evidence. | Complete |
| source-batch-081-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-081 candidate discovery with the recommended ArrowsAndBolts target and exclusion notes. | Complete |
| source-batch-081-target.md | Source batches | Durable target for SOURCE-BATCH-081 ArrowsAndBolts guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-081-arrowsandbolts-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-081 with ArrowsAndBolts guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-082-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-082+ after the ArrowsAndBolts queue was exhausted; includes ClothingBlessDeed and HairRestylingDeed candidates with gate/overlay evidence. | Complete |
| source-batch-082-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-082 candidate discovery with the recommended ClothingBlessDeed target and next-candidate notes. | Complete |
| source-batch-082-target.md | Source batches | Durable target for SOURCE-BATCH-082 ClothingBlessDeed guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-082-clothingblessdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-082 with ClothingBlessDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-083-target.md | Source batches | Durable target for SOURCE-BATCH-083 HairRestylingDeed guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-083-hairrestylingdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-083 with HairRestylingDeed guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-084-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-084+ after the HairRestylingDeed queue was exhausted; includes PotionOfWisdom, PotionOfMight, and PotionOfDexterity candidates with gate/overlay evidence. | Complete |
| source-batch-084-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-084 candidate discovery with the recommended PotionOfWisdom target and next-candidate notes. | Complete |
| source-batch-084-target.md | Source batches | Durable target for SOURCE-BATCH-084 PotionOfWisdom guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-084-potionofwisdom-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-084 with PotionOfWisdom guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-085-target.md | Source batches | Durable target for SOURCE-BATCH-085 PotionOfMight guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-085-potionofmight-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-085 with PotionOfMight guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-086-target.md | Source batches | Durable target for SOURCE-BATCH-086 PotionOfDexterity guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-086-potionofdexterity-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-086 with PotionOfDexterity guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-087-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-087+ after the stat-potion queue was exhausted; includes HairDyePotion, HairDyeBottle, GenderPotion, NecroSkinPotion, and HairOilPotion candidates with gate/overlay evidence. | Complete |
| source-batch-087-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-087 candidate discovery with the recommended HairDyePotion target and next-candidate notes. | Complete |
| source-batch-087-target.md | Source batches | Durable target for SOURCE-BATCH-087 HairDyePotion guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-087-hairdyepotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-087 with HairDyePotion guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-088-target.md | Source batches | Durable target for SOURCE-BATCH-088 HairDyeBottle guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-088-hairdyebottle-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-088 with HairDyeBottle guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-089-target.md | Source batches | Durable target for SOURCE-BATCH-089 GenderPotion guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-089-genderpotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-089 with GenderPotion guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-090-target.md | Source batches | Durable target for SOURCE-BATCH-090 NecroSkinPotion guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-090-necroskinpotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-090 with NecroSkinPotion guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-091-target.md | Source batches | Durable target for SOURCE-BATCH-091 HairOilPotion guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-091-hairoilpotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-091 with HairOilPotion guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-092-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-092+ after the appearance-potion queue was exhausted; includes HueStone with gate/overlay evidence. | Complete |
| source-batch-092-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-092 candidate discovery with the recommended HueStone target and exclusion notes. | Complete |
| source-batch-092-target.md | Source batches | Durable target for SOURCE-BATCH-092 HueStone guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-092-huestone-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-092 with HueStone guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-093-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-093+ after the HueStone queue was exhausted; includes BloodDrink, FreshBrain, TastyHeart, BakedBread, WaterFlask, and WaterVial candidates with gate/overlay evidence. | Complete |
| source-batch-093-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-093 candidate discovery with the recommended BloodDrink target and next-candidate notes. | Complete |
| source-batch-093-target.md | Source batches | Durable target for SOURCE-BATCH-093 BloodDrink guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-093-blooddrink-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-093 with BloodDrink guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-094-target.md | Source batches | Durable target for SOURCE-BATCH-094 FreshBrain guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-094-freshbrain-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-094 with FreshBrain guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-095-target.md | Source batches | Durable target for SOURCE-BATCH-095 TastyHeart guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-095-tastyheart-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-095 with TastyHeart guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-096-target.md | Source batches | Durable target for SOURCE-BATCH-096 BakedBread guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-096-bakedbread-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-096 with BakedBread guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-097-target.md | Source batches | Durable target for SOURCE-BATCH-097 WaterFlask guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-097-waterflask-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-097 with WaterFlask guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-098-target.md | Source batches | Durable target for SOURCE-BATCH-098 WaterVial guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-098-watervial-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-098 with WaterVial guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-099-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-099+ after the food/drink queue was exhausted; includes Wool, Cotton, Flax, YarnsAndThreads, and PolishBoneBrush candidates with gate/overlay evidence. | Complete |
| source-batch-099-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-099 candidate discovery with the recommended Wool target and exclusion notes. | Complete |
| source-batch-099-target.md | Source batches | Durable target for SOURCE-BATCH-099 Wool guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-099-wool-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-099 with Wool guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-100-target.md | Source batches | Durable target for SOURCE-BATCH-100 Cotton guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-100-cotton-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-100 with Cotton guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-101-target.md | Source batches | Durable target for SOURCE-BATCH-101 Flax guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-101-flax-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-101 with Flax guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-102-target.md | Source batches | Durable target for SOURCE-BATCH-102 YarnsAndThreads guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-102-yarnsandthreads-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-102 with YarnsAndThreads guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-103-target.md | Source batches | Durable target for SOURCE-BATCH-103 PolishBoneBrush guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-103-polishbonebrush-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-103 with PolishBoneBrush guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-104-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-104+ after the PolishBoneBrush queue was exhausted; includes Cloth, BoltOfCloth, and UncutCloth candidates with gate/overlay evidence. | Complete |
| source-batch-104-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-104 candidate discovery with the recommended Cloth target and next-candidate notes. | Complete |
| source-batch-104-target.md | Source batches | Durable target for SOURCE-BATCH-104 Cloth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-104-cloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-104 with Cloth guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-105-target.md | Source batches | Durable target for SOURCE-BATCH-105 BoltOfCloth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-105-boltofcloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-105 with BoltOfCloth guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-106-target.md | Source batches | Durable target for SOURCE-BATCH-106 UncutCloth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-106-uncutcloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-106 with UncutCloth guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-107-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-107+ after the tailoring cloth queue was exhausted; includes CaddelliteOre, RareMetals, HardScales, and HardCrystals candidates with gate/overlay evidence. | Complete |
| source-batch-107-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-107 candidate discovery with the recommended CaddelliteOre target and next-candidate notes. | Complete |
| source-batch-107-target.md | Source batches | Durable target for SOURCE-BATCH-107 CaddelliteOre guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-107-caddelliteore-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-107 with CaddelliteOre guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-108-target.md | Source batches | Durable target for SOURCE-BATCH-108 RareMetals guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-108-raremetals-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-108 with RareMetals guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-109-target.md | Source batches | Durable target for SOURCE-BATCH-109 HardScales guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-109-hardscales-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-109 with HardScales guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-110-target.md | Source batches | Durable target for SOURCE-BATCH-110 HardCrystals guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-110-hardcrystals-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-110 with HardCrystals guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-111-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-111+ after the blacksmithing resource queue was exhausted; includes eight trap/junk item candidates with gate/overlay evidence. | Complete |
| source-batch-111-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-111 candidate discovery with the recommended BrokenGear target and next-candidate notes. | Complete |
| source-batch-111-target.md | Source batches | Durable target for SOURCE-BATCH-111 BrokenGear guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-111-brokengear-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-111 with BrokenGear guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-112-target.md | Source batches | Durable target for SOURCE-BATCH-112 CurseItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-112-curseitem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-112 with CurseItem guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-113-target.md | Source batches | Durable target for SOURCE-BATCH-113 TaintedBandage guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-113-taintedbandage-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-113 with TaintedBandage guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-114-target.md | Source batches | Durable target for SOURCE-BATCH-114 WeedItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-114-weeditem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-114 with WeedItem guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-115-target.md | Source batches | Durable target for SOURCE-BATCH-115 SlimeItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-115-slimeitem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-115 with SlimeItem guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-116-target.md | Source batches | Durable target for SOURCE-BATCH-116 SewageItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-116-sewageitem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-116 with SewageItem guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-117-target.md | Source batches | Durable target for SOURCE-BATCH-117 RottedReagents guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-117-rottedreagents-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-117 with RottedReagents guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-118-target.md | Source batches | Durable target for SOURCE-BATCH-118 RuinedGems guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-118-ruinedgems-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-118 with RuinedGems guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-119-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-119+ after the trap/junk queue was exhausted; includes 25 Pagan reagent decorative rare candidates with gate/overlay evidence. | Complete |
| source-batch-119-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-119 candidate discovery with the recommended DecoBlackmoor target and next-candidate notes. | Complete |
| source-batch-119-target.md | Source batches | Durable target for SOURCE-BATCH-119 DecoBlackmoor guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-119-decoblackmoor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-119 with DecoBlackmoor guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-120-target.md | Source batches | Durable target for SOURCE-BATCH-120 DecoBloodspawn guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-120-decobloodspawn-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-120 with DecoBloodspawn guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-121-target.md | Source batches | Durable target for SOURCE-BATCH-121 DecoBrimstone guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-121-decobrimstone-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-121 with DecoBrimstone guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-122-target.md | Source batches | Durable target for SOURCE-BATCH-122 DecoDragonsBlood guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-122-decodragonsblood-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-122 with DecoDragonsBlood guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-123-target.md | Source batches | Durable target for SOURCE-BATCH-123 DecoDragonsBlood2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-123-decodragonsblood2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-123 with DecoDragonsBlood2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-124-target.md | Source batches | Durable target for SOURCE-BATCH-124 DecoEyeOfNewt guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-124-decoeyeofnewt-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-124 with DecoEyeOfNewt guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-125-target.md | Source batches | Durable target for SOURCE-BATCH-125 DecoGarlic guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-125-decogarlic-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-125 with DecoGarlic guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-126-target.md | Source batches | Durable target for SOURCE-BATCH-126 DecoGarlic2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-126-decogarlic2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-126 with DecoGarlic2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-127-target.md | Source batches | Durable target for SOURCE-BATCH-127 DecoGarlicBulb guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-127-decogarlicbulb-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-127 with DecoGarlicBulb guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-128-target.md | Source batches | Durable target for SOURCE-BATCH-128 DecoGarlicBulb2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-128-decogarlicbulb2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-128 with DecoGarlicBulb2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-129-target.md | Source batches | Durable target for SOURCE-BATCH-129 DecoGinseng guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-129-decoginseng-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-129 with DecoGinseng guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-130-target.md | Source batches | Durable target for SOURCE-BATCH-130 DecoGinseng2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-130-decoginseng2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-130 with DecoGinseng2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-131-target.md | Source batches | Durable target for SOURCE-BATCH-131 DecoGinsengRoot guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-131-decoginsengroot-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-131 with DecoGinsengRoot guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-132-target.md | Source batches | Durable target for SOURCE-BATCH-132 DecoGinsengRoot2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-132-decoginsengroot2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-132 with DecoGinsengRoot2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-133-target.md | Source batches | Durable target for SOURCE-BATCH-133 DecoMandrake guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-133-decomandrake-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-133 with DecoMandrake guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-134-target.md | Source batches | Durable target for SOURCE-BATCH-134 DecoMandrake2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-134-decomandrake2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-134 with DecoMandrake2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-135-target.md | Source batches | Durable target for SOURCE-BATCH-135 DecoMandrake3 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-135-decomandrake3-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-135 with DecoMandrake3 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-136-target.md | Source batches | Durable target for SOURCE-BATCH-136 DecoMandrakeRoot guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-136-decomandrakeroot-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-136 with DecoMandrakeRoot guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-137-target.md | Source batches | Durable target for SOURCE-BATCH-137 DecoMandrakeRoot2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-137-decomandrakeroot2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-137 with DecoMandrakeRoot2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-138-target.md | Source batches | Durable target for SOURCE-BATCH-138 DecoNightshade guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-138-deconightshade-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-138 with DecoNightshade guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-139-target.md | Source batches | Durable target for SOURCE-BATCH-139 DecoNightshade2 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-139-deconightshade2-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-139 with DecoNightshade2 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-140-target.md | Source batches | Durable target for SOURCE-BATCH-140 DecoNightshade3 guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-140-deconightshade3-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-140 with DecoNightshade3 guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-141-target.md | Source batches | Durable target for SOURCE-BATCH-141 DecoObsidian guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-141-decoobsidian-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-141 with DecoObsidian guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-142-target.md | Source batches | Durable target for SOURCE-BATCH-142 DecoPumice guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-142-decopumice-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-142 with DecoPumice guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-143-target.md | Source batches | Durable target for SOURCE-BATCH-143 DecoWyrmsHeart guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-143-decowyrmsheart-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-143 with DecoWyrmsHeart guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-144-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-144+ after the Pagan reagent queue was exhausted; includes ten zero-gate, zero-overlay guard candidates prioritized around leaf loot-container drag-lift repairs. | Complete |
| source-batch-144-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-144 candidate discovery with the recommended CorpseChest target and next-candidate notes. | Complete |
| source-batch-144-target.md | Source batches | Durable target for SOURCE-BATCH-144 CorpseChest guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-144-corpsechest-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-144 with CorpseChest guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-145-target.md | Source batches | Durable target for SOURCE-BATCH-145 CorpseSailor guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-145-corpsesailor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-145 with CorpseSailor guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-146-target.md | Source batches | Durable target for SOURCE-BATCH-146 LootBag guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-146-lootbag-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-146 with LootBag guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-147-target.md | Source batches | Durable target for SOURCE-BATCH-147 LootChest guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-147-lootchest-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-147 with LootChest guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-148-target.md | Source batches | Durable target for SOURCE-BATCH-148 PirateChest guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-148-piratechest-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-148 with PirateChest guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-149-target.md | Source batches | Durable target for SOURCE-BATCH-149 SunkenBag guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-149-sunkenbag-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-149 with SunkenBag guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-150-target.md | Source batches | Durable target for SOURCE-BATCH-150 MovingBox guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-150-movingbox-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-150 with MovingBox guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-151-target.md | Source batches | Durable target for SOURCE-BATCH-151 AlchemistPouch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-151-alchemistpouch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-151 with AlchemistPouch guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-152-target.md | Source batches | Durable target for SOURCE-BATCH-152 BaseMagicStaff guard repair, including fresh preflight evidence and skipped overlay-conflict decision. | Complete |
| source-batch-152-basemagicstaff-skip-closeout.md | Source batches | Close out SOURCE-BATCH-152 as skipped before source edits because active post-audit overlay rows still reference BaseMagicStaff.cs. | Complete |
| source-batch-153-target.md | Source batches | Durable target for SOURCE-BATCH-153 WindRunnerScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-153-windrunnerscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-153 with WindRunnerScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-154-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-154+ after the prior candidate queue was exhausted; includes nine zero-gate, zero-overlay Mystic scroll guard candidates. | Complete |
| source-batch-154-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-154 candidate discovery with the recommended AstralProjectionScroll target and next-candidate notes. | Complete |
| source-batch-154-target.md | Source batches | Durable target for SOURCE-BATCH-154 AstralProjectionScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-154-astralprojectionscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-154 with AstralProjectionScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-155-target.md | Source batches | Durable target for SOURCE-BATCH-155 AstralTravelScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-155-astraltravelscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-155 with AstralTravelScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-156-target.md | Source batches | Durable target for SOURCE-BATCH-156 CreateRobeScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-156-createrobescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-156 with CreateRobeScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-157-target.md | Source batches | Durable target for SOURCE-BATCH-157 GentleTouchScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-157-gentletouchscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-157 with GentleTouchScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-158-target.md | Source batches | Durable target for SOURCE-BATCH-158 LeapScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-158-leapscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-158 with LeapScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-159-target.md | Source batches | Durable target for SOURCE-BATCH-159 PsionicBlastScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-159-psionicblastscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-159 with PsionicBlastScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-160-target.md | Source batches | Durable target for SOURCE-BATCH-160 PsychicWallScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-160-psychicwallscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-160 with PsychicWallScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-161-target.md | Source batches | Durable target for SOURCE-BATCH-161 PurityOfBodyScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-161-purityofbodyscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-161 with PurityOfBodyScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-162-target.md | Source batches | Durable target for SOURCE-BATCH-162 QuiveringPalmScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-162-quiveringpalmscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-162 with QuiveringPalmScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-163-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-163+ after the Mystic scroll queue was exhausted; includes sixteen zero-gate, zero-overlay Bard scroll guard candidates. | Complete |
| source-batch-163-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-163 candidate discovery with the recommended ArmysPaeonScroll target and next-candidate notes. | Complete |
| source-batch-163-target.md | Source batches | Durable target for SOURCE-BATCH-163 ArmysPaeonScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-163-armyspaeonscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-163 with ArmysPaeonScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-164-target.md | Source batches | Durable target for SOURCE-BATCH-164 EnchantingEtudeScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-164-enchantingetudescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-164 with EnchantingEtudeScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-165-target.md | Source batches | Durable target for SOURCE-BATCH-165 EnergyCarolScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-165-energycarolscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-165 with EnergyCarolScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-166-target.md | Source batches | Durable target for SOURCE-BATCH-166 EnergyThrenodyScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-166-energythrenodyscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-166 with EnergyThrenodyScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-167-target.md | Source batches | Durable target for SOURCE-BATCH-167 FireCarolScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-167-firecarolscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-167 with FireCarolScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-168-target.md | Source batches | Durable target for SOURCE-BATCH-168 FireThrenodyScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-168-firethrenodyscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-168 with FireThrenodyScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-169-target.md | Source batches | Durable target for SOURCE-BATCH-169 FoeRequiemScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-169-foerequiemscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-169 with FoeRequiemScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-170-target.md | Source batches | Durable target for SOURCE-BATCH-170 IceCarolScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-170-icecarolscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-170 with IceCarolScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-171-target.md | Source batches | Durable target for SOURCE-BATCH-171 IceThrenodyScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-171-icethrenodyscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-171 with IceThrenodyScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-172-target.md | Source batches | Durable target for SOURCE-BATCH-172 KnightsMinneScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-172-knightsminnescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-172 with KnightsMinneScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-173-target.md | Source batches | Durable target for SOURCE-BATCH-173 MagesBalladScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-173-magesballadscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-173 with MagesBalladScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-174-target.md | Source batches | Durable target for SOURCE-BATCH-174 MagicFinaleScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-174-magicfinalescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-174 with MagicFinaleScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-175-target.md | Source batches | Durable target for SOURCE-BATCH-175 PoisonCarolScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-175-poisoncarolscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-175 with PoisonCarolScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-176-target.md | Source batches | Durable target for SOURCE-BATCH-176 PoisonThrenodyScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-176-poisonthrenodyscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-176 with PoisonThrenodyScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-177-target.md | Source batches | Durable target for SOURCE-BATCH-177 SheepfoeMamboScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-177-sheepfoemamboscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-177 with SheepfoeMamboScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-178-target.md | Source batches | Durable target for SOURCE-BATCH-178 SinewyEtudeScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-178-sinewyetudescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-178 with SinewyEtudeScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-179-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-179+ after the Bard scroll queue was exhausted; includes four zero-gate, zero-overlay magic token/book guard candidates. | Complete |
| source-batch-179-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-179 candidate discovery with the recommended DeathSkulls target and next-candidate notes. | Complete |
| source-batch-179-target.md | Source batches | Durable target for SOURCE-BATCH-179 DeathSkulls guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-179-deathskulls-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-179 with DeathSkulls guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-180-target.md | Source batches | Durable target for SOURCE-BATCH-180 HolySymbols guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-180-holysymbols-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-180 with HolySymbols guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-181-target.md | Source batches | Durable target for SOURCE-BATCH-181 DeathKnightSpellbook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-181-deathknightspellbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-181 with DeathKnightSpellbook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-182-target.md | Source batches | Durable target for SOURCE-BATCH-182 HolyManSpellbook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-182-holymanspellbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-182 with HolyManSpellbook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-183-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-183+ after the Death/Holy token/book queue was exhausted; includes four zero-gate, zero-overlay Jedi/Syth datacron and spellbook guard candidates. | Complete |
| source-batch-183-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-183 candidate discovery with the recommended SythDatacrons target and next-candidate notes. | Complete |
| source-batch-183-target.md | Source batches | Durable target for SOURCE-BATCH-183 SythDatacrons guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-183-sythdatacrons-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-183 with SythDatacrons guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-184-target.md | Source batches | Durable target for SOURCE-BATCH-184 JediDatacrons guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-184-jedidatacrons-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-184 with JediDatacrons guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-185-target.md | Source batches | Durable target for SOURCE-BATCH-185 SythSpellbook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-185-sythspellbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-185 with SythSpellbook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-186-target.md | Source batches | Durable target for SOURCE-BATCH-186 JediSpellbook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-186-jedispellbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-186 with JediSpellbook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-187-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-187+ after the Jedi/Syth spellbook queue was exhausted; includes one zero-gate, zero-overlay Research spellbook guard candidate. | Complete |
| source-batch-187-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-187 candidate discovery with the recommended AncientSpellbook target and exclusion notes. | Complete |
| source-batch-187-target.md | Source batches | Durable target for SOURCE-BATCH-187 AncientSpellbook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-187-ancientspellbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-187 with AncientSpellbook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-188-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-188+ after the Research spellbook queue was exhausted; includes one zero-gate, zero-overlay Halloween gift guard candidate. | Complete |
| source-batch-188-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-188 candidate discovery with the recommended CarvedPumpkins target and exclusion notes. | Complete |
| source-batch-188-target.md | Source batches | Durable target for SOURCE-BATCH-188 CarvedPumpkins guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-188-carvedpumpkins-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-188 with CarvedPumpkins guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-189-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-189+ after the Halloween gift-light queue was exhausted; includes three zero-gate, zero-overlay Halloween package unwrap guard candidates. | Complete |
| source-batch-189-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-189 candidate discovery with the recommended WrappedCandy target and next-candidate notes. | Complete |
| source-batch-189-target.md | Source batches | Durable target for SOURCE-BATCH-189 WrappedCandy guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-189-wrappedcandy-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-189 with WrappedCandy guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-190-target.md | Source batches | Durable target for SOURCE-BATCH-190 HalloweenPack guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-190-halloweenpack-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-190 with HalloweenPack guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-191-target.md | Source batches | Durable target for SOURCE-BATCH-191 PackedCostume guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-191-packedcostume-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-191 with PackedCostume guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-192-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-192+ after the Halloween package queue was exhausted; includes one zero-gate, zero-overlay CrystalToken guard candidate. | Complete |
| source-batch-192-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-192 candidate discovery with the recommended CrystalToken target. | Complete |
| source-batch-192-target.md | Source batches | Durable target for SOURCE-BATCH-192 CrystalToken guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-192-crystaltoken-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-192 with CrystalToken guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-193-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-193+ after the CrystalToken queue was exhausted; includes one zero-gate, zero-overlay ShadowToken guard candidate. | Complete |
| source-batch-193-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-193 candidate discovery with the recommended ShadowToken target. | Complete |
| source-batch-193-target.md | Source batches | Durable target for SOURCE-BATCH-193 ShadowToken guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-193-shadowtoken-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-193 with ShadowToken guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-194-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-194+ after the ShadowToken queue was exhausted; includes one zero-gate, zero-overlay NinthAnniversaryCoin guard candidate. | Complete |
| source-batch-194-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-194 candidate discovery with the recommended NinthAnniversaryCoin target. | Complete |
| source-batch-194-target.md | Source batches | Durable target for SOURCE-BATCH-194 NinthAnniversaryCoin guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-194-ninthanniversarycoin-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-194 with NinthAnniversaryCoin guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-195-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-195+ after the NinthAnniversaryCoin queue was exhausted; includes three zero-gate, zero-overlay addon-component guard candidates. | Complete |
| source-batch-195-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-195 candidate discovery with the recommended ECrystalAltar target and next-candidate notes. | Complete |
| source-batch-195-target.md | Source batches | Durable target for SOURCE-BATCH-195 ECrystalAltar guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-195-ecrystalaltar-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-195 with ECrystalAltar guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-196-target.md | Source batches | Durable target for SOURCE-BATCH-196 ECrystalBrazier guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-196-ecrystalbrazier-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-196 with ECrystalBrazier guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-197-target.md | Source batches | Durable target for SOURCE-BATCH-197 EGlobeOfSosaria guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-197-eglobeofsosaria-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-197 with EGlobeOfSosaria guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-198-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-198+ after the EGlobeOfSosaria queue was exhausted; includes three zero-gate, zero-overlay shadow addon-component guard candidates. | Complete |
| source-batch-198-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-198 candidate discovery with the recommended EObsidianPillar target and next-candidate notes. | Complete |
| source-batch-198-target.md | Source batches | Durable target for SOURCE-BATCH-198 EObsidianPillar guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-198-eobsidianpillar-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-198 with EObsidianPillar guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-199-target.md | Source batches | Durable target for SOURCE-BATCH-199 EObsidianRock guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-199-eobsidianrock-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-199 with EObsidianRock guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-200-target.md | Source batches | Durable target for SOURCE-BATCH-200 EShadowFirePit guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-200-eshadowfirepit-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-200 with EShadowFirePit guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-201-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-201+ after the EShadowFirePit queue was exhausted; includes three zero-gate, zero-overlay shadow addon-component guard candidates. | Complete |
| source-batch-201-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-201 candidate discovery with the recommended EShadowFirePitCross target and next-candidate notes. | Complete |
| source-batch-201-target.md | Source batches | Durable target for SOURCE-BATCH-201 EShadowFirePitCross guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-201-eshadowfirepitcross-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-201 with EShadowFirePitCross guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-202-target.md | Source batches | Durable target for SOURCE-BATCH-202 EShadowPillar guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-202-eshadowpillar-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-202 with EShadowPillar guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-203-target.md | Source batches | Durable target for SOURCE-BATCH-203 ESpikeColumn guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-203-espikecolumn-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-203 with ESpikeColumn guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-204-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-204+ after the ESpikeColumn queue was exhausted; includes two zero-gate, zero-overlay spike-post addon-component guard candidates. | Complete |
| source-batch-204-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-204 candidate discovery with the recommended ESpikePostEast target and next-candidate notes. | Complete |
| source-batch-204-target.md | Source batches | Durable target for SOURCE-BATCH-204 ESpikePostEast guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-204-espikeposteast-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-204 with ESpikePostEast guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-205-target.md | Source batches | Durable target for SOURCE-BATCH-205 ESpikePostSouth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-205-espikepostsouth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-205 with ESpikePostSouth guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-206-candidate-discovery.csv | Source batches | Candidate discovery for SOURCE-BATCH-206+ after the spike-post queue was exhausted; includes four zero-gate, zero-overlay gift interaction guard candidates. | Complete |
| source-batch-206-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-206 candidate discovery with the recommended QuestSouvenir target and next-candidate notes. | Complete |
| source-batch-206-target.md | Source batches | Durable target for SOURCE-BATCH-206 QuestSouvenir guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-206-questsouvenir-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-206 with QuestSouvenir guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-207-target.md | Source batches | Durable target for SOURCE-BATCH-207 PileOfGlacialSnow guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-207-pileofglacialsnow-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-207 with PileOfGlacialSnow guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-208-target.md | Source batches | Durable target for SOURCE-BATCH-208 SnowPile guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-208-snowpile-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-208 with SnowPile guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-209-target.md | Source batches | Durable target for SOURCE-BATCH-209 AppleBobbingBarrel guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-209-applebobbingbarrel-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-209 with AppleBobbingBarrel guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-210-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-210 candidate discovery for zero-gate, zero-overlay magical artifact activator guard repairs. | Complete |
| source-batch-210-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-210 discovery with recommended Artifact_HammerofThor target and queued Artifact_HelmOfBrilliance target. | Complete |
| source-batch-210-target.md | Source batches | Durable target for SOURCE-BATCH-210 Artifact_HammerofThor guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-210-artifact-hammerofthor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-210 with Artifact_HammerofThor guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-211-target.md | Source batches | Durable target for SOURCE-BATCH-211 Artifact_HelmOfBrilliance guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-211-artifact-helmofbrilliance-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-211 with Artifact_HelmOfBrilliance guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-212-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-212 candidate discovery for zero-gate, zero-overlay construction light guard repairs. | Complete |
| source-batch-212-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-212 discovery with recommended BaseLight target and queued TowerLantern target. | Complete |
| source-batch-212-target.md | Source batches | Durable target for SOURCE-BATCH-212 BaseLight guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-212-baselight-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-212 with BaseLight guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-213-target.md | Source batches | Durable target for SOURCE-BATCH-213 TowerLantern guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-213-towerlantern-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-213 with TowerLantern guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-214-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-214 candidate discovery for zero-gate, zero-overlay magical item guard repairs. | Complete |
| source-batch-214-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-214 discovery with recommended GemOfSeeing target and queued GiftThrowingGloves and GiftShepherdsCrook targets. | Complete |
| source-batch-214-target.md | Source batches | Durable target for SOURCE-BATCH-214 GemOfSeeing guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-214-gemofseeing-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-214 with GemOfSeeing guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-215-target.md | Source batches | Durable target for SOURCE-BATCH-215 GiftThrowingGloves guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-215-giftthrowinggloves-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-215 with GiftThrowingGloves guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-216-target.md | Source batches | Durable target for SOURCE-BATCH-216 GiftShepherdsCrook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-216-giftshepherdscrook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-216 with GiftShepherdsCrook guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-217-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-217 candidate discovery for zero-gate, zero-overlay special item guard repairs. | Complete |
| source-batch-217-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-217 discovery with recommended RewardCake target and queued HolidayBell and ValentinesCard targets. | Complete |
| source-batch-217-target.md | Source batches | Durable target for SOURCE-BATCH-217 RewardCake guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-217-rewardcake-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-217 with RewardCake guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-218-target.md | Source batches | Durable target for SOURCE-BATCH-218 HolidayBell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-218-holidaybell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-218 with HolidayBell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-219-target.md | Source batches | Durable target for SOURCE-BATCH-219 ValentinesCard guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-219-valentinescard-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-219 with ValentinesCard guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-220-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-220 candidate discovery for zero-gate, zero-overlay unknown-item identification guard repairs. | Complete |
| source-batch-220-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-220 discovery with recommended UnidentifiedArtifact target and queued UnidentifiedItem and UnknownWand targets. | Complete |
| source-batch-220-target.md | Source batches | Durable target for SOURCE-BATCH-220 UnidentifiedArtifact guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-220-unidentifiedartifact-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-220 with UnidentifiedArtifact guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-221-target.md | Source batches | Durable target for SOURCE-BATCH-221 UnidentifiedItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-221-unidentifieditem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-221 with UnidentifiedItem guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-222-target.md | Source batches | Durable target for SOURCE-BATCH-222 UnknownWand guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-222-unknownwand-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-222 with UnknownWand guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-223-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-223 candidate discovery for a zero-gate, zero-overlay special-item guard repair. | Complete |
| source-batch-223-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-223 discovery with recommended HugeWaterTub target. | Complete |
| source-batch-223-target.md | Source batches | Durable target for SOURCE-BATCH-223 HugeWaterTub guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-223-hugewatertub-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-223 with HugeWaterTub guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-224-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-224 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-224-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-224 discovery with recommended WoodWell target. | Complete |
| source-batch-224-target.md | Source batches | Durable target for SOURCE-BATCH-224 WoodWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-224-woodwell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-224 with WoodWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-225-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-225 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-225-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-225 discovery with recommended StoneWell target. | Complete |
| source-batch-225-target.md | Source batches | Durable target for SOURCE-BATCH-225 StoneWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-225-stonewell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-225 with StoneWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-226-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-226 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-226-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-226 discovery with recommended RedWell target. | Complete |
| source-batch-226-target.md | Source batches | Durable target for SOURCE-BATCH-226 RedWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-226-redwell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-226 with RedWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-227-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-227 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-227-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-227 discovery with recommended MarbleWell target. | Complete |
| source-batch-227-target.md | Source batches | Durable target for SOURCE-BATCH-227 MarbleWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-227-marblewell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-227 with MarbleWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-228-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-228 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-228-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-228 discovery with recommended BrownWell target. | Complete |
| source-batch-228-target.md | Source batches | Durable target for SOURCE-BATCH-228 BrownWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-228-brownwell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-228 with BrownWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-229-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-229 candidate discovery for a zero-gate, zero-overlay construction well component guard repair. | Complete |
| source-batch-229-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-229 discovery with recommended BlackWell target. | Complete |
| source-batch-229-target.md | Source batches | Durable target for SOURCE-BATCH-229 BlackWell guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-229-blackwell-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-229 with BlackWell guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-230-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-230 candidate discovery for a zero-gate, zero-overlay Crystals bank conversion guard repair. | Complete |
| source-batch-230-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-230 discovery with recommended Crystals target. | Complete |
| source-batch-230-target.md | Source batches | Durable target for SOURCE-BATCH-230 Crystals bank conversion guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-230-crystals-bank-conversion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-230 with Crystals guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-231-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-231 candidate discovery for a zero-gate, zero-overlay UnknownReagent identification guard repair. | Complete |
| source-batch-231-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-231 discovery with recommended UnknownReagent target. | Complete |
| source-batch-231-target.md | Source batches | Durable target for SOURCE-BATCH-231 UnknownReagent identification guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-231-unknownreagent-identification-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-231 with UnknownReagent guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-232-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-232 candidate discovery for a zero-gate, zero-overlay UnknownLiquid identification guard repair. | Complete |
| source-batch-232-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-232 discovery with recommended UnknownLiquid target. | Complete |
| source-batch-232-target.md | Source batches | Durable target for SOURCE-BATCH-232 UnknownLiquid identification guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-232-unknownliquid-identification-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-232 with UnknownLiquid guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-233-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-233 candidate discovery for a zero-gate, zero-overlay UnknownKeg identification guard repair. | Complete |
| source-batch-233-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-233 discovery with recommended UnknownKeg target. | Complete |
| source-batch-233-target.md | Source batches | Durable target for SOURCE-BATCH-233 UnknownKeg identification guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-233-unknownkeg-identification-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-233 with UnknownKeg guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-234-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-234 candidate discovery for a zero-gate, zero-overlay UnknownScroll identification guard repair. | Complete |
| source-batch-234-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-234 discovery with recommended UnknownScroll target. | Complete |
| source-batch-234-target.md | Source batches | Durable target for SOURCE-BATCH-234 UnknownScroll identification guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-234-unknownscroll-identification-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-234 with UnknownScroll guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-235-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-235 candidate discovery for a zero-gate, zero-overlay SwordsAndShackles book/gump guard repair. | Complete |
| source-batch-235-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-235 discovery with recommended SwordsAndShackles target. | Complete |
| source-batch-235-target.md | Source batches | Durable target for SOURCE-BATCH-235 SwordsAndShackles book/gump guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-235-swordsandshackles-book-gump-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-235 with SwordsAndShackles guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-236-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-236 candidate discovery for a zero-gate, zero-overlay PatchBoard guard repair. | Complete |
| source-batch-236-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-236 discovery with recommended PatchBoard target. | Complete |
| source-batch-236-target.md | Source batches | Durable target for SOURCE-BATCH-236 PatchBoard guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-236-patchboard-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-236 with PatchBoard guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-237-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-237 candidate discovery for a zero-gate, zero-overlay Pillows guard repair. | Complete |
| source-batch-237-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-237 discovery with recommended Pillows target. | Complete |
| source-batch-237-target.md | Source batches | Durable target for SOURCE-BATCH-237 Pillows guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-237-pillows-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-237 with Pillows guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-238-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-238 candidate discovery for a zero-gate, zero-overlay MountedPixieLime guard repair. | Complete |
| source-batch-238-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-238 discovery with recommended MountedPixieLime target. | Complete |
| source-batch-238-target.md | Source batches | Durable target for SOURCE-BATCH-238 MountedPixieLime guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-238-mountedpixielime-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-238 with MountedPixieLime guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-239-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-239 candidate discovery for a zero-gate, zero-overlay MountedPixieBlue guard repair. | Complete |
| source-batch-239-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-239 discovery with recommended MountedPixieBlue target. | Complete |
| source-batch-239-target.md | Source batches | Durable target for SOURCE-BATCH-239 MountedPixieBlue guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-239-mountedpixieblue-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-239 with MountedPixieBlue guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-240-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-240 candidate discovery for a zero-gate, zero-overlay MountedPixieGreen guard repair. | Complete |
| source-batch-240-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-240 discovery with recommended MountedPixieGreen target. | Complete |
| source-batch-240-target.md | Source batches | Durable target for SOURCE-BATCH-240 MountedPixieGreen guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-240-mountedpixiegreen-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-240 with MountedPixieGreen guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-241-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-241 candidate discovery for a zero-gate, zero-overlay MountedPixieOrange guard repair. | Complete |
| source-batch-241-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-241 discovery with recommended MountedPixieOrange target. | Complete |
| source-batch-241-target.md | Source batches | Durable target for SOURCE-BATCH-241 MountedPixieOrange guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-241-mountedpixieorange-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-241 with MountedPixieOrange guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-242-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-242 candidate discovery for a zero-gate, zero-overlay MountedPixieWhite guard repair. | Complete |
| source-batch-242-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-242 discovery with recommended MountedPixieWhite target. | Complete |
| source-batch-242-target.md | Source batches | Durable target for SOURCE-BATCH-242 MountedPixieWhite guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-242-mountedpixiewhite-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-242 with MountedPixieWhite guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-243-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-243 candidate discovery for a zero-gate, zero-overlay DisturbingPortrait guard repair. | Complete |
| source-batch-243-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-243 discovery with recommended DisturbingPortrait target. | Complete |
| source-batch-243-target.md | Source batches | Durable target for SOURCE-BATCH-243 DisturbingPortrait guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-243-disturbingportrait-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-243 with DisturbingPortrait guard changes, POST-BATCH-Y fence evidence, serializer and timer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-244-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-244 candidate discovery for a zero-gate, zero-overlay UnsettlingPortrait guard repair. | Complete |
| source-batch-244-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-244 discovery with recommended UnsettlingPortrait target. | Complete |
| source-batch-244-target.md | Source batches | Durable target for SOURCE-BATCH-244 UnsettlingPortrait guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-244-unsettlingportrait-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-244 with UnsettlingPortrait guard changes, POST-BATCH-Y fence evidence, serializer and timer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-245-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-245 candidate discovery for a zero-gate, zero-overlay AwesomeDisturbingPortrait guard repair. | Complete |
| source-batch-245-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-245 discovery with recommended AwesomeDisturbingPortrait target. | Complete |
| source-batch-245-target.md | Source batches | Durable target for SOURCE-BATCH-245 AwesomeDisturbingPortrait guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-245-awesomedisturbingportrait-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-245 with AwesomeDisturbingPortrait guard changes, POST-BATCH-Y fence evidence, serializer and timer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-246-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-246 candidate discovery for a zero-gate, zero-overlay CreepyPortrait guard repair. | Complete |
| source-batch-246-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-246 discovery with recommended CreepyPortrait target. | Complete |
| source-batch-246-target.md | Source batches | Durable target for SOURCE-BATCH-246 CreepyPortrait guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-246-creepyportrait-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-246 with CreepyPortrait guard changes, POST-BATCH-Y fence evidence, serializer plus movement/timer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-247-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-247 candidate discovery for a zero-gate, zero-overlay IndecipherableMap guard repair. | Complete |
| source-batch-247-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-247 discovery with recommended IndecipherableMap target. | Complete |
| source-batch-247-target.md | Source batches | Durable target for SOURCE-BATCH-247 IndecipherableMap guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-247-indecipherablemap-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-247 with IndecipherableMap guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-248-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-248 candidate discovery for a zero-gate, zero-overlay Dices guard repair. | Complete |
| source-batch-248-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-248 discovery with recommended Dices target. | Complete |
| source-batch-248-target.md | Source batches | Durable target for SOURCE-BATCH-248 Dices guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-248-dices-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-248 with Dices guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-249-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-249 candidate discovery for a zero-gate, zero-overlay PearlSkull guard repair. | Complete |
| source-batch-249-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-249 discovery with recommended PearlSkull target. | Complete |
| source-batch-249-target.md | Source batches | Durable target for SOURCE-BATCH-249 PearlSkull guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-249-pearlskull-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-249 with PearlSkull guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-250-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-250 candidate discovery for a zero-gate, zero-overlay AuraOfShadows guard repair. | Complete |
| source-batch-250-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-250 discovery with recommended AuraOfShadows target. | Complete |
| source-batch-250-target.md | Source batches | Durable target for SOURCE-BATCH-250 AuraOfShadows guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-250-auraofshadows-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-250 with AuraOfShadows guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-251-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-251 candidate discovery for a zero-gate, zero-overlay LevelCandle guard repair. | Complete |
| source-batch-251-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-251 discovery with recommended LevelCandle target. | Complete |
| source-batch-251-target.md | Source batches | Durable target for SOURCE-BATCH-251 LevelCandle guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-251-levelcandle-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-251 with LevelCandle guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-252-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-252 candidate discovery for a zero-gate, zero-overlay LevelLantern guard repair. | Complete |
| source-batch-252-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-252 discovery with recommended LevelLantern target. | Complete |
| source-batch-252-target.md | Source batches | Durable target for SOURCE-BATCH-252 LevelLantern guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-252-levellantern-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-252 with LevelLantern guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-253-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-253 candidate discovery for a zero-gate, zero-overlay LevelTorch guard repair. | Complete |
| source-batch-253-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-253 discovery with recommended LevelTorch target. | Complete |
| source-batch-253-target.md | Source batches | Durable target for SOURCE-BATCH-253 LevelTorch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-253-leveltorch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-253 with LevelTorch guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-254-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-254 candidate discovery for a zero-gate, zero-overlay GiftCandle guard repair. | Complete |
| source-batch-254-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-254 discovery with recommended GiftCandle target. | Complete |
| source-batch-254-target.md | Source batches | Durable target for SOURCE-BATCH-254 GiftCandle guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-254-giftcandle-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-254 with GiftCandle guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-255-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-255 candidate discovery for a zero-gate, zero-overlay GiftLantern guard repair. | Complete |
| source-batch-255-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-255 discovery with recommended GiftLantern target. | Complete |
| source-batch-255-target.md | Source batches | Durable target for SOURCE-BATCH-255 GiftLantern guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-255-giftlantern-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-255 with GiftLantern guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-256-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-256 candidate discovery for a zero-gate, zero-overlay GiftTorch guard repair. | Complete |
| source-batch-256-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-256 discovery with recommended GiftTorch target. | Complete |
| source-batch-256-target.md | Source batches | Durable target for SOURCE-BATCH-256 GiftTorch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-256-gifttorch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-256 with GiftTorch guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-257-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-257 candidate discovery for a zero-gate, zero-overlay MagicCandle guard repair. | Complete |
| source-batch-257-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-257 discovery with recommended MagicCandle target. | Complete |
| source-batch-257-target.md | Source batches | Durable target for SOURCE-BATCH-257 MagicCandle guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-257-magiccandle-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-257 with MagicCandle guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-258-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-258 candidate discovery for a zero-gate, zero-overlay MagicLantern guard repair. | Complete |
| source-batch-258-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-258 discovery with recommended MagicLantern target. | Complete |
| source-batch-258-target.md | Source batches | Durable target for SOURCE-BATCH-258 MagicLantern guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-258-magiclantern-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-258 with MagicLantern guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-259-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-259 candidate discovery for a zero-gate, zero-overlay MagicTorch guard repair. | Complete |
| source-batch-259-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-259 discovery with recommended MagicTorch target. | Complete |
| source-batch-259-target.md | Source batches | Durable target for SOURCE-BATCH-259 MagicTorch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-259-magictorch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-259 with MagicTorch guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-260-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-260 candidate discovery for a zero-gate, zero-overlay WallTorch guard repair. | Complete |
| source-batch-260-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-260 discovery with recommended WallTorch target. | Complete |
| source-batch-260-target.md | Source batches | Durable target for SOURCE-BATCH-260 WallTorch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-260-walltorch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-260 with WallTorch guard changes, POST-BATCH-Y fence evidence, resolved IntentionalLegacy serializer overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-261-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-261 candidate discovery for a zero-gate, zero-overlay FirstAidKit guard repair. | Complete |
| source-batch-261-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-261 discovery with recommended FirstAidKit target. | Complete |
| source-batch-261-target.md | Source batches | Durable target for SOURCE-BATCH-261 FirstAidKit guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-261-firstaidkit-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-261 with FirstAidKit guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
