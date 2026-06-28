# SOURCE-BATCH-096 BakedBread Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-096`
- Candidate: `SB093-CAND-004`
- Behavior: add stale/null/mobile/backpack/source-food guards to `BakedBread.OnDoubleClick(Mobile from)`.
- System: `Items:Food / BakedBread`
- Source file: `Data/Scripts/Items/Food/BakedBread.cs`

## Fence Result

- POST-BATCH-Y gate hits for `BakedBread.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/BakedBread.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source food is deleted.
- Treat missing backpacks and out-of-backpack source food as the existing backpack-use failure using message `This must be in your backpack to use.`

## Must Stay Unchanged

- Backpack failure message.
- `BloodDrinker` and `BrainEater` rejection.
- `Hunger += 3`, localized food messages, full-state behavior, consumption, random eat sound, human animation, Tasting heal, and poison-cure behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
