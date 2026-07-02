# SOURCE-BATCH-365 DDRelicArts Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-365`
- Candidate: `SB365-CAND-001`
- System: `Items:Relics / DDRelicArts`
- File: `Data/Scripts/Items/Relics/DDRelicArts.cs`

## Intended Source Change

Add a local guard to `DDRelicArts.OnDoubleClick(Mobile from)` so stale/null interaction state cannot send the existing informational relic-identification message through a null or deleted mobile, or from a deleted source relic item.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

`RelicGoldValue`, randomized item IDs, randomized hue/name/weight setup, the informational message, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
