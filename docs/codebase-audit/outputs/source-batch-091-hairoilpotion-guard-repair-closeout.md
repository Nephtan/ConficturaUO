# SOURCE-BATCH-091 HairOilPotion Guard Repair Closeout

## Summary

`SOURCE-BATCH-091` implemented `SB087-CAND-005`, the `HairOilPotion` guard repair from `source-batch-087-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/HairOilPotion.cs` now guards null/deleted mobiles, deleted source potions, missing backpacks, out-of-backpack source potions, and stale gump responses before race, backpack, hair style, consume, sound, or gump response state is evaluated.

## Preservation

The batch preserved:

- Race restriction message.
- Backpack localized message `1060640`.
- `PotionGump` layout, hair style IDs, and button IDs.
- Male/female hair option behavior.
- `ConsumeCharge`, `RevealingAction`, `BasePotion.PlayDrinkEffect`, sound, and consume behavior.
- Hair mutation and success message behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/HairOilPotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/HairOilPotion.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-087-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-potion/backpack/gump-response guards and preserved gump, hair-style, message, sound, effect, and consume evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named gump-response guard.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-091` is closed. The `source-batch-087-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-092+` requires fresh candidate discovery.
