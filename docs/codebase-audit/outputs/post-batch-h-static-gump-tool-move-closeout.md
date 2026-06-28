# POST-BATCH-H Static Gump Tool Move Closeout

Generated: 2026-06-13T22:44:53.5818869-05:00

## Summary

`POST-BATCH-H-03A` executed the approved Phase 12 Static Gump Tool move from `Data/Scripts/Custom/OzThothStaticGump` to `Data/Scripts/Custom/StaffTools/StaticGumpTool`. The batch moved the complete 23-file workspace, including 21 runtime-visible scripts, preserved all namespaces/classes/APIs, updated 21 `Scripts.csproj` compile include rows plus one content include row, updated current source-trace docs and package-local placement guidance, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The serialization register reports zero Static Gump Tool serialized rows, so this move did not introduce a save-format migration.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06811` |
| Original path | `Data/Scripts/Custom/OzThothStaticGump` |
| Target path | `Data/Scripts/Custom/StaffTools/StaticGumpTool` |
| Workspace files moved | `23` |
| Runtime-visible files moved | `21` |
| Namespace/type/API changes | `None` |
| Serialized Static Gump Tool rows | `0` |
| Runtime hook rows | `198` |
| Runtime script inventory target rows | `21` |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 21 Static Gump Tool target rows, and 0 old-path rows.
- runtime-hook-map.csv: regenerated with 198 Static Gump Tool hook rows (23 Command, 153 Gump, 22 Initialize).
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files
- ConficturaUO.sln Debug/Any CPU: Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\StaffTools\StaticGumpTool scripts; 53 warnings and 0 errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move `Data/Scripts/Custom/StaffTools/StaticGumpTool` back to `Data/Scripts/Custom/OzThothStaticGump`, restore the previous `Scripts.csproj` include/content paths, rerun project truth and path-sensitive audit generators, and revert Static Gump Tool documentation/source-trace paths.
