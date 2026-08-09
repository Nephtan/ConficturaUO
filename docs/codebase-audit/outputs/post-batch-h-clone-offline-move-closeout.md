# POST-BATCH-H Clone Offline Player Characters Move Closeout

Generated: 2026-06-13T23:38:41.8781692-05:00

## Summary

POST-BATCH-H-07A executed the approved Phase 12 Clone Offline Player Characters containment move from Data/Scripts/Custom/CloneOfflinePlayerCharacters to Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters. The batch moved all seven runtime-visible .cs files, preserved the Server.Custom.Confictura.CloneOfflinePlayerCharacters namespace and all serialized type names, updated seven Scripts.csproj compile references, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. No package-local path constant, XML/config file, command name, hook registration, AI selection, serialization version, or gameplay rule changed.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06815` |
| Original path | `Data/Scripts/Custom/CloneOfflinePlayerCharacters` |
| Target path | `Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters` |
| Workspace files moved | `7` |
| Runtime-visible files moved | `7` |
| Namespace/type/API changes | `None` |
| Serialized rows | `4` |
| Serialization evidence | `BackpackClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; CharacterClone:v0:WriteVersionOnly:CountMismatch:Writes=1;Reads=2; EtherealMountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; MountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1` |
| Runtime hook rows | `8` (Command=1; Event=2; Initialize=2; Login=1; Logout=1; Timer=1) |
| Runtime script inventory target rows | `7` |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 7 Clone Offline target rows, and 0 old-path rows.
- runtime-hook-map.csv: regenerated with 8 Clone Offline hook rows (Command=1; Event=2; Initialize=2; Login=1; Logout=1; Timer=1).
- serialization-register.csv: regenerated with 4 Clone Offline rows preserving namespace/type names and version 0 WriteVersionOnly handling (BackpackClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; CharacterClone:v0:WriteVersionOnly:CountMismatch:Writes=1;Reads=2; EtherealMountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; MountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1).
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors
- ConficturaUO.sln Debug/Any CPU: Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings and no errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters back to Data/Scripts/Custom/CloneOfflinePlayerCharacters, restore the previous Scripts.csproj compile paths, rerun project truth and path-sensitive audit generators, and revert Clone Offline documentation/source-trace paths.
