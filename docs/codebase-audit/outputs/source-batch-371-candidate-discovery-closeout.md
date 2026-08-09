# SOURCE-BATCH-371 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-371` ran fresh non-gated candidate discovery after `SOURCE-BATCH-370` and selected `SB371-CAND-001`.

## Recommended Target

- Candidate: `SB371-CAND-001`
- Batch: `SOURCE-BATCH-371`
- System: `Items:Relics / DDRelicLight`
- File: `Data/Scripts/Items/Relics/DDRelicLight.cs`
- Behavior: add stale/null mobile and source-item guards to `DDRelicLight1.OnDoubleClick(Mobile from)`, `DDRelicLight2.OnDoubleClick(Mobile from)`, and `DDRelicLight3.OnDoubleClick(Mobile from)` before the existing informational identification messages.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DDRelicLight.cs` contains three narrow relic-light interaction paths that only send informational relic-identification messages. The guard repair prevents stale/null interaction state from dereferencing `from` or a deleted source item without changing generated relic data, light behavior, value state, serialization, or policy behavior.

## Deferred Or Excluded Work

Broad guard sweeps across boats, housing, vendors, games, travel, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
