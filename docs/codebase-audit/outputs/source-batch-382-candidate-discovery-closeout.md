# SOURCE-BATCH-382 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-382` ran fresh non-gated candidate discovery after `SOURCE-BATCH-381` and selected `SB382-CAND-001`.

## Recommended Target

- Candidate: `SB382-CAND-001`
- Batch: `SOURCE-BATCH-382`
- System: `Items:Books / BardsTaleNote`
- File: `Data/Scripts/Items/Books/BardsTaleNote.cs`
- Behavior: add stale/null mobile, source-note, and backpack guards to `BardsTaleNote.OnDoubleClick(Mobile e)` before backpack membership checks, gump creation, or sound playback.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`BardsTaleNote.OnDoubleClick` is a narrow note-reading interaction path that checks backpack ownership and opens a clue gump with sound playback. The guard repair prevents stale/null interaction state from dereferencing invalid mobiles, deleted source notes, or missing backpacks without changing randomized note setup, clue text, gump layout, sound, backpack-use message, serialization, or policy behavior.

## Deferred Or Excluded Work

Book content changes, quest/progression design changes, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
