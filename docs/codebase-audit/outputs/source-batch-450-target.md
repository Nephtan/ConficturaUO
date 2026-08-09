# SOURCE-BATCH-450 MountedTrophyHead Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-450`
- Candidate: `SB450-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `MountedTrophyHead.OnDoubleClick`.
- System: `Items:Trades / Taxidermy / MountedTrophyHead`
- File: `Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `AnimalKiller` and `AnimalWhere` properties and property-list display
- all mounted trophy item ID flip pairs
- the existing backpack-use failure message
- constructed item ID, name, and weight
- serialization layout/versioning
- namespace/type/file layout
- Taxidermy corpse trophy generation and corpse visited state
- staff/access behavior, economy/reward tuning, region/map policy, project/config/data, XML/config/data, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-450 MountedTrophyHead Guard Repair`
