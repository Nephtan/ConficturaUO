# Phase 13 Repair Backlog Summary

Generated: 2026-06-05T18:41:38.2000224-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Project Truth Register | Present: `project-truth-register.csv` with 13152 rows |
| System Cards | Present: `phase-04-system-card-index.csv` with 27 rows |
| Runtime Hook Map | Present: `runtime-hook-map.csv` with 6604 rows |
| Serialization Register | Present: `serialization-register.csv` with 9158 rows |
| Documentation Truth Table | Present: `documentation-truth-table.csv` with 122 rows |
| Dependency Graph | Present: `dependency-graph.csv` with 30063 rows |
| Synergy and Conflict Matrix | Present: `synergy-conflict-matrix.csv` with 351 rows |
| Risk-Specific Findings | Present: `risk-track-findings.csv` with 6801 rows |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `repair-backlog.csv` | 6815 | Canonical prioritized repair backlog. |
| `phase-13-repair-backlog.csv` | 6815 | Phase-scoped backlog copy. |
| `accepted-risk-register.csv` | 3 | Canonical accepted-risk register. |
| `deferred-work-register.csv` | 21 | Canonical deferred work register. |
| `phase-13-batch-plan.csv` | 7 | Small-batch implementation plan. |
| `verification-matrix.csv` | 10 | Canonical category verification matrix. |

## Priority Counts

| Priority | Count |
| --- | ---: |
| P0 | 375 |
| P1 | 4699 |
| P2 | 1429 |
| P3 | 298 |
| P4 | 14 |

## Status Counts

| Status | Count |
| --- | ---: |
| Deferred | 14 |
| Open | 200 |
| Ready | 6601 |

## Category Counts

| Category | Count |
| --- | ---: |
| Command access | 499 |
| Documentation contradictions | 158 |
| Economy and reward loops | 26 |
| Folder and namespace cleanup | 14 |
| Gump guards | 938 |
| Legacy compatibility | 659 |
| Packet handlers | 17 |
| PlayerMobile coupling | 394 |
| Pooled enumerables | 408 |
| Project include drift | 61 |
| Region and map assumptions | 112 |
| Runtime hooks | 1548 |
| Save compatibility | 1625 |
| Staff tooling | 124 |
| XML/config schemas | 232 |

## Exit Criteria

- Every Phase 10 finding has a repair backlog item.
- Phase 12 move proposals are deferred organization backlog items with verification and rollback requirements.
- Accepted risks and deferred work are explicit durable registers.
- Batch plan and verification matrix let future repair batches proceed without rediscovery.
