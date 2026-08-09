# Phase 6 Serialization And Save Compatibility Summary

Generated: 2026-06-13T23:48:46.9008044-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Serialization marker scans | Present: `phase-01-serialization-marker-inventory.csv` with 5342 file rows |
| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` with 6700 rows |
| System Cards | Present: `system-owner-map.csv` with 2629 owner rows |
| Project Truth Register | Present: `project-truth-register.csv` with 13162 rows |
| Root serialization standards | Present in root `AGENTS.md` instructions |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `serialization-register.csv` | 9158 | Canonical serializer map with class, save version, ordered writes/reads, version handling, field alignment, and move risk. |
| `phase-06-serialization-register.csv` | 9158 | Phase-scoped copy of the serializer register. |
| `phase-06-high-risk-serializer-list.csv` | 3953 | Rows requiring manual review because of high-risk systems, project truth, versioning, pairing, alignment, or move risk. |
| `phase-06-move-rename-risk-list.csv` | 9158 | Serialized types with namespace/type or file-move risk classification. |
| `phase-06-serializer-comment-target-list.csv` | 3105 | Candidate Phase 11 source-comment targets for fragile save behavior. |
| `phase-06-save-compatibility-repair-backlog.csv` | 1616 | Concrete serializer follow-up items created from machine-detected risks. |

## Version Handling Counts

| Version Handling | Count |
| --- | ---: |
| IfVersionGates | 157 |
| NoVersionFound | 743 |
| ReadVersionOnly | 16 |
| SingleVersionOnly | 7179 |
| SuspiciousOrder | 258 |
| Switch | 539 |
| SwitchGotoCase | 172 |
| WriteVersionOnly | 94 |

## Field Alignment Counts

| Field Alignment | Count |
| --- | ---: |
| AlignedByCountAndKnownTypes | 8270 |
| CountMatchNeedsTypeReview:UnknownWrites=1 | 278 |
| CountMatchNeedsTypeReview:UnknownWrites=2 | 171 |
| CountMismatch:Writes=0;Reads=1 | 86 |
| CountMatchNeedsTypeReview:UnknownWrites=4 | 51 |
| CountMatchNeedsTypeReview:UnknownWrites=3 | 34 |
| CountMatchNeedsTypeReview:UnknownWrites=5 | 22 |
| CountMatchNeedsTypeReview:UnknownWrites=6 | 15 |
| CountMismatch:Writes=4;Reads=3 | 13 |
| CountMatchNeedsTypeReview:UnknownWrites=8 | 11 |
| CountMismatch:Writes=1;Reads=2 | 10 |
| CountMismatch:Writes=1;Reads=0 | 10 |

## Move/Rename Risk Counts

| Move/Rename Risk | Count |
| --- | ---: |
| DoNotMove | 1 |
| NamespaceOrTypeRenameDanger | 6892 |
| UnknownUntilBuildTruthRepaired | 23 |
| UnknownUntilSaveTest | 2242 |

## Exit Criteria

- Serialized marker files were deepened into per-class serializer rows with ordered write/read maps where method scopes could be extracted.
- High-risk, asymmetric, unversioned, project-truth, and move/rename-sensitive rows are explicit in focused registers and backlog outputs.
- Source comments are not added in Phase 6; candidate comments are written to the Phase 6 comment target list for Phase 11 review.
- Reorganization remains blocked for serialized namespace/type renames unless a migration plan and save-load verification are approved.
