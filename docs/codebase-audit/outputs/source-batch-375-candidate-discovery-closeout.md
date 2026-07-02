# SOURCE-BATCH-375 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-375` ran fresh non-gated candidate discovery after `SOURCE-BATCH-374` and selected `SB375-CAND-001`.

## Recommended Target

- Candidate: `SB375-CAND-001`
- Batch: `SOURCE-BATCH-375`
- System: `Items:Relics / DDRelicTablet`
- File: `Data/Scripts/Items/Relics/DDRelicTablet.cs`
- Behavior: add a stale/null mobile and source-item guard to `DDRelicTablet.OnDoubleClick(Mobile e)` before house access checks, backpack checks, gump sends, or tablet read messages.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DDRelicTablet.OnDoubleClick` is a narrow item interaction path with existing house-access, backpack, intelligence, and gump behavior. The guard repair prevents stale/null interaction state from dereferencing `e` or a deleted source item without changing generated tablet data, search clues, flip behavior, gump behavior, serialization, or policy behavior.

## Deferred Or Excluded Work

Broad guard sweeps across boats, housing, vendors, games, travel, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
