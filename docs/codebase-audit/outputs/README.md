# Codebase Audit Outputs

This directory stores durable outputs produced by the codebase systems audit and reorganization phase runner.

Raw command output may remain temporary when it is easy to regenerate. Curated tables, reviewed registers, blocker records, accepted risks, and verification notes belong here so future runs do not depend on chat history.

## Output Policy

- Keep outputs source-verified and phase-labeled.
- Prefer repository-relative paths in tables.
- Exclude `bin` and `obj` unless a phase explicitly reviews generated output.
- Mark uncertain records as `Unknown`, `Partial`, `Blocked`, `Deferred`, or `Intentional` instead of guessing.
- Record generation commands and verification in `../RUN_LOG.md`.
- Do not approve reorganization moves from this directory alone; moves require Phase 12 design and Phase 6 save-compatibility review where serialized types are affected.

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
| `post-batch-e-hooks-gumps-commands-regions-review.csv` | Post-audit | Review and repair runtime hooks, gump guards, command access, and region/map assumptions by focused system groups; current coverage is 213 reviewed P1 runtime-hook/gump-guard rows through `POST-BATCH-E-72A`. | InProgress |

## Initial State

The Phase 0 baseline output, Phase 1 reproducible inventory outputs, Phase 2 project truth outputs, Phase 3 runtime inventory outputs, Phase 4 system cards, Phase 5 runtime hook map outputs, Phase 6 serialization/save-compatibility outputs, Phase 7 documentation truth outputs, Phase 8 dependency graph outputs, Phase 9 synergy/conflict outputs, Phase 10 risk-specific review outputs, Phase 11 inline code documentation outputs, Phase 12 reorganization design outputs, Phase 13 repair backlog outputs, and Phase 14 verification and commit workflow outputs have been generated. The audit runner records all phases as committed after the final status-record commit.
