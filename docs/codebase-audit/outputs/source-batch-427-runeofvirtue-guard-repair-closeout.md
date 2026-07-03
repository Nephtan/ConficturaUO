# SOURCE-BATCH-427 RuneOfVirtue Guard Repair Closeout

## Result

`SOURCE-BATCH-427` implemented `SB427-CAND-001`, a non-gated guard repair for RuneOfVirtue double-click interaction.

## Source Change

- File: `Data/Scripts/Items/Magical/RuneOfVirtue.cs`
- `RuneOfVirtue.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source rune is deleted before reading `from.Backpack`.

## Preserved Behavior

Backpack-use localized message `1060640`, `RuneLook` name/item ID cycle, morality side hue behavior, owner/side fields, `OnEquip` and `MoralityCheck` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB427-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick guard is present; backpack-use message, `RuneLook(this)`, item cycle, hue behavior, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Magical/RuneOfVirtue.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Magical/RuneOfVirtue.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-427` source commit: pending. `SOURCE-BATCH-428+` should run fresh candidate discovery after `SOURCE-BATCH-427`.
