# SOURCE-BATCH-286 FestiveCactus Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-286`
- Candidate: `SB286-CAND-001`
- System: `Items:Gifts / Holiday / Christmas / FestiveCactus`
- Source file: `Data/Scripts/Items/Gifts/Holiday/Christmas/Christmas Gifts/FestiveCactus.cs`
- Behavior: add a stale/null/mobile/source-item guard to `FestiveCactus.OnSingleClick(Mobile from)` before base single-click dispatch or Winter 2004 label display.

## Allowed Source Change

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Valid-mobile base single-click dispatch.
- Winter 2004 label/cliloc and `GetProperties` behavior.
- Construction metadata, including item ID, weight, and blessed loot type.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-286 FestiveCactus Guard Repair`
