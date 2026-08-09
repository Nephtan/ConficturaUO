# SOURCE-BATCH-125 DecoGarlic Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-125`
- Candidate: `SB119-CAND-007`
- Behavior: add stale/null/mobile/source-item guards to `DecoGarlic.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoGarlic`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoGarlic.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoGarlic.cs`: `0`
- Active overlay rows for `DecoGarlic.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x18E1`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-125 DecoGarlic Guard Repair`
