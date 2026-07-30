# SOURCE-BATCH-354 MuseumBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-354`
- Candidate: `SB354-CAND-001`
- System: `Quests:Museum / MuseumBook`
- File: `Data/Scripts/Quests/Museum/MuseumBook.cs`

## Intended Source Change

Add a local guard to `MuseumBook.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, `from.Backpack`, or a deleted source item before the existing owner and gump flow runs.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Item metadata, `ArtOwner` assignment/check behavior, ownership denial message, `MuseumBookGump` type and constructor arguments, antique inventory helpers, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
