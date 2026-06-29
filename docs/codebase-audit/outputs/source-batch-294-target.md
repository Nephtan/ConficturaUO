# SOURCE-BATCH-294 PandorasBox Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-294`
- Candidate: `SB294-CAND-001`
- System: `Items:Magical / Artifacts / Minor / PandorasBox`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Minor/PandorasBox.cs`
- Behavior: add stale/null/mobile/source-item guards to `PandorasBox.ConsumeCharge(Mobile from)` and `PandorasBox.OnDoubleClick(Mobile from)` before charge decrement, replacement item creation, backpack add, or bank-box access.

## Allowed Source Changes

- In `ConsumeCharge(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- `Charges` property semantics.
- `ConsumeCharge` decrement behavior.
- Zero-charge localized message `1019073`.
- `MetalBox` replacement, hue `0x492`, backpack add, and source item `Delete`.
- Valid-state `BankBox.Open` behavior.
- Artifact label and property text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
- Do not add backpack, range, ownership, access-level, or policy restrictions.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-294 PandorasBox Guard Repair`
