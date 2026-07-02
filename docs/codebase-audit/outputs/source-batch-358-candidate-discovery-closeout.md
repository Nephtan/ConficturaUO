# SOURCE-BATCH-358 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-358` ran fresh non-gated candidate discovery after `SOURCE-BATCH-357` closed. The recommended target is `SB358-CAND-001`, a guard-only repair for `SerpentSpawners`.

## Recommended Target

- Batch: `SOURCE-BATCH-358`
- Candidate: `SB358-CAND-001`
- File: `Data/Scripts/Quests/Serpents/SerpentSpawners.cs`
- Behavior: add stale/null mobile, deleted source item, and missing/deleted backpack guards before `SerpentSpawnerOrder.OnDoubleClick` and `SerpentSpawnerChaos.OnDoubleClick` search backpacks or spawn serpents.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: preserve blackrock serpent item eligibility, serpent spawn classes, spawn location/map, sound, source deletion, blue/red no-serpent messages, and serialization.

## Skipped Candidate Notes

- `QuestTake.cs` was not selected because its guard surface crosses randomized major quest generation and a timer-bearing file.
- `FrankenItem.cs` was not selected because its target path mutates body-part journal state and consumes a sewing kit.
- `QuestTome.cs`, `RuneBox.cs`, and `Museum.cs` remain deferred because their guard surfaces cross larger quest reward, gump, economy/value, or fame/karma behavior.
- `HoardPile.cs` remains excluded because it crosses randomized reward/economy and region-sensitive loot behavior.
- Porter, robot, travel, housing, vendor, potion-base, and fishing-net target candidates remain excluded unless a later focused goal selects them explicitly.

## Next Step

Implement `SOURCE-BATCH-358 SerpentSpawners Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-359+` should run fresh discovery after `SOURCE-BATCH-358` commits.
