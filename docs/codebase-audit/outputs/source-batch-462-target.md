# SOURCE-BATCH-462 CourierMail Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-462`
- Candidate: `SB462-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `CourierMail.OnDoubleClick(Mobile e)`.
- System: `Quests:Epic / CourierMail`
- File: `Data/Scripts/Quests/Epic/CourierMail.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- courier mail contents
- `SearchGump` construction
- sound `0x249`
- existing backpack-use failure message
- owner/dungeon serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-462 CourierMail Guard Repair`
