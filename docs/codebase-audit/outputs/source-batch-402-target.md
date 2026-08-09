# SOURCE-BATCH-402 ThrowingWeapon Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-402`
- Candidate: `SB402-CAND-001`
- System: `Items:Weapons / Marksman / ThrowingWeapon ammo cycling`
- File: `Data/Scripts/Items/Weapons/Marksman/ThrowingWeapon.cs`
- Behavior: add stale/null mobile, deleted source-item, and missing-backpack guard coverage around `ThrowingWeapon.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- ThrowingWeapon randomized setup.
- Ammo labels and cycling order.
- Jester-only card/tomato branches.
- Item ID/name changes.
- Backpack-use message.
- Final ammo-change message.
- `InvalidateProperties` call.
- OnMoveOver pickup behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
