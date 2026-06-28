# SOURCE-BATCH-117 RottedReagents Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-117`
- Candidate: `SB111-CAND-007`
- Behavior: add stale/null/mobile/source-reagents guards to `RottedReagents.OnDoubleClick`.
- System: `Items:Traps / RottedReagents`
- File: `Data/Scripts/Items/Traps/RottedReagents.cs`

## Fence Result

- POST-BATCH-Y gate hits for `RottedReagents.cs`: `0`
- Active overlay rows for `RottedReagents.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `These reagents are useless.`
- Constructor item ID `0xE76`, name, hue, weight, `RottedCount` behavior, and property labels.
- Serialization layout/versioning, including `RottedCount` write/read order.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-117 RottedReagents Guard Repair`
