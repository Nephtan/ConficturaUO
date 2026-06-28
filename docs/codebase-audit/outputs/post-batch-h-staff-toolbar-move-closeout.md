# POST-BATCH-H Staff Toolbar Move Closeout

Generated: 2026-06-13T23:10:16.1908299-05:00

## Summary

POST-BATCH-H-05A executed the approved Phase 12 Staff Toolbar containment move from Data/Scripts/Custom/Staff Toolbar [2.0] to Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]. The batch moved the single-script workspace, preserved the Joeku namespace and serialized ToolbarInfos type, updated one Scripts.csproj compile include, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved script remains under `Data/Scripts`, so live runtime script compile visibility is preserved. The access/workflow gate was source-reviewed: `[Toolbar` remains Counselor-gated, the login hook still sends the toolbar only to Counselor-or-higher mobiles, and no gump command execution behavior changed.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06812` |
| Original path | `Data/Scripts/Custom/Staff Toolbar [2.0]` |
| Target path | `Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]` |
| Workspace files moved | `1` |
| Runtime-visible files moved | `1` |
| Namespace/type/API changes | `None` |
| Serialized row | `Joeku.ToolbarInfos` / `ToolbarInfos` |
| Serialization evidence | `ReadVersionOnly`, `CountMismatch:Writes=15;Reads=14` |
| Runtime hook rows | `26` (Event=1; Gump=23; Initialize=1; Login=1) |
| Runtime script inventory target rows | `1` |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 1 Staff Toolbar target row, and 0 old-path rows.
- runtime-hook-map.csv: regenerated with 26 Staff Toolbar hook rows (Event=1; Gump=23; Initialize=1; Login=1).
- serialization-register.csv: regenerated with one Staff Toolbar serialized row, Joeku.ToolbarInfos, preserving ReadVersionOnly and CountMismatch:Writes=15;Reads=14.
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed after sandbox SDK lookup denial rerun outside sandbox; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings; 53 warnings and 0 errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0] back to Data/Scripts/Custom/Staff Toolbar [2.0], restore the previous Scripts.csproj include path, rerun project truth and path-sensitive audit generators, and revert Staff Toolbar documentation/source-trace paths.
