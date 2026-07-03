# SOURCE-BATCH-412 Watcher Guard Repair Closeout

## Result

`SOURCE-BATCH-412` implemented `SB412-CAND-001`, a non-gated guard repair for `Watcher` pack-animal interactions.

## Source Change

- File: `Data/Scripts/Mobiles/Unusual/Watcher.cs`
- `Watcher.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source Watcher is deleted.
- `Watcher.GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)` now returns for null/deleted mobiles or deleted source Watchers before adding Watcher-specific context menu entries.

## Preserved Behavior

- Watcher stats, breath behavior, taming settings, pack creation, pack animal access policy, drag/drop behavior, death inventory behavior, meat/hides/favorite food, `PackAnimal.TryPackOpen` delegation, `PackAnimal.GetContextMenuEntries` delegation, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB412-CAND-001` row.
- Targeted source scan: passed; mobile/source-Watcher guards are present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Mobiles/Unusual/Watcher.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Mobiles/Unusual/Watcher.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-412` source commit: `e7767a59`. `SOURCE-BATCH-413+` should run fresh candidate discovery after `SOURCE-BATCH-412`.
