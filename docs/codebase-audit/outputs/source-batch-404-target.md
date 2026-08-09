# SOURCE-BATCH-404 LevelThrowingGloves Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-404`
- Candidate: `SB404-CAND-001`
- System: `Items:Magical / God / Weapons / LevelThrowingGloves type cycling`
- File: `Data/Scripts/Items/Magical/God/Weapons/LevelThrowingGloves.cs`
- Behavior: add stale/null mobile, deleted source-gloves, and missing-backpack guard coverage around `LevelThrowingGloves.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- LevelThrowingGloves defaults.
- Weapon skill/type/animation.
- Glove type cycling order.
- Backpack-use message.
- Final glove-change message.
- `InvalidateProperties` call.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
