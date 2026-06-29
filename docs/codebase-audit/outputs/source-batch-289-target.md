# SOURCE-BATCH-289 Candelabra Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-289`
- Candidate: `SB289-CAND-001`
- System: `Items:Construction / Lights / Candelabra`
- Source file: `Data/Scripts/Items/Construction/Lights/Candelabra.cs`
- Behavior: add a stale/null/mobile/source-item guard to `Candelabra.OnSingleClick(Mobile from)` before base single-click dispatch or shipwreck label display.

## Allowed Source Change

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Valid-mobile base single-click dispatch.
- Shipwreck label/cliloc and `AddNameProperties` behavior.
- `IsShipwreckedItem` property and persistence.
- Light settings and construction metadata.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-289 Candelabra Guard Repair`
