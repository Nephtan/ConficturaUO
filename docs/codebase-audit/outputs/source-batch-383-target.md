# SOURCE-BATCH-383 AssassinNote Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-383`
- Candidate: `SB383-CAND-001`
- System: `Items:Books / BulletinBoards / AssassinNote`
- File: `Data/Scripts/Items/Books/BulletinBoards/AssassinNote.cs`

## Intended Source Change

Add a local guard to `AssassinNote.OnDoubleClick(Mobile e)` so stale/null interaction state cannot dereference a null/deleted mobile or deleted note before the existing range, visibility, line-of-sight, gump creation, and sound behavior run.

Allowed changes:

- Return immediately from `OnDoubleClick` when `e == null || e.Deleted || Deleted`.

## Must Stay Unchanged

AssassinNote item ID/name/weight/hue, `LetterMessage` persistence, `KillGump` text and layout, range/visibility/line-of-sight requirement, gump close/send behavior, sound `0x249` behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
