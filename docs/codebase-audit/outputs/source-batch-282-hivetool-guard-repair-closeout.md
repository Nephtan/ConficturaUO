# SOURCE-BATCH-282 HiveTool Guard Repair Closeout

## Summary

`SOURCE-BATCH-282` implemented `SB282-CAND-001` in `Data/Scripts/Trades/Apiculture/Items/HiveTool.cs`.

`HiveTool.DisplayDurabilityTo(Mobile m)`, `OnSingleClick(Mobile from)`, and `OnDoubleClick(Mobile from)` now return immediately for null/deleted mobiles or deleted hive tools before durability label display, base single-click dispatch, overhead messaging, or `NetState` access.

## Preserved Behavior

- Durability label text and cliloc.
- Valid-mobile single-click base dispatch.
- Double-click private overhead message text, hue, and `NetState` target.
- `UsesRemaining` property behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item guards and preserved durability label, valid-mobile base single-click dispatch, overhead message text/hue, `NetState` use, `UsesRemaining` property, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Trades/Apiculture/Items/HiveTool.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
