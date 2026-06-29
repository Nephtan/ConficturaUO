# SOURCE-BATCH-269 RomulanAle Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-269`
- Candidate: `SB269-CAND-001`
- System: `Items:Technology / RomulanAle`
- Source file: `Data/Scripts/Items/Technology/RomulanAle.cs`
- Behavior: add a stale/null/mobile/source-item guard to `RomulanAle.OnDoubleClick(Mobile from)` before passing the drink item and mobile to `DrinkingFunctions.OnDrink`.

## Allowed Source Change

- In `RomulanAle.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- `Stackable`, `Name`, `Weight`, and `Hue` setup.
- `Server.Items.DrinkingFunctions.OnDrink(this, from)` drink handoff behavior.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-269 RomulanAle Guard Repair`
