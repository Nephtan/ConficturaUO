# Phase 3 Cross-Tree Runtime Inventory Summary

Generated: 2026-06-13T23:46:43.8276437-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Phase 1 file inventory | Present: `phase-01-source-files.csv` |
| Phase 2 project truth register | Present: `project-truth-register.csv` |
| Runtime marker scans | Present: `phase-01-runtime-marker-inventory.csv` |
| Serialization marker scans | Present: `phase-01-serialization-marker-inventory.csv` |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| cross-tree-runtime-inventory.csv | 6700 | Canonical runtime role and owner inventory. |
| phase-03-cross-tree-runtime-inventory.csv | 6700 | Phase-scoped copy of the runtime inventory. |
| phase-03-root-role-summary.csv | 8 | Root-level marker and role counts. |
| phase-03-unknown-owner-list.csv | 146 | Files with Unknown owner or role requiring follow-up. |
| phase-03-high-risk-root-summary.csv | 8 | Machine-readable high-risk root summary. |
| phase-03-high-risk-root-summary.md | 8 | Human-readable high-risk root summary. |

## Root Summary

| Root | Files | Init | Commands | Events | Packets | Serialization | Gumps | Unknown Roles | Unknown Owners |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Custom | 1134 | 96 | 65 | 11 | 2 | 870 | 245 | 28 | 0 |
| Items | 3029 | 40 | 14 | 13 | 5 | 2927 | 197 | 0 | 0 |
| Magic | 574 | 13 | 11 | 4 | 0 | 214 | 36 | 0 | 0 |
| Mobiles | 1102 | 2 | 0 | 2 | 0 | 1092 | 100 | 0 | 0 |
| Quests | 117 | 1 | 1 | 0 | 0 | 108 | 29 | 0 | 0 |
| ServerCore | 119 | 1 | 0 | 7 | 0 | 21 | 8 | 0 | 0 |
| System | 469 | 168 | 96 | 39 | 5 | 36 | 164 | 118 | 0 |
| Trades | 156 | 5 | 1 | 2 | 0 | 74 | 41 | 0 | 0 |

## Exit Criteria

- Every audited `.cs` file has a provisional primary role.
- Every file has a provisional system owner or an explicit `Unknown` row with follow-up.
- Each root has marker counts for initialization, commands, events, packets, serialization, and gumps.
- High-risk root follow-up notes are recorded for later system cards and risk tracks.
