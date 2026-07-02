# SOURCE-BATCH-395 Tarot Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-395`
- Candidate: `SB395-CAND-001`
- System: `Items:Misc / Games / Tarot`
- File: `Data/Scripts/Items/Misc/Games/Tarot.cs`

## Intended Source Change

Add local guards to `TarotDeck.OnDoubleClick(Mobile from)` and `DecoTarotDeck.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or mutate deleted source decks.

Allowed change:

- Return immediately from both `OnDoubleClick` methods when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

TarotDeck and DecoTarotDeck item IDs, `Flipable` behavior, fortune randomization, fortune text/image mappings, `PublicOverheadMessage` content, `TarotGump` construction, open/closed item-ID toggles, `OnAdded` normalization, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
