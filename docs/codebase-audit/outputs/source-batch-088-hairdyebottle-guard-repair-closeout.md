# SOURCE-BATCH-088 HairDyeBottle Guard Repair Closeout

## Summary

`SOURCE-BATCH-088` implemented `SB087-CAND-002`, the `HairDyeBottle` guard repair from `source-batch-087-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/HairDyeBottle.cs` now guards null/deleted mobiles, deleted source bottles, missing backpacks, and out-of-backpack source bottles before race, backpack, hue, hair-color mutation, sound, or delete state is evaluated.

## Preservation

The batch preserved:

- Race restriction message.
- Backpack failure message.
- Neutral hue restore behavior.
- Dyed hue assignment behavior.
- Hair and facial hair hue behavior.
- Sound `0x5A4` and `this.Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/HairDyeBottle.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/HairDyeBottle.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-087-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-bottle/backpack guards and preserved race, hair hue, message, sound, and delete evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-088` is closed. `SOURCE-BATCH-089+` remains pending the next concrete non-gated source target from `source-batch-087-candidate-discovery.csv`.
