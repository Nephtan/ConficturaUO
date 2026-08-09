# SOURCE-BATCH-378 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-378` ran fresh non-gated candidate discovery after `SOURCE-BATCH-377` and selected `SB378-CAND-001`.

## Recommended Target

- Candidate: `SB378-CAND-001`
- Batch: `SOURCE-BATCH-378`
- System: `Items:Doors / DoorStuck`
- File: `Data/Scripts/Items/Doors/DoorStuck.cs`
- Behavior: add stale/null mobile and source-door guards to `DoorStuck.OnDoubleClick(Mobile m)` and `DoorStuck.OnDoubleClickDead(Mobile m)` before sending the existing locked-door message.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`DoorStuck` has two message-only interaction paths that send the same locked-door text. The guard repair prevents stale/null interaction state from dereferencing `m` or a deleted source door without changing door behavior, travel behavior, message text, serialization, or policy behavior.

## Deferred Or Excluded Work

Teleporting doors, travel doors, boat/housing doors, staff/admin surfaces, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
