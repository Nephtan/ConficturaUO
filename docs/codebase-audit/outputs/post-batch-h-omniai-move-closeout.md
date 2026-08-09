# POST-BATCH-H OmniAI Move Closeout

Generated: 2026-06-13T22:56:37.1593591-05:00

## Summary

`POST-BATCH-H-04A` executed the approved Phase 12 OmniAI containment move from `Data/Scripts/Custom/OmniAI` to `Data/Scripts/Custom/ThirdParty/OmniAI`. The batch moved the complete eight-script workspace, preserved all namespaces/classes/APIs, updated eight `Scripts.csproj` compile include rows, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The one serialized type, `Server.Mobiles.AITester`, remains in the same namespace/type with version `0`; this move did not introduce a save-format migration.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06810` |
| Original path | `Data/Scripts/Custom/OmniAI` |
| Target path | `Data/Scripts/Custom/ThirdParty/OmniAI` |
| Workspace files moved | `8` |
| Runtime-visible files moved | `8` |
| Namespace/type/API changes | `None` |
| Serialized OmniAI rows | `1` |
| Serialized type | `Server.Mobiles.AITester` version `0` |
| Runtime hook rows | `2` |
| Runtime script inventory target rows | `8` |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 8 OmniAI target rows, and 0 old-path rows.
- serialization-register.csv: regenerated with one OmniAI serialized type, Server.Mobiles.AITester, version 0, with AlignedByCountAndKnownTypes.
- runtime-hook-map.csv: regenerated with 2 OmniAI hook rows (1 Event, 1 Timer).
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\ThirdParty\OmniAI scripts; 53 warnings and 0 errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move `Data/Scripts/Custom/ThirdParty/OmniAI` back to `Data/Scripts/Custom/OmniAI`, restore the previous `Scripts.csproj` include paths, rerun project truth and path-sensitive audit generators, and revert OmniAI documentation/source-trace paths.
