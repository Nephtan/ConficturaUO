# SOURCE-BATCH-293 BaseWaterContainer Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-293`
- Candidate: `SB293-CAND-001`
- System: `Items:Special / Rares / Containers / BaseWaterContainer`
- Source file: `Data/Scripts/Items/Special/Rares/Containers/BaseWaterContainer.cs`
- Behavior: add stale/null/mobile/source-container/drop-item guards to `BaseWaterContainer.OnDoubleClick`, `BaseWaterContainer.OnSingleClick`, and `BaseWaterContainer.OnDragDropInto` before forwarding to base container behavior.

## Allowed Source Changes

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDragDropInto(Mobile from, Item item, Point3D p)`, return `false` when `from == null || from.Deleted || item == null || item.Deleted || Deleted`.

## Must Stay Unchanged

- Quantity clamping and `IsEmpty`/`IsFull` behavior.
- `Movable` and `ItemID` updates.
- `DefaultGumpID`.
- Empty-container base forwarding behavior.
- The current `OnSingleClick` call to `base.OnDoubleClick(from)`.
- Full-container drag/drop rejection.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-293 BaseWaterContainer Guard Repair`
