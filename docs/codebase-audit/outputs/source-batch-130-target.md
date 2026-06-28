# SOURCE-BATCH-130 DecoGinseng2 Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-130`
- Candidate: `SB119-CAND-012`
- Behavior: add stale/null/mobile/source-item guards to `DecoGinseng2.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGinseng2`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGinseng2.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGinseng2.cs`: `0`
- Active overlay rows for `DecoGinseng2.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18EA`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-130 DecoGinseng2 Guard Repair`
