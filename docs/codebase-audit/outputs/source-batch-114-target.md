# SOURCE-BATCH-114 WeedItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-114`
- Candidate: `SB111-CAND-004`
- Behavior: add stale/null/mobile/source-item guards to `WeededItem.OnDoubleClick`.
- System: `Items:Traps / WeedItem`
- File: `Data/Scripts/Items/Traps/WeedItem.cs`

## Fence Result

- POST-BATCH-Y gate hits for `WeedItem.cs`: `0`
- Active overlay rows for `WeedItem.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `This item is wrapped in weeds and cannot be used.`
- Constructor item ID `0x9A8`, lock settings, name, hue, weight, and property labels.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-114 WeedItem Guard Repair`
