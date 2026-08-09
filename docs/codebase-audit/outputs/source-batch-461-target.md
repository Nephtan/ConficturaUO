# SOURCE-BATCH-461 SearchPage Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-461`
- Candidate: `SB461-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `SearchPage.OnDoubleClick(Mobile e)`.
- System: `Quests:Search / SearchPage`
- File: `Data/Scripts/Quests/Search/SearchPage.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- search page contents
- `SearchGump` construction
- existing backpack-use failure message
- search-location selection logic
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-461 SearchPage Guard Repair`
