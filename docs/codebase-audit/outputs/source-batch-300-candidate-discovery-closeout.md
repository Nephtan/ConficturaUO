# SOURCE-BATCH-300 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-300+` selected one narrow non-gated bulk-order target guard repair and kept its sibling target helper queued for the next one-item batch.

## Recommended Target

- Candidate: `SB300-CAND-001`
- Batch: `SOURCE-BATCH-300`
- Source file: `Data/Scripts/Trades/Bulk Orders/LargeBODTarget.cs`
- Behavior: add stale/null/mobile/source-deed/backpack guards to `LargeBODTarget.OnTarget(Mobile from, object targeted)` before deed backpack checks or `EndCombine` forwarding.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `SmallBODTarget.cs`: zero gate/overlay hits and the sibling helper to this file, but deferred to the next one-item batch to preserve the commit-after-each-item runner rule.

## Result

`SB300-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, silent failure behavior, the LargeBOD backpack requirement, `EndCombine` forwarding, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
