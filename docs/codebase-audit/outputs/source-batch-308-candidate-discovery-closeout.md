# SOURCE-BATCH-308 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-308+` selected the queued sibling WaxPaintings guard repair from `SOURCE-BATCH-307` discovery.

## Recommended Target

- Candidate: `SB308-CAND-001`
- Batch: `SOURCE-BATCH-308`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs`
- Behavior: add stale/null/mobile/source-painting/target guards to `WaxPainting.OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` before target assignment, mobile/title inspection, or painting naming mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `LargeWaxPot.cs`: zero gate/overlay hits, deferred as a stateful wax-pot batch.

## Result

`SB308-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change WaxPaintings prompt, target range, valid mobile body eligibility, real-person naming behavior, fictional naming behavior, title generation, random name/title behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
