# SOURCE-BATCH-282 HiveTool Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-282`
- Candidate: `SB282-CAND-001`
- System: `Trades:Apiculture / HiveTool`
- Source file: `Data/Scripts/Trades/Apiculture/Items/HiveTool.cs`
- Behavior: add stale/null/mobile/source-item guards to `HiveTool.DisplayDurabilityTo(Mobile m)`, `OnSingleClick(Mobile from)`, and `OnDoubleClick(Mobile from)` before durability label display, base single-click dispatch, overhead messaging, or `NetState` access.

## Allowed Source Change

- In `DisplayDurabilityTo(Mobile m)`, return immediately when `m == null || m.Deleted || Deleted`.
- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Durability label text and cliloc.
- Valid-mobile single-click base dispatch.
- Double-click private overhead message text, hue, and `NetState` target.
- `UsesRemaining` property behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-282 HiveTool Guard Repair`
