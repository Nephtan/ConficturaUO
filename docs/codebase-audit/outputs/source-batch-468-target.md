# SOURCE-BATCH-468 TrapKit Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-468`
- Candidate: `SB468-CAND-001`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards to `TrapKit.OnDoubleClick(Mobile from)`.
- System: `Items:Traps / TrapKit`
- File: `Data/Scripts/Items/Traps/TrapKit.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.
- The existing `Region.AllowHarmful(from, from)` placement policy is preserved, not changed.

## Must Stay Unchanged

- `TrapKit` item identity
- metal and charge properties
- existing backpack-use failure message
- nearby trap limit
- `IPooledEnumerable.Free()` behavior
- `Region.AllowHarmful(from, from)` policy check
- `RemoveTrap` skill gate
- charge consumption and worn-out deletion
- metal-based power bonuses
- sound `0x55`
- `SetTrap` creation
- map/hue/location placement
- messages
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-468 TrapKit Guard Repair`
