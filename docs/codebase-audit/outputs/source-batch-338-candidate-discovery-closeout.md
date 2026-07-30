# SOURCE-BATCH-338 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-338+` selected a clean `LevelShepherdsCrook` guard repair.

## Recommended Target

- Candidate: `SB338-CAND-001`
- Batch: `SOURCE-BATCH-338`
- Source file: `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs`
- Behavior: add stale/null/mobile/source-crook/deleted-creature guards to `LevelShepherdsCrook.OnDoubleClick`, `HerdingTarget.OnTarget`, and `InternalTarget.OnTarget` before target assignment, animal eligibility checks, creature overhead messages, `CheckTargetSkill`, or `TargetLocation` mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Region/map text candidates, crafting/economy tuning, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB338-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change level-staff abilities, stat requirements, damage/speed values, hit durability, `Weight`, `Resource`, target prompts, target ranges, animal eligibility, controlled-animal behavior, failure messages, herding skill checks, `TargetLocation` mutation, success message, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
