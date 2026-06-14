# Phase 4 System Cards Summary

Generated: 2026-06-13T23:31:38.2153555-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` |
| Project Truth Register | Present: `project-truth-register.csv` |
| Runtime Hook Map draft | Marker-derived draft from `phase-01-runtime-marker-inventory.csv` |
| Serialization Register draft | Marker-derived draft from `phase-01-serialization-marker-inventory.csv` |
| Existing wiki pages | Present: `phase-01-documentation-inventory.csv` |
| `SystemAudit.md` | Present under `docs/wiki/SystemAudit.md` |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| system-cards/ | 27 | One generated card per seeded high-risk system. |
| phase-04-system-card-index.csv | 27 | Card metadata and verification status. |
| phase-04-system-owner-map.csv | 2629 | Matched file-to-system ownership evidence. |
| system-owner-map.csv | 2629 | Canonical owner map copy. |
| phase-04-system-card-backlog.csv | 27 | Follow-up work for partial or blocked cards. |
| phase-04-high-risk-system-priority-list.csv | 27 | Review order for seeded high-risk systems. |

## Verification Status Counts

| Status | Count |
| --- | ---: |
| NeedsRuntimeReview | 3 |
| NeedsSerializationReview | 23 |
| PartiallyVerified | 1 |

## Exit Criteria

- Each seeded high-risk system has a generated system card.
- Cards list matched source files, docs, commands, runtime hooks, serialized marker files, config references, dependencies, and follow-up work.
- Generated cards are marked `PartiallyVerified`, `NeedsRuntimeReview`, `NeedsSerializationReview`, or `Blocked`; no card claims full verification from marker data alone.
- Full dependency and synergy claims are deferred to Phases 8 and 9.
