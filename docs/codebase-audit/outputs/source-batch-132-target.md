# SOURCE-BATCH-132 DecoGinsengRoot2 Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-132`
- Candidate: `SB119-CAND-014`
- Behavior: add stale/null/mobile/source-item guards to `DecoGinsengRoot2.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGinsengRoot2`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGinsengRoot2.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGinsengRoot2.cs`: `0`
- Active overlay rows for `DecoGinsengRoot2.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18EC`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-132 DecoGinsengRoot2 Guard Repair`
