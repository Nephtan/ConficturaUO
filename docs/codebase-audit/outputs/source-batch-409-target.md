# SOURCE-BATCH-409 BaseStatue Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-409`
- Candidate: `SB409-CAND-001`
- System: `Trades:Stone / BaseStatue rename prompt`
- File: `Data/Scripts/Trades/Stone/BaseStatue.cs`
- Behavior: add stale/null mobile, deleted source-statue, null text, and stale prompt-response guard coverage around `BaseStatue.OnDoubleClick` and `RenamePrompt.OnResponse`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- BaseStatue crafter/resource properties.
- Material display.
- Color mapping.
- Rename prompt text.
- Prompt assignment.
- Name assignment.
- Confirmation message.
- `IsNotGraveStone` behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
