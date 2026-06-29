# SOURCE-BATCH-301 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-301+` selected the queued sibling bulk-order target guard repair from `SOURCE-BATCH-300` discovery.

## Recommended Target

- Candidate: `SB301-CAND-001`
- Batch: `SOURCE-BATCH-301`
- Source file: `Data/Scripts/Trades/Bulk Orders/SmallBODTarget.cs`
- Behavior: add stale/null/mobile/source-deed/backpack guards to `SmallBODTarget.OnTarget(Mobile from, object targeted)` before deed backpack checks or `EndCombine` forwarding.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Result

`SB301-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, silent failure behavior, the SmallBOD backpack requirement, `EndCombine` forwarding, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
