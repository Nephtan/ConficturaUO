# SOURCE-BATCH-276 BlankMap Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-276`
- Candidate: `SB276-CAND-001`
- System: `Items:Trades / Maps / BlankMap`
- Source file: `Data/Scripts/Items/Trades/Maps/BlankMap.cs`
- Behavior: add a stale/null/mobile/source-item guard to `BlankMap.OnDoubleClick(Mobile from)` before sending the blank-map localized message.

## Allowed Source Change

- In `BlankMap.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Localized blank-map message `500208`.
- `MapItem` inheritance and blank-map behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, map policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-276 BlankMap Guard Repair`
