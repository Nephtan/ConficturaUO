# SOURCE-BATCH-122 DecoDragonsBlood Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-122`
- Candidate: `SB119-CAND-004`
- Behavior: add stale/null/mobile/source-item guards to `DecoDragonsBlood.OnDragLift`.
- System: `Items:Special / Rares / PaganReagents / DecoDragonsBlood`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoDragonsBlood.cs`

## Fence Result

- POST-BATCH-Y gate hits for `DecoDragonsBlood.cs`: `0`
- Active overlay rows for `DecoDragonsBlood.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state collectible message.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0x4077`, `Movable`, and `Stackable` state.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-122 DecoDragonsBlood Guard Repair`
