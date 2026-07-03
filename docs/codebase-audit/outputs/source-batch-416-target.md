# SOURCE-BATCH-416 HouseSign Rename Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-416`
- Candidate: `SB416-CAND-001`
- System: `Items:Decorations / HouseSign rename signs`
- File: `Data/Scripts/Items/Decorations/HouseSign.cs`
- Behavior: add stale/null mobile and deleted source-sign guard coverage around the HouseSign rename `OnDoubleClick` entry points.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Current rename access policy.
- Sign item IDs, names, weights, and movable flags.
- Prompt text.
- Prompt assignment.
- Prompt response validation.
- Name assignment.
- Confirmation message.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
