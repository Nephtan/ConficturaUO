# SOURCE-BATCH-339 BottleOil Guard Repair Closeout

## Summary

`SOURCE-BATCH-339` implemented `SB339-CAND-001`, a non-gated guard repair for `BottleOil`.

## Source Change

- File: `Data/Scripts/Quests/Golems/BottleOil.cs`
- `BottleOil.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted bottle oil items before sending the existing don't-drink message.

## Preserved Behavior

- Item ID `0xF0E`, `Weight = 0.01`, `Stackable = true`, `Amount`, `Hue = 0x497`, and `Name = "technomancer oil"`.
- Message text: `I don't think you want to drink that!`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Golems/BottleOil.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Golems/BottleOil.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved message, constructor metadata, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-339` is ready to commit as `fix: guard BottleOil interactions`. `SOURCE-BATCH-340+` should run fresh candidate discovery before any further source edits.
