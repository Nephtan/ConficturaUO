# SOURCE-BATCH-345 BookOfTruth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-345`
- Candidate: `SB345-CAND-001`
- System: `Quests:Shadowlords / BookOfTruth`
- Source file: `Data/Scripts/Quests/Shadowlords/BookOfTruth.cs`
- Behavior: add stale/null/mobile/source-item guards to `BookOfTruth.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-read failure message when the book item is deleted, the mobile has no backpack, or the book is not in the backpack.

## Must Stay Unchanged

- Item ID `0x1C13`, `Name = "Book of Truth"`, and `Weight = 1.0`.
- Existing backpack-read failure message: `This must be in your backpack to read.`
- Existing success message: `You learn a little bit more about the principles of truth.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-345 BookOfTruth Guard Repair`
