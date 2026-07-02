# SOURCE-BATCH-396 Tarot Poker Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-396`
- Candidate: `SB396-CAND-001`
- System: `Items:Misc / Games / Tarot Poker`
- File: `Data/Scripts/Items/Misc/Games/tarotpoker.cs`

## Intended Source Change

Add a local guard to `tarotpoker.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or use a deleted source deck before the existing range, gump, card text, and betting-instruction behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

tarotpoker item IDs, `Flipable` behavior, name, weight, `Stackable = false`, `IsNoisy` command property behavior, range 4 rule, card randomization, `TarotGump` image IDs, public overhead message text including betting instructions, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
