# SOURCE-BATCH-337 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-337+` selected a clean `ShepherdsCrook` guard repair.

## Recommended Target

- Candidate: `SB337-CAND-001`
- Batch: `SOURCE-BATCH-337`
- Source file: `Data/Scripts/Items/Weapons/Staves/ShepherdsCrook.cs`
- Behavior: add stale/null/mobile/source-crook/deleted-creature guards to `ShepherdsCrook.OnDoubleClick`, `HerdingTarget.OnTarget`, and `InternalTarget.OnTarget` before target assignment, creature overhead messages, herding skill math, `CheckTargetSkill`, or `TargetLocation` mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `Data/Scripts/Items/Magical/God/Weapons/Staves/LevelShepherdsCrook.cs` has similar stale target flow, but it is intentionally deferred for a separate one-item source batch.
- Region/map text candidates, crafting/economy tuning, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.

## Result

`SB337-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change weapon abilities, stat requirements, damage/speed values, hit durability, `Weight`, `Resource`, target prompts, target ranges, herdable eligibility, controlled-animal behavior, failure messages, herding skill math, `CheckTargetSkill`, `TargetLocation` mutation, success message, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
