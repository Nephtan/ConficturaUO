# SOURCE-BATCH-328 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-328+` selected a clean `HorseArmor` guard repair.

## Recommended Target

- Candidate: `SB328-CAND-001`
- Batch: `SOURCE-BATCH-328`
- Source file: `Data/Scripts/Items/Armor/HorseArmor.cs`
- Behavior: add stale/null/mobile/source-armor/backpack/deleted-target guards to `HorseArmor.OnDoubleClick` and `HorseTarget.OnTarget` before backpack checks, target assignment, mount checks, barding stat mutation, or source armor consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Mount balance, pet-stat tuning, material balance, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB328-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change horse/zebra eligibility, ownership rules, mount requirements, material hue/stat mapping, sound, source armor consumption, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
