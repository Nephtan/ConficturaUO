# SOURCE-BATCH-346 CandleOfLove Guard Repair Closeout

## Summary

`SOURCE-BATCH-346` implemented `SB346-CAND-001`, a non-gated guard repair for `CandleOfLove`.

## Source Change

- File: `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs`
- `CandleOfLove.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before reading `from.Backpack`.
- Deleted source candles, missing backpacks, and candles outside the backpack now use the existing backpack-use failure message before the existing success path.

## Preserved Behavior

- Item ID `0x1C14`, `Name = "Candle of Love"`, `Weight = 1.0`, and `Light = LightType.Circle150`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You feel the loving warmth of the flame.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved constructor metadata, backpack-use message, success message, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-346` is ready to commit as `fix: guard CandleOfLove interactions`. `SOURCE-BATCH-347+` should continue with `BellOfCourage` if exact-file preflight remains clean.
