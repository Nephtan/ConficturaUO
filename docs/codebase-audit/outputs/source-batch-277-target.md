# SOURCE-BATCH-277 BankChest Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-277`
- Candidate: `SB277-CAND-001`
- System: `Items:Containers / BankChest`
- Source file: `Data/Scripts/Items/Containers/BankChest.cs`
- Behavior: add a stale/null/mobile/source-item guard to `BankChest.OnDoubleClick(Mobile from)` before range checking and bank-box access.

## Allowed Source Change

- In `BankChest.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Range `4` access rule.
- `BankBox` lookup and `Open()` behavior.
- Too-far localized message `502138`.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, bank access policy, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-277 BankChest Guard Repair`
