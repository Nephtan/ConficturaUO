# SOURCE-BATCH-299 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-299+` selected one narrow non-gated guard repair and kept the remaining gardening spawn/effect candidate deferred for later focused review.

## Recommended Target

- Candidate: `SB299-CAND-001`
- Batch: `SOURCE-BATCH-299`
- Source file: `Data/Scripts/Trades/Gardening/MiscItems/OrangePetals.cs`
- Behavior: add stale/null/mobile/source-orange-petals/context guards to `OrangePetals.CheckItemUse`, `OnDoubleClick(Mobile from)`, context helper methods, and `OrangePetalsTimer.OnTick()` before root-parent checks, context table access, effect messaging, timer stopping, or item consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `GreenThorns.cs`: zero gate/overlay hits, but owns map/spawn/effect/cooldown behavior and remains deferred while simpler guard candidates are available.

## Result

`SB299-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change the RootParent use requirement, backpack-use message, effect-active message, success message, sound, five-minute duration, wear-off message, context behavior for valid mobiles, `Consume()` semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
