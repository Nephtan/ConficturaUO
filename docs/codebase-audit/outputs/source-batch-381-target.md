# SOURCE-BATCH-381 DoomFlayerNote Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-381`
- Candidate: `SB381-CAND-001`
- System: `Items:Books / DoomFlayerNote`
- File: `Data/Scripts/Items/Books/DoomFlayerNote.cs`

## Intended Source Change

Add a local guard to `DoomFlayerNote.OnDoubleClick(Mobile m)` so stale/null interaction state cannot dereference a null/deleted mobile or deleted note before the existing range check, gump creation, and sound behavior run.

Allowed changes:

- Return immediately from `OnDoubleClick` when `m == null || m.Deleted || Deleted`.

## Must Stay Unchanged

DoomFlayerNote item ID/name/weight/hue, `ClueGump` text and layout, range requirement, gump send behavior, sound `0x249` behavior, localized too-far message `502138`, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
