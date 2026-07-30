# SOURCE-BATCH-112 CurseItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-112`
- Candidate: `SB111-CAND-002`
- Behavior: add stale/null/mobile/source-item guards to `CurseItem.OnDoubleClick`.
- System: `Items:Traps / CurseItem`
- File: `Data/Scripts/Items/Traps/CurseItem.cs`

## Fence Result

- POST-BATCH-Y gate hits for `CurseItem.cs`: `0`
- Active overlay rows for `CurseItem.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `This item is cursed and cannot be used.`
- Constructor item ID `0x9A8`, lock settings, name, and weight.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-112 CurseItem Guard Repair`
