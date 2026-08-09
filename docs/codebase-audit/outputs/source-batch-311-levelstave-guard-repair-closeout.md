# SOURCE-BATCH-311 LevelStave Guard Repair Closeout

## Summary

`SOURCE-BATCH-311` implemented `SB311-CAND-001` as a focused, non-gated LevelStave interaction/helper guard repair.

## Source Change

- File: `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`
- Added null/deleted mobile and deleted source stave guards before possession checks and target assignment in `BaseLevelStave.OnDoubleClick`.
- Added missing-backpack handling while preserving valid equipped/held stave use.
- Added null/deleted mobile, null/deleted gem target, deleted gem target, and missing-backpack guards before gem backpack checks or conversion in `GemTarget.OnTarget`.
- Added null/deleted mobile and missing-backpack safety to `BaseLevelStave.HasStaff` while preserving valid equipped and backpack true results.

## Preserved Behavior

- LevelStave/LevelSceptre constructors
- Combat/ranged behavior
- Leveling behavior
- `damageType`
- Ammo consumption and `MageEye` item identity
- Possession rule for valid backpack/equipped state
- Gem eligibility and conversion amount math
- `MageEye` creation
- Messages
- Sound `0x243`
- `RevealingAction`
- Gem delete semantics
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and LevelStave prompt, gem conversion, helper, sound, `MageEye`, delete, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-312+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-311` is committed. `GiftStave.cs` remains the next deferred sibling candidate if exact-file gate and overlay scans still pass.
