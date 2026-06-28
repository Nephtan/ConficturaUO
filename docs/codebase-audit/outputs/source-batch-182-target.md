# SOURCE-BATCH-182 HolyManSpellbook Guard Repair

## Target

- Candidate: SB179-CAND-004
- System: Magic:Holy Man / HolyManSpellbook
- File: Data/Scripts/Magic/Holy Man/HolyManSpellBook.cs
- Behavior: add a stale/null/mobile/source-book guard to HolyManSpellbook.OnDoubleClick(Mobile from) before reading from.Backpack or opening the gump.

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
