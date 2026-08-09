# SOURCE-BATCH-414 HintItem Guard Repair Closeout

## Result

`SOURCE-BATCH-414` implemented `SB414-CAND-001`, a non-gated guard repair for `HintItem` double-click hint display.

## Source Change

- File: `Data/Scripts/Items/Misc/WarningItem.cs`
- `HintItem.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source HintItem is deleted.

## Preserved Behavior

- `WarningItem` movement broadcast behavior, pooled enumerable ownership, neighbor broadcast behavior, `HintItem` message dispatch, warning/hint string and number persistence, range/reset behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB414-CAND-001` row.
- Targeted source scan: passed; mobile/source-item guard is present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Misc/WarningItem.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Misc/WarningItem.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-414` source commit: `b27bb575`. `SOURCE-BATCH-415+` should run fresh candidate discovery after `SOURCE-BATCH-414`.
