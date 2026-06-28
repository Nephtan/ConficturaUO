# SOURCE-BATCH-138 DecoNightshade Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-138`
- Candidate: `SB119-CAND-020`
- Behavior: add stale/null/mobile/source-item guards to `DecoNightshade.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoNightshade`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoNightshade.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoNightshade.cs`: `0`
- Active overlay rows for `DecoNightshade.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18E7`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-138 DecoNightshade Guard Repair`
