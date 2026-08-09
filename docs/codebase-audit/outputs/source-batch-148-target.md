# SOURCE-BATCH-148 PirateChest Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-148`
- Candidate: `SB144-CAND-005`
- Behavior: add stale/null/mobile/source-container guards to `PirateChest.OnDragLift`.
- System: `Items:Containers / PirateChest`
- File: `Data/Scripts/Items/Containers/PirateChest.cs`

## Fence Result

- POST-BATCH-Y gate hits for `PirateChest.cs`: `0`
- Active overlay rows for `PirateChest.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state delayed-fill behavior when `Weight > 50`.
- `Movable` assignment.
- `FillMeUpLevel = (int)(Weight - 51)` math.
- `GetPlayerInfo.LuckyPlayer(from.Luck)` bonus behavior.
- `ContainerFunctions.FillTheContainer(FillMeUpLevel, this, from)` call.
- Valid-state return remains `true`.
- `ContainerOwner` display and serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-148 PirateChest Guard Repair`
