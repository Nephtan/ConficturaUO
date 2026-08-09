# SOURCE-BATCH-365 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-365` ran fresh non-gated candidate discovery after `SOURCE-BATCH-364` and selected `SB365-CAND-001`.

## Recommended Target

- Candidate: `SB365-CAND-001`
- Batch: `SOURCE-BATCH-365`
- System: `Items:Relics / DDRelicArts`
- File: `Data/Scripts/Items/Relics/DDRelicArts.cs`
- Behavior: add a stale/null mobile and source-item guard to `DDRelicArts.OnDoubleClick(Mobile from)` before the existing informational identification message.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DDRelicArts.OnDoubleClick` is a narrow one-method interaction path that only sends an informational relic-identification message. The guard repair prevents stale/null interaction state from dereferencing `from` or a deleted source item without changing generated relic data, value state, serialization, or policy behavior.

## Deferred Or Excluded Work

Broad guard sweeps across boats, housing, vendors, games, travel, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
