# SOURCE-BATCH-097 WaterFlask Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-097`
- Candidate: `SB093-CAND-005`
- Behavior: add stale/null/mobile/source-flask guards to `WaterFlask.OnDoubleClick(Mobile from)` before delegating to `DrinkingFunctions.OnDrink`.
- System: `Items:Food / WaterFlask`
- Source file: `Data/Scripts/Items/Food/WaterFlask.cs`

## Fence Result

- POST-BATCH-Y gate hits for `WaterFlask.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/WaterFlask.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source flask is deleted.
- Preserve delegation to `Server.Items.DrinkingFunctions.OnDrink(this, from)` for all valid state.

## Must Stay Unchanged

- `DrinkingFunctions.OnDrink` delegation.
- Thirst behavior inside `DrinkingFunctions`, consume behavior, and item reset in `Deserialize`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
