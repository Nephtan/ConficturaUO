# SOURCE-BATCH-374 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-374` ran fresh non-gated candidate discovery after `SOURCE-BATCH-373` and selected `SB374-CAND-001`.

## Recommended Target

- Candidate: `SB374-CAND-001`
- Batch: `SOURCE-BATCH-374`
- System: `Items:Relics / DDRelicVase`
- File: `Data/Scripts/Items/Relics/DDRelicVase.cs`
- Behavior: add a stale/null mobile and source-item guard to `DDRelicVase.OnDoubleClick(Mobile from)` before the existing informational identification message.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DDRelicVase.OnDoubleClick` is a narrow one-method interaction path that only sends an informational relic-identification message. The guard repair prevents stale/null interaction state from dereferencing `from` or a deleted source item without changing generated relic data, value state, serialization, or policy behavior.

## Deferred Or Excluded Work

Broad guard sweeps across boats, housing, vendors, games, travel, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
