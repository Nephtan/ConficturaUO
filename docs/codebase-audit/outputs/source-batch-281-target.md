# SOURCE-BATCH-281 FoodChest Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-281`
- Candidate: `SB281-CAND-001`
- System: `Items:Containers / FoodChest`
- Source file: `Data/Scripts/Items/Containers/FoodChest.cs`
- Behavior: add a stale/null/mobile/source-item guard to `FoodChest.OnDoubleClick(Mobile from)` before range checks, cooldown checks, food creation, backpack insertion, or messages.

## Allowed Source Change

- In `FoodChest.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range checks and too-far localized message.
- One-minute refill cooldown behavior.
- Random generated food choices and item amount ranges.
- Backpack insertion behavior.
- User-facing food and wait messages.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-281 FoodChest Guard Repair`
