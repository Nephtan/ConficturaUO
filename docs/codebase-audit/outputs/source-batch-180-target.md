# SOURCE-BATCH-180 HolySymbols Guard Repair

## Target

- Candidate: SB179-CAND-002
- System: Magic:Holy Man / HolySymbols
- File: Data/Scripts/Magic/Holy Man/HolySymbols.cs
- Behavior: add stale/null/mobile/source-token guards to all HolyManSymbol OnDoubleClick(Mobile from) message paths before sending the holy symbol message.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- Each HolyManSymbol OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the symbol token is deleted.

## Must Stay Unchanged

- Holy symbol message text
- Spell IDs, hue, names, and AddNameProperties labels
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state