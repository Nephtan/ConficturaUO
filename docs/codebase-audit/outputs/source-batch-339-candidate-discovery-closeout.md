# SOURCE-BATCH-339 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-339+` selected a clean `BottleOil` guard repair.

## Recommended Target

- Candidate: `SB339-CAND-001`
- Batch: `SOURCE-BATCH-339`
- Source file: `Data/Scripts/Quests/Golems/BottleOil.cs`
- Behavior: add stale/null/mobile/source-item guards to `BottleOil.OnDoubleClick` before sending the existing don't-drink message.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Region/map text candidates, crafting/economy tuning, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB339-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change item ID, weight, stackability, amount assignment, hue, name, message text, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
