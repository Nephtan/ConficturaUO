# SOURCE-BATCH-325 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-325+` selected a clean `BaseLiquid` guard repair.

## Recommended Target

- Candidate: `SB325-CAND-001`
- Batch: `SOURCE-BATCH-325`
- Source file: `Data/Scripts/Items/Potions/Mixtures/BaseLiquid.cs`
- Behavior: add stale/null/mobile/source-potion/backpack/target-point guards to `BaseLiquid.Drink` and `BaseLiquid.ThrowTarget.OnTarget` before backpack checks, target assignment, potion state checks, or `Point3D` construction.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `BaseMixture` remains a sibling candidate for a later focused batch.
- Government, housing, region/travel, economy/crop, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.

## Result

`SB325-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change the commented region-policy block, splatter cap, target prompt, target range, LOS/action-state checks, karma award, splatter creation, consume behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
