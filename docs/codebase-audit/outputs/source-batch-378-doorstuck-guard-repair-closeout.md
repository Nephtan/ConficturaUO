# SOURCE-BATCH-378 DoorStuck Guard Repair Closeout

## Result

`SOURCE-BATCH-378` implemented `SB378-CAND-001`, a non-gated guard repair for `DoorStuck`.

## Source Change

- File: `Data/Scripts/Items/Doors/DoorStuck.cs`
- Added early returns when `m == null || m.Deleted || Deleted` in `OnDoubleClick` and `OnDoubleClickDead`.

## Preserved Behavior

Door item ID/name, locked-door message text, valid-mobile message behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Doors/DoorStuck.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Doors/DoorStuck.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one row imported and required fields were present.
- Targeted source scan: passed; both stale/null/mobile/source-door guards exist and preserved behavior remained present.
- Exact-file POST-BATCH-Y scan: passed; `0` gate rows for `Data/Scripts/Items/Doors/DoorStuck.cs`.
- Exact-file active overlay scan: passed; `0` active overlay rows for `Data/Scripts/Items/Doors/DoorStuck.cs`.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-378` commit is pending. `SOURCE-BATCH-379+` should run fresh candidate discovery after `SOURCE-BATCH-378`.
