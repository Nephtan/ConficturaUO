# SOURCE-BATCH-335 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-335+` selected a clean `RustyJunk` guard repair.

## Recommended Target

- Candidate: `SB335-CAND-001`
- Batch: `SOURCE-BATCH-335`
- Source file: `Data/Scripts/Items/Trades/Fishing/RustyJunk.cs`
- Behavior: add stale/null/mobile/source-rusted/backpack guards to `RustyJunk.OnDoubleClick` and `InternalTarget.OnTarget` before backpack checks, target assignment, forge checks, mining skill reads, ingot conversion, sound behavior, or source rusty item deletion.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Region/map text candidates, crafting/economy tuning, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB335-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change generated rusty item names, item IDs, hues, weights, scrap iron property text, backpack message, target prompt, target range, forge requirement, mining skill threshold, `CheckTargetSkill` behavior, ingot conversion, sound behavior, source rusty item deletion behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
