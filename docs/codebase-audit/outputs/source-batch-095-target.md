# SOURCE-BATCH-095 TastyHeart Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-095`
- Candidate: `SB093-CAND-003`
- Behavior: add stale/null/mobile/backpack/source-food guards to `TastyHeart.OnDoubleClick(Mobile from)`.
- System: `Items:Food / TastyHeart`
- Source file: `Data/Scripts/Items/Food/TastyHeart.cs`

## Fence Result

- POST-BATCH-Y gate hits for `TastyHeart.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/TastyHeart.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source food is deleted.
- Treat missing backpacks and out-of-backpack source food as the existing backpack-use failure using message `This must be in your backpack to use.`

## Must Stay Unchanged

- Backpack failure message.
- `BloodDrinker` and `BrainEater` race branches.
- `Hunger += 3` and `Thirst += 3` special-race behavior.
- Consumption, random eat sound, human animation, Tasting heal, and `AwardKarma(from, -50, true)` behavior.
- `HeartName` serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
