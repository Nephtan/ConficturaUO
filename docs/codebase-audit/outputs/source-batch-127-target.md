# SOURCE-BATCH-127 DecoGarlicBulb Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-127`
- Candidate: `SB119-CAND-009`
- Behavior: add stale/null/mobile/source-item guards to `DecoGarlicBulb.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGarlicBulb`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGarlicBulb.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGarlicBulb.cs`: `0`
- Active overlay rows for `DecoGarlicBulb.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18E3`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-127 DecoGarlicBulb Guard Repair`
