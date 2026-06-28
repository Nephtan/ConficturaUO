# SOURCE-BATCH-119 DecoBlackmoor Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-119`
- Candidate: `SB119-CAND-001`
- Behavior: add stale/null/mobile/source-item guards to `DecoBlackmoor.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoBlackmoor`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoBlackmoor.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoBlackmoor.cs`: `0`
- Active overlay rows for `DecoBlackmoor.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0xF79`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-119 DecoBlackmoor Guard Repair`
