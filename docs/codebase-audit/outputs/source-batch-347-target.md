# SOURCE-BATCH-347 BellOfCourage Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-347`
- Candidate: `SB347-CAND-001`
- System: `Quests:Shadowlords / BellOfCourage`
- Source file: `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs`
- Behavior: add stale/null/mobile/source-item guards to `BellOfCourage.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-use failure message when the bell item is deleted, the mobile has no backpack, or the bell is not in the backpack.

## Must Stay Unchanged

- Item ID `0x1C12`, `Name = "Bell of Courage"`, and `Weight = 1.0`.
- `m_Sounds` values `{ 0x505, 0x506, 0x507 }` and random sound behavior.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You ring the bell, producing a courageous melody.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-347 BellOfCourage Guard Repair`
