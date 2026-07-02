# SOURCE-BATCH-379 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-379` ran fresh non-gated candidate discovery after `SOURCE-BATCH-378` and selected `SB379-CAND-001`.

## Recommended Target

- Candidate: `SB379-CAND-001`
- Batch: `SOURCE-BATCH-379`
- System: `Items:Containers / GypsyShelf`
- File: `Data/Scripts/Items/Containers/GypsyShelf.cs`
- Behavior: add stale/null mobile, source-shelf, and backpack guards to `GypsyShelf.OnDoubleClick(Mobile from)` before reading `from.Backpack` or granting a `BookGuideToAdventure`.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`GypsyShelf.OnDoubleClick` is a narrow item interaction path that checks for an existing guide book and may grant one. The guard repair prevents stale/null interaction state from dereferencing `from.Backpack` or granting through invalid state without changing ownership cleanup, sound, messages, serialization, or policy behavior.

## Deferred Or Excluded Work

Quest/progression design changes, book content changes, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
