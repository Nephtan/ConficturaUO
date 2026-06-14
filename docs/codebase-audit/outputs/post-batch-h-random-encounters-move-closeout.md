# POST-BATCH-H Random Encounters Move Closeout

Generated: 2026-06-13T23:22:41.7451596-05:00

## Summary

POST-BATCH-H-06A executed the approved Phase 12 Random Encounters containment move from Data/Scripts/Custom/RandomEncounters to Data/Scripts/Custom/PvE/RandomEncounters. The batch moved the complete 17-file package, preserved namespaces and serialized type names, updated 17 Scripts.csproj compile/content/none path references, updated the package-local XML load path constant, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. `RandomEncounters.xml` moved with the package and its contents were not rewritten; `EncounterEngine.cs` now points at the moved checked-in XML path so runtime loading remains aligned with the new file location.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06804` |
| Original path | `Data/Scripts/Custom/RandomEncounters` |
| Target path | `Data/Scripts/Custom/PvE/RandomEncounters` |
| Workspace files moved | `17` |
| Runtime-visible files moved | `11` |
| Namespace/type/API changes | `None` |
| Serialized rows | `2` (`Server.Engines.XmlSpawner2.XmlDateCount` plus `Import.cs` review row) |
| XmlDateCount serialization evidence | `SingleVersionOnly`, `CountMatchNeedsTypeReview:UnknownWrites=1` |
| Runtime hook rows | `8` (Command=1; Initialize=3; Timer=4) |
| Runtime script inventory target rows | `11` |

## Project Truth Result

| Evidence | Count |
| --- | ---: |
| Scripts.csproj compile includes | 6581 |
| Script source files | 6581 |
| Missing compile targets | 0 |
| Unincluded source files | 0 |
| Project cleanup backlog rows | 0 |

## Verification

- New-ProjectTruthRegister.ps1: passed with 6581 includes, 6581 sources, 0 missing compile targets, 0 unincluded sources, and 0 cleanup backlog rows.
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 11 Random Encounters target rows, and 0 old-path rows.
- runtime-hook-map.csv: regenerated with 8 Random Encounters hook rows (Command=1; Initialize=3; Timer=4).
- serialization-register.csv: regenerated with two Random Encounters rows: Server.Engines.XmlSpawner2.XmlDateCount version 0, preserving SingleVersionOnly and CountMatchNeedsTypeReview:UnknownWrites=1, plus the Import.cs NoVersionFound review row.
- EncounterEngine.cs XML path: updated to `./Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml` so the moved package continues loading its checked-in config file.
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings; 53 warnings and 0 errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move Data/Scripts/Custom/PvE/RandomEncounters back to Data/Scripts/Custom/RandomEncounters, restore the previous Scripts.csproj compile/content/none paths, rerun project truth and path-sensitive audit generators, and revert Random Encounters documentation/source-trace paths.
