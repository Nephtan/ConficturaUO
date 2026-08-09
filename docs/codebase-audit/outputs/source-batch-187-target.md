# SOURCE-BATCH-187 AncientSpellbook Guard Repair

## Target

- Candidate: SB187-CAND-001
- System: Magic:Research / AncientSpellbook
- File: Data/Scripts/Magic/Research/AncientSpellBook.cs
- Behavior: add a stale/null/mobile/source-book guard to AncientSpellbook.OnDoubleClick(Mobile from) before reading from.Backpack or opening the gump.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the spellbook is deleted.

## Must Stay Unchanged

- Owner logic
- Backpack requirement
- AncientSpellbookGump open flow and sound
- Scribbles and backpack messages
- Owner/paper/quill/names serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state
