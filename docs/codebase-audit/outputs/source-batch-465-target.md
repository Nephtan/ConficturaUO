# SOURCE-BATCH-465 BagOfTricks Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-465`
- Candidate: `SB465-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `BagOfTricks.OnDoubleClick(Mobile from)`.
- System: `Magic:Jester / BagOfTricks`
- File: `Data/Scripts/Magic/Jester/BagOfTricks.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- BagOfTricks item identity
- `PrankPoints` storage and accounting
- existing backpack-use failure message
- `BagOfTricksGump` construction
- sound `0x48`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-465 BagOfTricks Guard Repair`
