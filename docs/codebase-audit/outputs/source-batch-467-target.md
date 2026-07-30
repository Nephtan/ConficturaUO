# SOURCE-BATCH-467 LandmineSetup Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-467`
- Candidate: `SB467-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `LandmineSetup.OnDoubleClick(Mobile from)`.
- System: `Items:Technology / LandmineSetup`
- File: `Data/Scripts/Items/Technology/Landmine.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.
- The existing `Region.AllowHarmful(from, from)` placement policy is preserved, not changed.

## Must Stay Unchanged

- `LandmineSetup` item identity
- existing backpack-use failure message
- nearby landmine limit
- `IPooledEnumerable.Free()` behavior
- `Region.AllowHarmful(from, from)` policy check
- `RemoveTrap` power math
- sound `0x42`
- `Landmine` owner/power construction
- map/location placement
- setup item `Delete()` semantics
- messages
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-467 LandmineSetup Guard Repair`
