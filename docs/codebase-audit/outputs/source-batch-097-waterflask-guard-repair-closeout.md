# SOURCE-BATCH-097 WaterFlask Guard Repair Closeout

## Summary

`SOURCE-BATCH-097` implemented `SB093-CAND-005`, the `WaterFlask` guard repair from `source-batch-093-candidate-discovery.csv`.

`Data/Scripts/Items/Food/WaterFlask.cs` now guards null/deleted mobiles and deleted source flasks before dispatching to `DrinkingFunctions.OnDrink`.

## Preservation

The batch preserved:

- `DrinkingFunctions.OnDrink` delegation for valid state.
- Thirst behavior inside `DrinkingFunctions`, consume behavior, and item reset in `Deserialize`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Food/WaterFlask.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/WaterFlask.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-093-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-flask guard and preserved `DrinkingFunctions.OnDrink(this, from)` delegation plus deserialize reset evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-097` is closed. `SOURCE-BATCH-098+` remains pending the next concrete non-gated source target from `source-batch-093-candidate-discovery.csv`.
