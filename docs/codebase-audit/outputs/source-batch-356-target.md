# SOURCE-BATCH-356 FrankenJournalInBox Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-356`
- Candidate: `SB356-CAND-001`
- System: `Quests:Frankenstein / FrankenJournalInBox`
- File: `Data/Scripts/Quests/Frankenstein/FrankenJournalInBox.cs`

## Intended Source Change

Add a local guard to `FrankenJournalInBox.OnDoubleClick(Mobile from)` so stale/null interaction state cannot proceed into the existing journal scan, journal grant, discovery logging, or source box deletion path.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Item metadata, `AddNameProperties` text, `PlayerMobile` eligibility, duplicate `FrankenJournal` return behavior, new `FrankenJournal` creation and `JournalOwner` assignment, `AddToBackpack` behavior, messages, sound `0x3D`, `LoggingFunctions.LogGeneric` call, source box `Delete` behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
