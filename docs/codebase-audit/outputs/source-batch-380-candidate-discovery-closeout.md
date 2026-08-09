# SOURCE-BATCH-380 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-380` ran fresh non-gated candidate discovery after `SOURCE-BATCH-379` and selected `SB380-CAND-001`.

## Recommended Target

- Candidate: `SB380-CAND-001`
- Batch: `SOURCE-BATCH-380`
- System: `Items:Containers / Safe`
- File: `Data/Scripts/Items/Containers/Safe.cs`
- Behavior: add stale/null mobile and source-safe guards to `Safe.OnDoubleClick(Mobile from)` and `Safe.CheckAccess(Mobile m)` before secure-access checks or bank-box opening.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`Safe.OnDoubleClick` is a narrow item interaction path that reads mobile range, visibility, line-of-sight, house secure access, and bank-box state. The guard repair prevents stale/null interaction state from dereferencing invalid mobiles or deleted source safes without changing house access policy, secure levels, messages, serialization, or bank-box behavior.

## Deferred Or Excluded Work

House access policy changes, bank/storage behavior changes, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
