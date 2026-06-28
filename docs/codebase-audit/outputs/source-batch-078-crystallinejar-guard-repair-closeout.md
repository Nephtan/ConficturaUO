# SOURCE-BATCH-078 CrystallineJar Guard Repair Closeout

## Summary

`SOURCE-BATCH-078` completed fresh candidate discovery, selected `SB078-CAND-001`, and implemented the `CrystallineJar` guard repair.

`Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs` now guards null/deleted mobiles, deleted source jars, missing backpacks, out-of-backpack source jars, deleted scoop targets, and null throw targets before scoop or throw state is evaluated.

## Preservation

The batch preserved:

- Backpack message `1060640`.
- Empty flask scoop prompt and full flask throw prompt.
- `MonsterSplatter.TooMuchSplatter` behavior.
- `ThrowTarget` reuse check.
- `MonsterSplatter` and `HolyWater` scoop behavior.
- Throw range, distance, line-of-sight, casting, paralyzed, blessed, and frozen checks.
- `MonsterSplatter.AddSplatter` behavior.
- Reset-to-empty `Name`, `Hue`, and `Weight` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-078-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-jar/backpack/deleted-target/null-target guards and preserved scoop/throw evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-078` is closed. `SOURCE-BATCH-079+` remains pending the next concrete non-gated source target from `source-batch-078-candidate-discovery.csv`.
