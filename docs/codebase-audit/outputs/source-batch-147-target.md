# SOURCE-BATCH-147 LootChest Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-147`
- Candidate: `SB144-CAND-004`
- Behavior: add stale/null/mobile/source-container guards to `LootChest.OnDragLift`.
- System: `Items:Containers / LootChest`
- File: `Data/Scripts/Items/Containers/LootChest.cs`

## Fence Result

- POST-BATCH-Y gate hits for `LootChest.cs`: `0`
- Active overlay rows for `LootChest.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state delayed-fill behavior when `Weight > 50`.
- `Movable` assignment.
- `FillMeUpLevel = (int)(Weight - 51)` math.
- `GetPlayerInfo.LuckyPlayer(from.Luck)` bonus behavior.
- `ContainerFunctions.FillTheContainer(FillMeUpLevel, this, from)` call.
- Valid-state return remains `true`.
- Constructor state and serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-147 LootChest Guard Repair`
