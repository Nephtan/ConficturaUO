# SOURCE-BATCH-455 LearnStealingBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-455`
- Candidate: `SB455-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `LearnStealingBook.OnDoubleClick(Mobile e)`.
- System: `Items:Books / LearnStealingBook`
- File: `Data/Scripts/Items/Books/LearnStealing.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- book item ID, name, weight, and property text
- stealing guide text
- `LearnStealingGump` layout
- sound `0x249`
- `Server.Gumps.MyLibrary.readBook(this, e)` behavior
- existing backpack-use failure message
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-455 LearnStealingBook Guard Repair`
