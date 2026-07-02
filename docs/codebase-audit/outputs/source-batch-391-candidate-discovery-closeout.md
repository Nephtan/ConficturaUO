# SOURCE-BATCH-391 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-391` ran fresh non-gated candidate discovery after `SOURCE-BATCH-390` and selected `SB391-CAND-001`.

## Recommended Target

- Candidate: `SB391-CAND-001`
- Batch: `SOURCE-BATCH-391`
- System: `Items:Misc / Games / LiarsDice`
- File: `Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs`
- Behavior: add stale/null mobile and deleted source-item guards to `LiarsDice.OnDoubleClick(Mobile from)` before the existing range, bank-balance, freeze, and gump launch behavior.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`LiarsDice.OnDoubleClick` is a narrow game-entry interaction in a zero-gate file. The guard repair prevents invalid interaction state from dereferencing a null/deleted mobile or using a deleted game item without changing valid range checks, bank-balance threshold, `Frozen` behavior, game gump launch, serialization, or policy behavior.

## Deferred Or Excluded Work

Game balance constants, bank-balance threshold, freeze behavior, DiceState design, gump behavior, staff/access policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
