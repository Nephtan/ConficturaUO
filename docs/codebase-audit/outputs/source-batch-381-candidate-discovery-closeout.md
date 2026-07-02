# SOURCE-BATCH-381 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-381` ran fresh non-gated candidate discovery after `SOURCE-BATCH-380` and selected `SB381-CAND-001`.

## Recommended Target

- Candidate: `SB381-CAND-001`
- Batch: `SOURCE-BATCH-381`
- System: `Items:Books / DoomFlayerNote`
- File: `Data/Scripts/Items/Books/DoomFlayerNote.cs`
- Behavior: add stale/null mobile and source-note guards to `DoomFlayerNote.OnDoubleClick(Mobile m)` before range checks, gump creation, or sound playback.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DoomFlayerNote.OnDoubleClick` is a narrow note-reading interaction path that reads mobile range and either opens a note gump with sound playback or sends the existing too-far localized message. The guard repair prevents stale/null interaction state from dereferencing invalid mobiles or deleted source notes without changing note text, range, gump layout, sound, localization, serialization, or policy behavior.

## Deferred Or Excluded Work

Book content changes, quest/progression design changes, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
