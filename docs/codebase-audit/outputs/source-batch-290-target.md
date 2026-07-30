# SOURCE-BATCH-290 BoltOfCloth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-290`
- Candidate: `SB290-CAND-001`
- System: `Items:Trades / Tailor Resources / BoltOfCloth`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/BoltOfCloth.cs`
- Behavior: add a stale/null/mobile/source-item guard to `BoltOfCloth.OnSingleClick(Mobile from)` before localized cloth-quantity label packet send.

## Allowed Source Change

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Localized label numbers and amount multiplier.
- `MessageLocalized` packet values.
- `ICommodity` behavior.
- Existing `Dye` and `Scissor` behavior and guards.
- Construction metadata and stack behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-290 BoltOfCloth Guard Repair`
