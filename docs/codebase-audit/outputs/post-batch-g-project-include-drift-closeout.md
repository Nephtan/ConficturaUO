# POST-BATCH-G Project Include Drift Closeout

Generated: 2026-06-13T20:17:10.5956233-05:00

## Summary

`POST-BATCH-G` reviewed all 61 historical `Project include drift` rows from `repair-backlog.csv` and reconciled them to the already completed `ScriptsProjectTruth` and `IDEProjectHygiene` repairs. Current project truth has no remaining missing `Scripts.csproj` targets, no unlisted active script sources, and no project cleanup backlog groups.

The batch is audit/project-hygiene bookkeeping only. It does not edit `Data/Scripts/Scripts.csproj`, runtime source, XML/config files, namespaces, serialized types, save versions, runtime file locations, or gameplay behavior, and it does not start `POST-BATCH-H` reorganization.

## Scope Counts

| Priority | Count |
| --- | ---: |
| `P0` | 29 |
| `P1` | 31 |
| `P2` | 1 |

## Decision Counts

| Decision | Count |
| --- | ---: |
| `FixedProjectTruthDrift` | 61 |

## Active Overlay Counts

| Active status | Count |
| --- | ---: |
| `Fixed` | 61 |

## Project Truth Result

| Evidence | Count |
| --- | ---: |
| Scripts.csproj compile includes | 6581 |
| Script source files | 6581 |
| `missing-compile-targets.csv` rows | 0 |
| `unincluded-source-files.csv` rows | 0 |
| `project-cleanup-backlog.csv` rows | 0 |
| `phase-02-missing-compile-targets-classified.csv` rows | 0 |
| `phase-02-unincluded-source-classified.csv` rows | 0 |
| `phase-02-project-cleanup-backlog.csv` rows | 0 |

## Verification

- `git status --short` was clean before the batch.
- `rg --files -g AGENTS.md` was rerun; applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.
- `New-ProjectTruthRegister.ps1` regenerated Phase 2 project truth and reported 6,581 compile includes, 6,581 script source files, 0 missing compile targets, 0 unincluded source files, and 0 project cleanup backlog groups.
- The maintained `ConficturaUO.sln` Debug/`Any CPU` build previously passed with warnings after the `IDEProjectHygiene` repair. This closeout does not claim live runtime script compile proof.
- Direct `Data/Scripts/Scripts.csproj` Debug/`AnyCPU` still has the known standalone project-reference limitation because it passes `Debug|AnyCPU` to `Server.csproj`, which has no matching standalone output path outside solution mapping.
- Source/runtime/config/project verification was not run because this batch made no `.cs`, `.xml`, `.cfg`, `.csproj`, `Data/`, or runtime behavior changes.
- `git diff --check` passed before staging and commit with no whitespace errors.
- Final invariant checks found 61 POST-BATCH-G review rows, 61 POST-BATCH-G active overlay rows, 61 `FixedProjectTruthDrift` decisions, 61 active `Fixed` rows, and no blank decisions, verification fields, or overlay actions.
