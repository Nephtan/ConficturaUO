# SOURCE-BATCH-379 GypsyShelf Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-379`
- Candidate: `SB379-CAND-001`
- System: `Items:Containers / GypsyShelf`
- File: `Data/Scripts/Items/Containers/GypsyShelf.cs`

## Intended Source Change

Add a local guard to `GypsyShelf.OnDoubleClick(Mobile from)` so stale/null interaction state cannot read `from.Backpack` or grant a guide book through a null/deleted mobile, deleted source shelf, or missing backpack.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted || from.Backpack == null`.

## Must Stay Unchanged

Shelf item ID/name, duplicate-book message, `GetRidOf` behavior, `BookGuideToAdventure` owner assignment, sound `0x02E`, `AddToBackpack` behavior, grant message, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
