# SOURCE-BATCH-401 BaseSword Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-401`
- Candidate: `SB401-CAND-001`
- System: `Items:Weapons / Swords / BaseSword bladed-item use`
- File: `Data/Scripts/Items/Weapons/Swords/BaseSword.cs`
- Behavior: add stale/null mobile and deleted source-sword guard coverage around `BaseSword.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- BaseSword combat defaults.
- Weapon skill/type/animation.
- Localized prompt `1010018`.
- `BladedItemTarget` assignment.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
