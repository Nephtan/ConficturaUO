# Phase 2 Build And Project Truth Summary

Generated: 2026-06-13T19:14:45.9068477-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Phase 1 project include parser | Present: `docs/codebase-audit/tools/Invoke-CodebaseAuditInventory.ps1` and `phase-01-project-includes.csv` |
| `ConficturaUO.sln` | Present and solution configurations parsed |
| `Data/Scripts/Scripts.csproj` | Present; compile includes imported from Phase 1 parser output |
| `Data/System/Source/Server.csproj` | Present; referenced by solution and Scripts project |
| Full source file inventory | Present: `phase-01-source-files.csv` |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| phase-02-project-truth-register.csv | 13162 | One row per Scripts project include and one row per script source file. |
| phase-02-missing-compile-targets-classified.csv | 0 | Missing compile targets with drift classifications and repair verification. |
| phase-02-unincluded-source-classified.csv | 0 | Real script source files absent from Scripts.csproj, classified by likely build impact. |
| phase-02-intentional-noncompiled-source.csv | 0 | Sources currently classified as generated, backup, old, or intentionally not compiled. |
| phase-02-project-cleanup-backlog.csv | 0 | Grouped project truth repair backlog. |
| phase-02-solution-configurations.csv | 4 | Solution configuration to project configuration mapping. |

## Counts

| Scripts project compile includes | 6581 |
| Script source files on disk | 6581 |
| Missing compile targets | 0 |
| Unincluded script sources | 0 |
| Backlog groups | 0 |

## Build Verification

Build verification is recorded separately in `phase-02-build-verification.md`. Post-audit `IDEProjectHygiene` verification now records the maintained `ConficturaUO.sln` Debug/`Any CPU` build passing with warnings after `Scripts.csproj` reference and portable PDB repairs.

## Exit Criteria

- Project truth table explains every project/source mismatch with `DriftClass`, `LikelySystem`, and `Action`.
- Remaining discrepancies are grouped into `phase-02-project-cleanup-backlog.csv`.
- Solution configuration mapping identifies which solution builds cover both projects.
- Post-audit project repair has reconciled `ScriptsProjectTruth`; post-audit Visual Studio reference and Debug PDB repairs are recorded as `IDEProjectHygiene`, not live runtime script compile proof.
