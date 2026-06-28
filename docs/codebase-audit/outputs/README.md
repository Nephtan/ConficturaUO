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
