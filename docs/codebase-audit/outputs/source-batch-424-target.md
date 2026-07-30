# SOURCE-BATCH-424 HairDye Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-424`
- Candidate: `SB424-CAND-001`
- System: `Items:Misc / HairDye`
- File: `Data/Scripts/Items/Misc/HairDye.cs`
- Behavior: add stale/null mobile, deleted source dye, and stale gump response guards to HairDye interactions.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Range requirement.
- `HairDyeGump` layout.
- Hue entry table.
- Selected hue calculation.
- No-hair rejection.
- Backpack requirement.
- Hair and facial hair hue assignment.
- Dye `Delete` behavior.
- Localized messages `501199`, `501200`, `502623`, and `1042010`.
- Sound `0x4E`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
