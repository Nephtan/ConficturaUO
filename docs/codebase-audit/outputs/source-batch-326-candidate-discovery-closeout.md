# SOURCE-BATCH-326 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-326+` selected a clean `BaseMixture` guard repair.

## Recommended Target

- Candidate: `SB326-CAND-001`
- Batch: `SOURCE-BATCH-326`
- Source file: `Data/Scripts/Items/Potions/Mixtures/BaseMixture.cs`
- Behavior: add stale/null/mobile/source-potion/backpack/target-point guards to `BaseMixture.Drink` and `BaseMixture.ThrowTarget.OnTarget` before backpack checks, follower checks, target assignment, potion state checks, or `Point3D` construction.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Government, housing, region/travel, economy/crop, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB326-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change the commented region-policy block, follower-cap gate, target prompt, target range, LOS/action-state checks, slime creation, consume behavior, empty-jar return, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
