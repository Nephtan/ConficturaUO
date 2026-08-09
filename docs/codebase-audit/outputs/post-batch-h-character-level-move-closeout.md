# POST-BATCH-H Character Level Move Closeout

Generated: 2026-06-13T20:49:49.0363375-05:00

## Summary

`POST-BATCH-H-01A` executed the approved Phase 12 Character Level pilot move from `Data/Scripts/Custom/CharacterLevel` to `Data/Scripts/Custom/Progression/CharacterLevel`. The batch moved exactly two runtime-visible script files, preserved namespaces/classes/commands/APIs, updated two `Scripts.csproj` include rows, updated current documentation source-trace paths, and regenerated path-sensitive audit outputs.

The moved files remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The serialization register still reports zero Character Level serialized rows, so this move did not introduce a save-format migration.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06802` |
| Original path | `Data/Scripts/Custom/CharacterLevel` |
| Target path | `Data/Scripts/Custom/Progression/CharacterLevel` |
| Files moved | `CharacterLevelCommands.cs`, `CharacterLevelService.cs` |
| Namespace/type/command changes | None |
| Serialized Character Level rows | `0` |
| Runtime script inventory target rows | `2` |
| Moved hook rows | `3` |

## Decision Counts

| Decision | Count |
| --- | ---: |
| `ExecutedMoveNoNamespaceChange` | 1 |

## Active Overlay Counts

| Active status | Count |
| --- | ---: |
| `Fixed` | 1 |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 2 Character Level target rows, and 0 old-path rows.
- `Invoke-CodebaseAuditInventory.ps1`: passed after adding a zero-row export guard to the Phase 1 inventory helper.
- Regenerated cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed with existing warnings; Scripts built Data/Scripts/ClassLibrary.dll and generated script-project artifacts were removed before staging
- Server.csproj Debug/x86: Passed; Server built D:/ConficturaUO/ConficturaServer.exe and tracked root build artifacts were restored before staging
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move `Data/Scripts/Custom/Progression/CharacterLevel` back to `Data/Scripts/Custom/CharacterLevel`, restore the previous two `Scripts.csproj` include paths, rerun project truth and the path-sensitive audit generators, and revert Character Level documentation source traces.
