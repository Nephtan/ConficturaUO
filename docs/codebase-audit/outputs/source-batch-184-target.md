# SOURCE-BATCH-184 JediDatacrons Guard Repair

## Target

- Candidate: SB183-CAND-002
- System: Magic:Jedi / JediDatacrons
- File: Data/Scripts/Magic/Jedi/JediDatacrons.cs
- Behavior: add stale/null/mobile/source-token guards to all JediDatacron OnDoubleClick(Mobile from) message paths before sending the holocron message.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- Each JediDatacron OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the datacron token is deleted.

## Must Stay Unchanged

- Holocron message text
- Spell IDs, item IDs, hue, names, Light, and AddNameProperties labels
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state
