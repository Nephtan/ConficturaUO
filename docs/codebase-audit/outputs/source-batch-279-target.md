# SOURCE-BATCH-279 DDRelicAlchemy Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-279`
- Candidate: `SB279-CAND-001`
- System: `Items:Relics / DDRelicAlchemy`
- Source file: `Data/Scripts/Items/Relics/DDRelicAlchemy.cs`
- Behavior: add a stale/null/mobile/source-item guard to `DDRelicAlchemy.OnDoubleClick(Mobile from)` before sending the relic-identification message.

## Allowed Source Change

- In `DDRelicAlchemy.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Relic-identification message text.
- `RelicGoldValue` calculation/storage.
- Randomized alchemy flask `ItemID` and `Name` construction.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, relic value policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-279 DDRelicAlchemy Guard Repair`
