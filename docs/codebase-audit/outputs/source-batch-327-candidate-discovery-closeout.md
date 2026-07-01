# SOURCE-BATCH-327 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-327+` selected a clean `BasePoisonPotion` guard repair.

## Recommended Target

- Candidate: `SB327-CAND-001`
- Batch: `SOURCE-BATCH-327`
- Source file: `Data/Scripts/Items/Potions/Standard/Poison Potions/BasePoisonPotion.cs`
- Behavior: add stale/null/mobile/source-potion/backpack/target-point guards to `BasePoisonPotion.Drink` and `BasePoisonPotion.ThrowTarget.OnTarget` before skill reads, backpack checks, target assignment, potion state checks, or `Point3D` construction.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Broader potion policy, region rules, poison balance, splatter behavior, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB327-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change poison skill thresholds, self-poison behavior, drink effects, commented region-policy block, splatter cap, target prompt, target range, LOS/action-state checks, poison splatter creation, consume behavior, bottle return, karma award, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
