# SOURCE-BATCH-279 DDRelicAlchemy Guard Repair Closeout

## Summary

`SOURCE-BATCH-279` implemented `SB279-CAND-001` in `Data/Scripts/Items/Relics/DDRelicAlchemy.cs`.

`DDRelicAlchemy.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted relic alchemy items before sending the relic-identification message.

## Preserved Behavior

- Relic-identification message text.
- `RelicGoldValue` calculation/storage.
- Randomized alchemy flask `ItemID` and `Name` construction.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, relic value policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item guard and preserved relic-identification message, `RelicGoldValue`, randomized `Utility.RandomList` construction, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Relics/DDRelicAlchemy.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
