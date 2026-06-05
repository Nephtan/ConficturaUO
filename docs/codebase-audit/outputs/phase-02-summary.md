# Phase 2 Build And Project Truth Summary

Generated: 2026-06-05T16:49:12.9632111-05:00

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
| phase-02-project-truth-register.csv | 13152 | One row per Scripts project include and one row per script source file. |
| phase-02-missing-compile-targets-classified.csv | 82 | Missing compile targets with drift classifications and repair verification. |
| phase-02-unincluded-source-classified.csv | 92 | Real script source files absent from Scripts.csproj, classified by likely build impact. |
| phase-02-intentional-noncompiled-source.csv | 0 | Sources currently classified as generated, backup, old, or intentionally not compiled. |
| phase-02-project-cleanup-backlog.csv | 61 | Grouped project truth repair backlog. |
| phase-02-solution-configurations.csv | 4 | Solution configuration to project configuration mapping. |

## Counts

| Scripts project compile includes | 6571 |
| Script source files on disk | 6581 |
| Missing compile targets | 82 |
| Unincluded script sources | 92 |
| Backlog groups | 61 |

## Build Verification

Build verification is recorded separately in `phase-02-build-verification.md`. The direct `Scripts.csproj` build failed before compilation because `Platform=AnyCPU` flowed to `Server.csproj`; the maintained solution build reached script compilation and failed on the 82 missing compile targets recorded by this phase.

## Exit Criteria

- Project truth table explains every project/source mismatch with `DriftClass`, `LikelySystem`, and `Action`.
- Remaining discrepancies are grouped into `phase-02-project-cleanup-backlog.csv`.
- Solution configuration mapping identifies which solution builds cover both projects.
- Project repair is deferred to focused batches; no `Scripts.csproj` changes are made by Phase 2.
