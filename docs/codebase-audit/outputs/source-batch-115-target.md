# SOURCE-BATCH-115 SlimeItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-115`
- Candidate: `SB111-CAND-005`
- Behavior: add stale/null/mobile/source-item guards to `SlimeItem.OnDoubleClick`.
- System: `Items:Traps / SlimeItem`
- File: `Data/Scripts/Items/Traps/SlimeItem.cs`

## Fence Result

- POST-BATCH-Y gate hits for `SlimeItem.cs`: `0`
- Active overlay rows for `SlimeItem.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `This item is covered in slime and cannot be used.`
- Constructor item ID `0x9A8`, lock settings, name, weight, and property labels.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-115 SlimeItem Guard Repair`
