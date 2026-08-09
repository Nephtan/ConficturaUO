# SOURCE-BATCH-336 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-336+` selected a clean `HolidayFoods` guard repair.

## Recommended Target

- Candidate: `SB336-CAND-001`
- Batch: `SOURCE-BATCH-336`
- Source file: `Data/Scripts/Items/Special/Holiday/HolidayFoods.cs`
- Behavior: add stale/null/mobile/source-food guards to `CandyCane.OnDoubleClick` and `GingerBreadCookie.OnDoubleClick` before backpack/range checks, sound/animation, toothache timer updates, cookie message selection, base food consumption, or source food deletion.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Region/map text candidates, crafting/economy tuning, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB336-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change candy-cane item IDs, gingerbread item IDs, `Stackable`, `LootType`, backpack-or-range eligibility, sound/animation behavior, toothache timer behavior, localized messages, random cookie message behavior, base food consumption, source food deletion behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
