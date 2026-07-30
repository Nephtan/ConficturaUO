# SOURCE-BATCH-405 ThrowingDagger Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-405`
- Candidate: `SB405-CAND-001`
- System: `Items:Weapons / Knives / ThrowingDagger harmful throw`
- File: `Data/Scripts/Items/Weapons/Knives/ThrowingDagger.cs`
- Behavior: add stale/null mobile, deleted source-dagger, and deleted target-mobile guard coverage around `ThrowingDagger.OnDoubleClick` and `ThrowingDagger.InternalTarget.OnTarget`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- ThrowingDagger default name, item ID, weight, and layer.
- Held-weapon requirement.
- Target range.
- `TargetFlags.Harmful`.
- `HarmfulCheck`.
- Direction and animation behavior.
- Hit chance and damage calculation.
- Dagger `MoveToWorld` behavior.
- Moving effects.
- Miss message.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
