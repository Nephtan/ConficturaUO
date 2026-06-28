# SOURCE-BATCH-118 RuinedGems Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-118`
- Candidate: `SB111-CAND-008`
- Behavior: add stale/null/mobile/source-gems guards to `RuinedGems.OnDoubleClick`.
- System: `Items:Traps / RuinedGems`
- File: `Data/Scripts/Items/Traps/RuinedGems.cs`

## Fence Result

- POST-BATCH-Y gate hits for `RuinedGems.cs`: `0`
- Active overlay rows for `RuinedGems.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `This is a useless lump of rock.`
- Constructor item ID `0x5739`, name, weight, `RuinedCount` behavior, and property labels.
- Serialization layout/versioning, including `RuinedCount` write/read order.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-118 RuinedGems Guard Repair`
