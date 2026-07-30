# SOURCE-BATCH-131 DecoGinsengRoot Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-131`
- Candidate: `SB119-CAND-013`
- Behavior: add stale/null/mobile/source-item guards to `DecoGinsengRoot.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGinsengRoot`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGinsengRoot.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGinsengRoot.cs`: `0`
- Active overlay rows for `DecoGinsengRoot.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18EB`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-131 DecoGinsengRoot Guard Repair`
