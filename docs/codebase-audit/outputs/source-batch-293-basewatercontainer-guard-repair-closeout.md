# SOURCE-BATCH-293 BaseWaterContainer Guard Repair Closeout

## Summary

`SOURCE-BATCH-293` implemented `SB293-CAND-001` in `Data/Scripts/Items/Special/Rares/Containers/BaseWaterContainer.cs`.

`BaseWaterContainer.OnDoubleClick(Mobile from)` and `BaseWaterContainer.OnSingleClick(Mobile from)` now return immediately for null/deleted mobiles or deleted water containers before forwarding to base container behavior. `BaseWaterContainer.OnDragDropInto(Mobile from, Item item, Point3D p)` now returns `false` for null/deleted mobiles, null/deleted dropped items, or deleted water containers before full/empty checks or base drag/drop forwarding.

## Preserved Behavior

- Quantity clamping and `IsEmpty`/`IsFull` behavior.
- `Movable` and `ItemID` updates.
- `DefaultGumpID`.
- Empty-container base forwarding behavior.
- The current `OnSingleClick` call to `base.OnDoubleClick(from)`.
- Full-container drag/drop rejection.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-container/drop-item guards and preserved quantity clamping, `Movable`/`ItemID` updates, `DefaultGumpID`, empty-container base forwarding, the current `OnSingleClick` call to `base.OnDoubleClick`, full-container drag/drop rejection, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Special/Rares/Containers/BaseWaterContainer.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
