# SOURCE-BATCH-332 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-332+` selected a clean `EmbalmingFluid` guard repair.

## Recommended Target

- Candidate: `SB332-CAND-001`
- Batch: `SOURCE-BATCH-332`
- Source file: `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs`
- Behavior: add stale/null/mobile/source-fluid/backpack/deleted-target guards to `EmbalmingFluid.OnDoubleClick` and `FluidTarget.OnTarget` before backpack checks, target assignment, `FrankenPorterItem` charge reads, charge mutation, bottle return, sound/reveal behavior, or source fluid consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Quest behavior redesign, reanimation charge tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB332-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, prompts, messages, `FrankenPorterItem` eligibility, charge increment/cap behavior, bottle return, reveal/sound behavior, source fluid consumption behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
