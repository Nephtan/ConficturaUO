# SOURCE-BATCH-315 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-315+` selected a clean Waterskin drinking guard repair.

## Recommended Target

- Candidate: `SB315-CAND-001`
- Batch: `SOURCE-BATCH-315`
- Source file: `Data/Scripts/Items/Food/Waterskin.cs`
- Behavior: add stale/null/mobile/source-drink/backpack guards to `Waterskin.OnDoubleClick`, `DirtyWaterskin.OnDoubleClick`, `DrinkingFunctions.CheckWater`, `DrinkingFunctions.OnDrink`, and `DrinkingFunctions.DrinkBenefits` before water checks, backpack checks, thirst mutation, drink consumption, benefit calculation, or item/mobile dereferences.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- None. `SOURCE-BATCH-316+` requires fresh candidate discovery after `SOURCE-BATCH-315` is committed.

## Result

`SB315-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change water target IDs, `CheckWater` range/LOS behavior, fill/drink messages, item ID/name/weight transitions, thirst increments/messages, `BloodDrinker`/`BrainEater` restrictions, dirty waterskin conversion, `DrinkBenefits` stamina/poison behavior, sound/animation/gump behavior, consume semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
