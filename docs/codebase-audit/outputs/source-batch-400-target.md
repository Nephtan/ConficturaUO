# SOURCE-BATCH-400 BaseKnife Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-400`
- Candidate: `SB400-CAND-001`
- System: `Items:Weapons / Knives / BaseKnife bladed-item use`
- File: `Data/Scripts/Items/Weapons/Knives/BaseKnife.cs`
- Behavior: add stale/null mobile and deleted source-knife guard coverage around `BaseKnife.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- BaseKnife combat defaults.
- Weapon skill/type/animation.
- Hit and miss sounds.
- Localized prompt `1010018`.
- `BladedItemTarget` assignment.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
