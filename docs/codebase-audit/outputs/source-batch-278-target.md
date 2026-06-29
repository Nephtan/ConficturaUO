# SOURCE-BATCH-278 DDRelicBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-278`
- Candidate: `SB278-CAND-001`
- System: `Items:Relics / DDRelicBook`
- Source file: `Data/Scripts/Items/Relics/DDRelicBook.cs`
- Behavior: add a stale/null/mobile/source-item guard to `DDRelicBook.OnDoubleClick(Mobile from)` before sending the relic-book message.

## Allowed Source Change

- In `DDRelicBook.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Relic-book message text.
- `RelicGoldValue` calculation/storage.
- Randomized book `ItemID`, `Hue`, and `Name` construction.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, relic value policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-278 DDRelicBook Guard Repair`
