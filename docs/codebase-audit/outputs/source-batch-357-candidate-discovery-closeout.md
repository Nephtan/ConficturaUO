# SOURCE-BATCH-357 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-357` ran fresh non-gated candidate discovery after `SOURCE-BATCH-356` closed. The recommended target is `SB357-CAND-001`, a guard-only repair for `ObeliskOnCorpse`.

## Recommended Target

- Batch: `SOURCE-BATCH-357`
- Candidate: `SB357-CAND-001`
- File: `Data/Scripts/Quests/Pagan/ObeliskOnCorpse.cs`
- Behavior: add stale/null mobile and deleted source item guards before `ObeliskOnCorpse.OnDoubleClick` checks `PlayerMobile` state, returns or grants an `ObeliskTip`, logs discovery, or deletes the source item.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: preserve `OnDragLift` delegation, `PlayerMobile` eligibility, Titan rejection, duplicate-tip return behavior, `SetupObelisk` defaults, messages, sound, logging, source deletion, and serialization.

## Skipped Candidate Notes

- `FrankenItem.cs` was not selected because its target path mutates body-part journal state and consumes a sewing kit.
- `QuestTome.cs` and `RuneBox.cs` remain deferred because their guard surfaces cross larger quest reward, gump, fame/karma, and account-return behavior.
- `HoardPile.cs` remains excluded because it crosses randomized reward/economy and region-sensitive loot behavior.
- Porter, robot, travel, housing, vendor, potion-base, and fishing-net target candidates remain excluded unless a later focused goal selects them explicitly.

## Next Step

Implement `SOURCE-BATCH-357 ObeliskOnCorpse Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-358+` should run fresh discovery after `SOURCE-BATCH-357` commits.
