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
| `project-truth-register.*` | Phase 2 | Compare real source files with `Scripts.csproj` compile entries. | NotStarted |
| `cross-tree-runtime-inventory.*` | Phase 3 | Assign runtime role, owner, entry points, hooks, serialization, gumps, and config usage for source files. | NotStarted |
| `system-cards/` | Phase 4 | Store one canonical engineering card per major system. | NotStarted |
| `runtime-hook-map.*` | Phase 5 | Map commands, events, packets, timers, gumps, regions, speech, movement, login/logout, and world hooks. | NotStarted |
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

The Phase 0 baseline output has been generated. Continue through the phase gates in order, starting with Phase 1 reproducible inventory scripts after the Phase 0 batch is committed.
