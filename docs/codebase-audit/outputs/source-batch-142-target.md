# SOURCE-BATCH-142 DecoPumice Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-142`
- Candidate: `SB119-CAND-024`
- Behavior: add stale/null/mobile/source-item guards to `DecoPumice.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoPumice`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoPumice.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoPumice.cs`: `0`
- Active overlay rows for `DecoPumice.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0xF8B`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-142 DecoPumice Guard Repair`
