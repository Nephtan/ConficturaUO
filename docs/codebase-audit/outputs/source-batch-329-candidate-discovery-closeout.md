# SOURCE-BATCH-329 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-329+` selected a clean `GrapplingHook` guard repair.

## Recommended Target

- Candidate: `SB329-CAND-001`
- Batch: `SOURCE-BATCH-329`
- Source file: `Data/Scripts/Items/Boats/GrapplingHook.cs`
- Behavior: add stale/null/mobile/source-hook/backpack/map/deleted-target guards to `GrapplingHook.OnDoubleClick` and `HookTarget.OnTarget` before backpack checks, target assignment, pirate ship lookup, or teleport behavior.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Boat travel policy, pirate ship lookup rules, teleport behavior, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB329-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, prompts, invalid-target messages, pirate ship lookup, valid-location rule, teleport behavior, pet teleport behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
