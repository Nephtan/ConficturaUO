# SOURCE-BATCH-426 SpecialBeardDye Guard Repair Closeout

## Result

`SOURCE-BATCH-426` implemented `SB426-CAND-001`, a non-gated guard repair for SpecialBeardDye interactions.

## Source Change

- File: `Data/Scripts/Items/Misc/SpecialBeardDye.cs`
- `SpecialBeardDye.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source dye is deleted before range checks or gump display.
- `SpecialBeardDyeGump.OnResponse(NetState from, RelayInfo info)` now returns immediately for null `NetState`, null `RelayInfo`, null/deleted source dye, null/deleted response mobile, or null switch arrays before backpack checks, hue selection, facial hair hue assignment, messages, sound, or dye deletion.

## Preserved Behavior

Range requirement, `SpecialBeardDyeGump` layout, hue entry table, selected hue calculation, no-facial-hair rejection, backpack requirement, facial hair hue assignment, dye `Delete` behavior, localized messages `501199`, `501200`, `502623`, and `1042010`, sound `0x4E`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB426-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick and OnResponse state/mobile/switch guards are present; range/gump behavior, facial hair hue assignment, delete/sound/messages, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Misc/SpecialBeardDye.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Misc/SpecialBeardDye.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion beyond stale response guards, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-426` source commit: pending. `SOURCE-BATCH-427+` should run fresh candidate discovery after `SOURCE-BATCH-426`.
