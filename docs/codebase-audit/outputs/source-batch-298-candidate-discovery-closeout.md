# SOURCE-BATCH-298 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-298+` selected one narrow non-gated guard repair and deferred two gardening-adjacent files for later focused review.

## Recommended Target

- Candidate: `SB298-CAND-001`
- Batch: `SOURCE-BATCH-298`
- Source file: `Data/Scripts/Trades/Gardening/MiscItems/RedLeaves.cs`
- Behavior: add stale/null/mobile/source-red-leaves/backpack/target-item guards to `RedLeaves.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)` before target assignment, backpack checks, book eligibility checks, wax consumption, or book `Writable` mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `OrangePetals.cs`: zero gate/overlay hits, but owns poison-resistance timer/context behavior and should be reviewed separately.
- `GreenThorns.cs`: zero gate/overlay hits, but owns map/spawn/effect/cooldown behavior and is less clean while simpler guard candidates remain.

## Result

`SB298-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change red-leaf book sealing messages, `BaseBook` eligibility, `Writable` mutation, red leaves consumption, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
