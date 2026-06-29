# SOURCE-BATCH-306 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-306+` selected the queued sibling JarsOfWax guard repair from `SOURCE-BATCH-305` discovery.

## Recommended Target

- Candidate: `SB306-CAND-001`
- Batch: `SOURCE-BATCH-306`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs`
- Behavior: add stale/null/mobile/source-wax/target-item/backpack guards to the metal, leather, and instrument wax `OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` paths before target assignment, material/type checks, bonus mutation, Bottle return, or wax consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Result

`SB306-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change JarsOfWax prompts, target range, material/type eligibility, already-good and invalid-target messages, durability/use increments, sound, Bottle return, wax consumption, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
