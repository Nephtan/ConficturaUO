# SOURCE-BATCH-305 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-305+` selected a zero-gate, zero-overlay `PowderOfTemperament` guard repair.

## Recommended Target

- Candidate: `SB305-CAND-001`
- Batch: `SOURCE-BATCH-305`
- Source file: `Data/Scripts/Items/Special/Bulk Order Rewards/Blacksmithy/PowderOfTemperament.cs`
- Behavior: add stale/null/mobile/source-powder/target-item/backpack guards to `OnSingleClick(Mobile from)`, `OnDoubleClick(Mobile from)`, and `InternalTarget.OnTarget(Mobile from, object targeted)` before label display, target assignment, durability mutation, or powder use decrement/delete.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `JarsOfWax.cs`: zero gate/overlay hits, deferred as a sibling one-file item-family batch because it contains multiple wax classes.

## Result

`SB305-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change PowderOfTemperament label display, target assignment, messages, `CanFortify` eligibility, durability bonus math, use decrement/delete semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
