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
| source-batch-262-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-262 candidate discovery for a zero-gate, zero-overlay VenomSack guard repair. | Complete |
| source-batch-262-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-262 discovery with recommended VenomSack target. | Complete |
| source-batch-262-target.md | Source batches | Durable target for SOURCE-BATCH-262 VenomSack guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-262-venomsack-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-262 with VenomSack guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-263-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-263 candidate discovery for a zero-gate, zero-overlay Reagent Jars guard repair. | Complete |
| source-batch-263-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-263 discovery with recommended Reagent Jars target. | Complete |
| source-batch-263-target.md | Source batches | Durable target for SOURCE-BATCH-263 Reagent Jars guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-263-reagent-jars-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-263 with Reagent Jars guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-264-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-264 candidate discovery for a zero-gate, zero-overlay DartBoard guard repair. | Complete |
| source-batch-264-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-264 discovery with recommended DartBoard target. | Complete |
| source-batch-264-target.md | Source batches | Durable target for SOURCE-BATCH-264 DartBoard guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-264-dartboard-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-264 with DartBoard guard changes, POST-BATCH-Y fence evidence, resolved IntentionalLegacy serializer overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-265-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-265 candidate discovery for a zero-gate, zero-overlay MagicFish guard repair. | Complete |
| source-batch-265-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-265 discovery with recommended MagicFish target. | Complete |
| source-batch-265-target.md | Source batches | Durable target for SOURCE-BATCH-265 MagicFish guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-265-magicfish-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-265 with MagicFish guard changes, POST-BATCH-Y fence evidence, serializer preservation evidence, verification, and artifact restoration notes. | Complete |
| source-batch-266-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-266 candidate discovery for a zero-gate, zero-overlay DDRelicMoney guard repair. | Complete |
| source-batch-266-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-266 discovery with recommended DDRelicMoney target. | Complete |
| source-batch-266-target.md | Source batches | Durable target for SOURCE-BATCH-266 DDRelicMoney guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-266-ddrelicmoney-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-266 with DDRelicMoney guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-267-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-267 candidate discovery for a zero-gate, zero-overlay MagicTalisman guard repair. | Complete |
| source-batch-267-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-267 discovery with recommended MagicTalisman target. | Complete |
| source-batch-267-target.md | Source batches | Durable target for SOURCE-BATCH-267 MagicTalisman guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-267-magictalisman-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-267 with MagicTalisman guard changes, POST-BATCH-Y fence evidence, resolved SafeNoChange overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-268-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-268 candidate discovery for a zero-gate, zero-overlay SpaceJunk guard repair. | Complete |
| source-batch-268-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-268 discovery with recommended SpaceJunk target. | Complete |
| source-batch-268-target.md | Source batches | Durable target for SOURCE-BATCH-268 SpaceJunk guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-268-spacejunk-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-268 with SpaceJunk guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-269-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-269 candidate discovery for a zero-gate, zero-overlay RomulanAle guard repair. | Complete |
| source-batch-269-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-269 discovery with recommended RomulanAle target. | Complete |
| source-batch-269-target.md | Source batches | Durable target for SOURCE-BATCH-269 RomulanAle guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-269-romulanale-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-269 with RomulanAle guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-270-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-270 candidate discovery for a zero-gate, zero-overlay PlasmaTorch guard repair. | Complete |
| source-batch-270-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-270 discovery with recommended PlasmaTorch target. | Complete |
| source-batch-270-target.md | Source batches | Durable target for SOURCE-BATCH-270 PlasmaTorch guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-270-plasmatorch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-270 with PlasmaTorch guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-271-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-271 candidate discovery for a zero-gate, zero-overlay SpaceDyes guard repair. | Complete |
| source-batch-271-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-271 discovery with recommended SpaceDyes target. | Complete |
| source-batch-271-target.md | Source batches | Durable target for SOURCE-BATCH-271 SpaceDyes guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-271-spacedyes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-271 with SpaceDyes guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-272-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-272 candidate discovery for a zero-gate, zero-overlay Chainsaw guard repair. | Complete |
| source-batch-272-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-272 discovery with recommended Chainsaw target. | Complete |
| source-batch-272-target.md | Source batches | Durable target for SOURCE-BATCH-272 Chainsaw guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-272-chainsaw-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-272 with Chainsaw guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-273-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-273 candidate discovery for a zero-gate, zero-overlay PortableSmelter guard repair. | Complete |
| source-batch-273-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-273 discovery with recommended PortableSmelter target. | Complete |
| source-batch-273-target.md | Source batches | Durable target for SOURCE-BATCH-273 PortableSmelter guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-273-portablesmelter-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-273 with PortableSmelter guard changes, POST-BATCH-Y fence evidence, resolved Documented overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-274-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-274 candidate discovery for a zero-gate, zero-overlay ComputerDatabase guard repair. | Complete |
| source-batch-274-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-274 discovery with recommended ComputerDatabase target. | Complete |
| source-batch-274-target.md | Source batches | Durable target for SOURCE-BATCH-274 ComputerDatabase guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-274-computerdatabase-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-274 with ComputerDatabase guard changes, POST-BATCH-Y fence evidence, resolved Documented/ReviewedNoChange/SafeNoChange overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-275-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-275 candidate discovery for a zero-gate, zero-overlay MaterialLiquifier guard repair. | Complete |
| source-batch-275-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-275 discovery with recommended MaterialLiquifier target. | Complete |
| source-batch-275-target.md | Source batches | Durable target for SOURCE-BATCH-275 MaterialLiquifier guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-275-materialliquifier-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-275 with MaterialLiquifier guard changes, POST-BATCH-Y fence evidence, resolved Documented/FalsePositive/ReviewedNoChange overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-276-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-276 candidate discovery for a zero-gate, zero-overlay BlankMap guard repair. | Complete |
| source-batch-276-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-276 discovery with recommended BlankMap target. | Complete |
| source-batch-276-target.md | Source batches | Durable target for SOURCE-BATCH-276 BlankMap guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-276-blankmap-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-276 with BlankMap guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-277-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-277 candidate discovery for a zero-gate, zero-overlay BankChest guard repair. | Complete |
| source-batch-277-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-277 discovery with recommended BankChest target. | Complete |
| source-batch-277-target.md | Source batches | Durable target for SOURCE-BATCH-277 BankChest guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-277-bankchest-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-277 with BankChest guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-278-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-278 candidate discovery for a zero-gate, zero-overlay DDRelicBook guard repair. | Complete |
| source-batch-278-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-278 discovery with recommended DDRelicBook target. | Complete |
| source-batch-278-target.md | Source batches | Durable target for SOURCE-BATCH-278 DDRelicBook guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-278-ddrelicbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-278 with DDRelicBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-279-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-279 candidate discovery for a zero-gate, zero-overlay DDRelicAlchemy guard repair. | Complete |
| source-batch-279-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-279 discovery with recommended DDRelicAlchemy target. | Complete |
| source-batch-279-target.md | Source batches | Durable target for SOURCE-BATCH-279 DDRelicAlchemy guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-279-ddrelicalchemy-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-279 with DDRelicAlchemy guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-280-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-280 candidate discovery for a zero-gate, zero-overlay SavageTalisman guard repair. | Complete |
| source-batch-280-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-280 discovery with recommended SavageTalisman target. | Complete |
| source-batch-280-target.md | Source batches | Durable target for SOURCE-BATCH-280 SavageTalisman guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-280-savagetalisman-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-280 with SavageTalisman guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-281-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-281 candidate discovery for a zero-gate, zero-overlay FoodChest guard repair. | Complete |
| source-batch-281-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-281 discovery with recommended FoodChest target. | Complete |
| source-batch-281-target.md | Source batches | Durable target for SOURCE-BATCH-281 FoodChest guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-281-foodchest-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-281 with FoodChest guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-282-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-282 candidate discovery for a zero-gate, zero-overlay HiveTool guard repair. | Complete |
| source-batch-282-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-282 discovery with recommended HiveTool target. | Complete |
| source-batch-282-target.md | Source batches | Durable target for SOURCE-BATCH-282 HiveTool guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-282-hivetool-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-282 with HiveTool guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-283-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-283 candidate discovery for a zero-gate, zero-overlay SpellScroll guard repair. | Complete |
| source-batch-283-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-283 discovery with recommended SpellScroll target. | Complete |
| source-batch-283-target.md | Source batches | Durable target for SOURCE-BATCH-283 SpellScroll guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-283-spellscroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-283 with SpellScroll guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-284-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-284 candidate discovery for a zero-gate, zero-unresolved-overlay Artifact_AcidProofRobe guard repair with resolved FalsePositive overlay evidence. | Complete |
| source-batch-284-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-284 discovery with recommended Artifact_AcidProofRobe target. | Complete |
| source-batch-284-target.md | Source batches | Durable target for SOURCE-BATCH-284 Artifact_AcidProofRobe guard repair, including fence result, resolved overlay evidence, and unchanged behavior. | Complete |
| source-batch-284-artifact-acidproofrobe-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-284 with Artifact_AcidProofRobe guard changes, POST-BATCH-Y fence evidence, resolved FalsePositive overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-285-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-285 candidate discovery for a zero-gate, zero-unresolved-overlay obsolete AcidProofRobe guard repair with resolved FalsePositive/IntentionalLegacy overlay evidence. | Complete |
| source-batch-285-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-285 discovery with recommended obsolete AcidProofRobe target. | Complete |
| source-batch-285-target.md | Source batches | Durable target for SOURCE-BATCH-285 obsolete AcidProofRobe guard repair, including fence result, resolved overlay evidence, and unchanged behavior. | Complete |
| source-batch-285-obsolete-acidproofrobe-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-285 with obsolete AcidProofRobe guard changes, POST-BATCH-Y fence evidence, resolved FalsePositive/IntentionalLegacy overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-286-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-286 candidate discovery for a zero-gate, zero-overlay FestiveCactus guard repair. | Complete |
| source-batch-286-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-286 discovery with recommended FestiveCactus target. | Complete |
| source-batch-286-target.md | Source batches | Durable target for SOURCE-BATCH-286 FestiveCactus guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-286-festivecactus-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-286 with FestiveCactus guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-287-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-287 candidate discovery for a zero-gate, zero-overlay DecorativeTopiary guard repair. | Complete |
| source-batch-287-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-287 discovery with recommended DecorativeTopiary target. | Complete |
| source-batch-287-target.md | Source batches | Durable target for SOURCE-BATCH-287 DecorativeTopiary guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-287-decorativetopiary-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-287 with DecorativeTopiary guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-288-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-288 candidate discovery for a zero-gate, zero-overlay SnowyTree guard repair. | Complete |
| source-batch-288-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-288 discovery with recommended SnowyTree target. | Complete |
| source-batch-288-target.md | Source batches | Durable target for SOURCE-BATCH-288 SnowyTree guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-288-snowytree-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-288 with SnowyTree guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-289-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-289 candidate discovery for a zero-gate, zero-overlay Candelabra guard repair. | Complete |
| source-batch-289-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-289 discovery with recommended Candelabra target. | Complete |
| source-batch-289-target.md | Source batches | Durable target for SOURCE-BATCH-289 Candelabra guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-289-candelabra-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-289 with Candelabra guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-290-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-290 candidate discovery for a zero-gate, zero-overlay BoltOfCloth guard repair. | Complete |
| source-batch-290-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-290 discovery with recommended BoltOfCloth target. | Complete |
| source-batch-290-target.md | Source batches | Durable target for SOURCE-BATCH-290 BoltOfCloth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-290-boltofcloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-290 with BoltOfCloth guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-291-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-291 candidate discovery for a zero-gate, zero-overlay UncutCloth guard repair. | Complete |
| source-batch-291-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-291 discovery with recommended UncutCloth target. | Complete |
| source-batch-291-target.md | Source batches | Durable target for SOURCE-BATCH-291 UncutCloth guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-291-uncutcloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-291 with UncutCloth guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-292-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-292 candidate discovery for a zero-gate, zero-overlay ShipwreckedItem guard repair. | Complete |
| source-batch-292-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-292 discovery with recommended ShipwreckedItem target. | Complete |
| source-batch-292-target.md | Source batches | Durable target for SOURCE-BATCH-292 ShipwreckedItem guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-292-shipwreckeditem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-292 with ShipwreckedItem guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-293-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-293 candidate discovery for a zero-gate, zero-overlay BaseWaterContainer guard repair. | Complete |
| source-batch-293-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-293 discovery with recommended BaseWaterContainer target. | Complete |
| source-batch-293-target.md | Source batches | Durable target for SOURCE-BATCH-293 BaseWaterContainer guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-293-basewatercontainer-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-293 with BaseWaterContainer guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-294-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-294 candidate discovery for a zero-gate, zero-overlay PandorasBox guard repair. | Complete |
| source-batch-294-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-294 discovery with recommended PandorasBox target. | Complete |
| source-batch-294-target.md | Source batches | Durable target for SOURCE-BATCH-294 PandorasBox guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-294-pandorasbox-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-294 with PandorasBox guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-295-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-295 candidate discovery for a zero-gate, zero-overlay EvilSkull guard repair. | Complete |
| source-batch-295-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-295 discovery with recommended EvilSkull target. | Complete |
| source-batch-295-target.md | Source batches | Durable target for SOURCE-BATCH-295 EvilSkull guard repair, including fence result, active overlay result, and unchanged behavior. | Complete |
| source-batch-295-evilskull-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-295 with EvilSkull guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-296-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-296 candidate discovery for a zero-gate, zero-unresolved-overlay DragonBardingDeed guard repair with resolved IntentionalLegacy overlay evidence. | Complete |
| source-batch-296-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-296 discovery with recommended DragonBardingDeed target. | Complete |
| source-batch-296-target.md | Source batches | Durable target for SOURCE-BATCH-296 DragonBardingDeed guard repair, including fence result, resolved overlay evidence, and unchanged behavior. | Complete |
| source-batch-296-dragonbardingdeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-296 with DragonBardingDeed guard changes, POST-BATCH-Y fence evidence, resolved IntentionalLegacy overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-297-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-297 candidate discovery for a zero-gate, zero-unresolved-overlay Seed guard repair with resolved docs-trace overlay evidence. | Complete |
| source-batch-297-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-297 discovery with recommended Seed target. | Complete |
| source-batch-297-target.md | Source batches | Durable target for SOURCE-BATCH-297 Seed guard repair, including fence result, resolved overlay evidence, and unchanged behavior. | Complete |
| source-batch-297-seed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-297 with Seed guard changes, POST-BATCH-Y fence evidence, resolved docs-trace overlay evidence, verification, and artifact restoration notes. | Complete |
| source-batch-298-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-298 candidate discovery for a zero-gate, zero-overlay RedLeaves guard repair, with OrangePetals and GreenThorns deferred for separate focused review. | Complete |
| source-batch-298-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-298 discovery with recommended RedLeaves target and deferred gardening candidates. | Complete |
| source-batch-298-target.md | Source batches | Durable target for SOURCE-BATCH-298 RedLeaves guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-298-redleaves-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-298 with RedLeaves guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-299-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-299 candidate discovery for a zero-gate, zero-overlay OrangePetals guard repair, with GreenThorns deferred for separate focused review. | Complete |
| source-batch-299-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-299 discovery with recommended OrangePetals target and deferred GreenThorns candidate. | Complete |
| source-batch-299-target.md | Source batches | Durable target for SOURCE-BATCH-299 OrangePetals guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-299-orangepetals-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-299 with OrangePetals guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-300-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-300 candidate discovery for a zero-gate, zero-overlay LargeBODTarget guard repair, with SmallBODTarget deferred as the next sibling candidate. | Complete |
| source-batch-300-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-300 discovery with recommended LargeBODTarget target and deferred SmallBODTarget candidate. | Complete |
| source-batch-300-target.md | Source batches | Durable target for SOURCE-BATCH-300 LargeBODTarget guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-300-largebodtarget-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-300 with LargeBODTarget guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-301-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-301 candidate discovery for a zero-gate, zero-overlay SmallBODTarget guard repair. | Complete |
| source-batch-301-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-301 discovery with recommended SmallBODTarget target. | Complete |
| source-batch-301-target.md | Source batches | Durable target for SOURCE-BATCH-301 SmallBODTarget guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-301-smallbodtarget-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-301 with SmallBODTarget guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-302-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-302 candidate discovery for a zero-gate, zero-overlay DuctTape guard repair, with RepairPotion and DurabilityPotion deferred as sibling candidates. | Complete |
| source-batch-302-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-302 discovery with recommended DuctTape target and deferred repair/durability potion candidates. | Complete |
| source-batch-302-target.md | Source batches | Durable target for SOURCE-BATCH-302 DuctTape guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-302-ducttape-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-302 with DuctTape guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-303-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-303 candidate discovery for a zero-gate, zero-overlay RepairPotion guard repair, with DurabilityPotion deferred as a sibling candidate. | Complete |
| source-batch-303-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-303 discovery with recommended RepairPotion target and deferred DurabilityPotion candidate. | Complete |
| source-batch-303-target.md | Source batches | Durable target for SOURCE-BATCH-303 RepairPotion guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-303-repairpotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-303 with RepairPotion guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-304-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-304 candidate discovery for a zero-gate, zero-overlay DurabilityPotion guard repair. | Complete |
| source-batch-304-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-304 discovery with recommended DurabilityPotion target. | Complete |
| source-batch-304-target.md | Source batches | Durable target for SOURCE-BATCH-304 DurabilityPotion guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-304-durabilitypotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-304 with DurabilityPotion guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-305-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-305 candidate discovery for a zero-gate, zero-overlay PowderOfTemperament guard repair, with JarsOfWax deferred as a sibling candidate. | Complete |
| source-batch-305-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-305 discovery with recommended PowderOfTemperament target and deferred JarsOfWax candidate. | Complete |
| source-batch-305-target.md | Source batches | Durable target for SOURCE-BATCH-305 PowderOfTemperament guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-305-powderoftemperament-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-305 with PowderOfTemperament guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-306-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-306 candidate discovery for a zero-gate, zero-overlay JarsOfWax guard repair. | Complete |
| source-batch-306-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-306 discovery with recommended JarsOfWax target. | Complete |
| source-batch-306-target.md | Source batches | Durable target for SOURCE-BATCH-306 JarsOfWax guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-306-jarsofwax-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-306 with JarsOfWax guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-307-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-307 candidate discovery for a zero-gate, zero-overlay WaxSculptors guard repair, with WaxPaintings deferred as a sibling candidate. | Complete |
| source-batch-307-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-307 discovery with recommended WaxSculptors target and deferred WaxPaintings candidate. | Complete |
| source-batch-307-target.md | Source batches | Durable target for SOURCE-BATCH-307 WaxSculptors guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-307-waxsculptors-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-307 with WaxSculptors guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-308-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-308 candidate discovery for a zero-gate, zero-overlay WaxPaintings guard repair, with LargeWaxPot deferred as a sibling candidate. | Complete |
| source-batch-308-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-308 discovery with recommended WaxPaintings target and deferred LargeWaxPot candidate. | Complete |
| source-batch-308-target.md | Source batches | Durable target for SOURCE-BATCH-308 WaxPaintings guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-308-waxpaintings-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-308 with WaxPaintings guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-309-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-309 candidate discovery for a zero-gate, zero-overlay LargeWaxPot guard repair. | Complete |
| source-batch-309-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-309 discovery with recommended LargeWaxPot target. | Complete |
| source-batch-309-target.md | Source batches | Durable target for SOURCE-BATCH-309 LargeWaxPot guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-309-largewaxpot-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-309 with LargeWaxPot guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-310-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-310 candidate discovery for a zero-gate, zero-overlay WizardStaff guard repair, with LevelStave and GiftStave deferred as sibling candidates. | Complete |
| source-batch-310-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-310 discovery with recommended WizardStaff target and deferred staff-family candidates. | Complete |
| source-batch-310-target.md | Source batches | Durable target for SOURCE-BATCH-310 WizardStaff guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-310-wizardstaff-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-310 with WizardStaff guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-311-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-311 candidate discovery for a zero-gate, zero-overlay LevelStave guard repair, with GiftStave deferred as a sibling candidate. | Complete |
| source-batch-311-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-311 discovery with recommended LevelStave target and deferred GiftStave candidate. | Complete |
| source-batch-311-target.md | Source batches | Durable target for SOURCE-BATCH-311 LevelStave guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-311-levelstave-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-311 with LevelStave guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-312-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-312 candidate discovery for a zero-gate, zero-overlay GiftStave guard repair. | Complete |
| source-batch-312-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-312 discovery with recommended GiftStave target. | Complete |
| source-batch-312-target.md | Source batches | Durable target for SOURCE-BATCH-312 GiftStave guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-312-giftstave-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-312 with GiftStave guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-313-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-313 candidate discovery for a zero-gate, zero-overlay WeaponEngravingTool guard repair. | Complete |
| source-batch-313-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-313 discovery with recommended WeaponEngravingTool target. | Complete |
| source-batch-313-target.md | Source batches | Durable target for SOURCE-BATCH-313 WeaponEngravingTool guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-313-weaponengravingtool-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-313 with WeaponEngravingTool guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-314-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-314 candidate discovery for a zero-gate, zero-overlay JukaBow guard repair. | Complete |
| source-batch-314-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-314 discovery with recommended JukaBow target. | Complete |
| source-batch-314-target.md | Source batches | Durable target for SOURCE-BATCH-314 JukaBow guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-314-jukabow-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-314 with JukaBow guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-315-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-315 candidate discovery for a zero-gate, zero-overlay Waterskin guard repair. | Complete |
| source-batch-315-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-315 discovery with recommended Waterskin target. | Complete |
| source-batch-315-target.md | Source batches | Durable target for SOURCE-BATCH-315 Waterskin guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-315-waterskin-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-315 with Waterskin guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-316-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-316 candidate discovery for a zero-gate, zero-overlay DDRelicCoins guard repair. | Complete |
| source-batch-316-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-316 discovery with recommended DDRelicCoins target. | Complete |
| source-batch-316-target.md | Source batches | Durable target for SOURCE-BATCH-316 DDRelicCoins guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-316-ddreliccoins-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-316 with DDRelicCoins guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-317-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-317 candidate discovery for a zero-gate, zero-overlay DDRelicWeapon guard repair. | Complete |
| source-batch-317-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-317 discovery with recommended DDRelicWeapon target. | Complete |
| source-batch-317-target.md | Source batches | Durable target for SOURCE-BATCH-317 DDRelicWeapon guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-317-ddrelicweapon-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-317 with DDRelicWeapon guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-318-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-318 candidate discovery for a zero-gate, zero-overlay DDRelicArmor guard repair. | Complete |
| source-batch-318-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-318 discovery with recommended DDRelicArmor target. | Complete |
| source-batch-318-target.md | Source batches | Durable target for SOURCE-BATCH-318 DDRelicArmor guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-318-ddrelicarmor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-318 with DDRelicArmor guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-319-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-319 candidate discovery for a zero-gate, zero-overlay DDRelicBanner guard repair. | Complete |
| source-batch-319-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-319 discovery with recommended DDRelicBanner target. | Complete |
| source-batch-319-target.md | Source batches | Durable target for SOURCE-BATCH-319 DDRelicBanner guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-319-ddrelicbanner-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-319 with DDRelicBanner guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-320-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-320 candidate discovery for a zero-gate, zero-overlay DDRelicInstrument guard repair. | Complete |
| source-batch-320-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-320 discovery with recommended DDRelicInstrument target. | Complete |
| source-batch-320-target.md | Source batches | Durable target for SOURCE-BATCH-320 DDRelicInstrument guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-320-ddrelicinstrument-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-320 with DDRelicInstrument guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-321-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-321 candidate discovery for a zero-gate, zero-overlay DDRelicGrave guard repair. | Complete |
| source-batch-321-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-321 discovery with recommended DDRelicGrave target. | Complete |
| source-batch-321-target.md | Source batches | Durable target for SOURCE-BATCH-321 DDRelicGrave guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-321-ddrelicgrave-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-321 with DDRelicGrave guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-322-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-322 candidate discovery for a zero-gate, zero-overlay DDRelicPainting guard repair. | Complete |
| source-batch-322-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-322 discovery with recommended DDRelicPainting target. | Complete |
| source-batch-322-target.md | Source batches | Durable target for SOURCE-BATCH-322 DDRelicPainting guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-322-ddrelicpainting-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-322 with DDRelicPainting guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-323-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-323 candidate discovery for a zero-gate, zero-overlay DDRelicStatue guard repair. | Complete |
| source-batch-323-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-323 discovery with recommended DDRelicStatue target. | Complete |
| source-batch-323-target.md | Source batches | Durable target for SOURCE-BATCH-323 DDRelicStatue guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-323-ddrelicstatue-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-323 with DDRelicStatue guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-324-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-324 candidate discovery for a zero-gate, zero-overlay Bola guard repair. | Complete |
| source-batch-324-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-324 discovery with recommended Bola target. | Complete |
| source-batch-324-target.md | Source batches | Durable target for SOURCE-BATCH-324 Bola guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-324-bola-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-324 with Bola guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-325-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-325 candidate discovery for a zero-gate, zero-overlay BaseLiquid guard repair. | Complete |
| source-batch-325-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-325 discovery with recommended BaseLiquid target. | Complete |
| source-batch-325-target.md | Source batches | Durable target for SOURCE-BATCH-325 BaseLiquid guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-325-baseliquid-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-325 with BaseLiquid guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-326-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-326 candidate discovery for a zero-gate, zero-overlay BaseMixture guard repair. | Complete |
| source-batch-326-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-326 discovery with recommended BaseMixture target. | Complete |
| source-batch-326-target.md | Source batches | Durable target for SOURCE-BATCH-326 BaseMixture guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-326-basemixture-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-326 with BaseMixture guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-327-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-327 candidate discovery for a zero-gate, zero-overlay BasePoisonPotion guard repair. | Complete |
| source-batch-327-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-327 discovery with recommended BasePoisonPotion target. | Complete |
| source-batch-327-target.md | Source batches | Durable target for SOURCE-BATCH-327 BasePoisonPotion guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-327-basepoisonpotion-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-327 with BasePoisonPotion guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-328-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-328 candidate discovery for a zero-gate, zero-overlay HorseArmor guard repair. | Complete |
| source-batch-328-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-328 discovery with recommended HorseArmor target. | Complete |
| source-batch-328-target.md | Source batches | Durable target for SOURCE-BATCH-328 HorseArmor guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-328-horsearmor-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-328 with HorseArmor guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-329-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-329 candidate discovery for a zero-gate, zero-overlay GrapplingHook guard repair. | Complete |
| source-batch-329-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-329 discovery with recommended GrapplingHook target. | Complete |
| source-batch-329-target.md | Source batches | Durable target for SOURCE-BATCH-329 GrapplingHook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-329-grapplinghook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-329 with GrapplingHook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-330-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-330 candidate discovery for a zero-gate, zero-overlay BoatStain guard repair. | Complete |
| source-batch-330-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-330 discovery with recommended BoatStain target. | Complete |
| source-batch-330-target.md | Source batches | Durable target for SOURCE-BATCH-330 BoatStain guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-330-boatstain-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-330 with BoatStain guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-331-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-331 candidate discovery for a zero-gate, zero-overlay RobotBatteries guard repair. | Complete |
| source-batch-331-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-331 discovery with recommended RobotBatteries target. | Complete |
| source-batch-331-target.md | Source batches | Durable target for SOURCE-BATCH-331 RobotBatteries guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-331-robotbatteries-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-331 with RobotBatteries guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-332-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-332 candidate discovery for a zero-gate, zero-overlay EmbalmingFluid guard repair. | Complete |
| source-batch-332-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-332 discovery with recommended EmbalmingFluid target. | Complete |
| source-batch-332-target.md | Source batches | Durable target for SOURCE-BATCH-332 EmbalmingFluid guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-332-embalmingfluid-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-332 with EmbalmingFluid guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-333-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-333 candidate discovery for a zero-gate, zero-overlay SlaversNet guard repair. | Complete |
| source-batch-333-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-333 discovery with recommended SlaversNet target. | Complete |
| source-batch-333-target.md | Source batches | Durable target for SOURCE-BATCH-333 SlaversNet guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-333-slaversnet-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-333 with SlaversNet guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-334-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-334 candidate discovery for a zero-gate, zero-overlay RobotSheetMetal guard repair. | Complete |
| source-batch-334-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-334 discovery with recommended RobotSheetMetal target. | Complete |
| source-batch-334-target.md | Source batches | Durable target for SOURCE-BATCH-334 RobotSheetMetal guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-334-robotsheetmetal-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-334 with RobotSheetMetal guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-335-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-335 candidate discovery for a zero-gate, zero-overlay RustyJunk guard repair. | Complete |
| source-batch-335-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-335 discovery with recommended RustyJunk target. | Complete |
| source-batch-335-target.md | Source batches | Durable target for SOURCE-BATCH-335 RustyJunk guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-335-rustyjunk-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-335 with RustyJunk guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-336-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-336 candidate discovery for a zero-gate, zero-overlay HolidayFoods guard repair. | Complete |
| source-batch-336-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-336 discovery with recommended HolidayFoods target. | Complete |
| source-batch-336-target.md | Source batches | Durable target for SOURCE-BATCH-336 HolidayFoods guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-336-holidayfoods-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-336 with HolidayFoods guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-337-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-337 candidate discovery for a zero-gate, zero-overlay ShepherdsCrook guard repair. | Complete |
| source-batch-337-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-337 discovery with recommended ShepherdsCrook target. | Complete |
| source-batch-337-target.md | Source batches | Durable target for SOURCE-BATCH-337 ShepherdsCrook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-337-shepherdscrook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-337 with ShepherdsCrook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-338-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-338 candidate discovery for a zero-gate, zero-overlay LevelShepherdsCrook guard repair. | Complete |
| source-batch-338-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-338 discovery with recommended LevelShepherdsCrook target. | Complete |
| source-batch-338-target.md | Source batches | Durable target for SOURCE-BATCH-338 LevelShepherdsCrook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-338-levelshepherdscrook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-338 with LevelShepherdsCrook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-339-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-339 candidate discovery for a zero-gate, zero-overlay BottleOil guard repair. | Complete |
| source-batch-339-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-339 discovery with recommended BottleOil target. | Complete |
| source-batch-339-target.md | Source batches | Durable target for SOURCE-BATCH-339 BottleOil guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-339-bottleoil-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-339 with BottleOil guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-340-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-340 candidate discovery for a zero-gate, zero-overlay RuneStoneGate guard repair. | Complete |
| source-batch-340-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-340 discovery with recommended RuneStoneGate target. | Complete |
| source-batch-340-target.md | Source batches | Durable target for SOURCE-BATCH-340 RuneStoneGate guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-340-runestonegate-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-340 with RuneStoneGate guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-341-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-341 candidate discovery for a zero-gate, zero-overlay JokeBook guard repair. | Complete |
| source-batch-341-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-341 discovery with recommended JokeBook target. | Complete |
| source-batch-341-target.md | Source batches | Durable target for SOURCE-BATCH-341 JokeBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-341-jokebook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-341 with JokeBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-342-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-342 candidate discovery for a zero-gate, zero-overlay LanternOfDiscipline guard repair and deferred sibling candidates. | Complete |
| source-batch-342-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-342 discovery with recommended LanternOfDiscipline target and deferred sibling queue. | Complete |
| source-batch-342-target.md | Source batches | Durable target for SOURCE-BATCH-342 LanternOfDiscipline guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-342-lanternofdiscipline-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-342 with LanternOfDiscipline guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-343-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-343 candidate discovery for a zero-gate, zero-overlay OrbOfLogic guard repair and deferred sibling candidates. | Complete |
| source-batch-343-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-343 discovery with recommended OrbOfLogic target and deferred sibling queue. | Complete |
| source-batch-343-target.md | Source batches | Durable target for SOURCE-BATCH-343 OrbOfLogic guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-343-orboflogic-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-343 with OrbOfLogic guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-344-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-344 candidate discovery for a zero-gate, zero-overlay ScalesOfEthicality guard repair and deferred sibling candidates. | Complete |
| source-batch-344-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-344 discovery with recommended ScalesOfEthicality target and deferred sibling queue. | Complete |
| source-batch-344-target.md | Source batches | Durable target for SOURCE-BATCH-344 ScalesOfEthicality guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-344-scalesofethicality-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-344 with ScalesOfEthicality guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-345-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-345 candidate discovery for a zero-gate, zero-overlay BookOfTruth guard repair and deferred sibling candidates. | Complete |
| source-batch-345-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-345 discovery with recommended BookOfTruth target and deferred sibling queue. | Complete |
| source-batch-345-target.md | Source batches | Durable target for SOURCE-BATCH-345 BookOfTruth guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-345-bookoftruth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-345 with BookOfTruth guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-346-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-346 candidate discovery for a zero-gate, zero-overlay CandleOfLove guard repair and deferred sibling candidates. | Complete |
| source-batch-346-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-346 discovery with recommended CandleOfLove target and deferred sibling queue. | Complete |
| source-batch-346-target.md | Source batches | Durable target for SOURCE-BATCH-346 CandleOfLove guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-346-candleoflove-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-346 with CandleOfLove guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-347-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-347 candidate discovery for a zero-gate, zero-overlay BellOfCourage guard repair and deferred sibling candidates. | Complete |
| source-batch-347-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-347 discovery with recommended BellOfCourage target and deferred sibling queue. | Complete |
| source-batch-347-target.md | Source batches | Durable target for SOURCE-BATCH-347 BellOfCourage guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-347-bellofcourage-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-347 with BellOfCourage guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-348-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-348 candidate discovery for a zero-gate, zero-overlay ShardOfHatred guard repair and deferred sibling candidates. | Complete |
| source-batch-348-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-348 discovery with recommended ShardOfHatred target and deferred sibling queue. | Complete |
| source-batch-348-target.md | Source batches | Durable target for SOURCE-BATCH-348 ShardOfHatred guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-348-shardofhatred-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-348 with ShardOfHatred guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-349-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-349 candidate discovery for a zero-gate, zero-overlay ShardOfFalsehood guard repair and deferred sibling candidate. | Complete |
| source-batch-349-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-349 discovery with recommended ShardOfFalsehood target and deferred sibling queue. | Complete |
| source-batch-349-target.md | Source batches | Durable target for SOURCE-BATCH-349 ShardOfFalsehood guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-349-shardoffalsehood-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-349 with ShardOfFalsehood guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-350-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-350 candidate discovery for a zero-gate, zero-overlay ShardOfCowardice guard repair. | Complete |
| source-batch-350-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-350 discovery with recommended ShardOfCowardice target and exhausted deferred sibling queue. | Complete |
| source-batch-350-target.md | Source batches | Durable target for SOURCE-BATCH-350 ShardOfCowardice guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-350-shardofcowardice-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-350 with ShardOfCowardice guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-351-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-351 candidate discovery for a zero-gate, zero-overlay HighSeasRelic guard repair. | Complete |
| source-batch-351-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-351 discovery with recommended HighSeasRelic target and skipped candidate notes. | Complete |
| source-batch-351-target.md | Source batches | Durable target for SOURCE-BATCH-351 HighSeasRelic guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-351-highseasrelic-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-351 with HighSeasRelic guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-352-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-352 candidate discovery for a zero-gate, zero-overlay SpecialSeaweed guard repair. | Complete |
| source-batch-352-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-352 discovery with recommended SpecialSeaweed target and skipped candidate notes. | Complete |
| source-batch-352-target.md | Source batches | Durable target for SOURCE-BATCH-352 SpecialSeaweed guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-352-specialseaweed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-352 with SpecialSeaweed guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-353-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-353 candidate discovery for a zero-gate, zero-overlay ObeliskTip guard repair. | Complete |
| source-batch-353-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-353 discovery with recommended ObeliskTip target and skipped candidate notes. | Complete |
| source-batch-353-target.md | Source batches | Durable target for SOURCE-BATCH-353 ObeliskTip guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-353-obelisktip-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-353 with ObeliskTip guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-354-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-354 candidate discovery for a zero-gate, zero-overlay MuseumBook guard repair. | Complete |
| source-batch-354-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-354 discovery with recommended MuseumBook target and skipped candidate notes. | Complete |
| source-batch-354-target.md | Source batches | Durable target for SOURCE-BATCH-354 MuseumBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-354-museumbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-354 with MuseumBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-355-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-355 candidate discovery for a zero-gate, zero-overlay ThiefNote guard repair. | Complete |
| source-batch-355-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-355 discovery with recommended ThiefNote target and skipped candidate notes. | Complete |
| source-batch-355-target.md | Source batches | Durable target for SOURCE-BATCH-355 ThiefNote guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-355-thiefnote-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-355 with ThiefNote guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-356-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-356 candidate discovery for a zero-gate, zero-overlay FrankenJournalInBox guard repair. | Complete |
| source-batch-356-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-356 discovery with recommended FrankenJournalInBox target and skipped candidate notes. | Complete |
| source-batch-356-target.md | Source batches | Durable target for SOURCE-BATCH-356 FrankenJournalInBox guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-356-frankenjournalinbox-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-356 with FrankenJournalInBox guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-357-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-357 candidate discovery for a zero-gate, zero-overlay ObeliskOnCorpse guard repair. | Complete |
| source-batch-357-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-357 discovery with recommended ObeliskOnCorpse target and skipped candidate notes. | Complete |
| source-batch-357-target.md | Source batches | Durable target for SOURCE-BATCH-357 ObeliskOnCorpse guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-357-obeliskoncorpse-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-357 with ObeliskOnCorpse guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-358-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-358 candidate discovery for a zero-gate, zero-overlay SerpentSpawners guard repair. | Complete |
| source-batch-358-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-358 discovery with recommended SerpentSpawners target and skipped candidate notes. | Complete |
| source-batch-358-target.md | Source batches | Durable target for SOURCE-BATCH-358 SerpentSpawners guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-358-serpentspawners-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-358 with SerpentSpawners guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-359-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-359 candidate discovery for a zero-gate, zero-overlay Museums guard repair. | Complete |
| source-batch-359-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-359 discovery with recommended Museums target and skipped candidate notes. | Complete |
| source-batch-359-target.md | Source batches | Durable target for SOURCE-BATCH-359 Museums guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-359-museums-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-359 with Museums guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-360-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-360 candidate discovery for a zero-gate, zero-overlay QuestChests guard repair. | Complete |
| source-batch-360-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-360 discovery with recommended QuestChests target and skipped candidate notes. | Complete |
| source-batch-360-target.md | Source batches | Durable target for SOURCE-BATCH-360 QuestChests guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-360-questchests-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-360 with QuestChests guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-361-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-361 candidate discovery for a zero-gate, zero-overlay PaganArtifact guard repair. | Complete |
| source-batch-361-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-361 discovery with recommended PaganArtifact target and skipped candidate notes. | Complete |
| source-batch-361-target.md | Source batches | Durable target for SOURCE-BATCH-361 PaganArtifact guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-361-paganartifact-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-361 with PaganArtifact guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-362-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-362 candidate discovery for a zero-gate, zero-overlay SearchBook guard repair. | Complete |
| source-batch-362-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-362 discovery with recommended SearchBook target and skipped candidate notes. | Complete |
| source-batch-362-target.md | Source batches | Durable target for SOURCE-BATCH-362 SearchBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-362-searchbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-362 with SearchBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-363-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-363 candidate discovery for a zero-gate, zero-overlay Prisoner guard repair. | Complete |
| source-batch-363-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-363 discovery with recommended Prisoner target and skipped candidate notes. | Complete |
| source-batch-363-target.md | Source batches | Durable target for SOURCE-BATCH-363 Prisoner guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-363-prisoner-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-363 with Prisoner guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-364-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-364 candidate discovery for a zero-gate, zero-overlay RobotSchematics guard repair. | Complete |
| source-batch-364-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-364 discovery with recommended RobotSchematics target and skipped candidate notes. | Complete |
| source-batch-364-target.md | Source batches | Durable target for SOURCE-BATCH-364 RobotSchematics guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-364-robotschematics-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-364 with RobotSchematics guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-365-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-365 candidate discovery for a zero-gate, zero-overlay DDRelicArts guard repair. | Complete |
| source-batch-365-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-365 discovery with recommended DDRelicArts target and skipped candidate notes. | Complete |
| source-batch-365-target.md | Source batches | Durable target for SOURCE-BATCH-365 DDRelicArts guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-365-ddrelicarts-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-365 with DDRelicArts guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-366-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-366 candidate discovery for a zero-gate, zero-overlay DDRelicCloth guard repair. | Complete |
| source-batch-366-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-366 discovery with recommended DDRelicCloth target and skipped candidate notes. | Complete |
| source-batch-366-target.md | Source batches | Durable target for SOURCE-BATCH-366 DDRelicCloth guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-366-ddreliccloth-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-366 with DDRelicCloth guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-367-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-367 candidate discovery for a zero-gate, zero-overlay DDRelicFur guard repair. | Complete |
| source-batch-367-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-367 discovery with recommended DDRelicFur target and skipped candidate notes. | Complete |
| source-batch-367-target.md | Source batches | Durable target for SOURCE-BATCH-367 DDRelicFur guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-367-ddrelicfur-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-367 with DDRelicFur guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-368-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-368 candidate discovery for a zero-gate, zero-overlay DDRelicGem guard repair. | Complete |
| source-batch-368-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-368 discovery with recommended DDRelicGem target and skipped candidate notes. | Complete |
| source-batch-368-target.md | Source batches | Durable target for SOURCE-BATCH-368 DDRelicGem guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-368-ddrelicgem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-368 with DDRelicGem guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-369-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-369 candidate discovery for a zero-gate, zero-overlay DDRelicJewels guard repair. | Complete |
| source-batch-369-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-369 discovery with recommended DDRelicJewels target and skipped candidate notes. | Complete |
| source-batch-369-target.md | Source batches | Durable target for SOURCE-BATCH-369 DDRelicJewels guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-369-ddrelicjewels-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-369 with DDRelicJewels guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-370-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-370 candidate discovery for a zero-gate, zero-overlay DDRelicLeather guard repair. | Complete |
| source-batch-370-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-370 discovery with recommended DDRelicLeather target and skipped candidate notes. | Complete |
| source-batch-370-target.md | Source batches | Durable target for SOURCE-BATCH-370 DDRelicLeather guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-370-ddrelicleather-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-370 with DDRelicLeather guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-371-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-371 candidate discovery for a zero-gate, zero-overlay DDRelicLight guard repair. | Complete |
| source-batch-371-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-371 discovery with recommended DDRelicLight target and skipped candidate notes. | Complete |
| source-batch-371-target.md | Source batches | Durable target for SOURCE-BATCH-371 DDRelicLight guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-371-ddreliclight-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-371 with DDRelicLight guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-372-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-372 candidate discovery for a zero-gate, zero-overlay DDRelicReagent guard repair. | Complete |
| source-batch-372-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-372 discovery with recommended DDRelicReagent target and skipped candidate notes. | Complete |
| source-batch-372-target.md | Source batches | Durable target for SOURCE-BATCH-372 DDRelicReagent guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-372-ddrelicreagent-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-372 with DDRelicReagent guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-373-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-373 candidate discovery for a zero-gate, zero-overlay DDRelicScrolls guard repair. | Complete |
| source-batch-373-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-373 discovery with recommended DDRelicScrolls target and skipped candidate notes. | Complete |
| source-batch-373-target.md | Source batches | Durable target for SOURCE-BATCH-373 DDRelicScrolls guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-373-ddrelicscrolls-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-373 with DDRelicScrolls guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-374-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-374 candidate discovery for a zero-gate, zero-overlay DDRelicVase guard repair. | Complete |
| source-batch-374-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-374 discovery with recommended DDRelicVase target and skipped candidate notes. | Complete |
| source-batch-374-target.md | Source batches | Durable target for SOURCE-BATCH-374 DDRelicVase guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-374-ddrelicvase-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-374 with DDRelicVase guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-375-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-375 candidate discovery for a zero-gate, zero-overlay DDRelicTablet guard repair. | Complete |
| source-batch-375-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-375 discovery with recommended DDRelicTablet target and skipped candidate notes. | Complete |
| source-batch-375-target.md | Source batches | Durable target for SOURCE-BATCH-375 DDRelicTablet guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-375-ddrelictablet-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-375 with DDRelicTablet guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-376-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-376 candidate discovery for a zero-gate, zero-overlay DDRelicDrink guard repair. | Complete |
| source-batch-376-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-376 discovery with recommended DDRelicDrink target and skipped candidate notes. | Complete |
| source-batch-376-target.md | Source batches | Durable target for SOURCE-BATCH-376 DDRelicDrink guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-376-ddrelicdrink-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-376 with DDRelicDrink guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-377-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-377 candidate discovery for a zero-gate, zero-overlay DDRelicOrbs guard repair. | Complete |
| source-batch-377-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-377 discovery with recommended DDRelicOrbs target and skipped candidate notes. | Complete |
| source-batch-377-target.md | Source batches | Durable target for SOURCE-BATCH-377 DDRelicOrbs guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-377-ddrelicorbs-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-377 with DDRelicOrbs guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-378-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-378 candidate discovery for a zero-gate, zero-overlay DoorStuck guard repair. | Complete |
| source-batch-378-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-378 discovery with recommended DoorStuck target and skipped candidate notes. | Complete |
| source-batch-378-target.md | Source batches | Durable target for SOURCE-BATCH-378 DoorStuck guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-378-doorstuck-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-378 with DoorStuck guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-379-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-379 candidate discovery for a zero-gate, zero-overlay GypsyShelf guard repair. | Complete |
| source-batch-379-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-379 discovery with recommended GypsyShelf target and skipped candidate notes. | Complete |
| source-batch-379-target.md | Source batches | Durable target for SOURCE-BATCH-379 GypsyShelf guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-379-gypsyshelf-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-379 with GypsyShelf guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-380-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-380 candidate discovery for a zero-gate, zero-overlay Safe guard repair. | Complete |
| source-batch-380-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-380 discovery with recommended Safe target and skipped candidate notes. | Complete |
| source-batch-380-target.md | Source batches | Durable target for SOURCE-BATCH-380 Safe guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-380-safe-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-380 with Safe guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-381-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-381 candidate discovery for a zero-gate, zero-overlay DoomFlayerNote guard repair. | Complete |
| source-batch-381-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-381 discovery with recommended DoomFlayerNote target and skipped candidate notes. | Complete |
| source-batch-381-target.md | Source batches | Durable target for SOURCE-BATCH-381 DoomFlayerNote guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-381-doomflayernote-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-381 with DoomFlayerNote guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-382-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-382 candidate discovery for a zero-gate, zero-overlay BardsTaleNote guard repair. | Complete |
| source-batch-382-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-382 discovery with recommended BardsTaleNote target and skipped candidate notes. | Complete |
| source-batch-382-target.md | Source batches | Durable target for SOURCE-BATCH-382 BardsTaleNote guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-382-bardstalenote-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-382 with BardsTaleNote guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-383-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-383 candidate discovery for a zero-gate, zero-overlay AssassinNote guard repair. | Complete |
| source-batch-383-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-383 discovery with recommended AssassinNote target and skipped candidate notes. | Complete |
| source-batch-383-target.md | Source batches | Durable target for SOURCE-BATCH-383 AssassinNote guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-383-assassinnote-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-383 with AssassinNote guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-384-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-384 candidate discovery for a zero-gate, zero-overlay GuardNote guard repair. | Complete |
| source-batch-384-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-384 discovery with recommended GuardNote target and skipped candidate notes. | Complete |
| source-batch-384-target.md | Source batches | Durable target for SOURCE-BATCH-384 GuardNote guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-384-guardnote-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-384 with GuardNote guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-385-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-385 candidate discovery for a zero-gate, zero-overlay StatusBoard guard repair. | Complete |
| source-batch-385-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-385 discovery with recommended StatusBoard target and skipped candidate notes. | Complete |
| source-batch-385-target.md | Source batches | Durable target for SOURCE-BATCH-385 StatusBoard guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-385-statusboard-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-385 with StatusBoard guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-386-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-386 candidate discovery for a zero-gate, zero-overlay Clock guard repair. | Complete |
| source-batch-386-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-386 discovery with recommended Clock target and skipped candidate notes. | Complete |
| source-batch-386-target.md | Source batches | Durable target for SOURCE-BATCH-386 Clock guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-386-clock-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-386 with Clock guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-387-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-387 candidate discovery for a zero-gate, zero-overlay MedicalRecord guard repair. | Complete |
| source-batch-387-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-387 discovery with recommended MedicalRecord target and skipped candidate notes. | Complete |
| source-batch-387-target.md | Source batches | Durable target for SOURCE-BATCH-387 MedicalRecord guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-387-medicalrecord-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-387 with MedicalRecord guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-388-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-388 candidate discovery for a zero-gate, zero-overlay WetClothes guard repair. | Complete |
| source-batch-388-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-388 discovery with recommended WetClothes target and skipped candidate notes. | Complete |
| source-batch-388-target.md | Source batches | Durable target for SOURCE-BATCH-388 WetClothes guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-388-wetclothes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-388 with WetClothes guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-389-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-389 candidate discovery for a zero-gate, zero-overlay HalloweenMaiden guard repair. | Complete |
| source-batch-389-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-389 discovery with recommended HalloweenMaiden target and skipped candidate notes. | Complete |
| source-batch-389-target.md | Source batches | Durable target for SOURCE-BATCH-389 HalloweenMaiden guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-389-halloweenmaiden-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-389 with HalloweenMaiden guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-390-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-390 candidate discovery for a zero-gate, zero-overlay ChocolateMonster guard repair. | Complete |
| source-batch-390-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-390 discovery with recommended ChocolateMonster target and skipped candidate notes. | Complete |
| source-batch-390-target.md | Source batches | Durable target for SOURCE-BATCH-390 ChocolateMonster guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-390-chocolatemonster-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-390 with ChocolateMonster guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-391-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-391 candidate discovery for a zero-gate, zero-overlay LiarsDice guard repair. | Complete |
| source-batch-391-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-391 discovery with recommended LiarsDice target and skipped candidate notes. | Complete |
| source-batch-391-target.md | Source batches | Durable target for SOURCE-BATCH-391 LiarsDice guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-391-liarsdice-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-391 with LiarsDice guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-392-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-392 candidate discovery for a zero-gate, zero-overlay MahjongGame guard repair. | Complete |
| source-batch-392-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-392 discovery with recommended MahjongGame target and skipped candidate notes. | Complete |
| source-batch-392-target.md | Source batches | Durable target for SOURCE-BATCH-392 MahjongGame guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-392-mahjonggame-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-392 with MahjongGame guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-393-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-393 candidate discovery for a zero-gate, zero-overlay PlayersHandbook guard repair. | Complete |
| source-batch-393-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-393 discovery with recommended PlayersHandbook target and skipped candidate notes. | Complete |
| source-batch-393-target.md | Source batches | Durable target for SOURCE-BATCH-393 PlayersHandbook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-393-playershandbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-393 with PlayersHandbook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-394-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-394 candidate discovery for a zero-gate, zero-overlay MonsterManual guard repair. | Complete |
| source-batch-394-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-394 discovery with recommended MonsterManual target and skipped candidate notes. | Complete |
| source-batch-394-target.md | Source batches | Durable target for SOURCE-BATCH-394 MonsterManual guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-394-monstermanual-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-394 with MonsterManual guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-395-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-395 candidate discovery for a zero-gate, zero-overlay Tarot guard repair. | Complete |
| source-batch-395-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-395 discovery with recommended Tarot target and skipped candidate notes. | Complete |
| source-batch-395-target.md | Source batches | Durable target for SOURCE-BATCH-395 Tarot guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-395-tarot-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-395 with Tarot guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-396-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-396 candidate discovery for a zero-gate, zero-overlay Tarot Poker guard repair. | Complete |
| source-batch-396-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-396 discovery with recommended Tarot Poker target and skipped candidate notes. | Complete |
| source-batch-396-target.md | Source batches | Durable target for SOURCE-BATCH-396 Tarot Poker guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-396-tarotpoker-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-396 with Tarot Poker guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-397-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-397 candidate discovery for a zero-gate, zero-overlay FireworksWand guard repair. | Complete |
| source-batch-397-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-397 discovery with recommended FireworksWand target and skipped candidate notes. | Complete |
| source-batch-397-target.md | Source batches | Durable target for SOURCE-BATCH-397 FireworksWand guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-397-fireworkswand-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-397 with FireworksWand guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-398-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-398 candidate discovery for a zero-gate, zero-overlay BaseHat cowl hood guard repair. | Complete |
| source-batch-398-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-398 discovery with recommended BaseHat target and skipped candidate notes. | Complete |
| source-batch-398-target.md | Source batches | Durable target for SOURCE-BATCH-398 BaseHat cowl hood guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-398-basehat-cowl-hood-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-398 with BaseHat cowl hood guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-399-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-399 candidate discovery for a zero-gate, zero-overlay Sextant guard repair. | Complete |
| source-batch-399-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-399 discovery with recommended Sextant target and skipped candidate notes. | Complete |
| source-batch-399-target.md | Source batches | Durable target for SOURCE-BATCH-399 Sextant guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-399-sextant-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-399 with Sextant guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-400-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-400 candidate discovery for a zero-gate, zero-overlay BaseKnife guard repair. | Complete |
| source-batch-400-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-400 discovery with recommended BaseKnife target and skipped candidate notes. | Complete |
| source-batch-400-target.md | Source batches | Durable target for SOURCE-BATCH-400 BaseKnife guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-400-baseknife-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-400 with BaseKnife guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-401-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-401 candidate discovery for a zero-gate, zero-overlay BaseSword guard repair. | Complete |
| source-batch-401-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-401 discovery with recommended BaseSword target and skipped candidate notes. | Complete |
| source-batch-401-target.md | Source batches | Durable target for SOURCE-BATCH-401 BaseSword guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-401-basesword-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-401 with BaseSword guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-402-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-402 candidate discovery for a zero-gate, zero-overlay ThrowingWeapon guard repair. | Complete |
| source-batch-402-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-402 discovery with recommended ThrowingWeapon target and skipped candidate notes. | Complete |
| source-batch-402-target.md | Source batches | Durable target for SOURCE-BATCH-402 ThrowingWeapon guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-402-throwingweapon-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-402 with ThrowingWeapon guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-403-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-403 candidate discovery for a zero-gate, zero-overlay ThrowingGloves guard repair. | Complete |
| source-batch-403-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-403 discovery with recommended ThrowingGloves target and skipped candidate notes. | Complete |
| source-batch-403-target.md | Source batches | Durable target for SOURCE-BATCH-403 ThrowingGloves guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-403-throwinggloves-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-403 with ThrowingGloves guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-404-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-404 candidate discovery for a zero-gate, zero-overlay LevelThrowingGloves guard repair. | Complete |
| source-batch-404-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-404 discovery with recommended LevelThrowingGloves target and skipped candidate notes. | Complete |
| source-batch-404-target.md | Source batches | Durable target for SOURCE-BATCH-404 LevelThrowingGloves guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-404-levelthrowinggloves-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-404 with LevelThrowingGloves guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-405-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-405 candidate discovery for a zero-gate, zero-overlay ThrowingDagger guard repair. | Complete |
| source-batch-405-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-405 discovery with recommended ThrowingDagger target and skipped candidate notes. | Complete |
| source-batch-405-target.md | Source batches | Durable target for SOURCE-BATCH-405 ThrowingDagger guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-405-throwingdagger-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-405 with ThrowingDagger guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-406-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-406 candidate discovery for a zero-gate, zero-overlay Scissors guard repair. | Complete |
| source-batch-406-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-406 discovery with recommended Scissors target and skipped candidate notes. | Complete |
| source-batch-406-target.md | Source batches | Durable target for SOURCE-BATCH-406 Scissors guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-406-scissors-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-406 with Scissors guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-407-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-407 candidate discovery for a zero-gate, zero-overlay NewPlayerTicket guard repair. | Complete |
| source-batch-407-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-407 discovery with recommended NewPlayerTicket target and skipped candidate notes. | Complete |
| source-batch-407-target.md | Source batches | Durable target for SOURCE-BATCH-407 NewPlayerTicket guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-407-newplayerticket-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-407 with NewPlayerTicket guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-408-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-408 candidate discovery for a zero-gate, zero-overlay TitleChangeDeed guard repair. | Complete |
| source-batch-408-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-408 discovery with recommended TitleChangeDeed target and skipped candidate notes. | Complete |
| source-batch-408-target.md | Source batches | Durable target for SOURCE-BATCH-408 TitleChangeDeed guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-408-titlechangedeed-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-408 with TitleChangeDeed guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-409-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-409 candidate discovery for a zero-gate, zero-overlay BaseStatue guard repair. | Complete |
| source-batch-409-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-409 discovery with recommended BaseStatue target and skipped candidate notes. | Complete |
| source-batch-409-target.md | Source batches | Durable target for SOURCE-BATCH-409 BaseStatue guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-409-basestatue-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-409 with BaseStatue guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-410-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-410 candidate discovery for a zero-gate, zero-overlay BaseBook guard repair. | Complete |
| source-batch-410-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-410 discovery with recommended BaseBook target and skipped candidate notes. | Complete |
| source-batch-410-target.md | Source batches | Durable target for SOURCE-BATCH-410 BaseBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-410-basebook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-410 with BaseBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-411-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-411 candidate discovery for a zero-gate, zero-overlay SackOfHolding guard repair. | Complete |
| source-batch-411-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-411 discovery with recommended SackOfHolding target and skipped candidate notes. | Complete |
| source-batch-411-target.md | Source batches | Durable target for SOURCE-BATCH-411 SackOfHolding guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-411-sackofholding-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-411 with SackOfHolding guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-412-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-412 candidate discovery for a zero-gate, zero-overlay Watcher guard repair. | Complete |
| source-batch-412-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-412 discovery with recommended Watcher target and skipped candidate notes. | Complete |
| source-batch-412-target.md | Source batches | Durable target for SOURCE-BATCH-412 Watcher guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-412-watcher-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-412 with Watcher guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-413-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-413 candidate discovery for a zero-gate, zero-overlay ColoringBook guard repair. | Complete |
| source-batch-413-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-413 discovery with recommended ColoringBook target and skipped candidate notes. | Complete |
| source-batch-413-target.md | Source batches | Durable target for SOURCE-BATCH-413 ColoringBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-413-coloringbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-413 with ColoringBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-414-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-414 candidate discovery for a zero-gate, zero-overlay HintItem guard repair. | Complete |
| source-batch-414-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-414 discovery with recommended HintItem target and skipped candidate notes. | Complete |
| source-batch-414-target.md | Source batches | Durable target for SOURCE-BATCH-414 HintItem guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-414-hintitem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-414 with HintItem guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-415-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-415 candidate discovery for a zero-gate, zero-overlay WindChimes guard repair. | Complete |
| source-batch-415-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-415 discovery with recommended WindChimes target and skipped candidate notes. | Complete |
| source-batch-415-target.md | Source batches | Durable target for SOURCE-BATCH-415 WindChimes guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-415-windchimes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-415 with WindChimes guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-416-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-416 candidate discovery for a zero-gate, zero-overlay HouseSign rename guard repair. | Complete |
| source-batch-416-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-416 discovery with recommended HouseSign target and skipped candidate notes. | Complete |
| source-batch-416-target.md | Source batches | Durable target for SOURCE-BATCH-416 HouseSign rename guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-416-housesign-rename-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-416 with HouseSign rename guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-417-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-417 candidate discovery for a zero-gate, zero-overlay TarotCards guard repair. | Complete |
| source-batch-417-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-417 discovery with recommended TarotCards target and skipped candidate notes. | Complete |
| source-batch-417-target.md | Source batches | Durable target for SOURCE-BATCH-417 TarotCards guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-417-tarotcards-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-417 with TarotCards guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-418-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-418 candidate discovery for a zero-gate, zero-overlay HolidayBells guard repair. | Complete |
| source-batch-418-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-418 discovery with recommended HolidayBells target and skipped candidate notes. | Complete |
| source-batch-418-target.md | Source batches | Durable target for SOURCE-BATCH-418 HolidayBells guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-418-holidaybells-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-418 with HolidayBells guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-419-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-419 candidate discovery for a zero-gate, zero-overlay Halloween dart boards guard repair. | Complete |
| source-batch-419-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-419 discovery with recommended DartBoards target and skipped candidate notes. | Complete |
| source-batch-419-target.md | Source batches | Durable target for SOURCE-BATCH-419 DartBoards guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-419-dartboards-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-419 with DartBoards guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-420-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-420 candidate discovery for a zero-gate, zero-overlay HalloweenGraves rename guard repair. | Complete |
| source-batch-420-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-420 discovery with recommended HalloweenGraves target and skipped candidate notes. | Complete |
| source-batch-420-target.md | Source batches | Durable target for SOURCE-BATCH-420 HalloweenGraves rename guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-420-halloweengraves-rename-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-420 with HalloweenGraves guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-421-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-421 candidate discovery for a zero-gate, zero-overlay Guillotine guard repair. | Complete |
| source-batch-421-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-421 discovery with recommended Guillotine target and skipped candidate notes. | Complete |
| source-batch-421-target.md | Source batches | Durable target for SOURCE-BATCH-421 Guillotine guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-421-guillotine-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-421 with Guillotine guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-422-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-422 candidate discovery for a zero-gate, zero-overlay BaseImprisonedMobile guard repair. | Complete |
| source-batch-422-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-422 discovery with recommended BaseImprisonedMobile target and skipped candidate notes. | Complete |
| source-batch-422-target.md | Source batches | Durable target for SOURCE-BATCH-422 BaseImprisonedMobile guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-422-baseimprisonedmobile-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-422 with BaseImprisonedMobile guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-423-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-423 candidate discovery for a zero-gate, zero-overlay PowderOfTranslocation guard repair. | Complete |
| source-batch-423-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-423 discovery with recommended PowderOfTranslocation target and skipped candidate notes. | Complete |
| source-batch-423-target.md | Source batches | Durable target for SOURCE-BATCH-423 PowderOfTranslocation guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-423-powderoftranslocation-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-423 with PowderOfTranslocation guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-424-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-424 candidate discovery for a zero-gate, zero-overlay HairDye guard repair. | Complete |
| source-batch-424-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-424 discovery with recommended HairDye target and skipped candidate notes. | Complete |
| source-batch-424-target.md | Source batches | Durable target for SOURCE-BATCH-424 HairDye guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-424-hairdye-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-424 with HairDye guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-425-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-425 candidate discovery for a zero-gate, zero-overlay SpecialHairDye guard repair. | Complete |
| source-batch-425-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-425 discovery with recommended SpecialHairDye target and skipped candidate notes. | Complete |
| source-batch-425-target.md | Source batches | Durable target for SOURCE-BATCH-425 SpecialHairDye guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-425-specialhairdye-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-425 with SpecialHairDye guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-426-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-426 candidate discovery for a zero-gate, zero-overlay SpecialBeardDye guard repair. | Complete |
| source-batch-426-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-426 discovery with recommended SpecialBeardDye target and skipped candidate notes. | Complete |
| source-batch-426-target.md | Source batches | Durable target for SOURCE-BATCH-426 SpecialBeardDye guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-426-specialbearddye-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-426 with SpecialBeardDye guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-427-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-427 candidate discovery for a zero-gate, zero-overlay RuneOfVirtue guard repair. | Complete |
| source-batch-427-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-427 discovery with recommended RuneOfVirtue target and skipped candidate notes. | Complete |
| source-batch-427-target.md | Source batches | Durable target for SOURCE-BATCH-427 RuneOfVirtue guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-427-runeofvirtue-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-427 with RuneOfVirtue guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-428-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-428 candidate discovery for a zero-gate, zero-overlay SoulLantern guard repair. | Complete |
| source-batch-428-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-428 discovery with recommended SoulLantern target and skipped candidate notes. | Complete |
| source-batch-428-target.md | Source batches | Durable target for SOURCE-BATCH-428 SoulLantern guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-428-soullantern-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-428 with SoulLantern guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-429-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-429 candidate discovery for a zero-gate, zero-overlay ForgetfulGem guard repair. | Complete |
| source-batch-429-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-429 discovery with recommended ForgetfulGem target and skipped candidate notes. | Complete |
| source-batch-429-target.md | Source batches | Durable target for SOURCE-BATCH-429 ForgetfulGem guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-429-forgetfulgem-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-429 with ForgetfulGem guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-430-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-430 candidate discovery for a zero-gate, zero-overlay SongBook guard repair. | Complete |
| source-batch-430-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-430 discovery with recommended SongBook target and skipped candidate notes. | Complete |
| source-batch-430-target.md | Source batches | Durable target for SOURCE-BATCH-430 SongBook guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-430-songbook-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-430 with SongBook guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-431-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-431 candidate discovery for a zero-gate, zero-overlay MagicObjectTarget guard repair. | Complete |
| source-batch-431-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-431 discovery with recommended MagicObjectTarget target and skipped candidate notes. | Complete |
| source-batch-431-target.md | Source batches | Durable target for SOURCE-BATCH-431 MagicObjectTarget guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-431-magicobjecttarget-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-431 with MagicObjectTarget guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-432-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-432 candidate discovery for a zero-gate, zero-overlay RobeOfTeleportation guard repair. | Complete |
| source-batch-432-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-432 discovery with recommended RobeOfTeleportation target and skipped candidate notes. | Complete |
| source-batch-432-target.md | Source batches | Durable target for SOURCE-BATCH-432 RobeOfTeleportation guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-432-robeofteleportation-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-432 with RobeOfTeleportation guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-433-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-433 candidate discovery for a zero-gate, zero-overlay RecipeScroll guard repair. | Complete |
| source-batch-433-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-433 discovery with recommended RecipeScroll target and skipped candidate notes. | Complete |
| source-batch-433-target.md | Source batches | Durable target for SOURCE-BATCH-433 RecipeScroll guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-433-recipescroll-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-433 with RecipeScroll guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-434-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-434 candidate discovery for a zero-gate, zero-overlay MysticPack guard repair. | Complete |
| source-batch-434-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-434 discovery with recommended MysticPack target and skipped candidate notes. | Complete |
| source-batch-434-target.md | Source batches | Durable target for SOURCE-BATCH-434 MysticPack guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-434-mysticpack-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-434 with MysticPack guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-435-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-435 candidate discovery for a zero-gate, zero-overlay Fukiya guard repair. | Complete |
| source-batch-435-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-435 discovery with recommended Fukiya target and skipped candidate notes. | Complete |
| source-batch-435-target.md | Source batches | Durable target for SOURCE-BATCH-435 Fukiya guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-435-fukiya-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-435 with Fukiya guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-436-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-436 candidate discovery for a zero-gate, zero-overlay LeatherNinjaBelt guard repair. | Complete |
| source-batch-436-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-436 discovery with recommended LeatherNinjaBelt target and skipped candidate notes. | Complete |
| source-batch-436-target.md | Source batches | Durable target for SOURCE-BATCH-436 LeatherNinjaBelt guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-436-leatherninjabelt-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-436 with LeatherNinjaBelt guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-437-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-437 candidate discovery for a zero-gate, zero-overlay GandalfsStaff guard repair. | Complete |
| source-batch-437-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-437 discovery with recommended GandalfsStaff target and skipped/deferred candidate notes. | Complete |
| source-batch-437-target.md | Source batches | Durable target for SOURCE-BATCH-437 GandalfsStaff guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-437-gandalfsstaff-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-437 with GandalfsStaff guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-438-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-438 candidate discovery for a zero-gate, zero-overlay StaffofSnakes guard repair. | Complete |
| source-batch-438-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-438 discovery with recommended StaffofSnakes target and skipped candidate notes. | Complete |
| source-batch-438-target.md | Source batches | Durable target for SOURCE-BATCH-438 StaffofSnakes guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-438-staffofsnakes-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-438 with StaffofSnakes guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-439-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-439 candidate discovery for a zero-gate, zero-overlay Head guard repair. | Complete |
| source-batch-439-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-439 discovery with recommended Head target and skipped candidate notes. | Complete |
| source-batch-439-target.md | Source batches | Durable target for SOURCE-BATCH-439 Head guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-439-head-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-439 with Head guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-440-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-440 candidate discovery for a zero-gate, zero-overlay EssenceOrb guard repair. | Complete |
| source-batch-440-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-440 discovery with recommended EssenceOrb target and skipped candidate notes. | Complete |
| source-batch-440-target.md | Source batches | Durable target for SOURCE-BATCH-440 EssenceOrb guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-440-essenceorb-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-440 with EssenceOrb guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-441-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-441 candidate discovery for a zero-gate, zero-overlay TapestryOfSosaria guard repair. | Complete |
| source-batch-441-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-441 discovery with recommended TapestryOfSosaria target and skipped candidate notes. | Complete |
| source-batch-441-target.md | Source batches | Durable target for SOURCE-BATCH-441 TapestryOfSosaria guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-441-tapestryofsosaria-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-441 with TapestryOfSosaria guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-442-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-442 candidate discovery for a zero-gate, zero-overlay TowerLanternArtifact guard repair. | Complete |
| source-batch-442-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-442 discovery with recommended TowerLanternArtifact target and skipped candidate notes. | Complete |
| source-batch-442-target.md | Source batches | Durable target for SOURCE-BATCH-442 TowerLanternArtifact guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-442-towerlanternartifact-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-442 with TowerLanternArtifact guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-443-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-443 candidate discovery for a zero-gate, zero-overlay Lockpick guard repair. | Complete |
| source-batch-443-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-443 discovery with recommended Lockpick target and skipped candidate notes. | Complete |
| source-batch-443-target.md | Source batches | Durable target for SOURCE-BATCH-443 Lockpick guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-443-lockpick-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-443 with Lockpick guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-444-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-444 candidate discovery for a zero-gate, zero-overlay DoorSwitch guard repair. | Complete |
| source-batch-444-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-444 discovery with recommended DoorSwitch target and skipped candidate notes. | Complete |
| source-batch-444-target.md | Source batches | Durable target for SOURCE-BATCH-444 DoorSwitch guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-444-doorswitch-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-444 with DoorSwitch guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
| source-batch-445-candidate-discovery.csv | Source batches | Fresh SOURCE-BATCH-445 candidate discovery for a zero-gate, zero-overlay MagicStaffTarget guard repair. | Complete |
| source-batch-445-candidate-discovery-closeout.md | Source batches | Close out SOURCE-BATCH-445 discovery with recommended MagicStaffTarget target and skipped candidate notes. | Complete |
| source-batch-445-target.md | Source batches | Durable target for SOURCE-BATCH-445 MagicStaffTarget guard repair, including fence result and unchanged behavior. | Complete |
| source-batch-445-magicstafftarget-guard-repair-closeout.md | Source batches | Close out SOURCE-BATCH-445 with MagicStaffTarget guard changes, POST-BATCH-Y fence evidence, verification, and artifact restoration notes. | Complete |
