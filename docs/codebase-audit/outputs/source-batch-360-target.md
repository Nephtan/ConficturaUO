# SOURCE-BATCH-360 QuestChests Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-360`
- Candidate: `SB360-CAND-001`
- System: `Quests:QuestChests / Bards Tale, Undermountain, Skull Gate, Serpent Pillars, Dragon Riding`
- File: `Data/Scripts/Quests/QuestChests.cs`

## Intended Source Change

Add local guards to each quest chest/book `OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, read or write quest flags, send messages or gumps, play sounds, place reward items, or delete the source item.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Quest chest/book item IDs, names, hues, `Movable` settings, BardsTale and key flag names, `PlayerSettings` calls, `PrivateOverheadMessage` text, `ClueGump` text/titles, localized message `502138`, sound IDs, `CrystalStatueBoxKyl` placement behavior, `DragonRidingScroll` delete behavior, `ItemRemovalTimer` behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
