# Phase 13: Repair Backlog

## Purpose

Phase 13 converts audit findings into executable work. A finding is not useful
until it has priority, evidence, owner system, affected files, a recommended
fix, and verification steps.

## Required Inputs

- Project Truth Register.
- System Cards.
- Runtime Hook Map.
- Serialization Register.
- Documentation Truth Table.
- Dependency Graph.
- Synergy and Conflict Matrix.
- Risk-specific findings.

## Required Outputs

- Prioritized repair backlog.
- Accepted risk register.
- Deferred work register.
- Batch plan.
- Verification matrix.

## Backlog Item Template

| Field | Meaning |
| --- | --- |
| `Id` | Stable ID. |
| `Priority` | P0-P4. |
| `Status` | Open, Ready, In Progress, Blocked, Fixed, Committed, Intentional. |
| `System` | Owning system. |
| `Category` | Build, save, hook, packet, gump, docs, balance, legacy, organization. |
| `Files` | Affected paths. |
| `Evidence` | Source, command, or doc evidence. |
| `Risk` | Impact if not fixed. |
| `RecommendedFix` | Specific action. |
| `Verification` | Required check. |
| `Notes` | Additional context. |

## Subphase 13.1: Priority Rules

Use these priorities:

- `P0`: data loss, world load failure, build-blocking, security/network risk.
- `P1`: runtime crash, save corruption risk, global hook breakage, major player
  impact.
- `P2`: incorrect gameplay, staff tool hazard, serious documentation mismatch.
- `P3`: maintainability issue, minor player confusion, low-risk stale docs.
- `P4`: cleanup, naming, organization, future improvement.

Completion gate:

- Priority reflects impact, not convenience.

## Subphase 13.2: Category Buckets

Create buckets:

1. Project include drift.
2. Active source not compiled.
3. Missing compile targets.
4. Save compatibility.
5. Runtime hooks.
6. Packet handlers.
7. Gump guards.
8. Pooled enumerables.
9. Documentation contradictions.
10. Stale aliases and duplicate docs.
11. Known `Needs Rework` audit items.
12. Legacy code with active hooks.
13. Folder and namespace cleanup.
14. Inline comments.

Completion gate:

- Findings are easy to filter by work type.

## Subphase 13.3: Ready Criteria

A backlog item is `Ready` only when it has:

- exact files;
- exact evidence;
- proposed fix;
- risk explanation;
- verification command;
- no unresolved ownership question.

Completion gate:

- Ready items can be picked up without rediscovery.

## Subphase 13.4: Blocked Criteria

Use `Blocked` only when:

- human design decision is needed;
- save migration policy is needed;
- build tools are unavailable;
- source evidence conflicts and cannot be resolved locally;
- required test data is unavailable.

Completion gate:

- Blocked items name the missing decision or external dependency.

## Subphase 13.5: Batch Planning

Group work into small batches:

- docs-only batch;
- project file batch;
- serialization-safe code batch;
- gump guard batch;
- pooled enumerable batch;
- system-specific batch.

Completion gate:

- Each batch can be committed independently.

## Subphase 13.6: Verification Matrix

For each category, define verification:

| Category | Verification |
| --- | --- |
| Docs | link/source-trace checks |
| Project includes | project truth parser and build |
| Serialization | serializer register diff and build |
| Hooks | runtime hook scan and build |
| Gumps | response map review and build |
| Pooled enumerable | search and targeted review |
| Reorg | project truth, docs links, build |

Completion gate:

- Every backlog item has a matching verification method.

## Review Checklist

- Every finding is tracked.
- Priorities are justified.
- Ready items are actionable.
- Blocked items name the blocker.
- Accepted risks are explicit.
- Batches are small.

## Exit Criteria

Phase 13 is complete when audit findings are no longer scattered across notes,
docs, and chat: they are organized into a stable, prioritized backlog that can
drive implementation.
