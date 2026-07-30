# SOURCE-BATCH-291 UncutCloth Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-291`
- Candidate: `SB291-CAND-001`
- System: `Items:Trades / Tailor Resources / UncutCloth`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs`
- Behavior: add a stale/null/mobile/source-item guard to `UncutCloth.OnSingleClick(Mobile from)` before localized folded-cloth label packet send.

## Allowed Source Change

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Localized label numbers and amount string.
- `MessageLocalized` packet values.
- `ICommodity` behavior.
- Existing `Dye` and `Scissor` behavior and guards.
- Construction metadata, folded-cloth name, default weight, and stack behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-291 UncutCloth Guard Repair`
