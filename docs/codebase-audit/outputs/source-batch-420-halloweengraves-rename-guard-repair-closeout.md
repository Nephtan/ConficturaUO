# SOURCE-BATCH-420 HalloweenGraves Rename Guard Repair Closeout

## Result

`SOURCE-BATCH-420` implemented `SB420-CAND-001`, a non-gated guard repair for Halloween grave rename interactions.

## Source Change

- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave1.cs`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave2.cs`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenGrave3.cs`
- `HalloweenGrave1.OnDoubleClick(Mobile from)`, `HalloweenGrave2.OnDoubleClick(Mobile from)`, and `HalloweenGrave3.OnDoubleClick(Mobile from)` now return immediately when `from` is null, `from` is deleted, or the source grave is deleted before sending the rename prompt message or assigning `RenamePrompt`.

## Preserved Behavior

- Grave item IDs, `Flipable` attributes, `Furniture` attributes, default `Name` and `Weight`, rename prompt message, prompt assignment, prompt response stale-state guard, name assignment, confirmation message, old `Weight == 4.0` normalization, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB420-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick mobile/source guards are present in all three grave files; prompt assignment, rename confirmation message, existing prompt response guard, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; all three grave files have `0` gate hits.
- Exact-file active overlay scan: passed; all three grave files have `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-420` source commit: `5e053a11`. `SOURCE-BATCH-421+` should run fresh candidate discovery after `SOURCE-BATCH-420`.
