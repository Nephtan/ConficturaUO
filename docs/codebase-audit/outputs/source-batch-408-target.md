# SOURCE-BATCH-408 TitleChangeDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-408`
- Candidate: `SB408-CAND-001`
- System: `Items:Books / TitleChangeDeed`
- File: `Data/Scripts/Items/Books/TitleChangeDeed.cs`
- Behavior: add stale/null mobile, deleted source-deed, and missing-backpack guard coverage around `TitleChangeDeed.OnDoubleClick`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- TitleChangeDeed item ID/name/weight.
- Backpack-use localized message.
- Title prompt text.
- Prompt assignment.
- Deed `Delete` semantics.
- `RetitlePrompt` stale-response guards.
- Title assignment.
- Title confirmation message.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
