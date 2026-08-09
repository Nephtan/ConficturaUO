# SOURCE-BATCH-464 BookDruidBrewing Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-464`
- Candidate: `SB464-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `BookDruidBrewing.OnDoubleClick(Mobile e)`.
- System: `Magic:Druidism / BookDruidBrewing`
- File: `Data/Scripts/Magic/Druidism/BookDruidBrewing.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- druid brewing book contents
- `BookGump` construction
- recipes
- druid spell/pouch behavior
- existing backpack-use failure message
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-464 BookDruidBrewing Guard Repair`
