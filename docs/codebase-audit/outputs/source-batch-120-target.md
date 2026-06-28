# SOURCE-BATCH-120 DecoBloodspawn Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-120`
- Candidate: `SB119-CAND-002`
- Behavior: add stale/null/mobile/source-item guards to `DecoBloodspawn.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoBloodspawn`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoBloodspawn.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoBloodspawn.cs`: `0`
- Active overlay rows for `DecoBloodspawn.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0xF7C`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-120 DecoBloodspawn Guard Repair`
