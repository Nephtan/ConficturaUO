# SOURCE-BATCH-174 MagicFinaleScroll Guard Repair

## Target

- Candidate: SB163-CAND-012
- System: Magic:Bard / MagicFinale
- File: Data/Scripts/Magic/Bard/Scrolls/MagicFinale.cs
- Behavior: add a stale/null/mobile/source-scroll guard to MagicFinaleScroll.OnDoubleClick(Mobile from) before sending the sheet-music message.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the scroll is deleted.

## Must Stay Unchanged

- Sheet-music message
- Constructor spell ID, item ID, hue, and stackable behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state