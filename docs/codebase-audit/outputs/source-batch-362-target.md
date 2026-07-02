# SOURCE-BATCH-362 SearchBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-362`
- Candidate: `SB362-CAND-001`
- System: `Quests:Search / SearchBook`
- File: `Data/Scripts/Quests/Search/SearchBook.cs`

## Intended Source Change

Add local guards to `SearchBook.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from.Backpack`, run owner checks, play sounds, close/send gumps, or construct `SearchBookGump`.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.
- Treat `from.Backpack == null` the same as the existing out-of-backpack failure using message `This must be in your backpack to read.`

## Must Stay Unchanged

Owner assignment, `LegendLore`, `AddNameProperties` text, backpack-use message, owner rejection message, sound `0x55`, `CloseGump`/`SendGump` behavior, `SearchBookGump` content and pagination, artifact encyclopedia text, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
