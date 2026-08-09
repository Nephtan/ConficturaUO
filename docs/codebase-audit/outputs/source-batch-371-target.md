# SOURCE-BATCH-371 DDRelicLight Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-371`
- Candidate: `SB371-CAND-001`
- System: `Items:Relics / DDRelicLight`
- File: `Data/Scripts/Items/Relics/DDRelicLight.cs`

## Intended Source Change

Add local guards to the three `DDRelicLight` relic-light `OnDoubleClick(Mobile from)` methods so stale/null interaction state cannot send the existing informational relic-identification messages through a null or deleted mobile, or from deleted source relic items.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

`RelicGoldValue`, `BaseLight` lit/unlit IDs, `Duration`, `BurntOut`, `Burning`, `Light`, randomized names, the informational messages, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
