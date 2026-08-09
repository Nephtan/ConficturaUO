# SOURCE-BATCH-164 EnchantingEtudeScroll Guard Repair

## Target

- Candidate: SB163-CAND-002
- System: Magic:Bard / EnchantingEtude
- File: Data/Scripts/Magic/Bard/Scrolls/EnchantingEtude.cs
- Behavior: add a stale/null/mobile/source-scroll guard to EnchantingEtudeScroll.OnDoubleClick(Mobile from) before sending the sheet-music message.

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