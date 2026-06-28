# SOURCE-BATCH-087 HairDyePotion Guard Repair Closeout

## Summary

`SOURCE-BATCH-087` completed fresh candidate discovery, selected `SB087-CAND-001`, and implemented the `HairDyePotion` guard repair.

`Data/Scripts/Items/Potions/Special/HairDyePotion.cs` now guards null/deleted mobiles, deleted source potions, missing backpacks, and out-of-backpack source potions before race, backpack, hue, hair-color mutation, drink effect, sound, or consume state is evaluated.

## Preservation

The batch preserved:

- Race restriction message.
- Backpack failure message.
- Neutral hue restore behavior.
- Dyed hue assignment behavior.
- Hair and facial hair hue behavior.
- `ConsumeCharge`, `RevealingAction`, `BasePotion.PlayDrinkEffect`, sound, and `potion.Consume()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/HairDyePotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/HairDyePotion.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-087-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-potion/backpack guards and preserved race, hair hue, message, drink-effect, sound, and consume evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-087` is closed. `SOURCE-BATCH-088+` remains pending the next concrete non-gated source target from `source-batch-087-candidate-discovery.csv`.
