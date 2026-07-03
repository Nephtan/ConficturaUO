# SOURCE-BATCH-425 SpecialHairDye Guard Repair Closeout

## Result

`SOURCE-BATCH-425` implemented `SB425-CAND-001`, a non-gated guard repair for SpecialHairDye interactions.

## Source Change

- File: `Data/Scripts/Items/Misc/SpecialHairDye.cs`
- `SpecialHairDye.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source dye is deleted before range checks or gump display.
- `SpecialHairDyeGump.OnResponse(NetState from, RelayInfo info)` now returns immediately for null `NetState`, null `RelayInfo`, null/deleted source dye, null/deleted response mobile, or null switch arrays before backpack checks, hue selection, hair hue assignment, messages, sound, or dye deletion.

## Preserved Behavior

Range requirement, `SpecialHairDyeGump` layout, hue entry table, selected hue calculation, no-hair rejection, backpack requirement, hair hue assignment, dye `Delete` behavior, localized messages `501199`, `501200`, `502623`, and `1042010`, sound `0x4E`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB425-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick and OnResponse state/mobile/switch guards are present; range/gump behavior, hair hue assignment, delete/sound/messages, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Misc/SpecialHairDye.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Misc/SpecialHairDye.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion beyond stale response guards, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-425` source commit: pending. `SOURCE-BATCH-426+` should run fresh candidate discovery after `SOURCE-BATCH-425`.
