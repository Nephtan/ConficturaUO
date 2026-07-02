# SOURCE-BATCH-380 Safe Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-380`
- Candidate: `SB380-CAND-001`
- System: `Items:Containers / Safe`
- File: `Data/Scripts/Items/Containers/Safe.cs`

## Intended Source Change

Add local guards to `Safe.OnDoubleClick(Mobile from)` and `Safe.CheckAccess(Mobile m)` so stale/null interaction state cannot dereference a null/deleted mobile or deleted safe before the existing secure-access and bank-box behavior runs.

Allowed changes:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Return `false` from `CheckAccess` when `m == null || m.Deleted || Deleted`.

## Must Stay Unchanged

Safe item ID/name/weight, movable-secured failure message, range/visibility/line-of-sight checks and messages, house secure-access policy, `BankBox.Open` behavior, context menu behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
