# SOURCE-BATCH-129 DecoGinseng Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-129`
- Candidate: `SB119-CAND-011`
- Behavior: add stale/null/mobile/source-item guards to `DecoGinseng.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGinseng`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGinseng.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGinseng.cs`: `0`
- Active overlay rows for `DecoGinseng.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18E9`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-129 DecoGinseng Guard Repair`
