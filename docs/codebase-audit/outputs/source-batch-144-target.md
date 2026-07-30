# SOURCE-BATCH-144 CorpseChest Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-144`
- Candidate: `SB144-CAND-001`
- Behavior: add stale/null/mobile/source-container guards to `CorpseChest.OnDragLift`.
- System: `Items:Containers / CorpseChest`
- File: `Data/Scripts/Items/Containers/CorpseChest.cs`

## Fence Result

- POST-BATCH-Y gate hits for `CorpseChest.cs`: `0`
- Active overlay rows for `CorpseChest.cs`: `0`
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

`/goal SOURCE-BATCH-144 CorpseChest Guard Repair`
