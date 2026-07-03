# SOURCE-BATCH-419 DartBoards Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-419`
- Candidate: `SB419-CAND-001`
- System: `Items:Gifts / Holiday / Halloween / DartBoards`
- Files:
  - `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/MongbatDartBoard.cs`
  - `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/DaemonDartBoard.cs`
- Behavior: add stale/null mobile, deleted source dart-board, and delayed-callback deleted-source guard coverage around Halloween dart board interactions.

## Fence

- POST-BATCH-Y exact-file gate hits: `0` for both files.
- Active overlay rows: `0` for both files.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Scoring thresholds and localized score messages.
- `BaseKnife`-only eligibility.
- Range, line-of-sight, and facing checks.
- Animation choice.
- Moving effect behavior.
- Sound behavior.
- Timer delays.
- Item ID hit/reset behavior.
- Addon and deed definitions.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
