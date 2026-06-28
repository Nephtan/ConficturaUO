# SOURCE-BATCH-133 DecoMandrake Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-133`
- Candidate: `SB119-CAND-015`
- Behavior: add stale/null/mobile/source-item guards to `DecoMandrake.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoMandrake`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoMandrake.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoMandrake.cs`: `0`
- Active overlay rows for `DecoMandrake.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18DF`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-133 DecoMandrake Guard Repair`
