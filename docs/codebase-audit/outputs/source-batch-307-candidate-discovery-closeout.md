# SOURCE-BATCH-307 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-307+` selected a zero-gate, zero-overlay WaxSculptors guard repair.

## Recommended Target

- Candidate: `SB307-CAND-001`
- Batch: `SOURCE-BATCH-307`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs`
- Behavior: add stale/null/mobile/source-sculptor/target guards and a safe self-target type check to `WaxSculptors.OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` before target assignment, mobile/title inspection, sculptor naming mutation, or invalid self-target casts.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `WaxPaintings.cs`: zero gate/overlay hits, deferred as a sibling one-file portrait-naming batch.

## Result

`SB307-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change WaxSculptors prompt, target range, valid mobile body eligibility, real-person naming behavior, fictional naming behavior, title generation, random name/title behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
