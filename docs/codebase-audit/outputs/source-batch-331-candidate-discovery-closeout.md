# SOURCE-BATCH-331 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-331+` selected a clean `RobotBatteries` guard repair.

## Recommended Target

- Candidate: `SB331-CAND-001`
- Batch: `SOURCE-BATCH-331`
- Source file: `Data/Scripts/Quests/Robots/RobotBatteries.cs`
- Behavior: add stale/null/mobile/source-battery/backpack/deleted-target guards to `RobotBatteries.OnDoubleClick` and `PowerTarget.OnTarget` before backpack checks, target assignment, `RobotItem` charge reads, charge mutation, sound/reveal behavior, or source battery deletion.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Quest behavior redesign, robot charge tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB331-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, prompts, messages, `RobotItem` eligibility, charge increment/cap behavior, reveal/sound behavior, source battery deletion behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
