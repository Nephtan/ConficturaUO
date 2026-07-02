# SOURCE-BATCH-376 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-376` ran fresh non-gated candidate discovery after `SOURCE-BATCH-375` and selected `SB376-CAND-001`.

## Recommended Target

- Candidate: `SB376-CAND-001`
- Batch: `SOURCE-BATCH-376`
- System: `Items:Relics / DDRelicDrink`
- File: `Data/Scripts/Items/Relics/DDRelicDrink.cs`
- Behavior: add a stale/null mobile and source-item guard to `DDRelicDrink.OnDoubleClick(Mobile from)` before stamina, thirst, consumption, sound, or container-return behavior.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DDRelicDrink.OnDoubleClick` is a narrow item interaction path that mutates player state, consumes the source item, plays a sound, and returns a container. The guard repair prevents stale/null interaction state from dereferencing `from` or consuming a deleted source item without changing drink behavior, container returns, messages, serialization, or policy behavior.

## Deferred Or Excluded Work

Broad guard sweeps across boats, housing, vendors, games, travel, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
