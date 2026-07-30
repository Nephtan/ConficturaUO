# SOURCE-BATCH-393 PlayersHandbook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-393`
- Candidate: `SB393-CAND-001`
- System: `Items:Misc / Games / DandD / PlayersHandbook`
- File: `Data/Scripts/Items/Misc/Games/DandD/PlayersHandbook.cs`

## Intended Source Change

Add local guards to `PlayersHandbook.OnDoubleClick(Mobile from)` and `BookTarget.OnTarget(Mobile from, object targeted)` so stale/null interaction state cannot dereference an invalid mobile, use a deleted source book, or open lookup gumps for deleted target mobiles.

Allowed changes:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Return immediately from `BookTarget.OnTarget` when `from == null || from.Deleted || m_Book == null || m_Book.Deleted`.
- Treat deleted targeted mobiles as the existing invalid-target outcome using `"That doesn't seem to be in this book."`.

## Must Stay Unchanged

PlayersHandbook item ID, name, weight, Dungeons & Dragons property label, target range, people classification rules, target eligibility messages, `StatsGump` construction, `DruidismGump` construction, sound `0x55` behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
