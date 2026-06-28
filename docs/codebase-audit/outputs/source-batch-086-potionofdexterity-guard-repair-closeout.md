# SOURCE-BATCH-086 PotionOfDexterity Guard Repair Closeout

## Summary

`SOURCE-BATCH-086` implemented `SB084-CAND-003` from `source-batch-084-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs` now guards null/deleted mobiles, deleted source potions, missing backpacks, and out-of-backpack source potions before stat-cap, chance, `RawDex`, drink effect, or potion consume state is evaluated.

## Preservation

The batch preserved:

- Localized message `1060640`.
- `StatCap > RawStatTotal` gate.
- `Utility.RandomMinMax` chance thresholds and `EnhancePotions(from)` contribution.
- `AvailPoints` behavior.
- `RawDex` increment behavior.
- Success and no-effect messages.
- `BasePotion.PlayDrinkEffect(from)`.
- `this.Consume()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-084-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-potion/backpack guards and preserved stat-gain/message/drink-effect/consume evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-086` is closed. The `source-batch-084-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-087+` requires fresh candidate discovery.
