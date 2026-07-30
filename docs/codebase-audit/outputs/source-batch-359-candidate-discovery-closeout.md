# SOURCE-BATCH-359 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-359` ran fresh non-gated candidate discovery after `SOURCE-BATCH-358` closed. The recommended target is `SB359-CAND-001`, a guard-only repair for `Museums`.

## Recommended Target

- Batch: `SOURCE-BATCH-359`
- Candidate: `SB359-CAND-001`
- File: `Data/Scripts/Quests/Museum/Museum.cs`
- Behavior: add stale/null mobile and deleted source item guards before `Museums.OnDoubleClick` checks backpack membership, reports antique value, or toggles display item state.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: preserve antique value calculation, backpack detection, display item ID/light toggles, sounds, serialization, and all museum sale/value helpers.

## Skipped Candidate Notes

- `QuestTake.cs` remains deferred because its guard surface crosses randomized major quest generation and a timer-bearing file.
- `FrankenItem.cs` remains deferred because its target path mutates body-part journal state and consumes a sewing kit.
- `QuestTome.cs` and `RuneBox.cs` remain deferred because their guard surfaces cross larger quest reward, gump, fame/karma, and account-return behavior.
- `HoardPile.cs` remains excluded because it crosses randomized reward/economy and region-sensitive loot behavior.
- Porter, robot, travel, housing, vendor, potion-base, and fishing-net target candidates remain excluded unless a later focused goal selects them explicitly.

## Next Step

Implement `SOURCE-BATCH-359 Museums Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-360+` should run fresh discovery after `SOURCE-BATCH-359` commits.
