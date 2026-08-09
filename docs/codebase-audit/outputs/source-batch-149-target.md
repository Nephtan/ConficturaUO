# SOURCE-BATCH-149 SunkenBag Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-149`
- Candidate: `SB144-CAND-006`
- Behavior: add stale/null/mobile/source-container guards to `SunkenBag.OnDragLift`.
- System: `Items:Containers / SunkenBag`
- File: `Data/Scripts/Items/Containers/SunkenBag.cs`

## Fence Result

- POST-BATCH-Y gate hits for `SunkenBag.cs`: `0`
- Active overlay rows for `SunkenBag.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Random naming.
- Valid-state delayed-fill behavior when `Weight > 10`.
- `Movable` assignment.
- `FillMeUpLevel = (int)(Weight - 11)` math.
- Valid-state reset to `Weight = 2.0`.
- `GetPlayerInfo.LuckyPlayer(from.Luck)` bonus behavior.
- `ContainerFunctions.FillTheContainer(FillMeUpLevel, this, from)` call.
- Valid-state return remains `true`.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-149 SunkenBag Guard Repair`
