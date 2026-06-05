# Phase 8 Dependency Graph Summary

Generated: 2026-06-05T17:53:18.5614483-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| System Cards | Present: `phase-04-system-card-index.csv` with 27 rows |
| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` with 6700 rows |
| Runtime Hook Map | Present: `runtime-hook-map.csv` with 6604 rows |
| Serialization Register | Present: `serialization-register.csv` with 9158 rows |
| Documentation Truth Table | Present: `documentation-truth-table.csv` with 122 rows |
| Project Truth Register | Present: `project-truth-register.csv` with 13152 rows |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `dependency-graph.csv` | 30063 | Canonical dependency graph. |
| `phase-08-dependency-graph.csv` | 30063 | Phase-scoped dependency graph. |
| `phase-08-hard-dependency-list.csv` | 29933 | Hard source, runtime, serialization, project, and config dependencies. |
| `phase-08-soft-dependency-list.csv` | 130 | Soft or speculative documentation/config relationships. |
| `phase-08-conflict-edge-list.csv` | 1248 | Conflict, mismatch, duplicate, or manual-review edges. |
| `phase-08-standalone-proof-list.csv` | 27 | Negative-evidence standalone proof table for system-card systems. |

## Edge Type Counts

| Edge Type | Count |
| --- | ---: |
| DirectReference | 1508 |
| DocsOnly | 114 |
| DocsSourceTrace | 9 |
| DocumentationConflict | 2 |
| GlobalHook | 227 |
| PacketHandler | 17 |
| ProjectInclude | 159 |
| ProjectTruthConflict | 32 |
| RuntimeHook | 6360 |
| Serialization | 20303 |
| SerializationConflict | 1214 |
| XMLConfig | 118 |

## Strength Counts

| Strength | Count |
| --- | ---: |
| Hard | 29933 |
| Soft | 14 |
| Speculative | 116 |

## Exit Criteria

- Generated dependency edges cover direct source references, runtime hooks, serialization, project includes, XML/config references, and docs-only relationships.
- Standalone proof rows record negative evidence counts; generated evidence marks systems as `NotStandalone` rather than assuming independence.
- Systems without generated incoming or outgoing edges: None.
- Docs-only and speculative edges are separated from hard source/runtime/save/project edges.
