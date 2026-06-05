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
| `cross-tree-runtime-inventory.*` | Phase 3 | Assign runtime role, owner, entry points, hooks, serialization, gumps, and config usage for source files. | NotStarted |
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
| `phase-05-gump-response-risk-register.csv` | Phase 5 | Record gump send/response rows and conservative guard-review flags. | Complete |
| `phase-05-timer-world-hook-register.csv` | Phase 5 | Record timers, delayed calls, and world save/load hook rows. | Complete |
| `phase-05-summary.md` | Phase 5 | Summarize Phase 5 inputs, generated outputs, hook counts, risk counts, and exit criteria. | Complete |
| `runtime-hook-map.*` | Phase 5 | Map commands, events, packets, timers, gumps, regions, speech, movement, login/logout, and world hooks. | Complete |
| `serialization-register.*` | Phase 6 | Record save format, versioning, write/read order, and move/rename risk for serialized types. | NotStarted |
| `documentation-truth-table.*` | Phase 7 | Classify docs as canonical, alias, stale, partial, or verified with source traces. | NotStarted |
| `dependency-graph.*` | Phase 8 | Record source-verified hard, soft, speculative, docs-only, and conflict edges between systems. | NotStarted |
| `synergy-conflict-matrix.*` | Phase 9 | Classify gameplay, balance, maintenance, and documentation relationships between systems. | NotStarted |
| `risk-track-findings.*` | Phase 10 | Store findings, non-issues, accepted risks, and follow-up work from specialized review tracks. | NotStarted |
| `comment-target-register.*` | Phase 11 | Track approved and rejected inline source-comment targets. | NotStarted |
| `reorganization-design.*` | Phase 12 | Propose target layout, moves, keep-in-place decisions, project updates, docs updates, save risk, and rollback plans. | NotStarted |
| `repair-backlog.*` | Phase 13 | Convert findings into prioritized actionable repair items. | NotStarted |
| `accepted-risk-register.*` | Phase 13 | Record explicitly accepted risks with evidence and rationale. | NotStarted |
| `verification-matrix.*` | Phase 13 or 14 | Map change categories and backlog items to required verification. | NotStarted |

## Initial State

The Phase 0 baseline output, Phase 1 reproducible inventory outputs, Phase 2 project truth outputs, Phase 3 runtime inventory outputs, Phase 4 system cards, and Phase 5 runtime hook map outputs have been generated. Continue through the phase gates in order, starting with Phase 6 serialization and save-compatibility review after the Phase 5 batch is committed.
