# SOURCE-BATCH-297 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-297+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB297-CAND-001`
- Batch: `SOURCE-BATCH-297`
- Source file: `Data/Scripts/Trades/Gardening/Seed.cs`
- Behavior: add stale/null/mobile/source-seed/backpack/target-plant guards to `Seed.OnSingleClick(Mobile from)`, `Seed.OnDoubleClick(Mobile from)`, and `InternalTarget.OnTarget(Mobile from, object targeted)` before label sends, target assignment, or `PlantSeed` forwarding.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved overlay evidence: `RB-06693` and `RB-06694` are already closed as fixed documentation-trace rows. This batch does not change gardening design or tuning.

## Result

`SB297-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change seed labels, backpack message, target prompt, invalid-target message, `PlantSeed` behavior, plant type/hue/show-type semantics, random seed factories, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
