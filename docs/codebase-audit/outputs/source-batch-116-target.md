# SOURCE-BATCH-116 SewageItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-116`
- Candidate: `SB111-CAND-006`
- Behavior: add stale/null/mobile/source-item guards to `SewageItem.OnDoubleClick`.
- System: `Items:Traps / SewageItem`
- File: `Data/Scripts/Items/Traps/SewageItem.cs`

## Fence Result

- POST-BATCH-Y gate hits for `SewageItem.cs`: `0`
- Active overlay rows for `SewageItem.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `This item is covered in sewage and cannot be used.`
- Constructor item ID `0x9A8`, lock settings, name, hue, weight, and property labels.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-116 SewageItem Guard Repair`
