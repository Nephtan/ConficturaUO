# SOURCE-BATCH-414 HintItem Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-414`
- Candidate: `SB414-CAND-001`
- System: `Items:Misc / WarningItem HintItem`
- File: `Data/Scripts/Items/Misc/WarningItem.cs`
- Behavior: add stale/null mobile and deleted source-item guard coverage around `HintItem.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- `WarningItem` movement broadcast behavior.
- Pooled enumerable ownership.
- Neighbor broadcast behavior.
- `HintItem` message dispatch.
- Warning/hint string and number persistence.
- Range/reset behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
