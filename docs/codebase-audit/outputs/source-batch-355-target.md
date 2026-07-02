# SOURCE-BATCH-355 ThiefNote Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-355`
- Candidate: `SB355-CAND-001`
- System: `Quests:Thief / ThiefNote`
- File: `Data/Scripts/Quests/Thief/ThiefNote.cs`

## Intended Source Change

Add local guards to `ThiefNote.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, call cooldown logic on an invalid mobile, use `from.Backpack`, or operate on a deleted source note before the existing owner and gump flow runs.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.
- Use the existing localized backpack-use message when the mobile has no backpack or the note is outside the backpack.

## Must Stay Unchanged

Item metadata, `NoteOwner` behavior, cooldown timing/message behavior, localized backpack-use message, ownership mismatch message, account lookup/return/delete behavior, `NoteGump` type and constructor arguments, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
