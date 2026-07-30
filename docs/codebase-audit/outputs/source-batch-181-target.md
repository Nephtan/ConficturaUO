# SOURCE-BATCH-181 DeathKnightSpellbook Guard Repair

## Target

- Candidate: SB179-CAND-003
- System: Magic:Death Knight / DeathKnightSpellbook
- File: Data/Scripts/Magic/Death Knight/DeathKnightSpellBook.cs
- Behavior: add a stale/null/mobile/source-book guard to DeathKnightSpellbook.OnDoubleClick(Mobile from) before reading from.Backpack or opening the gump.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the spellbook is deleted.

## Must Stay Unchanged

- Owner logic
- Backpack requirement
- Gump open flow and sound
- Scribbles and backpack messages
- Owner serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state