# SOURCE-BATCH-090 NecroSkinPotion Guard Repair Closeout

## Summary

`SOURCE-BATCH-090` implemented `SB087-CAND-004`, the `NecroSkinPotion` guard repair from `source-batch-087-candidate-discovery.csv`.

`Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs` now guards null/deleted mobiles, deleted source potions, missing backpacks, and out-of-backpack source potions before race, backpack, hue/hair, thirst, delete, or jar-return state is evaluated.

## Preservation

The batch preserved:

- Race restriction message.
- Backpack failure message.
- Hue values `0x47E` and `0xB70`.
- `RecordSkinColor`, `RecordHairColor`, and `RecordBeardColor` restore behavior.
- Necromancy `>= 100` gate and thirst fallback.
- `this.Delete()` and `AddToBackpack(new Jar())` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-087-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-potion/backpack guards and preserved hue/hair restore, Necromancy, thirst, delete, and jar-return evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-090` is closed. `SOURCE-BATCH-091+` remains pending the next concrete non-gated source target from `source-batch-087-candidate-discovery.csv`.
