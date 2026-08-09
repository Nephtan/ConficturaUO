# SOURCE-BATCH-410 BaseBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-410`
- Candidate: `SB410-CAND-001`
- System: `Items:Books / BaseBook double-click display`
- File: `Data/Scripts/Items/Books/BaseBook.cs`
- Behavior: add stale/null mobile and deleted source-book guard coverage around `BaseBook.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- BaseBook title/author defaults.
- Writable-book author initialization.
- `BookHeader` send.
- `BookPageDetails` send.
- Packet handler behavior.
- Content editing.
- Secure-level behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
