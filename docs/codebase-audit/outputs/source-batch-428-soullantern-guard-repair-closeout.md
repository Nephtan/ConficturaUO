# SOURCE-BATCH-428 SoulLantern Guard Repair Closeout

## Result

`SOURCE-BATCH-428` implemented `SB428-CAND-001`, a non-gated guard repair for SoulLantern double-click interaction.

## Source Change

- File: `Data/Scripts/Magic/Death Knight/SoulLantern.cs`
- `SoulLantern.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source lantern is deleted before reading the mobile's equipped two-handed layer.

## Preserved Behavior

Owner comparison, equipped-layer detection, backpack-use localized message `1042001`, item ID transitions `0xA18` and `0xA15`, sounds `0x4BB` and `0x47`, base `OnRemoved`/`OnEquip` calls, `TrappedSouls` and `owner` fields, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB428-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick guard is present; equipped-layer lookup, AddToBackpack behavior, item ID transitions, sounds, owner check, base OnRemoved/OnEquip calls, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Magic/Death Knight/SoulLantern.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Magic/Death Knight/SoulLantern.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-428` source commit: `fe3737e9` (`fix: guard SoulLantern interactions`). `SOURCE-BATCH-429+` should run fresh candidate discovery after `SOURCE-BATCH-428`.
