# SOURCE-BATCH-386 Clock Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-386`
- Candidate: `SB386-CAND-001`
- System: `Items:Trades / Tinkering / Clock`
- File: `Data/Scripts/Items/Trades/Tinkering/Clocks.cs`

## Intended Source Change

Add a local guard to `Clock.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or deleted clock before the existing time calculation and localized message behavior runs.

Allowed changes:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Clock item IDs, flipable variants, weight, server start initialization, time and moon phase calculations, localized message IDs `1042950` through `1042958`, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
