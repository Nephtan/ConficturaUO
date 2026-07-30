# SOURCE-BATCH-385 StatusBoard Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-385`
- Candidate: `SB385-CAND-001`
- System: `Items:Books / BulletinBoards / StatusBoard`
- File: `Data/Scripts/Items/Books/BulletinBoards/StatusBoard.cs`

## Intended Source Change

Add local guards to `StatusBoard.OnDoubleClick(Mobile e)` and `StatusGump.OnResponse(NetState sender, RelayInfo info)` so stale/null interaction state cannot dereference an invalid mobile, deleted board, null `NetState`, or null `RelayInfo` before the existing range, gump, pagination, and mobile-state behavior runs.

Allowed changes:

- Return immediately from `OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Return immediately from `StatusGump.OnResponse` when `sender == null || info == null`.
- In `StatusGump.OnResponse`, preserve cancel behavior and check `from == null || from.Deleted || from.Map == null` before any further use of `from`.

## Must Stay Unchanged

StatusBoard item ID/name/weight/hue, range requirement, localized too-far message `502138`, `StatusGump` layout, displayed account/client/server values, access-level visibility filtering, pagination behavior, button IDs, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
