# SOURCE-BATCH-179 DeathSkulls Guard Repair

## Target

- Candidate: SB179-CAND-001
- System: Magic:Death Knight / DeathSkulls
- File: Data/Scripts/Magic/Death Knight/DeathSkulls.cs
- Behavior: add stale/null/mobile/source-token guards to all DeathKnightSkull OnDoubleClick(Mobile from) message paths before sending the skull message.

## Fence Result

- POST-BATCH-Y exact-file gate hits: 0
- Exact-file active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- Each DeathKnightSkull OnDoubleClick(Mobile from) returns immediately when from == null, from.Deleted, or the skull token is deleted.

## Must Stay Unchanged

- Skull message text
- Spell IDs, item ID randomization, hue, names, and AddNameProperties labels
- Serialization layout/versioning
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state