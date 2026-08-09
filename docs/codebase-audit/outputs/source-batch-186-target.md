# SOURCE-BATCH-186 JediSpellbook Guard Repair

## Target

- Candidate: SB183-CAND-004
- System: Magic:Jedi / JediSpellbook
- File: Data/Scripts/Magic/Jedi/JediSpellbook.cs
- Behavior: add a stale/null/mobile/source-book guard to JediSpellbook.OnDoubleClick(Mobile from) before reading from.Backpack or opening the gump.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the spellbook is deleted.

## Must Stay Unchanged

- Owner logic
- Backpack requirement
- JediSpellbookGump open flow and sound
- Strange-device and backpack messages
- OnDragDrop transformation behavior
- Owner/crystals/page/names/gem/steel serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state
