# SOURCE-BATCH-382 BardsTaleNote Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-382`
- Candidate: `SB382-CAND-001`
- System: `Items:Books / BardsTaleNote`
- File: `Data/Scripts/Items/Books/BardsTaleNote.cs`

## Intended Source Change

Add local guards to `BardsTaleNote.OnDoubleClick(Mobile e)` so stale/null interaction state cannot dereference a null/deleted mobile, deleted note, or missing backpack before the existing backpack-use check, gump creation, and sound behavior run.

Allowed changes:

- Return immediately from `OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Treat `e.Backpack == null` as the existing backpack-use failure path with message `This must be in your backpack to read.`

## Must Stay Unchanged

BardsTaleNote randomized item ID/name/hue/weight, clue message choices, `ScrollMessage` persistence, `ClueGump` text and layout, backpack-use failure message, gump close/send behavior, sound `0x249` behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
