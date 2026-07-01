# SOURCE-BATCH-324 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-324+` selected a clean `Bola` guard repair.

## Recommended Target

- Candidate: `SB324-CAND-001`
- Batch: `SOURCE-BATCH-324`
- Source file: `Data/Scripts/Items/Misc/Bola.cs`
- Behavior: add stale/null/mobile/source-bola/backpack/deleted-target guards to `Bola.OnDoubleClick` and `BolaTarget.OnTarget` before backpack checks, target validation, cooldown checks, or throw setup.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Timer callback hardening in `Bola.FinishThrow` remains out of this batch because the runner requires no timer diff.
- Government, housing, region/travel, economy/crop, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.

## Result

`SB324-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change combat policy, target range, harmful targeting, cooldowns, mount/hand/animal-form restrictions, target messages, consume behavior, animation/effects, timer callbacks, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
