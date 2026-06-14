# Phase 8 Dependency Graph Summary

Generated: 2026-06-13T22:51:38.5275753-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| System Cards | Present: `phase-04-system-card-index.csv` with 27 rows |
| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` with 6700 rows |
| Runtime Hook Map | Present: `runtime-hook-map.csv` with 6605 rows |
| Serialization Register | Present: `serialization-register.csv` with 9158 rows |
| Documentation Truth Table | Present: `documentation-truth-table.csv` with 122 rows |
| Project Truth Register | Present: `project-truth-register.csv` with 13162 rows |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `dependency-graph.csv` | 30212 | Canonical dependency graph. |
| `phase-08-dependency-graph.csv` | 30212 | Phase-scoped dependency graph. |
| `phase-08-hard-dependency-list.csv` | 29901 | Hard source, runtime, serialization, project, and config dependencies. |
| `phase-08-soft-dependency-list.csv` | 311 | Soft or speculative documentation/config relationships. |
| `phase-08-conflict-edge-list.csv` | 1214 | Conflict, mismatch, duplicate, or manual-review edges. |
| `phase-08-standalone-proof-list.csv` | 27 | Negative-evidence standalone proof table for system-card systems. |

## Edge Type Counts

| Edge Type | Count |
| --- | ---: |
| DirectReference | 1508 |
| DocsOnly | 114 |
| DocsSourceTrace | 190 |
| DocumentationConflict | 2 |
| GlobalHook | 227 |
| PacketHandler | 17 |
| ProjectInclude | 160 |
| RuntimeHook | 6361 |
| Serialization | 20303 |
| SerializationConflict | 1212 |
| XMLConfig | 118 |

## Strength Counts

| Strength | Count |
| --- | ---: |
| Hard | 29901 |
| Soft | 195 |
| Speculative | 116 |

## Exit Criteria

- Generated dependency edges cover direct source references, runtime hooks, serialization, project includes, XML/config references, and docs-only relationships.
- Standalone proof rows record negative evidence counts; generated evidence marks systems as `NotStandalone` rather than assuming independence.
- Systems without generated incoming or outgoing edges: None.
- Docs-only and speculative edges are separated from hard source/runtime/save/project edges.
