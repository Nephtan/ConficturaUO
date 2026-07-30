# SOURCE-BATCH-403 ThrowingGloves Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-403`
- Candidate: `SB403-CAND-001`
- System: `Items:Weapons / Marksman / ThrowingGloves type cycling`
- File: `Data/Scripts/Items/Weapons/Marksman/ThrowingGloves.cs`
- Behavior: add stale/null mobile, deleted source-gloves, and missing-backpack guard coverage around `ThrowingGloves.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- ThrowingGloves defaults.
- Weapon skill/type/animation.
- Glove type cycling order.
- Jester-only card/tomato branches.
- Backpack-use message.
- Final glove-change message.
- `InvalidateProperties` call.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
