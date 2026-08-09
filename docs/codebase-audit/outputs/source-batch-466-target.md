# SOURCE-BATCH-466 ResearchBag Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-466`
- Candidate: `SB466-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `ResearchBag.OnDoubleClick(Mobile from)`.
- System: `Magic:Research / ResearchBag`
- File: `Data/Scripts/Magic/Research/ResearchBag.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- ResearchBag item identity
- `BagOwner` behavior
- research progress/circle/school counters
- blank-scroll storage
- existing backpack-use failure message
- insufficient-skill message
- owner-return/delete behavior
- `ResearchGump` construction
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-466 ResearchBag Guard Repair`
