# SOURCE-BATCH-398 BaseHat Cowl Hood Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-398`
- Candidate: `SB398-CAND-001`
- System: `Items:Clothing / Hats / BaseHat cowl hood color matching`
- File: `Data/Scripts/Items/Clothing/Hats.cs`
- Behavior: add stale/null mobile, deleted source-hat, stale target callback, and deleted target-item guard coverage around `BaseHat.OnDoubleClick` and `HatTarget.OnTarget`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- BaseHat inherited serialization behavior.
- Cowl-hood-only activation.
- Equipped hat requirement.
- Target range.
- Eligible layer list.
- Distinct-color requirement.
- Hue assignment behavior.
- Existing messages.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
