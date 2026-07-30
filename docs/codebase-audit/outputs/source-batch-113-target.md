# SOURCE-BATCH-113 TaintedBandage Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-113`
- Candidate: `SB111-CAND-003`
- Behavior: add stale/null/mobile/source-item guards to `TaintedBandage.OnDoubleClick`.
- System: `Items:Traps / TaintedBandage`
- File: `Data/Scripts/Items/Traps/TaintedBandage.cs`

## Fence Result

- POST-BATCH-Y gate hits for `TaintedBandage.cs`: `0`
- Active overlay rows for `TaintedBandage.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state message: `You cannot use tainted bandages.`
- Constructor item ID `0xE21`, name, hue, weight, and `Ruined` property label.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-113 TaintedBandage Guard Repair`
