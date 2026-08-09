# POST-BATCH-H Monster Nests Move Closeout

Generated: 2026-06-13T23:53:11.6554902-05:00

## Summary

POST-BATCH-H-09A executed the approved Phase 12 Monster Nests containment move from Data/Scripts/Custom/MonsterNest to Data/Scripts/Custom/PvE/MonsterNests. The batch moved all six runtime-visible .cs files, preserved namespaces and serialized type names, updated six Scripts.csproj compile references, updated current source-trace docs, and regenerated path-sensitive audit outputs.

The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. No XML/config file, command name, hook behavior, timer cadence, serialization version, spawn table, reward rule, or gameplay behavior changed.

## Scope

| Evidence | Value |
| --- | --- |
| Backlog row | `RB-06805` |
| Original path | `Data/Scripts/Custom/MonsterNest` |
| Target path | `Data/Scripts/Custom/PvE/MonsterNests` |
| Runtime-visible files moved | `6` |
| Namespace/type/API changes | `None` |
| Serialized rows | `6` (Server.Custom.Confictura.LizardmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Custom.Confictura.RatmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNestLoot:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.UndeadNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Mobiles.MonsterNestEntity:v0:SingleVersionOnly:AlignedByCountAndKnownTypes) |
| Runtime hook rows | `4` (Timer=4) |
| Runtime script inventory target rows | `6` |

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
- runtime-script-compile-inventory.csv: regenerated with 6581 runtime-visible script rows, 6 Monster Nests target rows, and 0 old-path rows.
- runtime-hook-map.csv: regenerated with 4 Monster Nests hook rows (Timer=4).
- serialization-register.csv: regenerated with 6 Monster Nests rows preserving namespace/type names and version 0 SingleVersionOnly handling (Server.Custom.Confictura.LizardmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Custom.Confictura.RatmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNestLoot:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.UndeadNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Mobiles.MonsterNestEntity:v0:SingleVersionOnly:AlignedByCountAndKnownTypes).
- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.
- git diff --check: Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors
- ConficturaUO.sln Debug/Any CPU: Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings and no errors
- Server.csproj Debug/x86: Passed; Server.csproj Debug/x86 built ConficturaServer.exe
- .\ConficturaServer.exe -compileonly -nocache: Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done

## Rollback

Move Data/Scripts/Custom/PvE/MonsterNests back to Data/Scripts/Custom/MonsterNest, restore the previous Scripts.csproj compile paths, rerun project truth and path-sensitive audit generators, and revert Monster Nests documentation/source-trace paths.
