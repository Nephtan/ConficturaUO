# SOURCE-BATCH-146 LootBag Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-146`
- Candidate: `SB144-CAND-003`
- Behavior: add stale/null/mobile/source-container guards to `LootBag.OnDragLift`.
- System: `Items:Containers / LootBag`
- File: `Data/Scripts/Items/Containers/LootBag.cs`

## Fence Result

- POST-BATCH-Y gate hits for `LootBag.cs`: `0`
- Active overlay rows for `LootBag.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state delayed-fill behavior when `Weight > 10`.
- `Movable` assignment.
- `FillMeUpLevel = (int)(Weight - 11)` math.
- `GetPlayerInfo.LuckyPlayer(from.Luck)` bonus behavior.
- `ContainerFunctions.FillTheContainer(FillMeUpLevel, this, from)` call.
- Valid-state return remains `true`.
- Constructor state and serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-146 LootBag Guard Repair`
