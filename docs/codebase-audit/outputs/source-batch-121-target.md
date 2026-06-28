# SOURCE-BATCH-121 DecoBrimstone Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-121`
- Candidate: `SB119-CAND-003`
- Behavior: add stale/null/mobile/source-item guards to `DecoBrimstone.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoBrimstone`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoBrimstone.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoBrimstone.cs`: `0`
- Active overlay rows for `DecoBrimstone.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0xF7F`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-121 DecoBrimstone Guard Repair`
