# SOURCE-BATCH-356 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-356` ran fresh non-gated candidate discovery after `SOURCE-BATCH-355` closed. The recommended target is `SB356-CAND-001`, a guard-only repair for `FrankenJournalInBox`.

## Recommended Target

- Batch: `SOURCE-BATCH-356`
- Candidate: `SB356-CAND-001`
- File: `Data/Scripts/Quests/Frankenstein/FrankenJournalInBox.cs`
- Behavior: add stale/null mobile and deleted source item guards before `FrankenJournalInBox.OnDoubleClick` scans existing `FrankenJournal` items, returns an existing journal, creates a new journal, logs discovery, or deletes the source box.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: preserve `PlayerMobile` eligibility, duplicate-journal return behavior, new journal ownership assignment, messages, sound, logging, source box deletion, and serialization.

## Skipped Candidate Notes

- `FrankenItem.cs` was not selected because its target path mutates body-part journal state and consumes a sewing kit.
- `QuestTome.cs` and `RuneBox.cs` were not selected because their guard surfaces cross larger quest reward, gump, fame/karma, and account-return behavior.
- `HoardPile.cs` remains excluded because it crosses randomized reward/economy and region-sensitive loot behavior.
- Porter, robot, travel, housing, vendor, potion-base, and fishing-net target candidates remain excluded unless a later focused goal selects them explicitly.

## Next Step

Implement `SOURCE-BATCH-356 FrankenJournalInBox Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-357+` should run fresh discovery after `SOURCE-BATCH-356` commits.
