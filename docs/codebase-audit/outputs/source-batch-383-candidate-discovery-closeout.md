# SOURCE-BATCH-383 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-383` ran fresh non-gated candidate discovery after `SOURCE-BATCH-382` and selected `SB383-CAND-001`.

## Recommended Target

- Candidate: `SB383-CAND-001`
- Batch: `SOURCE-BATCH-383`
- System: `Items:Books / BulletinBoards / AssassinNote`
- File: `Data/Scripts/Items/Books/BulletinBoards/AssassinNote.cs`
- Behavior: add stale/null mobile and source-note guards to `AssassinNote.OnDoubleClick(Mobile e)` before range, visibility, line-of-sight, gump creation, or sound playback.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`AssassinNote.OnDoubleClick` is a narrow note-reading interaction path that checks range, visibility, and line-of-sight before opening a note gump with sound playback. The guard repair prevents stale/null interaction state from dereferencing invalid mobiles or deleted source notes without changing letter text, range, visibility, line-of-sight, gump layout, sound, serialization, or policy behavior.

## Deferred Or Excluded Work

Book content changes, quest/progression design changes, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
