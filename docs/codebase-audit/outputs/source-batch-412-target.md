# SOURCE-BATCH-412 Watcher Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-412`
- Candidate: `SB412-CAND-001`
- System: `Mobiles:Unusual / Watcher pack animal`
- File: `Data/Scripts/Mobiles/Unusual/Watcher.cs`
- Behavior: add stale/null mobile and deleted source-mobile guard coverage around `Watcher.OnDoubleClick` and `Watcher.GetContextMenuEntries`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Watcher stats.
- Breath behavior.
- Taming settings.
- Pack creation.
- Pack animal access policy.
- Drag/drop behavior.
- Death inventory behavior.
- Meat/hides/favorite food.
- `PackAnimal.TryPackOpen` delegation.
- `PackAnimal.GetContextMenuEntries` delegation.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
